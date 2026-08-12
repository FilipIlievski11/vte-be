-- backfill-vehicle-propulsion-doors.sql — четирите „полномошна-паритет" колони на
-- Vehicle (PropulsionAxleCount, DoorCount, HasHook, HasWinch) беа додадени по
-- бул-миграцијата и никогаш не се полнеа: 0/40.717 возила ги имаа (фатено
-- 2026-08-12 кај записникот — „Бр. погонски оски" печатеше 0 за сите).
--
-- Полн refresh од легаси за мигрираните возила (Id < 10M): PropulsionAxis,
-- NumberOfDoors (0 → NULL), Hook, Vitlo. Идемпотентна — повторно извршување
-- само ги презапишува истите вредности. migrate-incremental.sql е дополнет
-- истите колони да ги носи за идните нови возила.

USE VTE;
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

UPDATE tgt SET
    tgt.PropulsionAxleCount = NULLIF(src.PropulsionAxis, 0),
    tgt.DoorCount           = NULLIF(src.NumberOfDoors, 0),
    tgt.HasHook             = src.Hook,
    tgt.HasWinch            = src.Vitlo
FROM dbo.Vehicle tgt
JOIN VTEZVV_LIVE.VTEZVV.dbo.Vehicles src ON src.Id = tgt.Id
WHERE tgt.Id < 10000000;

PRINT CONCAT('Vozila osvezheni: ', @@ROWCOUNT);

COMMIT TRANSACTION;

SELECT 'so_pogonski'=COUNT(PropulsionAxleCount), 'so_vrati'=COUNT(DoorCount),
       'so_kuka'=SUM(CASE WHEN HasHook = 1 THEN 1 ELSE 0 END)
FROM dbo.Vehicle WHERE Id < 10000000;
GO
