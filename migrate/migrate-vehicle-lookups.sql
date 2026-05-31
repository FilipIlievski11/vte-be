-- =============================================================================
-- Vehicle-module lookups: legacy → VTE
--   VehicleBodytype                  → VehicleBodyType
--   VehicleCategories                → VehicleCategory
--   VehicleMakers                    → VehicleMaker
--   VehicleModel                     → VehicleModel
--   Colors                           → VehicleColor
--   VehicleEnginePowerSourceTypes    → VehicleFuel
--   VehicleEngineEcoProgram          → VehicleEcoProgram
--   VehicleEngineTypes               → VehicleEngineType
--   VehicleCategoryForPayments       → VehiclePaymentCategory
--   CustomerVehiclesRelationTypes    → ClientVehicleRelationType
--
-- Preserves legacy Ids exactly. Idempotent — wipes target first.
-- Run AFTER migrate-countries.sql.
--
-- Note: VehicleFuel/EcoProgram/PaymentCategory/ClientVehicleRelationType have
-- tinyint PKs that EF does NOT mark IDENTITY (would risk collisions on the
-- 1..255 range). So those use plain INSERTs with explicit Id values.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-vehicle-lookups.sql -b -X -I -f 65001
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

DECLARE @vehCount int = (SELECT COUNT(*) FROM dbo.Vehicle);
DECLARE @relCount int = (SELECT COUNT(*) FROM dbo.ClientVehicleRelation);
IF (@vehCount + @relCount) > 0
BEGIN
  RAISERROR('Refusing to wipe lookups: Vehicle=%d, ClientVehicleRelation=%d. Wipe those first.', 16, 1, @vehCount, @relCount);
  RETURN;
END;

PRINT '=== Wiping vehicle-module lookups ===';
DELETE FROM dbo.VehicleModel;
DELETE FROM dbo.VehicleMaker;
DELETE FROM dbo.VehicleBodyType;
DELETE FROM dbo.VehicleCategory;
DELETE FROM dbo.VehicleColor;
DELETE FROM dbo.VehicleFuel;
DELETE FROM dbo.VehicleEcoProgram;
DELETE FROM dbo.VehicleEngineType;
DELETE FROM dbo.VehiclePaymentCategory;
DELETE FROM dbo.ClientVehicleRelationType;

DECLARE @rows int;

-- ---------- VehicleBodyType (int, IDENTITY) ----------
PRINT '=== VehicleBodyType ===';
SET IDENTITY_INSERT dbo.VehicleBodyType ON;
INSERT INTO dbo.VehicleBodyType (Id, Code, Name, Active)
SELECT Id,
       NULLIF(LTRIM(RTRIM(BodytypeCode)),       N''),
       COALESCE(NULLIF(LTRIM(RTRIM(BodytypeDescriprion)), N''), N'?'),
       Active
FROM VTEZVV_Snapshot.dbo.VehicleBodytype;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleBodyType OFF;
PRINT CONCAT('  -> ', @rows, ' rows');

-- ---------- VehicleCategory (smallint, IDENTITY) ----------
PRINT '=== VehicleCategory ===';
SET IDENTITY_INSERT dbo.VehicleCategory ON;
INSERT INTO dbo.VehicleCategory (Id, Code, Name, Active)
SELECT CAST(Id AS smallint),
       NULLIF(LTRIM(RTRIM(CategoryCode)), N''),
       COALESCE(NULLIF(LTRIM(RTRIM(CategoryName)), N''), N'?'),
       Active
FROM VTEZVV_Snapshot.dbo.VehicleCategories
WHERE Id BETWEEN 1 AND 32767;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleCategory OFF;
PRINT CONCAT('  -> ', @rows, ' rows');

-- ---------- VehicleMaker (int, IDENTITY) ----------
PRINT '=== VehicleMaker ===';
SET IDENTITY_INSERT dbo.VehicleMaker ON;
INSERT INTO dbo.VehicleMaker (Id, CountryId, Name, Trademark, Active)
SELECT m.Id,
       CASE WHEN c.Id IS NOT NULL THEN CAST(m.IdCountry AS smallint) END,
       COALESCE(NULLIF(LTRIM(RTRIM(m.CompanyName)), N''), N'?'),
       NULLIF(LTRIM(RTRIM(m.CompanyTrademark)), N''),
       m.Active
