-- ============================================================================
-- fix-pricecatalog-vatrate-from-legacy.sql (2026-07-15)
--
-- PriceCatalog.VatRateId was imported with a blanket 18% default
-- (migrate-payments.sql @defaultVatId), but legacy derives each bill line's DDV
-- from the fee's CATEGORY: PaymentCategories.IdDDV -> DDVCatalog. Verified on
-- 12 months of live cash lines: dd.DDV == category DDV on 100% of rows
-- (58,913 @ 0% + 30,587 @ 18%, zero cross-terms).
--
-- Without this fix the first v2-native bill would stamp VatPercent=18 on lines
-- legacy bills at 0% -> the FISCAL TAX CLASS on the receipt flips from В (0%)
-- to А (18%) for ~2/3 of all cash lines. Bill creation reads the catalog at
-- billing time (PaymentDocumentsController.CreateFromDebts -> vatByPc), so
-- fixing the catalog fixes all future bills, including already-open debts.
--
-- Mapping luck: v2 VatRate ids are 1:1 with legacy DDVCatalog
-- (1 -> 0%, 2 -> 5%, 3 -> 18%), and PriceCatalog.PaymentCategoryGroupId ==
-- legacy PaymentCategories.Id. Unknown/zero IdDDV falls back to VatRate 1 (0%).
-- Rows with PaymentCategoryGroupId IS NULL keep their value (dead/sentinel
-- rows: no category chain -> never composed into a fiscal receipt anyway).
--
-- Idempotent: deterministic UPDATE — re-running rewrites the same values.
-- ============================================================================

USE VTE;
GO

DECLARE @src TABLE (CatId int PRIMARY KEY, IdDDV int NOT NULL);
INSERT INTO @src (CatId, IdDDV)
SELECT x.Id, x.IdDDV
FROM OPENQUERY(VTEZVV_LIVE, 'SELECT Id, IdDDV FROM VTEZVV.dbo.PaymentCategories') x;

DECLARE @n int;
UPDATE pc
SET pc.VatRateId = CASE WHEN s.IdDDV IN (1, 2, 3) THEN s.IdDDV ELSE 1 END
FROM dbo.PriceCatalog pc
JOIN @src s ON s.CatId = pc.PaymentCategoryGroupId
WHERE pc.VatRateId <> CASE WHEN s.IdDDV IN (1, 2, 3) THEN s.IdDDV ELSE 1 END;
SET @n = @@ROWCOUNT;
PRINT CONCAT('PriceCatalog rows re-pointed to the correct VatRate: ', @n);

-- Post-state overview
SELECT v.[Percent] AS VatPercent, COUNT(*) AS CatalogRows
FROM dbo.PriceCatalog pc
JOIN dbo.VatRate v ON v.Id = pc.VatRateId
GROUP BY v.[Percent]
ORDER BY v.[Percent];
