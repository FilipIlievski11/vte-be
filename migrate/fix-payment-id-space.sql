-- ============================================================================
-- One-time (2026-07-06): identity floor 10,000,000 for the payment tables,
-- same treatment as fix-v2-native-id-space.sql gave the 9 registry tables.
--
-- Why: legacy keeps inserting PaymentDocuments/Details below (currently ~326k /
-- ~1.6M) and v2 will start creating its own bills. Without the floor the two
-- id sequences collide and the new incremental bill sync would silently skip
-- or corrupt rows. As of today ALL rows in these tables are legacy-migrated
-- (LegacyId preserved, v2-native count = 0), so no relocation is needed —
-- only the reseed.
--
-- Idempotent: guarded by IDENT_CURRENT < @floor.
-- NEVER run a valueless DBCC CHECKIDENT(RESEED) on these tables afterwards.
-- ============================================================================
SET NOCOUNT ON;
DECLARE @floor bigint = 10000000;

DECLARE @t table (name sysname);
INSERT INTO @t VALUES ('PaymentDocument'), ('PaymentDocumentLine'),
                      ('InstallmentAgreement'), ('InstallmentSchedule');

DECLARE @name sysname, @cur bigint, @sql nvarchar(400);
DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT name FROM @t;
OPEN c;
FETCH NEXT FROM c INTO @name;
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @cur = CAST(IDENT_CURRENT(@name) AS bigint);
    IF @cur IS NULL
        PRINT CONCAT('  !! ', @name, ' has no identity — skipped.');
    ELSE IF @cur >= @floor
        PRINT CONCAT('  = ', @name, ' already floored (', @cur, ').');
    ELSE
    BEGIN
        SET @sql = CONCAT('DBCC CHECKIDENT(''dbo.', @name, ''', RESEED, ', @floor, ') WITH NO_INFOMSGS;');
        EXEC (@sql);
        PRINT CONCAT('  -> ', @name, ' reseeded ', @cur, ' -> ', @floor, '.');
    END
    FETCH NEXT FROM c INTO @name;
END
CLOSE c; DEALLOCATE c;