FROM VTEZVV_Snapshot.dbo.VehicleMakers m
LEFT JOIN dbo.Country c ON c.Id = CASE
    WHEN m.IdCountry BETWEEN 1 AND 32767 THEN CAST(m.IdCountry AS smallint) END;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleMaker OFF;
PRINT CONCAT('  -> ', @rows, ' rows');

-- ---------- VehicleModel (int, IDENTITY) ----------
PRINT '=== VehicleModel ===';
SET IDENTITY_INSERT dbo.VehicleModel ON;
INSERT INTO dbo.VehicleModel (Id, MakerId, Code, Name, ProductionStart, ProductionEnd, Active)
SELECT m.Id, m.IdVehicleMaker,
       NULLIF(LTRIM(RTRIM(m.ModelCode)), N''),
       COALESCE(NULLIF(LTRIM(RTRIM(m.ModelName)), N''), N'?'),
       m.YearOfBeginingProduction,
       m.YearOfEndingProduction,
       m.Active
FROM VTEZVV_Snapshot.dbo.VehicleModel m
INNER JOIN dbo.VehicleMaker vm ON vm.Id = m.IdVehicleMaker;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleModel OFF;
PRINT CONCAT('  -> ', @rows, ' rows');

-- ---------- VehicleColor (smallint, IDENTITY) ----------
PRINT '=== VehicleColor ===';
SET IDENTITY_INSERT dbo.VehicleColor ON;
INSERT INTO dbo.VehicleColor (Id, Code, Name, Active)
SELECT CAST(Id AS smallint),
       NULLIF(LTRIM(RTRIM(ColorCode)), N''),
       COALESCE(NULLIF(LTRIM(RTRIM(ColorDescription)), N''), N'?'),
       Active
FROM VTEZVV_Snapshot.dbo.Colors
WHERE Id BETWEEN 1 AND 32767;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleColor OFF;
PRINT CONCAT('  -> ', @rows, ' rows');

-- ---------- VehicleEngineType (int, IDENTITY) ----------
PRINT '=== VehicleEngineType ===';
SET IDENTITY_INSERT dbo.VehicleEngineType ON;
INSERT INTO dbo.VehicleEngineType (Id, Code, Name, Active)
SELECT Id,
       NULLIF(LTRIM(RTRIM(EngineTypeCode)),       N''),
       COALESCE(
         NULLIF(LTRIM(RTRIM(TechincalDescription)), N''),
         NULLIF(LTRIM(RTRIM(EngineTypeCode)),       N''),
         N'?'
       ),
       Active
FROM VTEZVV_Snapshot.dbo.VehicleEngineTypes;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.VehicleEngineType OFF;
PRINT CONCAT('  -> ', @rows, ' rows');

-- ---------- VehicleFuel (tinyint, NOT IDENTITY — direct INSERT) ----------
PRINT '=== VehicleFuel ===';
INSERT INTO dbo.VehicleFuel (Id, Name, Active)
SELECT CAST(Id AS tinyint),
       COALESCE(NULLIF(LTRIM(RTRIM(PowerSourceName)), N''), N'?'),
       Active
FROM VTEZVV_Snapshot.dbo.VehicleEnginePowerSourceTypes
WHERE Id BETWEEN 1 AND 255;
SET @rows = @@ROWCOUNT;
PRINT CONCAT('  -> ', @rows, ' rows');

-- ---------- VehicleEcoProgram (tinyint, NOT IDENTITY) ----------
PRINT '=== VehicleEcoProgram ===';
INSERT INTO dbo.VehicleEcoProgram (Id, Name, Active)
SELECT CAST(Id AS tinyint),
       COALESCE(NULLIF(LTRIM(RTRIM(EcoProgram)), N''), N'?'),
       Active
