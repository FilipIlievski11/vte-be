-- =============================================================================
-- Backfill VehiclePaymentCategory.ZelenMap from the legacy snapshot
-- (VehicleCategoryForPayments.ZelenMap). This drives which category checkbox the
-- Plav/Zelen forms tick (1=passenger, 2=cargo, 3=trailer, 4=moto; 0/5-11 = none).
--
-- Apply the EF migration AddPaymentCategoryZelenMap first (auto-applied on API
-- startup via Program.cs MigrateAsync), then run this.
--
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\backfill-payment-category-zelenmap.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

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
IF COL_LENGTH('dbo.VehiclePaymentCategory', 'ZelenMap') IS NULL
BEGIN
  RAISERROR('VehiclePaymentCategory.ZelenMap not found. Apply EF migration AddPaymentCategoryZelenMap first (restart the API).', 16, 1);
  RETURN;
END;

PRINT '=== Backfilling VehiclePaymentCategory.ZelenMap ===';
UPDATE pc
SET pc.ZelenMap = CAST(src.ZelenMap AS tinyint)
FROM dbo.VehiclePaymentCategory pc
INNER JOIN VTEZVV_Snapshot.dbo.VehicleCategoryForPayments src ON src.Id = pc.Id;
DECLARE @n int = @@ROWCOUNT;
PRINT CONCAT('  -> ', @n, ' rows updated.');

PRINT '';
PRINT '=== Distribution by ZelenMap ===';
SELECT ZelenMap, COUNT(*) AS Categories
FROM dbo.VehiclePaymentCategory GROUP BY ZelenMap ORDER BY ZelenMap;

COMMIT TRANSACTION;
PRINT '';
PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
  PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
  THROW;
END CATCH;
