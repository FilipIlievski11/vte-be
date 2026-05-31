-- =============================================================================
-- Backfill the Plav towing fields from the legacy snapshot:
--   Vehicle.MaxTrailerBrakedKg   <- Vehicles.MaxKonstVkMasaKocnaPrikolka   (e.g. 1500)
--   Vehicle.MaxTrailerUnbrakedKg <- Vehicles.MaxKonstVkMasaNeKocnaPrikolka (e.g. 750)
--   Vehicle.MaxHitchLoadKg       <- Vehicles.MaxKonstOptovaruvanjeVoPriklucok (e.g. 71)
--
-- These are the values the blue (Plav) form prints in the trailer / hitch boxes.
-- migrate-vehicles.sql never carried them (it mapped the empty TrailerWaight*
-- columns instead), so they are NULL until this runs. Apply the EF migration
-- AddVehicleTowingFields first (auto-applied on API startup), then run this.
--
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\backfill-vehicle-towing.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

-- sqlcmd connects with QUOTED_IDENTIFIER OFF by default; the Vehicle table has a
-- filtered index, so UPDATEs require these ON (SSMS sets them ON automatically).
SET QUOTED_IDENTIFIER ON;
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
IF COL_LENGTH('dbo.Vehicle', 'MaxTrailerBrakedKg') IS NULL
BEGIN
  RAISERROR('Vehicle.MaxTrailerBrakedKg not found. Apply EF migration AddVehicleTowingFields first (restart the API).', 16, 1);
  RETURN;
END;

PRINT '=== Backfilling towing fields from snapshot ===';
UPDATE v
SET v.MaxTrailerBrakedKg   = NULLIF(src.MaxKonstVkMasaKocnaPrikolka,    0),
    v.MaxTrailerUnbrakedKg = NULLIF(src.MaxKonstVkMasaNeKocnaPrikolka,  0),
    v.MaxHitchLoadKg       = NULLIF(src.MaxKonstOptovaruvanjeVoPriklucok, 0)
FROM dbo.Vehicle v
INNER JOIN VTEZVV_Snapshot.dbo.Vehicles src ON src.Id = v.Id;
DECLARE @n int = @@ROWCOUNT;
PRINT CONCAT('  -> ', @n, ' rows updated.');

PRINT '';
PRINT '=== Spot-check (vehicle 90515) ===';
SELECT Id, MaxTrailerBrakedKg, MaxTrailerUnbrakedKg, MaxHitchLoadKg
FROM dbo.Vehicle WHERE Id = 90515;

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
