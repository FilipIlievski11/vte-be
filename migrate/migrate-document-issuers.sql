-- =============================================================================
-- VTEZVV.RegistrationIssuers → VTE.DocumentIssuer
-- Source: (localdb)\MSSQLLocalDB\VTEZVV_Snapshot.dbo.RegistrationIssuers
-- Target: (localdb)\MSSQLLocalDB\VTE.dbo.DocumentIssuer
--
-- Preserves legacy Ids exactly. The legacy IdCommunity link is NOT copied here,
-- but it IS needed for the Plav print's destination MVR — run
-- migrate/backfill-document-issuer-community.sql AFTER this to restore
-- DocumentIssuer.CommunityId (added by EF migration AddDocumentIssuerCommunityId).
-- Legacy Id is INT but new schema is TINYINT (max 255) — script reports if any
-- legacy Id exceeds 255 and skips it.
--
-- Idempotent — wipes target first. Refuses to wipe if any ClientPersonalData
-- row references a DocumentIssuer.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-document-issuers.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN
  RAISERROR('Source DB VTEZVV_Snapshot is missing. Run migrate\snapshot-legacy.ps1 first.', 16, 1);
  RETURN;
END;

DECLARE @inUse int = (SELECT COUNT(*) FROM dbo.ClientPersonalData);
IF @inUse > 0
BEGIN
  RAISERROR('Refusing to wipe DocumentIssuer: %d ClientPersonalData rows reference it. Clear them first.', 16, 1, @inUse);
  RETURN;
END;

DECLARE @legacyTotal int = (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.RegistrationIssuers);
DECLARE @overflow    int = (
  SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.RegistrationIssuers WHERE Id > 255 OR Id < 1
);
PRINT CONCAT('Legacy RegistrationIssuers: ', @legacyTotal,
             '   Will skip (Id outside tinyint 1..255): ', @overflow);

PRINT '=== Wiping dbo.DocumentIssuer ===';
DELETE FROM dbo.DocumentIssuer;
DBCC CHECKIDENT ('dbo.DocumentIssuer', RESEED, 0) WITH NO_INFOMSGS;

PRINT '=== Inserting (Ids preserved) ===';
SET IDENTITY_INSERT dbo.DocumentIssuer ON;
INSERT INTO dbo.DocumentIssuer (Id, Name, Active)
SELECT
  CAST(r.Id AS tinyint),
  r.IssuerName,
  r.Active
FROM VTEZVV_Snapshot.dbo.RegistrationIssuers r
WHERE r.Id BETWEEN 1 AND 255;
DECLARE @rows int = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.DocumentIssuer OFF;

DECLARE @maxId tinyint = (SELECT ISNULL(MAX(Id), 0) FROM dbo.DocumentIssuer);
DBCC CHECKIDENT ('dbo.DocumentIssuer', RESEED, @maxId) WITH NO_INFOMSGS;

PRINT CONCAT('  -> ', @rows, ' rows inserted. Identity counter reseeded to ', @maxId, '.');

COMMIT;

PRINT '';
PRINT '=== Verification ===';
SELECT
  (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.RegistrationIssuers) AS LegacyCount,
  (SELECT COUNT(*) FROM dbo.DocumentIssuer)                      AS NewCount,
  (SELECT MIN(Id) FROM dbo.DocumentIssuer)                       AS MinId,
  (SELECT MAX(Id) FROM dbo.DocumentIssuer)                       AS MaxId;

PRINT 'First 10 rows side-by-side:';
SELECT TOP 10 'legacy' AS src, Id, IssuerName AS Name, Active
FROM VTEZVV_Snapshot.dbo.RegistrationIssuers ORDER BY Id;

SELECT TOP 10 'new' AS src, Id, Name, Active
FROM dbo.DocumentIssuer ORDER BY Id;

PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  PRINT '!!! Migration failed — rolled back';
  THROW;
END CATCH;
GO
