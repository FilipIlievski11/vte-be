-- =============================================================================
-- VTE2 — Full consolidated bootstrap (schema + default seed)
-- Generated: 2026-05-24
--
-- This script is a verbatim concatenation of the 8 module bootstrap files plus
-- the global default seed, in correct dependency order. Each section is preceded
-- by a divider naming its source file so it can be cross-referenced.
--
-- SAFE TO RE-RUN end-to-end: every statement is idempotent (IF NOT EXISTS guards
-- on schema, NOT EXISTS guards on seed data).
--
-- Apply with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i docs\superpowers\work\bootstrap-vte2-full.sql -b -X -I
--
-- The script will:
--   1. Create database VTE2 if absent.
--   2. Create ASP.NET Core Identity tables + Stations + Operators (auth/foundation).
--   3. Create 29 reference-data lookup tables (Cities, Colors, VehicleCategories, …).
--   4. Create the 5 module schemas: Customers, Vehicles, Requests, Payments, Documents.
--   5. Wire deferred FK constraints from business tables → REF lookups.
--   6. Create 5 attachment tables (one per business entity that supports uploads).
--   7. Seed default global REF data (16 countries, 31 cities, EU vehicle cats, VAT, etc.).
--
-- Expected end state: 73 tables, 121 FKs, 20 CHECK constraints, ~250 non-PK indexes.
--
-- For continuation context (what to do next, where things live), see HANDOFF.md
-- in the same folder.
-- =============================================================================

-- =============================================================================
-- SECTION 1 / 9 — Foundation (database, Identity, Stations, Operators)
-- Source: docs/superpowers/work/bootstrap-vte2.sql
-- =============================================================================

-- VTE2 — Bootstrap database for the new VTE rewrite (Pass-1 auth design only)
-- Generated: 2026-04-30
-- Source design: docs/superpowers/specs/2026-04-30-vte-domain-audit.md §3
--
-- SAFE TO RE-RUN: every statement is idempotent (IF NOT EXISTS guards).
-- This script does NOT drop anything. To remove the database, do it manually.
--
-- Schema scope (intentionally minimal for Pass 1):
--   * Stations (tenant table — replaces legacy DataBases)
--   * ASP.NET Core Identity 9 standard tables (AspNetUsers, AspNetRoles, etc.)
--   * Operators (extends AspNetUsers with VTE-specific data + StationId FK)
--   * Seeded: 2 roles (Administrator, Operator)
--
-- NOT in scope here (will be added in later sub-projects):
--   * Customers, Vehicles, Requests, Documents, Payments, Reports, Attachments
--   * Stored procedures, views, triggers
--   * Per-station seed data

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-- 1. Create the database
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'VTE2')
BEGIN
    CREATE DATABASE [VTE2];
    PRINT 'Database [VTE2] created.';
END
ELSE
    PRINT 'Database [VTE2] already exists — using existing.';
GO

USE [VTE2];
GO

-- 2. Stations (tenant table)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Stations')
BEGIN
    CREATE TABLE [dbo].[Stations] (
        [Id]              INT IDENTITY(1,1) NOT NULL,
        [Name]            NVARCHAR(200) NOT NULL,
        [Code]            NVARCHAR(20)  NOT NULL,
        [IsActive]        BIT           NOT NULL CONSTRAINT [DF_Stations_IsActive]      DEFAULT (1),
        [CreatedUtc]      DATETIME2     NOT NULL CONSTRAINT [DF_Stations_CreatedUtc]    DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2     NOT NULL CONSTRAINT [DF_Stations_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Stations]      PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [UQ_Stations_Code] UNIQUE ([Code])
    );
    PRINT 'Table [dbo].[Stations] created.';
END
ELSE
    PRINT 'Table [dbo].[Stations] already exists.';
GO

-- 3. ASP.NET Core Identity 9 standard tables
--    (Schema matches what `dotnet ef database update` would produce, so future
--     EF migrations against an empty migration history can be reconciled
--     with `--from-migration 0` or a baseline migration.)

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'AspNetRoles')
BEGIN
    CREATE TABLE [dbo].[AspNetRoles] (
        [Id]               NVARCHAR(450) NOT NULL,
        [Name]             NVARCHAR(256) NULL,
        [NormalizedName]   NVARCHAR(256) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id])
    );
    CREATE UNIQUE INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]([NormalizedName])
        WHERE [NormalizedName] IS NOT NULL;
    PRINT 'Table [dbo].[AspNetRoles] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'AspNetUsers')
BEGIN
    CREATE TABLE [dbo].[AspNetUsers] (
        [Id]                   NVARCHAR(450) NOT NULL,
        [UserName]             NVARCHAR(256) NULL,
        [NormalizedUserName]   NVARCHAR(256) NULL,
        [Email]                NVARCHAR(256) NULL,
        [NormalizedEmail]      NVARCHAR(256) NULL,
        [EmailConfirmed]       BIT           NOT NULL,
        [PasswordHash]         NVARCHAR(MAX) NULL,
        [SecurityStamp]        NVARCHAR(MAX) NULL,
        [ConcurrencyStamp]     NVARCHAR(MAX) NULL,
        [PhoneNumber]          NVARCHAR(MAX) NULL,
        [PhoneNumberConfirmed] BIT           NOT NULL,
        [TwoFactorEnabled]     BIT           NOT NULL,
        [LockoutEnd]           DATETIMEOFFSET NULL,
        [LockoutEnabled]       BIT           NOT NULL,
        [AccessFailedCount]    INT           NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id])
    );
    CREATE INDEX [EmailIndex] ON [dbo].[AspNetUsers]([NormalizedEmail]);
    CREATE UNIQUE INDEX [UserNameIndex] ON [dbo].[AspNetUsers]([NormalizedUserName])
        WHERE [NormalizedUserName] IS NOT NULL;
    PRINT 'Table [dbo].[AspNetUsers] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'AspNetRoleClaims')
BEGIN
    CREATE TABLE [dbo].[AspNetRoleClaims] (
        [Id]         INT IDENTITY(1,1) NOT NULL,
        [RoleId]     NVARCHAR(450) NOT NULL,
        [ClaimType]  NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
            FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]([RoleId]);
    PRINT 'Table [dbo].[AspNetRoleClaims] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'AspNetUserClaims')
BEGIN
    CREATE TABLE [dbo].[AspNetUserClaims] (
        [Id]         INT IDENTITY(1,1) NOT NULL,
        [UserId]     NVARCHAR(450) NOT NULL,
        [ClaimType]  NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]([UserId]);
    PRINT 'Table [dbo].[AspNetUserClaims] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'AspNetUserLogins')
BEGIN
    CREATE TABLE [dbo].[AspNetUserLogins] (
        [LoginProvider]       NVARCHAR(450) NOT NULL,
        [ProviderKey]         NVARCHAR(450) NOT NULL,
        [ProviderDisplayName] NVARCHAR(MAX) NULL,
        [UserId]              NVARCHAR(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]([UserId]);
    PRINT 'Table [dbo].[AspNetUserLogins] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'AspNetUserRoles')
BEGIN
    CREATE TABLE [dbo].[AspNetUserRoles] (
        [UserId] NVARCHAR(450) NOT NULL,
        [RoleId] NVARCHAR(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
            FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]([RoleId]);
    PRINT 'Table [dbo].[AspNetUserRoles] created.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'AspNetUserTokens')
BEGIN
    CREATE TABLE [dbo].[AspNetUserTokens] (
        [UserId]        NVARCHAR(450) NOT NULL,
        [LoginProvider] NVARCHAR(450) NOT NULL,
        [Name]          NVARCHAR(450) NOT NULL,
        [Value]         NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id]) ON DELETE CASCADE
    );
    PRINT 'Table [dbo].[AspNetUserTokens] created.';
END
GO

-- 4. Operators — extends AspNetUsers with VTE-specific data
--    StationId is NULL for Administrators (cross-tenant), NOT NULL for Operators
--    (the role is enforced at the API layer; the schema only models the binding)

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Operators')
BEGIN
    CREATE TABLE [dbo].[Operators] (
        [UserId]          NVARCHAR(450) NOT NULL,
        [StationId]       INT           NULL,
        [FullName]        NVARCHAR(200) NOT NULL,
        [EMBG]            NVARCHAR(13)  NULL,
        [IsActive]        BIT           NOT NULL CONSTRAINT [DF_Operators_IsActive]        DEFAULT (1),
        [CreatedUtc]      DATETIME2     NOT NULL CONSTRAINT [DF_Operators_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc] DATETIME2     NOT NULL CONSTRAINT [DF_Operators_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        CONSTRAINT [PK_Operators] PRIMARY KEY CLUSTERED ([UserId]),
        CONSTRAINT [FK_Operators_AspNetUsers_UserId]
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Operators_Stations_StationId]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id])
    );
    CREATE INDEX [IX_Operators_StationId] ON [dbo].[Operators]([StationId]);
    PRINT 'Table [dbo].[Operators] created.';
END
GO

-- 5. Seed the two roles (idempotent — only inserts if absent)
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetRoles] WHERE NormalizedName = N'ADMINISTRATOR')
BEGIN
    INSERT INTO [dbo].[AspNetRoles] (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (LOWER(CAST(NEWID() AS NVARCHAR(36))), N'Administrator', N'ADMINISTRATOR', LOWER(CAST(NEWID() AS NVARCHAR(36))));
    PRINT 'Seeded role: Administrator';
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetRoles] WHERE NormalizedName = N'OPERATOR')
BEGIN
    INSERT INTO [dbo].[AspNetRoles] (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (LOWER(CAST(NEWID() AS NVARCHAR(36))), N'Operator', N'OPERATOR', LOWER(CAST(NEWID() AS NVARCHAR(36))));
    PRINT 'Seeded role: Operator';
END
GO

-- 6. Final state report
PRINT '';
PRINT '=== VTE2 bootstrap complete ===';
SELECT name AS [Tables in VTE2]
FROM sys.tables
ORDER BY name;
SELECT Name AS [Seeded roles] FROM [dbo].[AspNetRoles] ORDER BY Name;
GO


-- =============================================================================
-- SECTION 2 / 9 — Reference data — 29 lookup tables
-- Source: docs\superpowers\work\bootstrap-reference-data.sql
-- =============================================================================

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


-- =============================================================================
-- SECTION 3 / 9 — Customers module schema
-- Source: docs\superpowers\work\bootstrap-customers.sql
-- =============================================================================

-- VTE2 — Customers module schema
-- Generated: 2026-04-30
-- Source: docs/superpowers/work/customers-business-rules.md (rules BR-CUS-001..032)
--         docs/superpowers/specs/2026-04-30-vte-domain-audit.md §4.2.2
--
-- SAFE TO RE-RUN: idempotent (IF NOT EXISTS guards).
-- Reference-data FKs (LivingCityId, BusinessTypeId, etc.) are NOT enforced yet —
-- the REF tables don't exist in VTE2 in this Pass. They become FK constraints
-- when the REF module is migrated.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- 1. Customers (root entity)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Customers')
BEGIN
    CREATE TABLE [dbo].[Customers] (
        [Id]                       BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]                INT                  NOT NULL,
        [IsCompany]                BIT                  NOT NULL CONSTRAINT [DF_Customers_IsCompany] DEFAULT (0),

        -- Personal / company identity
        [EMBG]                     NVARCHAR(13)         NULL,                       -- BR-CUS-001 (renamed from Mb)
        [FirstName]                NVARCHAR(100)        NOT NULL,                   -- BR-CUS-005
        [Surname]                  NVARCHAR(100)        NULL,                       -- BR-CUS-004 (NULL allowed for companies)
        [ParentName]               NVARCHAR(100)        NULL,                       -- Macedonian "татково име"
        [DateOfBirth]              DATE                 NULL,                       -- BR-CUS-011 (NULL for companies; check at app level)
        [CitizenshipId]            INT                  NULL,                       -- FK → Countries (deferred)
        [Occupation]               NVARCHAR(50)         NULL,                       -- BR-CUS-010
        [WorksInCompany]           NVARCHAR(200)        NULL,
        [BusinessTypeId]           INT                  NULL,                       -- FK → BusinessTypes (deferred); for companies

        -- Living address
        [LivingAddressId]          INT                  NULL,                       -- FK → Streets (deferred)
        [LivingAddressNumber]      NVARCHAR(100)        NULL,                       -- BR-CUS-008
        [LivingCityId]             INT                  NULL,                       -- FK → Cities (deferred)

        -- Birth place (typos fixed: legacy "IdBirhCity"/"BrithAddressNumber")
        [BirthCityId]              INT                  NULL,                       -- FK → Cities (deferred)
        [BirthAddressId]           INT                  NULL,                       -- FK → Streets (deferred)
        [BirthAddressNumber]       NVARCHAR(100)        NULL,                       -- BR-CUS-009

        -- Contact
        [PhoneNumber]              NVARCHAR(20)         NULL,                       -- BR-CUS-006
        [Fax]                      NVARCHAR(20)         NULL,                       -- BR-CUS-007 (legacy field; kept for migration)
        [Email]                    NVARCHAR(200)        NULL,                       -- BR-CUS-012, BR-CUS-013

        -- Identity documents (typos fixed: legacy "BLK"/"Driveing")
        [IDCardNumber]             NVARCHAR(20)         NULL,                       -- legacy "BLK"
        [IDCardDateIssued]         DATE                 NULL,
        [IDCardIssuerId]           INT                  NULL,                       -- FK → RegistrationIssuers (deferred)
        [PassportNumber]           NVARCHAR(20)         NULL,
        [PassportDateIssued]       DATE                 NULL,
        [PassportIssuerId]         INT                  NULL,                       -- FK → RegistrationIssuers (deferred)
        [DrivingLicenceNumber]     NVARCHAR(20)         NULL,
        [DrivingLicenceDateIssued] DATE                 NULL,
        [DrivingLicenceIssuerId]   INT                  NULL,                       -- FK → RegistrationIssuers (deferred)

        -- Tax (corporate)
        [TaxNumber]                NVARCHAR(15)         NULL,

        -- Notifications
        [CanSendNotifications]     BIT                  NOT NULL CONSTRAINT [DF_Customers_CanSendNotifications] DEFAULT (0),

        -- Free-text
        [Status]                   NVARCHAR(50)         NULL,
        [Note]                     NVARCHAR(MAX)        NULL,

        -- Soft-delete
        [IsActive]                 BIT                  NOT NULL CONSTRAINT [DF_Customers_IsActive] DEFAULT (1),

        -- Audit
        [CreatedUtc]               DATETIME2            NOT NULL CONSTRAINT [DF_Customers_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]          DATETIME2            NOT NULL CONSTRAINT [DF_Customers_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [CreatedByUserId]          NVARCHAR(450)        NULL,
        [LastModifiedByUserId]     NVARCHAR(450)        NULL,

        -- Concurrency
        [RowVersion]               ROWVERSION           NOT NULL,

        CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_Customers_Stations_StationId]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_Customers_AspNetUsers_CreatedByUserId]
            FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [FK_Customers_AspNetUsers_LastModifiedByUserId]
            FOREIGN KEY ([LastModifiedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id])
    );

    -- Indexes
    -- BR-CUS-002 modernized: EMBG unique per tenant when present
    CREATE UNIQUE INDEX [UX_Customers_StationId_EMBG]
        ON [dbo].[Customers]([StationId], [EMBG])
        WHERE [EMBG] IS NOT NULL;

    -- Search index
    CREATE INDEX [IX_Customers_StationId_Surname_FirstName]
        ON [dbo].[Customers]([StationId], [Surname], [FirstName])
        INCLUDE ([Id], [IsActive]);

    -- Tenant filter
    CREATE INDEX [IX_Customers_StationId]
        ON [dbo].[Customers]([StationId]);

    PRINT 'Table [dbo].[Customers] created.';
