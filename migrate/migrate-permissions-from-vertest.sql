-- =============================================================================
-- migrate-permissions-from-vertest.sql
-- Полномошна (Одобренија за туѓо возило): pull the LIVE registry from
-- [VERTEST].dbo.DocumentsPermisions (4,687 rows, 2014→today, written daily;
-- VTEZVV's copy died in 2018). Reached via linked server [VTEZVV_LIVE]
-- (cross-catalog). Idempotent + incremental on VehiclePermission.LegacyId.
--
-- Mapping strategy (verified 2026-07-06):
--   * Owner/authorized are CustomerVehiclesRelations in VERTEST's OWN id space →
--     resolve both CLIENTS by MB (owner 98.3%, authorized 83.8% match); the rest
--     are inserted as new v2 Clients (identity ≥ 10M floor, name-swap, Македонско).
--   * The v2 FK anchor (ClientVehicleRelationId) uses the owner's vehicle-less
--     "Лично" relation (type 3) — get-or-create, same convention as the IDL debts.
--     The REAL vehicle facts (display/plate/VIN/engine) live in the print snapshot,
--     pulled from VERTEST Vehicles/Models/Makers.
--   * Issuer + city resolved BY NAME (VERTEST id spaces differ), with fallbacks.
--   * The registry repeats active (authorized, owner) pairs 803× → the v2 unique
--     rule is scoped to LegacyId IS NULL (AddPermissionLegacyId migration).
--   * No CustomerDebt rows — historical documents are long paid.
--
--   sqlcmd -S <server> -d VTE -I -f 65001 -i migrate\migrate-permissions-from-vertest.sql
-- =============================================================================
USE VTE;
GO

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

IF NOT EXISTS (SELECT 1 FROM sys.servers WHERE name = 'VTEZVV_LIVE')
BEGIN RAISERROR('Linked server VTEZVV_LIVE missing. Create it first.', 16, 1); RETURN; END;

DECLARE @mkCitizenship int =
    (SELECT TOP 1 c.CitizenshipId FROM dbo.Client c JOIN dbo.Citizenship z ON z.Id = c.CitizenshipId
     WHERE z.Name = N'Македонско' GROUP BY c.CitizenshipId ORDER BY COUNT(*) DESC);
DECLARE @fallbackIssuer tinyint = (SELECT TOP 1 Id FROM dbo.DocumentIssuer ORDER BY Id);
DECLARE @velesCityId int = (SELECT TOP 1 Id FROM dbo.City WHERE LTRIM(RTRIM(Name)) = N'Велес' ORDER BY Id);
DECLARE @rows int;

-- ============================================================================
-- 0a. Stage the raw VERTEST tables locally FIRST (single-table remote pulls).
--     Joining across the linked server is catastrophically slow (N+1 remote
--     round-trips — a 12-join staging query ran >5 min); the whole DB is small
--     (~70k rows across these tables), so pull-then-join-locally is seconds.
-- ============================================================================
IF OBJECT_ID('tempdb..#vPerm')   IS NOT NULL DROP TABLE #vPerm;
IF OBJECT_ID('tempdb..#vCust')   IS NOT NULL DROP TABLE #vCust;
IF OBJECT_ID('tempdb..#vRel')    IS NOT NULL DROP TABLE #vRel;
IF OBJECT_ID('tempdb..#vVeh')    IS NOT NULL DROP TABLE #vVeh;
IF OBJECT_ID('tempdb..#vModel')  IS NOT NULL DROP TABLE #vModel;
IF OBJECT_ID('tempdb..#vMaker')  IS NOT NULL DROP TABLE #vMaker;
IF OBJECT_ID('tempdb..#vStreet') IS NOT NULL DROP TABLE #vStreet;
IF OBJECT_ID('tempdb..#vCity')   IS NOT NULL DROP TABLE #vCity;
IF OBJECT_ID('tempdb..#vIss')    IS NOT NULL DROP TABLE #vIss;

SELECT Id, IdCustomerVehicleRelation, IdCustomerVehicleRelationOwner, IdIssuer, IdCityOfIssuing,
       PermissionNumber, TrafficLicenceNumber, TriptiqueNumber, DateCreated, ValidTillDate, DateStart, Note, Active
INTO #vPerm  FROM VTEZVV_LIVE.VERTEST.dbo.DocumentsPermisions;
SELECT Id, MB, CustomerFirstName, CustomerSurname, ParentName, DateOfBirth, IsCompany, TaxNumber, PhoneNumber, eMail,
       IdLivingAddress, LivingAddressNumber, IdLivingCity, BLK, BLKDateIssued, BLKIssuer,
       PassportNumber, PassDateIssued, PassIssuer, DriveingLicenceNumber, DriveingLicenceDateIssued, DriveingLicenceIssuer
INTO #vCust  FROM VTEZVV_LIVE.VERTEST.dbo.Customers;
SELECT Id, IdCustomer, IdVehicle INTO #vRel FROM VTEZVV_LIVE.VERTEST.dbo.CustomerVehiclesRelations;
SELECT Id, IdVehicleModel, LastRegistratinNumber, ShellNumber, EngineNumber, VehicleModelAdding
INTO #vVeh   FROM VTEZVV_LIVE.VERTEST.dbo.Vehicles;
SELECT Id, IdVehicleMaker, ModelName INTO #vModel FROM VTEZVV_LIVE.VERTEST.dbo.VehicleModel;
SELECT Id, CompanyName              INTO #vMaker FROM VTEZVV_LIVE.VERTEST.dbo.VehicleMakers;
SELECT Id, StreetName               INTO #vStreet FROM VTEZVV_LIVE.VERTEST.dbo.Streets;
SELECT Id, CityName                 INTO #vCity  FROM VTEZVV_LIVE.VERTEST.dbo.Cities;
SELECT Id, IssuerName               INTO #vIss   FROM VTEZVV_LIVE.VERTEST.dbo.RegistrationIssuers;

-- ============================================================================
-- 0b. Build the staging rows from LOCAL joins.
-- ============================================================================
IF OBJECT_ID('tempdb..#src') IS NOT NULL DROP TABLE #src;
SELECT
    p.Id AS LegacyId,
    NULLIF(LTRIM(RTRIM(p.PermissionNumber)),     N'') AS PermissionNumber,
    ISNULL(NULLIF(LTRIM(RTRIM(p.TrafficLicenceNumber)), N''), N'-') AS TrafficLicenceNumber,
    NULLIF(LTRIM(RTRIM(p.TriptiqueNumber)),      N'') AS TriptiqueNumber,
    p.DateCreated, p.DateStart,
    CASE WHEN YEAR(p.ValidTillDate) > 2100 THEN DATEADD(YEAR, -1000, p.ValidTillDate) ELSE p.ValidTillDate END AS ValidTillDate,
    NULLIF(LTRIM(RTRIM(p.Note)), N'') AS Note,
    p.Active,
    riN.IssuerName  AS IssuerName,
    cty.CityName    AS IssuingCityName,
    -- owner
    ro.IdCustomer                                       AS OwnIdCustomer,
    NULLIF(LTRIM(RTRIM(co.MB)), N'')                    AS OwnMB,
    co.IsCompany                                        AS OwnIsCompany,
    NULLIF(LTRIM(RTRIM(co.CustomerFirstName)), N'')     AS OwnSurnameRaw,
    NULLIF(LTRIM(RTRIM(co.CustomerSurname)),   N'')     AS OwnFirstNameRaw,
    NULLIF(LTRIM(RTRIM(co.ParentName)),        N'')     AS OwnParentName,
    co.DateOfBirth                                      AS OwnDateOfBirth,
    NULLIF(LTRIM(RTRIM(co.TaxNumber)),   N'')           AS OwnTaxNumber,
    NULLIF(LTRIM(RTRIM(co.PhoneNumber)), N'')           AS OwnPhone,
    NULLIF(LTRIM(RTRIM(co.eMail)),       N'')           AS OwnEmail,
    NULLIF(LTRIM(RTRIM(CONCAT_WS(N' ',
        NULLIF(LTRIM(RTRIM(sto.StreetName)),           N''),
        NULLIF(LTRIM(RTRIM(co.LivingAddressNumber)),   N''),
        NULLIF(LTRIM(RTRIM(lco.CityName)),             N'')))), N'') AS OwnAddress,
    -- authorized
    ra.IdCustomer                                       AS AutIdCustomer,
    NULLIF(LTRIM(RTRIM(ca.MB)), N'')                    AS AutMB,
    ca.IsCompany                                        AS AutIsCompany,
    NULLIF(LTRIM(RTRIM(ca.CustomerFirstName)), N'')     AS AutSurnameRaw,
    NULLIF(LTRIM(RTRIM(ca.CustomerSurname)),   N'')     AS AutFirstNameRaw,
    NULLIF(LTRIM(RTRIM(ca.ParentName)),        N'')     AS AutParentName,
    ca.DateOfBirth                                      AS AutDateOfBirth,
    NULLIF(LTRIM(RTRIM(ca.TaxNumber)),   N'')           AS AutTaxNumber,
    NULLIF(LTRIM(RTRIM(ca.PhoneNumber)), N'')           AS AutPhone,
    NULLIF(LTRIM(RTRIM(ca.eMail)),       N'')           AS AutEmail,
    NULLIF(LTRIM(RTRIM(ca.BLK)),            N'')        AS AutIdCardNumber,
    NULLIF(LTRIM(RTRIM(ca.PassportNumber)), N'')        AS AutPassportNumber,
    ca.BLKDateIssued  AS AutIdCardDate,  ca.BLKIssuer  AS AutIdCardIssuer,
    ca.PassDateIssued AS AutPassDate,    ca.PassIssuer AS AutPassIssuer,
    NULLIF(LTRIM(RTRIM(ca.DriveingLicenceNumber)), N'') AS AutDrvNumber,
    ca.DriveingLicenceDateIssued AS AutDrvDate, ca.DriveingLicenceIssuer AS AutDrvIssuer,
    NULLIF(LTRIM(RTRIM(CONCAT_WS(N' ',
        NULLIF(LTRIM(RTRIM(sta.StreetName)),           N''),
        NULLIF(LTRIM(RTRIM(ca.LivingAddressNumber)),   N''),
        NULLIF(LTRIM(RTRIM(lca.CityName)),             N'')))), N'') AS AutAddress,
    -- возило (за snapshot; НЕ мапираме кон v2 возило)
    NULLIF(LTRIM(RTRIM(CONCAT_WS(N' ',
        NULLIF(LTRIM(RTRIM(vmk.CompanyName)),      N''),
        NULLIF(LTRIM(RTRIM(vmd.ModelName)),        N''),
        NULLIF(LTRIM(RTRIM(vo.VehicleModelAdding)),N'')))), N'') AS VehDisplay,
    NULLIF(LTRIM(RTRIM(vo.LastRegistratinNumber)), N'') AS VehPlate,
    NULLIF(LTRIM(RTRIM(vo.ShellNumber)),           N'') AS VehVin,
    NULLIF(LTRIM(RTRIM(vo.EngineNumber)),          N'') AS VehEngine
INTO #src
FROM #vPerm p
LEFT JOIN #vRel  ro  ON ro.Id  = p.IdCustomerVehicleRelationOwner
LEFT JOIN #vCust co  ON co.Id  = ro.IdCustomer
LEFT JOIN #vRel  ra  ON ra.Id  = p.IdCustomerVehicleRelation
LEFT JOIN #vCust ca  ON ca.Id  = ra.IdCustomer
LEFT JOIN #vVeh  vo  ON vo.Id  = ro.IdVehicle
LEFT JOIN #vModel vmd ON vmd.Id = vo.IdVehicleModel
LEFT JOIN #vMaker vmk ON vmk.Id = vmd.IdVehicleMaker
LEFT JOIN #vIss  riN ON riN.Id = NULLIF(p.IdIssuer, 0)
LEFT JOIN #vCity cty ON cty.Id = NULLIF(p.IdCityOfIssuing, 0)
LEFT JOIN #vStreet sto ON sto.Id = NULLIF(co.IdLivingAddress, 0)
LEFT JOIN #vCity  lco ON lco.Id = NULLIF(co.IdLivingCity, 0)
LEFT JOIN #vStreet sta ON sta.Id = NULLIF(ca.IdLivingAddress, 0)
LEFT JOIN #vCity  lca ON lca.Id = NULLIF(ca.IdLivingCity, 0)
WHERE NOT EXISTS (SELECT 1 FROM dbo.VehiclePermission x WHERE x.LegacyId = p.Id);

SET @rows = @@ROWCOUNT;
PRINT CONCAT('Staged ', @rows, ' new permission rows from VERTEST.');
IF @rows = 0 BEGIN PRINT 'Nothing to migrate.'; COMMIT; RETURN; END;

-- ============================================================================
-- 1. Resolve/insert CLIENTS for both parties (union of owner + authorized).
-- ============================================================================
IF OBJECT_ID('tempdb..#cust') IS NOT NULL DROP TABLE #cust;
SELECT x.IdCustomer, MIN(x.MB) AS MB, CAST(NULL AS bigint) AS ClientId
INTO #cust
FROM (
    SELECT OwnIdCustomer AS IdCustomer, OwnMB AS MB FROM #src WHERE OwnIdCustomer IS NOT NULL
    UNION ALL
    SELECT AutIdCustomer, AutMB FROM #src WHERE AutIdCustomer IS NOT NULL
) x
GROUP BY x.IdCustomer;

UPDATE cu SET cu.ClientId = m.ClientId
FROM #cust cu
CROSS APPLY (SELECT TOP 1 c.Id AS ClientId FROM dbo.Client c
             WHERE c.MB = cu.MB AND cu.MB IS NOT NULL
             ORDER BY CASE WHEN c.Id < 10000000 THEN 0 ELSE 1 END, c.Id) m;

DECLARE @newClients TABLE (IdCustomer bigint, ClientId bigint);

MERGE dbo.Client AS tgt
USING (
    SELECT cu.IdCustomer, d.MB, d.IsCompany, d.FirstNameRaw, d.SurnameRaw, d.ParentName,
           d.DateOfBirth, d.Address, d.Phone, d.Email, d.TaxNumber
    FROM #cust cu
    CROSS APPLY (
        SELECT TOP 1 *
        FROM (
            SELECT s.OwnMB AS MB, s.OwnIsCompany AS IsCompany, s.OwnFirstNameRaw AS FirstNameRaw,
                   s.OwnSurnameRaw AS SurnameRaw, s.OwnParentName AS ParentName, s.OwnDateOfBirth AS DateOfBirth,
                   s.OwnAddress AS Address, s.OwnPhone AS Phone, s.OwnEmail AS Email, s.OwnTaxNumber AS TaxNumber, s.LegacyId
            FROM #src s WHERE s.OwnIdCustomer = cu.IdCustomer
            UNION ALL
            SELECT s.AutMB, s.AutIsCompany, s.AutFirstNameRaw, s.AutSurnameRaw, s.AutParentName, s.AutDateOfBirth,
                   s.AutAddress, s.AutPhone, s.AutEmail, s.AutTaxNumber, s.LegacyId
            FROM #src s WHERE s.AutIdCustomer = cu.IdCustomer
        ) u ORDER BY u.LegacyId DESC
    ) d
    WHERE cu.ClientId IS NULL
) AS src ON 1 = 0
WHEN NOT MATCHED THEN
    INSERT (CompanyId, CitizenshipId, Business,
            FirstName, MiddleName, ParentName, LastName, MB,
            Address, TaxNumber, PhoneNumber, Email, DateOfBirth, Active, CreatedAt)
    VALUES (CAST(4 AS tinyint), @mkCitizenship, ISNULL(src.IsCompany, 0),
            CASE WHEN src.IsCompany = 1 THEN src.SurnameRaw ELSE COALESCE(src.FirstNameRaw, src.SurnameRaw) END,
            src.ParentName, src.ParentName,
            CASE WHEN src.IsCompany = 1 THEN src.FirstNameRaw
                 ELSE CASE WHEN src.FirstNameRaw IS NULL THEN NULL ELSE src.SurnameRaw END END,
            src.MB, src.Address, src.TaxNumber, src.Phone, src.Email, src.DateOfBirth, 1, GETUTCDATE())
OUTPUT src.IdCustomer, inserted.Id INTO @newClients (IdCustomer, ClientId);

UPDATE cu SET cu.ClientId = n.ClientId
FROM #cust cu JOIN @newClients n ON n.IdCustomer = cu.IdCustomer
WHERE cu.ClientId IS NULL;

DECLARE @newClientCount int = (SELECT COUNT(*) FROM @newClients);
PRINT CONCAT('Created ', @newClientCount, ' new clients (no MB match).');

-- ============================================================================
-- 2. Get-or-create the owner's "Лично" relation (type 3, vehicle-less) — FK anchor.
-- ============================================================================
IF OBJECT_ID('tempdb..#ownerRel') IS NOT NULL DROP TABLE #ownerRel;
SELECT DISTINCT cu.ClientId, CAST(NULL AS bigint) AS RelationId
INTO #ownerRel
FROM #src s JOIN #cust cu ON cu.IdCustomer = s.OwnIdCustomer
WHERE cu.ClientId IS NOT NULL;

UPDATE r SET r.RelationId = x.Id
FROM #ownerRel r
CROSS APPLY (SELECT TOP 1 cvr.Id FROM dbo.ClientVehicleRelation cvr
             WHERE cvr.ClientId = r.ClientId AND cvr.VehicleId IS NULL AND cvr.RelationTypeId = 3
             ORDER BY cvr.Active DESC, cvr.StartDate DESC) x;

INSERT INTO dbo.ClientVehicleRelation (ClientId, VehicleId, RelationTypeId, StartDate, Active)
SELECT r.ClientId, NULL, 3, GETUTCDATE(), 1
FROM #ownerRel r WHERE r.RelationId IS NULL;

UPDATE r SET r.RelationId = cvr.Id
FROM #ownerRel r
JOIN dbo.ClientVehicleRelation cvr
  ON cvr.ClientId = r.ClientId AND cvr.VehicleId IS NULL AND cvr.RelationTypeId = 3
WHERE r.RelationId IS NULL;

-- ============================================================================
-- 3. Insert the permissions (full print snapshot; no debts).
-- ============================================================================
INSERT INTO dbo.VehiclePermission (
    CompanyId, ClientVehicleRelationId, AuthorizedClientId,
    IssuerId, IssuingCityId, IssuerOrganizationId,
    PermissionNumber, TrafficLicenceNumber, TriptiqueNumber,
    IssuedDate, StartDate, ValidTillDate, Note,
    OwnerName, OwnerIdNumber, OwnerAddress,
    AuthorizedName, AuthorizedEmbg, AuthorizedIdCardNumber, AuthorizedPassportNumber, AuthorizedAddress,
    VehicleDisplay, PlateNumber, VehicleVin, VehicleEngineNumber,
    Active, CreatedAt, LegacyId)
SELECT
    CAST(4 AS tinyint), orel.RelationId, acu.ClientId,
    COALESCE(di.Id, @fallbackIssuer),
    COALESCE(ci.Id, @velesCityId),
    37,
    s.PermissionNumber, s.TrafficLicenceNumber, s.TriptiqueNumber,
    s.DateCreated, s.DateStart, s.ValidTillDate, s.Note,
    -- owner display: фирма = целото име; лице = ИМЕ ПРЕЗИМЕ (swap од легаси редослед)
    CASE WHEN s.OwnIsCompany = 1 THEN s.OwnSurnameRaw
         ELSE NULLIF(LTRIM(RTRIM(CONCAT_WS(N' ', s.OwnFirstNameRaw, s.OwnSurnameRaw))), N'') END,
    s.OwnMB, s.OwnAddress,
    CASE WHEN s.AutIsCompany = 1 THEN s.AutSurnameRaw
         ELSE NULLIF(LTRIM(RTRIM(CONCAT_WS(N' ', s.AutFirstNameRaw, s.AutSurnameRaw))), N'') END,
    s.AutMB, s.AutIdCardNumber, s.AutPassportNumber, s.AutAddress,
    s.VehDisplay, s.VehPlate, s.VehVin, s.VehEngine,
    s.Active, GETUTCDATE(), s.LegacyId
FROM #src s
JOIN #cust ocu  ON ocu.IdCustomer = s.OwnIdCustomer AND ocu.ClientId IS NOT NULL
JOIN #ownerRel orel ON orel.ClientId = ocu.ClientId
JOIN #cust acu  ON acu.IdCustomer = s.AutIdCustomer AND acu.ClientId IS NOT NULL
-- TOP 1 lookups: the v2 lookup tables contain duplicate names (e.g. Citizenship had
-- 3× „Македонско") — a name-equality JOIN fans the insert out into duplicate LegacyIds.
OUTER APPLY (SELECT TOP 1 d.Id FROM dbo.DocumentIssuer d
             WHERE LTRIM(RTRIM(d.Name)) = LTRIM(RTRIM(s.IssuerName)) ORDER BY d.Id) di
OUTER APPLY (SELECT TOP 1 c2.Id FROM dbo.City c2
             WHERE LTRIM(RTRIM(c2.Name)) = LTRIM(RTRIM(s.IssuingCityName)) ORDER BY c2.Id) ci;

SET @rows = @@ROWCOUNT;
PRINT CONCAT('Inserted ', @rows, ' vehicle permissions.');

DECLARE @skipped int = (SELECT COUNT(*) FROM #src s
    WHERE NOT EXISTS (SELECT 1 FROM dbo.VehiclePermission x WHERE x.LegacyId = s.LegacyId));
IF @skipped > 0
    PRINT CONCAT('WARNING: ', @skipped, ' rows skipped (missing owner/authorized customer in VERTEST).');

COMMIT;
PRINT 'DONE.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    DECLARE @msg nvarchar(2048) = ERROR_MESSAGE();
    RAISERROR('migrate-permissions-from-vertest FAILED: %s', 16, 1, @msg);
END CATCH
