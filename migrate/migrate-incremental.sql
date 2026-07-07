-- =============================================================================
-- INCREMENTAL top-up: pull NEW legacy records (Id > current target max) from the
-- LIVE remote DB (linked server [VTEZVV_LIVE] → 195.26.159.162,7899 / VTEZVV)
-- into VTE. Purely additive — never wipes or updates existing rows.
--
-- FK order: Client → ClientPersonalData → Vehicle → VehicleRegistration →
--           ClientVehicleRelation → Request → Ownership/Payment proofs.
--
-- Reuses the exact column mappings from migrate-clients/vehicles/requests.sql,
-- swapping the source to the linked server and adding `Id > @beforeMax` guards.
-- Vehicle insert also fills the later-added columns (MaxLegalGroupMassKg,
-- MaxTrailer*Kg, MaxHitchLoadKg) so new vehicles don't need a separate backfill.
--
-- Prereq: linked server VTEZVV_LIVE exists (sp_addlinkedserver, MSOLEDBSQL,
--         @provstr='TrustServerCertificate=yes'), and the API has been booted
--         at least once (admin user present).
--
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-incremental.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET QUOTED_IDENTIFIER ON;   -- required for distributed (linked server) queries + filtered indexes
SET ANSI_NULLS ON;
SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

IF NOT EXISTS (SELECT 1 FROM sys.servers WHERE name = 'VTEZVV_LIVE')
BEGIN RAISERROR('Linked server VTEZVV_LIVE missing. Create it first.', 16, 1); RETURN; END;

DECLARE @adminId nvarchar(450) = (SELECT TOP 1 Id FROM dbo.AspNetUsers WHERE UserName = 'admin');
IF @adminId IS NULL
BEGIN RAISERROR('Admin user not found. Boot the API once so DataSeeder creates it.', 16, 1); RETURN; END;

-- Capture the "before" high-water marks so each section inserts only new rows.
-- Since v2 went live it creates its OWN rows with identities reseeded to the 10M floor
-- (migrate/fix-v2-native-id-space.sql). Watermarks must ignore those — only ids BELOW
-- the floor mirror legacy 1:1.
DECLARE @v2floor bigint = 10000000;
DECLARE @bClient bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.Client                    WHERE Id < @v2floor);
DECLARE @bVeh    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.Vehicle                   WHERE Id < @v2floor);
DECLARE @bReg    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleRegistration       WHERE Id < @v2floor);
DECLARE @bRel    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.ClientVehicleRelation     WHERE Id < @v2floor);
DECLARE @bReq    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.Request                   WHERE Id < @v2floor);
DECLARE @bOwn    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestOwnershipProof     WHERE Id < @v2floor);
DECLARE @bPay    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestPaymentProof       WHERE Id < @v2floor);
DECLARE @bTeh    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.TechnicalExamReport       WHERE Id < @v2floor);
DECLARE @bTehD   bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.TechnicalExamReportDetail WHERE Id < @v2floor);
DECLARE @fallbackIssuer tinyint = (SELECT TOP 1 Id FROM dbo.DocumentIssuer ORDER BY Id);
DECLARE @rows int;
PRINT CONCAT('Before maxes — Client:', @bClient, ' Vehicle:', @bVeh, ' Reg:', @bReg,
             ' Rel:', @bRel, ' Request:', @bReq);

-- ============================================================================
-- 1. Client (Id > @bClient)
-- ============================================================================
PRINT '=== Client ===';
SET IDENTITY_INSERT dbo.Client ON;
-- Name order: legacy stores the SURNAME in CustomerFirstName for persons (see
-- migrate/fix-client-name-order.sql, applied 2026-07-01) — swap on the way in.
-- Companies keep the full firm name in FirstName. Citizenship: legacy never stored it
-- (IdCitizenship=0) — default to Македонско per the standing backfill policy
-- (migrate/backfill-client-citizenship.sql, dominant row = 77).
INSERT INTO dbo.Client (Id, CompanyId, CityId, CitizenshipId, Business,
  FirstName, MiddleName, ParentName, LastName, MB, Address,
  TaxNumber, PhoneNumber, Email, DateOfBirth, Note, Active, CreatedAt)
SELECT
  c.Id, CAST(4 AS tinyint), ci.Id,
  COALESCE(cz.Id, (SELECT TOP 1 z.Id FROM dbo.Citizenship z WHERE z.Name = N'Македонско' ORDER BY z.Id)),
  c.IsCompany,
  CASE WHEN c.IsCompany = 1 THEN NULLIF(LTRIM(RTRIM(c.CustomerFirstName)), N'')
       ELSE COALESCE(NULLIF(LTRIM(RTRIM(c.CustomerSurname)), N''), NULLIF(LTRIM(RTRIM(c.CustomerFirstName)), N'')) END,
  NULLIF(LTRIM(RTRIM(c.ParentName)),        N''),
  NULLIF(LTRIM(RTRIM(c.ParentName)),        N''),
  CASE WHEN c.IsCompany = 1 THEN NULLIF(LTRIM(RTRIM(c.CustomerSurname)), N'')
       ELSE CASE WHEN NULLIF(LTRIM(RTRIM(c.CustomerSurname)), N'') IS NULL THEN NULL
                 ELSE NULLIF(LTRIM(RTRIM(c.CustomerFirstName)), N'') END END,
  NULLIF(LTRIM(RTRIM(c.MB)),                N''),
  NULLIF(LTRIM(RTRIM(CONCAT_WS(N' ',
      NULLIF(LTRIM(RTRIM(s.StreetName)),          N''),
      NULLIF(LTRIM(RTRIM(c.LivingAddressNumber)), N''),
      NULLIF(LTRIM(RTRIM(ci.Name)),               N'')))), N''),
  NULLIF(LTRIM(RTRIM(c.TaxNumber)),   N''),
  NULLIF(LTRIM(RTRIM(c.PhoneNumber)), N''),
  NULLIF(LTRIM(RTRIM(c.eMail)),       N''),
  c.DateOfBirth, NULLIF(LTRIM(RTRIM(c.Note)), N''), c.Active, GETUTCDATE()
FROM VTEZVV_LIVE.VTEZVV.dbo.Customers c
LEFT JOIN VTEZVV_LIVE.VTEZVV.dbo.Streets s ON s.Id = NULLIF(c.IdLivingAddress, 0)
LEFT JOIN dbo.City        ci ON ci.Id        = NULLIF(c.IdLivingCity,  0)
LEFT JOIN dbo.Citizenship cz ON cz.CountryId = NULLIF(c.IdCitizenship, 0)
WHERE c.Id > @bClient;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.Client OFF;
PRINT CONCAT('  -> ', @rows, ' new Clients.');

