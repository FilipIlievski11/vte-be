-- ============================================================================
-- One-time import (2026-07-06): уплатни сметки + категории на наплата.
--
--   CalculationItem       ← VTEZVV.dbo.CalculationItems   (13 rows, ids 1:1)
--   PaymentCategoryGroup  ← VTEZVV.dbo.PaymentCategories   (~317 rows, ids 1:1 —
--                            these are the ids PriceCatalog.PaymentCategoryGroupId
--                            already carries)
--
-- v2 is the MASTER after this import: the admin edits names/accounts/links in the
-- new „Категории на наплата" screen, so the legacy sync must NEVER overwrite these.
-- Idempotent: NOT EXISTS by Id — re-running only fills holes, never updates.
-- ============================================================================
SET NOCOUNT ON;
SET XACT_ABORT ON;
DECLARE @rows int;

PRINT '=== CalculationItem ===';
SELECT c.Id, c.ItemName, c.BankAccount, c.Bank, c.Form, c.Active
INTO #ci
FROM VTEZVV_LIVE.VTEZVV.dbo.CalculationItems c;

SET IDENTITY_INSERT dbo.CalculationItem ON;
INSERT INTO dbo.CalculationItem (Id, Name, BankAccount, Bank, Form, Active)
SELECT s.Id,
       LEFT(LTRIM(RTRIM(s.ItemName)), 300),
       NULLIF(LTRIM(RTRIM(s.BankAccount)), N''),
       NULLIF(LTRIM(RTRIM(s.Bank)), N''),
       NULLIF(LTRIM(RTRIM(s.Form)), N''),
       ISNULL(s.Active, 1)
FROM #ci s
WHERE NOT EXISTS (SELECT 1 FROM dbo.CalculationItem x WHERE x.Id = s.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.CalculationItem OFF;
PRINT CONCAT('  -> ', @rows, ' уплатни сметки imported.');
DROP TABLE #ci;

PRINT '=== PaymentCategoryGroup ===';
SELECT p.Id, p.CategoryName, p.IdCalculationItem, p.IdCompany, p.VisibleOrder, p.Active
INTO #pc
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentCategories p;

SET IDENTITY_INSERT dbo.PaymentCategoryGroup ON;
INSERT INTO dbo.PaymentCategoryGroup (Id, Name, CalculationItemId, CompanyScope, VisibleOrder, Active)
SELECT s.Id,
       LEFT(LTRIM(RTRIM(s.CategoryName)), 300),
       CASE WHEN EXISTS (SELECT 1 FROM dbo.CalculationItem c WHERE c.Id = s.IdCalculationItem)
            THEN s.IdCalculationItem END,
       ISNULL(s.IdCompany, 0),
       s.VisibleOrder,
       ISNULL(s.Active, 1)
FROM #pc s
WHERE NOT EXISTS (SELECT 1 FROM dbo.PaymentCategoryGroup x WHERE x.Id = s.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.PaymentCategoryGroup OFF;
PRINT CONCAT('  -> ', @rows, ' категории imported.');
DROP TABLE #pc;

-- Own-account flag: the уплатна сметка(s) named after one of our Companies is the
-- station's OWN account — money there stays with the station (see the daily
-- distribution report). Idempotent: recompute every run.
UPDATE dbo.CalculationItem SET IsOwnAccount = 0;
UPDATE ci SET ci.IsOwnAccount = 1
FROM dbo.CalculationItem ci
WHERE EXISTS (SELECT 1 FROM dbo.Company c WHERE LTRIM(RTRIM(c.Name)) = LTRIM(RTRIM(ci.Name)));
PRINT CONCAT('  -> ', @@ROWCOUNT, ' own-account(s) flagged.');

SELECT 'CalculationItem' t, COUNT(*) c FROM dbo.CalculationItem
UNION ALL SELECT 'PaymentCategoryGroup', COUNT(*) FROM dbo.PaymentCategoryGroup
UNION ALL SELECT 'OwnAccounts', COUNT(*) FROM dbo.CalculationItem WHERE IsOwnAccount = 1;
