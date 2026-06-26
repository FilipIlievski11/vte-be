-- =============================================================================
-- VTEZVV Vehicles → VTE Vehicle (+ VehicleRegistration + ClientVehicleRelation)
--   Source: (localdb)\MSSQLLocalDB\VTEZVV_Snapshot
--   Target: (localdb)\MSSQLLocalDB\VTE
--
-- Plan:
--   1. Vehicles  → Vehicle  (40,308 rows, CompanyId=4, preserve Ids)
--      - LEFT JOIN every FK lookup; FK is NULLed when legacy value doesn't resolve
--      - Carry only the columns identified as meaningful in the analysis
--   2. Vehicle.Registrations → VehicleRegistration  (139,580 rows, preserve Ids)
--   3. CustomerVehiclesRelations → ClientVehicleRelation  (73,793 rows, preserve Ids)
--      - VehicleId NULL allowed for RelationType=3 (client-only)
--
-- Idempotent — wipes target in FK order before inserting.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-vehicles.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN
  RAISERROR('Source DB VTEZVV_Snapshot is missing. Run snapshot-legacy.ps1 first.', 16, 1);
  RETURN;
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Company WHERE Id = 4)
BEGIN
  RAISERROR('Target Company Id=4 not found.', 16, 1);
  RETURN;
END;
IF (SELECT COUNT(*) FROM dbo.VehicleBodyType) = 0
BEGIN
  RAISERROR('Vehicle lookups not populated. Run migrate-vehicle-lookups.sql first.', 16, 1);
  RETURN;
END;
IF (SELECT COUNT(*) FROM dbo.Client) = 0
BEGIN
  RAISERROR('Clients not populated. Run migrate-clients.sql first.', 16, 1);
  RETURN;
END;

PRINT '=== Wiping target tables ===';
DELETE FROM dbo.ClientVehicleRelation;
DELETE FROM dbo.VehicleRegistration;
DELETE FROM dbo.Vehicle;
DBCC CHECKIDENT('dbo.ClientVehicleRelation', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.VehicleRegistration',   RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.Vehicle',               RESEED, 0) WITH NO_INFOMSGS;

DECLARE @rows int;