-- 1b. ClientPersonalData for the new clients (3 types)
INSERT INTO dbo.ClientPersonalData (ClientId, PersonalDataTypeId, DocumentIssuerId, Number, CreatedAt, Active)
SELECT c.Id, CAST(3 AS tinyint), COALESCE(d.Id, @fallbackIssuer), LTRIM(RTRIM(c.BLK)), ISNULL(c.BLKDateIssued, GETUTCDATE()), c.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.Customers c
LEFT JOIN dbo.DocumentIssuer d ON d.Id = CASE WHEN c.BLKIssuer BETWEEN 1 AND 255 THEN CAST(c.BLKIssuer AS tinyint) END
WHERE c.Id > @bClient AND c.BLK IS NOT NULL AND LEN(LTRIM(RTRIM(c.BLK))) > 0;
INSERT INTO dbo.ClientPersonalData (ClientId, PersonalDataTypeId, DocumentIssuerId, Number, CreatedAt, Active)
SELECT c.Id, CAST(2 AS tinyint), COALESCE(d.Id, @fallbackIssuer), LTRIM(RTRIM(c.PassportNumber)), ISNULL(c.PassDateIssued, GETUTCDATE()), c.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.Customers c
LEFT JOIN dbo.DocumentIssuer d ON d.Id = CASE WHEN c.PassIssuer BETWEEN 1 AND 255 THEN CAST(c.PassIssuer AS tinyint) END
WHERE c.Id > @bClient AND c.PassportNumber IS NOT NULL AND LEN(LTRIM(RTRIM(c.PassportNumber))) > 0;
INSERT INTO dbo.ClientPersonalData (ClientId, PersonalDataTypeId, DocumentIssuerId, Number, CreatedAt, Active)
SELECT c.Id, CAST(1 AS tinyint), COALESCE(d.Id, @fallbackIssuer), LTRIM(RTRIM(c.DriveingLicenceNumber)), ISNULL(c.DriveingLicenceDateIssued, GETUTCDATE()), c.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.Customers c
LEFT JOIN dbo.DocumentIssuer d ON d.Id = CASE WHEN c.DriveingLicenceIssuer BETWEEN 1 AND 255 THEN CAST(c.DriveingLicenceIssuer AS tinyint) END
WHERE c.Id > @bClient AND c.DriveingLicenceNumber IS NOT NULL AND LEN(LTRIM(RTRIM(c.DriveingLicenceNumber))) > 0;

-- ============================================================================
-- 1c. Vehicle lookups (VehicleMaker → VehicleModel) — top up new lookup rows.
-- The base lookups came from a static snapshot; live keeps adding makers/models.
-- Use NOT EXISTS (by Id) so this catches both "added after snapshot" and rows
-- the original INNER-JOIN skipped because their maker wasn't present yet.
-- Must run BEFORE Vehicle so new vehicles can reference a freshly-added model.
-- ============================================================================
PRINT '=== VehicleMaker (new) ===';
SET IDENTITY_INSERT dbo.VehicleMaker ON;
INSERT INTO dbo.VehicleMaker (Id, CountryId, Name, Trademark, Active)
SELECT m.Id,
       CASE WHEN c.Id IS NOT NULL THEN CAST(m.IdCountry AS smallint) END,
       COALESCE(NULLIF(LTRIM(RTRIM(m.CompanyName)), N''), N'?'),
       NULLIF(LTRIM(RTRIM(m.CompanyTrademark)), N''),
       m.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.VehicleMakers m
