-- =============================================================================
-- VTEZVV.Communities → VTE.Community
-- Source: (localdb)\MSSQLLocalDB\VTEZVV_Snapshot.dbo.Communities
-- Target: (localdb)\MSSQLLocalDB\VTE.dbo.Community
--
-- Preserves legacy Ids exactly. Defaults CountryId to the Country row with
-- ShortName='MK' (legacy Communities have no CountryId — Macedonian
-- municipalities only). Code is mapped from legacy CommunityCode;
-- PlateNumberPrefix is mapped from legacy RegistrationCode (with NULLIF on
-- blanks so empty strings don't fill the column).
--
-- Idempotent — wipes target first. Refuses to wipe if any City row points at
-- a Community (since we'd orphan it).
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-communities.sql -b -X -I -f 65001
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

-- Macedonia lookup — prefer the ISO MK row.
DECLARE @MacedoniaId smallint = (
  SELECT TOP 1 Id FROM dbo.Country
  WHERE ShortName IN (N'MK', N'MKD')
     OR Name LIKE N'%МАКЕДОН%'
  ORDER BY CASE WHEN ShortName = N'MK' THEN 0
                WHEN ShortName = N'MKD' THEN 1
                ELSE 2 END,
           Id
);
IF @MacedoniaId IS NULL
BEGIN
  RAISERROR('Macedonia not found in Country table — run migrate-countries.sql first.', 16, 1);
  RETURN;
END;
PRINT CONCAT('Macedonia CountryId = ', @MacedoniaId);

-- Block wipe if Cities exist that reference Communities
DECLARE @cityRefs int = (
  SELECT COUNT(*) FROM dbo.City ci
  WHERE EXISTS (SELECT 1 FROM dbo.Community co WHERE co.Id = ci.CommunityId)
);
IF @cityRefs > 0
BEGIN
  RAISERROR('Refusing to wipe Community: %d Cities reference current Communities. Clear City first.', 16, 1, @cityRefs);
  RETURN;
END;

PRINT '=== Wiping dbo.Community ===';
DELETE FROM dbo.Community;
DBCC CHECKIDENT ('dbo.Community', RESEED, 0) WITH NO_INFOMSGS;

PRINT '=== Inserting from VTEZVV_Snapshot.dbo.Communities (Ids preserved) ===';
SET IDENTITY_INSERT dbo.Community ON;
INSERT INTO dbo.Community (Id, Name, CountryId, Code, PlateNumberPrefix, Active)
SELECT
  c.Id,
  c.CommunityName,
  @MacedoniaId,
  NULLIF(LTRIM(RTRIM(c.CommunityCode)),   N''),
  NULLIF(LTRIM(RTRIM(c.RegistrationCode)), N''),
  c.Active
FROM VTEZVV_Snapshot.dbo.Communities c;
DECLARE @rows int = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.Community OFF;

DECLARE @maxId int = (SELECT ISNULL(MAX(Id), 0) FROM dbo.Community);
DBCC CHECKIDENT ('dbo.Community', RESEED, @maxId) WITH NO_INFOMSGS;

PRINT CONCAT('  -> ', @rows, ' rows inserted. Identity counter reseeded to ', @maxId, '.');

COMMIT;

PRINT '';
PRINT '=== Verification ===';
SELECT
  (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.Communities) AS LegacyCount,
  (SELECT COUNT(*) FROM dbo.Community)                   AS NewCount,
  (SELECT MIN(Id) FROM dbo.Community)                    AS MinId,
  (SELECT MAX(Id) FROM dbo.Community)                    AS MaxId;

PRINT 'First 10 rows side-by-side:';
SELECT TOP 10 'legacy' AS src, Id, CommunityName AS Name, CommunityCode AS Code, RegistrationCode AS [Plate], Active
FROM VTEZVV_Snapshot.dbo.Communities ORDER BY Id;

SELECT TOP 10 'new' AS src, Id, Name, Code, PlateNumberPrefix AS [Plate], Active, CountryId
FROM dbo.Community ORDER BY Id;

PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  PRINT '!!! Migration failed — rolled back';
  THROW;
END CATCH;
GO
