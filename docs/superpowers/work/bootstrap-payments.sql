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
