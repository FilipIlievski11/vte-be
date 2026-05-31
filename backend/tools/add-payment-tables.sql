-- Adds the 4 missing payment-catalog tables and back-fills new columns on
-- existing PaymentTypes / PaymentDocumentDetails. Idempotent.
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -d VTE2 -E -C -i add-payment-tables.sql

SET NOCOUNT ON;

-- ---------------------------------------------------------------------------
-- 1. PaymentCategories (driver for auto-calc; 317 rows in legacy)
-- ---------------------------------------------------------------------------
IF OBJECT_ID('dbo.PaymentCategories','U') IS NULL
BEGIN
    CREATE TABLE dbo.PaymentCategories (
        Id                                   int            NOT NULL PRIMARY KEY,
        DDVId                                int            NULL,
        CalculationItemId                    int            NULL,
        CategoryName                         nvarchar(250)  NOT NULL,
        AllowDiscount                        bit            NOT NULL DEFAULT 0,
        TriggerdByRequest                    bit            NOT NULL DEFAULT 0,
        TriggerdByTechnicalExam              bit            NOT NULL DEFAULT 0,
        TriggerdByTrafficLicence             bit            NOT NULL DEFAULT 0,
        TriggerdByPermissionForVehicle       bit            NOT NULL DEFAULT 0,
        TriggerdByInternationalDriverLicence bit            NOT NULL DEFAULT 0,
        TriggerdByIrregularTechnicalExam     bit            NOT NULL DEFAULT 0,
        VisibleOrder                         int            NULL,
        CommunityId                          int            NULL,
        IsActive                             bit            NOT NULL DEFAULT 1,
        CreatedUtc                           datetime2      NOT NULL DEFAULT SYSUTCDATETIME(),
        LastModifiedUtc                      datetime2      NOT NULL DEFAULT SYSUTCDATETIME(),
        RowVersion                           rowversion     NOT NULL
    );
    CREATE INDEX IX_PaymentCategories_Trigger_Request ON dbo.PaymentCategories (TriggerdByRequest, IsActive);
    PRINT 'Created PaymentCategories.';
END

-- ---------------------------------------------------------------------------
-- 2. PaymentItems (price book; 6 020 rows in legacy)
-- ---------------------------------------------------------------------------
IF OBJECT_ID('dbo.PaymentItems','U') IS NULL
BEGIN
    CREATE TABLE dbo.PaymentItems (
        Id                            int            NOT NULL PRIMARY KEY,
        PaymentCategoryId             int            NOT NULL,
        VehicleCategoryForPaymentsId  int            NULL,
        ItemName                      nvarchar(250)  NOT NULL,
        IsActive                      bit            NOT NULL DEFAULT 1,
        CreatedUtc                    datetime2      NOT NULL DEFAULT SYSUTCDATETIME(),
        LastModifiedUtc               datetime2      NOT NULL DEFAULT SYSUTCDATETIME(),
        RowVersion                    rowversion     NOT NULL
    );
    CREATE INDEX IX_PaymentItems_Category_VehCat ON dbo.PaymentItems (PaymentCategoryId, VehicleCategoryForPaymentsId);
    PRINT 'Created PaymentItems.';
END

-- ---------------------------------------------------------------------------
-- 3. PaymentItemParametars (range rules; 2 521 rows in legacy)
-- ---------------------------------------------------------------------------
IF OBJECT_ID('dbo.PaymentItemParametars','U') IS NULL
BEGIN
    CREATE TABLE dbo.PaymentItemParametars (
        Id              int            NOT NULL PRIMARY KEY,
        PaymentItemId   int            NOT NULL,
        ParametarName   nvarchar(250)  NOT NULL,
        VehicleField    nvarchar(150)  NULL,
        ParametarFrom   decimal(18,4)  NULL,
        ParametarTo     decimal(18,4)  NULL,
        Price           decimal(18,2)  NOT NULL DEFAULT 0,
        IsOptional      bit            NOT NULL DEFAULT 0,
        IsActive        bit            NOT NULL DEFAULT 1,
        CreatedUtc      datetime2      NOT NULL DEFAULT SYSUTCDATETIME(),
        LastModifiedUtc datetime2      NOT NULL DEFAULT SYSUTCDATETIME(),
        RowVersion      rowversion     NOT NULL
    );
    CREATE INDEX IX_PaymentItemParametars_Item ON dbo.PaymentItemParametars (PaymentItemId);
    PRINT 'Created PaymentItemParametars.';
END

-- ---------------------------------------------------------------------------
-- 4. PaymentDocumentNumbers (per-station, per-type sequence; 47 rows in legacy)
-- ---------------------------------------------------------------------------
IF OBJECT_ID('dbo.PaymentDocumentNumbers','U') IS NULL
BEGIN
    CREATE TABLE dbo.PaymentDocumentNumbers (
        Id                  int        IDENTITY(1,1) NOT NULL PRIMARY KEY,
        StationId           int        NOT NULL,
        PaymentTypeId       int        NOT NULL,
        Number              int        NOT NULL DEFAULT 0,
        IsTechExamReport    bit        NOT NULL DEFAULT 0,
        LastModifiedUtc     datetime2  NOT NULL DEFAULT SYSUTCDATETIME(),
        RowVersion          rowversion NOT NULL
    );
    CREATE UNIQUE INDEX UX_PaymentDocumentNumbers_Station_Type_Tech
        ON dbo.PaymentDocumentNumbers (StationId, PaymentTypeId, IsTechExamReport);
    PRINT 'Created PaymentDocumentNumbers.';
END

-- ---------------------------------------------------------------------------
-- 5. Extend PaymentTypes with legacy flag columns
-- ---------------------------------------------------------------------------
IF COL_LENGTH('dbo.PaymentTypes', 'Prefix') IS NULL
    ALTER TABLE dbo.PaymentTypes ADD Prefix nvarchar(15) NULL;
IF COL_LENGTH('dbo.PaymentTypes', 'PrintText') IS NULL
    ALTER TABLE dbo.PaymentTypes ADD PrintText nvarchar(50) NULL;
IF COL_LENGTH('dbo.PaymentTypes', 'PayedAmount') IS NULL
    ALTER TABLE dbo.PaymentTypes ADD PayedAmount bit NOT NULL DEFAULT 0;

-- ---------------------------------------------------------------------------
-- 6. Extend PaymentDocumentDetails with PaymentItemId (legacy IdPriceCatalog
--    misleadingly named — actually references PaymentItems)
-- ---------------------------------------------------------------------------
IF COL_LENGTH('dbo.PaymentDocumentDetails', 'PaymentItemId') IS NULL
    ALTER TABLE dbo.PaymentDocumentDetails ADD PaymentItemId int NULL;

PRINT 'Done.';
