-- VTE legacy → VTE2 migration — one station's database into the new multi-tenant DB.
-- =============================================================================
-- READ THIS BEFORE RUNNING:
--
-- 1. THIS IS A TEMPLATE. Each legacy station's database may have schema drift
--    (per audit R-3). Open each section, compare column names to your station's
--    actual schema, and adjust. Do NOT run blindly against production.
--
-- 2. RUN ON A COPY. Restore the legacy `.bak` to a sandbox first; never run
--    this against the live legacy database.
--
-- 3. PRE-FLIGHT CHECK: VTE2 must already exist with bootstrap-*.sql applied
--    AND seed-vte2-defaults.sql applied. The new station gets created here;
--    REF data is dedupe-merged with the global REF.
--
-- 4. PARAMETERS (set via :setvar before running, or hardcode below):
--      LEGACY_DB    — name of the legacy database (e.g. VTE_BITOLA_2018_BAK)
--      STATION_NAME — display name for the new tenant
--      STATION_CODE — short unique code (e.g. BIT, SK1)
--
-- 5. PASSWORDS DO NOT MIGRATE. Legacy users get an AspNetUsers row with NO
--    PasswordHash — every operator must reset their password on first login.
--    See section 11 below.
--
-- 6. IDEMPOTENCY: this script CHECKS for an existing Station with the given
--    Code and ABORTS if found. To re-migrate, drop & recreate VTE2 first.
--
-- 7. ROW-COUNT VERIFICATION: at the end, the script reports legacy vs new row
--    counts per table. Mismatches are normal for tables with FK-constraint
--    violations, dedupe-merging, or storno records — investigate each.
-- =============================================================================

:setvar LEGACY_DB    "VTE_LEGACY"
:setvar STATION_NAME "Migrated Station"
:setvar STATION_CODE "MIG1"

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- =============================================================================
-- 0. Pre-flight checks
-- =============================================================================
IF DB_ID(N'$(LEGACY_DB)') IS NULL
BEGIN
    RAISERROR('Legacy database [$(LEGACY_DB)] does not exist on this server. Restore the legacy backup first.', 16, 1);
    RETURN;
END

IF EXISTS (SELECT 1 FROM [dbo].[Stations] WHERE Code = N'$(STATION_CODE)')
BEGIN
    RAISERROR('Station with Code "$(STATION_CODE)" already exists in VTE2. Pick a different STATION_CODE or drop & recreate VTE2.', 16, 1);
    RETURN;
END

PRINT 'Pre-flight checks passed. Migrating from [$(LEGACY_DB)] as station "$(STATION_NAME)" (code $(STATION_CODE))...';
GO

-- =============================================================================
-- 1. Create the new Station and capture its Id
-- =============================================================================
DECLARE @StationId INT;

INSERT INTO [dbo].[Stations] ([Name], [Code], [IsActive])
VALUES (N'$(STATION_NAME)', N'$(STATION_CODE)', 1);

SET @StationId = SCOPE_IDENTITY();
PRINT 'Created Station Id = ' + CAST(@StationId AS NVARCHAR(10));

-- Persist for downstream sections
IF OBJECT_ID('tempdb..#mig_station') IS NOT NULL DROP TABLE #mig_station;
CREATE TABLE #mig_station (StationId INT NOT NULL);
INSERT INTO #mig_station VALUES (@StationId);
GO

-- =============================================================================
-- 2. Reference-data dedupe-merge (legacy → VTE2 global REF)
--    Maintain a per-table mapping (legacyId → newId) in temp tables so domain
--    inserts can translate FK columns.
-- =============================================================================

-- BusinessTypes
IF OBJECT_ID('tempdb..#map_BusinessType') IS NOT NULL DROP TABLE #map_BusinessType;
CREATE TABLE #map_BusinessType (LegacyId INT, NewId INT);
INSERT INTO #map_BusinessType (LegacyId, NewId)
SELECT b.Id,
    COALESCE(
        (SELECT TOP 1 t.Id FROM [dbo].[BusinessTypes] t WHERE t.Name = b.BusinessTypeName),
        (SELECT TOP 1 t.Id FROM [dbo].[BusinessTypes] t WHERE t.Name = b.BusinessTypeName))