LEFT JOIN dbo.Country c ON c.Id = CASE WHEN m.IdCountry BETWEEN 1 AND 32767 THEN CAST(m.IdCountry AS smallint) END
WHERE NOT EXISTS (SELECT 1 FROM dbo.VehicleMaker v WHERE v.Id = m.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleMaker OFF;
DBCC CHECKIDENT('dbo.VehicleMaker', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' new VehicleMakers.');

PRINT '=== VehicleModel (new) ===';
SET IDENTITY_INSERT dbo.VehicleModel ON;
INSERT INTO dbo.VehicleModel (Id, MakerId, Code, Name, ProductionStart, ProductionEnd, Active)
SELECT m.Id, m.IdVehicleMaker,
       NULLIF(LTRIM(RTRIM(m.ModelCode)), N''),
       COALESCE(NULLIF(LTRIM(RTRIM(m.ModelName)), N''), N'?'),
       m.YearOfBeginingProduction,
       m.YearOfEndingProduction,
       m.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.VehicleModel m
INNER JOIN dbo.VehicleMaker vm ON vm.Id = m.IdVehicleMaker
WHERE NOT EXISTS (SELECT 1 FROM dbo.VehicleModel v WHERE v.Id = m.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleModel OFF;
DBCC CHECKIDENT('dbo.VehicleModel', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' new VehicleModels.');

-- ============================================================================
-- 2. Vehicle (Id > @bVeh) — incl. later-added towing/group-mass columns
-- ============================================================================
PRINT '=== Vehicle ===';
SET IDENTITY_INSERT dbo.Vehicle ON;
INSERT INTO dbo.Vehicle (
  Id, CompanyId, Vin, EngineNumber, Plate, LastRegistrationValidUntil,
  CategoryId, BodyTypeId, ModelId, PrimaryColorId, SecondaryColorId, MadeCountryId,
  FuelId, SecondFuelId, EngineTypeId, EcoProgramId, PaymentCategoryId,
  EnginePowerKw, EngineWorkingCapacityCc, MaxRpm, MaxSpeedKmh, HasLpg, ManufactureDate,
  LengthMm, WidthMm, HeightMm,
  EmptyWeightKg, MaxAllowedWeightKg, MaxLegalTotalMassKg, MaxConstructiveTotalMassKg,
  MaxLegalGroupMassKg, TrailerMassWithBrakesKg, TrailerMassWithoutBrakesKg,
  MaxTrailerBrakedKg, MaxTrailerUnbrakedKg, MaxHitchLoadKg,
  AxleCount, WheelCount, AxleLoad1Kg, AxleLoad2Kg,
  Seats, StandingSeats, Co2GKm, NoiseStaticDb, NoiseMovingDb,
  TypeText, ModelVariant, ApprovalMark, Note, Active, CreatedAt)
SELECT
  v.Id, CAST(4 AS tinyint),
  COALESCE(NULLIF(LTRIM(RTRIM(v.ShellNumber)), N''), N''),
  NULLIF(LTRIM(RTRIM(v.EngineNumber)),          N''),
  NULLIF(LTRIM(RTRIM(v.LastRegistratinNumber)), N''),
  CASE WHEN v.LastRegistrationValidTill > '1900-01-01' AND v.LastRegistrationValidTill < '2100-01-01' THEN v.LastRegistrationValidTill END,
  cat.Id, bt.Id, mdl.Id, col1.Id, col2.Id, co.Id,
  f1.Id, f2.Id, et.Id, eco.Id, pc.Id,
  NULLIF(v.EnginePowerOutPut, 0), NULLIF(v.EngineWorkingCapacity, 0), NULLIF(v.BrojNaVrtezi, 0), NULLIF(v.MaxSpeed, 0), v.TNG, v.MakeDate,
  NULLIF(v.VehicleSizeLength, 0), NULLIF(v.VehicleSizeWidth, 0), NULLIF(v.VehicleSizeHight, 0),
  NULLIF(v.EmptyWaight, 0), NULLIF(v.MaximunAllowedWaight, 0), NULLIF(v.MaxLegVkMasa, 0), NULLIF(v.MaxKonstVkMasa, 0),
  NULLIF(v.MaxLegVkMasaGrupa, 0),
  NULLIF(LTRIM(RTRIM(v.TrailerWaightWithBreak)), N''), NULLIF(LTRIM(RTRIM(v.TrailerWaightWithoutBreak)), N''),
  NULLIF(v.MaxKonstVkMasaKocnaPrikolka, 0), NULLIF(v.MaxKonstVkMasaNeKocnaPrikolka, 0), NULLIF(v.MaxKonstOptovaruvanjeVoPriklucok, 0),
  NULLIF(v.NumberOfAxis, 0), NULLIF(v.NumberOfWheels, 0), NULLIF(v.OsnoOptovaruvanje1, 0), NULLIF(v.OsnoOptovaruvanje2, 0),
  NULLIF(v.NumberOfSeats, 0), NULLIF(v.NumberOfStandingSeats, 0), NULLIF(v.CO2, 0), NULLIF(v.NoiseStatic, 0), NULLIF(v.NoiseMovment, 0),
  NULLIF(LTRIM(RTRIM(v.Tip)), N''), NULLIF(LTRIM(RTRIM(v.VehicleModelAdding)), N''), NULLIF(LTRIM(RTRIM(v.OznakaNaOdobrenie)), N''),
  NULLIF(LTRIM(RTRIM(v.Note)), N''), v.Active, GETUTCDATE()
FROM VTEZVV_LIVE.VTEZVV.dbo.Vehicles v
LEFT JOIN dbo.VehicleCategory   cat  ON cat.Id  = CAST(NULLIF(v.IdVehicleCategories, 0) AS smallint)
LEFT JOIN dbo.VehicleBodyType   bt   ON bt.Id   = NULLIF(v.IdVehicleBodyType, 0)
LEFT JOIN dbo.VehicleModel      mdl  ON mdl.Id  = NULLIF(v.IdVehicleModel, 0)
LEFT JOIN dbo.VehicleColor      col1 ON col1.Id = CAST(NULLIF(v.IdPrimaryColor, 0) AS smallint)
LEFT JOIN dbo.VehicleColor      col2 ON col2.Id = CAST(NULLIF(v.IdSecondaryColor, 0) AS smallint)
LEFT JOIN dbo.Country           co   ON co.Id   = CAST(NULLIF(v.IdMadeCountry, 0) AS smallint)
LEFT JOIN dbo.VehicleFuel       f1   ON f1.Id   = CASE WHEN v.IdEnginePowerSource BETWEEN 1 AND 255 THEN CAST(v.IdEnginePowerSource AS tinyint) END
LEFT JOIN dbo.VehicleFuel       f2   ON f2.Id   = CASE WHEN v.IdEngineSecondPowerSource BETWEEN 1 AND 255 THEN CAST(v.IdEngineSecondPowerSource AS tinyint) END
LEFT JOIN dbo.VehicleEngineType et   ON et.Id   = NULLIF(v.IdEngineType, 0)
LEFT JOIN dbo.VehicleEcoProgram eco  ON eco.Id  = CASE WHEN v.IdEngineEcoProgram BETWEEN 1 AND 255 THEN CAST(v.IdEngineEcoProgram AS tinyint) END
LEFT JOIN dbo.VehiclePaymentCategory pc ON pc.Id = CASE WHEN v.IdVehicleCategoryForPayments BETWEEN 1 AND 255 THEN CAST(v.IdVehicleCategoryForPayments AS tinyint) END
WHERE v.Id > @bVeh;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.Vehicle OFF;
-- (no RESEED: identity floor 10,000,000 for v2-native rows — see fix-v2-native-id-space.sql)
PRINT CONCAT('  -> ', @rows, ' new Vehicles.');

-- ============================================================================
-- 3. VehicleRegistration (gap-fill, Id < 10M)
-- ============================================================================
PRINT '=== VehicleRegistration ===';
-- Gap-fill (NOT EXISTS), not a watermark: the bulk migration + earlier filters left
-- holes below the max id which a watermark can never revisit.
-- Issuer resolves to NULL when legacy has none (IdRegistrationIssuer=0) — those rows
-- carry real plates (51 were the vehicle's LATEST registration) and must not be dropped.
SET IDENTITY_INSERT dbo.VehicleRegistration ON;
INSERT INTO dbo.VehicleRegistration (Id, VehicleId, IssuerId, PlateNumber, RegisteredDate, ValidUntil, IsFirstRegistration, Active)
SELECT r.Id, r.IdVehicle, di.Id, LTRIM(RTRIM(r.RegistrationNumber)),
       r.DateOfRegistration, r.DateRegistrationValidTill, r.IsFirstRegistration, r.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.[Vehicle.Registrations] r
INNER JOIN dbo.Vehicle        v  ON v.Id  = r.IdVehicle
LEFT  JOIN dbo.DocumentIssuer di ON di.Id = CASE WHEN r.IdRegistrationIssuer BETWEEN 1 AND 255 THEN CAST(r.IdRegistrationIssuer AS tinyint) END
WHERE r.Id < 10000000 AND r.RegistrationNumber IS NOT NULL AND LEN(LTRIM(RTRIM(r.RegistrationNumber))) > 0
  AND NOT EXISTS (SELECT 1 FROM dbo.VehicleRegistration x WHERE x.Id = r.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleRegistration OFF;
-- (no RESEED: identity floor 10,000,000 for v2-native rows — see fix-v2-native-id-space.sql)
PRINT CONCAT('  -> ', @rows, ' new VehicleRegistrations.');

-- ============================================================================
-- 4. ClientVehicleRelation (gap-fill, Id < 10M)
-- ============================================================================
PRINT '=== ClientVehicleRelation ===';
-- Gap-fill (NOT EXISTS), not a watermark: the bulk migration's stricter filters left
-- ~7.7k holes below the max id (vehicle=0 sentinels, inactive relations…) which the
-- old `Id > @bRel` could never revisit — open legacy debts anchored on those relations
-- were silently unimportable. Vehicle resolves to NULL when 0/missing; relations whose
-- CLIENT is a legacy orphan stay excluded (INNER JOIN).
SET IDENTITY_INSERT dbo.ClientVehicleRelation ON;
INSERT INTO dbo.ClientVehicleRelation (Id, ClientId, VehicleId, RelationTypeId, StartDate, EndDate, StartNote, EndNote, Active)
SELECT r.Id, r.IdCustomer,
  CASE WHEN v.Id IS NOT NULL THEN r.IdVehicle END,
  CAST(r.IdRelationType AS tinyint), r.StartDate, r.EndDate,
  NULLIF(LTRIM(RTRIM(r.BeginNote)), N''), NULLIF(LTRIM(RTRIM(r.TerminationNote)), N''), r.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.CustomerVehiclesRelations r
INNER JOIN dbo.Client                     c  ON c.Id  = r.IdCustomer
INNER JOIN dbo.ClientVehicleRelationType rt ON rt.Id = CAST(r.IdRelationType AS tinyint)
LEFT  JOIN dbo.Vehicle                    v  ON v.Id  = NULLIF(r.IdVehicle, 0)
WHERE r.Id < 10000000
  AND NOT EXISTS (SELECT 1 FROM dbo.ClientVehicleRelation x WHERE x.Id = r.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.ClientVehicleRelation OFF;
-- (no RESEED: identity floor 10,000,000 for v2-native rows — see fix-v2-native-id-space.sql)
PRINT CONCAT('  -> ', @rows, ' new ClientVehicleRelations.');

-- ============================================================================
-- 5. Request (Id > @bReq)
-- ============================================================================
PRINT '=== Request ===';
SET IDENTITY_INSERT dbo.Request ON;
INSERT INTO dbo.Request (Id, CompanyId, RequestTypeId, ClientVehicleRelationId, NewClientVehicleRelationId,
  TechnicalExamReportId, PreviousRegistrationId, CreatedAt, ModifiedAt, EndedAt,
  CreatedByUserId, ModifiedByUserId, EndedByUserId, VehicleDataChanged, ClientDataChanged, Note, Active)
SELECT r.Id, CAST(4 AS tinyint), CAST(r.IdRequestType AS tinyint), r.IdCustomerVehicleRelation,
  CASE WHEN r.IdCustomerVehicleRelationNew IS NULL OR r.IdCustomerVehicleRelationNew = 0 THEN NULL
       WHEN EXISTS (SELECT 1 FROM dbo.ClientVehicleRelation x WHERE x.Id = r.IdCustomerVehicleRelationNew) THEN r.IdCustomerVehicleRelationNew
       ELSE NULL END,
  NULL,
  CASE WHEN r.IdPreviousRegistration > 0 AND EXISTS (SELECT 1 FROM dbo.VehicleRegistration vr WHERE vr.Id = r.IdPreviousRegistration)
       THEN CAST(r.IdPreviousRegistration AS bigint) ELSE NULL END,
  r.DateCreated, r.DateModified, r.DateEnded,
  @adminId,
  CASE WHEN r.DateModified IS NOT NULL THEN @adminId ELSE NULL END,
  CASE WHEN r.DateEnded    IS NOT NULL THEN @adminId ELSE NULL END,
  r.IsVehicleChanged, r.IsCustomerChanged,
  LEFT(CONCAT(NULLIF(LTRIM(RTRIM(r.Note)), N''), N' [legacy operator ids C=', r.IdOperatorCreated,
       N' M=', ISNULL(CAST(r.IdOperatorModified AS nvarchar(10)), N'-'), N' E=', r.IdOperatorEnded, N']'), 500),
  r.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.Requests r
INNER JOIN dbo.ClientVehicleRelation cvr ON cvr.Id = r.IdCustomerVehicleRelation
INNER JOIN dbo.RequestType rt ON rt.Id = CASE WHEN r.IdRequestType BETWEEN 1 AND 255 THEN CAST(r.IdRequestType AS tinyint) END
WHERE r.Id > @bReq;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.Request OFF;
-- (no RESEED: identity floor 10,000,000 for v2-native rows — see fix-v2-native-id-space.sql)
PRINT CONCAT('  -> ', @rows, ' new Requests.');

-- ============================================================================
-- 6. RequestOwnershipProof + 7. RequestPaymentProof (Id > before)
-- ============================================================================
PRINT '=== Request proofs ===';
SET IDENTITY_INSERT dbo.RequestOwnershipProof ON;
INSERT INTO dbo.RequestOwnershipProof (Id, RequestId, OwnershipProofTypeId, Detail, Active)
SELECT p.Id, p.IdRequest, CAST(p.IdVehicleOwnershipProof AS tinyint), NULLIF(LTRIM(RTRIM(p.VehicleOwnershipProof)), N''), p.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.[Request.VehicleOwnershipProofs] p
INNER JOIN dbo.Request r ON r.Id = p.IdRequest
INNER JOIN dbo.RequestOwnershipProofType ot ON ot.Id = CASE WHEN p.IdVehicleOwnershipProof BETWEEN 1 AND 255 THEN CAST(p.IdVehicleOwnershipProof AS tinyint) END
WHERE p.Id > @bOwn;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.RequestOwnershipProof OFF;
-- (no RESEED: identity floor 10,000,000 for v2-native rows — see fix-v2-native-id-space.sql)
PRINT CONCAT('  -> ', @rows, ' new OwnershipProofs.');

SET IDENTITY_INSERT dbo.RequestPaymentProof ON;
INSERT INTO dbo.RequestPaymentProof (Id, RequestId, PaymentProofTypeId, Detail, Active)
SELECT p.Id, p.IdRequest, CAST(p.IdPaymentProof AS tinyint), NULLIF(LTRIM(RTRIM(p.PaymentProof)), N''), p.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.[Request.PaymentProof] p
INNER JOIN dbo.Request r ON r.Id = p.IdRequest
INNER JOIN dbo.RequestPaymentProofType pt ON pt.Id = CASE WHEN p.IdPaymentProof BETWEEN 1 AND 255 THEN CAST(p.IdPaymentProof AS tinyint) END
WHERE p.Id > @bPay;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.RequestPaymentProof OFF;
-- (no RESEED: identity floor 10,000,000 for v2-native rows — see fix-v2-native-id-space.sql)
PRINT CONCAT('  -> ', @rows, ' new PaymentProofs.');

-- ============================================================================
-- 8. Technical Exam — lookups top-up (NOT EXISTS) + new reports/details (Id > max).
--    Lookups first so new reports can reference a freshly-added type/org/part.
-- ============================================================================
PRINT '=== TechnicalExam lookups (new) ===';
SET IDENTITY_INSERT dbo.TechnicalExamType ON;
INSERT INTO dbo.TechnicalExamType (Id, Code, Description, ValidDays, PercentOfFullExam, IsInRegister, Active)
SELECT t.Id, NULLIF(LTRIM(RTRIM(t.Code)), N''), COALESCE(NULLIF(LTRIM(RTRIM(t.Description)), N''), N'?'),
       t.ValidNumOfDays, t.PercentOfFullExam, t.IsInRegistar, t.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.TehnicalExamsTypes t
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamType x WHERE x.Id = t.Id);
SET IDENTITY_INSERT dbo.TechnicalExamType OFF;
DBCC CHECKIDENT('dbo.TechnicalExamType', RESEED) WITH NO_INFOMSGS;

SET IDENTITY_INSERT dbo.TechnicalExamDetailStatus ON;
INSERT INTO dbo.TechnicalExamDetailStatus (Id, Name, Active)
SELECT s.Id, COALESCE(NULLIF(LTRIM(RTRIM(s.StatusName)), N''), N'?'), s.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.DocumentsTehnicalExamsReportsDetailsStatus s
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamDetailStatus x WHERE x.Id = s.Id);
SET IDENTITY_INSERT dbo.TechnicalExamDetailStatus OFF;
DBCC CHECKIDENT('dbo.TechnicalExamDetailStatus', RESEED) WITH NO_INFOMSGS;

SET IDENTITY_INSERT dbo.TechnicalExamVehiclePart ON;
INSERT INTO dbo.TechnicalExamVehiclePart (Id, CategoryId, Code, Description, Active)
SELECT p.Id, p.IdCategoryVehicleParts, COALESCE(NULLIF(LTRIM(RTRIM(p.Code)), N''), N'?'),
       COALESCE(NULLIF(LTRIM(RTRIM(p.Description)), N''), N'?'), p.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.TehnicalExamVehicleParts p
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamVehiclePart x WHERE x.Id = p.Id);
SET IDENTITY_INSERT dbo.TechnicalExamVehiclePart OFF;
DBCC CHECKIDENT('dbo.TechnicalExamVehiclePart', RESEED) WITH NO_INFOMSGS;

SET IDENTITY_INSERT dbo.TechnicalExamOrganization ON;
INSERT INTO dbo.TechnicalExamOrganization
  (Id, CompanyId, Code, Name, CityId, CommunityId, Address, Phone, Fax,
   BankAccount, Depositor, TaxNumber, ResponsibleOfficer, Secretary, Active)
SELECT o.Id, CAST(NULLIF(o.IdCompany, 0) AS tinyint),
       NULLIF(LTRIM(RTRIM(o.Code)), N''), NULLIF(LTRIM(RTRIM(o.Station)), N''),
       NULLIF(o.IdCity, 0), NULLIF(o.IdCommunity, 0),
       NULLIF(LTRIM(RTRIM(o.StationAddress)), N''), NULLIF(LTRIM(RTRIM(o.Tel)), N''), NULLIF(LTRIM(RTRIM(o.Fax)), N''),
       NULLIF(LTRIM(RTRIM(o.ZiroSmetka)), N''), NULLIF(LTRIM(RTRIM(o.Deponent)), N''), NULLIF(LTRIM(RTRIM(o.EDB)), N''),
       NULLIF(LTRIM(RTRIM(o.OdgovorenOrgan)), N''), NULLIF(LTRIM(RTRIM(o.Sekretar)), N''), o.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.TehnicalExamOrganizations o
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamOrganization x WHERE x.Id = o.Id);
SET IDENTITY_INSERT dbo.TechnicalExamOrganization OFF;
DBCC CHECKIDENT('dbo.TechnicalExamOrganization', RESEED) WITH NO_INFOMSGS;

PRINT '=== TechnicalExamReport ===';
SET IDENTITY_INSERT dbo.TechnicalExamReport ON;
INSERT INTO dbo.TechnicalExamReport (
  Id, CompanyId, CustomerVehicleRelationId, TechnicalExamTypeId, OrganizationId,
  RegNumber, MadeDate, ValidTillDate, FirstControllerLegacyId, SecondControllerLegacyId,
  VehicleIsRight, ExplanationNote, DriversWarning, Note, TechnicalChanges,
  Axis1Left, Axis1Right, Axis1Gj, Axis1LeftRightDiff, Axis1Coefficient,
  Axis2Left, Axis2Right, Axis2Gj, Axis2LeftRightDiff, Axis2Coefficient,
  Axis3Left, Axis3Right, Axis3Gj, Axis3LeftRightDiff, Axis3Coefficient,
  Axis4Left, Axis4Right, Axis4Gj, Axis4LeftRightDiff, Axis4Coefficient,
  AxisParkingLeft, AxisParkingRight, AxisParkingGj, AxisParkingLeftRightDiff, AxisParkingCoefficient,
  Weight, EffectOfWorkingBrakeEmpty, EffectOfWorkingBrakeFull, EffectOfSecondaryBrake, EffectOfParkingBrake,
  SpeedOfTurns, CO, EngineRpm, COPlusTurns, Lambda, Pinpoints, Noise, EngineOilTemp,
  Active, CreatedAt, ModifiedAt)
SELECT
  r.Id, CAST(4 AS tinyint), cvr.Id,
  r.IdTypeOfTehnicalExam, r.IdOrganizationForTehnicalExam,
  NULLIF(LTRIM(RTRIM(r.RegNumber)), N''),
  CAST(r.MadeDate AS date), CAST(r.ValidTillDate AS date),
  NULLIF(r.IdFirsControler, 0), NULLIF(r.IdSecondControler, 0),
  r.VehicleIsRight,
  NULLIF(LTRIM(RTRIM(r.ExplanationNote)), N''), NULLIF(LTRIM(RTRIM(r.DriversWarning)), N''),
  NULLIF(LTRIM(RTRIM(r.Note)), N''), NULLIF(LTRIM(RTRIM(r.TechnicalChanges)), N''),
  NULLIF(r.Axis1Left,0), NULLIF(r.Axis1Right,0), NULLIF(r.Axis1Gj,0), NULLIF(r.Axis1LeftPj,0), NULLIF(r.Axis1PN,0),
  NULLIF(r.Axis2Left,0), NULLIF(r.Axis2Right,0), NULLIF(r.Axis2Gj,0), NULLIF(r.Axis2LeftPj,0), NULLIF(r.Axis2PN,0),
  NULLIF(r.Axis3Left,0), NULLIF(r.Axis3Right,0), NULLIF(r.Axis3Gj,0), NULLIF(r.Axis3LeftPj,0), NULLIF(r.Axis3PN,0),
  NULLIF(r.Axis4Left,0), NULLIF(r.Axis4Right,0), NULLIF(r.Axis4Gj,0), NULLIF(r.Axis4LeftPj,0), NULLIF(r.Axis4PN,0),
  NULLIF(r.AxisParkingLeft,0), NULLIF(r.AxisParkingRight,0), NULLIF(r.AxisParkingGj,0), NULLIF(r.AxisParkingLeftPj,0), NULLIF(r.AxisParkingPN,0),
  NULLIF(r.Waight,0), NULLIF(r.EffectOfWorkingBreakEmpty,0), NULLIF(r.EffectOfWorkingBreakFull,0), NULLIF(r.EffectOfSecondaryBreak,0), NULLIF(r.EffectOfParkingBreak,0),
  NULLIF(r.SpeedOfTurns,0), NULLIF(r.CO,0), NULLIF(r.NumEngineTurns,0), NULLIF(r.COPlusTurns,0), NULLIF(r.Lambda,0), NULLIF(r.Pinpoints,0), NULLIF(r.Noise,0), NULLIF(r.TempOfEngineOil,0),
  r.Active, CAST(r.MadeDate AS datetime2), NULL
FROM VTEZVV_LIVE.VTEZVV.dbo.DocumentsTehnicalExamsReports r
LEFT JOIN dbo.ClientVehicleRelation cvr ON cvr.Id = r.IdCustomerVehicleRelation
WHERE r.Id > @bTeh;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.TechnicalExamReport OFF;
-- (no RESEED: identity floor 10,000,000 for v2-native rows — see fix-v2-native-id-space.sql)
PRINT CONCAT('  -> ', @rows, ' new TechnicalExamReports.');

PRINT '=== TechnicalExamReportDetail ===';
SET IDENTITY_INSERT dbo.TechnicalExamReportDetail ON;
INSERT INTO dbo.TechnicalExamReportDetail
  (Id, TechnicalExamReportId, VehiclePartId, StatusId, Front, Back, OnLeft, OnRight, EnteredAt, Note, Active)
SELECT d.Id, d.IdTehnicalExamsReports, d.IdTehnicalExamVehivlePart, d.IdStatus,
       d.Front, d.Back, d.OnLeft, d.OnRight, CAST(d.DateEnter AS datetime2),
       NULLIF(LTRIM(RTRIM(d.Note)), N''), d.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.DocumentsTehnicalExamsReportsDetails d
INNER JOIN dbo.TechnicalExamReport p ON p.Id = d.IdTehnicalExamsReports
WHERE d.Id > @bTehD;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.TechnicalExamReportDetail OFF;
-- (no RESEED: identity floor 10,000,000 for v2-native rows — see fix-v2-native-id-space.sql)
PRINT CONCAT('  -> ', @rows, ' new TechnicalExamReportDetails.');

-- ============================================================================
-- 9. Reference numbers for the new tech-exam requests (restore exam id + build
--    the legacy "1 + ExamId + Operator + / + Year" reference, same as the Zelen backfill).
-- ============================================================================
PRINT '=== Reference numbers ===';
UPDATE r SET r.TechnicalExamReportId = src.IdTechnicalExamReport
FROM dbo.Request r INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.Requests src ON src.Id = r.Id
WHERE r.Id > @bReq AND r.TechnicalExamReportId IS NULL AND src.IdTechnicalExamReport > 0;
UPDATE r SET r.LegacyReferenceNumber = CONCAT(N'1', CAST(src.IdTechnicalExamReport AS nvarchar(20)),
            CAST(src.IdOperatorCreated AS nvarchar(10)), N'/', CAST(YEAR(src.DateCreated) AS nvarchar(4)))
FROM dbo.Request r INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.Requests src ON src.Id = r.Id
WHERE r.Id > @bReq AND r.LegacyReferenceNumber IS NULL AND src.IdTechnicalExamReport > 0;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' reference numbers computed.');

