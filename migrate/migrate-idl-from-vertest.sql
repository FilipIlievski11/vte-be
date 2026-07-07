-- =============================================================================
-- migrate-idl-from-vertest.sql
-- Меѓународни возачки дозволи: pull the LIVE registry from the OTHER legacy DB —
-- [VERTEST].dbo.DocumentsInternationalDriveingLicences (2,077 rows, 2007→today,
-- still written to daily by the station; VTEZVV's copy died in 2018).
--
-- Reached through the existing linked server [VTEZVV_LIVE] (same SQL Server host,
-- cross-catalog 4-part names). Idempotent + incremental: keyed on
-- InternationalDrivingLicence.LegacyId (unique filtered index, AddIdlLegacyId
-- migration) — re-running only inserts rows not yet present. Safe to re-run any time.
--
-- VERTEST semantics (verified 2026-07-06, OPPOSITE of the dead VTEZVV table):
--   NumberOfLicence            = МЕЃУНАРОДНИОТ сериски број (2611 = денешниот)
--   NumberOfNationalLicence    = националниот U-број
--   DateCreated/ValidTillDate  = НАЦИОНАЛНАТА возачка (издадена/важи до)
--   DateCreatedInternatioanal/ValidTillDateInternatioanal [sic] = меѓународната
--   (12 old rows lack the international dates → fall back to the national pair;
--    1 row has a typo year 3025 → -1000 years.)
--
-- Client resolution: VERTEST has its OWN Customers id space → match by MB (ЕМБГ),
-- 1,680/1,719 match v2; the rest are inserted as new v2 Clients (identity ≥ 10M floor)
-- with the standard conventions: person name-order swap, citizenship Македонско,
-- ParentName, and the 3 ClientPersonalData documents.
--
-- No CustomerDebt rows are created — historical documents are already paid.
--
--   sqlcmd -S <server> -d VTE -I -f 65001 -i migrate\migrate-idl-from-vertest.sql
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
DECLARE @rows int;

-- ============================================================================
-- 0a. Stage the raw VERTEST tables locally FIRST (single-table remote pulls) —
--     joining across the linked server is N+1-slow; local joins are instant.
-- ============================================================================
IF OBJECT_ID('tempdb..#vIdl')    IS NOT NULL DROP TABLE #vIdl;
IF OBJECT_ID('tempdb..#vCust')   IS NOT NULL DROP TABLE #vCust;
IF OBJECT_ID('tempdb..#vStreet') IS NOT NULL DROP TABLE #vStreet;
IF OBJECT_ID('tempdb..#vCity')   IS NOT NULL DROP TABLE #vCity;
IF OBJECT_ID('tempdb..#vIss')    IS NOT NULL DROP TABLE #vIss;

SELECT Id, IdCustomer, IdIssuer, NumberOfLicence, NumberOfNationalLicence,
       DateCreated, ValidTillDate, DateCreatedInternatioanal, ValidTillDateInternatioanal, Note, Active
INTO #vIdl   FROM VTEZVV_LIVE.VERTEST.dbo.DocumentsInternationalDriveingLicences;
SELECT Id, MB, CustomerFirstName, CustomerSurname, ParentName, DateOfBirth, IsCompany, TaxNumber, PhoneNumber, eMail,
       IdLivingAddress, LivingAddressNumber, IdLivingCity, IdBirhCity, BLK, BLKDateIssued, BLKIssuer,
       PassportNumber, PassDateIssued, PassIssuer, DriveingLicenceNumber, DriveingLicenceDateIssued, DriveingLicenceIssuer
INTO #vCust  FROM VTEZVV_LIVE.VERTEST.dbo.Customers;
SELECT Id, StreetName INTO #vStreet FROM VTEZVV_LIVE.VERTEST.dbo.Streets;
SELECT Id, CityName   INTO #vCity   FROM VTEZVV_LIVE.VERTEST.dbo.Cities;
SELECT Id, IssuerName INTO #vIss    FROM VTEZVV_LIVE.VERTEST.dbo.RegistrationIssuers;

-- ============================================================================
-- 0b. Build the staging rows from LOCAL joins.
-- ============================================================================
IF OBJECT_ID('tempdb..#src') IS NOT NULL DROP TABLE #src;
SELECT
    d.Id            AS LegacyId,
    d.IdCustomer,
    d.NumberOfLicence,
    ISNULL(NULLIF(LTRIM(RTRIM(d.NumberOfNationalLicence)), N''), N'-') AS NumberOfNationalLicence,
    -- меѓународни датуми со fallback на националните + поправка на 3025-та година
    CASE WHEN YEAR(COALESCE(d.DateCreatedInternatioanal, d.DateCreated)) > 2100
         THEN DATEADD(YEAR, -1000, COALESCE(d.DateCreatedInternatioanal, d.DateCreated))
         ELSE COALESCE(d.DateCreatedInternatioanal, d.DateCreated) END AS IssuedDate,
    CASE WHEN YEAR(COALESCE(d.ValidTillDateInternatioanal, d.ValidTillDate)) > 2100
         THEN DATEADD(YEAR, -1000, COALESCE(d.ValidTillDateInternatioanal, d.ValidTillDate))
         ELSE COALESCE(d.ValidTillDateInternatioanal, d.ValidTillDate) END AS ValidTillDate,
    NULLIF(LTRIM(RTRIM(d.Note)), N'') AS Note,
    d.Active,
    -- национална возачка (за applicant snapshot)
    d.DateCreated   AS NatIssued,
    CASE WHEN YEAR(d.ValidTillDate) > 2100 THEN DATEADD(YEAR, -1000, d.ValidTillDate) ELSE d.ValidTillDate END AS NatValid,
    riNat.IssuerName AS NatIssuerName,
    -- клиент (VERTEST Customers) — за MB-мапирање + snapshot + евентуално креирање
    NULLIF(LTRIM(RTRIM(c.MB)), N'')                  AS MB,
    c.IsCompany,
    NULLIF(LTRIM(RTRIM(c.CustomerFirstName)), N'')   AS SurnameRaw,   -- легаси: презиме
    NULLIF(LTRIM(RTRIM(c.CustomerSurname)),   N'')   AS FirstNameRaw, -- легаси: име
    NULLIF(LTRIM(RTRIM(c.ParentName)),        N'')   AS ParentName,
    c.DateOfBirth,
    NULLIF(LTRIM(RTRIM(bc.CityName)),  N'')          AS BirthCityName,
    NULLIF(LTRIM(RTRIM(CONCAT_WS(N' ',
        NULLIF(LTRIM(RTRIM(st.StreetName)),          N''),
        NULLIF(LTRIM(RTRIM(c.LivingAddressNumber)),  N''),
        NULLIF(LTRIM(RTRIM(lc.CityName)),            N'')))), N'') AS LivingAddress,
    NULLIF(LTRIM(RTRIM(c.PhoneNumber)), N'')         AS PhoneNumber,
    NULLIF(LTRIM(RTRIM(c.eMail)),       N'')         AS Email,
    NULLIF(LTRIM(RTRIM(c.TaxNumber)),   N'')         AS TaxNumber,
    -- документи (за snapshot + ClientPersonalData на новите клиенти)
    NULLIF(LTRIM(RTRIM(c.PassportNumber)), N'')      AS PassportNumber,
    c.PassDateIssued,
    riPass.IssuerName                                 AS PassIssuerName,
    NULLIF(LTRIM(RTRIM(c.BLK)), N'')                 AS IdCardNumber,
    c.BLKDateIssued,
    riBlk.IssuerName                                  AS IdCardIssuerName,
    NULLIF(LTRIM(RTRIM(c.DriveingLicenceNumber)), N'') AS DrvLicNumber,
    c.DriveingLicenceDateIssued,
    riDrv.IssuerName                                  AS DrvLicIssuerName,
    c.BLKIssuer, c.PassIssuer, c.DriveingLicenceIssuer
INTO #src
FROM #vIdl d
LEFT JOIN #vCust c    ON c.Id = d.IdCustomer
LEFT JOIN #vIss riNat  ON riNat.Id  = NULLIF(d.IdIssuer, 0)
LEFT JOIN #vIss riPass ON riPass.Id = NULLIF(c.PassIssuer, 0)
LEFT JOIN #vIss riBlk  ON riBlk.Id  = NULLIF(c.BLKIssuer, 0)
LEFT JOIN #vIss riDrv  ON riDrv.Id  = NULLIF(c.DriveingLicenceIssuer, 0)
LEFT JOIN #vCity  bc ON bc.Id = NULLIF(c.IdBirhCity, 0)
LEFT JOIN #vCity  lc ON lc.Id = NULLIF(c.IdLivingCity, 0)
LEFT JOIN #vStreet st ON st.Id = NULLIF(c.IdLivingAddress, 0)
WHERE NOT EXISTS (SELECT 1 FROM dbo.InternationalDrivingLicence x WHERE x.LegacyId = d.Id);

SET @rows = @@ROWCOUNT;
PRINT CONCAT('Staged ', @rows, ' new registry rows from VERTEST.');
IF @rows = 0 BEGIN PRINT 'Nothing to migrate.'; COMMIT; RETURN; END;

-- ============================================================================
-- 1. Resolve/insert clients.
--    Match by MB against the legacy-mirror range (< 10M) first, then whole table.
-- ============================================================================
IF OBJECT_ID('tempdb..#cust') IS NOT NULL DROP TABLE #cust;
SELECT s.IdCustomer,
       MIN(s.MB) AS MB,
       CAST(NULL AS bigint) AS ClientId
INTO #cust
FROM #src s
WHERE s.IdCustomer IS NOT NULL
GROUP BY s.IdCustomer;

-- 1a. по MB (најмал Id, прво огледалниот опсег)
UPDATE cu SET cu.ClientId = m.ClientId
FROM #cust cu
CROSS APPLY (SELECT TOP 1 c.Id AS ClientId FROM dbo.Client c
             WHERE c.MB = cu.MB AND cu.MB IS NOT NULL
             ORDER BY CASE WHEN c.Id < 10000000 THEN 0 ELSE 1 END, c.Id) m;

-- 1b. креирај ги останатите (нема MB-пар во v2) — идентитетот е над 10M подот
DECLARE @newClients TABLE (IdCustomer bigint, ClientId bigint);

MERGE dbo.Client AS tgt
USING (
    SELECT cu.IdCustomer, s.MB, s.IsCompany, s.FirstNameRaw, s.SurnameRaw, s.ParentName,
           s.DateOfBirth, s.LivingAddress, s.PhoneNumber, s.Email, s.TaxNumber
    FROM #cust cu
    JOIN (SELECT s2.*, ROW_NUMBER() OVER (PARTITION BY s2.IdCustomer ORDER BY s2.LegacyId DESC) rn FROM #src s2) s
      ON s.IdCustomer = cu.IdCustomer AND s.rn = 1
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
            src.MB, src.LivingAddress, src.TaxNumber, src.PhoneNumber, src.Email, src.DateOfBirth, 1, GETUTCDATE())
OUTPUT src.IdCustomer, inserted.Id INTO @newClients (IdCustomer, ClientId);

UPDATE cu SET cu.ClientId = n.ClientId
FROM #cust cu JOIN @newClients n ON n.IdCustomer = cu.IdCustomer
WHERE cu.ClientId IS NULL;

DECLARE @newClientCount int = (SELECT COUNT(*) FROM @newClients);
PRINT CONCAT('Created ', @newClientCount, ' new clients (no MB match).');

-- документи за новокреираните клиенти
INSERT INTO dbo.ClientPersonalData (ClientId, PersonalDataTypeId, DocumentIssuerId, Number, CreatedAt, Active)
SELECT n.ClientId, t.typeId,
       COALESCE(di.Id, @fallbackIssuer),
       t.num, ISNULL(t.dateIssued, GETUTCDATE()), 1
FROM @newClients n
JOIN (SELECT s2.*, ROW_NUMBER() OVER (PARTITION BY s2.IdCustomer ORDER BY s2.LegacyId DESC) rn FROM #src s2) s
  ON s.IdCustomer = n.IdCustomer AND s.rn = 1
CROSS APPLY (VALUES
    (CAST(3 AS tinyint), s.IdCardNumber,   s.BLKDateIssued,             s.BLKIssuer),
    (CAST(2 AS tinyint), s.PassportNumber, s.PassDateIssued,            s.PassIssuer),
    (CAST(1 AS tinyint), s.DrvLicNumber,   s.DriveingLicenceDateIssued, s.DriveingLicenceIssuer)
) t (typeId, num, dateIssued, legacyIssuer)
LEFT JOIN dbo.DocumentIssuer di ON di.Id = CASE WHEN t.legacyIssuer BETWEEN 1 AND 255 THEN CAST(t.legacyIssuer AS tinyint) END
WHERE t.num IS NOT NULL;

-- ============================================================================
-- 2. Insert the licences (with the full applicant print snapshot).
-- ============================================================================
INSERT INTO dbo.InternationalDrivingLicence (
    CompanyId, ClientId, IssuerOrganizationId,
    NumberOfLicence, NumberOfNationalLicence, IssuedDate, ValidTillDate, Note,
    ApplicantFirstName, ApplicantLastName, ApplicantParentName, ApplicantCitizenship,
    ApplicantDateOfBirth, ApplicantBirthPlace, ApplicantAddress,
    ApplicantPassportNumber, ApplicantPassportIssuer, ApplicantPassportDate,
    ApplicantIdCardNumber, ApplicantIdCardIssuer, ApplicantIdCardDate,
    ApplicantNationalLicenceIssuer, ApplicantNationalLicenceDate, ApplicantNationalLicenceExpiry,
    Active, CreatedAt, LegacyId)
SELECT
    CAST(4 AS tinyint), cu.ClientId, 37,
    s.NumberOfLicence, s.NumberOfNationalLicence, s.IssuedDate, s.ValidTillDate, s.Note,
    CASE WHEN s.IsCompany = 1 THEN s.SurnameRaw ELSE COALESCE(s.FirstNameRaw, s.SurnameRaw) END,
    CASE WHEN s.IsCompany = 1 THEN s.FirstNameRaw
         ELSE CASE WHEN s.FirstNameRaw IS NULL THEN NULL ELSE s.SurnameRaw END END,
    s.ParentName, N'Македонско',
    s.DateOfBirth, s.BirthCityName, s.LivingAddress,
    s.PassportNumber, s.PassIssuerName, s.PassDateIssued,
    s.IdCardNumber, s.IdCardIssuerName, s.BLKDateIssued,
    s.NatIssuerName, s.NatIssued, s.NatValid,
    s.Active, GETUTCDATE(), s.LegacyId
FROM #src s
JOIN #cust cu ON cu.IdCustomer = s.IdCustomer AND cu.ClientId IS NOT NULL;

SET @rows = @@ROWCOUNT;
PRINT CONCAT('Inserted ', @rows, ' international driving licences.');

-- ============================================================================
-- 3. Categories (checked only; VERTEST DriveingLicenceCtegories ids 3-18 = v2 seed 1:1).
-- ============================================================================
IF OBJECT_ID('tempdb..#vIdlCat') IS NOT NULL DROP TABLE #vIdlCat;
SELECT IdInternationalDrivingLicence, IdLicenceCategorie
INTO #vIdlCat
FROM VTEZVV_LIVE.VERTEST.dbo.[DocumentsInternationalDriveingLicences.ValidForCategories]
WHERE IsCheck = 1;

INSERT INTO dbo.InternationalDrivingLicenceCategory (InternationalDrivingLicenceId, DrivingLicenceCategoryId)
SELECT x.Id, vc.IdLicenceCategorie
FROM #vIdlCat vc
JOIN dbo.InternationalDrivingLicence x ON x.LegacyId = vc.IdInternationalDrivingLicence
JOIN dbo.DrivingLicenceCategory dc ON dc.Id = vc.IdLicenceCategorie
WHERE NOT EXISTS (SELECT 1 FROM dbo.InternationalDrivingLicenceCategory ec
                  WHERE ec.InternationalDrivingLicenceId = x.Id AND ec.DrivingLicenceCategoryId = vc.IdLicenceCategorie);

SET @rows = @@ROWCOUNT;
PRINT CONCAT('Inserted ', @rows, ' licence-category rows.');

COMMIT;
PRINT 'DONE.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    DECLARE @msg nvarchar(2048) = ERROR_MESSAGE();
    RAISERROR('migrate-idl-from-vertest FAILED: %s', 16, 1, @msg);
END CATCH
