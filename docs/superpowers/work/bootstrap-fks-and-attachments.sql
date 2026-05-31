-- VTE2 — Wire deferred FK constraints + add Attachments tables
-- Generated: 2026-04-30
-- Source: cumulative deferred-FK notes across all bootstrap-*.sql files
--
-- SAFE TO RE-RUN: each ALTER TABLE is guarded with sys.foreign_keys lookup;
--                  each CREATE TABLE is guarded with sys.tables lookup.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- =============================================================================
-- Helper procedure: add FK only if it does not already exist
-- (created locally, dropped at end so no permanent footprint)
-- =============================================================================
IF OBJECT_ID('tempdb..#AddFK') IS NOT NULL DROP PROCEDURE #AddFK;
GO

-- We use direct EXEC pattern instead of a temp proc to keep things simple.

-- =============================================================================
-- Customers FKs to REF
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_Countries_CitizenshipId')
    ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [FK_Customers_Countries_CitizenshipId]
        FOREIGN KEY ([CitizenshipId]) REFERENCES [dbo].[Countries]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_BusinessTypes')
    ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [FK_Customers_BusinessTypes]
        FOREIGN KEY ([BusinessTypeId]) REFERENCES [dbo].[BusinessTypes]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_Streets_LivingAddressId')
    ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [FK_Customers_Streets_LivingAddressId]
        FOREIGN KEY ([LivingAddressId]) REFERENCES [dbo].[Streets]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_Cities_LivingCityId')
    ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [FK_Customers_Cities_LivingCityId]
        FOREIGN KEY ([LivingCityId]) REFERENCES [dbo].[Cities]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_Cities_BirthCityId')
    ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [FK_Customers_Cities_BirthCityId]
        FOREIGN KEY ([BirthCityId]) REFERENCES [dbo].[Cities]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_Streets_BirthAddressId')
    ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [FK_Customers_Streets_BirthAddressId]
        FOREIGN KEY ([BirthAddressId]) REFERENCES [dbo].[Streets]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_RegistrationIssuers_IDCardIssuerId')
    ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [FK_Customers_RegistrationIssuers_IDCardIssuerId]
        FOREIGN KEY ([IDCardIssuerId]) REFERENCES [dbo].[RegistrationIssuers]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_RegistrationIssuers_PassportIssuerId')
    ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [FK_Customers_RegistrationIssuers_PassportIssuerId]
        FOREIGN KEY ([PassportIssuerId]) REFERENCES [dbo].[RegistrationIssuers]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Customers_RegistrationIssuers_DrivingLicenceIssuerId')
    ALTER TABLE [dbo].[Customers] ADD CONSTRAINT [FK_Customers_RegistrationIssuers_DrivingLicenceIssuerId]
        FOREIGN KEY ([DrivingLicenceIssuerId]) REFERENCES [dbo].[RegistrationIssuers]([Id]);
PRINT 'Customers FKs wired (9).';
GO

