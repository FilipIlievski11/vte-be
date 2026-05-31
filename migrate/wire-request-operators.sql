-- =============================================================================
-- Wire the migrated REQUESTS to their real operators.
--
-- The request migration set CreatedByUserId/ModifiedByUserId/EndedByUserId to the
-- seeded "admin" and stashed the legacy operator ids in the Note
-- ("[legacy operator ids C=<created> M=<modified> E=<ended>]"). The operators are
-- the same VTESecurity users we already recreated for the tech-exam controllers
-- (AspNetUsers.Id == legacy VTESecurity user id). This script:
--   1. maps every v2 request → its legacy operator ids (snapshot for the bulk,
--      note-parse for the few incremental rows not in the snapshot),
--   2. recreates any still-missing referenced operators from VTESecurity,
--   3. repoints CreatedByUserId / ModifiedByUserId / EndedByUserId to those users.
--
-- The requests list resolves CreatedByUserId → FullName, so after this the
-- "ОПЕРАТОР" column shows the real operator (no backend rebuild needed).
-- Re-runnable. Operators with no VTESecurity match stay on "admin".
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\wire-request-operators.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET XACT_ABORT ON; SET NOCOUNT ON;

DECLARE @operatorRoleId nvarchar(450) = (SELECT Id FROM dbo.AspNetRoles WHERE NormalizedName = N'OPERATOR');
IF @operatorRoleId IS NULL
BEGIN RAISERROR('Operator role not found — boot the API once.', 16, 1); RETURN; END;

-- ---------------------------------------------------------------------------
-- 1) Map request -> legacy operator ids.
-- ---------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#map') IS NOT NULL DROP TABLE #map;
CREATE TABLE #map (ReqId bigint NOT NULL PRIMARY KEY, C int NULL, M int NULL, E int NULL);
-- 1a) bulk: from the snapshot (authoritative IdOperator* columns), for v2 requests that exist there.
INSERT INTO #map (ReqId, C, M, E)
SELECT s.Id, s.IdOperatorCreated, s.IdOperatorModified, s.IdOperatorEnded
FROM VTEZVV_Snapshot.dbo.Requests s
WHERE EXISTS (SELECT 1 FROM dbo.Request r WHERE r.Id = s.Id);

-- 1b) the rest (incremental rows added after the snapshot) → parse the note marker.
INSERT INTO #map (ReqId, C, M, E)
SELECT r.Id,
       TRY_CAST(SUBSTRING(r.Note, mk + 24, CHARINDEX(N' M=', r.Note, mk) - (mk + 24)) AS int),
       TRY_CAST(SUBSTRING(r.Note, CHARINDEX(N' M=', r.Note, mk) + 3, CHARINDEX(N' E=', r.Note, mk) - (CHARINDEX(N' M=', r.Note, mk) + 3)) AS int),
       TRY_CAST(SUBSTRING(r.Note, CHARINDEX(N' E=', r.Note, mk) + 3, CHARINDEX(N']', r.Note, mk) - (CHARINDEX(N' E=', r.Note, mk) + 3)) AS int)
FROM (
    SELECT r.Id, r.Note, CHARINDEX(N' [legacy operator ids C=', r.Note) AS mk
    FROM dbo.Request r
    WHERE NOT EXISTS (SELECT 1 FROM #map m WHERE m.ReqId = r.Id)
) r
WHERE r.mk > 0;
DECLARE @mapped int = (SELECT COUNT(*) FROM #map);
PRINT CONCAT('Mapped requests: ', @mapped);

-- ---------------------------------------------------------------------------
-- 2) Recreate any referenced operators that aren't users yet (from VTESecurity).
-- ---------------------------------------------------------------------------
IF OBJECT_ID('tempdb..#vu') IS NOT NULL DROP TABLE #vu;
SELECT * INTO #vu FROM OPENQUERY(VTEZVV_LIVE,
  'SELECT u.ID, u.UserName, u.UserFullName, u.FirstName, u.SureName, u.Active FROM VTESecurity.dbo.Users u');

