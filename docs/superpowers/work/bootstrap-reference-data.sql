-- VTE2 — Reference data module schema (~25 essential lookup tables)
-- Generated: 2026-04-30
-- Source: docs/superpowers/specs/2026-04-30-vte-domain-audit.md §4.1.2
--
-- SAFE TO RE-RUN: idempotent.
--
-- Most reference data is GLOBAL (not per-tenant) — cities, colours, vehicle categories
-- don't vary by station. Per-tenant lookups (e.g. operator-defined custom REF) would
-- carry StationId, but none of the tables in this Pass need it.
--
-- The 18 less-essential REF tables (e.g. ColorsDetails, VehicleJUSCategories,
-- VehicleRequiredFields, VehicleDisabledFields, AttachmentTypes, DocumentTypes,
-- DocumentTypePrints, DocumentTypesOptions, DocumentTypesOptionsDetails) are
-- deferred — they're not referenced by FKs of the modules already built.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- Pattern: most REF tables share this shape:
--   Id INT IDENTITY PK, Name NVARCHAR(100|150) NOT NULL, optional Code NVARCHAR,
--   IsActive BIT (default 1), CreatedUtc, LastModifiedUtc, RowVersion.

-- =============================================================================
-- Geography
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Countries')
BEGIN
    CREATE TABLE [dbo].[Countries] (
        [Id]              INT IDENTITY(1,1) NOT NULL,
        [Name]            NVARCHAR(100)     NOT NULL,
        [Iso2]            NVARCHAR(2)       NULL,                                                   -- ISO 3166-1 alpha-2
        [Iso3]            NVARCHAR(3)       NULL,                                                   -- ISO 3166-1 alpha-3
        [IsActive]        BIT               NOT NULL CONSTRAINT [DF_Countries_IsActive]        DEFAULT (1),
        [CreatedUtc]      DATETIME2         NOT NULL CONSTRAINT [DF_Countries_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2         NOT NULL CONSTRAINT [DF_Countries_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]      ROWVERSION        NOT NULL,
        CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([Id])
    );
    PRINT 'Table [dbo].[Countries] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Communities')
BEGIN
    CREATE TABLE [dbo].[Communities] (
        [Id]              INT IDENTITY(1,1) NOT NULL,
        [Name]            NVARCHAR(150)     NOT NULL,
        [CountryId]       INT               NULL,
        [IsActive]        BIT               NOT NULL CONSTRAINT [DF_Communities_IsActive]        DEFAULT (1),
        [CreatedUtc]      DATETIME2         NOT NULL CONSTRAINT [DF_Communities_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2         NOT NULL CONSTRAINT [DF_Communities_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]      ROWVERSION        NOT NULL,
        CONSTRAINT [PK_Communities] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_Communities_Countries] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries]([Id])
    );
    PRINT 'Table [dbo].[Communities] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Cities')
BEGIN
    CREATE TABLE [dbo].[Cities] (
        [Id]              INT IDENTITY(1,1) NOT NULL,
        [Name]            NVARCHAR(150)     NOT NULL,
        [PostalCode]      NVARCHAR(20)      NULL,
        [CommunityId]     INT               NULL,
        [CountryId]       INT               NULL,
        [Description]     NVARCHAR(250)     NULL,                                                   -- legacy "Deskription" — typo fix
        [IsActive]        BIT               NOT NULL CONSTRAINT [DF_Cities_IsActive]        DEFAULT (1),
        [CreatedUtc]      DATETIME2         NOT NULL CONSTRAINT [DF_Cities_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2         NOT NULL CONSTRAINT [DF_Cities_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]      ROWVERSION        NOT NULL,
        CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_Cities_Communities] FOREIGN KEY ([CommunityId]) REFERENCES [dbo].[Communities]([Id]),
        CONSTRAINT [FK_Cities_Countries]   FOREIGN KEY ([CountryId])   REFERENCES [dbo].[Countries]([Id])
    );
    CREATE INDEX [IX_Cities_Name] ON [dbo].[Cities]([Name]);
    PRINT 'Table [dbo].[Cities] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Streets')
BEGIN
    CREATE TABLE [dbo].[Streets] (
        [Id]              INT IDENTITY(1,1) NOT NULL,
        [Name]            NVARCHAR(200)     NOT NULL,
        [CityId]          INT               NULL,
        [IsActive]        BIT               NOT NULL CONSTRAINT [DF_Streets_IsActive]        DEFAULT (1),
        [CreatedUtc]      DATETIME2         NOT NULL CONSTRAINT [DF_Streets_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2         NOT NULL CONSTRAINT [DF_Streets_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]      ROWVERSION        NOT NULL,
        CONSTRAINT [PK_Streets] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_Streets_Cities] FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities]([Id])
    );
    CREATE INDEX [IX_Streets_CityId_Name] ON [dbo].[Streets]([CityId], [Name]);
    PRINT 'Table [dbo].[Streets] created.';
