-- =============================================================================
-- Merge the duplicate "nina" into a single account wired to the tech-exam data.
--
-- Two records existed for the same person (Марија Гичева / Marija Gicheva):
--   • GUID account  "nina"      — the real current login (has password, email), but
--                                 NOT wired to reports (reports reference legacy id 128).
--   • recreated      "nina.128" — Id "128", wired to tech-exam reports (controller id 128),
--                                 but no password.
--
-- We keep the WIRED account (Id "128") so reports keep resolving, copy the real
-- login/credentials onto it, rename it back to "nina", and delete the GUID dup.
-- Result: one "nina" account, with its real login, resolving on every report whose
-- controller is legacy id 128.
--
-- Re-runnable: a no-op once the GUID "nina" is gone.
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\merge-nina-operator.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET XACT_ABORT ON; SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

DECLARE @keep nvarchar(450) = N'128';                                   -- the wired account
DECLARE @guid nvarchar(450) = (SELECT Id FROM dbo.AspNetUsers WHERE NormalizedUserName = N'NINA' AND Id <> @keep);

IF @guid IS NULL
    PRINT 'Nothing to merge — the duplicate GUID "nina" is already gone.';
ELSE IF NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers WHERE Id = @keep)
    RAISERROR('Kept account "128" not found — run create-tech-exam-operators.sql first.', 16, 1);
ELSE
BEGIN
    -- 1) Move the real account's role(s) onto the kept account (skip ones it already has).
    INSERT INTO dbo.AspNetUserRoles (UserId, RoleId)
    SELECT @keep, ur.RoleId FROM dbo.AspNetUserRoles ur
    WHERE ur.UserId = @guid
      AND NOT EXISTS (SELECT 1 FROM dbo.AspNetUserRoles k WHERE k.UserId = @keep AND k.RoleId = ur.RoleId);
    DELETE FROM dbo.AspNetUserRoles WHERE UserId = @guid;

    -- (claims / logins / tokens / requests / attachments referencing the GUID = 0 — nothing else to repoint)

    -- 2) Copy the real login + contact fields onto the kept (wired) account.
    UPDATE k SET
        k.PasswordHash         = g.PasswordHash,
        k.SecurityStamp        = g.SecurityStamp,
        k.ConcurrencyStamp     = CONVERT(nvarchar(450), NEWID()),
        k.Email                = g.Email,
        k.NormalizedEmail      = g.NormalizedEmail,
        k.EmailConfirmed       = g.EmailConfirmed,
        k.PhoneNumber          = g.PhoneNumber,
        k.PhoneNumberConfirmed = g.PhoneNumberConfirmed,
        k.TwoFactorEnabled     = g.TwoFactorEnabled,
        k.LockoutEnabled       = g.LockoutEnabled,
        k.AccessFailedCount    = g.AccessFailedCount,
        k.IsActive             = 1
    FROM dbo.AspNetUsers k JOIN dbo.AspNetUsers g ON g.Id = @guid
    WHERE k.Id = @keep;

    -- 3) Delete the GUID duplicate (now unreferenced).
    DELETE FROM dbo.AspNetUsers WHERE Id = @guid;

    -- 4) Rename the kept account back to "nina" (safe now that the GUID row is gone).
    UPDATE dbo.AspNetUsers SET UserName = N'nina', NormalizedUserName = N'NINA' WHERE Id = @keep;

    PRINT 'Merged: account "128" is now the single "nina", with its real login, wired to controller id 128.';
END

COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    PRINT 'ERROR ' + CAST(ERROR_NUMBER() AS nvarchar(10)) + ': ' + ERROR_MESSAGE();
    THROW;
END CATCH;