-- ============================================================================
-- 10. CustomerDebt ← legacy CustomerFinancialState (the Наплата panel).
--     Unlike the sections above this one is a state sync, not purely additive:
--       a) INSERT every OPEN legacy debt (Payed=0, Active=1) not yet imported
--          (idempotent via CustomerDebt.LegacyId, unique-filtered).
--          Historical PAID rows are NOT imported — that history already lives
--          in PaymentDocument*; Наплата only shows open items.
--       b) UPDATE Paid/Active on previously-imported rows so debts settled or
--          stornoed in legacy close here too. v2-native debts (LegacyId NULL)
--          are never touched.
--     Origin mapping (DebtOrigin enum): IdDocumentTehnicalExam→2 TechnicalExam,
--     IdDocument→1 Request, TrafficLicences→4, Permisions→5, IDL→6, else 0.
-- ============================================================================
PRINT '=== CustomerDebt (Наплата) ===';
DECLARE @bDebt bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.CustomerDebt);

INSERT INTO dbo.CustomerDebt
  (CompanyId, CustomerVehicleRelationId, PriceCatalogId, Price, VatPercent, Note,
   Origin, OriginRequestId, OriginTechnicalExamId, OrganizationId,
   Paid, SettledByLineId, CreatedAt, CreatedByUserId, Active, LegacyId)