END
GO

-- =============================================================================
-- Customer reference data
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'BusinessTypes')
BEGIN
    CREATE TABLE [dbo].[BusinessTypes] (
        [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(150) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_BusinessTypes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_BusinessTypes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_BusinessTypes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL,
        CONSTRAINT [PK_BusinessTypes] PRIMARY KEY CLUSTERED ([Id])
    );
    PRINT 'Table [dbo].[BusinessTypes] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'RegistrationIssuers')
BEGIN
    CREATE TABLE [dbo].[RegistrationIssuers] (
        [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(200) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_RegistrationIssuers_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_RegistrationIssuers_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_RegistrationIssuers_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL,
        CONSTRAINT [PK_RegistrationIssuers] PRIMARY KEY CLUSTERED ([Id])
    );
    PRINT 'Table [dbo].[RegistrationIssuers] created.';
END
GO

-- =============================================================================
-- Customer-Vehicle relations
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'CustomerVehicleRelationTypes')
BEGIN
    CREATE TABLE [dbo].[CustomerVehicleRelationTypes] (
        [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_CustomerVehicleRelationTypes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_CustomerVehicleRelationTypes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_CustomerVehicleRelationTypes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL,
        CONSTRAINT [PK_CustomerVehicleRelationTypes] PRIMARY KEY CLUSTERED ([Id])
    );
    PRINT 'Table [dbo].[CustomerVehicleRelationTypes] created.';
END
GO

-- =============================================================================
-- Vehicle reference data
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleBodyTypes')
BEGIN
    CREATE TABLE [dbo].[VehicleBodyTypes] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleBodyTypes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleBodyTypes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleBodyTypes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleBodyTypes] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleBodyTypes] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleCategories')
BEGIN
    CREATE TABLE [dbo].[VehicleCategories] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(50) NOT NULL, [Code] NVARCHAR(10) NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleCategories_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleCategories_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleCategories_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleCategories] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleCategories] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleUses')
BEGIN
    CREATE TABLE [dbo].[VehicleUses] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleUses_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleUses_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleUses_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleUses] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleUses] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleMakers')
BEGIN
    CREATE TABLE [dbo].[VehicleMakers] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleMakers_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleMakers_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleMakers_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleMakers] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleMakers] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleModels')
BEGIN
    CREATE TABLE [dbo].[VehicleModels] ( [Id] INT IDENTITY(1,1) NOT NULL, [VehicleMakerId] INT NOT NULL, [Name] NVARCHAR(150) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleModels_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleModels_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleModels_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleModels] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleModels_VehicleMakers] FOREIGN KEY ([VehicleMakerId]) REFERENCES [dbo].[VehicleMakers]([Id]));
    CREATE INDEX [IX_VehicleModels_VehicleMakerId] ON [dbo].[VehicleModels]([VehicleMakerId]);
    PRINT 'Table [dbo].[VehicleModels] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleEngineTypes')
BEGIN
    CREATE TABLE [dbo].[VehicleEngineTypes] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleEngineTypes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleEngineTypes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleEngineTypes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleEngineTypes] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleEngineTypes] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleEnginePowerSourceTypes')
BEGIN
    CREATE TABLE [dbo].[VehicleEnginePowerSourceTypes] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleEnginePowerSourceTypes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleEnginePowerSourceTypes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleEnginePowerSourceTypes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleEnginePowerSourceTypes] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleEnginePowerSourceTypes] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleEngineEcoPrograms')
BEGIN
    CREATE TABLE [dbo].[VehicleEngineEcoPrograms] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(50) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleEngineEcoPrograms_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleEngineEcoPrograms_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleEngineEcoPrograms_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleEngineEcoPrograms] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleEngineEcoPrograms] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleGearBoxes')
BEGIN
    CREATE TABLE [dbo].[VehicleGearBoxes] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(50) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleGearBoxes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleGearBoxes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleGearBoxes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleGearBoxes] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleGearBoxes] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleBrakes')
BEGIN
    CREATE TABLE [dbo].[VehicleBrakes] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleBrakes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleBrakes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleBrakes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleBrakes] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleBrakes] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleSupportings')
BEGIN
    CREATE TABLE [dbo].[VehicleSupportings] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleSupportings_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleSupportings_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleSupportings_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleSupportings] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleSupportings] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Colors')
BEGIN
    CREATE TABLE [dbo].[Colors] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(50) NOT NULL, [HexCode] NVARCHAR(7) NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_Colors_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_Colors_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_Colors_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_Colors] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[Colors] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleCategoriesForPayments')
BEGIN
    CREATE TABLE [dbo].[VehicleCategoriesForPayments] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(150) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleCategoriesForPayments_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleCategoriesForPayments_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleCategoriesForPayments_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleCategoriesForPayments] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleCategoriesForPayments] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleTireTypes')
