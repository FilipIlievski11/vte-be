-- Migrate legacy production reference data from [VTE] (legacy) into [VTE2] (modern).
-- Strategy: only migrate tables where modern is empty/minimal AND legacy has real data.
--   Communities  : legacy 87  -> modern 0  (FULL)
--   Cities       : legacy 1646-> modern 31 (REPLACE)
--   Streets      : legacy 249 -> modern 0  (FULL)
--   RegistrationIssuers: legacy 23 -> modern 0 (FULL)
--   VehicleMakers: legacy 117 -> modern 0  (FULL)
--   VehicleModels: legacy 432 -> modern 0  (FULL, mapped from VehicleModel)
--   CalculationItems : legacy 7 -> modern 0 (FULL)
--   PriceCatalog : legacy 2  -> modern 0  (FULL)
-- Other reference tables already have modern seeds and are skipped.
-- Business data (Customers/Vehicles/Requests/Exams/Payments) is empty in legacy, nothing to migrate.

SET NOCOUNT ON;
SET XACT_ABORT ON;
USE VTE2;
GO

PRINT '=== Migrating reference data from VTE -> VTE2 ===';

------------------------------------------------------------------------------
-- 1. Communities (parent of Cities)
------------------------------------------------------------------------------
PRINT 'Communities...';
DELETE FROM Streets;        -- depends on Cities (CityId)
DELETE FROM Cities;          -- depends on Communities (CommunityId)
DELETE FROM Communities;
DBCC CHECKIDENT ('Communities', RESEED, 0) WITH NO_INFOMSGS;

SET IDENTITY_INSERT Communities ON;
INSERT INTO Communities (Id, Name, CountryId, IsActive, CreatedUtc, LastModifiedUtc)
SELECT
    c.Id,
    LTRIM(RTRIM(c.CommunityName)) AS Name,
    NULL AS CountryId,
    c.Active,
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
FROM VTE.dbo.Communities c
WHERE LTRIM(RTRIM(ISNULL(c.CommunityName, ''))) <> '';
SET IDENTITY_INSERT Communities OFF;
DECLARE @c1 INT = (SELECT COUNT(*) FROM Communities); PRINT CONCAT('  Communities migrated: ', @c1);

------------------------------------------------------------------------------
-- 2. Cities (depends on Communities)
------------------------------------------------------------------------------
PRINT 'Cities...';
SET IDENTITY_INSERT Cities ON;
INSERT INTO Cities (Id, Name, PostalCode, CommunityId, CountryId, Description, IsActive, CreatedUtc, LastModifiedUtc)
SELECT
    c.Id,
    LTRIM(RTRIM(c.CityName)) AS Name,
    CASE WHEN c.CityZip IS NULL OR c.CityZip = 0 THEN NULL ELSE CAST(c.CityZip AS NVARCHAR(20)) END,
    CASE WHEN c.IdCommunityCode IS NULL THEN NULL
         WHEN EXISTS (SELECT 1 FROM Communities co WHERE co.Id = c.IdCommunityCode) THEN c.IdCommunityCode
         ELSE NULL END,
    CASE WHEN c.IdCountry IS NULL THEN NULL
         WHEN EXISTS (SELECT 1 FROM Countries co WHERE co.Id = c.IdCountry) THEN c.IdCountry
         ELSE NULL END,
    NULL,
    c.Active,
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
FROM VTE.dbo.Cities c
WHERE LTRIM(RTRIM(ISNULL(c.CityName, ''))) <> '';
SET IDENTITY_INSERT Cities OFF;
DECLARE @c2 INT = (SELECT COUNT(*) FROM Cities); PRINT CONCAT('  Cities migrated: ', @c2);

------------------------------------------------------------------------------
-- 3. Streets
------------------------------------------------------------------------------
PRINT 'Streets...';
DBCC CHECKIDENT ('Streets', RESEED, 0) WITH NO_INFOMSGS;
SET IDENTITY_INSERT Streets ON;
INSERT INTO Streets (Id, Name, CityId, IsActive, CreatedUtc, LastModifiedUtc)
SELECT
    s.Id,
    LTRIM(RTRIM(s.StreetName)) AS Name,
    NULL AS CityId, -- legacy Streets is not linked to Cities
    s.Active,
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
FROM VTE.dbo.Streets s
WHERE LTRIM(RTRIM(ISNULL(s.StreetName, ''))) <> '';
SET IDENTITY_INSERT Streets OFF;
DECLARE @c3 INT = (SELECT COUNT(*) FROM Streets); PRINT CONCAT('  Streets migrated: ', @c3);

