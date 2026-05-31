-- =============================================================================
-- Legacy Requests → VTE Request (+ ownership proofs + payment proofs).
--   Source: (localdb)\MSSQLLocalDB\VTEZVV_Snapshot
--   Target: (localdb)\MSSQLLocalDB\VTE
--
-- Plan:
--   1. Requests       (preserve Ids, CompanyId=4, operators → admin user)
--   2. OwnershipProofs (preserve Ids; free-text legacy label → Detail)
--   3. PaymentProofs   (preserve Ids; free-text legacy label → Detail)
--
-- Notes:
--   - Operator id mapping: legacy IdOperatorCreated/Modified/Ended (int) →
--     ASP.NET admin user's string Id. The original numeric operator id is
--     appended to Note (in []) so traceability isn't lost.
--   - IdTechnicalExamReport → NULL (module not migrated yet).
--   - IdPreviousRegistration: kept as-is when > 0, NULL otherwise.
--   - Requests whose IdCustomerVehicleRelation is NULL or doesn't exist in
--     the target ClientVehicleRelation table are SKIPPED (logged as a count).
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-requests.sql -b -X -I -f 65001
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

IF NOT EXISTS (SELECT 1 FROM dbo.Company WHERE Id = 4)
BEGIN
  RAISERROR('Target Company Id=4 not found.', 16, 1);
  RETURN;
END;
IF (SELECT COUNT(*) FROM dbo.RequestType) = 0
BEGIN
  RAISERROR('No RequestType rows. Run migrate-request-catalogs.sql first.', 16, 1);
  RETURN;
END;
IF (SELECT COUNT(*) FROM dbo.ClientVehicleRelation) = 0
BEGIN
  RAISERROR('No ClientVehicleRelation rows. Run migrate-vehicles.sql first.', 16, 1);
  RETURN;
END;

DECLARE @adminId nvarchar(450) =
  (SELECT TOP 1 Id FROM dbo.AspNetUsers WHERE UserName = 'admin');
IF @adminId IS NULL
BEGIN
  RAISERROR('Admin user not found. Boot the API once so DataSeeder creates the admin.', 16, 1);
  RETURN;
END;
PRINT CONCAT('Admin user id resolved: ', @adminId);

PRINT '=== Wiping target Request + children (in FK order) ===';
DELETE FROM dbo.RequestAttachment;
DELETE FROM dbo.RequestOwnershipProof;
DELETE FROM dbo.RequestPaymentProof;
DELETE FROM dbo.Request;
DBCC CHECKIDENT('dbo.RequestAttachment',     RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.RequestOwnershipProof', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.RequestPaymentProof',   RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.Request',               RESEED, 0) WITH NO_INFOMSGS;

DECLARE @rows int;

-- ============================================================================
-- 1. Request (the main table)
-- ============================================================================
PRINT '=== Request (CompanyId=4, preserve Ids, operators → admin user) ===';
SET IDENTITY_INSERT dbo.Request ON;
INSERT INTO dbo.Request (
  Id, CompanyId, RequestTypeId,
  ClientVehicleRelationId, NewClientVehicleRelationId,
  TechnicalExamReportId, PreviousRegistrationId,
  CreatedAt, ModifiedAt, EndedAt,
  CreatedByUserId, ModifiedByUserId, EndedByUserId,
  VehicleDataChanged, ClientDataChanged,
  Note, Active)
SELECT
  r.Id                                                                     AS Id,
  CAST(4 AS tinyint)                                                       AS CompanyId,
  CAST(r.IdRequestType AS tinyint)                                         AS RequestTypeId,
  r.IdCustomerVehicleRelation                                              AS ClientVehicleRelationId,
  -- NewClientVehicleRelationId: NULL when 0 or absent in target.
  CASE WHEN r.IdCustomerVehicleRelationNew IS NULL
         OR r.IdCustomerVehicleRelationNew = 0 THEN NULL
       WHEN EXISTS (SELECT 1 FROM dbo.ClientVehicleRelation x WHERE x.Id = r.IdCustomerVehicleRelationNew)
         THEN r.IdCustomerVehicleRelationNew
       ELSE NULL END                                                        AS NewClientVehicleRelationId,
  NULL                                                                     AS TechnicalExamReportId,   -- module not migrated
  -- Map to VehicleRegistration.Id when > 0 and exists in target.
  CASE WHEN r.IdPreviousRegistration > 0
         AND EXISTS (SELECT 1 FROM dbo.VehicleRegistration vr WHERE vr.Id = r.IdPreviousRegistration)
       THEN CAST(r.IdPreviousRegistration AS bigint) ELSE NULL END           AS PreviousRegistrationId,
  r.DateCreated                                                            AS CreatedAt,
  r.DateModified                                                           AS ModifiedAt,
  r.DateEnded                                                              AS EndedAt,
  @adminId                                                                 AS CreatedByUserId,
  CASE WHEN r.DateModified IS NOT NULL THEN @adminId ELSE NULL END         AS ModifiedByUserId,
  CASE WHEN r.DateEnded    IS NOT NULL THEN @adminId ELSE NULL END         AS EndedByUserId,
  r.IsVehicleChanged                                                       AS VehicleDataChanged,
  r.IsCustomerChanged                                                      AS ClientDataChanged,
  -- Preserve original operator ids inside the Note for traceability.
  LEFT(
    CONCAT(
      NULLIF(LTRIM(RTRIM(r.Note)), N''),
      N' [legacy operator ids C=', r.IdOperatorCreated,
      N' M=', ISNULL(CAST(r.IdOperatorModified AS nvarchar(10)), N'-'),
      N' E=', r.IdOperatorEnded, N']'
    ), 500
  )                                                                        AS Note,
  r.Active                                                                 AS Active
FROM VTEZVV_Snapshot.dbo.Requests r
-- Inner join to ClientVehicleRelation skips orphan rows.
INNER JOIN dbo.ClientVehicleRelation cvr
  ON cvr.Id = r.IdCustomerVehicleRelation
-- Inner join to RequestType skips rows pointing at non-existent / out-of-range types.
INNER JOIN dbo.RequestType rt
  ON rt.Id = CASE WHEN r.IdRequestType BETWEEN 1 AND 255
                  THEN CAST(r.IdRequestType AS tinyint) END;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.Request OFF;
DECLARE @maxReq bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.Request);
DBCC CHECKIDENT('dbo.Request', RESEED, @maxReq) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' Requests. Identity reseeded to ', @maxReq, '.');

