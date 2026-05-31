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