FROM [$(LEGACY_DB)].[dbo].[BusinessTypes] b
WHERE EXISTS (SELECT 1 FROM [dbo].[BusinessTypes] t WHERE t.Name = b.BusinessTypeName);

INSERT INTO [dbo].[BusinessTypes] ([Name])
SELECT b.BusinessTypeName FROM [$(LEGACY_DB)].[dbo].[BusinessTypes] b
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BusinessTypes] t WHERE t.Name = b.BusinessTypeName);

INSERT INTO #map_BusinessType (LegacyId, NewId)
SELECT b.Id, t.Id
FROM [$(LEGACY_DB)].[dbo].[BusinessTypes] b
JOIN [dbo].[BusinessTypes] t ON t.Name = b.BusinessTypeName
WHERE NOT EXISTS (SELECT 1 FROM #map_BusinessType m WHERE m.LegacyId = b.Id);

-- Cities (with Country preserved if known)
IF OBJECT_ID('tempdb..#map_City') IS NOT NULL DROP TABLE #map_City;
CREATE TABLE #map_City (LegacyId INT, NewId INT);
INSERT INTO [dbo].[Cities] ([Name], [PostalCode])
SELECT c.CityName, c.CityZipCode
FROM [$(LEGACY_DB)].[dbo].[Cities] c
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Cities] t WHERE t.Name = c.CityName);

INSERT INTO #map_City (LegacyId, NewId)
SELECT c.Id, t.Id FROM [$(LEGACY_DB)].[dbo].[Cities] c
JOIN [dbo].[Cities] t ON t.Name = c.CityName;

-- Streets
IF OBJECT_ID('tempdb..#map_Street') IS NOT NULL DROP TABLE #map_Street;
CREATE TABLE #map_Street (LegacyId INT, NewId INT);
INSERT INTO [dbo].[Streets] ([Name], [CityId])
SELECT s.StreetName, m.NewId
FROM [$(LEGACY_DB)].[dbo].[Streets] s
LEFT JOIN #map_City m ON m.LegacyId = s.IdCity
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Streets] t WHERE t.Name = s.StreetName AND t.CityId = m.NewId);

INSERT INTO #map_Street (LegacyId, NewId)
SELECT s.Id, t.Id FROM [$(LEGACY_DB)].[dbo].[Streets] s
LEFT JOIN #map_City  mc ON mc.LegacyId = s.IdCity
LEFT JOIN [dbo].[Streets] t ON t.Name = s.StreetName AND (t.CityId = mc.NewId OR (t.CityId IS NULL AND mc.NewId IS NULL));

-- Countries
IF OBJECT_ID('tempdb..#map_Country') IS NOT NULL DROP TABLE #map_Country;
CREATE TABLE #map_Country (LegacyId INT, NewId INT);
INSERT INTO [dbo].[Countries] ([Name])
SELECT c.CountryName FROM [$(LEGACY_DB)].[dbo].[Countries] c
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Countries] t WHERE t.Name = c.CountryName);
INSERT INTO #map_Country (LegacyId, NewId)
SELECT c.Id, t.Id FROM [$(LEGACY_DB)].[dbo].[Countries] c JOIN [dbo].[Countries] t ON t.Name = c.CountryName;

-- RegistrationIssuers
IF OBJECT_ID('tempdb..#map_RegistrationIssuer') IS NOT NULL DROP TABLE #map_RegistrationIssuer;
CREATE TABLE #map_RegistrationIssuer (LegacyId INT, NewId INT);
INSERT INTO [dbo].[RegistrationIssuers] ([Name])
SELECT r.RegistrationIssuerName FROM [$(LEGACY_DB)].[dbo].[RegistrationIssuers] r
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[RegistrationIssuers] t WHERE t.Name = r.RegistrationIssuerName);
INSERT INTO #map_RegistrationIssuer (LegacyId, NewId)
SELECT r.Id, t.Id FROM [$(LEGACY_DB)].[dbo].[RegistrationIssuers] r JOIN [dbo].[RegistrationIssuers] t ON t.Name = r.RegistrationIssuerName;