SELECT 4, s.IdCustomerVehicleRelation, s.IdPriceCatalog, s.Price, 0,
       LEFT(NULLIF(LTRIM(RTRIM(s.Note)), N''), 300),
       CASE WHEN s.IdDocumentTehnicalExam > 0 THEN 2
            WHEN s.IdDocument > 0 THEN 1
            WHEN s.IdDocumentsTrafficLicences > 0 THEN 4
            WHEN s.IdDocumentPermisions > 0 THEN 5
            WHEN s.IdDocumentIternationalDriveingLicence > 0 THEN 6
            ELSE 0 END,
       NULLIF(s.IdDocument, 0), NULLIF(s.IdDocumentTehnicalExam, 0),
       s.IdOrganization, 0, NULL, SYSUTCDATETIME(), NULL, 1, s.Id
FROM VTEZVV_LIVE.VTEZVV.dbo.CustomerFinancialState s
INNER JOIN dbo.ClientVehicleRelation rel ON rel.Id = s.IdCustomerVehicleRelation
INNER JOIN dbo.PriceCatalog pc ON pc.Id = s.IdPriceCatalog
WHERE s.Payed = 0 AND s.Active = 1
  -- Mirror legacy VISIBILITY, not raw table state: the legacy dashboard reads
  -- depCustomerFinansicalStateView which INNER JOINs Vehicles (+Model+Maker), so a
  -- debt on a vehicle-less relation NEVER shows in legacy — it is dead debris there
  -- (371 such rows, some since 2012). Importing them floods Наплата with items the
  -- station has never seen. v2-native debts (LegacyId NULL) are unaffected.
  AND rel.VehicleId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.CustomerDebt d WHERE d.LegacyId = s.Id);
