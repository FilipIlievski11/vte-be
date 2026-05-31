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
