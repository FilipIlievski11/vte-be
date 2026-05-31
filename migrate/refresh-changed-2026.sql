-- =============================================================================
-- 2026 REFRESH — bring stale migrated records in line with the live legacy DB.
-- The base migration was a point-in-time snapshot; some 2026 vehicles/clients
-- were edited in live afterwards (plate re-issued, model corrected, name fixed),
-- and 44 vehicles synced before ManufactureDate was added carry a NULL make-year.
-- This UPDATES (in place) only the records reachable from a 2026 request, only
-- where a print-relevant field actually differs. Re-runnable / idempotent.
--
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\refresh-changed-2026.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET NOCOUNT ON;

-- 1) Live 2026 vehicle fields (Id preserved across migration).
IF OBJECT_ID('tempdb..#liveVeh') IS NOT NULL DROP TABLE #liveVeh;
SELECT * INTO #liveVeh FROM OPENQUERY(VTEZVV_LIVE, '
  SELECT DISTINCT ve.Id AS VehId,
         NULLIF(LTRIM(RTRIM(ve.LastRegistratinNumber)), '''') AS Plate,
         NULLIF(ve.IdVehicleModel, 0)                          AS ModelId,
         ve.MakeDate                                           AS MakeDate
  FROM VTEZVV.dbo.Requests r
  JOIN VTEZVV.dbo.CustomerVehiclesRelations cvr ON cvr.Id = r.IdCustomerVehicleRelation
  JOIN VTEZVV.dbo.Vehicles ve ON ve.Id = cvr.IdVehicle
  WHERE YEAR(r.DateCreated) = 2026');

-- 2) Refresh v2 vehicles where plate / model / make-year drifted.
--    ModelId uses COALESCE so a model added in live after migration (absent from
--    the v2 lookup) never nulls an existing value or violates the FK.
UPDATE v
  SET v.Plate           = lv.Plate,
      v.ModelId         = COALESCE(mdl.Id, v.ModelId),
      v.ManufactureDate = lv.MakeDate
FROM dbo.Vehicle v
JOIN #liveVeh lv ON lv.VehId = v.Id
LEFT JOIN dbo.VehicleModel mdl ON mdl.Id = lv.ModelId
WHERE ISNULL(v.Plate, N'#')                  <> ISNULL(lv.Plate, N'#')
   OR ISNULL(v.ModelId, -1)                  <> ISNULL(mdl.Id, -1)
   OR ISNULL(YEAR(v.ManufactureDate), -1)    <> ISNULL(YEAR(lv.MakeDate), -1);
PRINT '=== Vehicles refreshed: ' + CAST(@@ROWCOUNT AS varchar(10)) + ' ===';

-- 3) Live 2026 client names (legacy CustomerFirstName=surname, CustomerSurname=given).
IF OBJECT_ID('tempdb..#liveCli') IS NOT NULL DROP TABLE #liveCli;
SELECT * INTO #liveCli FROM OPENQUERY(VTEZVV_LIVE, '
  SELECT DISTINCT cu.Id AS CliId,
         NULLIF(LTRIM(RTRIM(cu.CustomerFirstName)), '''') AS FirstName,
         NULLIF(LTRIM(RTRIM(cu.CustomerSurname)),   '''') AS LastName
  FROM VTEZVV.dbo.Requests r
  JOIN VTEZVV.dbo.CustomerVehiclesRelations cvr ON cvr.Id = r.IdCustomerVehicleRelation
  JOIN VTEZVV.dbo.Customers cu ON cu.Id = cvr.IdCustomer
  WHERE YEAR(r.DateCreated) = 2026');

-- 4) Refresh v2 client names where they drifted.
UPDATE c
  SET c.FirstName = lc.FirstName,
      c.LastName  = lc.LastName
FROM dbo.Client c
JOIN #liveCli lc ON lc.CliId = c.Id
WHERE ISNULL(c.FirstName, N'#') <> ISNULL(lc.FirstName, N'#')
   OR ISNULL(c.LastName,  N'#') <> ISNULL(lc.LastName,  N'#');
PRINT '=== Clients refreshed: ' + CAST(@@ROWCOUNT AS varchar(10)) + ' ===';

DROP TABLE #liveVeh; DROP TABLE #liveCli;
PRINT '=== Refresh done ===';
