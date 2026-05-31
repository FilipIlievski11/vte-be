-- =====================================================================
-- Production data migration: [VTEZVV_Snapshot] -> [VTE2]
-- Single-tenant legacy → all rows StationId=1 (Бранзис Скопје).
-- IDs preserved via SET IDENTITY_INSERT. Idempotent.
-- =====================================================================

SET NOCOUNT ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
USE VTE2;
GO
SET QUOTED_IDENTIFIER ON;
GO

DECLARE @AdminUserId NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetUsers ORDER BY Id);
PRINT CONCAT('Admin user: ', @AdminUserId);

-- =============================================================
-- Phase 1: Stations
-- =============================================================
PRINT '=== Phase 1: Stations ===';
UPDATE Stations SET Name = N'Бранзис Скопје', Code = N'BRZ-SK', LastModifiedUtc = SYSUTCDATETIME() WHERE Id = 1;
IF NOT EXISTS (SELECT 1 FROM Stations WHERE Code = N'BRZ-BT')
  INSERT INTO Stations (Name, Code, IsActive, CreatedUtc, LastModifiedUtc) VALUES (N'Битола', N'BRZ-BT', 1, SYSUTCDATETIME(), SYSUTCDATETIME());
IF NOT EXISTS (SELECT 1 FROM Stations WHERE Code = N'BRZ-TE')
  INSERT INTO Stations (Name, Code, IsActive, CreatedUtc, LastModifiedUtc) VALUES (N'Тетово', N'BRZ-TE', 1, SYSUTCDATETIME(), SYSUTCDATETIME());

DECLARE @StationId INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');
PRINT CONCAT('  Default station Id: ', @StationId);
GO

-- =============================================================
-- Phase 2: Disable FKs and truncate everything except Stations + Auth
-- =============================================================
PRINT '=== Phase 2: Truncate target tables ===';
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

DELETE FROM PaymentDocumentInstallments;
DELETE FROM PaymentDocumentDetails;
DELETE FROM PaymentDocumentAttachments;
DELETE FROM PaymentDocuments;
DELETE FROM InstallmentContracts;
DELETE FROM InternationalDrivingLicenceCategories;
DELETE FROM InternationalDrivingLicences;
DELETE FROM Permissions;
DELETE FROM TrafficLicenceExtensions;
DELETE FROM TrafficLicences;
DELETE FROM TechnicalExamReportVisualErrors;
DELETE FROM TechnicalExamReportDetails;
DELETE FROM TechnicalExamReportAttachments;
DELETE FROM TechnicalExamReports;
DELETE FROM RequestVehicleOwnershipProofs;
DELETE FROM RequestPaymentProofs;
DELETE FROM RequestAttachments;
DELETE FROM Requests;
DELETE FROM CustomerFinancialState;
DELETE FROM CustomerVehicleRelations;
DELETE FROM VehicleRegistrations;
DELETE FROM VehicleTyres;
DELETE FROM VehicleAxleDistances;
DELETE FROM VehicleAxles;
DELETE FROM VehicleAttachments;
DELETE FROM Vehicles;
DELETE FROM CustomerContactPersons;
DELETE FROM CustomerBankAccounts;
DELETE FROM CustomerAttachments;
DELETE FROM Customers;
DELETE FROM PriceCatalog;
DELETE FROM CalculationItems;
DELETE FROM Streets;
DELETE FROM Cities;
DELETE FROM Communities;
DELETE FROM DrivingLicenceCategories;
DELETE FROM RequestTypes;
DELETE FROM PaymentTypes;
DELETE FROM PaymentProofTypes;
DELETE FROM VehicleOwnershipProofTypes;
DELETE FROM TechnicalExamVehicleParts;
DELETE FROM TechnicalExamVehiclePartCategories;
DELETE FROM TechnicalExamReportDetailStatuses;
DELETE FROM TechnicalExamTypes;
DELETE FROM TechnicalExamOrganizations;
DELETE FROM DDVCatalog;
DELETE FROM CustomerVehicleRelationTypes;
DELETE FROM Colors;
DELETE FROM VehicleTireTypes;
DELETE FROM VehicleUses;
DELETE FROM VehicleCategoriesForPayments;
DELETE FROM VehicleCategories;
DELETE FROM VehicleBodyTypes;
DELETE FROM VehicleSupportings;
DELETE FROM VehicleBrakes;
DELETE FROM VehicleGearBoxes;
DELETE FROM VehicleEngineEcoPrograms;
DELETE FROM VehicleEnginePowerSourceTypes;
DELETE FROM VehicleEngineTypes;
DELETE FROM VehicleModels;
DELETE FROM VehicleMakers;
DELETE FROM RegistrationIssuers;
DELETE FROM BusinessTypes;
DELETE FROM Countries;
PRINT '  Cleared.';
GO

-- =============================================================
-- Phase 3: Reference tables
-- =============================================================
PRINT '=== Phase 3: Reference tables ===';
DECLARE @StationId INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');

SET IDENTITY_INSERT Countries ON;
INSERT INTO Countries (Id, Name, Iso2, Iso3, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(CountryName)),
  CASE WHEN LEN(LTRIM(RTRIM(ISNULL(CountryShortName,'')))) <= 2 THEN LEFT(CountryShortName, 2) ELSE NULL END,
  CASE WHEN LEN(LTRIM(RTRIM(ISNULL(CountryShortName,'')))) = 3 THEN CountryShortName ELSE NULL END,
  Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.Countries WHERE LTRIM(RTRIM(ISNULL(CountryName,''))) <> '';
SET IDENTITY_INSERT Countries OFF;
PRINT CONCAT('  Countries: ', @@ROWCOUNT);

SET IDENTITY_INSERT BusinessTypes ON;
INSERT INTO BusinessTypes (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(COALESCE(NULLIF(BusinessTypeDescription,''), BusinessTypeCode))), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.BusinessTypes WHERE LTRIM(RTRIM(COALESCE(BusinessTypeDescription, BusinessTypeCode, ''))) <> '';
SET IDENTITY_INSERT BusinessTypes OFF;
PRINT CONCAT('  BusinessTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT Communities ON;
INSERT INTO Communities (Id, Name, CountryId, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(CommunityName)), NULL, Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.Communities WHERE LTRIM(RTRIM(ISNULL(CommunityName,''))) <> '';
SET IDENTITY_INSERT Communities OFF;
PRINT CONCAT('  Communities: ', @@ROWCOUNT);

SET IDENTITY_INSERT Cities ON;
INSERT INTO Cities (Id, Name, PostalCode, CommunityId, CountryId, Description, IsActive, CreatedUtc, LastModifiedUtc)
SELECT c.Id, LTRIM(RTRIM(c.CityName)),
  CASE WHEN c.CityZip IS NULL OR c.CityZip = 0 THEN NULL ELSE CAST(c.CityZip AS NVARCHAR(20)) END,
  CASE WHEN EXISTS(SELECT 1 FROM Communities co WHERE co.Id = c.IdCommunityCode) THEN c.IdCommunityCode ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM Countries co WHERE co.Id = c.IdCountry) THEN c.IdCountry ELSE NULL END,
  NULL, c.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.Cities c WHERE LTRIM(RTRIM(ISNULL(c.CityName,''))) <> '';
SET IDENTITY_INSERT Cities OFF;
PRINT CONCAT('  Cities: ', @@ROWCOUNT);

SET IDENTITY_INSERT Streets ON;
INSERT INTO Streets (Id, Name, CityId, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(StreetName)), NULL, Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.Streets WHERE LTRIM(RTRIM(ISNULL(StreetName,''))) <> '';
SET IDENTITY_INSERT Streets OFF;
PRINT CONCAT('  Streets: ', @@ROWCOUNT);

