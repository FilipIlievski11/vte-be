-- =============================================================================
-- Backfill Vehicle.ManufactureDate from legacy VTEZVV_Snapshot.dbo.Vehicles.MakeDate.
--
-- The AddVehicleManufactureDate EF migration adds the column. This script
-- populates it for all 40k+ rows we previously migrated.
--
-- Idempotent — only fills rows where ManufactureDate IS NULL.
--
-- Also fixes StandingSeats: the original migrate-vehicles.sql used
-- NULLIF(v.NumberOfStandingSeats, 0) which clobbered 0 to NULL. Standing-seats
-- count of 0 IS a meaningful value (passenger cars have 0 standing seats).
-- We restore 0 wherever the legacy held 0.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\backfill-vehicle-manufacture-date.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET QUOTED_IDENTIFIER ON;   -- required for UPDATE on Vehicle (filtered index) under sqlcmd
SET ANSI_NULLS ON;
SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN
  RAISERROR('Source DB VTEZVV_Snapshot is missing. Reattach it first.', 16, 1);
  RETURN;
END;
IF COL_LENGTH('dbo.Vehicle', 'ManufactureDate') IS NULL
BEGIN
  RAISERROR('Vehicle.ManufactureDate column not found. Apply EF migration AddVehicleManufactureDate first.', 16, 1);
  RETURN;
END;

-- ============================================================================
-- 1. Backfill ManufactureDate
-- ============================================================================
PRINT '=== Backfilling Vehicle.ManufactureDate from legacy MakeDate ===';
UPDATE v
SET v.ManufactureDate = src.MakeDate
FROM dbo.Vehicle v
INNER JOIN VTEZVV_Snapshot.dbo.Vehicles src ON src.Id = v.Id
WHERE v.ManufactureDate IS NULL
  AND src.MakeDate IS NOT NULL;
DECLARE @dates int = @@ROWCOUNT;
PRINT CONCAT('  -> ', @dates, ' rows updated.');

-- ============================================================================
-- 2. Restore StandingSeats = 0 where the legacy had 0
--    (migrate-vehicles.sql used NULLIF(..., 0) which set them to NULL)
-- ============================================================================
PRINT '=== Restoring Vehicle.StandingSeats = 0 ===';
UPDATE v
SET v.StandingSeats = 0
FROM dbo.Vehicle v
INNER JOIN VTEZVV_Snapshot.dbo.Vehicles src ON src.Id = v.Id
WHERE v.StandingSeats IS NULL
  AND src.NumberOfStandingSeats = 0;
DECLARE @standing int = @@ROWCOUNT;
PRINT CONCAT('  -> ', @standing, ' rows updated.');

-- ============================================================================
-- Spot-check
-- ============================================================================
PRINT '';
PRINT '=== Sample after backfill ===';
SELECT TOP 5
  v.Id, v.Vin, v.ManufactureDate, v.Seats, v.StandingSeats, v.HasLpg
FROM dbo.Vehicle v
WHERE v.Id IN (
  SELECT TOP 5 Id FROM VTEZVV_Snapshot.dbo.Vehicles
  WHERE MakeDate IS NOT NULL ORDER BY Id
)
ORDER BY v.Id;

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