END
ELSE
    PRINT 'Table [dbo].[Customers] already exists.';
GO

-- 2. CustomerContactPersons (legacy: Customers.ContactPersons — dot removed)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'CustomerContactPersons')
BEGIN
    CREATE TABLE [dbo].[CustomerContactPersons] (
        [Id]                   INT IDENTITY(1,1) NOT NULL,
        [CustomerId]           BIGINT            NOT NULL,
        [EMBG]                 NVARCHAR(13)      NULL,                              -- BR-CUS-020
        [FirstName]            NVARCHAR(50)      NOT NULL,                          -- BR-CUS-021
        [Surname]              NVARCHAR(50)      NOT NULL,                          -- BR-CUS-022
        [PhoneNumber]          NVARCHAR(20)      NULL,                              -- BR-CUS-023
        [MobileNumber]         NVARCHAR(20)      NULL,                              -- BR-CUS-024
        [Email]                NVARCHAR(200)     NULL,                              -- BR-CUS-025 (widened from 50)
        [CreatedUtc]           DATETIME2         NOT NULL CONSTRAINT [DF_CustomerContactPersons_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]      DATETIME2         NOT NULL CONSTRAINT [DF_CustomerContactPersons_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]           ROWVERSION        NOT NULL,
        CONSTRAINT [PK_CustomerContactPersons] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_CustomerContactPersons_Customers_CustomerId]
            FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_CustomerContactPersons_CustomerId]
        ON [dbo].[CustomerContactPersons]([CustomerId]);
    PRINT 'Table [dbo].[CustomerContactPersons] created.';
END
ELSE
    PRINT 'Table [dbo].[CustomerContactPersons] already exists.';
GO

-- 3. CustomerBankAccounts (legacy: CustomersBankAccounts — drop redundant plural)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'CustomerBankAccounts')
BEGIN
    CREATE TABLE [dbo].[CustomerBankAccounts] (
        [Id]                   BIGINT IDENTITY(1,1) NOT NULL,
        [CustomerId]           BIGINT               NOT NULL,
        [BankAccount]          NVARCHAR(50)         NOT NULL,                       -- BR-CUS-030
        [DeponentBank]         NVARCHAR(50)         NOT NULL,                       -- BR-CUS-031
        [TaxNumber]            NVARCHAR(15)         NULL,                           -- BR-CUS-032 (Q-007: why separate from Customer.TaxNumber?)
        [CreatedUtc]           DATETIME2            NOT NULL CONSTRAINT [DF_CustomerBankAccounts_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]      DATETIME2            NOT NULL CONSTRAINT [DF_CustomerBankAccounts_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]           ROWVERSION           NOT NULL,
        CONSTRAINT [PK_CustomerBankAccounts] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_CustomerBankAccounts_Customers_CustomerId]
            FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_CustomerBankAccounts_CustomerId]
        ON [dbo].[CustomerBankAccounts]([CustomerId]);
    PRINT 'Table [dbo].[CustomerBankAccounts] created.';
END
ELSE
    PRINT 'Table [dbo].[CustomerBankAccounts] already exists.';
GO

PRINT '';
PRINT '=== Customers module schema applied ===';
SELECT name AS [Tables in VTE2 (Customers module)]
FROM sys.tables
WHERE name IN (N'Customers', N'CustomerContactPersons', N'CustomerBankAccounts')
ORDER BY name;
GO


-- =============================================================================
-- SECTION 4 / 9 — Vehicles module schema
-- Source: docs\superpowers\work\bootstrap-vehicles.sql
-- =============================================================================

-- VTE2 — Vehicles module schema
-- Generated: 2026-04-30
-- Source: docs/superpowers/work/vehicles-business-rules.md (rules BR-VEH-001..107)
--         docs/superpowers/specs/2026-04-30-vte-domain-audit.md §4.3.2
--
-- SAFE TO RE-RUN: idempotent (IF NOT EXISTS guards).
-- Reference-data FKs (BodyTypeId, VehicleCategoryId, etc.) are NOT enforced yet —
-- the REF tables don't exist in VTE2 in this Pass. They become FK constraints
-- when the REF module is migrated.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- 1. Vehicles (main entity)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Vehicles')
BEGIN
    CREATE TABLE [dbo].[Vehicles] (
        [Id]                                BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]                         INT                  NOT NULL,

        -- Identity (the central business value of a vehicle record)
        [ShellNumber]                       NVARCHAR(17)         NOT NULL,                       -- VIN; BR-VEH-001, BR-VEH-002, BR-VEH-003

        -- Classification (FKs deferred to REF migration)
        [BodyTypeId]                        INT                  NULL,                           -- legacy IdVehicleBodyType
        [VehicleCategoryId]                 INT                  NULL,
        [VehicleUseId]                      INT                  NULL,
        [VehicleModelId]                    INT                  NULL,
        [VehicleModelAdding]                NVARCHAR(200)        NULL,                           -- free-text extra after model
        [MadeCountryId]                     INT                  NULL,
        [VehicleCategoryForPaymentsId]      INT                  NULL,

        -- Engine
        [EngineNumber]                      NVARCHAR(50)         NULL,
        [EngineTypeId]                      INT                  NULL,
        [EnginePowerSourceId]               INT                  NULL,
        [EngineSecondPowerSourceId]         INT                  NULL,
        [EngineEcoProgramId]                INT                  NULL,
        [EnginePowerKw]                     DECIMAL(10,2)        NULL,                           -- legacy EnginePower (kW); BR-VEH-091
        [EnginePowerOutput]                 DECIMAL(10,2)        NULL,
        [EngineTorque]                      NVARCHAR(50)         NULL,
        [EngineTorqueUnderGas]              DECIMAL(10,2)        NULL,                           -- typo fix
        [EngineWorkingCapacity]             DECIMAL(10,2)        NULL,                           -- cc displacement
        [RPM]                               INT                  NULL,                           -- legacy BrojNaVrtezi
        [EngineIdentificationLocationMethod] NVARCHAR(200)       NULL,                           -- legacy IdentifikacijaNaMotorMestoMetod
        [GearBoxId]                         INT                  NULL,
        [KwToCcRatio]                       NVARCHAR(50)         NULL,                           -- legacy OdnosKwCcm

        -- Brakes / suspension
        [BrakesId]                          INT                  NULL,                           -- typo fix (was IdBreakes)
        [SupportingId]                      INT                  NULL,

        -- Geometry
        [VehicleHeight]                     DECIMAL(10,2)        NULL,                           -- typo fix (was VehicleSizeHight); BR-VEH-104
        [VehicleWidth]                      DECIMAL(10,2)        NULL,
        [VehicleLength]                     DECIMAL(10,2)        NULL,

        -- Doors / seats
        [NumberOfDoors]                     INT                  NULL,                           -- BR-VEH-092
        [NumberOfSeats]                     SMALLINT             NULL,                           -- BR-VEH-093
        [NumberOfStandingSeats]             SMALLINT             NULL,
        [NumberOfLyingSeats]                SMALLINT             NULL,                           -- typo fix (was Lieing)

        -- Wheels / axles
        [NumberOfAxes]                      INT                  NULL,                           -- BR-VEH-006
        [NumberOfPropulsionAxes]            INT                  NULL,                           -- legacy PropulsionAxis
        [NumberOfWheels]                    INT                  NULL,                           -- BR-VEH-007
        [NumberOfPropulsionWheels]          INT                  NULL,

        -- Mass (basic)
        [EmptyWeight]                       DECIMAL(10,2)        NULL,                           -- typo fix; BR-VEH-094, 105
        [MaxAllowedWeight]                  DECIMAL(10,2)        NULL,                           -- typo fix; BR-VEH-095, 105
        [MaxConstructiveTotalMass]          DECIMAL(10,2)        NULL,                           -- legacy MaxKonstVkMasa
        [MaxLegalTotalMass]                 DECIMAL(10,2)        NULL,
        [MaxLegalTotalMassGroup]            DECIMAL(10,2)        NULL,

        -- Mass (per axle, ax 1-5 + trailer)
        [AxleLoad1]                         INT                  NULL,                           -- legacy MasaPoOska1
        [AxleLoad2]                         INT                  NULL,
        [AxleLoad3]                         INT                  NULL,
        [AxleLoad4]                         INT                  NULL,
        [AxleLoad5]                         INT                  NULL,
        [TrailerAxleLoad]                   INT                  NULL,                           -- legacy MasaPoOskaPriklucna
        [AxleBaseLoad1]                     INT                  NULL,                           -- legacy OsnoOptovaruvanje1
        [AxleBaseLoad2]                     INT                  NULL,
        [AxleBaseLoad3]                     INT                  NULL,
        [AxleBaseLoad4]                     INT                  NULL,
        [AxleBaseLoad5]                     INT                  NULL,
        [TrailerAxleBaseLoad]               INT                  NULL,

        -- Trailer / coupling specs
        [TrailerWeightBraked]               NVARCHAR(20)         NULL,                           -- typo fix (was TrailerWaightWithBreak)
        [TrailerWeightUnbraked]             NVARCHAR(20)         NULL,                           -- typo fix
        [MaxConstructiveBrakedTrailerMass]  INT                  NULL,
        [MaxConstructiveUnbrakedTrailerMass] INT                 NULL,
        [MaxConstructiveCouplingLoad]       INT                  NULL,
        [MaxConstructiveCombinationMass]    INT                  NULL,
        [MaxConstructiveSemiTrailerMass]    INT                  NULL,
        [MaxConstructiveTrailerMass]        INT                  NULL,
        [MaxConstructiveTrailerMassWithCentralAxle] INT          NULL,
        [MaxConstructiveAttachableTrailerMass] INT               NULL,
        [MaxHorizontalVerticalCouplingLoad] INT                  NULL,
        [MinMass]                           INT                  NULL,
        [MechanicalCouplingType]            NVARCHAR(100)        NULL,
        [MechanicalCouplingMark]            NVARCHAR(100)        NULL,
        [MechanicalCouplingApprovalNumber]  NVARCHAR(100)        NULL,
        [CouplingDeviceApprovalMark]        NVARCHAR(100)        NULL,

        -- Type approval / homologation
        [HomologationCertificateNumber]     NVARCHAR(100)        NULL,                           -- typo fix; BR-VEH-099
        [ApprovalMark]                      NVARCHAR(100)        NULL,
        [EUCertificateNumber]               NVARCHAR(100)        NULL,
        [VariantImplementation]             NVARCHAR(100)        NULL,
        [Type]                              NVARCHAR(100)        NULL,                           -- legacy "Tip"

        -- Performance
        [MaxSpeed]                          DECIMAL(6,2)         NULL,
        [TempOfEngineOil]                   DECIMAL(6,2)         NULL,

        -- Emissions / noise
        [NoiseStatic]                       DECIMAL(6,2)         NULL,
        [NoiseMovement]                     DECIMAL(6,2)         NULL,                           -- typo fix
        [NoiseTechnicalSpec]                NVARCHAR(50)         NULL,
        [Co]                                DECIMAL(6,3)         NULL,
        [Hc]                                DECIMAL(6,3)         NULL,
        [NOx]                               DECIMAL(6,3)         NULL,
        [HCNOx]                             DECIMAL(6,3)         NULL,
        [Co2]                               DECIMAL(6,3)         NULL,
        [Blackening]                        NVARCHAR(20)         NULL,                           -- BR-VEH-100
        [Pinpoints]                         NVARCHAR(20)         NULL,
        [FuelConsumption]                   NVARCHAR(20)         NULL,                           -- BR-VEH-102
        [CapacityFuelTank]                  DECIMAL(8,2)         NULL,

        -- Color
        [ColorCode]                         NVARCHAR(20)         NULL,
        [PrimaryColorId]                    INT                  NULL,
        [SecondaryColorId]                  INT                  NULL,

        -- Build dates
        [MakeDate]                          DATE                 NULL,                           -- BR-VEH-096

        -- Registration history (denormalized; full history in VehicleRegistrations)
        [FirstRegistrationNumber]           NVARCHAR(50)         NOT NULL,                       -- BR-VEH-004
        [LastRegistrationNumber]            NVARCHAR(50)         NOT NULL,                       -- BR-VEH-005 (typo fix)
        [FirstRegistrationMakeDate]         DATE                 NULL,
        [FirstRegistrationValidTill]        DATE                 NULL,
        [LastRegistrationMakeDate]          DATE                 NULL,
        [LastRegistrationValidTill]         DATE                 NULL,
        [FirstRegistrationIssuerId]         INT                  NULL,
        [LastRegistrationIssuerId]          INT                  NULL,

        -- Equipment flags
        [Suffocation]                       BIT                  NOT NULL CONSTRAINT [DF_Vehicles_Suffocation]                DEFAULT (0),
        [Hook]                              BIT                  NOT NULL CONSTRAINT [DF_Vehicles_Hook]                       DEFAULT (0),
        [Winch]                             BIT                  NOT NULL CONSTRAINT [DF_Vehicles_Winch]                      DEFAULT (0), -- legacy "Vitlo"
        [VerticalBurdenOnTheSeat]           BIT                  NOT NULL CONSTRAINT [DF_Vehicles_VerticalBurdenOnTheSeat]    DEFAULT (0),
        [VerticalBurdenOnTheSeatNote]       NVARCHAR(200)        NULL,
        [TNG]                               BIT                  NOT NULL CONSTRAINT [DF_Vehicles_TNG]                        DEFAULT (0), -- compressed natural gas
        [ProtectiveCabin]                   NVARCHAR(50)         NULL,
        [ProtectiveFrame]                   NVARCHAR(50)         NULL,
        [IsSocialNotPrivate]                BIT                  NOT NULL CONSTRAINT [DF_Vehicles_IsSocialNotPrivate]         DEFAULT (0), -- Q-012
        [ForPrivateTransportNotPublic]      BIT                  NOT NULL CONSTRAINT [DF_Vehicles_ForPrivateTransportNotPublic] DEFAULT (0),

        -- Free-text
        [Note]                              NVARCHAR(500)        NULL,                           -- BR-VEH-103

        -- Soft-delete
        [IsActive]                          BIT                  NOT NULL CONSTRAINT [DF_Vehicles_IsActive]                   DEFAULT (1),

        -- Audit
        [CreatedUtc]                        DATETIME2            NOT NULL CONSTRAINT [DF_Vehicles_CreatedUtc]                 DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]                   DATETIME2            NOT NULL CONSTRAINT [DF_Vehicles_LastModifiedUtc]            DEFAULT (SYSUTCDATETIME()),
        [CreatedByUserId]                   NVARCHAR(450)        NULL,
        [LastModifiedByUserId]              NVARCHAR(450)        NULL,

        -- Concurrency
        [RowVersion]                        ROWVERSION           NOT NULL,

        CONSTRAINT [PK_Vehicles] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_Vehicles_Stations_StationId]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_Vehicles_AspNetUsers_CreatedByUserId]
            FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [FK_Vehicles_AspNetUsers_LastModifiedByUserId]
            FOREIGN KEY ([LastModifiedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [CK_Vehicles_AxesGEPropulsion]   CHECK ([NumberOfAxes]   IS NULL OR [NumberOfPropulsionAxes]   IS NULL OR [NumberOfAxes]   >= [NumberOfPropulsionAxes]),    -- BR-VEH-006
        CONSTRAINT [CK_Vehicles_WheelsGEPropulsion] CHECK ([NumberOfWheels] IS NULL OR [NumberOfPropulsionWheels] IS NULL OR [NumberOfWheels] >= [NumberOfPropulsionWheels])  -- BR-VEH-007
    );

    -- BR-VEH-003: VIN unique per tenant
    CREATE UNIQUE INDEX [UX_Vehicles_StationId_ShellNumber]
        ON [dbo].[Vehicles]([StationId], [ShellNumber]);

    -- Search by registration number
    CREATE INDEX [IX_Vehicles_StationId_LastRegistrationNumber]
        ON [dbo].[Vehicles]([StationId], [LastRegistrationNumber]);

    -- Tenant filter
    CREATE INDEX [IX_Vehicles_StationId]
        ON [dbo].[Vehicles]([StationId]);

    PRINT 'Table [dbo].[Vehicles] created.';
END
ELSE
    PRINT 'Table [dbo].[Vehicles] already exists.';
GO

-- 2. VehicleRegistrations (registration history)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleRegistrations')
BEGIN
    CREATE TABLE [dbo].[VehicleRegistrations] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [RegistrationNumber] NVARCHAR(50)        NOT NULL,
        [MakeDate]          DATE                 NULL,
        [ValidTill]         DATE                 NULL,
        [IssuerId]          INT                  NULL,                                            -- FK → RegistrationIssuers (deferred)
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_VehicleRegistrations_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_VehicleRegistrations_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_VehicleRegistrations] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleRegistrations_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_VehicleRegistrations_VehicleId] ON [dbo].[VehicleRegistrations]([VehicleId]);
    PRINT 'Table [dbo].[VehicleRegistrations] created.';
