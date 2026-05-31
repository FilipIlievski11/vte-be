/*
  backfill-pricecatalog-from-paymentitems.sql

  Replaces the "Legacy line item (Phase 1 import)" sentinel name on migrated
  PaymentDocumentLines with the real fee name. Legacy `PaymentDocumentsDetails.
  IdPriceCatalog` (values 1-2884, 961 distinct) actually point into
  `PaymentItems` (6,020 named rows), NOT into the 2-row legacy PriceCatalog.

  Idempotent + safe to re-run.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

PRINT '== backfill-pricecatalog-from-paymentitems start =='

DECLARE @defaultVatId int;
SET @defaultVatId = (SELECT TOP 1 Id FROM dbo.VatRate WHERE [Percent] = 18 ORDER BY Id);
IF @defaultVatId IS NULL SET @defaultVatId = (SELECT TOP 1 Id FROM dbo.VatRate ORDER BY Id);

-- 1. If the sentinel Id=1 collides with a real PaymentItem.Id=1, take that
--    PaymentItem's name onto the existing row (instead of inserting a duplicate).
PRINT '1. Refreshing names of PriceCatalog rows that overlap PaymentItems.Id'
UPDATE pc
SET pc.Code = NULL,
    pc.Name = LEFT(pi.ItemName, 250)
FROM dbo.PriceCatalog pc
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentItems pi ON pi.Id = pc.Id;

-- 2. Insert every legacy PaymentItem that's not already in PriceCatalog.
PRINT '2. Inserting missing PaymentItems → PriceCatalog'
SET IDENTITY_INSERT dbo.PriceCatalog ON;
INSERT INTO dbo.PriceCatalog (Id, Code, Name, BasePrice, VatRateId, [Trigger],
                              VehicleCategoryFilter, BankAccount, PaymentForm, Active)
SELECT pi.Id,
       NULL,
       LEFT(pi.ItemName, 250),
       0,                                          -- price is per-context in legacy; keep base 0
       @defaultVatId,
       CONVERT(tinyint, 0),                        -- Trigger.None — to be classified in Phase 3
       NULL, NULL, NULL,
       ISNULL(pi.Active, CONVERT(bit, 1))
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentItems pi
WHERE NOT EXISTS (SELECT 1 FROM dbo.PriceCatalog pc WHERE pc.Id = pi.Id);
SET IDENTITY_INSERT dbo.PriceCatalog OFF;

DECLARE @pcTotal int = (SELECT COUNT(*) FROM dbo.PriceCatalog);
PRINT '   PriceCatalog rows now: ' + CAST(@pcTotal AS varchar);

-- 3. Re-link PaymentDocumentLine.PriceCatalogId to the real PriceCatalog row.
--    The line.Id == legacy PaymentDocumentsDetails.Id (preserved via IDENTITY_INSERT
--    in the original migration), so we can fetch the original IdPriceCatalog directly.
--    Also reset Note to the legacy Note (drop the synthetic "legacy item #N" prefix).
PRINT '3. Re-linking PaymentDocumentLine → real PriceCatalog (this takes ~1 min)'

-- Stage the legacy ids in a local table so the UPDATE doesn't do row-by-row remote joins.
SELECT d.Id AS LineId, d.IdPriceCatalog AS NewPriceCatalogId, d.Note AS LegacyNote
  INTO #linemap
  FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsDetails d
  WHERE EXISTS (SELECT 1 FROM dbo.PaymentDocumentLine l WHERE l.Id = d.Id)
    AND EXISTS (SELECT 1 FROM dbo.PriceCatalog pc WHERE pc.Id = d.IdPriceCatalog);

CREATE UNIQUE CLUSTERED INDEX IX_linemap ON #linemap(LineId);

UPDATE l
SET l.PriceCatalogId = m.NewPriceCatalogId,
    l.Note           = LEFT(NULLIF(LTRIM(RTRIM(m.LegacyNote)), N''), 300)
FROM dbo.PaymentDocumentLine l
INNER JOIN #linemap m ON m.LineId = l.Id;

DECLARE @linked int = @@ROWCOUNT;
PRINT '   Relinked rows: ' + CAST(@linked AS varchar);

DROP TABLE #linemap;

------------------------------------------------------------------
-- Summary
------------------------------------------------------------------
PRINT ''
PRINT '== backfill complete =='

SELECT 'lines still on sentinel #1'           AS metric, COUNT(*) AS rows_
  FROM dbo.PaymentDocumentLine WHERE PriceCatalogId = 1
UNION ALL
SELECT 'lines linked to a real PaymentItem',  COUNT(*)
  FROM dbo.PaymentDocumentLine WHERE PriceCatalogId <> 1
UNION ALL
SELECT 'PriceCatalog rows total',             COUNT(*) FROM dbo.PriceCatalog;
