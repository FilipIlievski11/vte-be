-- =============================================================================
-- Backfill Vehicle.Plate where it's the legacy sentinel "{CommunityCode}-000-AA"
-- (e.g. "VE-000-AA").
--
-- Background:
-- The legacy app stores "{Community.RegistrationCode}-000-AA" in
-- Vehicles.LastRegistratinNumber whenever a vehicle has no real plate yet.
-- migrate-vehicles.sql copied that column straight into dbo.Vehicle.Plate, so
-- many Veles vehicles now show "VE-000-AA" — even when a real plate exists
-- in VehicleRegistration.
--
-- The legacy print code (PrintVehcileInfo.vb LastRegistration getter, lines
-- 339-343) collapses the sentinel to "{Code}-" at display time:
--
--   If _lastRegistrationNumber = pom & "-000-AA" Then
--     Return pom & "-"
--   Else
--     Return _lastRegistrationNumber
--   End If
--
-- The frontend Zelen template already mirrors that runtime collapse. But where
-- a real VehicleRegistration row exists, we'd rather show the real plate.
-- This script lifts the most-recent registration's PlateNumber into
-- Vehicle.Plate for affected rows.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\backfill-vehicle-plate.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET QUOTED_IDENTIFIER ON;   -- required for UPDATE on Vehicle (filtered index) under sqlcmd
SET ANSI_NULLS ON;
SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

-- ---------------------------------------------------------------------------
-- 1. Diagnostic — how many vehicles carry the sentinel today, and how many of
--    those have a real registration we could promote?
-- ---------------------------------------------------------------------------
PRINT '=== Sentinel plates currently on Vehicle ===';
SELECT
  COUNT(*) AS SentinelVehicles
FROM dbo.Vehicle v
WHERE v.Plate IS NOT NULL
  AND v.Plate LIKE '[A-Z][A-Z]-000-AA';

PRINT '';
PRINT '=== Sentinel vehicles that also have a real registration ===';
SELECT
  COUNT(DISTINCT v.Id) AS VehiclesWithRealReg
FROM dbo.Vehicle v
INNER JOIN dbo.VehicleRegistration r ON r.VehicleId = v.Id
WHERE v.Plate LIKE '[A-Z][A-Z]-000-AA'
  AND r.PlateNumber IS NOT NULL
  AND r.PlateNumber NOT LIKE '[A-Z][A-Z]-000-AA'
  AND LEN(LTRIM(RTRIM(r.PlateNumber))) > 0;

-- ---------------------------------------------------------------------------
-- 2. Promote the most-recent real registration's PlateNumber into Vehicle.Plate
--    "Most recent" = highest ValidUntil, tie-break on RegisteredDate, then Id.
--    Keep Vehicle.Plate's sentinel for vehicles that have no real reg at all —
--    the frontend resolver collapses it to "VE-" at print time.
-- ---------------------------------------------------------------------------
PRINT '';
PRINT '=== Promoting real plates from VehicleRegistration ===';

;WITH RealReg AS (
  SELECT
    r.VehicleId,
    r.PlateNumber,
    ROW_NUMBER() OVER (
      PARTITION BY r.VehicleId
      ORDER BY r.ValidUntil DESC, r.RegisteredDate DESC, r.Id DESC
    ) AS rn
  FROM dbo.VehicleRegistration r
  WHERE r.PlateNumber IS NOT NULL
    AND r.PlateNumber NOT LIKE '[A-Z][A-Z]-000-AA'
    AND LEN(LTRIM(RTRIM(r.PlateNumber))) > 0
)
UPDATE v
SET v.Plate = LTRIM(RTRIM(rr.PlateNumber))
FROM dbo.Vehicle v
INNER JOIN RealReg rr ON rr.VehicleId = v.Id AND rr.rn = 1
WHERE v.Plate LIKE '[A-Z][A-Z]-000-AA';

DECLARE @promoted int = @@ROWCOUNT;
PRINT CONCAT('  -> ', @promoted, ' Vehicle rows updated with the real plate.');

-- ---------------------------------------------------------------------------
-- 3. Spot-check a few rows
-- ---------------------------------------------------------------------------
PRINT '';
PRINT '=== Sample after backfill ===';
SELECT TOP 10
  v.Id, v.Plate, v.Vin
FROM dbo.Vehicle v
WHERE v.Plate IS NOT NULL
  AND v.Plate NOT LIKE '[A-Z][A-Z]-000-AA'
ORDER BY v.Id DESC;

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
