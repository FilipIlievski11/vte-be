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