-- ============================================================================
-- 1. Vehicle
-- ============================================================================
PRINT '=== Vehicles (preserve Ids, CompanyId=4) ===';
SET IDENTITY_INSERT dbo.Vehicle ON;
INSERT INTO dbo.Vehicle (
  Id, CompanyId,
  Vin, EngineNumber, Plate, LastRegistrationValidUntil,
  CategoryId, BodyTypeId, ModelId, PrimaryColorId, SecondaryColorId, MadeCountryId,
  FuelId, SecondFuelId, EngineTypeId, EcoProgramId, PaymentCategoryId,
  EnginePowerKw, EngineWorkingCapacityCc, MaxRpm, MaxSpeedKmh, HasLpg,
  LengthMm, WidthMm, HeightMm,
  EmptyWeightKg, MaxAllowedWeightKg, MaxLegalTotalMassKg, MaxConstructiveTotalMassKg,
  TrailerMassWithBrakesKg, TrailerMassWithoutBrakesKg,
  AxleCount, WheelCount, AxleLoad1Kg, AxleLoad2Kg,
  Seats, StandingSeats,
  Co2GKm, NoiseStaticDb, NoiseMovingDb,
  TypeText, ModelVariant, ApprovalMark,
  Note, Active, CreatedAt
)
SELECT
  v.Id,
  CAST(4 AS tinyint)                                                   AS CompanyId,

  COALESCE(NULLIF(LTRIM(RTRIM(v.ShellNumber)), N''), N'')               AS Vin,
  NULLIF(LTRIM(RTRIM(v.EngineNumber)),         N'')                     AS EngineNumber,
  NULLIF(LTRIM(RTRIM(v.LastRegistratinNumber)),N'')                     AS Plate,
  CASE WHEN v.LastRegistrationValidTill > '1900-01-01' AND v.LastRegistrationValidTill < '2100-01-01'
       THEN v.LastRegistrationValidTill END                             AS LastRegistrationValidUntil,

  CASE WHEN cat.Id IS NOT NULL THEN cat.Id  END                         AS CategoryId,
  CASE WHEN bt.Id  IS NOT NULL THEN bt.Id   END                         AS BodyTypeId,
  CASE WHEN mdl.Id IS NOT NULL THEN mdl.Id  END                         AS ModelId,
  CASE WHEN col1.Id IS NOT NULL THEN col1.Id END                        AS PrimaryColorId,
  CASE WHEN col2.Id IS NOT NULL THEN col2.Id END                        AS SecondaryColorId,
  CASE WHEN co.Id  IS NOT NULL THEN co.Id   END                         AS MadeCountryId,
  CASE WHEN f1.Id  IS NOT NULL THEN f1.Id   END                         AS FuelId,
  CASE WHEN f2.Id  IS NOT NULL THEN f2.Id   END                         AS SecondFuelId,
  CASE WHEN et.Id  IS NOT NULL THEN et.Id   END                         AS EngineTypeId,
  CASE WHEN eco.Id IS NOT NULL THEN eco.Id  END                         AS EcoProgramId,
  CASE WHEN pc.Id  IS NOT NULL THEN pc.Id   END                         AS PaymentCategoryId,

  NULLIF(v.EnginePowerOutPut, 0)                                        AS EnginePowerKw,
  NULLIF(v.EngineWorkingCapacity, 0)                                    AS EngineWorkingCapacityCc,
  NULLIF(v.BrojNaVrtezi, 0)                                             AS MaxRpm,
  NULLIF(v.MaxSpeed, 0)                                                 AS MaxSpeedKmh,
  v.TNG                                                                  AS HasLpg,

  NULLIF(v.VehicleSizeLength, 0)                                        AS LengthMm,
  NULLIF(v.VehicleSizeWidth,  0)                                        AS WidthMm,
  NULLIF(v.VehicleSizeHight,  0)                                        AS HeightMm,

  NULLIF(v.EmptyWaight, 0)                                              AS EmptyWeightKg,
  NULLIF(v.MaximunAllowedWaight, 0)                                     AS MaxAllowedWeightKg,
  NULLIF(v.MaxLegVkMasa, 0)                                             AS MaxLegalTotalMassKg,
  NULLIF(v.MaxKonstVkMasa, 0)                                           AS MaxConstructiveTotalMassKg,
  NULLIF(LTRIM(RTRIM(v.TrailerWaightWithBreak)),    N'')                 AS TrailerMassWithBrakesKg,
  NULLIF(LTRIM(RTRIM(v.TrailerWaightWithoutBreak)), N'')                 AS TrailerMassWithoutBrakesKg,

  NULLIF(v.NumberOfAxis, 0)                                             AS AxleCount,
  NULLIF(v.NumberOfWheels, 0)                                           AS WheelCount,
  NULLIF(v.OsnoOptovaruvanje1, 0)                                       AS AxleLoad1Kg,
  NULLIF(v.OsnoOptovaruvanje2, 0)                                       AS AxleLoad2Kg,

  NULLIF(v.NumberOfSeats, 0)                                            AS Seats,
  NULLIF(v.NumberOfStandingSeats, 0)                                    AS StandingSeats,

  NULLIF(v.CO2, 0)                                                       AS Co2GKm,
  NULLIF(v.NoiseStatic, 0)                                              AS NoiseStaticDb,
  NULLIF(v.NoiseMovment, 0)                                             AS NoiseMovingDb,

  NULLIF(LTRIM(RTRIM(v.Tip)),                  N'')                     AS TypeText,
  NULLIF(LTRIM(RTRIM(v.VehicleModelAdding)),   N'')                     AS ModelVariant,
  NULLIF(LTRIM(RTRIM(v.OznakaNaOdobrenie)),    N'')                     AS ApprovalMark,

  NULLIF(LTRIM(RTRIM(v.Note)), N'')                                     AS Note,
  v.Active                                                              AS Active,
  GETUTCDATE()                                                          AS CreatedAt