-- =============================================================================
-- Vehicles FKs to REF
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleBodyTypes')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleBodyTypes]
        FOREIGN KEY ([BodyTypeId]) REFERENCES [dbo].[VehicleBodyTypes]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleCategories')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleCategories]
        FOREIGN KEY ([VehicleCategoryId]) REFERENCES [dbo].[VehicleCategories]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleUses')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleUses]
        FOREIGN KEY ([VehicleUseId]) REFERENCES [dbo].[VehicleUses]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleModels')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleModels]
        FOREIGN KEY ([VehicleModelId]) REFERENCES [dbo].[VehicleModels]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_Countries_MadeCountryId')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_Countries_MadeCountryId]
        FOREIGN KEY ([MadeCountryId]) REFERENCES [dbo].[Countries]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleCategoriesForPayments')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleCategoriesForPayments]
        FOREIGN KEY ([VehicleCategoryForPaymentsId]) REFERENCES [dbo].[VehicleCategoriesForPayments]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleEngineTypes')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleEngineTypes]
        FOREIGN KEY ([EngineTypeId]) REFERENCES [dbo].[VehicleEngineTypes]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleEnginePowerSourceTypes_Primary')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleEnginePowerSourceTypes_Primary]
        FOREIGN KEY ([EnginePowerSourceId]) REFERENCES [dbo].[VehicleEnginePowerSourceTypes]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleEnginePowerSourceTypes_Secondary')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleEnginePowerSourceTypes_Secondary]
        FOREIGN KEY ([EngineSecondPowerSourceId]) REFERENCES [dbo].[VehicleEnginePowerSourceTypes]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleEngineEcoPrograms')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleEngineEcoPrograms]
        FOREIGN KEY ([EngineEcoProgramId]) REFERENCES [dbo].[VehicleEngineEcoPrograms]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleGearBoxes')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleGearBoxes]
        FOREIGN KEY ([GearBoxId]) REFERENCES [dbo].[VehicleGearBoxes]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleBrakes')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleBrakes]
        FOREIGN KEY ([BrakesId]) REFERENCES [dbo].[VehicleBrakes]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_VehicleSupportings')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_VehicleSupportings]
        FOREIGN KEY ([SupportingId]) REFERENCES [dbo].[VehicleSupportings]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_Colors_Primary')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_Colors_Primary]
        FOREIGN KEY ([PrimaryColorId]) REFERENCES [dbo].[Colors]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_Colors_Secondary')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_Colors_Secondary]
        FOREIGN KEY ([SecondaryColorId]) REFERENCES [dbo].[Colors]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_RegistrationIssuers_First')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_RegistrationIssuers_First]
        FOREIGN KEY ([FirstRegistrationIssuerId]) REFERENCES [dbo].[RegistrationIssuers]([Id]);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_RegistrationIssuers_Last')
    ALTER TABLE [dbo].[Vehicles] ADD CONSTRAINT [FK_Vehicles_RegistrationIssuers_Last]
        FOREIGN KEY ([LastRegistrationIssuerId]) REFERENCES [dbo].[RegistrationIssuers]([Id]);
PRINT 'Vehicles FKs wired (17).';
GO

-- =============================================================================
-- VehicleRegistrations + VehicleTyres + CustomerVehicleRelations FKs
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_VehicleRegistrations_RegistrationIssuers')
    ALTER TABLE [dbo].[VehicleRegistrations] ADD CONSTRAINT [FK_VehicleRegistrations_RegistrationIssuers]
        FOREIGN KEY ([IssuerId]) REFERENCES [dbo].[RegistrationIssuers]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_VehicleTyres_VehicleTireTypes')
    ALTER TABLE [dbo].[VehicleTyres] ADD CONSTRAINT [FK_VehicleTyres_VehicleTireTypes]
        FOREIGN KEY ([TireTypeId]) REFERENCES [dbo].[VehicleTireTypes]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_CustomerVehicleRelations_RelationTypes')
    ALTER TABLE [dbo].[CustomerVehicleRelations] ADD CONSTRAINT [FK_CustomerVehicleRelations_RelationTypes]
        FOREIGN KEY ([RelationTypeId]) REFERENCES [dbo].[CustomerVehicleRelationTypes]([Id]);
PRINT 'Vehicle-related FKs wired (3).';
GO