END
ELSE
    PRINT 'Table [dbo].[VehicleRegistrations] already exists.';
GO

-- 3. VehicleAxles (legacy: Vehicle.Axis — per-vehicle axle inventory)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleAxles')
BEGIN
    CREATE TABLE [dbo].[VehicleAxles] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [AxleNumber]        INT                  NOT NULL,
        [IsPropulsion]      BIT                  NOT NULL CONSTRAINT [DF_VehicleAxles_IsPropulsion]      DEFAULT (0),
        [IsSteering]        BIT                  NOT NULL CONSTRAINT [DF_VehicleAxles_IsSteering]        DEFAULT (0),
        [Note]              NVARCHAR(200)        NULL,
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_VehicleAxles_CreatedUtc]        DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_VehicleAxles_LastModifiedUtc]   DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_VehicleAxles] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleAxles_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE CASCADE,
        CONSTRAINT [UQ_VehicleAxles_VehicleId_AxleNumber] UNIQUE ([VehicleId], [AxleNumber])
    );
    PRINT 'Table [dbo].[VehicleAxles] created.';
END
ELSE
    PRINT 'Table [dbo].[VehicleAxles] already exists.';
GO

-- 4. VehicleAxleDistances (legacy: Vehicle.BetweenAxesDestinations — distances between axles)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleAxleDistances')
BEGIN
    CREATE TABLE [dbo].[VehicleAxleDistances] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [FromAxleNumber]    INT                  NOT NULL,
        [ToAxleNumber]      INT                  NOT NULL,
        [Distance]          DECIMAL(8,2)         NULL,                           -- in mm or cm — confirm with regulatory unit
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_VehicleAxleDistances_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_VehicleAxleDistances_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_VehicleAxleDistances] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleAxleDistances_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE CASCADE,
        CONSTRAINT [CK_VehicleAxleDistances_FromLTTo] CHECK ([FromAxleNumber] < [ToAxleNumber])
    );
    PRINT 'Table [dbo].[VehicleAxleDistances] created.';
END
ELSE
    PRINT 'Table [dbo].[VehicleAxleDistances] already exists.';
GO

-- 5. VehicleTyres (legacy: Vehicle.Tyres — per-vehicle tyre inventory; not the catalog REF table)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleTyres')
BEGIN
    CREATE TABLE [dbo].[VehicleTyres] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [TireTypeId]        INT                  NULL,                                            -- FK → VehicleTireTypes (deferred REF)
        [PositionNote]      NVARCHAR(50)         NULL,                                            -- e.g. "front-left", "rear-right"
        [Dimensions]        NVARCHAR(50)         NULL,                                            -- e.g. "205/55 R16"
        [PressureFront]     DECIMAL(5,2)         NULL,
        [PressureRear]      DECIMAL(5,2)         NULL,
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_VehicleTyres_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_VehicleTyres_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_VehicleTyres] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleTyres_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE CASCADE
    );
    PRINT 'Table [dbo].[VehicleTyres] created.';
END
ELSE
    PRINT 'Table [dbo].[VehicleTyres] already exists.';
GO

-- 6. CustomerVehicleRelations (legacy: CustomerVehiclesRelations — many-to-many between Customers and Vehicles with relation type)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'CustomerVehicleRelations')
BEGIN
    CREATE TABLE [dbo].[CustomerVehicleRelations] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [CustomerId]        BIGINT               NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [RelationTypeId]    INT                  NULL,                                            -- FK → CustomerVehicleRelationTypes (deferred REF)
        [ValidFrom]         DATE                 NULL,
        [ValidTo]           DATE                 NULL,                                            -- NULL = current
        [Note]              NVARCHAR(200)        NULL,
        [IsActive]          BIT                  NOT NULL CONSTRAINT [DF_CustomerVehicleRelations_IsActive]        DEFAULT (1),
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_CustomerVehicleRelations_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_CustomerVehicleRelations_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_CustomerVehicleRelations] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_CustomerVehicleRelations_Customers_CustomerId]
            FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers]([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CustomerVehicleRelations_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_CustomerVehicleRelations_CustomerId] ON [dbo].[CustomerVehicleRelations]([CustomerId]);
    CREATE INDEX [IX_CustomerVehicleRelations_VehicleId]  ON [dbo].[CustomerVehicleRelations]([VehicleId]);
    PRINT 'Table [dbo].[CustomerVehicleRelations] created.';
END
ELSE
    PRINT 'Table [dbo].[CustomerVehicleRelations] already exists.';
GO

PRINT '';
PRINT '=== Vehicles module schema applied ===';
SELECT name AS [Tables in VTE2 (Vehicles module)]
FROM sys.tables
WHERE name IN (N'Vehicles', N'VehicleRegistrations', N'VehicleAxles', N'VehicleAxleDistances', N'VehicleTyres', N'CustomerVehicleRelations')
ORDER BY name;
GO


-- =============================================================================
-- SECTION 5 / 9 — Requests module schema (RequestTypes + Requests + proofs)
-- Source: docs\superpowers\work\bootstrap-requests.sql
-- =============================================================================

-- VTE2 — Requests module schema
-- Generated: 2026-04-30
-- Source: docs/superpowers/work/requests-business-rules.md (rules BR-REQ-001..021)
--         docs/superpowers/specs/2026-04-30-vte-domain-audit.md §4.4.2
--
-- SAFE TO RE-RUN: idempotent.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- 1. RequestTypes (configurable workflow definitions; technically REF, but included
--    here because Request depends on its 10 flags)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'RequestTypes')
BEGIN
    CREATE TABLE [dbo].[RequestTypes] (
        [Id]                              INT IDENTITY(1,1) NOT NULL,
        [ParentRequestTypeId]             INT               NULL,                                -- self-reference (Q-015)
        [DocumentPrintId]                 INT               NULL,                                -- FK → DocumentTypePrint (deferred); selects Plav/Bel/Zelen
        [TypeName]                        NVARCHAR(250)     NOT NULL,                            -- BR-REQ-020
        [TypeDescription]                 NVARCHAR(250)     NULL,                                -- BR-REQ-021

        -- Workflow flags (drive Request validation and side-effects)
        [IsTechnicalExamRequired]         INT               NOT NULL CONSTRAINT [DF_RequestTypes_IsTechnicalExamRequired]      DEFAULT (0),  -- tri-state: 0=no, 1=yes, 2=conditional
        [IsPayRequired]                   BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsPayRequired]                DEFAULT (0),
        [IsNewRegistration]               BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsNewRegistration]            DEFAULT (0),
        [IsRelationDeleted]               BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsRelationDeleted]            DEFAULT (0),
        [IsVehicleDeleted]                BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsVehicleDeleted]             DEFAULT (0),
        [IsNewCustomer]                   BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsNewCustomer]                DEFAULT (0),
        [IsVehicleChanged]                BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsVehicleChanged]             DEFAULT (0),
        [IsCustomerChanged]               BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsCustomerChanged]            DEFAULT (0),
        [IsSufficient]                    BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsSufficient]                 DEFAULT (0),
        [IsPreviousRegistrationRequired]  BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsPreviousRegistrationRequired] DEFAULT (0),

        [IsActive]                        BIT               NOT NULL CONSTRAINT [DF_RequestTypes_IsActive]                     DEFAULT (1),
        [CreatedUtc]                      DATETIME2         NOT NULL CONSTRAINT [DF_RequestTypes_CreatedUtc]                   DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]                 DATETIME2         NOT NULL CONSTRAINT [DF_RequestTypes_LastModifiedUtc]              DEFAULT (SYSUTCDATETIME()),
        [RowVersion]                      ROWVERSION        NOT NULL,

        CONSTRAINT [PK_RequestTypes] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_RequestTypes_RequestTypes_ParentRequestTypeId]
            FOREIGN KEY ([ParentRequestTypeId]) REFERENCES [dbo].[RequestTypes]([Id])
    );
    PRINT 'Table [dbo].[RequestTypes] created.';