FROM VTEZVV_Snapshot.dbo.Vehicles v
LEFT JOIN dbo.VehicleCategory   cat ON cat.Id  = CAST(NULLIF(v.IdVehicleCategories, 0)            AS smallint)
LEFT JOIN dbo.VehicleBodyType   bt  ON bt.Id   = NULLIF(v.IdVehicleBodyType, 0)
LEFT JOIN dbo.VehicleModel      mdl ON mdl.Id  = NULLIF(v.IdVehicleModel, 0)
LEFT JOIN dbo.VehicleColor      col1 ON col1.Id = CAST(NULLIF(v.IdPrimaryColor, 0)                 AS smallint)
LEFT JOIN dbo.VehicleColor      col2 ON col2.Id = CAST(NULLIF(v.IdSecondaryColor, 0)               AS smallint)
LEFT JOIN dbo.Country           co   ON co.Id  = CAST(NULLIF(v.IdMadeCountry, 0)                   AS smallint)
LEFT JOIN dbo.VehicleFuel       f1   ON f1.Id  = CASE WHEN v.IdEnginePowerSource BETWEEN 1 AND 255
                                                       THEN CAST(v.IdEnginePowerSource AS tinyint) END
LEFT JOIN dbo.VehicleFuel       f2   ON f2.Id  = CASE WHEN v.IdEngineSecondPowerSource BETWEEN 1 AND 255
                                                       THEN CAST(v.IdEngineSecondPowerSource AS tinyint) END
LEFT JOIN dbo.VehicleEngineType et   ON et.Id  = NULLIF(v.IdEngineType, 0)
LEFT JOIN dbo.VehicleEcoProgram eco  ON eco.Id = CASE WHEN v.IdEngineEcoProgram BETWEEN 1 AND 255
                                                       THEN CAST(v.IdEngineEcoProgram AS tinyint) END
LEFT JOIN dbo.VehiclePaymentCategory pc ON pc.Id = CASE WHEN v.IdVehicleCategoryForPayments BETWEEN 1 AND 255
                                                       THEN CAST(v.IdVehicleCategoryForPayments AS tinyint) END;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.Vehicle OFF;
DECLARE @maxVeh bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.Vehicle);
DBCC CHECKIDENT('dbo.Vehicle', RESEED, @maxVeh) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' Vehicles. Identity counter reseeded to ', @maxVeh, '.');

-- ============================================================================
-- 2. VehicleRegistration
--    Filter to rows whose Vehicle exists in our migrated set + Issuer resolves.
-- ============================================================================
PRINT '=== VehicleRegistration ===';
SET IDENTITY_INSERT dbo.VehicleRegistration ON;
INSERT INTO dbo.VehicleRegistration (Id, VehicleId, IssuerId, PlateNumber, RegisteredDate, ValidUntil, IsFirstRegistration, Active)
SELECT
  r.Id,
  r.IdVehicle,
  CAST(r.IdRegistrationIssuer AS tinyint),
  LTRIM(RTRIM(r.RegistrationNumber)),
  r.DateOfRegistration,
  r.DateRegistrationValidTill,
  r.IsFirstRegistration,
  r.Active
FROM VTEZVV_Snapshot.dbo.[Vehicle.Registrations] r
INNER JOIN dbo.Vehicle        v ON v.Id   = r.IdVehicle
INNER JOIN dbo.DocumentIssuer di ON di.Id = CASE WHEN r.IdRegistrationIssuer BETWEEN 1 AND 255
                                                  THEN CAST(r.IdRegistrationIssuer AS tinyint) END
WHERE r.RegistrationNumber IS NOT NULL
  AND LEN(LTRIM(RTRIM(r.RegistrationNumber))) > 0;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleRegistration OFF;
