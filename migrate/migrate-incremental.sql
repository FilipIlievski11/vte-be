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
DECLARE @bClient bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.Client);
DECLARE @bVeh    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.Vehicle);
DECLARE @bReg    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleRegistration);
DECLARE @bRel    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.ClientVehicleRelation);
DECLARE @bReq    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.Request);
DECLARE @bOwn    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestOwnershipProof);
DECLARE @bPay    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestPaymentProof);
DECLARE @bTeh    bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.TechnicalExamReport);
DECLARE @bTehD   bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.TechnicalExamReportDetail);
DECLARE @fallbackIssuer tinyint = (SELECT TOP 1 Id FROM dbo.DocumentIssuer ORDER BY Id);
DECLARE @rows int;
PRINT CONCAT('Before maxes — Client:', @bClient, ' Vehicle:', @bVeh, ' Reg:', @bReg,
             ' Rel:', @bRel, ' Request:', @bReq);

-- ============================================================================
-- 1. Client (Id > @bClient)
-- ============================================================================
PRINT '=== Client ===';
SET IDENTITY_INSERT dbo.Client ON;
INSERT INTO dbo.Client (Id, CompanyId, CityId, CitizenshipId, Business,
  FirstName, MiddleName, LastName, MB, Address,
  TaxNumber, PhoneNumber, Email, DateOfBirth, Note, Active, CreatedAt)
SELECT
  c.Id, CAST(4 AS tinyint), ci.Id, cz.Id, c.IsCompany,
  NULLIF(LTRIM(RTRIM(c.CustomerFirstName)), N''),
  NULLIF(LTRIM(RTRIM(c.ParentName)),        N''),
  NULLIF(LTRIM(RTRIM(c.CustomerSurname)),   N''),
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
DBCC CHECKIDENT('dbo.Client', RESEED) WITH NO_INFOMSGS;
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
  Id, CompanyId, Vin, EngineNumber, Plate,
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
DBCC CHECKIDENT('dbo.Vehicle', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' new Vehicles.');

-- ============================================================================
-- 3. VehicleRegistration (Id > @bReg)
-- ============================================================================
PRINT '=== VehicleRegistration ===';
SET IDENTITY_INSERT dbo.VehicleRegistration ON;
INSERT INTO dbo.VehicleRegistration (Id, VehicleId, IssuerId, PlateNumber, RegisteredDate, ValidUntil, IsFirstRegistration, Active)
SELECT r.Id, r.IdVehicle, CAST(r.IdRegistrationIssuer AS tinyint), LTRIM(RTRIM(r.RegistrationNumber)),
       r.DateOfRegistration, r.DateRegistrationValidTill, r.IsFirstRegistration, r.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.[Vehicle.Registrations] r
INNER JOIN dbo.Vehicle        v  ON v.Id  = r.IdVehicle
INNER JOIN dbo.DocumentIssuer di ON di.Id = CASE WHEN r.IdRegistrationIssuer BETWEEN 1 AND 255 THEN CAST(r.IdRegistrationIssuer AS tinyint) END
WHERE r.Id > @bReg AND r.RegistrationNumber IS NOT NULL AND LEN(LTRIM(RTRIM(r.RegistrationNumber))) > 0;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleRegistration OFF;
DBCC CHECKIDENT('dbo.VehicleRegistration', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' new VehicleRegistrations.');

-- ============================================================================
-- 4. ClientVehicleRelation (Id > @bRel)
-- ============================================================================
PRINT '=== ClientVehicleRelation ===';
SET IDENTITY_INSERT dbo.ClientVehicleRelation ON;
INSERT INTO dbo.ClientVehicleRelation (Id, ClientId, VehicleId, RelationTypeId, StartDate, EndDate, StartNote, EndNote, Active)
SELECT r.Id, r.IdCustomer,
  CASE WHEN r.IdVehicle IS NOT NULL AND v.Id IS NOT NULL THEN r.IdVehicle END,
  CAST(r.IdRelationType AS tinyint), r.StartDate, r.EndDate,
  NULLIF(LTRIM(RTRIM(r.BeginNote)), N''), NULLIF(LTRIM(RTRIM(r.TerminationNote)), N''), r.Active
FROM VTEZVV_LIVE.VTEZVV.dbo.CustomerVehiclesRelations r
INNER JOIN dbo.Client                     c  ON c.Id  = r.IdCustomer
INNER JOIN dbo.ClientVehicleRelationType rt ON rt.Id = CAST(r.IdRelationType AS tinyint)
LEFT  JOIN dbo.Vehicle                    v  ON v.Id  = r.IdVehicle
WHERE r.Id > @bRel
  AND ((r.IdVehicle IS NULL AND rt.IsCustomerOnly = 1) OR (v.Id IS NOT NULL));
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.ClientVehicleRelation OFF;
DBCC CHECKIDENT('dbo.ClientVehicleRelation', RESEED) WITH NO_INFOMSGS;
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
DBCC CHECKIDENT('dbo.Request', RESEED) WITH NO_INFOMSGS;
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
DBCC CHECKIDENT('dbo.RequestOwnershipProof', RESEED) WITH NO_INFOMSGS;
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
DBCC CHECKIDENT('dbo.RequestPaymentProof', RESEED) WITH NO_INFOMSGS;
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
DBCC CHECKIDENT('dbo.TechnicalExamReport', RESEED) WITH NO_INFOMSGS;
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
DBCC CHECKIDENT('dbo.TechnicalExamReportDetail', RESEED) WITH NO_INFOMSGS;
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
UNION ALL SELECT 'TechExamReportDetail', COUNT(*) FROM dbo.TechnicalExamReportDetail WHERE Id > @bTehD;

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Incremental top-up done ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