PRINT CONCAT('  -> ', @@ROWCOUNT, ' new open debts imported.');

-- legacy rows that we hold open but legacy has since paid/stornoed.
-- SettledByLineId guard: a debt settled NATIVELY in v2 (billed here) must never be
-- re-opened just because legacy still shows it unpaid — v2 is authoritative for
-- its own payments.
UPDATE d SET d.Paid = s.Payed, d.Active = s.Active
FROM dbo.CustomerDebt d
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.CustomerFinancialState s ON s.Id = d.LegacyId
WHERE d.LegacyId IS NOT NULL
  AND d.SettledByLineId IS NULL
  AND (d.Paid <> s.Payed OR d.Active <> s.Active);
PRINT CONCAT('  -> ', @@ROWCOUNT, ' imported debts state-synced (paid/storno).');

-- ============================================================================
-- 11. PaymentDocument / PaymentDocumentLine / InstallmentAgreement /
--     InstallmentSchedule ← the billing mirror (needed by the monthly
--     „Јавни патишта" report and Плаќања).
--
--     Cheap per-sync strategy (these tables are too big for full re-scans):
--       a) APPEND new rows via Id watermark (id-floor 10M keeps v2-native rows
--          out of the legacy range — see fix-payment-id-space.sql);
--       b) STATE-SYNC a 90-day window: doc Active/Note/DatePay/Payed/Storno
--          (storno + payment normally happen near creation) and rata payments
--          by recent DatePayed regardless of age.
--     Holes below the watermark are repaired by the manual
--     backfill-payment-holes.sql (bulk-era exclusions), not here.
-- ============================================================================
PRINT '=== InstallmentAgreement (append) ===';
DECLARE @bAgr bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.InstallmentAgreement WHERE Id < @v2floor);
SET IDENTITY_INSERT dbo.InstallmentAgreement ON;
INSERT INTO dbo.InstallmentAgreement (Id, CompanyId, Number, [Date], TotalInstallments,
                                      GuarantorName, GuarantorAddress, GuarantorEmbg,
                                      Active, CreatedAt, ModifiedAt)