END
ELSE
    PRINT 'Table [dbo].[RequestTypes] already exists.';
GO

-- 2. Requests (the workflow instance — every customer interaction starts here)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Requests')
BEGIN
    CREATE TABLE [dbo].[Requests] (
        [Id]                              BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]                       INT                  NOT NULL,
        [RequestTypeId]                   INT                  NOT NULL,                          -- BR-REQ-003
        [CustomerVehicleRelationId]       BIGINT               NOT NULL,                          -- BR-REQ-004 (current owner relation)
        [NewCustomerVehicleRelationId]    BIGINT               NULL,                              -- BR-REQ-005 (new owner if RequestType.IsNewCustomer)
        [TechnicalExamReportId]           BIGINT               NULL,                              -- FK → DocumentsTehnicalExamsReports (DOC module, deferred)
        [PreviousRegistrationId]          BIGINT               NULL,                              -- FK → VehicleRegistrations
        [TechnicalExamOrganizationId]     INT                  NULL,                              -- FK → TehnicalExamOrganizations (REF, deferred)

        -- Workflow timestamps
        [DateCreated]                     DATE                 NOT NULL,                          -- BR-REQ-001
        [DateModified]                    DATE                 NULL,
        [DateEnded]                       DATE                 NULL,                              -- NULL = request still open

        -- Workflow operators
        [CreatedByOperatorUserId]         NVARCHAR(450)        NULL,                              -- FK → AspNetUsers
        [ModifiedByOperatorUserId]        NVARCHAR(450)        NULL,
        [EndedByOperatorUserId]           NVARCHAR(450)        NULL,

        -- Result trace flags (Q-017: derived or operator-set?)
        [IsCustomerChanged]               BIT                  NOT NULL CONSTRAINT [DF_Requests_IsCustomerChanged] DEFAULT (0),
        [IsVehicleChanged]                BIT                  NOT NULL CONSTRAINT [DF_Requests_IsVehicleChanged]  DEFAULT (0),

        -- Free-text
        [Note]                            NVARCHAR(250)        NULL,                              -- BR-REQ-002

        -- Audit
        [CreatedUtc]                      DATETIME2            NOT NULL CONSTRAINT [DF_Requests_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]                 DATETIME2            NOT NULL CONSTRAINT [DF_Requests_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),

        -- Concurrency
        [RowVersion]                      ROWVERSION           NOT NULL,

        CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_Requests_Stations_StationId]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_Requests_RequestTypes_RequestTypeId]
            FOREIGN KEY ([RequestTypeId]) REFERENCES [dbo].[RequestTypes]([Id]),
        CONSTRAINT [FK_Requests_CustomerVehicleRelations_CustomerVehicleRelationId]
            FOREIGN KEY ([CustomerVehicleRelationId]) REFERENCES [dbo].[CustomerVehicleRelations]([Id]),
        CONSTRAINT [FK_Requests_CustomerVehicleRelations_NewCustomerVehicleRelationId]
            FOREIGN KEY ([NewCustomerVehicleRelationId]) REFERENCES [dbo].[CustomerVehicleRelations]([Id]),
        CONSTRAINT [FK_Requests_VehicleRegistrations_PreviousRegistrationId]
            FOREIGN KEY ([PreviousRegistrationId]) REFERENCES [dbo].[VehicleRegistrations]([Id]),
        CONSTRAINT [FK_Requests_AspNetUsers_CreatedByOperatorUserId]
            FOREIGN KEY ([CreatedByOperatorUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [FK_Requests_AspNetUsers_ModifiedByOperatorUserId]
            FOREIGN KEY ([ModifiedByOperatorUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [FK_Requests_AspNetUsers_EndedByOperatorUserId]
            FOREIGN KEY ([EndedByOperatorUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        -- BR-REQ-005: when the workflow says new-customer required, force the field
        -- (enforced at app layer; CHECK can't query RequestTypes — left to API)
        CONSTRAINT [CK_Requests_DateOrdering]
            CHECK ([DateModified] IS NULL OR [DateModified] >= [DateCreated]),
        CONSTRAINT [CK_Requests_EndedAfterCreated]
            CHECK ([DateEnded]    IS NULL OR [DateEnded]    >= [DateCreated])
    );

    CREATE INDEX [IX_Requests_StationId_DateCreated]   ON [dbo].[Requests]([StationId], [DateCreated]);
    CREATE INDEX [IX_Requests_RequestTypeId]           ON [dbo].[Requests]([RequestTypeId]);
    CREATE INDEX [IX_Requests_CustomerVehicleRelation] ON [dbo].[Requests]([CustomerVehicleRelationId]);
    CREATE INDEX [IX_Requests_OpenRequests]            ON [dbo].[Requests]([StationId]) WHERE [DateEnded] IS NULL;

    PRINT 'Table [dbo].[Requests] created.';
END
ELSE
    PRINT 'Table [dbo].[Requests] already exists.';
GO

-- 3. RequestVehicleOwnershipProofs (child of Request)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'RequestVehicleOwnershipProofs')
BEGIN
    CREATE TABLE [dbo].[RequestVehicleOwnershipProofs] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [RequestId]         BIGINT               NOT NULL,
        [OwnershipProofId]  INT                  NULL,                                            -- FK → DocumentVehicleOwnershipProof (REF, deferred)
        [Number]            NVARCHAR(100)        NULL,
        [DateIssued]        DATE                 NULL,
        [Note]              NVARCHAR(250)        NULL,
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_RequestVehicleOwnershipProofs_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_RequestVehicleOwnershipProofs_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_RequestVehicleOwnershipProofs] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_RequestVehicleOwnershipProofs_Requests_RequestId]
            FOREIGN KEY ([RequestId]) REFERENCES [dbo].[Requests]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_RequestVehicleOwnershipProofs_RequestId] ON [dbo].[RequestVehicleOwnershipProofs]([RequestId]);
    PRINT 'Table [dbo].[RequestVehicleOwnershipProofs] created.';
END
ELSE
    PRINT 'Table [dbo].[RequestVehicleOwnershipProofs] already exists.';
GO

-- 4. RequestPaymentProofs (child of Request)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'RequestPaymentProofs')
BEGIN
    CREATE TABLE [dbo].[RequestPaymentProofs] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [RequestId]         BIGINT               NOT NULL,
        [PaymentProofId]    INT                  NULL,                                            -- FK → DocumentPaymentProof (REF, deferred)
        [Number]            NVARCHAR(100)        NULL,
        [Amount]            DECIMAL(15,2)        NULL,                                            -- the amount of the proof, copy of payment-document amount for traceability
        [DateIssued]        DATE                 NULL,
        [Note]              NVARCHAR(250)        NULL,
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_RequestPaymentProofs_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_RequestPaymentProofs_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_RequestPaymentProofs] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_RequestPaymentProofs_Requests_RequestId]
            FOREIGN KEY ([RequestId]) REFERENCES [dbo].[Requests]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_RequestPaymentProofs_RequestId] ON [dbo].[RequestPaymentProofs]([RequestId]);
    PRINT 'Table [dbo].[RequestPaymentProofs] created.';
END
ELSE
    PRINT 'Table [dbo].[RequestPaymentProofs] already exists.';
GO

PRINT '';
PRINT '=== Requests module schema applied ===';
SELECT name AS [Tables in VTE2 (Requests module)]
FROM sys.tables
WHERE name IN (N'RequestTypes', N'Requests', N'RequestVehicleOwnershipProofs', N'RequestPaymentProofs')
ORDER BY name;
GO


-- =============================================================================
-- SECTION 6 / 9 — Payments module schema
-- Source: docs\superpowers\work\bootstrap-payments.sql
-- =============================================================================

-- VTE2 — Payments module schema
-- Generated: 2026-04-30
-- Source: docs/superpowers/work/payments-business-rules.md (rules BR-PAY-001..043)
--         docs/superpowers/specs/2026-04-30-vte-domain-audit.md §4.6.2
--
-- SAFE TO RE-RUN: idempotent.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- 1. PaymentTypes (REF lookup — small, included here)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'PaymentTypes')
BEGIN
    CREATE TABLE [dbo].[PaymentTypes] (
        [Id]                  INT IDENTITY(1,1) NOT NULL,
        [Name]                NVARCHAR(100)     NOT NULL,
        [IsInvoice]           BIT               NOT NULL CONSTRAINT [DF_PaymentTypes_IsInvoice]            DEFAULT (0),  -- legacy "Faktura" flag
        [IsCash]              BIT               NOT NULL CONSTRAINT [DF_PaymentTypes_IsCash]               DEFAULT (0),  -- legacy "Fiskalna_kes"
        [IsFiscalCard]        BIT               NOT NULL CONSTRAINT [DF_PaymentTypes_IsFiscalCard]         DEFAULT (0),  -- legacy "Fiskalna_karticka"
        [IsAccount]           BIT               NOT NULL CONSTRAINT [DF_PaymentTypes_IsAccount]            DEFAULT (0),  -- legacy "Smetka"
        [IsInstallments]      BIT               NOT NULL CONSTRAINT [DF_PaymentTypes_IsInstallments]       DEFAULT (0),  -- legacy "Rati"
        [IsActive]            BIT               NOT NULL CONSTRAINT [DF_PaymentTypes_IsActive]             DEFAULT (1),
        [CreatedUtc]          DATETIME2         NOT NULL CONSTRAINT [DF_PaymentTypes_CreatedUtc]           DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]     DATETIME2         NOT NULL CONSTRAINT [DF_PaymentTypes_LastModifiedUtc]      DEFAULT (SYSUTCDATETIME()),
        [RowVersion]          ROWVERSION        NOT NULL,
        CONSTRAINT [PK_PaymentTypes] PRIMARY KEY CLUSTERED ([Id])
    );
    PRINT 'Table [dbo].[PaymentTypes] created.';
END
GO

-- 2. CalculationItems (fee catalog — each item has its own target bank account)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'CalculationItems')
BEGIN
    CREATE TABLE [dbo].[CalculationItems] (
        [Id]                  INT IDENTITY(1,1) NOT NULL,
        [StationId]           INT               NOT NULL,
        [ItemName]            NVARCHAR(150)     NOT NULL,                                              -- BR-PAY-040
        [BankAccount]         NVARCHAR(50)      NOT NULL,                                              -- BR-PAY-041
        [Bank]                NVARCHAR(150)     NOT NULL,                                              -- BR-PAY-042
        [Form]                NVARCHAR(50)      NOT NULL,                                              -- BR-PAY-043
        [IsActive]            BIT               NOT NULL CONSTRAINT [DF_CalculationItems_IsActive]        DEFAULT (1),
        [CreatedUtc]          DATETIME2         NOT NULL CONSTRAINT [DF_CalculationItems_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]     DATETIME2         NOT NULL CONSTRAINT [DF_CalculationItems_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]          ROWVERSION        NOT NULL,
        CONSTRAINT [PK_CalculationItems] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_CalculationItems_Stations_StationId]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id])
    );
    CREATE INDEX [IX_CalculationItems_StationId] ON [dbo].[CalculationItems]([StationId]);
    PRINT 'Table [dbo].[CalculationItems] created.';
END
GO

-- 3. DDVCatalog (VAT rates with effective dates)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'DDVCatalog')
BEGIN
    CREATE TABLE [dbo].[DDVCatalog] (
        [Id]                  INT IDENTITY(1,1) NOT NULL,
        [Name]                NVARCHAR(100)     NOT NULL,                                              -- e.g. "ДДВ 18%"
        [Rate]                DECIMAL(5,2)      NOT NULL,                                              -- the VAT percentage, e.g. 18.00
        [EffectiveFrom]       DATE              NOT NULL,
        [EffectiveTo]         DATE              NULL,                                                  -- NULL = currently in force
        [IsActive]            BIT               NOT NULL CONSTRAINT [DF_DDVCatalog_IsActive]        DEFAULT (1),
        [CreatedUtc]          DATETIME2         NOT NULL CONSTRAINT [DF_DDVCatalog_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]     DATETIME2         NOT NULL CONSTRAINT [DF_DDVCatalog_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]          ROWVERSION        NOT NULL,
        CONSTRAINT [PK_DDVCatalog] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [CK_DDVCatalog_RateRange]    CHECK ([Rate] >= 0 AND [Rate] <= 100),
        CONSTRAINT [CK_DDVCatalog_DateOrdering] CHECK ([EffectiveTo] IS NULL OR [EffectiveTo] >= [EffectiveFrom])
    );
    PRINT 'Table [dbo].[DDVCatalog] created.';