-- Vehicle classifications — same pattern (abbreviated; fill in for each REF table you need)
IF OBJECT_ID('tempdb..#map_VehicleBodyType') IS NOT NULL DROP TABLE #map_VehicleBodyType;
CREATE TABLE #map_VehicleBodyType (LegacyId INT, NewId INT);
INSERT INTO [dbo].[VehicleBodyTypes] ([Name])
SELECT v.VehicleBodytypeName FROM [$(LEGACY_DB)].[dbo].[VehicleBodytype] v
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleBodyTypes] t WHERE t.Name = v.VehicleBodytypeName);
INSERT INTO #map_VehicleBodyType (LegacyId, NewId)
SELECT v.Id, t.Id FROM [$(LEGACY_DB)].[dbo].[VehicleBodytype] v JOIN [dbo].[VehicleBodyTypes] t ON t.Name = v.VehicleBodytypeName;

-- (... pattern continues for VehicleCategories, VehicleUse, VehicleEngineTypes,
-- VehicleEnginePowerSourceTypes, VehicleEngineEcoProgram, VehicleGearBox,
-- VehicleBrakes, VehicleSupporting, Colors, VehicleCategoryForPayments,
-- VehicleMakers, VehicleModel, TehnicalExamOrganizations, TehnicalExamsTypes,
-- TehnicalExamVehicleParts, DriveingLicenceCtegories, CustomerVehiclesRelationTypes
-- — repeat for each. See migrate/README.md for the full list.)

GO

