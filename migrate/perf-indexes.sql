-- Adds covering / search indexes recommended after profiling. Idempotent.
SET NOCOUNT ON;
USE VTE2;
GO

-- Customers: hot search columns
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Customers_PhoneNumber' AND object_id=OBJECT_ID('dbo.Customers'))
  CREATE INDEX IX_Customers_PhoneNumber ON dbo.Customers (PhoneNumber) WHERE PhoneNumber IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Customers_TaxNumber' AND object_id=OBJECT_ID('dbo.Customers'))
  CREATE INDEX IX_Customers_TaxNumber ON dbo.Customers (TaxNumber) WHERE TaxNumber IS NOT NULL;
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Customers_Email' AND object_id=OBJECT_ID('dbo.Customers'))
  CREATE INDEX IX_Customers_Email ON dbo.Customers (Email) WHERE Email IS NOT NULL;

-- Vehicles: search by FirstRegistrationNumber (legacy registration)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Vehicles_FirstRegistrationNumber' AND object_id=OBJECT_ID('dbo.Vehicles'))
  CREATE INDEX IX_Vehicles_FirstRegistrationNumber ON dbo.Vehicles (FirstRegistrationNumber);

-- TechnicalExamReports: search by RegNumber
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_TechnicalExamReports_RegNumber' AND object_id=OBJECT_ID('dbo.TechnicalExamReports'))
  CREATE INDEX IX_TechnicalExamReports_RegNumber ON dbo.TechnicalExamReports (RegNumber) WHERE RegNumber IS NOT NULL;

-- VehicleModels: filter-by-Maker (used by /api/ref/vehicle-models?makerId=)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_VehicleModels_VehicleMakerId' AND object_id=OBJECT_ID('dbo.VehicleModels'))
  CREATE INDEX IX_VehicleModels_VehicleMakerId ON dbo.VehicleModels (VehicleMakerId, IsActive) INCLUDE (Name);

-- Cities filter by Community
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Cities_CommunityId' AND object_id=OBJECT_ID('dbo.Cities'))
  CREATE INDEX IX_Cities_CommunityId ON dbo.Cities (CommunityId) WHERE CommunityId IS NOT NULL;

-- VehicleRegistrations expiring soon
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_VehicleRegistrations_ValidTill' AND object_id=OBJECT_ID('dbo.VehicleRegistrations'))
  CREATE INDEX IX_VehicleRegistrations_ValidTill ON dbo.VehicleRegistrations (ValidTill) INCLUDE (VehicleId);

-- Update statistics
EXEC sp_updatestats;
PRINT 'Indexes created and statistics updated.';
GO