END
GO

-- 4. PriceCatalog (the price list — per-tenant)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'PriceCatalog')
BEGIN
    CREATE TABLE [dbo].[PriceCatalog] (
        [Id]                  INT IDENTITY(1,1) NOT NULL,
        [StationId]           INT               NOT NULL,
        [CalculationItemId]   INT               NULL,                                                  -- which fee category
        [Name]                NVARCHAR(200)     NOT NULL,
        [Price]               DECIMAL(15,2)     NOT NULL,
        [DDVId]               INT               NULL,                                                  -- which VAT rate applies
        [EffectiveFrom]       DATE              NULL,
        [EffectiveTo]         DATE              NULL,
        [IsActive]            BIT               NOT NULL CONSTRAINT [DF_PriceCatalog_IsActive]        DEFAULT (1),
        [CreatedUtc]          DATETIME2         NOT NULL CONSTRAINT [DF_PriceCatalog_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]     DATETIME2         NOT NULL CONSTRAINT [DF_PriceCatalog_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]          ROWVERSION        NOT NULL,
        CONSTRAINT [PK_PriceCatalog] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_PriceCatalog_Stations_StationId]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_PriceCatalog_CalculationItems_CalculationItemId]
            FOREIGN KEY ([CalculationItemId]) REFERENCES [dbo].[CalculationItems]([Id]),
        CONSTRAINT [FK_PriceCatalog_DDVCatalog_DDVId]
            FOREIGN KEY ([DDVId]) REFERENCES [dbo].[DDVCatalog]([Id]),
        CONSTRAINT [CK_PriceCatalog_PriceNonNegative] CHECK ([Price] >= 0)
    );
    CREATE INDEX [IX_PriceCatalog_StationId] ON [dbo].[PriceCatalog]([StationId]);
    PRINT 'Table [dbo].[PriceCatalog] created.';
END
GO

-- 5. InstallmentContracts (legacy DogovorZaRati)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'InstallmentContracts')
BEGIN
    CREATE TABLE [dbo].[InstallmentContracts] (
        [Id]                  BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]           INT                  NOT NULL,
        [ContractNumber]      NVARCHAR(50)         NOT NULL,                                          -- BR-PAY-030 (legacy "Broj")
        [ContractDate]        DATE                 NOT NULL,                                          -- legacy "Datum"
        [NumberOfInstallments] INT                 NOT NULL,                                          -- BR-PAY-034 (legacy "BrNaRati")
        -- Guarantor
        [GuarantorName]       NVARCHAR(50)         NULL,                                              -- BR-PAY-031 (legacy "GarantNaziv")
        [GuarantorAddress]    NVARCHAR(250)        NULL,                                              -- BR-PAY-032 (legacy "GarantAdresa")
        [GuarantorEMBG]       NVARCHAR(20)         NULL,                                              -- BR-PAY-033 (legacy "GartEMB" — typo in legacy)
        [Note]                NVARCHAR(500)        NULL,
        [CreatedUtc]          DATETIME2            NOT NULL CONSTRAINT [DF_InstallmentContracts_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]     DATETIME2            NOT NULL CONSTRAINT [DF_InstallmentContracts_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]          ROWVERSION           NOT NULL,
        CONSTRAINT [PK_InstallmentContracts] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_InstallmentContracts_Stations_StationId]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [CK_InstallmentContracts_NumberOfInstallments] CHECK ([NumberOfInstallments] >= 2 AND [NumberOfInstallments] <= 60),
        CONSTRAINT [UQ_InstallmentContracts_Station_Number] UNIQUE ([StationId], [ContractNumber])
    );
    PRINT 'Table [dbo].[InstallmentContracts] created.';
END
GO

-- 6. PaymentDocuments (the bill/receipt — root of the payment workflow)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'PaymentDocuments')
BEGIN
    CREATE TABLE [dbo].[PaymentDocuments] (
        [Id]                          BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]                   INT                  NOT NULL,
        [DocumentNumber]              NVARCHAR(50)         NOT NULL,                                  -- generated; numbering scheme TBD (Q-023)
        [PaymentTypeId]               INT                  NOT NULL,                                  -- BR-PAY-004
        [CustomerVehicleRelationId]   BIGINT               NOT NULL,                                  -- BR-PAY-005
        [InstallmentContractId]       BIGINT               NULL,                                      -- legacy IdDogovor (NULL when not on installments)
        [BillToCustomerId]            BIGINT               NULL,                                      -- legacy IdFakturiraNa (Q-027 — B2B billing override)
        [TechnicalExamOrganizationId] INT                  NULL,                                      -- legacy IdOrganization
        [DatePay]                     DATE                 NOT NULL,                                  -- BR-PAY-001
        [DateRequired]                DATE                 NOT NULL,                                  -- BR-PAY-002 (when payment is due)
        [DiscountPercent]             DECIMAL(5,2)         NOT NULL CONSTRAINT [DF_PaymentDocuments_DiscountPercent] DEFAULT (0),  -- document-level discount
        [Payed]                       BIT                  NOT NULL CONSTRAINT [DF_PaymentDocuments_Payed]   DEFAULT (0),
        [Storno]                      BIT                  NOT NULL CONSTRAINT [DF_PaymentDocuments_Storno]  DEFAULT (0),
        [PolicyNumber]                DECIMAL(15,0)        NULL,                                      -- legacy "Polisa" (Q-026)
        [Note]                        NVARCHAR(150)        NULL,                                      -- BR-PAY-003
        [CreatedByOperatorUserId]     NVARCHAR(450)        NULL,                                      -- legacy IdOperator
        [CreatedUtc]                  DATETIME2            NOT NULL CONSTRAINT [DF_PaymentDocuments_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]             DATETIME2            NOT NULL CONSTRAINT [DF_PaymentDocuments_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]                  ROWVERSION           NOT NULL,
        CONSTRAINT [PK_PaymentDocuments] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_PaymentDocuments_Stations_StationId]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_PaymentDocuments_PaymentTypes_PaymentTypeId]
            FOREIGN KEY ([PaymentTypeId]) REFERENCES [dbo].[PaymentTypes]([Id]),
        CONSTRAINT [FK_PaymentDocuments_CustomerVehicleRelations_CustomerVehicleRelationId]
            FOREIGN KEY ([CustomerVehicleRelationId]) REFERENCES [dbo].[CustomerVehicleRelations]([Id]),
        CONSTRAINT [FK_PaymentDocuments_InstallmentContracts_InstallmentContractId]
            FOREIGN KEY ([InstallmentContractId]) REFERENCES [dbo].[InstallmentContracts]([Id]),
        CONSTRAINT [FK_PaymentDocuments_Customers_BillToCustomerId]
            FOREIGN KEY ([BillToCustomerId]) REFERENCES [dbo].[Customers]([Id]),
        CONSTRAINT [FK_PaymentDocuments_AspNetUsers_CreatedByOperatorUserId]
            FOREIGN KEY ([CreatedByOperatorUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [CK_PaymentDocuments_DiscountRange] CHECK ([DiscountPercent] >= 0 AND [DiscountPercent] < 100),  -- BR-PAY-012 at doc level
        CONSTRAINT [UQ_PaymentDocuments_Station_DocumentNumber] UNIQUE ([StationId], [DocumentNumber])
    );
    CREATE INDEX [IX_PaymentDocuments_StationId_DatePay]      ON [dbo].[PaymentDocuments]([StationId], [DatePay]);
    CREATE INDEX [IX_PaymentDocuments_CustomerVehicleRelation] ON [dbo].[PaymentDocuments]([CustomerVehicleRelationId]);
    CREATE INDEX [IX_PaymentDocuments_InstallmentContract]    ON [dbo].[PaymentDocuments]([InstallmentContractId]) WHERE [InstallmentContractId] IS NOT NULL;
    CREATE INDEX [IX_PaymentDocuments_Unpaid]                  ON [dbo].[PaymentDocuments]([StationId]) WHERE [Payed] = 0 AND [Storno] = 0;
    PRINT 'Table [dbo].[PaymentDocuments] created.';
END
GO

-- 7. PaymentDocumentDetails (line items)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'PaymentDocumentDetails')
BEGIN
    CREATE TABLE [dbo].[PaymentDocumentDetails] (
        [Id]                       BIGINT IDENTITY(1,1) NOT NULL,
        [PaymentDocumentId]        BIGINT               NOT NULL,
        [PriceCatalogId]           INT                  NOT NULL,                                     -- BR-PAY-013
        [Price]                    DECIMAL(15,2)        NOT NULL,                                     -- the line price (snapshot of catalog at issue time)
        [DDVRate]                  DECIMAL(5,2)         NOT NULL CONSTRAINT [DF_PaymentDocumentDetails_DDVRate] DEFAULT (0),  -- VAT % at issue time
        [DiscountPercent]          DECIMAL(5,2)         NOT NULL CONSTRAINT [DF_PaymentDocumentDetails_DiscountPercent] DEFAULT (0),  -- BR-PAY-012
        [PrePayed]                 BIT                  NOT NULL CONSTRAINT [DF_PaymentDocumentDetails_PrePayed]        DEFAULT (0),
        [Note]                     NVARCHAR(150)        NULL,                                         -- BR-PAY-010
        [NotePrePayed]             NVARCHAR(150)        NULL,                                         -- BR-PAY-011
        [CustomerFinancialStateId] BIGINT               NULL,                                         -- legacy IdCustomerFinancialState
        [CreatedUtc]               DATETIME2            NOT NULL CONSTRAINT [DF_PaymentDocumentDetails_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]          DATETIME2            NOT NULL CONSTRAINT [DF_PaymentDocumentDetails_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]               ROWVERSION           NOT NULL,
        CONSTRAINT [PK_PaymentDocumentDetails] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_PaymentDocumentDetails_PaymentDocuments]
            FOREIGN KEY ([PaymentDocumentId]) REFERENCES [dbo].[PaymentDocuments]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PaymentDocumentDetails_PriceCatalog]
            FOREIGN KEY ([PriceCatalogId]) REFERENCES [dbo].[PriceCatalog]([Id]),
        CONSTRAINT [CK_PaymentDocumentDetails_DiscountRange] CHECK ([DiscountPercent] >= 0 AND [DiscountPercent] < 100),  -- BR-PAY-012
        CONSTRAINT [CK_PaymentDocumentDetails_PriceNonNegative] CHECK ([Price] >= 0),
        CONSTRAINT [CK_PaymentDocumentDetails_DDVRange]      CHECK ([DDVRate] >= 0 AND [DDVRate] <= 100)
    );
    CREATE INDEX [IX_PaymentDocumentDetails_PaymentDocumentId] ON [dbo].[PaymentDocumentDetails]([PaymentDocumentId]);
    PRINT 'Table [dbo].[PaymentDocumentDetails] created.';
END
GO

-- 8. PaymentDocumentInstallments (legacy PaymentDocumentsRata)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'PaymentDocumentInstallments')
BEGIN
    CREATE TABLE [dbo].[PaymentDocumentInstallments] (
        [Id]                  BIGINT IDENTITY(1,1) NOT NULL,
        [PaymentDocumentId]   BIGINT               NOT NULL,
        [InstallmentNumber]   INT                  NOT NULL,                                          -- 1, 2, 3, ... within this document
        [Price]               DECIMAL(15,2)        NOT NULL,                                          -- amount of this installment
        [DueDate]             DATE                 NULL,                                              -- when the installment is due
        [Payed]               BIT                  NOT NULL CONSTRAINT [DF_PaymentDocumentInstallments_Payed] DEFAULT (0),
        [DatePayed]           DATE                 NULL,                                              -- when actually paid
        [Note]                NVARCHAR(150)        NULL,                                              -- BR-PAY-020
        [CollectedByOperatorUserId] NVARCHAR(450)  NULL,                                              -- legacy IdOperator
        [CreatedUtc]          DATETIME2            NOT NULL CONSTRAINT [DF_PaymentDocumentInstallments_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]     DATETIME2            NOT NULL CONSTRAINT [DF_PaymentDocumentInstallments_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]          ROWVERSION           NOT NULL,
        CONSTRAINT [PK_PaymentDocumentInstallments] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_PaymentDocumentInstallments_PaymentDocuments]
            FOREIGN KEY ([PaymentDocumentId]) REFERENCES [dbo].[PaymentDocuments]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PaymentDocumentInstallments_AspNetUsers_CollectedByOperatorUserId]
            FOREIGN KEY ([CollectedByOperatorUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [CK_PaymentDocumentInstallments_PriceNonNegative] CHECK ([Price] >= 0),
        CONSTRAINT [UQ_PaymentDocumentInstallments_Document_Number] UNIQUE ([PaymentDocumentId], [InstallmentNumber])
    );
    CREATE INDEX [IX_PaymentDocumentInstallments_DueDate] ON [dbo].[PaymentDocumentInstallments]([DueDate]) WHERE [Payed] = 0;
    PRINT 'Table [dbo].[PaymentDocumentInstallments] created.';
END
GO

-- 9. CustomerFinancialState (running balance per customer — preserves legacy AR-style tracking)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'CustomerFinancialState')
BEGIN
    CREATE TABLE [dbo].[CustomerFinancialState] (
        [Id]                  BIGINT IDENTITY(1,1) NOT NULL,
        [CustomerId]          BIGINT               NOT NULL,
        [StationId]           INT                  NOT NULL,
        [Date]                DATE                 NOT NULL,
        [Description]         NVARCHAR(250)        NULL,
        [DebitAmount]         DECIMAL(15,2)        NOT NULL CONSTRAINT [DF_CustomerFinancialState_DebitAmount]  DEFAULT (0),
        [CreditAmount]        DECIMAL(15,2)        NOT NULL CONSTRAINT [DF_CustomerFinancialState_CreditAmount] DEFAULT (0),
        [RunningBalance]      DECIMAL(15,2)        NOT NULL CONSTRAINT [DF_CustomerFinancialState_RunningBalance] DEFAULT (0),
        [PaymentDocumentId]   BIGINT               NULL,                                              -- if this row was triggered by a payment document
        [CreatedUtc]          DATETIME2            NOT NULL CONSTRAINT [DF_CustomerFinancialState_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [RowVersion]          ROWVERSION           NOT NULL,
        CONSTRAINT [PK_CustomerFinancialState] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_CustomerFinancialState_Customers]
            FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers]([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CustomerFinancialState_Stations]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_CustomerFinancialState_PaymentDocuments]
            FOREIGN KEY ([PaymentDocumentId]) REFERENCES [dbo].[PaymentDocuments]([Id])
    );
    CREATE INDEX [IX_CustomerFinancialState_Customer_Date] ON [dbo].[CustomerFinancialState]([CustomerId], [Date]);
    PRINT 'Table [dbo].[CustomerFinancialState] created.';