-- =============================================================================
-- Requests + PaymentDocuments organization FKs
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Requests_TechnicalExamOrganizations')
    ALTER TABLE [dbo].[Requests] ADD CONSTRAINT [FK_Requests_TechnicalExamOrganizations]
        FOREIGN KEY ([TechnicalExamOrganizationId]) REFERENCES [dbo].[TechnicalExamOrganizations]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_PaymentDocuments_TechnicalExamOrganizations')
    ALTER TABLE [dbo].[PaymentDocuments] ADD CONSTRAINT [FK_PaymentDocuments_TechnicalExamOrganizations]
        FOREIGN KEY ([TechnicalExamOrganizationId]) REFERENCES [dbo].[TechnicalExamOrganizations]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_RequestVehicleOwnershipProofs_VehicleOwnershipProofTypes')
    ALTER TABLE [dbo].[RequestVehicleOwnershipProofs] ADD CONSTRAINT [FK_RequestVehicleOwnershipProofs_VehicleOwnershipProofTypes]
        FOREIGN KEY ([OwnershipProofId]) REFERENCES [dbo].[VehicleOwnershipProofTypes]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_RequestPaymentProofs_PaymentProofTypes')
    ALTER TABLE [dbo].[RequestPaymentProofs] ADD CONSTRAINT [FK_RequestPaymentProofs_PaymentProofTypes]
        FOREIGN KEY ([PaymentProofId]) REFERENCES [dbo].[PaymentProofTypes]([Id]);
PRINT 'Requests/PaymentDocuments FKs wired (4).';
GO

-- =============================================================================
-- Document FKs
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_TrafficLicences_TechnicalExamOrganizations')
    ALTER TABLE [dbo].[TrafficLicences] ADD CONSTRAINT [FK_TrafficLicences_TechnicalExamOrganizations]
        FOREIGN KEY ([IssuingOrganizationId]) REFERENCES [dbo].[TechnicalExamOrganizations]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_InternationalDrivingLicences_RegistrationIssuers')
    ALTER TABLE [dbo].[InternationalDrivingLicences] ADD CONSTRAINT [FK_InternationalDrivingLicences_RegistrationIssuers]
        FOREIGN KEY ([IssuerId]) REFERENCES [dbo].[RegistrationIssuers]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_InternationalDrivingLicenceCategories_DrivingLicenceCategories')
    ALTER TABLE [dbo].[InternationalDrivingLicenceCategories] ADD CONSTRAINT [FK_InternationalDrivingLicenceCategories_DrivingLicenceCategories]
        FOREIGN KEY ([DrivingLicenceCategoryId]) REFERENCES [dbo].[DrivingLicenceCategories]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_TechnicalExamReports_TechnicalExamTypes')
    ALTER TABLE [dbo].[TechnicalExamReports] ADD CONSTRAINT [FK_TechnicalExamReports_TechnicalExamTypes]
        FOREIGN KEY ([TechnicalExamTypeId]) REFERENCES [dbo].[TechnicalExamTypes]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_TechnicalExamReports_TechnicalExamOrganizations')
    ALTER TABLE [dbo].[TechnicalExamReports] ADD CONSTRAINT [FK_TechnicalExamReports_TechnicalExamOrganizations]
        FOREIGN KEY ([OrganizationForTechnicalExamId]) REFERENCES [dbo].[TechnicalExamOrganizations]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_TechnicalExamReportDetails_TechnicalExamVehicleParts')
    ALTER TABLE [dbo].[TechnicalExamReportDetails] ADD CONSTRAINT [FK_TechnicalExamReportDetails_TechnicalExamVehicleParts]
        FOREIGN KEY ([TechnicalExamVehiclePartId]) REFERENCES [dbo].[TechnicalExamVehicleParts]([Id]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_TechnicalExamReportDetails_TechnicalExamReportDetailStatuses')
    ALTER TABLE [dbo].[TechnicalExamReportDetails] ADD CONSTRAINT [FK_TechnicalExamReportDetails_TechnicalExamReportDetailStatuses]
        FOREIGN KEY ([StatusId]) REFERENCES [dbo].[TechnicalExamReportDetailStatuses]([Id]);
PRINT 'Document FKs wired (7).';
GO

-- =============================================================================
-- Operators -> Stations FK (already had column, verify FK exists)
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Operators_Stations_StationId')
    PRINT 'WARN: Operators.StationId FK was missing — should already be present from bootstrap-vte2.sql.';
GO

-- =============================================================================
-- Attachments — generic blob storage per major entity
-- =============================================================================

-- Pattern: each major entity that can have attachments gets its own attachment table.
-- Files are stored as VARBINARY(MAX) for now (small to medium PDFs / images);
-- larger files can be migrated to external blob storage by adding ExternalStoragePath
-- and switching to a stored-path-only approach later. This is the default ASP.NET
-- Identity / EF Core pattern and works fine up to ~5 MB per file.

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'CustomerAttachments')
BEGIN
    CREATE TABLE [dbo].[CustomerAttachments] (
        [Id]                       BIGINT IDENTITY(1,1) NOT NULL,
        [CustomerId]               BIGINT               NOT NULL,
        [AttachmentTypeId]         INT                  NULL,                                       -- FK to a future AttachmentTypes REF (optional)
        [FileName]                 NVARCHAR(255)        NOT NULL,
        [ContentType]              NVARCHAR(100)        NOT NULL,                                   -- MIME type
        [FileSize]                 BIGINT               NOT NULL,
        [Content]                  VARBINARY(MAX)       NOT NULL,
        [Description]              NVARCHAR(500)        NULL,
        [UploadedByUserId]         NVARCHAR(450)        NULL,
        [CreatedUtc]               DATETIME2            NOT NULL CONSTRAINT [DF_CustomerAttachments_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]               ROWVERSION           NOT NULL,
        CONSTRAINT [PK_CustomerAttachments] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_CustomerAttachments_Customers]
            FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CustomerAttachments_AspNetUsers]
            FOREIGN KEY ([UploadedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
    );
    CREATE INDEX [IX_CustomerAttachments_CustomerId] ON [dbo].[CustomerAttachments]([CustomerId]);
    PRINT 'Table [dbo].[CustomerAttachments] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleAttachments')