BEGIN
    CREATE TABLE [dbo].[VehicleTireTypes] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(100) NOT NULL, [Dimensions] NVARCHAR(50) NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleTireTypes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleTireTypes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleTireTypes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleTireTypes] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleTireTypes] created.';
END
GO

-- =============================================================================
-- Technical exam reference data
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TechnicalExamOrganizations')
BEGIN
    CREATE TABLE [dbo].[TechnicalExamOrganizations] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(200) NOT NULL,
        [Address] NVARCHAR(250) NULL, [TaxNumber] NVARCHAR(15) NULL, [Phone] NVARCHAR(50) NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_TechnicalExamOrganizations_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamOrganizations_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamOrganizations_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_TechnicalExamOrganizations] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[TechnicalExamOrganizations] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TechnicalExamTypes')
BEGIN
    CREATE TABLE [dbo].[TechnicalExamTypes] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(150) NOT NULL,
        [ValidityMonths] INT NULL,                                                                  -- typical validity period for this exam type (Q-032)
        [IsActive] BIT NOT NULL CONSTRAINT [DF_TechnicalExamTypes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamTypes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamTypes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_TechnicalExamTypes] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[TechnicalExamTypes] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TechnicalExamVehiclePartCategories')
BEGIN
    CREATE TABLE [dbo].[TechnicalExamVehiclePartCategories] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(150) NOT NULL,
        [SortOrder] INT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_TechnicalExamVehiclePartCategories_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamVehiclePartCategories_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamVehiclePartCategories_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_TechnicalExamVehiclePartCategories] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[TechnicalExamVehiclePartCategories] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TechnicalExamVehicleParts')
BEGIN
    CREATE TABLE [dbo].[TechnicalExamVehicleParts] ( [Id] INT IDENTITY(1,1) NOT NULL, [CategoryId] INT NULL, [Name] NVARCHAR(200) NOT NULL,
        [SortOrder] INT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_TechnicalExamVehicleParts_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamVehicleParts_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamVehicleParts_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_TechnicalExamVehicleParts] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_TechnicalExamVehicleParts_TechnicalExamVehiclePartCategories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[TechnicalExamVehiclePartCategories]([Id]));
    CREATE INDEX [IX_TechnicalExamVehicleParts_CategoryId] ON [dbo].[TechnicalExamVehicleParts]([CategoryId]);
    PRINT 'Table [dbo].[TechnicalExamVehicleParts] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TechnicalExamReportDetailStatuses')
BEGIN
    CREATE TABLE [dbo].[TechnicalExamReportDetailStatuses] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(50) NOT NULL,
        [IsPass] BIT NOT NULL CONSTRAINT [DF_TechnicalExamReportDetailStatuses_IsPass] DEFAULT (0),  -- which statuses count as "passed"
        [IsActive] BIT NOT NULL CONSTRAINT [DF_TechnicalExamReportDetailStatuses_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamReportDetailStatuses_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_TechnicalExamReportDetailStatuses_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_TechnicalExamReportDetailStatuses] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[TechnicalExamReportDetailStatuses] created.';
END
GO

-- =============================================================================
-- Driving licence reference data
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'DrivingLicenceCategories')
BEGIN
    CREATE TABLE [dbo].[DrivingLicenceCategories] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(20) NOT NULL, [Description] NVARCHAR(200) NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_DrivingLicenceCategories_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_DrivingLicenceCategories_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_DrivingLicenceCategories_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_DrivingLicenceCategories] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[DrivingLicenceCategories] created.';
END
GO

-- =============================================================================
-- Document-proof reference data (used by RequestVehicleOwnershipProofs etc.)
-- =============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleOwnershipProofTypes')
BEGIN
    CREATE TABLE [dbo].[VehicleOwnershipProofTypes] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(150) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_VehicleOwnershipProofTypes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleOwnershipProofTypes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_VehicleOwnershipProofTypes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_VehicleOwnershipProofTypes] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[VehicleOwnershipProofTypes] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'PaymentProofTypes')
BEGIN
    CREATE TABLE [dbo].[PaymentProofTypes] ( [Id] INT IDENTITY(1,1) NOT NULL, [Name] NVARCHAR(150) NOT NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_PaymentProofTypes_IsActive] DEFAULT (1),
        [CreatedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_PaymentProofTypes_CreatedUtc] DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2 NOT NULL CONSTRAINT [DF_PaymentProofTypes_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL, CONSTRAINT [PK_PaymentProofTypes] PRIMARY KEY CLUSTERED ([Id]));
    PRINT 'Table [dbo].[PaymentProofTypes] created.';
END
GO

PRINT '';
PRINT '=== Reference data module schema applied ===';
SELECT COUNT(*) AS [Total tables in VTE2] FROM sys.tables;
GO
