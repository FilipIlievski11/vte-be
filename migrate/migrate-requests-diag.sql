-- =============================================================================
-- Pre-flight diagnostic for the Requests migration. READ-ONLY.
--
-- Confirms:
--   * Snapshot exists + has rows
--   * Legacy RequestTypes Ids fit in tinyint (≤ 255)
--   * DocumentTypePrint / DocumentVehicleOwnershipProof / DocumentPaymentProof
--     / AttachmentTypes Id ranges fit in tinyint
--   * Admin user exists in target (so we can stamp historical operator audit)
--   * The migration-target relations exist (ClientVehicleRelation must be in)
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-requests-diag.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET NOCOUNT ON;

IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN
  PRINT '!!! Snapshot DB VTEZVV_Snapshot is missing. Run snapshot-legacy.ps1 first.';
  RETURN;
END;

PRINT '=== Snapshot row counts ===';
SELECT 'Requests'                       AS [Table], COUNT(*) AS [Rows] FROM VTEZVV_Snapshot.dbo.Requests
UNION ALL SELECT 'Requests (open)',      COUNT(*) FROM VTEZVV_Snapshot.dbo.Requests WHERE DateEnded IS NULL
UNION ALL SELECT 'Requests (closed)',    COUNT(*) FROM VTEZVV_Snapshot.dbo.Requests WHERE DateEnded IS NOT NULL
UNION ALL SELECT 'RequestTypes',         COUNT(*) FROM VTEZVV_Snapshot.dbo.RequestTypes
UNION ALL SELECT 'DocumentTypePrint',    COUNT(*) FROM VTEZVV_Snapshot.dbo.DocumentTypePrint
UNION ALL SELECT 'DocumentVehicleOwnershipProof', COUNT(*) FROM VTEZVV_Snapshot.dbo.DocumentVehicleOwnershipProof
UNION ALL SELECT 'DocumentPaymentProof', COUNT(*) FROM VTEZVV_Snapshot.dbo.DocumentPaymentProof
UNION ALL SELECT 'AttachmentTypes',      COUNT(*) FROM VTEZVV_Snapshot.dbo.AttachmentTypes
UNION ALL SELECT 'Request.VehicleOwnershipProofs (active)', COUNT(*) FROM VTEZVV_Snapshot.dbo.[Request.VehicleOwnershipProofs] WHERE Active = 1
UNION ALL SELECT 'Request.PaymentProof (active)',           COUNT(*) FROM VTEZVV_Snapshot.dbo.[Request.PaymentProof] WHERE Active = 1
ORDER BY [Table];

PRINT '';
PRINT '=== Id ranges (must all be <= 255 for tinyint catalogs) ===';
SELECT
  (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.RequestTypes)                  AS MaxRequestTypeId,
  (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.DocumentTypePrint)             AS MaxDocumentTypePrintId,
  (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.DocumentVehicleOwnershipProof) AS MaxOwnershipProofTypeId,
  (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.DocumentPaymentProof)          AS MaxPaymentProofTypeId,
  (SELECT MAX(Id) FROM VTEZVV_Snapshot.dbo.AttachmentTypes)               AS MaxAttachmentTypeId;

PRINT '';
PRINT '=== Orphan check — Requests pointing at a non-existent ClientVehicleRelation ===';
SELECT COUNT(*) AS Orphans
FROM VTEZVV_Snapshot.dbo.Requests r
LEFT JOIN dbo.ClientVehicleRelation cvr ON cvr.Id = r.IdCustomerVehicleRelation
WHERE cvr.Id IS NULL;

PRINT '';
PRINT '=== Orphan check — child proof rows pointing at a non-existent Request ===';
SELECT 'OwnershipProofs orphans' AS [Group], COUNT(*) AS [Count]
FROM VTEZVV_Snapshot.dbo.[Request.VehicleOwnershipProofs] p
LEFT JOIN VTEZVV_Snapshot.dbo.Requests r ON r.Id = p.IdRequest
WHERE r.Id IS NULL
UNION ALL
SELECT 'PaymentProofs orphans', COUNT(*)
FROM VTEZVV_Snapshot.dbo.[Request.PaymentProof] p
LEFT JOIN VTEZVV_Snapshot.dbo.Requests r ON r.Id = p.IdRequest
WHERE r.Id IS NULL;

PRINT '';
PRINT '=== Target preconditions ===';
SELECT
  (SELECT COUNT(*) FROM dbo.Company WHERE Id = 4)              AS Company4_exists,
  (SELECT COUNT(*) FROM dbo.ClientVehicleRelation)             AS Target_ClientVehicleRelations,
  (SELECT TOP 1 Id FROM dbo.AspNetUsers WHERE UserName='admin') AS Admin_UserId,
  (SELECT COUNT(*) FROM dbo.Request)                           AS Target_Requests_NOW;

PRINT '';
PRINT '=== Done. Review the numbers, then run migrate-request-catalogs.sql + migrate-requests.sql ===';
