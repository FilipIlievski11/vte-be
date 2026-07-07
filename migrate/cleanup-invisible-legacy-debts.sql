-- ============================================================================
-- One-time cleanup (2026-07-06): remove imported legacy debts that legacy itself
-- never displays.
--
-- The legacy dashboard's Наплата reads dbo.depCustomerFinansicalStateView, which
-- INNER JOINs Vehicles/VehicleModel/VehicleMakers — a CustomerFinancialState row
-- anchored to a vehicle-less relation is INVISIBLE in legacy (dead debris, some
-- rows dating to 2012). The 2026-07-06 gap-fill sync imported 189 of them into
-- CustomerDebt, flooding v2's Наплата with items the station has never seen.
--
-- migrate-incremental.sql now imports only debts whose relation carries a vehicle
-- (mirrors legacy visibility), so these rows will not come back.
--
-- Deletes ONLY:
--   * imported rows (LegacyId IS NOT NULL) — v2-native debts are untouched;
--   * never billed in v2 (SettledByLineId IS NULL) — a debt settled by a v2
--     payment document must never be deleted.
-- Idempotent: second run deletes 0.
-- ============================================================================
SET NOCOUNT ON;

DECLARE @n int;

DELETE d
FROM dbo.CustomerDebt d
INNER JOIN dbo.ClientVehicleRelation rel ON rel.Id = d.CustomerVehicleRelationId
WHERE d.LegacyId IS NOT NULL
  AND d.SettledByLineId IS NULL
  AND rel.VehicleId IS NULL;
SET @n = @@ROWCOUNT;
PRINT CONCAT('=== invisible-in-legacy imported debts deleted: ', @n, ' ===');

SELECT
  (SELECT COUNT(*) FROM dbo.CustomerDebt WHERE LegacyId IS NOT NULL)   AS importedRemaining,
  (SELECT COUNT(*) FROM dbo.CustomerDebt WHERE Paid = 0 AND Active = 1) AS openDebts;
