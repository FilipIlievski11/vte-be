-- =============================================================================
-- VTEZVV.Countries → VTE.Country
-- Source: (localdb)\MSSQLLocalDB\VTEZVV_Snapshot.dbo.Countries
-- Target: (localdb)\MSSQLLocalDB\VTE.dbo.Country
--
-- Preserves legacy Ids exactly (cast int → smallint; legacy has ≤49 rows).
-- Drops & re-inserts target rows so the script is safe to re-run.
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

-- Verify no FK-dependent rows would block the wipe
DECLARE @cit int = (SELECT COUNT(*) FROM dbo.Citizenship);
DECLARE @com int = (SELECT COUNT(*) FROM dbo.Community);
DECLARE @cli int = (SELECT COUNT(*) FROM dbo.Client WHERE CitizenshipId IS NOT NULL);
IF (@cit + @com + @cli) > 0
BEGIN
  RAISERROR('Refusing to wipe Country: there are %d Citizenship rows, %d Community rows, %d Clients with citizenship — clear them first or extend this script.',
            16, 1, @cit, @com, @cli);
  RETURN;
END;

PRINT '=== Wiping dbo.Country ===';
DELETE FROM dbo.Country;
DBCC CHECKIDENT ('dbo.Country', RESEED, 0) WITH NO_INFOMSGS;

PRINT '=== Inserting from VTEZVV_Snapshot.dbo.Countries (Ids preserved) ===';
SET IDENTITY_INSERT dbo.Country ON;
INSERT INTO dbo.Country (Id, Name, ShortName, Active)
SELECT
  CAST(c.Id AS smallint),
  c.CountryName,
  c.CountryShortName,
  c.Active
FROM VTEZVV_Snapshot.dbo.Countries c
WHERE c.Id BETWEEN 1 AND 32767;
DECLARE @rows int = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.Country OFF;

-- Reseed the counter past the largest preserved Id so future inserts don't collide
DECLARE @maxId smallint = (SELECT ISNULL(MAX(Id), 0) FROM dbo.Country);
DBCC CHECKIDENT ('dbo.Country', RESEED, @maxId) WITH NO_INFOMSGS;

PRINT CONCAT('  -> ', @rows, ' rows inserted. Identity counter reseeded to ', @maxId, '.');

COMMIT;

PRINT '';
PRINT '=== Verification ===';
SELECT
  (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.Countries) AS LegacyCount,
  (SELECT COUNT(*) FROM dbo.Country)                  AS NewCount,
  (SELECT MIN(Id) FROM dbo.Country)                   AS MinId,
  (SELECT MAX(Id) FROM dbo.Country)                   AS MaxId;

PRINT 'First 5 rows side-by-side:';
SELECT TOP 5 'legacy' AS src, Id, CountryName AS Name, CountryShortName AS ShortName, Active
FROM VTEZVV_Snapshot.dbo.Countries ORDER BY Id;

SELECT TOP 5 'new' AS src, Id, Name, ShortName, Active
FROM dbo.Country ORDER BY Id;

PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  PRINT '!!! Migration failed — rolled back';
  THROW;
END CATCH;
GO