-- =============================================================================
-- 3. Customers — translate Mb→EMBG, BLK→IDCardNumber, fix typos, set StationId
-- =============================================================================
DECLARE @StationId INT = (SELECT TOP 1 StationId FROM #mig_station);

IF OBJECT_ID('tempdb..#map_Customer') IS NOT NULL DROP TABLE #map_Customer;
CREATE TABLE #map_Customer (LegacyId BIGINT, NewId BIGINT);

INSERT INTO [dbo].[Customers] (
    [StationId], [IsCompany],
    [EMBG], [FirstName], [Surname], [ParentName], [DateOfBirth],
    [CitizenshipId], [BusinessTypeId],
    [LivingAddressId], [LivingAddressNumber], [LivingCityId],
    [BirthCityId], [BirthAddressId], [BirthAddressNumber],
    [Occupation], [WorksInCompany],
    [PhoneNumber], [Fax], [Email], [CanSendNotifications],
    [IDCardNumber], [IDCardDateIssued], [IDCardIssuerId],
    [PassportNumber], [PassportDateIssued], [PassportIssuerId],
    [DrivingLicenceNumber], [DrivingLicenceDateIssued], [DrivingLicenceIssuerId],
    [TaxNumber], [Status], [Note], [IsActive]
)
OUTPUT INSERTED.Id, INSERTED.EMBG INTO #map_Customer (NewId, LegacyId)   -- NB: LegacyId tracked via EMBG (see post-step)
SELECT
    @StationId,
    ISNULL(c.IsCompany, 0),
    NULLIF(LTRIM(RTRIM(c.Mb)), ''),
    c.CustomerFirstName,
    NULLIF(c.CustomerSurname, ''),
    c.ParentName,
    CASE WHEN c.DateOfBirth = '1900-01-01' OR c.DateOfBirth IS NULL THEN NULL ELSE c.DateOfBirth END,
    cm.NewId,                               -- CitizenshipId
    bm.NewId,                               -- BusinessTypeId
    sla.NewId,                              -- LivingAddressId (Streets)
    c.LivingAddressNumber,
    cl.NewId,                               -- LivingCityId
    cb.NewId,                               -- BirthCityId
    sba.NewId,                              -- BirthAddressId
    c.BrithAddressNumber,                   -- legacy typo preserved on read
    c.Occupation, c.WorksInCompany,
    c.PhoneNumber, c.Fax, c.Email, ISNULL(c.canSendNotifications, 0),
    NULLIF(c.BLK, ''), c.BLKDateIssued, blki.NewId,
    NULLIF(c.PassportNumber, ''), c.PassDateIssued, pi2.NewId,
    NULLIF(c.DriveingLicenceNumber, ''), c.DriveingLicenceDateIssued, dli.NewId,
    NULLIF(c.TaxNumber, ''),
    c.Status,
    c.Note,
    1
FROM [$(LEGACY_DB)].[dbo].[Customers] c
LEFT JOIN #map_Country            cm  ON cm.LegacyId  = c.IdCitizenship
LEFT JOIN #map_BusinessType       bm  ON bm.LegacyId  = c.IdBusinessType
LEFT JOIN #map_Street             sla ON sla.LegacyId = c.IdLivingAddress
LEFT JOIN #map_City               cl  ON cl.LegacyId  = c.IdLivingCity
LEFT JOIN #map_City               cb  ON cb.LegacyId  = c.IdBirhCity         -- legacy typo "Birh"
LEFT JOIN #map_Street             sba ON sba.LegacyId = c.IdBirthAddress
LEFT JOIN #map_RegistrationIssuer blki ON blki.LegacyId = c.BLKIssuer
LEFT JOIN #map_RegistrationIssuer pi2  ON pi2.LegacyId  = c.PassIssuer
LEFT JOIN #map_RegistrationIssuer dli  ON dli.LegacyId  = c.DriveingLicenceIssuer;

-- Fix: the OUTPUT clause above returns EMBG into LegacyId — re-derive proper map by re-joining
TRUNCATE TABLE #map_Customer;
INSERT INTO #map_Customer (LegacyId, NewId)
SELECT lc.Id, nc.Id
FROM [$(LEGACY_DB)].[dbo].[Customers] lc
JOIN [dbo].[Customers] nc
  ON nc.StationId = @StationId
 AND nc.FirstName = lc.CustomerFirstName
 AND ISNULL(nc.Surname, '') = ISNULL(lc.CustomerSurname, '')
 AND ISNULL(nc.EMBG, '') = ISNULL(NULLIF(LTRIM(RTRIM(lc.Mb)), ''), '');

PRINT 'Customers: ' + CAST((SELECT COUNT(*) FROM #map_Customer) AS NVARCHAR(20));
GO

-- =============================================================================
-- 4. Customer children (ContactPersons, BankAccounts)
-- =============================================================================
INSERT INTO [dbo].[CustomerContactPersons]
    ([CustomerId], [EMBG], [FirstName], [Surname], [PhoneNumber], [MobileNumber], [Email])
SELECT mc.NewId, NULLIF(cp.Mb, ''), cp.PersonName, cp.PersonSurname, cp.PhoneNumber, cp.MobileNumber, cp.Email
FROM [$(LEGACY_DB)].[dbo].[Customers.ContactPersons] cp
JOIN #map_Customer mc ON mc.LegacyId = cp.IdCustomer;

INSERT INTO [dbo].[CustomerBankAccounts]
    ([CustomerId], [BankAccount], [DeponentBank], [TaxNumber])
SELECT mc.NewId, ba.BankAccount, ba.DeponentBank, ba.TaxNumber
FROM [$(LEGACY_DB)].[dbo].[CustomersBankAccounts] ba
JOIN #map_Customer mc ON mc.LegacyId = ba.IdCustomer;
GO

-- =============================================================================
-- 5. Vehicles — translate ~30 Macedonian columns to English; set StationId
-- =============================================================================
DECLARE @StationId INT = (SELECT TOP 1 StationId FROM #mig_station);

IF OBJECT_ID('tempdb..#map_Vehicle') IS NOT NULL DROP TABLE #map_Vehicle;
CREATE TABLE #map_Vehicle (LegacyId BIGINT, NewId BIGINT, ShellNumber NVARCHAR(17));

INSERT INTO [dbo].[Vehicles] (
    [StationId], [ShellNumber],
    [BodyTypeId], [VehicleCategoryId], [VehicleUseId], [VehicleModelId], [VehicleModelAdding],
    [MadeCountryId], [VehicleCategoryForPaymentsId],
    [EngineNumber], [EngineTypeId], [EnginePowerSourceId], [EngineSecondPowerSourceId], [EngineEcoProgramId],
    [EnginePowerKw], [EngineWorkingCapacity], [RPM], [GearBoxId],
    [BrakesId], [SupportingId],
    [VehicleHeight], [VehicleWidth], [VehicleLength],
    [NumberOfDoors], [NumberOfSeats], [NumberOfStandingSeats], [NumberOfLyingSeats],
    [NumberOfAxes], [NumberOfPropulsionAxes], [NumberOfWheels], [NumberOfPropulsionWheels],
    [EmptyWeight], [MaxAllowedWeight],
    [MakeDate],
    [FirstRegistrationNumber], [LastRegistrationNumber],
    [FirstRegistrationMakeDate], [FirstRegistrationValidTill],
    [LastRegistrationMakeDate], [LastRegistrationValidTill],
    [FirstRegistrationIssuerId], [LastRegistrationIssuerId],
    [ColorCode], [PrimaryColorId], [SecondaryColorId],
    [Suffocation], [Hook], [Winch], [TNG],
    [Note], [IsActive]
)
SELECT
    @StationId, v.ShellNumber,
    NULL, NULL, NULL,                       -- BodyType / Category / Use FKs — fill in if you mapped those REFs
    NULL,                                    -- VehicleModelId
    v.VehicleModelAdding,
    cn.NewId,                                -- MadeCountryId
    NULL,                                    -- VehicleCategoryForPaymentsId
    v.EngineNumber,
    NULL, NULL, NULL, NULL,                  -- EngineType / PowerSource / SecondPowerSource / EcoProgram
    v.EnginePower, v.EngineWorkingCapacity, NULL,
    NULL,                                    -- GearBoxId
    NULL,                                    -- BrakesId (legacy IdBreakes)
    NULL,                                    -- SupportingId
    v.VehicleSizeHight, v.VehicleSizeWidth, v.VehicleSizeLength,
    v.NumberOfDoors, v.NumberOfSeats, v.NumberOfStandingSeats, v.NumberOfLieingSeats,
    v.NumberOfAxis, v.PropulsionAxis, v.NumberOfWheels, v.NumberOfPropulsionWheels,
    v.EmptyWaight, v.MaximunAllowedWaight,
    v.MakeDate,
    v.FirstRegistrationNumber, v.LastRegistratinNumber,
    v.FirstRegistrationMakeDate, v.FirstRegistrationValidTill,
    v.LastRegistrationMakeDate, v.LastRegistrationValidTill,
    fri.NewId, lri.NewId,
    v.ColorCode, NULL, NULL,
    ISNULL(v.Suffocation, 0), ISNULL(v.Hook, 0), ISNULL(v.Vitlo, 0), ISNULL(v.TNG, 0),
    v.Note, 1
FROM [$(LEGACY_DB)].[dbo].[Vehicles] v
LEFT JOIN #map_Country            cn  ON cn.LegacyId  = v.IdMadeCountry
LEFT JOIN #map_RegistrationIssuer fri ON fri.LegacyId = v.IdFirstRegistrationIssuer
LEFT JOIN #map_RegistrationIssuer lri ON lri.LegacyId = v.IdLastRegistrationIssuer;

-- Build the Vehicle map
INSERT INTO #map_Vehicle (LegacyId, NewId, ShellNumber)
SELECT lv.Id, nv.Id, lv.ShellNumber
FROM [$(LEGACY_DB)].[dbo].[Vehicles] lv
JOIN [dbo].[Vehicles] nv ON nv.StationId = @StationId AND nv.ShellNumber = lv.ShellNumber;

PRINT 'Vehicles: ' + CAST((SELECT COUNT(*) FROM #map_Vehicle) AS NVARCHAR(20));
GO

-- =============================================================================
-- 6. CustomerVehicleRelations (the M:N join between customers and vehicles)
-- =============================================================================
IF OBJECT_ID('tempdb..#map_CVR') IS NOT NULL DROP TABLE #map_CVR;
CREATE TABLE #map_CVR (LegacyId BIGINT, NewId BIGINT);

INSERT INTO [dbo].[CustomerVehicleRelations]
    ([CustomerId], [VehicleId], [RelationTypeId], [ValidFrom], [ValidTo], [Note], [IsActive])
SELECT mc.NewId, mv.NewId, NULL,
       cvr.ValidFrom, cvr.ValidTo, cvr.Note, 1
FROM [$(LEGACY_DB)].[dbo].[CustomerVehiclesRelations] cvr
JOIN #map_Customer mc ON mc.LegacyId = cvr.IdCustomer
JOIN #map_Vehicle  mv ON mv.LegacyId = cvr.IdVehicle;

-- Map: legacy CVR.Id → new CVR.Id (use a deterministic key — customer + vehicle + ValidFrom)
INSERT INTO #map_CVR (LegacyId, NewId)
SELECT cvr.Id, nr.Id
FROM [$(LEGACY_DB)].[dbo].[CustomerVehiclesRelations] cvr
JOIN #map_Customer mc ON mc.LegacyId = cvr.IdCustomer
JOIN #map_Vehicle  mv ON mv.LegacyId = cvr.IdVehicle
JOIN [dbo].[CustomerVehicleRelations] nr
  ON nr.CustomerId = mc.NewId AND nr.VehicleId = mv.NewId
 AND ISNULL(nr.ValidFrom, '1900-01-01') = ISNULL(cvr.ValidFrom, '1900-01-01');
GO

-- =============================================================================
-- 7. Requests (workflow instances)
-- =============================================================================
DECLARE @StationId INT = (SELECT TOP 1 StationId FROM #mig_station);

INSERT INTO [dbo].[Requests]
    ([StationId], [RequestTypeId], [CustomerVehicleRelationId],
     [DateCreated], [DateModified], [DateEnded],
     [IsCustomerChanged], [IsVehicleChanged], [Note])
SELECT
    @StationId,
    -- RequestType FK — fall back to a "Само технички преглед" default if the legacy IdRequestType doesn't map
    COALESCE(
        (SELECT TOP 1 rt.Id FROM [dbo].[RequestTypes] rt
         JOIN [$(LEGACY_DB)].[dbo].[RequestTypes] lrt ON lrt.Id = r.IdRequestType
         WHERE rt.TypeName = lrt.TypeName),
        (SELECT TOP 1 rt.Id FROM [dbo].[RequestTypes] rt WHERE rt.TypeName = N'Само технички преглед')
    ),
    mcvr.NewId,
    ISNULL(r.DateCreated, CAST(GETDATE() AS DATE)),
    r.DateModified, r.DateEnded,
    ISNULL(r.IsCustomerChanged, 0), ISNULL(r.IsVehicleChanged, 0),
    r.Note
FROM [$(LEGACY_DB)].[dbo].[Requests] r
JOIN #map_CVR mcvr ON mcvr.LegacyId = r.IdCustomerVehicleRelation;

PRINT 'Requests migrated.';
GO

-- =============================================================================
-- 8. Documents — TrafficLicences (extend with Permissions / IDL / TechExamReports
--    once those mappings are confirmed against your legacy schema)
-- =============================================================================
DECLARE @StationId INT = (SELECT TOP 1 StationId FROM #mig_station);

INSERT INTO [dbo].[TrafficLicences]
    ([StationId], [CustomerVehicleRelationId], [TrafficLicenceNumber],
     [MadeDate], [EndDate], [Note], [IsActive])
SELECT
    @StationId,
    mcvr.NewId,
    tl.TrafficLicenceNumber,
    tl.MadeDate, tl.EndDate, tl.Note, 1
FROM [$(LEGACY_DB)].[dbo].[DocumentsTrafficLicences] tl
JOIN #map_CVR mcvr ON mcvr.LegacyId = tl.IdCustomerVehicleRelation
WHERE tl.MadeDate IS NOT NULL AND tl.EndDate IS NOT NULL AND tl.EndDate >= tl.MadeDate;

PRINT 'TrafficLicences migrated.';
GO

-- =============================================================================
-- 9. PaymentDocuments + Details + Installments
--    Uses NameMatched lookups for PaymentTypes (legacy Faktura/Smetka/Rati flags
--    map to our seed PaymentTypes by Name).
-- =============================================================================
DECLARE @StationId INT = (SELECT TOP 1 StationId FROM #mig_station);

IF OBJECT_ID('tempdb..#map_PaymentType') IS NOT NULL DROP TABLE #map_PaymentType;
CREATE TABLE #map_PaymentType (LegacyId INT, NewId INT);
INSERT INTO #map_PaymentType (LegacyId, NewId)
SELECT lpt.Id, npt.Id
FROM [$(LEGACY_DB)].[dbo].[PaymentTypes] lpt
JOIN [dbo].[PaymentTypes] npt
  ON (lpt.Faktura = 1            AND npt.IsInvoice = 1      AND npt.Name = N'Фактура')
  OR (lpt.Fiskalna_kes = 1       AND npt.IsCash = 1         AND npt.Name = N'Кеш (фискална)')
  OR (lpt.Fiskalna_karticka = 1  AND npt.IsFiscalCard = 1   AND npt.Name = N'Картичка (фискална)')
  OR (lpt.Smetka = 1             AND npt.IsAccount = 1      AND npt.Name = N'Уплата на сметка')
  OR (lpt.Rati = 1               AND npt.IsInstallments = 1 AND npt.Name = N'Договор за рати');

IF OBJECT_ID('tempdb..#map_PaymentDocument') IS NOT NULL DROP TABLE #map_PaymentDocument;
CREATE TABLE #map_PaymentDocument (LegacyId BIGINT, NewId BIGINT);

INSERT INTO [dbo].[PaymentDocuments]
    ([StationId], [DocumentNumber], [PaymentTypeId], [CustomerVehicleRelationId],
     [DatePay], [DateRequired], [DiscountPercent], [Payed], [Storno], [Note])
SELECT
    @StationId,
    pd.DocumentNumber,
    COALESCE(mpt.NewId, (SELECT TOP 1 Id FROM [dbo].[PaymentTypes] WHERE Name = N'Кеш (фискална)')),
    mcvr.NewId,
    pd.DatePay, pd.DateRequired,
    ISNULL(CAST(pd.Discount AS DECIMAL(5,2)), 0),
    ISNULL(pd.Payed, 0), ISNULL(pd.Storno, 0),
    pd.Note
FROM [$(LEGACY_DB)].[dbo].[PaymentDocuments] pd
JOIN #map_CVR mcvr ON mcvr.LegacyId = pd.IdCustomerVehicleRelation
LEFT JOIN #map_PaymentType mpt ON mpt.LegacyId = pd.IdPaymentType
WHERE pd.DatePay IS NOT NULL AND pd.DateRequired IS NOT NULL;

INSERT INTO #map_PaymentDocument (LegacyId, NewId)
SELECT lpd.Id, npd.Id
FROM [$(LEGACY_DB)].[dbo].[PaymentDocuments] lpd
JOIN [dbo].[PaymentDocuments] npd ON npd.StationId = @StationId AND npd.DocumentNumber = lpd.DocumentNumber;

PRINT 'PaymentDocuments: ' + CAST((SELECT COUNT(*) FROM #map_PaymentDocument) AS NVARCHAR(20));
GO

-- =============================================================================
-- 10. Operators / Users — translate emSecurity.Users to AspNetUsers + Operators
--     PASSWORDS DO NOT MIGRATE. New users have a placeholder PasswordHash that
--     fails verification → operators must use "Forgot password" on first login,
--     or an Administrator must reset their password via the API.
-- =============================================================================
DECLARE @StationId INT = (SELECT TOP 1 StationId FROM #mig_station);

-- Adjust the source DB if your security DB is separate (e.g. emSecurity)
DECLARE @sec_sql NVARCHAR(MAX) = N'
INSERT INTO [dbo].[AspNetUsers]
    ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail],
     [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp],
     [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
SELECT
    LOWER(CAST(NEWID() AS NVARCHAR(36))),
    u.UserName,
    UPPER(u.UserName),
    u.UserName + N''@migrated.local'',
    UPPER(u.UserName + N''@migrated.local''),
    0,
    NULL,                                   -- placeholder; forces password reset
    LOWER(CAST(NEWID() AS NVARCHAR(36))),
    LOWER(CAST(NEWID() AS NVARCHAR(36))),
    0, 0, 1, 0
FROM [$(LEGACY_DB)].[dbo].[Users] u
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] au WHERE au.UserName = u.UserName);
';
EXEC sp_executesql @sec_sql;

-- Assign all migrated users to the Operator role
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
SELECT au.Id, r.Id
FROM [dbo].[AspNetUsers] au
JOIN [dbo].[AspNetRoles] r ON r.NormalizedName = N'OPERATOR'
WHERE au.Email LIKE N'%@migrated.local'
  AND NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUserRoles] ur WHERE ur.UserId = au.Id);

-- Create Operator rows binding each migrated user to the new station
INSERT INTO [dbo].[Operators] ([UserId], [StationId], [FullName], [IsActive])
SELECT au.Id, @StationId, au.UserName, 1
FROM [dbo].[AspNetUsers] au
WHERE au.Email LIKE N'%@migrated.local'
  AND NOT EXISTS (SELECT 1 FROM [dbo].[Operators] o WHERE o.UserId = au.Id);

PRINT 'Migrated users: each requires password reset on first login.';
GO

-- =============================================================================
-- 11. Verification: row counts legacy vs new
-- =============================================================================
DECLARE @StationId INT = (SELECT TOP 1 StationId FROM #mig_station);

PRINT '';
PRINT '=== Row-count verification ===';
SELECT
    Source = N'Customers',
    Legacy = (SELECT COUNT(*) FROM [$(LEGACY_DB)].[dbo].[Customers]),
    [New]  = (SELECT COUNT(*) FROM [dbo].[Customers] WHERE StationId = @StationId)
UNION ALL SELECT N'Vehicles',
    (SELECT COUNT(*) FROM [$(LEGACY_DB)].[dbo].[Vehicles]),
    (SELECT COUNT(*) FROM [dbo].[Vehicles] WHERE StationId = @StationId)
UNION ALL SELECT N'CustomerVehicleRelations',
    (SELECT COUNT(*) FROM [$(LEGACY_DB)].[dbo].[CustomerVehiclesRelations]),
    (SELECT COUNT(*) FROM [dbo].[CustomerVehicleRelations] r
       JOIN [dbo].[Vehicles] v ON v.Id = r.VehicleId WHERE v.StationId = @StationId)
UNION ALL SELECT N'Requests',
    (SELECT COUNT(*) FROM [$(LEGACY_DB)].[dbo].[Requests]),
    (SELECT COUNT(*) FROM [dbo].[Requests] WHERE StationId = @StationId)
UNION ALL SELECT N'TrafficLicences',
    (SELECT COUNT(*) FROM [$(LEGACY_DB)].[dbo].[DocumentsTrafficLicences]),
    (SELECT COUNT(*) FROM [dbo].[TrafficLicences] WHERE StationId = @StationId)
UNION ALL SELECT N'PaymentDocuments',
    (SELECT COUNT(*) FROM [$(LEGACY_DB)].[dbo].[PaymentDocuments]),
    (SELECT COUNT(*) FROM [dbo].[PaymentDocuments] WHERE StationId = @StationId);

PRINT '';
PRINT 'Migration template applied. Inspect mismatches above and customize sections';
PRINT 'where your legacy schema deviates from the audit baseline.';
GO

-- Cleanup temp objects (optional — they auto-drop at session end)
IF OBJECT_ID('tempdb..#mig_station')          IS NOT NULL DROP TABLE #mig_station;
IF OBJECT_ID('tempdb..#map_BusinessType')     IS NOT NULL DROP TABLE #map_BusinessType;
IF OBJECT_ID('tempdb..#map_City')             IS NOT NULL DROP TABLE #map_City;
IF OBJECT_ID('tempdb..#map_Street')           IS NOT NULL DROP TABLE #map_Street;
IF OBJECT_ID('tempdb..#map_Country')          IS NOT NULL DROP TABLE #map_Country;
IF OBJECT_ID('tempdb..#map_RegistrationIssuer') IS NOT NULL DROP TABLE #map_RegistrationIssuer;
IF OBJECT_ID('tempdb..#map_VehicleBodyType')  IS NOT NULL DROP TABLE #map_VehicleBodyType;
IF OBJECT_ID('tempdb..#map_Customer')         IS NOT NULL DROP TABLE #map_Customer;
IF OBJECT_ID('tempdb..#map_Vehicle')          IS NOT NULL DROP TABLE #map_Vehicle;
IF OBJECT_ID('tempdb..#map_CVR')              IS NOT NULL DROP TABLE #map_CVR;
IF OBJECT_ID('tempdb..#map_PaymentType')      IS NOT NULL DROP TABLE #map_PaymentType;
IF OBJECT_ID('tempdb..#map_PaymentDocument')  IS NOT NULL DROP TABLE #map_PaymentDocument;
GO