END
GO

PRINT '';
PRINT '=== Payments module schema applied ===';
SELECT name AS [Tables in VTE2 (Payments module)]
FROM sys.tables
WHERE name IN (N'PaymentTypes', N'CalculationItems', N'DDVCatalog', N'PriceCatalog', N'InstallmentContracts',
               N'PaymentDocuments', N'PaymentDocumentDetails', N'PaymentDocumentInstallments', N'CustomerFinancialState')
ORDER BY name;
GO


-- =============================================================================
-- SECTION 7 / 9 — Documents module schema (TrafficLicences, TechExamReports, ...)
-- Source: docs\superpowers\work\bootstrap-documents.sql
-- =============================================================================

-- VTE2 — Documents module schema (TrafficLicences, Permissions, IntlDrivingLicences, TechnicalExamReports)
-- Generated: 2026-04-30
-- Source: docs/superpowers/work/documents-business-rules.md (rules BR-DOC-001..453)
--         docs/superpowers/specs/2026-04-30-vte-domain-audit.md §4.5.2
--
-- SAFE TO RE-RUN: idempotent.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- 1. TrafficLicences (the road-traffic licence — issued document)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TrafficLicences')
BEGIN
    CREATE TABLE [dbo].[TrafficLicences] (
        [Id]                              BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]                       INT                  NOT NULL,
        [CustomerVehicleRelationId]       BIGINT               NOT NULL,                              -- BR-DOC-104
        [IssuingOrganizationId]           INT                  NULL,                                  -- legacy IdTehnicalExamOrganizationsIssuedBy (FK to TehnicalExamOrganizations REF, deferred)
        [TrafficLicenceNumber]            NVARCHAR(50)         NULL,                                  -- BR-DOC-100 (Q-028: required?)
        [MadeDate]                        DATE                 NOT NULL,                              -- BR-DOC-101 (issue date)
        [EndDate]                         DATE                 NOT NULL,                              -- BR-DOC-102 (expiry)
        [Note]                            NVARCHAR(250)        NULL,                                  -- BR-DOC-103
        [IsActive]                        BIT                  NOT NULL CONSTRAINT [DF_TrafficLicences_IsActive] DEFAULT (1),
        [CreatedUtc]                      DATETIME2            NOT NULL CONSTRAINT [DF_TrafficLicences_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]                 DATETIME2            NOT NULL CONSTRAINT [DF_TrafficLicences_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]                      ROWVERSION           NOT NULL,
        CONSTRAINT [PK_TrafficLicences] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_TrafficLicences_Stations]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_TrafficLicences_CustomerVehicleRelations]
            FOREIGN KEY ([CustomerVehicleRelationId]) REFERENCES [dbo].[CustomerVehicleRelations]([Id]),
        CONSTRAINT [CK_TrafficLicences_DateOrdering] CHECK ([EndDate] >= [MadeDate])
    );
    CREATE INDEX [IX_TrafficLicences_CustomerVehicleRelation] ON [dbo].[TrafficLicences]([CustomerVehicleRelationId]);
    CREATE INDEX [IX_TrafficLicences_StationId_EndDate]       ON [dbo].[TrafficLicences]([StationId], [EndDate]);  -- expiry queries
    PRINT 'Table [dbo].[TrafficLicences] created.';
END
GO

-- 2. TrafficLicenceExtensions (extensions to a traffic licence — N per licence)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TrafficLicenceExtensions')
BEGIN
    CREATE TABLE [dbo].[TrafficLicenceExtensions] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [TrafficLicenceId]  BIGINT               NOT NULL,
        [ExtensionDate]     DATE                 NOT NULL,
        [ValidTill]         DATE                 NOT NULL,
        [Note]              NVARCHAR(250)        NULL,
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_TrafficLicenceExtensions_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_TrafficLicenceExtensions_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_TrafficLicenceExtensions] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_TrafficLicenceExtensions_TrafficLicences]
            FOREIGN KEY ([TrafficLicenceId]) REFERENCES [dbo].[TrafficLicences]([Id]) ON DELETE CASCADE,
        CONSTRAINT [CK_TrafficLicenceExtensions_DateOrdering] CHECK ([ValidTill] >= [ExtensionDate])
    );
    PRINT 'Table [dbo].[TrafficLicenceExtensions] created.';
END
GO

-- 3. Permissions (legacy DocumentsPermisions — typo fix)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Permissions')
BEGIN
    CREATE TABLE [dbo].[Permissions] (
        [Id]                              BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]                       INT                  NOT NULL,
        [CustomerVehicleRelationId]       BIGINT               NOT NULL,
        [PermissionNumber]                NVARCHAR(50)         NULL,
        [PermissionTypeId]                INT                  NULL,                                  -- FK to a permissions-types REF (deferred)
        [MadeDate]                        DATE                 NOT NULL,
        [EndDate]                         DATE                 NULL,
        [Note]                            NVARCHAR(500)        NULL,
        [IsActive]                        BIT                  NOT NULL CONSTRAINT [DF_Permissions_IsActive] DEFAULT (1),
        [CreatedUtc]                      DATETIME2            NOT NULL CONSTRAINT [DF_Permissions_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]                 DATETIME2            NOT NULL CONSTRAINT [DF_Permissions_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]                      ROWVERSION           NOT NULL,
        CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_Permissions_Stations]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_Permissions_CustomerVehicleRelations]
            FOREIGN KEY ([CustomerVehicleRelationId]) REFERENCES [dbo].[CustomerVehicleRelations]([Id]),
        CONSTRAINT [CK_Permissions_DateOrdering] CHECK ([EndDate] IS NULL OR [EndDate] >= [MadeDate])
    );
    CREATE INDEX [IX_Permissions_CustomerVehicleRelation] ON [dbo].[Permissions]([CustomerVehicleRelationId]);
    PRINT 'Table [dbo].[Permissions] created.';
END
GO

-- 4. InternationalDrivingLicences (legacy DocumentsInternationalDriveingLicences — typo fix)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'InternationalDrivingLicences')
BEGIN
    CREATE TABLE [dbo].[InternationalDrivingLicences] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]         INT                  NOT NULL,
        [CustomerId]        BIGINT               NOT NULL,                                              -- driving licence is per-customer, not per-vehicle
        [LicenceNumber]     NVARCHAR(50)         NOT NULL,
        [IssuedDate]        DATE                 NOT NULL,
        [ValidTill]         DATE                 NOT NULL,
        [IssuerId]          INT                  NULL,                                                  -- FK → RegistrationIssuers (deferred)
        [Note]              NVARCHAR(500)        NULL,
        [IsActive]          BIT                  NOT NULL CONSTRAINT [DF_InternationalDrivingLicences_IsActive] DEFAULT (1),
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_InternationalDrivingLicences_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_InternationalDrivingLicences_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_InternationalDrivingLicences] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_InternationalDrivingLicences_Stations]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_InternationalDrivingLicences_Customers]
            FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers]([Id]),
        CONSTRAINT [CK_InternationalDrivingLicences_DateOrdering] CHECK ([ValidTill] >= [IssuedDate])
    );
    CREATE INDEX [IX_InternationalDrivingLicences_Customer] ON [dbo].[InternationalDrivingLicences]([CustomerId]);
    PRINT 'Table [dbo].[InternationalDrivingLicences] created.';
END
GO

-- 5. InternationalDrivingLicenceCategories (the categories an IDL is valid for)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'InternationalDrivingLicenceCategories')
BEGIN
    CREATE TABLE [dbo].[InternationalDrivingLicenceCategories] (
        [Id]                          BIGINT IDENTITY(1,1) NOT NULL,
        [InternationalDrivingLicenceId] BIGINT             NOT NULL,
        [DrivingLicenceCategoryId]    INT                  NOT NULL,                                  -- FK → DrivingLicenceCategories REF (deferred)
        [CreatedUtc]                  DATETIME2            NOT NULL CONSTRAINT [DF_InternationalDrivingLicenceCategories_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]             DATETIME2            NOT NULL CONSTRAINT [DF_InternationalDrivingLicenceCategories_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]                  ROWVERSION           NOT NULL,
        CONSTRAINT [PK_InternationalDrivingLicenceCategories] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_InternationalDrivingLicenceCategories_InternationalDrivingLicences]
            FOREIGN KEY ([InternationalDrivingLicenceId]) REFERENCES [dbo].[InternationalDrivingLicences]([Id]) ON DELETE CASCADE,
        CONSTRAINT [UQ_InternationalDrivingLicenceCategories]
            UNIQUE ([InternationalDrivingLicenceId], [DrivingLicenceCategoryId])
    );
    PRINT 'Table [dbo].[InternationalDrivingLicenceCategories] created.';
END
GO

