-- =============================================================================
-- Legacy Request catalogs → VTE Request catalogs.
--   Source: (localdb)\MSSQLLocalDB\VTEZVV_Snapshot
--   Target: (localdb)\MSSQLLocalDB\VTE
--
-- Five tables, preserve original Ids (tinyint range).
--
-- Wipes target catalog rows first — this clobbers the 5 starter RequestType
-- rows + 3 print forms + 6/6/6 proof/attachment types seeded by the API on
-- first boot. The DataSeeder will skip re-seeding because it now checks for
-- ANY RequestType row before running (see DataSeeder.cs).
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-request-catalogs.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN
  RAISERROR('Source DB VTEZVV_Snapshot is missing.', 16, 1);
  RETURN;
END;

-- Safety: refuse to wipe catalogs if any historical Request rows are present
-- (would orphan FKs into RequestType). Run migrate-requests.sql AFTER this.
IF (SELECT COUNT(*) FROM dbo.Request) > 0
BEGIN
  RAISERROR('Target dbo.Request already has rows — wipe Requests + children before re-running catalogs.', 16, 1);
  RETURN;
END;

-- Confirm legacy Ids fit
IF (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.RequestTypes) > 255
BEGIN
  RAISERROR('Legacy RequestTypes has Id > 255 — would overflow tinyint. Aborting.', 16, 1);
  RETURN;
END;
IF (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.DocumentTypePrint) > 255
BEGIN
  RAISERROR('Legacy DocumentTypePrint has Id > 255.', 16, 1);
  RETURN;
END;
IF (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.DocumentVehicleOwnershipProof) > 255
BEGIN
  RAISERROR('Legacy DocumentVehicleOwnershipProof has Id > 255.', 16, 1);
  RETURN;
END;
IF (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.DocumentPaymentProof) > 255
BEGIN
  RAISERROR('Legacy DocumentPaymentProof has Id > 255.', 16, 1);
  RETURN;
END;
IF (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.AttachmentTypes) > 255
BEGIN
  RAISERROR('Legacy AttachmentTypes has Id > 255.', 16, 1);
  RETURN;
END;

PRINT '=== Wiping target catalogs (order matters: children of RequestType last) ===';
DELETE FROM dbo.RequestType;            -- has self-FK; wipe before children below would orphan, but we have no children yet
DELETE FROM dbo.RequestDocumentPrint;
DELETE FROM dbo.RequestOwnershipProofType;
DELETE FROM dbo.RequestPaymentProofType;
DELETE FROM dbo.RequestAttachmentType;
DBCC CHECKIDENT('dbo.RequestType',                RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.RequestDocumentPrint',       RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.RequestOwnershipProofType',  RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.RequestPaymentProofType',    RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.RequestAttachmentType',      RESEED, 0) WITH NO_INFOMSGS;

DECLARE @rows int;

-- ============================================================================
-- 1. RequestDocumentPrint  (legacy DocumentTypePrint)
--    Columns: Id, Opis (description / display label), Active
--    We need: Id, Code, Name, TemplatePath, Active.
--    Strategy: use Opis as both Code (sluggified upper-cased) and Name.
-- ============================================================================
PRINT '=== RequestDocumentPrint ===';
SET IDENTITY_INSERT dbo.RequestDocumentPrint ON;
INSERT INTO dbo.RequestDocumentPrint (Id, Code, Name, TemplatePath, Active)
SELECT
  CAST(p.Id AS tinyint),
  -- Code: take the first 20 chars of Opis, upper-cased, spaces → underscores.
  LEFT(UPPER(REPLACE(LTRIM(RTRIM(p.Opis)), N' ', N'_')), 20)  AS Code,
  LTRIM(RTRIM(p.Opis))                                         AS Name,
  NULL                                                          AS TemplatePath,
  p.Active                                                      AS Active
FROM VTEZVV_Snapshot.dbo.DocumentTypePrint p;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.RequestDocumentPrint OFF;
DECLARE @maxPrint int = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestDocumentPrint);
DBCC CHECKIDENT('dbo.RequestDocumentPrint', RESEED, @maxPrint) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' rows. Identity reseeded to ', @maxPrint, '.');