SELECT i.Id, CONVERT(tinyint, 4), LEFT(ISNULL(i.Broj, N''), 40), CONVERT(date, i.Datum),
       ISNULL(i.BrNaRati, 0), LEFT(i.GarantNaziv, 200), LEFT(i.GarantAdresa, 300),
       LEFT(i.GartEMB, 13), ISNULL(i.Active, CONVERT(bit, 1)), GETUTCDATE(), NULL
FROM VTEZVV_LIVE.VTEZVV.dbo.DogovorZaRati i
WHERE i.Id > @bAgr AND i.Id < @v2floor;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.InstallmentAgreement OFF;
PRINT CONCAT('  -> ', @rows, ' new agreements.');

PRINT '=== PaymentDocument (append) ===';
DECLARE @bDoc bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.PaymentDocument WHERE Id < @v2floor);
SELECT p.Id, p.IdPaymentType, p.IdCustomerVehicleRelation, p.IdOperator, p.IdOrganization,
       p.DocumentNumber, p.DatePay, p.DateRequired, p.Discount, p.Payed, p.Storno,
       p.Note, p.IdDogovor, p.IdFakturiraNa, p.Active
INTO #newDoc
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocuments p
WHERE p.Id > @bDoc AND p.Id < @v2floor;

SET IDENTITY_INSERT dbo.PaymentDocument ON;
INSERT INTO dbo.PaymentDocument
    (Id, CompanyId, PaymentTypeId, CustomerVehicleRelationId, OperatorLegacyId, OrganizationId,
     DocumentNumber, IssueDate, DueDate, Discount, Paid, Stornoed, StornoReason, Note,
     AgreementId, InvoicedToCompanyId, FiscalPrintedAt,
     LegacyId, Active, CreatedAt, ModifiedAt, CreatedByUserId, ModifiedByUserId)
SELECT
    p.Id, CONVERT(tinyint, 4), p.IdPaymentType, p.IdCustomerVehicleRelation,
    NULLIF(p.IdOperator, 0), p.IdOrganization,
    COALESCE(p.DocumentNumber, ''), p.DatePay, p.DateRequired, p.Discount,
    CONVERT(bit, COALESCE(p.Payed, 0)), CONVERT(bit, COALESCE(p.Storno, 0)), NULL, p.Note,
    CASE WHEN EXISTS (SELECT 1 FROM dbo.InstallmentAgreement ia WHERE ia.Id = p.IdDogovor)
         THEN p.IdDogovor ELSE NULL END,
    NULLIF(p.IdFakturiraNa, 0), NULL,
    p.Id, CONVERT(bit, COALESCE(p.Active, 1)), COALESCE(p.DatePay, GETUTCDATE()), NULL, NULL, NULL
FROM #newDoc p
INNER JOIN dbo.ClientVehicleRelation r ON r.Id = p.IdCustomerVehicleRelation
INNER JOIN dbo.PaymentType          pt ON pt.Id = p.IdPaymentType
WHERE NOT EXISTS (SELECT 1 FROM dbo.PaymentDocument x WHERE x.Id = p.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.PaymentDocument OFF;
PRINT CONCAT('  -> ', @rows, ' new payment documents.');
DROP TABLE #newDoc;

PRINT '=== PaymentDocument (state-sync, 90d window) ===';
SELECT p.Id, p.DatePay, p.Payed, p.Storno, p.Note, p.Active
INTO #docState
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocuments p
WHERE p.Id < @v2floor AND p.DatePay >= DATEADD(day, -90, GETDATE());

UPDATE d SET d.IssueDate = s.DatePay, d.Paid = s.Payed, d.Stornoed = s.Storno,
             d.Note = s.Note, d.Active = s.Active, d.ModifiedAt = SYSUTCDATETIME()
FROM dbo.PaymentDocument d
INNER JOIN #docState s ON s.Id = d.Id
WHERE d.LegacyId IS NOT NULL
  AND (d.Paid <> s.Payed OR d.Stornoed <> s.Storno OR d.Active <> s.Active
       OR ISNULL(d.Note, N'') <> ISNULL(s.Note, N'') OR d.IssueDate <> s.DatePay);
PRINT CONCAT('  -> ', @@ROWCOUNT, ' documents state-synced.');
DROP TABLE #docState;

PRINT '=== PaymentDocumentLine (append) ===';
DECLARE @bLine bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.PaymentDocumentLine WHERE Id < @v2floor);
SELECT l.Id, l.IdPaymentDocuments, l.IdPriceCatalog, l.Price, l.DDV, l.Discount,
       l.Note, l.PrePayed, l.NotePrePayed, l.Active