BEGIN
    CREATE TABLE [dbo].[VehicleAttachments] (
        [Id]                       BIGINT IDENTITY(1,1) NOT NULL,
        [VehicleId]                BIGINT               NOT NULL,
        [AttachmentTypeId]         INT                  NULL,
        [FileName]                 NVARCHAR(255)        NOT NULL,
        [ContentType]              NVARCHAR(100)        NOT NULL,
        [FileSize]                 BIGINT               NOT NULL,
        [Content]                  VARBINARY(MAX)       NOT NULL,
        [Description]              NVARCHAR(500)        NULL,
        [UploadedByUserId]         NVARCHAR(450)        NULL,
        [CreatedUtc]               DATETIME2            NOT NULL CONSTRAINT [DF_VehicleAttachments_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]               ROWVERSION           NOT NULL,
        CONSTRAINT [PK_VehicleAttachments] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleAttachments_Vehicles]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_VehicleAttachments_AspNetUsers]
            FOREIGN KEY ([UploadedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
    );
    CREATE INDEX [IX_VehicleAttachments_VehicleId] ON [dbo].[VehicleAttachments]([VehicleId]);
    PRINT 'Table [dbo].[VehicleAttachments] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'RequestAttachments')
BEGIN
    CREATE TABLE [dbo].[RequestAttachments] (
        [Id]                       BIGINT IDENTITY(1,1) NOT NULL,
        [RequestId]                BIGINT               NOT NULL,
        [AttachmentTypeId]         INT                  NULL,
        [FileName]                 NVARCHAR(255)        NOT NULL,
        [ContentType]              NVARCHAR(100)        NOT NULL,
        [FileSize]                 BIGINT               NOT NULL,
        [Content]                  VARBINARY(MAX)       NOT NULL,
        [Description]              NVARCHAR(500)        NULL,
        [UploadedByUserId]         NVARCHAR(450)        NULL,
        [CreatedUtc]               DATETIME2            NOT NULL CONSTRAINT [DF_RequestAttachments_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]               ROWVERSION           NOT NULL,
        CONSTRAINT [PK_RequestAttachments] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_RequestAttachments_Requests]
            FOREIGN KEY ([RequestId]) REFERENCES [dbo].[Requests]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RequestAttachments_AspNetUsers]
            FOREIGN KEY ([UploadedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
    );
    CREATE INDEX [IX_RequestAttachments_RequestId] ON [dbo].[RequestAttachments]([RequestId]);
    PRINT 'Table [dbo].[RequestAttachments] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TechnicalExamReportAttachments')
