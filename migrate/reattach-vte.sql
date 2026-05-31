-- =============================================================================
-- Re-attach VTE + VTEZVV_Snapshot to (localdb)\MSSQLLocalDB.
-- Idempotent — silently skips DBs that are already attached.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\reattach-vte.sql -b -X -I
-- =============================================================================
SET NOCOUNT ON;

IF DB_ID('VTE') IS NULL
BEGIN
  PRINT 'Attaching VTE...';
  CREATE DATABASE [VTE] ON
    (FILENAME = 'C:\Users\filip\VTE.mdf'),
    (FILENAME = 'C:\Users\filip\VTE_log.ldf')
    FOR ATTACH;
  PRINT '  -> VTE attached.';
END
ELSE
  PRINT 'VTE already attached.';

IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN
  PRINT 'Attaching VTEZVV_Snapshot...';
  CREATE DATABASE [VTEZVV_Snapshot] ON
    (FILENAME = 'C:\Users\filip\VTEZVV_Snapshot.mdf'),
    (FILENAME = 'C:\Users\filip\VTEZVV_Snapshot_log.ldf')
    FOR ATTACH;
  PRINT '  -> VTEZVV_Snapshot attached.';
END
ELSE
  PRINT 'VTEZVV_Snapshot already attached.';

PRINT '';
PRINT 'Attached user databases:';
SELECT name, state_desc FROM sys.databases WHERE database_id > 4 ORDER BY name;
