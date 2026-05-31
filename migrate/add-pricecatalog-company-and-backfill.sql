/*
  add-pricecatalog-company-and-backfill.sql

  Adds PriceCompanyId column to dbo.PriceCatalog and backfills it from legacy
  PaymentCategories.IdCompany via the chain:
      PriceCatalog.Id == PaymentItemParametars.Id
        ─→ PaymentItems.IdPymentCategory == PaymentCategories.Id
            ─→ PaymentCategories.IdCompany

  Why: without this filter, multiple companies' "Operating fee" rules all fire
  for the same vehicle, producing duplicate CustomerDebt lines (the bug
  visible in test exam 167585 — 6× "Оперативни трошоци" at 354.00).

  Idempotent: safe to re-run.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
PRINT '== add PriceCompanyId + backfill =='

IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.PriceCatalog') AND name = 'PriceCompanyId')
BEGIN
    PRINT '1. Adding column PriceCatalog.PriceCompanyId'
    ALTER TABLE dbo.PriceCatalog ADD PriceCompanyId tinyint NULL;
END
ELSE PRINT '1. PriceCompanyId already exists — skipping ALTER'
GO

PRINT '2. Backfilling PriceCompanyId from legacy'

SELECT
    pip.Id  AS PriceCatalogId,
    CONVERT(tinyint, NULLIF(pc.IdCompany, 0)) AS PriceCompanyId
INTO #m
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentItemParametars pip
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentItems      pi ON pi.Id = pip.IdPaymentItem
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentCategories pc ON pc.Id = pi.IdPymentCategory;

CREATE UNIQUE CLUSTERED INDEX IX_m ON #m(PriceCatalogId);

UPDATE p
SET p.PriceCompanyId = m.PriceCompanyId
FROM dbo.PriceCatalog p
INNER JOIN #m m ON m.PriceCatalogId = p.Id;

DECLARE @updated int = @@ROWCOUNT;
PRINT '   Updated: ' + CAST(@updated AS varchar);

DROP TABLE #m;
GO

PRINT '3. Final distribution (NULL = applies-to-all):'
SELECT ISNULL(CAST(PriceCompanyId AS varchar), '<NULL>') AS PriceCompanyId, COUNT(*) AS rules_
FROM dbo.PriceCatalog
GROUP BY PriceCompanyId
ORDER BY PriceCompanyId;