INTO #newLine
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsDetails l
WHERE l.Id > @bLine AND l.Id < @v2floor;

SET IDENTITY_INSERT dbo.PaymentDocumentLine ON;
INSERT INTO dbo.PaymentDocumentLine
    (Id, CompanyId, PaymentDocumentId, PriceCatalogId, UnitPrice, VatPercent,
     Discount, Quantity, Note, PrePaid, PrePaidNote, CustomerDebtId, Active)
SELECT
    c.Id, CONVERT(tinyint, 4), c.IdPaymentDocuments,
    CASE WHEN EXISTS (SELECT 1 FROM dbo.PriceCatalog pc WHERE pc.Id = c.IdPriceCatalog)
         THEN c.IdPriceCatalog ELSE 999999 END,
    CONVERT(decimal(18,4), c.Price), CONVERT(float, c.DDV),
    CONVERT(float, COALESCE(c.Discount, 0)), 1,
    LEFT(NULLIF(LTRIM(RTRIM(c.Note)), N''), 300),
    CONVERT(bit, COALESCE(c.PrePayed, 0)), c.NotePrePayed, NULL,
    CONVERT(bit, COALESCE(c.Active, 1))
FROM #newLine c
INNER JOIN dbo.PaymentDocument pd ON pd.Id = c.IdPaymentDocuments
WHERE NOT EXISTS (SELECT 1 FROM dbo.PaymentDocumentLine x WHERE x.Id = c.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.PaymentDocumentLine OFF;
PRINT CONCAT('  -> ', @rows, ' new payment lines.');
DROP TABLE #newLine;

PRINT '=== InstallmentSchedule (append + recent payments) ===';
DECLARE @bSched bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.InstallmentSchedule WHERE Id < @v2floor);
SELECT r.Id, r.IdPaymentDocument, r.Price, r.Payed, r.DatePayed,
       r.IdOrganization, r.IdOperator, r.Note, r.Active
INTO #newSched
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsRata r
WHERE r.Id > @bSched AND r.Id < @v2floor;

SET IDENTITY_INSERT dbo.InstallmentSchedule ON;
INSERT INTO dbo.InstallmentSchedule
    (Id, CompanyId, PaymentDocumentId, SequenceNo, Amount, DueDate,
     Paid, PaidAt, PaidAmount, OrganizationId, OperatorLegacyId, Note, Active)
SELECT
    c.Id, CONVERT(tinyint, 4), c.IdPaymentDocument,
    CAST((SELECT COUNT(*) FROM dbo.InstallmentSchedule s2
          WHERE s2.PaymentDocumentId = c.IdPaymentDocument AND s2.Id < c.Id) +
         ROW_NUMBER() OVER (PARTITION BY c.IdPaymentDocument ORDER BY c.Id) AS int),
    CONVERT(decimal(18,4), c.Price), NULL,
    CONVERT(bit, COALESCE(c.Payed, 0)), c.DatePayed,
    CASE WHEN COALESCE(c.Payed,0) = 1 THEN CONVERT(decimal(18,4), c.Price) ELSE NULL END,
    NULLIF(c.IdOrganization, 0), NULLIF(c.IdOperator, 0), c.Note,
    CONVERT(bit, COALESCE(c.Active, 1))
FROM #newSched c
INNER JOIN dbo.PaymentDocument pd ON pd.Id = c.IdPaymentDocument
WHERE NOT EXISTS (SELECT 1 FROM dbo.InstallmentSchedule x WHERE x.Id = c.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.InstallmentSchedule OFF;
PRINT CONCAT('  -> ', @rows, ' new installment schedules.');
DROP TABLE #newSched;

-- rata payments land whenever the customer shows up — sync by recent DatePayed,
-- not by row age.
SELECT r.Id, r.Payed, r.DatePayed, r.Price
INTO #schedPay
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsRata r
WHERE r.Id < @v2floor AND r.Payed = 1 AND r.DatePayed >= DATEADD(day, -90, GETDATE());

UPDATE s SET s.Paid = 1, s.PaidAt = p.DatePayed, s.PaidAmount = CONVERT(decimal(18,4), p.Price)
FROM dbo.InstallmentSchedule s
INNER JOIN #schedPay p ON p.Id = s.Id
WHERE s.Paid = 0;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' installment payments state-synced.');
DROP TABLE #schedPay;

-- ============================================================================
-- Verification
-- ============================================================================
PRINT '';
PRINT '=== New rows added this run ===';
SELECT 'Client'                AS [Table], COUNT(*) AS [New] FROM dbo.Client                WHERE Id > @bClient
UNION ALL SELECT 'Vehicle',              COUNT(*) FROM dbo.Vehicle               WHERE Id > @bVeh
UNION ALL SELECT 'VehicleRegistration',  COUNT(*) FROM dbo.VehicleRegistration   WHERE Id > @bReg
UNION ALL SELECT 'ClientVehicleRelation',COUNT(*) FROM dbo.ClientVehicleRelation WHERE Id > @bRel
UNION ALL SELECT 'Request',              COUNT(*) FROM dbo.Request               WHERE Id > @bReq
UNION ALL SELECT 'OwnershipProof',       COUNT(*) FROM dbo.RequestOwnershipProof WHERE Id > @bOwn
UNION ALL SELECT 'PaymentProof',         COUNT(*) FROM dbo.RequestPaymentProof   WHERE Id > @bPay
UNION ALL SELECT 'TechnicalExamReport',  COUNT(*) FROM dbo.TechnicalExamReport       WHERE Id > @bTeh
UNION ALL SELECT 'TechExamReportDetail', COUNT(*) FROM dbo.TechnicalExamReportDetail WHERE Id > @bTehD
UNION ALL SELECT 'CustomerDebt',         COUNT(*) FROM dbo.CustomerDebt              WHERE Id > @bDebt
UNION ALL SELECT 'PaymentDocument',      COUNT(*) FROM dbo.PaymentDocument           WHERE Id > @bDoc  AND Id < @v2floor
UNION ALL SELECT 'PaymentDocumentLine',  COUNT(*) FROM dbo.PaymentDocumentLine       WHERE Id > @bLine AND Id < @v2floor
UNION ALL SELECT 'InstallmentAgreement', COUNT(*) FROM dbo.InstallmentAgreement      WHERE Id > @bAgr  AND Id < @v2floor
UNION ALL SELECT 'InstallmentSchedule',  COUNT(*) FROM dbo.InstallmentSchedule       WHERE Id > @bSched AND Id < @v2floor;

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Incremental top-up done ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