IF OBJECT_ID('tempdb..#ops') IS NOT NULL DROP TABLE #ops;
SELECT DISTINCT op INTO #ops FROM (
    SELECT C AS op FROM #map WHERE C > 0
    UNION SELECT M FROM #map WHERE M > 0
    UNION SELECT E FROM #map WHERE E > 0
) x;

INSERT INTO dbo.AspNetUsers
  (Id, CompanyId, FullName, IsActive, CreatedAt, UserName, NormalizedUserName,
   Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
   PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
SELECT CAST(v.ID AS nvarchar(450)), CAST(4 AS tinyint),
       COALESCE(NULLIF(LTRIM(RTRIM(v.UserFullName)), N''),
                NULLIF(LTRIM(RTRIM(CONCAT_WS(N' ', v.FirstName, v.SureName))), N''),
                N'Оператор #' + CAST(v.ID AS nvarchar(10))),
       CAST(ISNULL(v.Active, 0) AS bit), GETUTCDATE(),
       LEFT(LTRIM(RTRIM(v.UserName)) + N'.' + CAST(v.ID AS nvarchar(10)), 256),
       UPPER(LEFT(LTRIM(RTRIM(v.UserName)) + N'.' + CAST(v.ID AS nvarchar(10)), 256)),
       NULL, NULL, 0, NULL, CONVERT(nvarchar(450), NEWID()), CONVERT(nvarchar(450), NEWID()),
       NULL, 0, 0, NULL, 1, 0
FROM #ops o JOIN #vu v ON v.ID = o.op
WHERE NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = CAST(v.ID AS nvarchar(450)))
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.NormalizedUserName = UPPER(LEFT(LTRIM(RTRIM(v.UserName)) + N'.' + CAST(v.ID AS nvarchar(10)), 256)));
PRINT CONCAT('  -> ', @@ROWCOUNT, ' additional operator users created.');

INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
SELECT CAST(v.ID AS nvarchar(450)), @operatorRoleId
FROM #ops o JOIN #vu v ON v.ID = o.op
WHERE EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = CAST(v.ID AS nvarchar(450)))
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUserRoles ur WHERE ur.UserId = CAST(v.ID AS nvarchar(450)) AND ur.RoleId = @operatorRoleId);
PRINT CONCAT('  -> ', @@ROWCOUNT, ' role assignments added.');

-- ---------------------------------------------------------------------------
-- 3) Repoint the request audit users to the real operators (where the user exists).
-- ---------------------------------------------------------------------------
UPDATE r SET r.CreatedByUserId = CAST(m.C AS nvarchar(450))
FROM dbo.Request r JOIN #map m ON m.ReqId = r.Id
WHERE m.C > 0 AND EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = CAST(m.C AS nvarchar(450)));
PRINT CONCAT('CreatedByUserId wired: ', @@ROWCOUNT);

UPDATE r SET r.ModifiedByUserId = CAST(m.M AS nvarchar(450))
FROM dbo.Request r JOIN #map m ON m.ReqId = r.Id
WHERE m.M > 0 AND r.ModifiedByUserId IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = CAST(m.M AS nvarchar(450)));
PRINT CONCAT('ModifiedByUserId wired: ', @@ROWCOUNT);

UPDATE r SET r.EndedByUserId = CAST(m.E AS nvarchar(450))
FROM dbo.Request r JOIN #map m ON m.ReqId = r.Id
WHERE m.E > 0 AND r.EndedByUserId IS NOT NULL AND EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = CAST(m.E AS nvarchar(450)));
PRINT CONCAT('EndedByUserId wired: ', @@ROWCOUNT);

-- ---------------------------------------------------------------------------
-- 4) Coverage summary.
-- ---------------------------------------------------------------------------
DECLARE @admin nvarchar(450) = (SELECT Id FROM dbo.AspNetUsers WHERE NormalizedUserName = N'ADMIN');
PRINT '=== Coverage ===';
SELECT
  (SELECT COUNT(*) FROM dbo.Request) AS TotalRequests,
  (SELECT COUNT(*) FROM dbo.Request WHERE CreatedByUserId <> @admin) AS WiredToOperator,
  (SELECT COUNT(*) FROM dbo.Request WHERE CreatedByUserId = @admin) AS StillAdmin;
PRINT '=== Done ===';
