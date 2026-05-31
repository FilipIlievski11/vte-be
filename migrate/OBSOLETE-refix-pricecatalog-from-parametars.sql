/*
  refix-pricecatalog-from-parametars.sql

  CORRECTION to the earlier two backfill scripts:

    backfill-pricecatalog-from-paymentitems.sql + fix-pricecatalog-orphan-sentinel.sql
    wrongly assumed legacy PaymentDocumentsDetails.IdPriceCatalog references
    PaymentItems.Id. It does NOT — it references PaymentItemParametars.Id
    (100% match across all 961 distinct values, vs only 87% by accidental numeric
    collision against PaymentItems.Id).

  The legacy hierarchy is:
    PaymentItems              (broad groupings: "за Патнички возила", "за Автобуси и комбибуси")
      └─ PaymentItemParametars (specific priced fees: "Црвен крст 95 ден.", "Оперативни трошоци 354 ден.")
                                = THE row PaymentDocumentsDetails.IdPriceCatalog points at

  This script wipes the bad PriceCatalog rebuilt by the prior scripts and rebuilds
  it from PaymentItemParametars JOIN PaymentItems, so:
    • Every PriceCatalog row has Id = legacy PaymentItemParametars.Id.
    • Name = PaymentItems.ItemName + " — " + PaymentItemParametars.PrametarName.
    • BasePrice = PaymentItemParametars.Price.
    • ZERO lines should remain on the unknown sentinel after this runs.

  Idempotent + safe to re-run.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
PRINT '== refix-pricecatalog-from-parametars start =='

DECLARE @defaultVatId int;
SET @defaultVatId = (SELECT TOP 1 Id FROM dbo.VatRate WHERE [Percent] = 18 ORDER BY Id);
IF @defaultVatId IS NULL SET @defaultVatId = (SELECT TOP 1 Id FROM dbo.VatRate ORDER BY Id);

-- 1. Stage the new PriceCatalog from PaymentItemParametars JOIN PaymentItems
PRINT '1. Staging new PriceCatalog rows from PaymentItemParametars'
SELECT
    pip.Id                                                                                     AS Id,
    LEFT(CONCAT(ISNULL(pi.ItemName, N'(no item)'), N' — ', pip.PrametarName), 250)             AS Name,
    CONVERT(decimal(18,4), ISNULL(pip.Price, 0))                                               AS BasePrice,
    ISNULL(pip.Active, CONVERT(bit, 1))                                                        AS Active
INTO #pc
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentItemParametars pip
LEFT JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentItems pi ON pi.Id = pip.IdPaymentItem;
CREATE UNIQUE CLUSTERED INDEX IX_pc_id ON #pc(Id);

DECLARE @stageCount int = (SELECT COUNT(*) FROM #pc);
PRINT '   Staged: ' + CAST(@stageCount AS varchar);

-- 2. Park all lines on the unknown sentinel so we can rebuild PriceCatalog without FK violations.
PRINT '2. Parking all lines on sentinel #999999 to clear the catalog'
-- Ensure sentinel exists
IF NOT EXISTS (SELECT 1 FROM dbo.PriceCatalog WHERE Id = 999999)
BEGIN
    SET IDENTITY_INSERT dbo.PriceCatalog ON;
    INSERT INTO dbo.PriceCatalog (Id, Code, Name, BasePrice, VatRateId, [Trigger],
                                  VehicleCategoryFilter, BankAccount, PaymentForm, Active)
    VALUES (999999, 'UNKNOWN', N'Непозната ставка (мигрирана)', 0, @defaultVatId, 0, NULL, NULL, NULL, 1);
    SET IDENTITY_INSERT dbo.PriceCatalog OFF;
END
UPDATE dbo.PaymentDocumentLine SET PriceCatalogId = 999999 WHERE PriceCatalogId <> 999999;

-- 3. Wipe the catalog (except sentinel) and re-insert from staging.
PRINT '3. Rebuilding PriceCatalog'
DELETE FROM dbo.PriceCatalog WHERE Id <> 999999;

SET IDENTITY_INSERT dbo.PriceCatalog ON;
INSERT INTO dbo.PriceCatalog (Id, Code, Name, BasePrice, VatRateId, [Trigger],
                              VehicleCategoryFilter, BankAccount, PaymentForm, Active)
SELECT Id, NULL, Name, BasePrice, @defaultVatId, CONVERT(tinyint, 0),
       NULL, NULL, NULL, Active
FROM #pc;
SET IDENTITY_INSERT dbo.PriceCatalog OFF;

DECLARE @newPcCount int = (SELECT COUNT(*) FROM dbo.PriceCatalog);
PRINT '   PriceCatalog rows now: ' + CAST(@newPcCount AS varchar);

-- 4. Re-link every PaymentDocumentLine to its real PriceCatalog row.
PRINT '4. Re-linking PaymentDocumentLines (this takes ~1 min)'
SELECT d.Id AS LineId, d.IdPriceCatalog AS RealPcId, d.Note AS LegacyNote
  INTO #m
  FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsDetails d
  WHERE EXISTS (SELECT 1 FROM dbo.PaymentDocumentLine l WHERE l.Id = d.Id);
CREATE UNIQUE CLUSTERED INDEX IX_m ON #m(LineId);

UPDATE l
SET l.PriceCatalogId =
        CASE WHEN EXISTS (SELECT 1 FROM dbo.PriceCatalog pc WHERE pc.Id = m.RealPcId)
             THEN m.RealPcId
             ELSE 999999
        END,
    l.Note = LEFT(NULLIF(LTRIM(RTRIM(m.LegacyNote)), N''), 300)
FROM dbo.PaymentDocumentLine l
INNER JOIN #m m ON m.LineId = l.Id;

DECLARE @relinked int = @@ROWCOUNT;
PRINT '   Lines updated: ' + CAST(@relinked AS varchar);

DROP TABLE #m;
DROP TABLE #pc;

PRINT ''
PRINT '== refix complete =='
SELECT 'lines on unknown sentinel #999999' AS metric, COUNT(*) AS rows_
  FROM dbo.PaymentDocumentLine WHERE PriceCatalogId = 999999
UNION ALL
SELECT 'lines linked to real Parametars',   COUNT(*)
  FROM dbo.PaymentDocumentLine WHERE PriceCatalogId <> 999999
UNION ALL
SELECT 'PriceCatalog rows total',           COUNT(*) FROM dbo.PriceCatalog;