SET IDENTITY_INSERT RegistrationIssuers ON;
INSERT INTO RegistrationIssuers (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(IssuerName)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.RegistrationIssuers WHERE LTRIM(RTRIM(ISNULL(IssuerName,''))) <> '';
SET IDENTITY_INSERT RegistrationIssuers OFF;
PRINT CONCAT('  RegistrationIssuers: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleMakers ON;
INSERT INTO VehicleMakers (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(CompanyName)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleMakers WHERE LTRIM(RTRIM(ISNULL(CompanyName,''))) <> '';
SET IDENTITY_INSERT VehicleMakers OFF;
PRINT CONCAT('  VehicleMakers: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleModels ON;
INSERT INTO VehicleModels (Id, VehicleMakerId, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT m.Id, m.IdVehicleMaker,
  LTRIM(RTRIM(CASE WHEN LTRIM(RTRIM(ISNULL(m.ModelName,''))) <> '' THEN m.ModelName ELSE m.ModelCode END)),
  m.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleModel m
WHERE m.IdVehicleMaker IS NOT NULL
  AND EXISTS(SELECT 1 FROM VehicleMakers vm WHERE vm.Id = m.IdVehicleMaker)
  AND LTRIM(RTRIM(COALESCE(m.ModelName, m.ModelCode, ''))) <> '';
SET IDENTITY_INSERT VehicleModels OFF;
PRINT CONCAT('  VehicleModels: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleEngineTypes ON;
INSERT INTO VehicleEngineTypes (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(COALESCE(NULLIF(EngineTypeCode,''), TechincalDescription))), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleEngineTypes WHERE LTRIM(RTRIM(COALESCE(EngineTypeCode, TechincalDescription, ''))) <> '';
SET IDENTITY_INSERT VehicleEngineTypes OFF;
PRINT CONCAT('  VehicleEngineTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleEnginePowerSourceTypes ON;
INSERT INTO VehicleEnginePowerSourceTypes (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(PowerSourceName)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleEnginePowerSourceTypes WHERE LTRIM(RTRIM(ISNULL(PowerSourceName,''))) <> '';
SET IDENTITY_INSERT VehicleEnginePowerSourceTypes OFF;
PRINT CONCAT('  VehicleEnginePowerSourceTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleEngineEcoPrograms ON;
INSERT INTO VehicleEngineEcoPrograms (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(LEFT(COALESCE(NULLIF(EcoProgram,''), Code, ''), 200))), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleEngineEcoProgram WHERE LTRIM(RTRIM(COALESCE(EcoProgram, Code, ''))) <> '';
SET IDENTITY_INSERT VehicleEngineEcoPrograms OFF;
PRINT CONCAT('  VehicleEngineEcoPrograms: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleGearBoxes ON;
INSERT INTO VehicleGearBoxes (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(GearBoxDescription)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleGearBox WHERE LTRIM(RTRIM(ISNULL(GearBoxDescription,''))) <> '';
SET IDENTITY_INSERT VehicleGearBoxes OFF;
PRINT CONCAT('  VehicleGearBoxes: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleBrakes ON;
INSERT INTO VehicleBrakes (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(BreakesDescription)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleBrakes WHERE LTRIM(RTRIM(ISNULL(BreakesDescription,''))) <> '';
SET IDENTITY_INSERT VehicleBrakes OFF;
PRINT CONCAT('  VehicleBrakes: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleSupportings ON;
INSERT INTO VehicleSupportings (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(SupportingDescription)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleSupporting WHERE LTRIM(RTRIM(ISNULL(SupportingDescription,''))) <> '';
SET IDENTITY_INSERT VehicleSupportings OFF;
PRINT CONCAT('  VehicleSupportings: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleBodyTypes ON;
INSERT INTO VehicleBodyTypes (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(COALESCE(NULLIF(BodytypeDescriprion,''), BodytypeCode))), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleBodytype WHERE LTRIM(RTRIM(COALESCE(BodytypeDescriprion, BodytypeCode, ''))) <> '';
SET IDENTITY_INSERT VehicleBodyTypes OFF;
PRINT CONCAT('  VehicleBodyTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleCategories ON;
INSERT INTO VehicleCategories (Id, Name, Code, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(COALESCE(NULLIF(CategoryName,''), CategoryCode))), CategoryCode, Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleCategories WHERE LTRIM(RTRIM(COALESCE(CategoryName, CategoryCode, ''))) <> '';
SET IDENTITY_INSERT VehicleCategories OFF;
PRINT CONCAT('  VehicleCategories: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleCategoriesForPayments ON;
INSERT INTO VehicleCategoriesForPayments (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(COALESCE(NULLIF(Name,''), Code))), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleCategoryForPayments WHERE LTRIM(RTRIM(COALESCE(Name, Code, ''))) <> '';
SET IDENTITY_INSERT VehicleCategoriesForPayments OFF;
PRINT CONCAT('  VehicleCategoriesForPayments: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleUses ON;
INSERT INTO VehicleUses (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(UseDescription)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleUse WHERE LTRIM(RTRIM(ISNULL(UseDescription,''))) <> '';
SET IDENTITY_INSERT VehicleUses OFF;
PRINT CONCAT('  VehicleUses: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleTireTypes ON;
INSERT INTO VehicleTireTypes (Id, Name, Dimensions, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(COALESCE(NULLIF(TireType,''), Seria))),
  CASE WHEN Dimenzions IS NULL THEN NULL ELSE CAST(Dimenzions AS NVARCHAR(50)) END,
  Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.VehicleTireTypes WHERE LTRIM(RTRIM(COALESCE(TireType, Seria, ''))) <> '';
SET IDENTITY_INSERT VehicleTireTypes OFF;
PRINT CONCAT('  VehicleTireTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT Colors ON;
INSERT INTO Colors (Id, Name, HexCode, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(COALESCE(NULLIF(ColorDescription,''), ColorCode))), NULL, Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.Colors WHERE LTRIM(RTRIM(COALESCE(ColorDescription, ColorCode, ''))) <> '';
SET IDENTITY_INSERT Colors OFF;
PRINT CONCAT('  Colors: ', @@ROWCOUNT);

SET IDENTITY_INSERT CustomerVehicleRelationTypes ON;
INSERT INTO CustomerVehicleRelationTypes (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(RelationTypeName)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.CustomerVehiclesRelationTypes WHERE LTRIM(RTRIM(ISNULL(RelationTypeName,''))) <> '';
SET IDENTITY_INSERT CustomerVehicleRelationTypes OFF;
PRINT CONCAT('  CustomerVehicleRelationTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT DDVCatalog ON;
INSERT INTO DDVCatalog (Id, Name, Rate, EffectiveFrom, EffectiveTo, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(DDVName)), DDVValue, '2000-01-01', NULL, Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DDVCatalog WHERE LTRIM(RTRIM(ISNULL(DDVName,''))) <> '';
SET IDENTITY_INSERT DDVCatalog OFF;
PRINT CONCAT('  DDVCatalog: ', @@ROWCOUNT);

SET IDENTITY_INSERT DrivingLicenceCategories ON;
INSERT INTO DrivingLicenceCategories (Id, Name, Description, IsActive, CreatedUtc, LastModifiedUtc)
SELECT CAST(Id AS INT), LTRIM(RTRIM(COALESCE(NULLIF(Code,''), Description))), Description, Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DriveingLicenceCtegories WHERE LTRIM(RTRIM(COALESCE(Code, Description, ''))) <> '';
SET IDENTITY_INSERT DrivingLicenceCategories OFF;
PRINT CONCAT('  DrivingLicenceCategories: ', @@ROWCOUNT);

SET IDENTITY_INSERT TechnicalExamOrganizations ON;
INSERT INTO TechnicalExamOrganizations (Id, Name, Address, TaxNumber, Phone, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LEFT(LTRIM(RTRIM(Station)), 200), LEFT(StationAddress, 500), LEFT(LTRIM(RTRIM(EDB)), 15), LEFT(Tel, 50), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.TehnicalExamOrganizations WHERE LTRIM(RTRIM(ISNULL(Station,''))) <> '';
SET IDENTITY_INSERT TechnicalExamOrganizations OFF;
PRINT CONCAT('  TechnicalExamOrganizations: ', @@ROWCOUNT);

SET IDENTITY_INSERT TechnicalExamTypes ON;
INSERT INTO TechnicalExamTypes (Id, Name, ValidityMonths, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(COALESCE(NULLIF(Description,''), Code))),
  CASE WHEN ValidNumOfDays IS NULL OR ValidNumOfDays <= 0 THEN 12 ELSE ValidNumOfDays/30 END,
  Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.TehnicalExamsTypes WHERE LTRIM(RTRIM(COALESCE(Description, Code, ''))) <> '';
SET IDENTITY_INSERT TechnicalExamTypes OFF;
PRINT CONCAT('  TechnicalExamTypes: ', @@ROWCOUNT);

-- Tech Exam Vehicle Part Categories: legacy table is empty, seed minimal one
INSERT INTO TechnicalExamVehiclePartCategories (Name, SortOrder, IsActive, CreatedUtc, LastModifiedUtc)
VALUES (N'Општо', 1, 1, SYSUTCDATETIME(), SYSUTCDATETIME());
DECLARE @DefaultCatId INT = (SELECT TOP 1 Id FROM TechnicalExamVehiclePartCategories ORDER BY Id);

SET IDENTITY_INSERT TechnicalExamVehicleParts ON;
INSERT INTO TechnicalExamVehicleParts (Id, CategoryId, Name, SortOrder, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, @DefaultCatId, LTRIM(RTRIM(COALESCE(NULLIF(Description,''), Code))), Id, Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.TehnicalExamVehicleParts WHERE LTRIM(RTRIM(COALESCE(Description, Code, ''))) <> '';
SET IDENTITY_INSERT TechnicalExamVehicleParts OFF;
PRINT CONCAT('  TechnicalExamVehicleParts: ', @@ROWCOUNT);

SET IDENTITY_INSERT TechnicalExamReportDetailStatuses ON;
INSERT INTO TechnicalExamReportDetailStatuses (Id, Name, IsPass, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(StatusName)), 0, Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DocumentsTehnicalExamsReportsDetailsStatus WHERE LTRIM(RTRIM(ISNULL(StatusName,''))) <> '';
SET IDENTITY_INSERT TechnicalExamReportDetailStatuses OFF;
PRINT CONCAT('  TechnicalExamReportDetailStatuses: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleOwnershipProofTypes ON;
INSERT INTO VehicleOwnershipProofTypes (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(VehicleOwnershipProofName)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DocumentVehicleOwnershipProof WHERE LTRIM(RTRIM(ISNULL(VehicleOwnershipProofName,''))) <> '';
SET IDENTITY_INSERT VehicleOwnershipProofTypes OFF;
PRINT CONCAT('  VehicleOwnershipProofTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT PaymentProofTypes ON;
INSERT INTO PaymentProofTypes (Id, Name, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(PaymentProofName)), Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DocumentPaymentProof WHERE LTRIM(RTRIM(ISNULL(PaymentProofName,''))) <> '';
SET IDENTITY_INSERT PaymentProofTypes OFF;
PRINT CONCAT('  PaymentProofTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT PaymentTypes ON;
INSERT INTO PaymentTypes (Id, Name, IsInvoice, IsCash, IsFiscalCard, IsAccount, IsInstallments, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id, LTRIM(RTRIM(Name)),
  ISNULL(Faktura,0), ISNULL(Fiskalna_kes,0), ISNULL(Fiskalna_karticka,0), ISNULL(Smetka,0), ISNULL(Rati,0),
  Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.PaymentTypes WHERE LTRIM(RTRIM(ISNULL(Name,''))) <> '';
SET IDENTITY_INSERT PaymentTypes OFF;
PRINT CONCAT('  PaymentTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT RequestTypes ON;
INSERT INTO RequestTypes (Id, ParentRequestTypeId, DocumentPrintId, TypeName, TypeDescription,
  IsTechnicalExamRequired, IsPayRequired, IsNewRegistration, IsRelationDeleted,
  IsVehicleDeleted, IsNewCustomer, IsVehicleChanged, IsCustomerChanged,
  IsSufficient, IsPreviousRegistrationRequired, IsActive, CreatedUtc, LastModifiedUtc)
SELECT Id,
  CASE WHEN IdRequestType = Id OR IdRequestType IS NULL THEN NULL
       WHEN EXISTS(SELECT 1 FROM VTEZVV_Snapshot.dbo.RequestTypes p WHERE p.Id = IdRequestType) THEN IdRequestType ELSE NULL END,
  IdDocumentPrint, LTRIM(RTRIM(TypeName)), TypeDescription,
  CAST(ISNULL(IsTehnicalExamRequired,0) AS INT),
  ISNULL(IsPayRequired,0), ISNULL(IsNewRegistration,0), ISNULL(IsRelationDeleted,0),
  ISNULL(IsVehicleDeleted,0), ISNULL(IsNewCustomer,0), ISNULL(IsVehicleChanged,0),
  ISNULL(IsCustomerChanged,0), ISNULL(IsSufficient,0), ISNULL(IsPreviosRegistrationReqired,0),
  Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.RequestTypes WHERE LTRIM(RTRIM(ISNULL(TypeName,''))) <> '';
SET IDENTITY_INSERT RequestTypes OFF;
PRINT CONCAT('  RequestTypes: ', @@ROWCOUNT);

SET IDENTITY_INSERT CalculationItems ON;
INSERT INTO CalculationItems (Id, StationId, ItemName, BankAccount, Bank, Form, IsActive, CreatedUtc, LastModifiedUtc)
SELECT ci.Id, @StationId, LTRIM(RTRIM(ci.ItemName)), ci.BankAccount, ci.Bank, ci.Form, ci.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.CalculationItems ci WHERE LTRIM(RTRIM(ISNULL(ci.ItemName,''))) <> '';
SET IDENTITY_INSERT CalculationItems OFF;
PRINT CONCAT('  CalculationItems: ', @@ROWCOUNT);

SET IDENTITY_INSERT PriceCatalog ON;
INSERT INTO PriceCatalog (Id, StationId, CalculationItemId, Name, Price, DDVId, EffectiveFrom, EffectiveTo, IsActive, CreatedUtc, LastModifiedUtc)
SELECT pc.Id, @StationId, NULL, LTRIM(RTRIM(pc.Name)), pc.Price,
  CASE WHEN EXISTS(SELECT 1 FROM DDVCatalog d WHERE d.Id = pc.IdDDVCatalog) THEN pc.IdDDVCatalog ELSE NULL END,
  NULL, NULL, pc.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.PriceCatalog pc WHERE LTRIM(RTRIM(ISNULL(pc.Name,''))) <> '';
SET IDENTITY_INSERT PriceCatalog OFF;
PRINT CONCAT('  PriceCatalog: ', @@ROWCOUNT);

PRINT '=== Phase 3 done ===';
GO

-- =============================================================
-- Phase 4: Customers + dependents
-- =============================================================
PRINT '=== Phase 4: Customers ===';
DECLARE @StationId INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');

SET IDENTITY_INSERT Customers ON;
WITH dedup AS (
  SELECT *,
    ROW_NUMBER() OVER (PARTITION BY LTRIM(RTRIM(ISNULL(MB,''))) ORDER BY Id) AS rn,
    LEN(LTRIM(RTRIM(ISNULL(MB,'')))) AS mb_len
  FROM VTEZVV_Snapshot.dbo.Customers
)
INSERT INTO Customers (Id, StationId, IsCompany,
  EMBG, FirstName, Surname, ParentName, DateOfBirth, CitizenshipId, Occupation, WorksInCompany, BusinessTypeId,
  LivingAddressId, LivingAddressNumber, LivingCityId, BirthCityId, BirthAddressId, BirthAddressNumber,
  PhoneNumber, Fax, Email,
  IDCardNumber, IDCardDateIssued, IDCardIssuerId,
  PassportNumber, PassportDateIssued, PassportIssuerId,
  DrivingLicenceNumber, DrivingLicenceDateIssued, DrivingLicenceIssuerId,
  TaxNumber, CanSendNotifications, Status, Note, IsActive, CreatedUtc, LastModifiedUtc)
SELECT
  c.Id, @StationId, c.IsCompany,
  CASE WHEN c.rn = 1 AND c.mb_len > 0 THEN LEFT(LTRIM(RTRIM(c.MB)), 13) ELSE NULL END,
  ISNULL(LEFT(NULLIF(LTRIM(RTRIM(c.CustomerFirstName)),''), 100), '?'),
  LEFT(NULLIF(LTRIM(RTRIM(c.CustomerSurname)),''), 100),
  LEFT(NULLIF(LTRIM(RTRIM(c.ParentName)),''), 100),
  CASE WHEN c.DateOfBirth IS NULL THEN NULL ELSE CAST(c.DateOfBirth AS DATE) END,
  CASE WHEN EXISTS(SELECT 1 FROM Countries co WHERE co.Id = c.IdCitizenship) THEN c.IdCitizenship ELSE NULL END,
  LEFT(NULLIF(LTRIM(RTRIM(c.Occupation)),''), 50),
  LEFT(NULLIF(LTRIM(RTRIM(c.WorksInCompany)),''), 200),
  CASE WHEN EXISTS(SELECT 1 FROM BusinessTypes b WHERE b.Id = c.IdBusinessType) THEN c.IdBusinessType ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM Streets s WHERE s.Id = c.IdLivingAddress) THEN c.IdLivingAddress ELSE NULL END,
  LEFT(NULLIF(LTRIM(RTRIM(c.LivingAddressNumber)),''), 100),
  CASE WHEN EXISTS(SELECT 1 FROM Cities ci WHERE ci.Id = c.IdLivingCity) THEN c.IdLivingCity ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM Cities ci WHERE ci.Id = c.IdBirhCity) THEN c.IdBirhCity ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM Streets s WHERE s.Id = c.IdBirthAddress) THEN c.IdBirthAddress ELSE NULL END,
  LEFT(NULLIF(LTRIM(RTRIM(c.BrithAddressNumber)),''), 100),
  LEFT(NULLIF(LTRIM(RTRIM(c.PhoneNumber)),''), 20),
  LEFT(NULLIF(LTRIM(RTRIM(c.Fax)),''), 20),
  LEFT(NULLIF(LTRIM(RTRIM(c.eMail)),''), 200),
  LEFT(NULLIF(LTRIM(RTRIM(c.BLK)),''), 20),
  CASE WHEN c.BLKDateIssued IS NULL THEN NULL ELSE CAST(c.BLKDateIssued AS DATE) END,
  CASE WHEN EXISTS(SELECT 1 FROM RegistrationIssuers r WHERE r.Id = c.BLKIssuer) THEN c.BLKIssuer ELSE NULL END,
  LEFT(NULLIF(LTRIM(RTRIM(c.PassportNumber)),''), 20),
  CASE WHEN c.PassDateIssued IS NULL THEN NULL ELSE CAST(c.PassDateIssued AS DATE) END,
  CASE WHEN EXISTS(SELECT 1 FROM RegistrationIssuers r WHERE r.Id = c.PassIssuer) THEN c.PassIssuer ELSE NULL END,
  LEFT(NULLIF(LTRIM(RTRIM(c.DriveingLicenceNumber)),''), 20),
  CASE WHEN c.DriveingLicenceDateIssued IS NULL THEN NULL ELSE CAST(c.DriveingLicenceDateIssued AS DATE) END,
  CASE WHEN EXISTS(SELECT 1 FROM RegistrationIssuers r WHERE r.Id = c.DriveingLicenceIssuer) THEN c.DriveingLicenceIssuer ELSE NULL END,
  LEFT(NULLIF(LTRIM(RTRIM(c.TaxNumber)),''), 15),
  c.CanSendNotifications, LEFT(NULLIF(LTRIM(RTRIM(c.Status)),''), 50), c.Note,
  c.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM dedup c;
SET IDENTITY_INSERT Customers OFF;
PRINT CONCAT('  Customers: ', @@ROWCOUNT);

SET IDENTITY_INSERT CustomerBankAccounts ON;
INSERT INTO CustomerBankAccounts (Id, CustomerId, BankAccount, DeponentBank, TaxNumber, CreatedUtc, LastModifiedUtc)
SELECT b.Id, b.IdCustomer, LEFT(b.BankAccount, 50), LEFT(b.DeponentBank, 50), LEFT(b.TaxNumber, 15), SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.CustomersBankAccounts b
WHERE EXISTS(SELECT 1 FROM Customers c WHERE c.Id = b.IdCustomer);
SET IDENTITY_INSERT CustomerBankAccounts OFF;
PRINT CONCAT('  CustomerBankAccounts: ', @@ROWCOUNT);

SET IDENTITY_INSERT CustomerContactPersons ON;
INSERT INTO CustomerContactPersons (Id, CustomerId, EMBG, FirstName, Surname, PhoneNumber, MobileNumber, Email, CreatedUtc, LastModifiedUtc)
SELECT cp.Id, cp.IdCustomer, LEFT(cp.MB, 13), LEFT(ISNULL(cp.PersonName,'?'), 50), LEFT(ISNULL(cp.PersonSurname,'?'), 50),
  LEFT(cp.PhoneNumber, 20), LEFT(cp.MobileNumber, 20), LEFT(cp.Email, 200), SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.[Customers.ContactPersons] cp
WHERE EXISTS(SELECT 1 FROM Customers c WHERE c.Id = cp.IdCustomer);
SET IDENTITY_INSERT CustomerContactPersons OFF;
PRINT CONCAT('  CustomerContactPersons: ', @@ROWCOUNT);

PRINT '=== Phase 4 done ===';
GO

-- =============================================================
-- Phase 5: Vehicles + child tables
-- =============================================================
PRINT '=== Phase 5: Vehicles ===';
DECLARE @StationId INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');

SET IDENTITY_INSERT Vehicles ON;
;WITH vdup AS (
  SELECT *,
    ROW_NUMBER() OVER (PARTITION BY LEFT(ISNULL(NULLIF(LTRIM(RTRIM(ShellNumber)), ''), CONCAT('UNKNOWN-', Id)), 17) ORDER BY Id) AS rn
  FROM VTEZVV_Snapshot.dbo.Vehicles
)
INSERT INTO Vehicles (Id, StationId, ShellNumber,
  BodyTypeId, VehicleCategoryId, VehicleUseId, VehicleModelId, VehicleModelAdding, MadeCountryId, VehicleCategoryForPaymentsId,
  EngineNumber, EngineTypeId, EnginePowerSourceId, EngineSecondPowerSourceId, EngineEcoProgramId,
  EnginePowerKw, EnginePowerOutput, EngineTorque, EngineTorqueUnderGas, EngineWorkingCapacity, RPM, EngineIdentificationLocationMethod,
  GearBoxId, KwToCcRatio, BrakesId, SupportingId,
  VehicleHeight, VehicleWidth, VehicleLength,
  NumberOfDoors, NumberOfSeats, NumberOfStandingSeats, NumberOfLyingSeats,
  NumberOfAxes, NumberOfPropulsionAxes, NumberOfWheels, NumberOfPropulsionWheels,
  EmptyWeight, MaxAllowedWeight, MaxConstructiveTotalMass, MaxLegalTotalMass, MaxLegalTotalMassGroup,
  AxleLoad1, AxleLoad2, AxleLoad3, AxleLoad4, AxleLoad5, TrailerAxleLoad,
  AxleBaseLoad1, AxleBaseLoad2, AxleBaseLoad3, AxleBaseLoad4, AxleBaseLoad5, TrailerAxleBaseLoad,
  TrailerWeightBraked, TrailerWeightUnbraked,
  MaxConstructiveBrakedTrailerMass, MaxConstructiveUnbrakedTrailerMass,
  MaxConstructiveCouplingLoad, MaxConstructiveCombinationMass,
  MaxConstructiveSemiTrailerMass, MaxConstructiveTrailerMass,
  MaxConstructiveTrailerMassWithCentralAxle, MaxConstructiveAttachableTrailerMass,
  MaxHorizontalVerticalCouplingLoad, MinMass,
  MechanicalCouplingType, MechanicalCouplingMark, MechanicalCouplingApprovalNumber, CouplingDeviceApprovalMark,
  HomologationCertificateNumber, ApprovalMark, EUCertificateNumber, VariantImplementation, Type,
  MaxSpeed, TempOfEngineOil,
  NoiseStatic, NoiseMovement, NoiseTechnicalSpec,
  Co, Hc, NOx, HCNOx, Co2, Blackening, Pinpoints, FuelConsumption, CapacityFuelTank,
  ColorCode, PrimaryColorId, SecondaryColorId,
  MakeDate, FirstRegistrationNumber, LastRegistrationNumber,
  FirstRegistrationMakeDate, FirstRegistrationValidTill, LastRegistrationMakeDate, LastRegistrationValidTill,
  FirstRegistrationIssuerId, LastRegistrationIssuerId,
  Suffocation, Hook, Winch, VerticalBurdenOnTheSeat, VerticalBurdenOnTheSeatNote,
  TNG, ProtectiveCabin, ProtectiveFrame,
  IsSocialNotPrivate, ForPrivateTransportNotPublic,
  Note, IsActive, CreatedUtc, LastModifiedUtc)
SELECT
  v.Id, @StationId,
  CASE WHEN v.rn = 1 THEN LEFT(ISNULL(NULLIF(LTRIM(RTRIM(v.ShellNumber)),''), CONCAT('UNKNOWN-', v.Id)), 17)
       ELSE LEFT(CONCAT('DUP-', v.Id), 17) END,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleBodyTypes bt WHERE bt.Id = v.IdVehicleBodyType) THEN v.IdVehicleBodyType ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleCategories vc WHERE vc.Id = v.IdVehicleCategories) THEN v.IdVehicleCategories ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleUses vu WHERE vu.Id = v.IdVehicleUse) THEN v.IdVehicleUse ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleModels vm WHERE vm.Id = v.IdVehicleModel) THEN v.IdVehicleModel ELSE NULL END,
  LEFT(v.VehicleModelAdding, 200),
  CASE WHEN EXISTS(SELECT 1 FROM Countries co WHERE co.Id = v.IdMadeCountry) THEN v.IdMadeCountry ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleCategoriesForPayments cf WHERE cf.Id = v.IdVehicleCategoryForPayments) THEN v.IdVehicleCategoryForPayments ELSE NULL END,
  LEFT(v.EngineNumber, 50),
  CASE WHEN EXISTS(SELECT 1 FROM VehicleEngineTypes et WHERE et.Id = v.IdEngineType) THEN v.IdEngineType ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleEnginePowerSourceTypes ps WHERE ps.Id = v.IdEnginePowerSource) THEN v.IdEnginePowerSource ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleEnginePowerSourceTypes ps WHERE ps.Id = v.IdEngineSecondPowerSource) THEN v.IdEngineSecondPowerSource ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleEngineEcoPrograms ep WHERE ep.Id = v.IdEngineEcoProgram) THEN v.IdEngineEcoProgram ELSE NULL END,
  TRY_CAST(v.EnginePower AS DECIMAL(10,2)), TRY_CAST(v.EnginePowerOutPut AS DECIMAL(10,2)), LEFT(v.EngineTorque, 50), TRY_CAST(v.EngineTorqueUnderGass AS DECIMAL(10,2)), TRY_CAST(v.EngineWorkingCapacity AS DECIMAL(10,2)), v.BrojNaVrtezi, LEFT(v.IdentifikacijaNaMotorMestoMetod, 200),
  CASE WHEN EXISTS(SELECT 1 FROM VehicleGearBoxes gb WHERE gb.Id = v.IdGearBox) THEN v.IdGearBox ELSE NULL END,
  LEFT(v.OdnosKwCcm, 50),
  CASE WHEN EXISTS(SELECT 1 FROM VehicleBrakes b WHERE b.Id = v.IdBreakes) THEN v.IdBreakes ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleSupportings s WHERE s.Id = v.IdSupporting) THEN v.IdSupporting ELSE NULL END,
  TRY_CAST(v.VehicleSizeHight AS DECIMAL(10,2)), TRY_CAST(v.VehicleSizeWidth AS DECIMAL(10,2)), TRY_CAST(v.VehicleSizeLength AS DECIMAL(10,2)),
  v.NumberOfDoors, v.NumberOfSeats, v.NumberOfStandingSeats, v.NumberOfLieingSeats,
  v.NumberOfAxis, v.PropulsionAxis, v.NumberOfWheels, v.NumberOfPropulsionWheels,
  TRY_CAST(v.EmptyWaight AS DECIMAL(10,2)), TRY_CAST(v.MaximunAllowedWaight AS DECIMAL(10,2)), TRY_CAST(v.MaxKonstVkMasa AS DECIMAL(10,2)), TRY_CAST(v.MaxLegVkMasa AS DECIMAL(10,2)), TRY_CAST(v.MaxLegVkMasaGrupa AS DECIMAL(10,2)),
  v.MasaPoOska1, v.MasaPoOska2, v.MasaPoOska3, v.MasaPoOska4, v.MasaPoOska5, v.MasaPoOskaPriklucna,
  v.OsnoOptovaruvanje1, v.OsnoOptovaruvanje2, v.OsnoOptovaruvanje3, v.OsnoOptovaruvanje4, v.OsnoOptovaruvanje5, v.OsnoOptovaruvanjePriklucna,
  LEFT(v.TrailerWaightWithBreak, 20), LEFT(v.TrailerWaightWithoutBreak, 20),
  v.MaxKonstVkMasaKocnaPrikolka, v.MaxKonstVkMasaNeKocnaPrikolka,
  v.MaxKonstOptovaruvanjeVoPriklucok, v.TMaxKonstVkMasaNaKombinacija,
  v.TMaxKonstVkMasaPoluprikolka, v.TMaxKonstVkMasaPrikolka,
  v.TMaxKonstVkMasaPrikolkaSoCenOska, v.TMaxKonstVkMasaPrikolkaStoMozePrikluci,
  v.TMaxHorVerOptovaruvanjePriklucok, v.TMinMasa,
  LEFT(v.TTipMehanPriklucok, 100), LEFT(v.TMarkaMehanPriklucok, 100), LEFT(v.TBrOdobrenieMehanPriklucok, 100), LEFT(v.OznakaNaOdobrenieZaPriklucUred, 100),
  LEFT(v.HologationSertificateNumber, 100), LEFT(v.OznakaNaOdobrenie, 100), LEFT(v.BrojEUPotvrda, 100), LEFT(v.VarijantaIzvedba, 100), LEFT(v.Tip, 100),
  TRY_CAST(v.MaxSpeed AS DECIMAL(6,2)), TRY_CAST(v.TempOfEngineOil AS DECIMAL(6,2)),
  TRY_CAST(v.NoiseStatic AS DECIMAL(6,2)), TRY_CAST(v.NoiseMovment AS DECIMAL(6,2)), LEFT(v.NoiseTechnicalSpec, 50),
  TRY_CAST(v.CO AS DECIMAL(6,3)), TRY_CAST(v.HC AS DECIMAL(6,3)), TRY_CAST(v.NOx AS DECIMAL(6,3)), TRY_CAST(v.HCNOx AS DECIMAL(6,3)), TRY_CAST(v.CO2 AS DECIMAL(6,3)), LEFT(v.Blackening, 20), LEFT(v.Pinpoints, 20), LEFT(v.FuelConsumption, 20), TRY_CAST(v.CapacityFuelTank AS DECIMAL(8,2)),
  LEFT(v.ColorCode, 20),
  CASE WHEN EXISTS(SELECT 1 FROM Colors c WHERE c.Id = v.IdPrimaryColor) THEN v.IdPrimaryColor ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM Colors c WHERE c.Id = v.IdSecondaryColor) THEN v.IdSecondaryColor ELSE NULL END,
  CAST(v.MakeDate AS DATE),
  LEFT(ISNULL(NULLIF(LTRIM(RTRIM(v.FirstRegistrationNumber)),''), ''), 50),
  LEFT(ISNULL(NULLIF(LTRIM(RTRIM(v.LastRegistratinNumber)),''), ''), 50),
  CASE WHEN v.FirstRegistrationMakeDate IS NULL THEN NULL ELSE CAST(v.FirstRegistrationMakeDate AS DATE) END,
  CASE WHEN v.FirstRegistrationValidTill IS NULL THEN NULL ELSE CAST(v.FirstRegistrationValidTill AS DATE) END,
  CASE WHEN v.LastRegistrationMakeDate IS NULL THEN NULL ELSE CAST(v.LastRegistrationMakeDate AS DATE) END,
  CASE WHEN v.LastRegistrationValidTill IS NULL THEN NULL ELSE CAST(v.LastRegistrationValidTill AS DATE) END,
  CASE WHEN EXISTS(SELECT 1 FROM RegistrationIssuers r WHERE r.Id = v.IdFirstRegistrationIssuer) THEN v.IdFirstRegistrationIssuer ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM RegistrationIssuers r WHERE r.Id = v.IdLastRegistrationIssuer) THEN v.IdLastRegistrationIssuer ELSE NULL END,
  ISNULL(v.Suffocation,0), ISNULL(v.Hook,0), ISNULL(v.Vitlo,0), ISNULL(v.VerticalBurdenOnTheSeat,0), LEFT(v.VerticalBurdenOnTheSeatNote, 200),
  ISNULL(v.TNG,0), LEFT(v.TZastitnaKabina, 50), LEFT(v.TZastitnaRamka, 50),
  ISNULL(v.IsSocialNotPrivate,0), ISNULL(v.ForPrivateTransportNotPublic,0),
  LEFT(v.Note, 500), v.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM vdup v;
SET IDENTITY_INSERT Vehicles OFF;
PRINT CONCAT('  Vehicles: ', @@ROWCOUNT);

-- VehicleAxles: legacy(AxisNumber, CarryingCapacity, AxisLength) → modern(AxleNumber, IsPropulsion=0, IsSteering=0, Note='cap=X, len=Y')
;WITH adup AS (
  SELECT *, ROW_NUMBER() OVER (PARTITION BY IdVehicle, AxisNumber ORDER BY Id) AS rn
  FROM VTEZVV_Snapshot.dbo.[Vehicle.Axis]
)
INSERT INTO VehicleAxles (VehicleId, AxleNumber, IsPropulsion, IsSteering, Note, CreatedUtc, LastModifiedUtc)
SELECT a.IdVehicle, a.AxisNumber, 0, 0,
  CONCAT(N'capacity=', CAST(a.CarryingCapacity AS NVARCHAR(50)),
         CASE WHEN a.AxisLength IS NULL THEN '' ELSE CONCAT(', length=', CAST(a.AxisLength AS NVARCHAR(50))) END),
  SYSUTCDATETIME(), SYSUTCDATETIME()
FROM adup a
WHERE a.rn = 1
  AND EXISTS(SELECT 1 FROM Vehicles v WHERE v.Id = a.IdVehicle);
PRINT CONCAT('  VehicleAxles: ', @@ROWCOUNT);

-- VehicleAxleDistances: parse FromTo "1-2" -> FromAxleNumber, ToAxleNumber
SET IDENTITY_INSERT VehicleAxleDistances ON;
INSERT INTO VehicleAxleDistances (Id, VehicleId, FromAxleNumber, ToAxleNumber, Distance, CreatedUtc, LastModifiedUtc)
SELECT b.Id, b.IdVehicle,
  TRY_CAST(LEFT(b.FromTo, CHARINDEX('-', b.FromTo)-1) AS INT),
  TRY_CAST(SUBSTRING(b.FromTo, CHARINDEX('-', b.FromTo)+1, 10) AS INT),
  TRY_CAST(b.Destination AS DECIMAL(8,2)),
  SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.[Vehicle.BetweenAxesDestinations] b
WHERE EXISTS(SELECT 1 FROM Vehicles v WHERE v.Id = b.IdVehicle)
  AND CHARINDEX('-', ISNULL(b.FromTo,'')) > 0;
SET IDENTITY_INSERT VehicleAxleDistances OFF;
PRINT CONCAT('  VehicleAxleDistances: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleTyres ON;
INSERT INTO VehicleTyres (Id, VehicleId, TireTypeId, PositionNote, Dimensions, PressureFront, PressureRear, CreatedUtc, LastModifiedUtc)
SELECT t.Id, t.IdVehicle,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleTireTypes tt WHERE tt.Id = t.IdTireType) THEN t.IdTireType ELSE NULL END,
  NULL, NULL, NULL, NULL, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.[Vehicle.Tyres] t
WHERE EXISTS(SELECT 1 FROM Vehicles v WHERE v.Id = t.IdVehicle);
SET IDENTITY_INSERT VehicleTyres OFF;
PRINT CONCAT('  VehicleTyres: ', @@ROWCOUNT);

SET IDENTITY_INSERT VehicleRegistrations ON;
INSERT INTO VehicleRegistrations (Id, VehicleId, RegistrationNumber, MakeDate, ValidTill, IssuerId, CreatedUtc, LastModifiedUtc)
SELECT r.Id, r.IdVehicle, LEFT(r.RegistrationNumber, 50),
  CAST(r.DateOfRegistration AS DATE), CAST(r.DateRegistrationValidTill AS DATE),
  CASE WHEN EXISTS(SELECT 1 FROM RegistrationIssuers ri WHERE ri.Id = r.IdRegistrationIssuer) THEN r.IdRegistrationIssuer ELSE NULL END,
  SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.[Vehicle.Registrations] r
WHERE EXISTS(SELECT 1 FROM Vehicles v WHERE v.Id = r.IdVehicle);
SET IDENTITY_INSERT VehicleRegistrations OFF;
PRINT CONCAT('  VehicleRegistrations: ', @@ROWCOUNT);

PRINT '=== Phase 5 done ===';
GO

-- =============================================================
-- Phase 6: CustomerVehicleRelations + Requests
-- =============================================================
PRINT '=== Phase 6: Relations + Requests ===';
DECLARE @StationId INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');

-- CustomerVehicleRelations: VehicleId NOT NULL in modern; skip rows with no vehicle
SET IDENTITY_INSERT CustomerVehicleRelations ON;
INSERT INTO CustomerVehicleRelations (Id, CustomerId, VehicleId, RelationTypeId, ValidFrom, ValidTo, Note, IsActive, CreatedUtc, LastModifiedUtc)
SELECT r.Id, r.IdCustomer, r.IdVehicle,
  CASE WHEN EXISTS(SELECT 1 FROM CustomerVehicleRelationTypes t WHERE t.Id = r.IdRelationType) THEN r.IdRelationType ELSE NULL END,
  CAST(r.StartDate AS DATE),
  CASE WHEN r.EndDate IS NULL THEN NULL ELSE CAST(r.EndDate AS DATE) END,
  LEFT(COALESCE(r.BeginNote, r.TerminationNote), 500),
  r.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.CustomerVehiclesRelations r
WHERE r.IdVehicle IS NOT NULL
  AND EXISTS(SELECT 1 FROM Customers c WHERE c.Id = r.IdCustomer)
  AND EXISTS(SELECT 1 FROM Vehicles v WHERE v.Id = r.IdVehicle);
SET IDENTITY_INSERT CustomerVehicleRelations OFF;
PRINT CONCAT('  CustomerVehicleRelations: ', @@ROWCOUNT);

-- Requests
SET IDENTITY_INSERT Requests ON;
INSERT INTO Requests (Id, StationId, RequestTypeId, CustomerVehicleRelationId, NewCustomerVehicleRelationId,
  TechnicalExamReportId, PreviousRegistrationId, TechnicalExamOrganizationId,
  DateCreated, DateModified, DateEnded,
  CreatedByOperatorUserId, ModifiedByOperatorUserId, EndedByOperatorUserId,
  IsCustomerChanged, IsVehicleChanged, Note, CreatedUtc, LastModifiedUtc)
SELECT r.Id, @StationId,
  CASE WHEN EXISTS(SELECT 1 FROM RequestTypes rt WHERE rt.Id = r.IdRequestType) THEN r.IdRequestType
       ELSE (SELECT TOP 1 Id FROM RequestTypes ORDER BY Id) END,
  r.IdCustomerVehicleRelation,
  CASE WHEN r.IdCustomerVehicleRelationNew IS NULL THEN NULL
       WHEN EXISTS(SELECT 1 FROM CustomerVehicleRelations cvr WHERE cvr.Id = r.IdCustomerVehicleRelationNew) THEN r.IdCustomerVehicleRelationNew ELSE NULL END,
  NULL, -- TechnicalExamReportId fixed up later
  CASE WHEN r.IdPreviousRegistration = 0 THEN NULL ELSE CAST(r.IdPreviousRegistration AS BIGINT) END,
  CASE WHEN EXISTS(SELECT 1 FROM TechnicalExamOrganizations teo WHERE teo.Id = r.IdOrganisation) THEN r.IdOrganisation ELSE NULL END,
  CAST(r.DateCreated AS DATE),
  CASE WHEN r.DateModified IS NULL THEN NULL ELSE CAST(r.DateModified AS DATE) END,
  CASE WHEN r.DateEnded IS NULL THEN NULL ELSE CAST(r.DateEnded AS DATE) END,
  NULL, NULL, NULL,
  ISNULL(r.IsCustomerChanged,0), ISNULL(r.IsVehicleChanged,0),
  LEFT(r.Note, 500), SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.Requests r
WHERE EXISTS(SELECT 1 FROM CustomerVehicleRelations cvr WHERE cvr.Id = r.IdCustomerVehicleRelation);
SET IDENTITY_INSERT Requests OFF;
PRINT CONCAT('  Requests: ', @@ROWCOUNT);

-- RequestPaymentProofs (legacy "Request.PaymentProof")
SET IDENTITY_INSERT RequestPaymentProofs ON;
INSERT INTO RequestPaymentProofs (Id, RequestId, PaymentProofId, Number, Amount, DateIssued, Note, CreatedUtc, LastModifiedUtc)
SELECT p.Id, p.IdRequest,
  CASE WHEN EXISTS(SELECT 1 FROM PaymentProofTypes ppt WHERE ppt.Id = p.IdPaymentProof) THEN p.IdPaymentProof ELSE NULL END,
  LEFT(p.PaymentProof, 50), NULL, NULL, NULL, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.[Request.PaymentProof] p
WHERE EXISTS(SELECT 1 FROM Requests r WHERE r.Id = p.IdRequest);
SET IDENTITY_INSERT RequestPaymentProofs OFF;
PRINT CONCAT('  RequestPaymentProofs: ', @@ROWCOUNT);

-- RequestVehicleOwnershipProofs (legacy "Request.VehicleOwnershipProofs")
SET IDENTITY_INSERT RequestVehicleOwnershipProofs ON;
INSERT INTO RequestVehicleOwnershipProofs (Id, RequestId, OwnershipProofId, Number, DateIssued, Note, CreatedUtc, LastModifiedUtc)
SELECT p.Id, p.IdRequest,
  CASE WHEN EXISTS(SELECT 1 FROM VehicleOwnershipProofTypes vot WHERE vot.Id = p.IdVehicleOwnershipProof) THEN p.IdVehicleOwnershipProof ELSE NULL END,
  LEFT(p.VehicleOwnershipProof, 50), NULL, NULL, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.[Request.VehicleOwnershipProofs] p
WHERE EXISTS(SELECT 1 FROM Requests r WHERE r.Id = p.IdRequest);
SET IDENTITY_INSERT RequestVehicleOwnershipProofs OFF;
PRINT CONCAT('  RequestVehicleOwnershipProofs: ', @@ROWCOUNT);

PRINT '=== Phase 6 done ===';
GO

-- =============================================================
-- Phase 7: Tech Exams + Traffic Licences + Permissions + IDL
-- =============================================================
PRINT '=== Phase 7: Exams & Licences ===';
DECLARE @StationId INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');
DECLARE @AdminUserId NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetUsers ORDER BY Id);

-- TechnicalExamReports — FirstInspectorOperatorUserId is NOT NULL → use admin user GUID for all rows
SET IDENTITY_INSERT TechnicalExamReports ON;
INSERT INTO TechnicalExamReports (Id, StationId, CustomerVehicleRelationId, TechnicalExamTypeId,
  RegNumber, MadeDate, ValidTillDate, OrganizationForTechnicalExamId,
  FirstInspectorOperatorUserId, SecondInspectorOperatorUserId,
  VehicleIsRight, ExplanationNote, DriversWarning, Note,
  Axis1Left, Axis1Right, Axis1Gj, Axis1LeftRightDiff, Axis1Coefficient,
  Axis2Left, Axis2Right, Axis2Gj, Axis2LeftRightDiff, Axis2Coefficient,
  Axis3Left, Axis3Right, Axis3Gj, Axis3LeftRightDiff, Axis3Coefficient,
  Axis4Left, Axis4Right, Axis4Gj, Axis4LeftRightDiff, Axis4Coefficient,
  AxisParkingLeft, AxisParkingRight, AxisParkingGj, AxisParkingLeftRightDiff, AxisParkingCoefficient,
  Weight,
  EffectOfWorkingBrakeEmpty, EffectOfWorkingBrakeFull, EffectOfSecondaryBrake, EffectOfParkingBrake,
  SpeedOfTurns, CO, NumEngineTurns, COPlusTurns, Lambda, Pinpoints, Noise, TempOfEngineOil,
  TechnicalChanges, CreatedUtc, LastModifiedUtc)
SELECT
  r.Id, @StationId, r.IdCustomerVehicleRelation,
  CASE WHEN EXISTS(SELECT 1 FROM TechnicalExamTypes tt WHERE tt.Id = r.IdTypeOfTehnicalExam) THEN r.IdTypeOfTehnicalExam
       ELSE (SELECT TOP 1 Id FROM TechnicalExamTypes ORDER BY Id) END,
  LEFT(r.RegNumber, 50), CAST(r.MadeDate AS DATE), CAST(r.ValidTillDate AS DATE),
  CASE WHEN EXISTS(SELECT 1 FROM TechnicalExamOrganizations teo WHERE teo.Id = r.IdOrganizationForTehnicalExam) THEN r.IdOrganizationForTehnicalExam
       ELSE (SELECT TOP 1 Id FROM TechnicalExamOrganizations ORDER BY Id) END,
  @AdminUserId, NULL,
  r.VehicleIsRight, r.ExplanationNote, r.DriversWarning, LEFT(r.Note, 250),
  CAST(r.Axis1Left AS DECIMAL(18,3)), CAST(r.Axis1Right AS DECIMAL(18,3)), CAST(r.Axis1Gj AS DECIMAL(18,3)), CAST(r.Axis1LeftPj AS DECIMAL(18,3)), CAST(r.Axis1PN AS DECIMAL(18,3)),
  CAST(r.Axis2Left AS DECIMAL(18,3)), CAST(r.Axis2Right AS DECIMAL(18,3)), CAST(r.Axis2Gj AS DECIMAL(18,3)), CAST(r.Axis2LeftPj AS DECIMAL(18,3)), CAST(r.Axis2PN AS DECIMAL(18,3)),
  CAST(r.Axis3Left AS DECIMAL(18,3)), CAST(r.Axis3Right AS DECIMAL(18,3)), CAST(r.Axis3Gj AS DECIMAL(18,3)), CAST(r.Axis3LeftPj AS DECIMAL(18,3)), CAST(r.Axis3PN AS DECIMAL(18,3)),
  CAST(r.Axis4Left AS DECIMAL(18,3)), CAST(r.Axis4Right AS DECIMAL(18,3)), CAST(r.Axis4Gj AS DECIMAL(18,3)), CAST(r.Axis4LeftPj AS DECIMAL(18,3)), CAST(r.Axis4PN AS DECIMAL(18,3)),
  CAST(r.AxisParkingLeft AS DECIMAL(18,3)), CAST(r.AxisParkingRight AS DECIMAL(18,3)), CAST(r.AxisParkingGj AS DECIMAL(18,3)), CAST(r.AxisParkingLeftPj AS DECIMAL(18,3)), CAST(r.AxisParkingPN AS DECIMAL(18,3)),
  CAST(r.Waight AS DECIMAL(18,3)),
  CAST(r.EffectOfWorkingBreakEmpty AS DECIMAL(18,3)), CAST(r.EffectOfWorkingBreakFull AS DECIMAL(18,3)),
  CAST(r.EffectOfSecondaryBreak AS DECIMAL(18,3)), CAST(r.EffectOfParkingBreak AS DECIMAL(18,3)),
  CAST(r.SpeedOfTurns AS DECIMAL(18,3)), CAST(r.CO AS DECIMAL(18,3)),
  CAST(r.NumEngineTurns AS DECIMAL(18,3)), CAST(r.COPlusTurns AS DECIMAL(18,3)),
  CAST(r.Lambda AS DECIMAL(18,3)), CAST(r.Pinpoints AS DECIMAL(18,3)),
  CAST(r.Noise AS DECIMAL(18,3)), CAST(r.TempOfEngineOil AS DECIMAL(18,3)),
  LEFT(r.TechnicalChanges, 500), SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DocumentsTehnicalExamsReports r
WHERE EXISTS(SELECT 1 FROM CustomerVehicleRelations cvr WHERE cvr.Id = r.IdCustomerVehicleRelation);
SET IDENTITY_INSERT TechnicalExamReports OFF;
PRINT CONCAT('  TechnicalExamReports: ', @@ROWCOUNT);

-- TechnicalExamReportDetails
SET IDENTITY_INSERT TechnicalExamReportDetails ON;
INSERT INTO TechnicalExamReportDetails (Id, TechnicalExamReportId, TechnicalExamVehiclePartId, StatusId,
  Front, Back, OnLeft, OnRight, DateEnter, Note, CreatedUtc, LastModifiedUtc)
SELECT d.Id, d.IdTehnicalExamsReports, d.IdTehnicalExamVehivlePart, d.IdStatus,
  d.Front, d.Back, d.OnLeft, d.OnRight, CAST(d.DateEnter AS DATE), LEFT(d.Note, 150), SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DocumentsTehnicalExamsReportsDetails d
WHERE EXISTS(SELECT 1 FROM TechnicalExamReports r WHERE r.Id = d.IdTehnicalExamsReports)
  AND EXISTS(SELECT 1 FROM TechnicalExamVehicleParts p WHERE p.Id = d.IdTehnicalExamVehivlePart)
  AND EXISTS(SELECT 1 FROM TechnicalExamReportDetailStatuses s WHERE s.Id = d.IdStatus);
SET IDENTITY_INSERT TechnicalExamReportDetails OFF;
PRINT CONCAT('  TechnicalExamReportDetails: ', @@ROWCOUNT);

-- Backfill Requests.TechnicalExamReportId now that exams are loaded
UPDATE r SET r.TechnicalExamReportId = lr.IdTechnicalExamReport
FROM Requests r
JOIN VTEZVV_Snapshot.dbo.Requests lr ON lr.Id = r.Id
WHERE lr.IdTechnicalExamReport IS NOT NULL
  AND EXISTS(SELECT 1 FROM TechnicalExamReports e WHERE e.Id = lr.IdTechnicalExamReport);

-- TrafficLicences: EndDate NOT NULL — coalesce missing to 9999-12-31
SET IDENTITY_INSERT TrafficLicences ON;
INSERT INTO TrafficLicences (Id, StationId, CustomerVehicleRelationId, IssuingOrganizationId,
  TrafficLicenceNumber, MadeDate, EndDate, Note, IsActive, CreatedUtc, LastModifiedUtc)
SELECT t.Id, @StationId, t.IdCustomerVehicleRelation,
  CASE WHEN EXISTS(SELECT 1 FROM TechnicalExamOrganizations teo WHERE teo.Id = t.IdTehnicalExamOrganizationsIssuedBy) THEN t.IdTehnicalExamOrganizationsIssuedBy ELSE NULL END,
  LEFT(t.TrafficLicenceNumber, 50), CAST(t.MadeDate AS DATE),
  COALESCE(CAST(t.EndDate AS DATE), '9999-12-31'),
  LEFT(t.Note, 250), t.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DocumentsTrafficLicences t
WHERE EXISTS(SELECT 1 FROM CustomerVehicleRelations cvr WHERE cvr.Id = t.IdCustomerVehicleRelation);
SET IDENTITY_INSERT TrafficLicences OFF;
PRINT CONCAT('  TrafficLicences: ', @@ROWCOUNT);

-- TrafficLicenceExtensions: ExtensionDate + ValidTill NOT NULL → drop rows without ValidTill
SET IDENTITY_INSERT TrafficLicenceExtensions ON;
INSERT INTO TrafficLicenceExtensions (Id, TrafficLicenceId, ExtensionDate, ValidTill, Note, CreatedUtc, LastModifiedUtc)
SELECT e.Id, e.IdTrafficLicence,
  CAST(SYSUTCDATETIME() AS DATE), -- legacy didn't track extension date; default to today
  CAST(e.ValidTill AS DATE),
  LEFT(e.Note, 250), SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.[DocumentsTrafficLicences.Extensions] e
WHERE e.ValidTill IS NOT NULL
  AND EXISTS(SELECT 1 FROM TrafficLicences t WHERE t.Id = e.IdTrafficLicence);
SET IDENTITY_INSERT TrafficLicenceExtensions OFF;
PRINT CONCAT('  TrafficLicenceExtensions: ', @@ROWCOUNT);

-- Permissions: simplified schema; pack legacy extras (TrafficLicenceNumber, TriptiqueNumber, IssuingCity, etc.) into Note
SET IDENTITY_INSERT Permissions ON;
INSERT INTO Permissions (Id, StationId, CustomerVehicleRelationId, PermissionNumber, PermissionTypeId,
  MadeDate, EndDate, Note, IsActive, CreatedUtc, LastModifiedUtc)
SELECT p.Id, @StationId, p.IdCustomerVehicleRelation,
  LEFT(p.PermissionNumber, 50), NULL,
  CAST(p.DateCreated AS DATE),
  CAST(p.ValidTillDate AS DATE),
  LEFT(CONCAT(
    CASE WHEN p.Note IS NULL THEN '' ELSE CONCAT(p.Note, N' | ') END,
    CASE WHEN p.TrafficLicenceNumber IS NULL THEN '' ELSE CONCAT(N'СД: ', p.TrafficLicenceNumber, N' | ') END,
    CASE WHEN p.TriptiqueNumber IS NULL THEN '' ELSE CONCAT(N'Триптик: ', p.TriptiqueNumber) END
  ), 500),
  p.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DocumentsPermisions p
WHERE EXISTS(SELECT 1 FROM CustomerVehicleRelations cvr WHERE cvr.Id = p.IdCustomerVehicleRelation);
SET IDENTITY_INSERT Permissions OFF;
PRINT CONCAT('  Permissions: ', @@ROWCOUNT);

-- InternationalDrivingLicences (LicenceNumber + IssuedDate + ValidTill NOTNULL)
SET IDENTITY_INSERT InternationalDrivingLicences ON;
INSERT INTO InternationalDrivingLicences (Id, StationId, CustomerId, LicenceNumber,
  IssuedDate, ValidTill, IssuerId, Note, IsActive, CreatedUtc, LastModifiedUtc)
SELECT i.Id, @StationId, i.IdCustomer,
  LEFT(ISNULL(NULLIF(LTRIM(RTRIM(i.NumberOfLicence)),''), CONCAT('LEGACY-', i.Id)), 50),
  CAST(i.DateCreated AS DATE), CAST(i.ValidTillDate AS DATE),
  CASE WHEN EXISTS(SELECT 1 FROM RegistrationIssuers r WHERE r.Id = i.IdIssuer) THEN i.IdIssuer ELSE NULL END,
  LEFT(CONCAT(
    CASE WHEN i.Note IS NULL THEN '' ELSE CONCAT(i.Note, N' | ') END,
    CASE WHEN i.NumberOfNationalLicence IS NULL THEN '' ELSE CONCAT(N'Нац. возачка: ', i.NumberOfNationalLicence) END
  ), 250),
  i.Active, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DocumentsInternationalDriveingLicences i
WHERE EXISTS(SELECT 1 FROM Customers c WHERE c.Id = i.IdCustomer);
SET IDENTITY_INSERT InternationalDrivingLicences OFF;
PRINT CONCAT('  InternationalDrivingLicences: ', @@ROWCOUNT);

-- InternationalDrivingLicenceCategories (junction)
SET IDENTITY_INSERT InternationalDrivingLicenceCategories ON;
INSERT INTO InternationalDrivingLicenceCategories (Id, InternationalDrivingLicenceId, DrivingLicenceCategoryId, CreatedUtc, LastModifiedUtc)
SELECT v.Id, v.IdInternationalDrivingLicence, v.IdLicenceCategorie, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.[DocumentsInternationalDriveingLicences.ValidForCategories] v
WHERE v.IsCheck = 1
  AND EXISTS(SELECT 1 FROM InternationalDrivingLicences i WHERE i.Id = v.IdInternationalDrivingLicence)
  AND EXISTS(SELECT 1 FROM DrivingLicenceCategories d WHERE d.Id = v.IdLicenceCategorie);
SET IDENTITY_INSERT InternationalDrivingLicenceCategories OFF;
PRINT CONCAT('  InternationalDrivingLicenceCategories: ', @@ROWCOUNT);

PRINT '=== Phase 7 done ===';
GO

-- =============================================================
-- Phase 8: Payments + InstallmentContracts
-- =============================================================
PRINT '=== Phase 8: Payments ===';
DECLARE @StationId INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');

-- InstallmentContracts (DogovorZaRati)
SET IDENTITY_INSERT InstallmentContracts ON;
;WITH dzr AS (
  SELECT *, ROW_NUMBER() OVER (PARTITION BY ISNULL(LTRIM(RTRIM(Broj)),'') ORDER BY Id) AS rn
  FROM VTEZVV_Snapshot.dbo.DogovorZaRati
)
INSERT INTO InstallmentContracts (Id, StationId, ContractNumber, ContractDate, NumberOfInstallments,
  GuarantorName, GuarantorAddress, GuarantorEMBG, Note, CreatedUtc, LastModifiedUtc)
SELECT d.Id, @StationId,
  CASE WHEN d.rn = 1 THEN LEFT(ISNULL(NULLIF(LTRIM(RTRIM(d.Broj)),''), CONCAT('LEGACY-', d.Id)), 50)
       ELSE LEFT(CONCAT(ISNULL(NULLIF(LTRIM(RTRIM(d.Broj)),''), 'LEGACY'), '-', d.Id), 50) END,
  CAST(d.Datum AS DATE), d.BrNaRati,
  LEFT(d.GarantNaziv, 50), d.GarantAdresa, LEFT(d.GartEMB, 20), NULL, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM dzr d;
SET IDENTITY_INSERT InstallmentContracts OFF;
PRINT CONCAT('  InstallmentContracts: ', @@ROWCOUNT);

-- PaymentDocuments
SET IDENTITY_INSERT PaymentDocuments ON;
;WITH pdup AS (
  SELECT *, ROW_NUMBER() OVER (PARTITION BY ISNULL(LTRIM(RTRIM(DocumentNumber)),'') ORDER BY Id) AS rn
  FROM VTEZVV_Snapshot.dbo.PaymentDocuments
)
INSERT INTO PaymentDocuments (Id, StationId, DocumentNumber, PaymentTypeId, CustomerVehicleRelationId,
  InstallmentContractId, BillToCustomerId, TechnicalExamOrganizationId,
  DatePay, DateRequired, DiscountPercent, Payed, Storno, PolicyNumber, Note,
  CreatedByOperatorUserId, CreatedUtc, LastModifiedUtc)
SELECT p.Id, @StationId,
  CASE WHEN p.rn = 1 THEN LEFT(p.DocumentNumber, 50)
       ELSE LEFT(CONCAT(p.DocumentNumber, '-', p.Id), 50) END,
  CASE WHEN EXISTS(SELECT 1 FROM PaymentTypes pt WHERE pt.Id = p.IdPaymentType) THEN p.IdPaymentType
       ELSE (SELECT TOP 1 Id FROM PaymentTypes ORDER BY Id) END,
  p.IdCustomerVehicleRelation,
  CASE WHEN p.IdDogovor = 0 OR NOT EXISTS(SELECT 1 FROM InstallmentContracts ic WHERE ic.Id = p.IdDogovor) THEN NULL ELSE p.IdDogovor END,
  CASE WHEN p.IdFakturiraNa IS NULL THEN NULL
       WHEN EXISTS(SELECT 1 FROM Customers c WHERE c.Id = p.IdFakturiraNa) THEN p.IdFakturiraNa ELSE NULL END,
  CASE WHEN EXISTS(SELECT 1 FROM TechnicalExamOrganizations teo WHERE teo.Id = p.IdOrganization) THEN p.IdOrganization ELSE NULL END,
  CAST(p.DatePay AS DATE), CAST(p.DateRequired AS DATE),
  CAST(ISNULL(p.Discount, 0) AS DECIMAL(18,2)),
  p.Payed, ISNULL(p.Storno,0), NULL, LEFT(p.Note, 150),
  NULL, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM pdup p
WHERE EXISTS(SELECT 1 FROM CustomerVehicleRelations cvr WHERE cvr.Id = p.IdCustomerVehicleRelation);
SET IDENTITY_INSERT PaymentDocuments OFF;
PRINT CONCAT('  PaymentDocuments: ', @@ROWCOUNT);

-- PaymentDocumentDetails (1.16M)
SET IDENTITY_INSERT PaymentDocumentDetails ON;
INSERT INTO PaymentDocumentDetails (Id, PaymentDocumentId, PriceCatalogId, Price, DDVRate, DiscountPercent,
  PrePayed, Note, NotePrePayed, CustomerFinancialStateId, CreatedUtc, LastModifiedUtc)
SELECT d.Id, d.IdPaymentDocuments,
  CASE WHEN EXISTS(SELECT 1 FROM PriceCatalog pc WHERE pc.Id = d.IdPriceCatalog) THEN d.IdPriceCatalog
       ELSE (SELECT TOP 1 Id FROM PriceCatalog ORDER BY Id) END,
  CAST(d.Price AS DECIMAL(18,2)),
  CAST(ISNULL(d.DDV,0) AS DECIMAL(18,2)),
  CAST(ISNULL(d.Discount,0) AS DECIMAL(18,2)),
  ISNULL(d.PrePayed,0), LEFT(d.Note, 150), LEFT(d.NotePrePayed, 150), NULL, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.PaymentDocumentsDetails d
WHERE EXISTS(SELECT 1 FROM PaymentDocuments pd WHERE pd.Id = d.IdPaymentDocuments);
SET IDENTITY_INSERT PaymentDocumentDetails OFF;
PRINT CONCAT('  PaymentDocumentDetails: ', @@ROWCOUNT);

-- PaymentDocumentInstallments (PaymentDocumentsRata) — derive InstallmentNumber via row_number
;WITH numbered AS (
  SELECT r.*, ROW_NUMBER() OVER (PARTITION BY r.IdPaymentDocument ORDER BY r.Id) AS InstallmentNumber
  FROM VTEZVV_Snapshot.dbo.PaymentDocumentsRata r
)
INSERT INTO PaymentDocumentInstallments (PaymentDocumentId, InstallmentNumber, Price, DueDate,
  Payed, DatePayed, Note, CollectedByOperatorUserId, CreatedUtc, LastModifiedUtc)
SELECT n.IdPaymentDocument, n.InstallmentNumber,
  CAST(n.Price AS DECIMAL(18,2)), NULL,
  n.Payed,
  CASE WHEN n.DatePayed IS NULL THEN NULL ELSE CAST(n.DatePayed AS DATE) END,
  LEFT(n.Note, 150), NULL, SYSUTCDATETIME(), SYSUTCDATETIME()
FROM numbered n
WHERE EXISTS(SELECT 1 FROM PaymentDocuments pd WHERE pd.Id = n.IdPaymentDocument);
PRINT CONCAT('  PaymentDocumentInstallments: ', @@ROWCOUNT);

PRINT '=== Phase 8 done ===';
GO

-- =============================================================
-- Phase 9: Reseed identities, re-enable FKs, summary
-- =============================================================
PRINT '=== Phase 9: Finalize ===';

DECLARE @sql NVARCHAR(MAX) = '';
SELECT @sql = @sql + 'DBCC CHECKIDENT (''' + t.name + ''', RESEED) WITH NO_INFOMSGS;' + CHAR(10)
FROM sys.tables t
WHERE t.is_ms_shipped = 0 AND EXISTS (SELECT 1 FROM sys.columns c WHERE c.object_id = t.object_id AND c.is_identity = 1);
EXEC sp_executesql @sql;

EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL';
PRINT '  FK constraints re-enabled.';

PRINT '';
PRINT '=== Migration summary ===';
SELECT 'Customers' Tab, COUNT(*) Cnt FROM Customers UNION ALL
SELECT 'Vehicles', COUNT(*) FROM Vehicles UNION ALL
SELECT 'CustomerVehicleRelations', COUNT(*) FROM CustomerVehicleRelations UNION ALL
SELECT 'Requests', COUNT(*) FROM Requests UNION ALL
SELECT 'TechnicalExamReports', COUNT(*) FROM TechnicalExamReports UNION ALL
SELECT 'TechnicalExamReportDetails', COUNT(*) FROM TechnicalExamReportDetails UNION ALL
SELECT 'TrafficLicences', COUNT(*) FROM TrafficLicences UNION ALL
SELECT 'Permissions', COUNT(*) FROM Permissions UNION ALL
SELECT 'InternationalDrivingLicences', COUNT(*) FROM InternationalDrivingLicences UNION ALL
SELECT 'InstallmentContracts', COUNT(*) FROM InstallmentContracts UNION ALL
SELECT 'PaymentDocuments', COUNT(*) FROM PaymentDocuments UNION ALL
SELECT 'PaymentDocumentDetails', COUNT(*) FROM PaymentDocumentDetails UNION ALL
SELECT 'PaymentDocumentInstallments', COUNT(*) FROM PaymentDocumentInstallments UNION ALL
SELECT 'VehicleRegistrations', COUNT(*) FROM VehicleRegistrations UNION ALL
SELECT 'VehicleAxles', COUNT(*) FROM VehicleAxles UNION ALL
SELECT 'VehicleAxleDistances', COUNT(*) FROM VehicleAxleDistances UNION ALL
SELECT 'VehicleTyres', COUNT(*) FROM VehicleTyres UNION ALL
SELECT 'CustomerBankAccounts', COUNT(*) FROM CustomerBankAccounts UNION ALL
SELECT 'CustomerContactPersons', COUNT(*) FROM CustomerContactPersons UNION ALL
SELECT 'RequestPaymentProofs', COUNT(*) FROM RequestPaymentProofs UNION ALL
SELECT 'RequestVehicleOwnershipProofs', COUNT(*) FROM RequestVehicleOwnershipProofs UNION ALL
SELECT 'TrafficLicenceExtensions', COUNT(*) FROM TrafficLicenceExtensions UNION ALL
SELECT 'InternationalDrivingLicenceCategories', COUNT(*) FROM InternationalDrivingLicenceCategories
ORDER BY Tab;
GO

PRINT '=== Migration complete ===';
