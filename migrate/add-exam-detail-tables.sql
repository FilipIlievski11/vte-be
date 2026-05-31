-- Adds TechnicalExamReportDetails (parts checklist faults) and TechnicalExamReportVisualErrors
SET NOCOUNT ON;
USE VTE_Modern;
GO

IF OBJECT_ID('dbo.TechnicalExamReportDetails', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.TechnicalExamReportDetails(
    Id BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TechnicalExamReportDetails PRIMARY KEY,
    TechnicalExamReportId BIGINT NOT NULL,
    TechnicalExamVehiclePartId INT NOT NULL,
    StatusId INT NOT NULL,
    Front BIT NOT NULL CONSTRAINT DF_TERD_Front DEFAULT(0),
    Back BIT NOT NULL CONSTRAINT DF_TERD_Back DEFAULT(0),
    OnLeft BIT NOT NULL CONSTRAINT DF_TERD_OnLeft DEFAULT(0),
    OnRight BIT NOT NULL CONSTRAINT DF_TERD_OnRight DEFAULT(0),
    DateEnter DATE NOT NULL CONSTRAINT DF_TERD_DateEnter DEFAULT(CAST(SYSUTCDATETIME() AS DATE)),
    Note NVARCHAR(150) NULL,
    CreatedUtc DATETIME2 NOT NULL CONSTRAINT DF_TERD_CreatedUtc DEFAULT(SYSUTCDATETIME()),
    LastModifiedUtc DATETIME2 NOT NULL CONSTRAINT DF_TERD_LastModifiedUtc DEFAULT(SYSUTCDATETIME()),
    RowVersion ROWVERSION NOT NULL,
    CONSTRAINT FK_TERD_Report FOREIGN KEY(TechnicalExamReportId) REFERENCES dbo.TechnicalExamReports(Id),
    CONSTRAINT FK_TERD_Part FOREIGN KEY(TechnicalExamVehiclePartId) REFERENCES dbo.TechnicalExamVehicleParts(Id),
    CONSTRAINT FK_TERD_Status FOREIGN KEY(StatusId) REFERENCES dbo.TechnicalExamReportDetailStatuses(Id)
  );
  CREATE INDEX IX_TERD_Report ON dbo.TechnicalExamReportDetails(TechnicalExamReportId);
  PRINT 'Created TechnicalExamReportDetails';
END
ELSE PRINT 'TechnicalExamReportDetails already exists';
GO

IF OBJECT_ID('dbo.TechnicalExamReportVisualErrors', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.TechnicalExamReportVisualErrors(
    Id BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TechnicalExamReportVisualErrors PRIMARY KEY,
    TechnicalExamReportId BIGINT NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    Severity NVARCHAR(50) NULL,
    CreatedUtc DATETIME2 NOT NULL CONSTRAINT DF_TERVE_CreatedUtc DEFAULT(SYSUTCDATETIME()),
    LastModifiedUtc DATETIME2 NOT NULL CONSTRAINT DF_TERVE_LastModifiedUtc DEFAULT(SYSUTCDATETIME()),
    RowVersion ROWVERSION NOT NULL,
    CONSTRAINT FK_TERVE_Report FOREIGN KEY(TechnicalExamReportId) REFERENCES dbo.TechnicalExamReports(Id)
  );
  CREATE INDEX IX_TERVE_Report ON dbo.TechnicalExamReportVisualErrors(TechnicalExamReportId);
  PRINT 'Created TechnicalExamReportVisualErrors';
END
ELSE PRINT 'TechnicalExamReportVisualErrors already exists';
GO
