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
