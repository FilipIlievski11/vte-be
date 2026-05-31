-- =============================================================================
-- Backfill Vehicle.MaxLegalGroupMassKg from the legacy snapshot column
-- Vehicles.MaxLegVkMasaGrupa (max permissible laden mass of the whole
-- combination — vehicle + trailer). This is field F.3 on the Plav (blue) form,
-- e.g. "3130" for the Fiat Stilo in request 210509.
--
-- migrate-vehicles.sql didn't carry this column, so it is NULL until this runs.
-- Apply the AddVehicleMaxLegalGroupMass EF migration first (auto-applied on API
-- startup via Program.cs MigrateAsync), then run this.
--
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\backfill-vehicle-group-mass.sql -b -X -I -f 65001
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
IF COL_LENGTH('dbo.Vehicle', 'MaxLegalGroupMassKg') IS NULL
BEGIN
  RAISERROR('Vehicle.MaxLegalGroupMassKg not found. Apply EF migration AddVehicleMaxLegalGroupMass first (restart the API).', 16, 1);
  RETURN;
END;

PRINT '=== Backfilling Vehicle.MaxLegalGroupMassKg from MaxLegVkMasaGrupa ===';
UPDATE v
SET v.MaxLegalGroupMassKg = NULLIF(src.MaxLegVkMasaGrupa, 0)
FROM dbo.Vehicle v
INNER JOIN VTEZVV_Snapshot.dbo.Vehicles src ON src.Id = v.Id
WHERE v.MaxLegalGroupMassKg IS NULL
  AND src.MaxLegVkMasaGrupa > 0;
DECLARE @n int = @@ROWCOUNT;
PRINT CONCAT('  -> ', @n, ' rows updated.');

PRINT '';
PRINT '=== Spot-check (vehicle 90514 should be 3130) ===';
SELECT Id, MaxLegalTotalMassKg, MaxConstructiveTotalMassKg, MaxLegalGroupMassKg
FROM dbo.Vehicle WHERE Id = 90514;

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