-- 6. TechnicalExamReports (the inspection result — regulatory heart)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TechnicalExamReports')
BEGIN
    CREATE TABLE [dbo].[TechnicalExamReports] (
        [Id]                              BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]                       INT                  NOT NULL,
        [CustomerVehicleRelationId]       BIGINT               NOT NULL,
        [TechnicalExamTypeId]             INT                  NOT NULL,                              -- BR-DOC-400 (legacy IdTypeOfTehnicalExam)
        [OrganizationForTechnicalExamId]  INT                  NOT NULL,                              -- BR-DOC-401
        [FirstInspectorOperatorUserId]    NVARCHAR(450)        NOT NULL,                              -- BR-DOC-402 (typo fix: FirsControler→FirstInspector)
        [SecondInspectorOperatorUserId]   NVARCHAR(450)        NULL,                                  -- BR-DOC-404 (Q-029)
        [RegNumber]                       NVARCHAR(50)         NULL,
        [MadeDate]                        DATE                 NOT NULL,
        [ValidTillDate]                   DATE                 NOT NULL,                              -- BR-DOC-403
        [VehicleIsRight]                  BIT                  NOT NULL CONSTRAINT [DF_TechnicalExamReports_VehicleIsRight] DEFAULT (1),  -- pass/fail

        -- Brake-force tests (per axle 1..4 + parking) — Q-030 abbreviations, Q-031 units
        [Axis1Left]              DECIMAL(8,2) NULL, [Axis1Right]              DECIMAL(8,2) NULL, [Axis1Gj]              DECIMAL(8,2) NULL, [Axis1LeftRightDiff]              DECIMAL(8,2) NULL, [Axis1Coefficient]              DECIMAL(8,2) NULL,
        [Axis2Left]              DECIMAL(8,2) NULL, [Axis2Right]              DECIMAL(8,2) NULL, [Axis2Gj]              DECIMAL(8,2) NULL, [Axis2LeftRightDiff]              DECIMAL(8,2) NULL, [Axis2Coefficient]              DECIMAL(8,2) NULL,
        [Axis3Left]              DECIMAL(8,2) NULL, [Axis3Right]              DECIMAL(8,2) NULL, [Axis3Gj]              DECIMAL(8,2) NULL, [Axis3LeftRightDiff]              DECIMAL(8,2) NULL, [Axis3Coefficient]              DECIMAL(8,2) NULL,
        [Axis4Left]              DECIMAL(8,2) NULL, [Axis4Right]              DECIMAL(8,2) NULL, [Axis4Gj]              DECIMAL(8,2) NULL, [Axis4LeftRightDiff]              DECIMAL(8,2) NULL, [Axis4Coefficient]              DECIMAL(8,2) NULL,
        [AxisParkingLeft]        DECIMAL(8,2) NULL, [AxisParkingRight]        DECIMAL(8,2) NULL, [AxisParkingGj]        DECIMAL(8,2) NULL, [AxisParkingLeftRightDiff]        DECIMAL(8,2) NULL, [AxisParkingCoefficient]        DECIMAL(8,2) NULL,

        [Weight]                          DECIMAL(10,2)        NULL,                                  -- typo fix (legacy "waight")

        -- Brake-effect overall
        [EffectOfWorkingBrakeEmpty]       DECIMAL(8,2)         NULL,                                  -- typo fix (Break→Brake)
        [EffectOfWorkingBrakeFull]        DECIMAL(8,2)         NULL,
        [EffectOfSecondaryBrake]          DECIMAL(8,2)         NULL,
        [EffectOfParkingBrake]            DECIMAL(8,2)         NULL,

        [SpeedOfTurns]                    DECIMAL(8,2)         NULL,

        -- Emissions (petrol)
        [CO]                              DECIMAL(8,3)         NULL,
        [NumEngineTurns]                  DECIMAL(8,2)         NULL,
        [COPlusTurns]                     DECIMAL(8,3)         NULL,
        [Lambda]                          DECIMAL(6,3)         NULL,

        -- Diesel
        [Pinpoints]                       DECIMAL(6,3)         NULL,

        -- Other
        [Noise]                           DECIMAL(6,2)         NULL,
        [TempOfEngineOil]                 DECIMAL(6,2)         NULL,
        [TechnicalChanges]                NVARCHAR(MAX)        NULL,
        [ExplanationNote]                 NVARCHAR(MAX)        NULL,
        [DriversWarning]                  NVARCHAR(MAX)        NULL,
        [Note]                            NVARCHAR(MAX)        NULL,

        [CreatedUtc]                      DATETIME2            NOT NULL CONSTRAINT [DF_TechnicalExamReports_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]                 DATETIME2            NOT NULL CONSTRAINT [DF_TechnicalExamReports_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]                      ROWVERSION           NOT NULL,

        CONSTRAINT [PK_TechnicalExamReports] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_TechnicalExamReports_Stations]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_TechnicalExamReports_CustomerVehicleRelations]
            FOREIGN KEY ([CustomerVehicleRelationId]) REFERENCES [dbo].[CustomerVehicleRelations]([Id]),
        CONSTRAINT [FK_TechnicalExamReports_AspNetUsers_FirstInspector]
            FOREIGN KEY ([FirstInspectorOperatorUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [FK_TechnicalExamReports_AspNetUsers_SecondInspector]
            FOREIGN KEY ([SecondInspectorOperatorUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [CK_TechnicalExamReports_DateOrdering] CHECK ([ValidTillDate] >= [MadeDate]),  -- BR-DOC-403
        CONSTRAINT [CK_TechnicalExamReports_DifferentInspectors]
            CHECK ([SecondInspectorOperatorUserId] IS NULL OR [SecondInspectorOperatorUserId] <> [FirstInspectorOperatorUserId])  -- BR-DOC-404
    );
    CREATE INDEX [IX_TechnicalExamReports_StationId_MadeDate]            ON [dbo].[TechnicalExamReports]([StationId], [MadeDate]);
    CREATE INDEX [IX_TechnicalExamReports_CustomerVehicleRelation]       ON [dbo].[TechnicalExamReports]([CustomerVehicleRelationId]);
    CREATE INDEX [IX_TechnicalExamReports_StationId_ValidTillDate]       ON [dbo].[TechnicalExamReports]([StationId], [ValidTillDate]);  -- expiry queries
    PRINT 'Table [dbo].[TechnicalExamReports] created.';
END
GO

-- 7. TechnicalExamReportDetails (per-part check results)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TechnicalExamReportDetails')
BEGIN
    CREATE TABLE [dbo].[TechnicalExamReportDetails] (
        [Id]                          BIGINT IDENTITY(1,1) NOT NULL,
        [TechnicalExamReportId]       BIGINT               NOT NULL,
        [TechnicalExamVehiclePartId]  INT                  NOT NULL,                                  -- BR-DOC-452 (FK → TehnicalExamVehicleParts REF, deferred)
        [StatusId]                    INT                  NOT NULL,                                  -- BR-DOC-453 (FK → TechnicalExamReportDetailStatuses REF, deferred)
        -- Position flags (any combination of Front/Back/Left/Right may apply)
        [Front]                       BIT                  NOT NULL CONSTRAINT [DF_TechnicalExamReportDetails_Front]   DEFAULT (0),
        [Back]                        BIT                  NOT NULL CONSTRAINT [DF_TechnicalExamReportDetails_Back]    DEFAULT (0),
        [OnLeft]                      BIT                  NOT NULL CONSTRAINT [DF_TechnicalExamReportDetails_OnLeft]  DEFAULT (0),
        [OnRight]                     BIT                  NOT NULL CONSTRAINT [DF_TechnicalExamReportDetails_OnRight] DEFAULT (0),
        [DateEnter]                   DATE                 NOT NULL,                                  -- BR-DOC-450
        [Note]                        NVARCHAR(150)        NULL,                                      -- BR-DOC-451
        [CreatedUtc]                  DATETIME2            NOT NULL CONSTRAINT [DF_TechnicalExamReportDetails_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]             DATETIME2            NOT NULL CONSTRAINT [DF_TechnicalExamReportDetails_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]                  ROWVERSION           NOT NULL,
        CONSTRAINT [PK_TechnicalExamReportDetails] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_TechnicalExamReportDetails_TechnicalExamReports]
            FOREIGN KEY ([TechnicalExamReportId]) REFERENCES [dbo].[TechnicalExamReports]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_TechnicalExamReportDetails_TechnicalExamReportId] ON [dbo].[TechnicalExamReportDetails]([TechnicalExamReportId]);
    PRINT 'Table [dbo].[TechnicalExamReportDetails] created.';
END
GO

-- 8. TechnicalExamReportVisualErrors (visual defects checklist)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'TechnicalExamReportVisualErrors')
BEGIN
    CREATE TABLE [dbo].[TechnicalExamReportVisualErrors] (
        [Id]                          BIGINT IDENTITY(1,1) NOT NULL,
        [TechnicalExamReportId]       BIGINT               NOT NULL,
        [Description]                 NVARCHAR(500)        NOT NULL,
        [Severity]                    NVARCHAR(50)         NULL,                                      -- e.g. minor / major / critical
        [CreatedUtc]                  DATETIME2            NOT NULL CONSTRAINT [DF_TechnicalExamReportVisualErrors_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]             DATETIME2            NOT NULL CONSTRAINT [DF_TechnicalExamReportVisualErrors_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]                  ROWVERSION           NOT NULL,
        CONSTRAINT [PK_TechnicalExamReportVisualErrors] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_TechnicalExamReportVisualErrors_TechnicalExamReports]
            FOREIGN KEY ([TechnicalExamReportId]) REFERENCES [dbo].[TechnicalExamReports]([Id]) ON DELETE CASCADE
    );
    PRINT 'Table [dbo].[TechnicalExamReportVisualErrors] created.';
END
GO

PRINT '';
PRINT '=== Documents module schema applied ===';
SELECT name AS [Tables in VTE2 (Documents module)]
FROM sys.tables
WHERE name IN (N'TrafficLicences', N'TrafficLicenceExtensions', N'Permissions',
               N'InternationalDrivingLicences', N'InternationalDrivingLicenceCategories',
               N'TechnicalExamReports', N'TechnicalExamReportDetails', N'TechnicalExamReportVisualErrors')
ORDER BY name;
GO


-- =============================================================================
-- SECTION 8 / 9 — Deferred FK constraints + Attachment tables
-- Source: docs\superpowers\work\bootstrap-fks-and-attachments.sql
-- =============================================================================

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


-- =============================================================================
-- SECTION 9 / 9 — Default seed data (countries, cities, REF defaults, RequestTypes)
-- Source: migrate\seed-vte2-defaults.sql
-- =============================================================================

-- VTE2 — default seed data
-- Purpose: make a fresh VTE2 database actually usable.
-- Idempotent (every INSERT is guarded with NOT EXISTS).
-- Run AFTER all bootstrap-*.sql scripts have populated the schema.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- =============================================================================
-- Countries (Macedonia + neighbours + common others)
-- =============================================================================
INSERT INTO [dbo].[Countries] ([Name], [Iso2], [Iso3])
SELECT v.Name, v.Iso2, v.Iso3 FROM (VALUES
    (N'Северна Македонија', 'MK', 'MKD'),
    (N'Албанија',           'AL', 'ALB'),
    (N'Косово',             'XK', 'XKK'),
    (N'Србија',             'RS', 'SRB'),
    (N'Бугарија',           'BG', 'BGR'),
    (N'Грција',             'GR', 'GRC'),
    (N'Турција',            'TR', 'TUR'),
    (N'Германија',          'DE', 'DEU'),
    (N'Италија',            'IT', 'ITA'),
    (N'Австрија',           'AT', 'AUT'),
    (N'Швајцарија',         'CH', 'CHE'),
    (N'Хрватска',           'HR', 'HRV'),
    (N'Словенија',          'SI', 'SVN'),
    (N'Босна и Херцеговина','BA', 'BIH'),
    (N'Црна Гора',          'ME', 'MNE'),
    (N'Унгарија',           'HU', 'HUN')
) v(Name, Iso2, Iso3)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Countries] WHERE Name = v.Name);
GO

-- =============================================================================
-- Vehicle categories (EU type-approval codes)
-- =============================================================================
INSERT INTO [dbo].[VehicleCategories] ([Name], [Code])
SELECT v.Name, v.Code FROM (VALUES
    (N'Мопед',              'L1'),
    (N'Мотоцикл лесен',     'L3'),
    (N'Мотоцикл',           'L4'),
    (N'Трицикл',            'L5'),
    (N'Лесно четиритркало', 'L6'),
    (N'Тешко четиритркало', 'L7'),
    (N'Патнички автомобил', 'M1'),
    (N'Автобус',            'M2'),
    (N'Тежок автобус',      'M3'),
    (N'Лесен товарен',      'N1'),
    (N'Среден товарен',     'N2'),
    (N'Тежок товарен',      'N3'),
    (N'Лесна приколка',     'O1'),
    (N'Лесна приколка 2',   'O2'),
    (N'Тешка приколка',     'O3'),
    (N'Тешка приколка 2',   'O4'),
    (N'Трактор',            'T'),
    (N'Земјоделски',        'C')
) v(Name, Code)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleCategories] WHERE Code = v.Code);
GO

-- =============================================================================
-- Vehicle body types (typical)
-- =============================================================================
INSERT INTO [dbo].[VehicleBodyTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Седан'), (N'Хечбек'), (N'Караван (стејшн)'), (N'Купе'), (N'Кабриолет'),
    (N'Внедорожник (SUV)'), (N'Минивен'), (N'Пикап'), (N'Бус'), (N'Тенда'),
    (N'Сандак'), (N'Цистерна'), (N'Самосвал'), (N'Камион'), (N'Полуприколка')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleBodyTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- Vehicle uses
-- =============================================================================
INSERT INTO [dbo].[VehicleUses] ([Name])
SELECT v.Name FROM (VALUES
    (N'Приватна употреба'),
    (N'Јавна употреба'),
    (N'Такси'),
    (N'Рент-а-кар'),
    (N'Авто-школа'),
    (N'Службена'),
    (N'Транспорт на патници'),
    (N'Транспорт на стока'),
    (N'Брза помош'),
    (N'Полиција'),
    (N'Противпожарна')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleUses] WHERE Name = v.Name);
GO

-- =============================================================================
-- Engine types / power source / eco programs
-- =============================================================================
INSERT INTO [dbo].[VehicleEngineTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Бензин'), (N'Дизел'), (N'Хибрид'), (N'Електричен'),
    (N'Бензин + ТНГ'), (N'Бензин + ЦНГ'), (N'Воден')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleEngineTypes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[VehicleEnginePowerSourceTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Бензин'), (N'Дизел'), (N'ТНГ (LPG)'), (N'ЦНГ (CNG)'),
    (N'Електрична енергија'), (N'Хибрид')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleEnginePowerSourceTypes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[VehicleEngineEcoPrograms] ([Name])
SELECT v.Name FROM (VALUES
    (N'Euro 1'), (N'Euro 2'), (N'Euro 3'), (N'Euro 4'),
    (N'Euro 5'), (N'Euro 6'), (N'EEV'), (N'Електричен (нула емисии)')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleEngineEcoPrograms] WHERE Name = v.Name);
GO

-- =============================================================================
-- Gearboxes / brakes / supportings
-- =============================================================================
INSERT INTO [dbo].[VehicleGearBoxes] ([Name])
SELECT v.Name FROM (VALUES (N'Рачен'), (N'Автоматски'), (N'Полуавтоматски'), (N'CVT'), (N'DCT'))
v(Name) WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleGearBoxes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[VehicleBrakes] ([Name])
SELECT v.Name FROM (VALUES (N'Дискови'), (N'Тапани'), (N'Дискови + тапани'), (N'ABS дискови'), (N'ABS + EBD'))
v(Name) WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleBrakes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[VehicleSupportings] ([Name])
SELECT v.Name FROM (VALUES (N'Макферсон'), (N'Двојни виши вилици'), (N'Многузглобни'), (N'Торзиона греда'), (N'Воздушни'))
v(Name) WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleSupportings] WHERE Name = v.Name);
GO

