-- backfill-client-citizenship.sql
-- Set CitizenshipId = Македонско (77) for every Client that has none.
--
-- Why: legacy never stored citizenship (Customers.IdCitizenship = 0 for all rows),
-- so ~28.6k migrated clients have CitizenshipId = NULL and every screen shows "—".
-- The station is Macedonian and virtually every client is a Macedonian citizen;
-- the ~24 real foreigners already carry their correct non-null CitizenshipId and
-- are NOT touched. Approved by Filip 2026-07-03 ("lets put makedonsko default").
--
-- Idempotent: re-running finds 0 NULL rows and does nothing.

SET NOCOUNT ON;

DECLARE @mk int;

-- Resolve by the dominant in-use row, not by hardcoded id: the lookup has duplicate
-- "Македонско" rows (77/84/87); 77 is the one 4,168 existing clients reference.
SELECT TOP 1 @mk = c.CitizenshipId
FROM Client c
JOIN Citizenship z ON z.Id = c.CitizenshipId
WHERE z.Name = N'Македонско'
GROUP BY c.CitizenshipId
ORDER BY COUNT(*) DESC;

IF @mk IS NULL
    SELECT TOP 1 @mk = Id FROM Citizenship WHERE Name = N'Македонско' ORDER BY Id;

IF @mk IS NULL
BEGIN
    PRINT 'ERROR: no "Македонско" row in Citizenship — nothing changed.';
    RETURN;
END

DECLARE @before int = (SELECT COUNT(*) FROM Client WHERE CitizenshipId IS NULL);

UPDATE Client SET CitizenshipId = @mk WHERE CitizenshipId IS NULL;

DECLARE @msg nvarchar(200) =
    N'Backfilled ' + CAST(@before AS nvarchar(20)) + N' clients to CitizenshipId=' + CAST(@mk AS nvarchar(10))
    + N' (Македонско). Remaining NULL: ' + CAST((SELECT COUNT(*) FROM Client WHERE CitizenshipId IS NULL) AS nvarchar(20));
PRINT @msg;