DECLARE @skippedReq int =
  (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.Requests) - @rows;
IF @skippedReq > 0
  PRINT CONCAT('  !! Skipped ', @skippedReq, ' Requests with orphaned relation or invalid RequestType.');

-- ============================================================================
-- 2. RequestOwnershipProof  (legacy Request.VehicleOwnershipProofs)
-- ============================================================================
PRINT '=== RequestOwnershipProof (preserve Ids; legacy label → Detail) ===';
SET IDENTITY_INSERT dbo.RequestOwnershipProof ON;
INSERT INTO dbo.RequestOwnershipProof (Id, RequestId, OwnershipProofTypeId, Detail, Active)
SELECT
  p.Id,
  p.IdRequest,
  CAST(p.IdVehicleOwnershipProof AS tinyint) AS OwnershipProofTypeId,
  -- Use the legacy free-text label as the detail. If matches the catalog name,
  -- it's redundant but harmless. Otherwise it's useful operator notes.
  NULLIF(LTRIM(RTRIM(p.VehicleOwnershipProof)), N''),
  p.Active
FROM VTEZVV_Snapshot.dbo.[Request.VehicleOwnershipProofs] p
INNER JOIN dbo.Request r ON r.Id = p.IdRequest
INNER JOIN dbo.RequestOwnershipProofType ot
  ON ot.Id = CASE WHEN p.IdVehicleOwnershipProof BETWEEN 1 AND 255
                  THEN CAST(p.IdVehicleOwnershipProof AS tinyint) END;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.RequestOwnershipProof OFF;
DECLARE @maxOwn bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestOwnershipProof);
DBCC CHECKIDENT('dbo.RequestOwnershipProof', RESEED, @maxOwn) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' OwnershipProofs. Identity reseeded to ', @maxOwn, '.');

-- ============================================================================
-- 3. RequestPaymentProof   (legacy Request.PaymentProof)
-- ============================================================================
PRINT '=== RequestPaymentProof (preserve Ids; legacy label → Detail) ===';
SET IDENTITY_INSERT dbo.RequestPaymentProof ON;
INSERT INTO dbo.RequestPaymentProof (Id, RequestId, PaymentProofTypeId, Detail, Active)
SELECT
  p.Id,
  p.IdRequest,
  CAST(p.IdPaymentProof AS tinyint) AS PaymentProofTypeId,
  NULLIF(LTRIM(RTRIM(p.PaymentProof)), N''),
  p.Active
FROM VTEZVV_Snapshot.dbo.[Request.PaymentProof] p
INNER JOIN dbo.Request r ON r.Id = p.IdRequest
INNER JOIN dbo.RequestPaymentProofType pt
  ON pt.Id = CASE WHEN p.IdPaymentProof BETWEEN 1 AND 255
                  THEN CAST(p.IdPaymentProof AS tinyint) END;
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.RequestPaymentProof OFF;
DECLARE @maxPay bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestPaymentProof);
DBCC CHECKIDENT('dbo.RequestPaymentProof', RESEED, @maxPay) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' PaymentProofs. Identity reseeded to ', @maxPay, '.');

-- ============================================================================
-- Row-count verification
-- ============================================================================
PRINT '';
PRINT '=== Row-count verification ===';
SELECT 'Source Requests'        AS [Group], COUNT(*) AS [Count] FROM VTEZVV_Snapshot.dbo.Requests
UNION ALL SELECT 'Target Request',           COUNT(*) FROM dbo.Request
UNION ALL SELECT 'Source OwnershipProofs',   COUNT(*) FROM VTEZVV_Snapshot.dbo.[Request.VehicleOwnershipProofs]
UNION ALL SELECT 'Target OwnershipProof',    COUNT(*) FROM dbo.RequestOwnershipProof
UNION ALL SELECT 'Source PaymentProofs',     COUNT(*) FROM VTEZVV_Snapshot.dbo.[Request.PaymentProof]
UNION ALL SELECT 'Target PaymentProof',      COUNT(*) FROM dbo.RequestPaymentProof
ORDER BY [Group];

PRINT '';
PRINT '=== Status breakdown ===';
SELECT
  SUM(CASE WHEN EndedAt IS NULL    THEN 1 ELSE 0 END) AS OpenRequests,
  SUM(CASE WHEN EndedAt IS NOT NULL THEN 1 ELSE 0 END) AS ClosedRequests,
  SUM(CASE WHEN Active = 0          THEN 1 ELSE 0 END) AS InactiveRequests,
  COUNT(*)                                             AS Total
FROM dbo.Request;

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Requests migrated successfully ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
