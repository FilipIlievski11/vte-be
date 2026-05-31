/*
  fix-pricecatalog-active-cascade.sql

  Bug: my migration set PriceCatalog.Active = PaymentItemParametars.Active alone,
  but legacy filters by the AND of all three levels:
       PaymentCategoriesActive=1 AND PaymentItemsActive=1 AND PaymentItemParametarsActive=1
  (See getPaymentCatalog SP body.)

  Result: catalog rows whose parent PaymentCategory was deactivated stayed
  Active=true in v2, so they leaked into the evaluator and produced bogus debts.
  Example: 6 "Оперативни трошоци" duplicates on Filip's exam — all 6 catalog
  parents are PaymentCategoriesActive=False in legacy.

  This script cascades the legacy active-ness down to v2 PriceCatalog.Active.
  Idempotent: safe to re-run.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
PRINT '== cascade Active from legacy hierarchy =='

UPDATE pc
SET pc.Active = CASE
    WHEN ISNULL(pip.Active, CONVERT(bit, 0)) = 1
     AND ISNULL(pi.Active,  CONVERT(bit, 0)) = 1
     AND ISNULL(pcat.Active,CONVERT(bit, 0)) = 1
    THEN CONVERT(bit, 1)
    ELSE CONVERT(bit, 0)
END
FROM dbo.PriceCatalog pc
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentItemParametars pip ON pip.Id   = pc.Id
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentItems          pi  ON pi.Id    = pip.IdPaymentItem
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentCategories     pcat ON pcat.Id = pi.IdPymentCategory
WHERE pc.Id <> 999999;  -- leave the unknown sentinel alone

DECLARE @updated int = @@ROWCOUNT;
PRINT '   Updated: ' + CAST(@updated AS varchar);

PRINT ''
PRINT '== distribution after cascade =='
SELECT 'Active = 1 (will fire)' AS metric, COUNT(*) AS rows_ FROM dbo.PriceCatalog WHERE Active = 1
UNION ALL
SELECT 'Active = 0 (will be skipped)',     COUNT(*)         FROM dbo.PriceCatalog WHERE Active = 0;