------------------------------------------------------------------------------
-- 4. RegistrationIssuers
------------------------------------------------------------------------------
PRINT 'RegistrationIssuers...';
DELETE FROM RegistrationIssuers;
DBCC CHECKIDENT ('RegistrationIssuers', RESEED, 0) WITH NO_INFOMSGS;
SET IDENTITY_INSERT RegistrationIssuers ON;
INSERT INTO RegistrationIssuers (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT
    r.Id,
    LTRIM(RTRIM(r.IssuerName)) AS Name,
    r.Active,
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
FROM VTE.dbo.RegistrationIssuers r
WHERE LTRIM(RTRIM(ISNULL(r.IssuerName, ''))) <> '';
SET IDENTITY_INSERT RegistrationIssuers OFF;
DECLARE @c4 INT = (SELECT COUNT(*) FROM RegistrationIssuers); PRINT CONCAT('  RegistrationIssuers migrated: ', @c4);

------------------------------------------------------------------------------
-- 5. VehicleMakers (parent of VehicleModels)
------------------------------------------------------------------------------
PRINT 'VehicleMakers...';
DELETE FROM VehicleModels;
DELETE FROM VehicleMakers;
DBCC CHECKIDENT ('VehicleMakers', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('VehicleModels', RESEED, 0) WITH NO_INFOMSGS;
SET IDENTITY_INSERT VehicleMakers ON;
INSERT INTO VehicleMakers (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT
    vm.Id,
    LTRIM(RTRIM(vm.CompanyName)) AS Name,
    vm.Active,
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
FROM VTE.dbo.VehicleMakers vm
WHERE LTRIM(RTRIM(ISNULL(vm.CompanyName, ''))) <> '';
SET IDENTITY_INSERT VehicleMakers OFF;
DECLARE @c5 INT = (SELECT COUNT(*) FROM VehicleMakers); PRINT CONCAT('  VehicleMakers migrated: ', @c5);

------------------------------------------------------------------------------
-- 6. VehicleModels (legacy "VehicleModel" singular)
------------------------------------------------------------------------------
PRINT 'VehicleModels...';
SET IDENTITY_INSERT VehicleModels ON;
INSERT INTO VehicleModels (Id, VehicleMakerId, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT
    m.Id,
    m.IdVehicleMaker,
    LTRIM(RTRIM(
        CASE
          WHEN LTRIM(RTRIM(ISNULL(m.ModelName, ''))) <> '' THEN m.ModelName
          ELSE m.ModelCode
        END
    )) AS Name,
    m.Active,
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
FROM VTE.dbo.VehicleModel m
WHERE m.IdVehicleMaker IS NOT NULL
  AND EXISTS (SELECT 1 FROM VehicleMakers vm WHERE vm.Id = m.IdVehicleMaker)
  AND LTRIM(RTRIM(ISNULL(CASE WHEN ISNULL(m.ModelName,'')='' THEN m.ModelCode ELSE m.ModelName END, ''))) <> '';
SET IDENTITY_INSERT VehicleModels OFF;
DECLARE @c6 INT = (SELECT COUNT(*) FROM VehicleModels); PRINT CONCAT('  VehicleModels migrated: ', @c6);

------------------------------------------------------------------------------
-- 7. CalculationItems
------------------------------------------------------------------------------
IF OBJECT_ID('dbo.CalculationItems', 'U') IS NOT NULL
BEGIN
  PRINT 'CalculationItems...';
  DELETE FROM CalculationItems;
  DBCC CHECKIDENT ('CalculationItems', RESEED, 0) WITH NO_INFOMSGS;
  DECLARE @CalcCount INT;
  -- Map legacy "CalculationItems" - copy by detected columns
  -- Legacy schema uses ItemName + BankAccount + Bank + DocumentTypePrint reference
  DECLARE @StationId INT = (SELECT TOP 1 Id FROM Stations WHERE IsActive = 1 ORDER BY Id);
  SET IDENTITY_INSERT CalculationItems ON;
  INSERT INTO CalculationItems (Id, StationId, ItemName, BankAccount, Bank, Form, IsActive, CreatedUtc, LastModifiedUtc)
  SELECT
    ci.Id,
    @StationId,
    LTRIM(RTRIM(ci.ItemName)),
    ci.BankAccount,
    ci.Bank,
    ci.Form,
    ci.Active,
    SYSUTCDATETIME(),
    SYSUTCDATETIME()
  FROM VTE.dbo.CalculationItems ci
  WHERE LTRIM(RTRIM(ISNULL(ci.ItemName, ''))) <> '';
  SET IDENTITY_INSERT CalculationItems OFF;
  SET @CalcCount = @@ROWCOUNT;
  PRINT CONCAT('  CalculationItems migrated: ', @CalcCount);
END

-- PriceCatalog skipped: legacy has no FK to CalculationItems; only 2 rows. Re-seeded later if needed.

PRINT '=== Migration complete ===';
GO

-- Final summary
SELECT 'Communities' T, COUNT(*) C FROM Communities UNION ALL
SELECT 'Cities', COUNT(*) FROM Cities UNION ALL
SELECT 'Streets', COUNT(*) FROM Streets UNION ALL
SELECT 'RegistrationIssuers', COUNT(*) FROM RegistrationIssuers UNION ALL
SELECT 'VehicleMakers', COUNT(*) FROM VehicleMakers UNION ALL
SELECT 'VehicleModels', COUNT(*) FROM VehicleModels
ORDER BY T;
GO