FROM VTEZVV_Snapshot.dbo.VehicleEngineEcoProgram
WHERE Id BETWEEN 1 AND 255;
SET @rows = @@ROWCOUNT;
PRINT CONCAT('  -> ', @rows, ' rows');

-- ---------- VehiclePaymentCategory (tinyint, NOT IDENTITY) ----------
PRINT '=== VehiclePaymentCategory ===';
INSERT INTO dbo.VehiclePaymentCategory (Id, Name, Active)
SELECT CAST(Id AS tinyint),
       COALESCE(NULLIF(LTRIM(RTRIM(Name)), N''), N'?'),
       Active
FROM VTEZVV_Snapshot.dbo.VehicleCategoryForPayments
WHERE Id BETWEEN 1 AND 255;
SET @rows = @@ROWCOUNT;
PRINT CONCAT('  -> ', @rows, ' rows');

-- ---------- ClientVehicleRelationType (tinyint, NOT IDENTITY) ----------
PRINT '=== ClientVehicleRelationType ===';
INSERT INTO dbo.ClientVehicleRelationType (Id, Name, IsOwner, IsAuthorized, IsCustomerOnly, Description, Active)
SELECT CAST(Id AS tinyint),
       COALESCE(NULLIF(LTRIM(RTRIM(RelationTypeName)), N''), N'?'),
       IsOwner, IsAuthorized, IsCustomerOnly,
       NULLIF(LTRIM(RTRIM(RelationDescription)), N''),
       Active
FROM VTEZVV_Snapshot.dbo.CustomerVehiclesRelationTypes
WHERE Id BETWEEN 1 AND 255;
SET @rows = @@ROWCOUNT;
PRINT CONCAT('  -> ', @rows, ' rows');

-- Reseed IDENTITY counters past the largest preserved Id (DBCC doesn't accept subqueries)
DECLARE @maxBody  int      = (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleBodyType);
DECLARE @maxCat   smallint = (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleCategory);
DECLARE @maxMk    int      = (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleMaker);
DECLARE @maxMdl   int      = (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleModel);
DECLARE @maxCol   smallint = (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleColor);
DECLARE @maxEng   int      = (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleEngineType);
DBCC CHECKIDENT('dbo.VehicleBodyType',   RESEED, @maxBody) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.VehicleCategory',   RESEED, @maxCat)  WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.VehicleMaker',      RESEED, @maxMk)   WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.VehicleModel',      RESEED, @maxMdl)  WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.VehicleColor',      RESEED, @maxCol)  WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.VehicleEngineType', RESEED, @maxEng)  WITH NO_INFOMSGS;

COMMIT;

PRINT '';
PRINT '=== Final row counts ===';
SELECT [Table] = N'VehicleBodyType',           [Rows] = (SELECT COUNT(*) FROM dbo.VehicleBodyType)
UNION ALL SELECT N'VehicleCategory',                   (SELECT COUNT(*) FROM dbo.VehicleCategory)
UNION ALL SELECT N'VehicleMaker',                      (SELECT COUNT(*) FROM dbo.VehicleMaker)
UNION ALL SELECT N'VehicleModel',                      (SELECT COUNT(*) FROM dbo.VehicleModel)
UNION ALL SELECT N'VehicleColor',                      (SELECT COUNT(*) FROM dbo.VehicleColor)
UNION ALL SELECT N'VehicleFuel',                       (SELECT COUNT(*) FROM dbo.VehicleFuel)
UNION ALL SELECT N'VehicleEcoProgram',                 (SELECT COUNT(*) FROM dbo.VehicleEcoProgram)
UNION ALL SELECT N'VehicleEngineType',                 (SELECT COUNT(*) FROM dbo.VehicleEngineType)
UNION ALL SELECT N'VehiclePaymentCategory',            (SELECT COUNT(*) FROM dbo.VehiclePaymentCategory)
UNION ALL SELECT N'ClientVehicleRelationType',         (SELECT COUNT(*) FROM dbo.ClientVehicleRelationType);

PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  PRINT '!!! Migration failed — rolled back';
  THROW;
END CATCH;
GO