BEGIN
    CREATE TABLE [dbo].[TechnicalExamReportAttachments] (
        [Id]                       BIGINT IDENTITY(1,1) NOT NULL,
        [TechnicalExamReportId]    BIGINT               NOT NULL,
        [AttachmentTypeId]         INT                  NULL,
        [FileName]                 NVARCHAR(255)        NOT NULL,
        [ContentType]              NVARCHAR(100)        NOT NULL,
        [FileSize]                 BIGINT               NOT NULL,
        [Content]                  VARBINARY(MAX)       NOT NULL,
        [Description]              NVARCHAR(500)        NULL,
        [UploadedByUserId]         NVARCHAR(450)        NULL,
        [CreatedUtc]               DATETIME2            NOT NULL CONSTRAINT [DF_TechnicalExamReportAttachments_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]               ROWVERSION           NOT NULL,
        CONSTRAINT [PK_TechnicalExamReportAttachments] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_TechnicalExamReportAttachments_TechnicalExamReports]
            FOREIGN KEY ([TechnicalExamReportId]) REFERENCES [dbo].[TechnicalExamReports]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TechnicalExamReportAttachments_AspNetUsers]
            FOREIGN KEY ([UploadedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
    );
    CREATE INDEX [IX_TechnicalExamReportAttachments_TechnicalExamReportId] ON [dbo].[TechnicalExamReportAttachments]([TechnicalExamReportId]);
    PRINT 'Table [dbo].[TechnicalExamReportAttachments] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'PaymentDocumentAttachments')
BEGIN
    CREATE TABLE [dbo].[PaymentDocumentAttachments] (
        [Id]                       BIGINT IDENTITY(1,1) NOT NULL,
        [PaymentDocumentId]        BIGINT               NOT NULL,
        [AttachmentTypeId]         INT                  NULL,
        [FileName]                 NVARCHAR(255)        NOT NULL,
        [ContentType]              NVARCHAR(100)        NOT NULL,
        [FileSize]                 BIGINT               NOT NULL,
        [Content]                  VARBINARY(MAX)       NOT NULL,
        [Description]              NVARCHAR(500)        NULL,
        [UploadedByUserId]         NVARCHAR(450)        NULL,
        [CreatedUtc]               DATETIME2            NOT NULL CONSTRAINT [DF_PaymentDocumentAttachments_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]               ROWVERSION           NOT NULL,
        CONSTRAINT [PK_PaymentDocumentAttachments] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_PaymentDocumentAttachments_PaymentDocuments]
            FOREIGN KEY ([PaymentDocumentId]) REFERENCES [dbo].[PaymentDocuments]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PaymentDocumentAttachments_AspNetUsers]
            FOREIGN KEY ([UploadedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
    );
    CREATE INDEX [IX_PaymentDocumentAttachments_PaymentDocumentId] ON [dbo].[PaymentDocumentAttachments]([PaymentDocumentId]);
    PRINT 'Table [dbo].[PaymentDocumentAttachments] created.';
END
GO

PRINT '';
PRINT '=== FK wiring + Attachments complete ===';
SELECT
    (SELECT COUNT(*) FROM sys.tables)          AS [Tables],
    (SELECT COUNT(*) FROM sys.foreign_keys)    AS [FKs],
    (SELECT COUNT(*) FROM sys.check_constraints) AS [CHECKs],
    (SELECT COUNT(*) FROM sys.indexes WHERE is_primary_key = 0 AND index_id > 0) AS [Indexes];
GO