-- ============================================================================
-- 2. RequestType
--    Map legacy `IsTehnicalExamRequired` (bit) → new TechnicalExamRequirement enum
--    (0=NotRequired, 1=Required, 2=Optional). Legacy is bit-only so 0/1.
--    Map legacy `IdRequestType` (parent) → ParentRequestTypeId (NULL when 0).
-- ============================================================================
PRINT '=== RequestType ===';
SET IDENTITY_INSERT dbo.RequestType ON;
INSERT INTO dbo.RequestType (
  Id, ParentRequestTypeId, DocumentPrintId, Name, Description,
  TechnicalExamRequirement, PaymentRequired, IssuesNewRegistration,
  DeactivatesRelation, DeactivatesVehicle, TransfersOwnership,
  MutatesVehicleData, MutatesClientData, IsSufficient,
  PreviousRegistrationRequired, Active)
SELECT
  CAST(t.Id AS tinyint),
  CASE WHEN t.IdRequestType > 0 AND t.IdRequestType <= 255
       THEN CAST(t.IdRequestType AS tinyint) ELSE NULL END,
  CAST(t.IdDocumentPrint AS tinyint),
  LTRIM(RTRIM(t.TypeName)),
  NULLIF(LTRIM(RTRIM(t.TypeDescription)), N''),
  CAST(t.IsTehnicalExamRequired AS tinyint),
  t.IsPayRequired,
  t.IsNewRegistration,
  t.IsRelationDeleted,
  t.IsVehicleDeleted,
  t.IsNewCustomer,
  t.IsVehicleChanged,
  t.IsCustomerChanged,
  t.IsSufficient,
  t.IsPreviosRegistrationReqired,
  t.Active
FROM VTEZVV_Snapshot.dbo.RequestTypes t
-- Only insert legacy types whose parent (if any) is also present or zero.
WHERE t.IdRequestType = 0
   OR t.IdRequestType IS NULL
   OR EXISTS (SELECT 1 FROM VTEZVV_Snapshot.dbo.RequestTypes p WHERE p.Id = t.IdRequestType);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.RequestType OFF;
DECLARE @maxType int = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestType);
DBCC CHECKIDENT('dbo.RequestType', RESEED, @maxType) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' rows. Identity reseeded to ', @maxType, '.');

-- ============================================================================
-- 3. RequestOwnershipProofType  (legacy DocumentVehicleOwnershipProof)
-- ============================================================================
PRINT '=== RequestOwnershipProofType ===';
SET IDENTITY_INSERT dbo.RequestOwnershipProofType ON;
INSERT INTO dbo.RequestOwnershipProofType (Id, Name, Active)
SELECT
  CAST(o.Id AS tinyint),
  LTRIM(RTRIM(o.VehicleOwnershipProofName)),
  o.Active
FROM VTEZVV_Snapshot.dbo.DocumentVehicleOwnershipProof o;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.RequestOwnershipProofType OFF;
DECLARE @maxOwn int = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestOwnershipProofType);
DBCC CHECKIDENT('dbo.RequestOwnershipProofType', RESEED, @maxOwn) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' rows. Identity reseeded to ', @maxOwn, '.');

-- ============================================================================
-- 4. RequestPaymentProofType  (legacy DocumentPaymentProof)
-- ============================================================================
PRINT '=== RequestPaymentProofType ===';
SET IDENTITY_INSERT dbo.RequestPaymentProofType ON;
INSERT INTO dbo.RequestPaymentProofType (Id, Name, Active)
SELECT
  CAST(p.Id AS tinyint),
  LTRIM(RTRIM(p.PaymentProofName)),
  p.Active
FROM VTEZVV_Snapshot.dbo.DocumentPaymentProof p;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.RequestPaymentProofType OFF;
DECLARE @maxPay int = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestPaymentProofType);
DBCC CHECKIDENT('dbo.RequestPaymentProofType', RESEED, @maxPay) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' rows. Identity reseeded to ', @maxPay, '.');

-- ============================================================================
-- 5. RequestAttachmentType  (legacy AttachmentTypes)
-- ============================================================================
PRINT '=== RequestAttachmentType ===';
SET IDENTITY_INSERT dbo.RequestAttachmentType ON;
INSERT INTO dbo.RequestAttachmentType (Id, Name, Active)
SELECT
  CAST(a.Id AS tinyint),
  LTRIM(RTRIM(a.AttachmentType)),
  a.Active
FROM VTEZVV_Snapshot.dbo.AttachmentTypes a;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.RequestAttachmentType OFF;
DECLARE @maxAtt int = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestAttachmentType);
DBCC CHECKIDENT('dbo.RequestAttachmentType', RESEED, @maxAtt) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' rows. Identity reseeded to ', @maxAtt, '.');

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Catalogs migrated. Now run migrate-requests.sql to migrate the Requests + children. ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