DECLARE @maxReg bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleRegistration);
DBCC CHECKIDENT('dbo.VehicleRegistration', RESEED, @maxReg) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' rows. Identity counter reseeded to ', @maxReg, '.');

-- ============================================================================
-- 3. ClientVehicleRelation
--    VehicleId may be NULL (RelationType=3 "client only").
--    Skip rows whose Client or non-null Vehicle doesn't resolve to our data.
-- ============================================================================
PRINT '=== ClientVehicleRelation ===';
SET IDENTITY_INSERT dbo.ClientVehicleRelation ON;
INSERT INTO dbo.ClientVehicleRelation (Id, ClientId, VehicleId, RelationTypeId, StartDate, EndDate, StartNote, EndNote, Active)
SELECT
  r.Id,
  r.IdCustomer,
  CASE WHEN r.IdVehicle IS NOT NULL AND v.Id IS NOT NULL THEN r.IdVehicle END     AS VehicleId,
  CAST(r.IdRelationType AS tinyint),
  r.StartDate,
  r.EndDate,
  NULLIF(LTRIM(RTRIM(r.BeginNote)), N''),
  NULLIF(LTRIM(RTRIM(r.TerminationNote)), N''),
  r.Active
FROM VTEZVV_Snapshot.dbo.CustomerVehiclesRelations r
INNER JOIN dbo.Client                     c  ON c.Id  = r.IdCustomer
INNER JOIN dbo.ClientVehicleRelationType rt ON rt.Id = CAST(r.IdRelationType AS tinyint)
LEFT  JOIN dbo.Vehicle                    v  ON v.Id  = r.IdVehicle
WHERE
  -- only include rows whose Vehicle resolves OR where VehicleId is legitimately NULL (RelationType=3 / IsCustomerOnly)
  (r.IdVehicle IS NULL AND rt.IsCustomerOnly = 1) OR (v.Id IS NOT NULL);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.ClientVehicleRelation OFF;
DECLARE @maxRel bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.ClientVehicleRelation);
DBCC CHECKIDENT('dbo.ClientVehicleRelation', RESEED, @maxRel) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' rows. Identity counter reseeded to ', @maxRel, '.');

COMMIT;

PRINT '';
PRINT '=== Verification ===';
SELECT [Table] = N'Vehicle',                  [Rows] = (SELECT COUNT(*) FROM dbo.Vehicle)
UNION ALL SELECT N'VehicleRegistration',              (SELECT COUNT(*) FROM dbo.VehicleRegistration)
UNION ALL SELECT N'ClientVehicleRelation',            (SELECT COUNT(*) FROM dbo.ClientVehicleRelation);

PRINT 'Vehicles with at least one Client (relation type=Owner):';
SELECT COUNT(DISTINCT cvr.VehicleId) AS VehiclesWithOwners
FROM dbo.ClientVehicleRelation cvr
JOIN dbo.ClientVehicleRelationType rt ON rt.Id = cvr.RelationTypeId
WHERE rt.IsOwner = 1 AND cvr.VehicleId IS NOT NULL;

PRINT 'Sample 3 Vehicles + maker + model + body + color:';
SELECT TOP 3 v.Id, v.Vin, v.Plate,
       vmk.Name AS Maker, vmod.Name AS Model,
       vbt.Name AS BodyType, vc.Name AS Category, vcol.Name AS Color
FROM dbo.Vehicle v
LEFT JOIN dbo.VehicleModel    vmod ON vmod.Id = v.ModelId
LEFT JOIN dbo.VehicleMaker    vmk  ON vmk.Id  = vmod.MakerId
LEFT JOIN dbo.VehicleBodyType vbt  ON vbt.Id  = v.BodyTypeId
LEFT JOIN dbo.VehicleCategory vc   ON vc.Id   = v.CategoryId
LEFT JOIN dbo.VehicleColor    vcol ON vcol.Id = v.PrimaryColorId
ORDER BY v.Id;

PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  PRINT '!!! Migration failed — rolled back';
  THROW;
END CATCH;
GO
