-- =============================================================================
-- Recreate the technical-exam CONTROLLERS as v2 operator users.
--
-- Legacy DocumentsTehnicalExamsReports.IdFirsControler / IdSecondControler point
-- at VTESecurity.dbo.Users (the legacy security DB). v2 had no operator directory,
-- so the migrated TechnicalExamReport keeps those as FirstControllerLegacyId /
-- SecondControllerLegacyId (ints). This script recreates each referenced user as a
-- v2 ASP.NET Identity user with the Operator role, keyed so reports wire directly:
--
--     AspNetUsers.Id = CAST(VTESecurity.Users.ID AS nvarchar)   (e.g. "122")
--
-- so report.FirstControllerLegacyId.ToString() == AspNetUsers.Id. The API then
-- resolves the controller's name from AspNetUsers.FullName.
--
-- UserName = "{legacyUserName}.{id}" to guarantee uniqueness (the source has a
-- duplicate "ААА" and one name "nina" collides with an existing v2 user).
-- PasswordHash is NULL (legacy hashes can't migrate) → no login until an admin
-- sets a password; IsActive mirrors the legacy Active flag.
--
-- Re-runnable (skips users that already exist by Id). Reads VTESecurity over the
-- VTEZVV_LIVE linked server.
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\create-tech-exam-operators.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET NOCOUNT ON;

DECLARE @operatorRoleId nvarchar(450) = (SELECT Id FROM dbo.AspNetRoles WHERE NormalizedName = N'OPERATOR');
IF @operatorRoleId IS NULL
BEGIN RAISERROR('Operator role not found — boot the API once so roles are seeded.', 16, 1); RETURN; END;

-- 1) Distinct controllers referenced in the migrated tech-exam reports.
IF OBJECT_ID('tempdb..#ctrl') IS NOT NULL DROP TABLE #ctrl;
SELECT FirstControllerLegacyId AS Id INTO #ctrl FROM dbo.TechnicalExamReport WHERE FirstControllerLegacyId IS NOT NULL
UNION SELECT SecondControllerLegacyId FROM dbo.TechnicalExamReport WHERE SecondControllerLegacyId IS NOT NULL;

-- 2) Their VTESecurity records (over the linked server).
IF OBJECT_ID('tempdb..#vu') IS NOT NULL DROP TABLE #vu;
SELECT * INTO #vu FROM OPENQUERY(VTEZVV_LIVE,
  'SELECT u.ID, u.UserName, u.UserFullName, u.FirstName, u.SureName, u.Active FROM VTESecurity.dbo.Users u');

IF OBJECT_ID('tempdb..#ops') IS NOT NULL DROP TABLE #ops;
SELECT
  CAST(v.ID AS nvarchar(450)) AS UserId,
  LEFT(LTRIM(RTRIM(v.UserName)) + N'.' + CAST(v.ID AS nvarchar(10)), 256) AS UserName,
  COALESCE(NULLIF(LTRIM(RTRIM(v.UserFullName)), N''),
           NULLIF(LTRIM(RTRIM(CONCAT_WS(N' ', v.FirstName, v.SureName))), N''),
           N'Оператор #' + CAST(v.ID AS nvarchar(10))) AS FullName,
  CAST(ISNULL(v.Active, 0) AS bit) AS IsActive
INTO #ops
FROM #ctrl c JOIN #vu v ON v.ID = c.Id;

-- 3) Create the users (skip any that already exist by Id).
INSERT INTO dbo.AspNetUsers
  (Id, CompanyId, FullName, IsActive, CreatedAt, UserName, NormalizedUserName,
   Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
   PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
SELECT o.UserId, CAST(4 AS tinyint), o.FullName, o.IsActive, GETUTCDATE(),
       o.UserName, UPPER(o.UserName),
       NULL, NULL, 0, NULL, CONVERT(nvarchar(450), NEWID()), CONVERT(nvarchar(450), NEWID()),
       NULL, 0, 0, NULL, 1, 0
FROM #ops o
WHERE NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers x WHERE x.Id = o.UserId)
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers x WHERE x.NormalizedUserName = UPPER(o.UserName));
PRINT CONCAT('  -> ', @@ROWCOUNT, ' operator users created.');

-- 4) Assign the Operator role.
INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
SELECT o.UserId, @operatorRoleId
FROM #ops o
WHERE EXISTS (SELECT 1 FROM dbo.AspNetUsers x WHERE x.Id = o.UserId)
  AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUserRoles ur WHERE ur.UserId = o.UserId AND ur.RoleId = @operatorRoleId);
PRINT CONCAT('  -> ', @@ROWCOUNT, ' Operator role assignments.');

-- 5) Report wiring coverage.
PRINT '=== Wiring coverage ===';
SELECT
  (SELECT COUNT(*) FROM #ctrl) AS DistinctControllers,
  (SELECT COUNT(*) FROM #ctrl c WHERE EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = CAST(c.Id AS nvarchar(450)))) AS Wired,
  (SELECT COUNT(*) FROM dbo.TechnicalExamReport r WHERE r.FirstControllerLegacyId IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.Id = CAST(r.FirstControllerLegacyId AS nvarchar(450)))) AS ReportsFirstCtrlUnresolved;
PRINT '=== Done ===';
