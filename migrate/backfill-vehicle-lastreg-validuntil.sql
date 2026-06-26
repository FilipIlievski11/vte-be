-- =============================================================================
-- Backfill Vehicle.LastRegistrationValidUntil from the live legacy
-- Vehicles.LastRegistrationValidTill — the AUTHORITATIVE registration expiry the MVR
-- forms print ("Регистрација важи до" on Zelen, "Рег. важи до" on Plav).
--
-- Why: legacy keeps the last registration denormalized on the Vehicles row. The
-- Vehicle.Registrations child table can hold only a "{Code}-000-AA" sentinel
-- placeholder for some vehicles (e.g. deregistered ones), so deriving the expiry from
-- the v2 VehicleRegistration table shows the sentinel's date instead of the real one
-- (see request #211690: VE-008-BS expired 26.04.2005, but the sentinel said 25.06.2027).
-- migrate-vehicles.sql + migrate-incremental.sql now populate this column on insert;
-- this script backfills an existing DB. Re-runnable / idempotent.
--
-- Run on the LocalDB carrying the VTEZVV_LIVE linked server:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\backfill-vehicle-lastreg-validuntil.sql -b -X -I
-- Prod has no linked server: backfill local then re-lift, OR apply the same (Id, date)
-- pairs to prod via a staging table + UPDATE JOIN (how the 2026-06 prod backfill was done).
-- =============================================================================
USE VTE;
GO
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM sys.servers WHERE name = 'VTEZVV_LIVE')
BEGIN RAISERROR('Linked server VTEZVV_LIVE missing. Create it first.', 16, 1); RETURN; END;

UPDATE v
  SET v.LastRegistrationValidUntil = src.D
FROM dbo.Vehicle v
JOIN OPENQUERY(VTEZVV_LIVE,
  'SELECT Id, LastRegistrationValidTill
     FROM VTEZVV.dbo.Vehicles
    WHERE LastRegistrationValidTill > ''1900-01-01'' AND LastRegistrationValidTill < ''2100-01-01''') src(Id, D)
  ON src.Id = v.Id
WHERE v.LastRegistrationValidUntil IS NULL
   OR v.LastRegistrationValidUntil <> src.D;
PRINT CONCAT('=== Vehicle.LastRegistrationValidUntil backfilled: ', @@ROWCOUNT, ' ===');
