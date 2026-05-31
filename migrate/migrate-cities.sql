-- =============================================================================
-- VTEZVV.Cities → VTE.City
-- Source: (localdb)\MSSQLLocalDB\VTEZVV_Snapshot.dbo.Cities
-- Target: (localdb)\MSSQLLocalDB\VTE.dbo.City
--
-- Preserves legacy Ids. Joins on Community so we only insert Cities whose
-- IdCommunityCode resolves to an existing Community row (gaps are reported).
-- Legacy CityZip is INT (nullable); maps to PostalCode nvarchar(40):
--   - NULL / 0  → '0000'
--   - otherwise → printable int
--
-- Idempotent — wipes target first. Refuses to wipe if any Client references
-- a City row.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-cities.sql -b -X -I -f 65001
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

IF (SELECT COUNT(*) FROM dbo.Community) = 0
BEGIN
  RAISERROR('No Community rows in target. Run migrate-communities.sql first.', 16, 1);
  RETURN;
END;

-- Refuse if Clients already point at Cities
DECLARE @clientRefs int = (SELECT COUNT(*) FROM dbo.Client WHERE CityId IS NOT NULL);
IF @clientRefs > 0
BEGIN
  RAISERROR('Refusing to wipe City: %d Clients have CityId set. Clear them first.', 16, 1, @clientRefs);
  RETURN;
END;

PRINT '=== Wiping dbo.City ===';
DELETE FROM dbo.City;
DBCC CHECKIDENT ('dbo.City', RESEED, 0) WITH NO_INFOMSGS;

-- Diagnostics: how many legacy Cities point at a Community that does not exist?
DECLARE @legacyTotal int   = (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.Cities);
DECLARE @orphaned    int   = (
  SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.Cities c
  LEFT JOIN dbo.Community co ON co.Id = c.IdCommunityCode
  WHERE co.Id IS NULL
);
PRINT CONCAT('Legacy Cities: ', @legacyTotal, '   Will skip (no matching Community): ', @orphaned);

PRINT '=== Inserting (Ids preserved, joined on Community) ===';
SET IDENTITY_INSERT dbo.City ON;
INSERT INTO dbo.City (Id, Name, CommunityId, PostalCode, Active)
SELECT
  c.Id,
  c.CityName,
  c.IdCommunityCode,
  CASE
    WHEN c.CityZip IS NULL OR c.CityZip = 0 THEN N'0000'
    ELSE CAST(c.CityZip AS nvarchar(40))
  END,
  c.Active
FROM VTEZVV_Snapshot.dbo.Cities c
INNER JOIN dbo.Community co ON co.Id = c.IdCommunityCode;
DECLARE @rows int = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.City OFF;

DECLARE @maxId int = (SELECT ISNULL(MAX(Id), 0) FROM dbo.City);
DBCC CHECKIDENT ('dbo.City', RESEED, @maxId) WITH NO_INFOMSGS;

PRINT CONCAT('  -> ', @rows, ' rows inserted. Identity counter reseeded to ', @maxId, '.');

COMMIT;

PRINT '';
PRINT '=== Verification ===';
SELECT
  (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.Cities) AS LegacyCount,
  (SELECT COUNT(*) FROM dbo.City)                   AS NewCount,
  (SELECT MIN(Id) FROM dbo.City)                    AS MinId,
  (SELECT MAX(Id) FROM dbo.City)                    AS MaxId;

PRINT 'Per-community counts (top 10 by row count):';
SELECT TOP 10
  co.Id AS CommunityId, co.Name AS Community, COUNT(*) AS Cities
FROM dbo.City ci
JOIN dbo.Community co ON co.Id = ci.CommunityId
GROUP BY co.Id, co.Name
ORDER BY COUNT(*) DESC;

PRINT 'First 10 cities (sample):';
SELECT TOP 10 ci.Id, ci.Name AS City, ci.PostalCode AS Zip, co.Name AS Community, ci.Active
FROM dbo.City ci
JOIN dbo.Community co ON co.Id = ci.CommunityId
ORDER BY ci.Id;

PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  PRINT '!!! Migration failed — rolled back';
  THROW;
END CATCH;
GO