-- =============================================================================
-- Colors (basic palette)
-- =============================================================================
INSERT INTO [dbo].[Colors] ([Name], [HexCode])
SELECT v.Name, v.HexCode FROM (VALUES
    (N'Бела',    '#FFFFFF'),
    (N'Црна',    '#000000'),
    (N'Сива',    '#808080'),
    (N'Сребрена','#C0C0C0'),
    (N'Црвена',  '#C8102E'),
    (N'Сина',    '#1F4E8C'),
    (N'Зелена',  '#2E7D32'),
    (N'Жолта',   '#FBC02D'),
    (N'Кафена',  '#5D4037'),
    (N'Беж',     '#D7B98E'),
    (N'Портокалова', '#F57C00')
) v(Name, HexCode)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Colors] WHERE Name = v.Name);
GO

-- =============================================================================
-- Customer-Vehicle relation types
-- =============================================================================
INSERT INTO [dbo].[CustomerVehicleRelationTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Сопственик'), (N'Корисник'), (N'Закупец'), (N'Сосопственик'), (N'Овластен возач')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[CustomerVehicleRelationTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- Driving licence categories
-- =============================================================================
INSERT INTO [dbo].[DrivingLicenceCategories] ([Name], [Description])
SELECT v.Name, v.Description FROM (VALUES
    (N'AM',  N'Мопеди'),
    (N'A1',  N'Лесни мотоцикли до 125 cm³'),
    (N'A2',  N'Мотоцикли до 35 kW'),
    (N'A',   N'Мотоцикли без ограничување'),
    (N'B1',  N'Четиритркала'),
    (N'B',   N'Патнички автомобили'),
    (N'BE',  N'Патнички со приколка'),
    (N'C1',  N'Лесни товарни 3.5–7.5 t'),
    (N'C1E', N'Лесни товарни со приколка'),
    (N'C',   N'Тешки товарни преку 7.5 t'),
    (N'CE',  N'Тешки товарни со приколка'),
    (N'D1',  N'Минибуси до 16 патници'),
    (N'D1E', N'Минибуси со приколка'),
    (N'D',   N'Автобуси'),
    (N'DE',  N'Автобуси со приколка'),
    (N'F',   N'Трактори'),
    (N'G',   N'Земјоделски возила'),
    (N'H',   N'Работни машини'),
    (N'M',   N'Самовозечки трицикли')
) v(Name, Description)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[DrivingLicenceCategories] WHERE Name = v.Name);
GO

-- =============================================================================
-- Payment types (drives the legacy IsInvoice/IsCash/etc. flags from PaymentTypes)
-- =============================================================================
INSERT INTO [dbo].[PaymentTypes] ([Name], [IsCash], [IsInvoice], [IsFiscalCard], [IsAccount], [IsInstallments])
SELECT v.Name, v.IsCash, v.IsInvoice, v.IsFiscalCard, v.IsAccount, v.IsInstallments FROM (VALUES
    (N'Кеш (фискална)',         CAST(1 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Картичка (фискална)',    CAST(0 AS BIT), CAST(0 AS BIT), CAST(1 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Фактура',                CAST(0 AS BIT), CAST(1 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Уплата на сметка',       CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(1 AS BIT), CAST(0 AS BIT)),
    (N'Договор за рати',        CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(1 AS BIT))
) v(Name, IsCash, IsInvoice, IsFiscalCard, IsAccount, IsInstallments)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[PaymentTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- DDV (VAT) catalog — current Macedonian rates
-- Sources: https://www.ujp.gov.mk (Macedonian Public Revenue Office)
-- Rates current as of 2026: 18 % standard, 5 % preferential, 0 % zero-rated
-- =============================================================================
INSERT INTO [dbo].[DDVCatalog] ([Name], [Rate], [EffectiveFrom])
SELECT v.Name, v.Rate, v.EffectiveFrom FROM (VALUES
    (N'ДДВ 0%',  CAST(0.00  AS DECIMAL(5,2)), CAST('2000-01-01' AS DATE)),
    (N'ДДВ 5%',  CAST(5.00  AS DECIMAL(5,2)), CAST('2000-01-01' AS DATE)),
    (N'ДДВ 18%', CAST(18.00 AS DECIMAL(5,2)), CAST('2000-01-01' AS DATE))
) v(Name, Rate, EffectiveFrom)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[DDVCatalog] WHERE Name = v.Name);
GO

-- =============================================================================
-- Technical exam types
-- =============================================================================
INSERT INTO [dbo].[TechnicalExamTypes] ([Name], [ValidityMonths])
SELECT v.Name, v.ValidityMonths FROM (VALUES
    (N'Редовен годишен',       12),
    (N'Шестомесечен',           6),
    (N'Прв (за нови возила)',  24),
    (N'Вонреден',              NULL),
    (N'По сообраќајна несреќа',NULL),
    (N'По преправка',          NULL)
) v(Name, ValidityMonths)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[TechnicalExamTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- Technical exam vehicle-part categories + parts (the inspection checklist)
-- =============================================================================
DECLARE @cat TABLE (Name NVARCHAR(150), SortOrder INT);
INSERT INTO @cat VALUES
    (N'Кочни систем',                10),
    (N'Управувачки систем',          20),
    (N'Носачки систем',              30),
    (N'Светла и сигнализација',      40),
    (N'Гуми и тркала',               50),
    (N'Каросерија и шасија',         60),
    (N'Безбедносни уреди',           70),
    (N'Систем за издувни гасови',    80),
    (N'Електричен систем',           90),
    (N'Идентификација на возилото', 100);

INSERT INTO [dbo].[TechnicalExamVehiclePartCategories] ([Name], [SortOrder])
SELECT c.Name, c.SortOrder FROM @cat c
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = c.Name);
GO

-- Status set
INSERT INTO [dbo].[TechnicalExamReportDetailStatuses] ([Name], [IsPass])
SELECT v.Name, v.IsPass FROM (VALUES
    (N'Исправно',                CAST(1 AS BIT)),
    (N'Исправно со забелешка',   CAST(1 AS BIT)),
    (N'Условно исправно',        CAST(0 AS BIT)),
    (N'Неисправно',              CAST(0 AS BIT)),
    (N'Опасно неисправно',       CAST(0 AS BIT))
) v(Name, IsPass)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[TechnicalExamReportDetailStatuses] WHERE Name = v.Name);
GO

-- =============================================================================
-- Vehicle ownership proof types & payment proof types
-- =============================================================================
INSERT INTO [dbo].[VehicleOwnershipProofTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Сообраќајна дозвола'),
    (N'Договор за купопродажба'),
    (N'Договор за подарок'),
    (N'Решение за наследство'),
    (N'Решение за лизинг'),
    (N'Царинска декларација'),
    (N'Друго')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleOwnershipProofTypes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[PaymentProofTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Уплатница'),
    (N'Уплатна сметка'),
    (N'Бесплатно (ослободено)'),
    (N'Друго')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[PaymentProofTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- Request types (the workflow engine — drives Plav/Bel/Zelen behaviour)
-- =============================================================================
INSERT INTO [dbo].[RequestTypes] (
    [TypeName], [TypeDescription],
    [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration],
    [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer],
    [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired]
)
SELECT v.* FROM (VALUES
    -- (TypeName, TypeDescription, TER, PR, NReg, RelDel, VehDel, NewCust, VehCh, CustCh, Suf, PrevReg)
    (N'Прва регистрација',                  N'Регистрација на ново возило',                    1, 1, 1, 0, 0, 0, 0, 0, 0, 0),
    (N'Продолжување (редовно)',             N'Продолжување на регистрација по технички',       1, 1, 1, 0, 0, 0, 0, 0, 0, 1),
    (N'Промена на сопственик',              N'Префрлање на сопственост',                       1, 1, 1, 1, 0, 1, 0, 0, 0, 1),
    (N'Промена на возило',                  N'Само-промена на технички податоци',              1, 1, 0, 0, 0, 0, 1, 0, 0, 1),
    (N'Промена на сопственички податоци',   N'Адреса, име, презиме',                           0, 1, 0, 0, 0, 0, 0, 1, 0, 1),
    (N'Дерегистрација',                     N'Привремено отстранување од сообраќај',           0, 1, 0, 1, 0, 0, 0, 0, 0, 1),
    (N'Бришење на возило',                  N'Конечно бришење — расходувано',                  0, 1, 0, 1, 1, 0, 0, 0, 0, 1),
    (N'Само технички преглед',              N'Без регистрациски дејствија',                    1, 1, 0, 0, 0, 0, 0, 0, 1, 0),
    (N'Издавање меѓународна возачка',       N'Меѓународна возачка дозвола за возач',           0, 1, 0, 0, 0, 0, 0, 0, 1, 0),
    (N'Издавање дозвола за управување',     N'Специјална дозвола',                             0, 1, 0, 0, 0, 0, 0, 0, 1, 0)
) v(TypeName, TypeDescription, IsTechnicalExamRequired, IsPayRequired, IsNewRegistration,
    IsRelationDeleted, IsVehicleDeleted, IsNewCustomer, IsVehicleChanged, IsCustomerChanged,
    IsSufficient, IsPreviousRegistrationRequired)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[RequestTypes] WHERE TypeName = v.TypeName);
GO

-- =============================================================================
-- Cities (a starter set for North Macedonia)
-- =============================================================================
DECLARE @mkId INT = (SELECT TOP 1 Id FROM [dbo].[Countries] WHERE Iso2 = 'MK');

IF @mkId IS NOT NULL
BEGIN
    INSERT INTO [dbo].[Cities] ([Name], [PostalCode], [CountryId])
    SELECT v.Name, v.PostalCode, @mkId FROM (VALUES
        (N'Скопје',     '1000'),
        (N'Битола',     '7000'),
        (N'Куманово',   '1300'),
        (N'Прилеп',     '7500'),
        (N'Тетово',     '1200'),
        (N'Велес',      '1400'),
        (N'Штип',       '2000'),
        (N'Охрид',      '6000'),
        (N'Гостивар',   '1230'),
        (N'Струмица',   '2400'),
        (N'Кавадарци',  '1430'),
        (N'Кочани',     '2300'),
        (N'Кичево',     '6250'),
        (N'Струга',     '6330'),
        (N'Радовиш',    '2420'),
        (N'Гевгелија',  '1480'),
        (N'Дебар',      '1250'),
        (N'Свети Николе','2220'),
        (N'Неготино',   '1440'),
        (N'Делчево',    '2320'),
        (N'Кратово',    '1360'),
        (N'Берово',     '2330'),
        (N'Пехчево',    '2326'),
        (N'Виница',     '2310'),
        (N'Македонски Брод', '6510'),
        (N'Крушево',    '7550'),
        (N'Демир Хисар','7240'),
        (N'Ресен',      '7310'),
        (N'Валандово',  '1460'),
        (N'Дојран',     '1487'),
        (N'Пробиштип',  '2210')
    ) v(Name, PostalCode)
    WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Cities] WHERE Name = v.Name AND CountryId = @mkId);
END
GO

PRINT '';
PRINT '=== seed-vte2-defaults.sql complete ===';
SELECT
    (SELECT COUNT(*) FROM [dbo].[Countries])                          AS Countries,
    (SELECT COUNT(*) FROM [dbo].[Cities])                             AS Cities,
    (SELECT COUNT(*) FROM [dbo].[VehicleCategories])                  AS VehicleCategories,
    (SELECT COUNT(*) FROM [dbo].[VehicleBodyTypes])                   AS BodyTypes,
    (SELECT COUNT(*) FROM [dbo].[VehicleEngineTypes])                 AS EngineTypes,
    (SELECT COUNT(*) FROM [dbo].[Colors])                             AS Colors,
    (SELECT COUNT(*) FROM [dbo].[DrivingLicenceCategories])           AS DrivingLicenceCategories,
    (SELECT COUNT(*) FROM [dbo].[PaymentTypes])                       AS PaymentTypes,
    (SELECT COUNT(*) FROM [dbo].[DDVCatalog])                         AS DDV,
    (SELECT COUNT(*) FROM [dbo].[TechnicalExamTypes])                 AS TechExamTypes,
    (SELECT COUNT(*) FROM [dbo].[TechnicalExamReportDetailStatuses])  AS TechExamStatuses,
    (SELECT COUNT(*) FROM [dbo].[RequestTypes])                       AS RequestTypes;
GO


-- =============================================================================
-- SECTION 10 / 9 — Final verification
-- =============================================================================
PRINT '';
PRINT '=== VTE2 full bootstrap complete ===';
SELECT
    (SELECT COUNT(*) FROM sys.tables WHERE schema_id = SCHEMA_ID('dbo')) AS [Tables],
    (SELECT COUNT(*) FROM sys.foreign_keys)                              AS [FKs],
    (SELECT COUNT(*) FROM sys.check_constraints)                         AS [CHECKs],
    (SELECT COUNT(*) FROM sys.indexes WHERE is_primary_key = 0 AND index_id > 0) AS [Indexes];
GO

SELECT name AS [Tables in VTE2] FROM sys.tables ORDER BY name;
GO
