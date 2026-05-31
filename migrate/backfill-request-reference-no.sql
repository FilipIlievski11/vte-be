-- =============================================================================
-- Backfill Request.TechnicalExamReportId + Request.LegacyReferenceNumber from
-- the legacy snapshot.
--
-- The reference number is what the legacy app prints at the bottom of each
-- request form (e.g. "167403128/2026"). Format derived from VTE.Library /
-- PrintZelenInfo.vb (lines 180-197):
--
--   {StationCode}{IdTechnicalExamReport}{OperatorId}/{Year(DateCreated)}
--
-- Components (decoded for sample request 210483 → 167403128/2026):
--   StationCode             = TehnicalExamOrganizations.Code            = "1"     (VELES)
--   IdTechnicalExamReport   = Requests.IdTechnicalExamReport             = 67403
--   OperatorId              = Requests.IdOperatorCreated                = 28
--   Year                    = YEAR(Requests.DateCreated)                = 2026
--   ⇒ "1" || "67403" || "28" || "/" || "2026" = "167403128/2026"
--
-- The station code is hard-coded "1" for VELES (CompanyId=4) since our
-- migration imported only that station. When we onboard more stations,
-- look up TehnicalExamOrganizations.Code per legacy DB.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\backfill-request-reference-no.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN
  RAISERROR('Source DB VTEZVV_Snapshot is missing. Reattach it first.', 16, 1);
  RETURN;
END;
IF COL_LENGTH('dbo.Request', 'LegacyReferenceNumber') IS NULL
BEGIN
  RAISERROR('Request.LegacyReferenceNumber column not found. Apply EF migration AddRequestLegacyReferenceNumber first.', 16, 1);
  RETURN;
END;

DECLARE @VELES_STATION_CODE NVARCHAR(10) = N'1';     -- TehnicalExamOrganizations.Code for VELES

-- ============================================================================
-- 1. Restore TechnicalExamReportId from legacy IdTechnicalExamReport
--    (migrate-requests.sql set it to NULL because we didn't have the exam
--    module yet — but we need the raw value to print the reference number)
-- ============================================================================
PRINT '=== Restoring Request.TechnicalExamReportId from legacy ===';
UPDATE r
SET r.TechnicalExamReportId = src.IdTechnicalExamReport
FROM dbo.Request r
INNER JOIN VTEZVV_Snapshot.dbo.Requests src ON src.Id = r.Id
WHERE r.TechnicalExamReportId IS NULL
  AND src.IdTechnicalExamReport > 0;
DECLARE @tex int = @@ROWCOUNT;
PRINT CONCAT('  -> ', @tex, ' rows updated (TechnicalExamReportId restored).');

-- ============================================================================
-- 2. Compute and store the reference number string for each migrated Request
-- ============================================================================
PRINT '=== Computing LegacyReferenceNumber per request ===';
UPDATE r
SET r.LegacyReferenceNumber = CONCAT(
      @VELES_STATION_CODE,
      CAST(src.IdTechnicalExamReport AS NVARCHAR(20)),
      CAST(src.IdOperatorCreated AS NVARCHAR(10)),
      N'/',
      CAST(YEAR(src.DateCreated) AS NVARCHAR(4))
    )
FROM dbo.Request r
INNER JOIN VTEZVV_Snapshot.dbo.Requests src ON src.Id = r.Id
WHERE r.LegacyReferenceNumber IS NULL
  AND src.IdTechnicalExamReport > 0;
DECLARE @refs int = @@ROWCOUNT;
PRINT CONCAT('  -> ', @refs, ' rows updated (LegacyReferenceNumber computed).');

-- ============================================================================
-- Spot-check sample rows
-- ============================================================================
PRINT '';
PRINT '=== Sample after backfill (look for "167403128/2026" on Id=210483) ===';
SELECT TOP 5
  r.Id, r.TechnicalExamReportId, r.LegacyReferenceNumber, r.CreatedAt
FROM dbo.Request r
WHERE r.LegacyReferenceNumber IS NOT NULL
  AND r.Id IN (210483, 210484, 210485, 210486, 210487)
ORDER BY r.Id;

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
