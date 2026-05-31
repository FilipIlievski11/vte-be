/*
  fix-pricecatalog-orphan-sentinel.sql

  After backfill-pricecatalog-from-paymentitems.sql, the sentinel PriceCatalog
  row (Id=1) was overwritten with PaymentItem #1's name ("за Возила ослободени
  од давачки"). This caused ~178k legacy lines whose IdPriceCatalog points at
  a NON-EXISTENT PaymentItem (true orphans, 121 distinct ids) to wrongly inherit
  PaymentItem #1's name.

  This script splits them out:
    • Lines whose legacy IdPriceCatalog = 1 stay on PriceCatalog #1 (correct
      — those are genuine PaymentItem #1 sales).
    • Lines whose legacy IdPriceCatalog ≠ 1 (and didn't match a PaymentItem)
      move to a high-Id sentinel = 'Непозната ставка (мигрирана)'.
    • Note column gets the legacy d.Note (or "(legacy ref #N)" forensic string
      if legacy note was empty).

  Idempotent + safe to re-run.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
PRINT '== fix-pricecatalog-orphan-sentinel start =='

DECLARE @sentinelId int = 999999;
DECLARE @defaultVatId int;
SET @defaultVatId = (SELECT TOP 1 Id FROM dbo.VatRate WHERE [Percent] = 18 ORDER BY Id);
IF @defaultVatId IS NULL SET @defaultVatId = (SELECT TOP 1 Id FROM dbo.VatRate ORDER BY Id);

-- 1. Ensure the unknown-fee sentinel exists.
IF NOT EXISTS (SELECT 1 FROM dbo.PriceCatalog WHERE Id = @sentinelId)
BEGIN
    SET IDENTITY_INSERT dbo.PriceCatalog ON;
    INSERT INTO dbo.PriceCatalog (Id, Code, Name, BasePrice, VatRateId, [Trigger],
                                  VehicleCategoryFilter, BankAccount, PaymentForm, Active)
    VALUES (@sentinelId, 'UNKNOWN', N'Непозната ставка (мигрирана)', 0, @defaultVatId, 0, NULL, NULL, NULL, 1);
    SET IDENTITY_INSERT dbo.PriceCatalog OFF;
    PRINT '   Inserted unknown sentinel #' + CAST(@sentinelId AS varchar);
END

-- 2. Stage all lines currently on PriceCatalogId=1 whose legacy IdPriceCatalog
--    was actually NOT 1 — they're masquerading.
SELECT l.Id AS LineId, d.IdPriceCatalog AS LegacyPcId, d.Note AS LegacyNote
  INTO #orphans
  FROM dbo.PaymentDocumentLine l
  INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsDetails d ON d.Id = l.Id
  WHERE l.PriceCatalogId = 1
    AND d.IdPriceCatalog <> 1;

CREATE UNIQUE CLUSTERED INDEX IX_orphans ON #orphans(LineId);

PRINT '2. Re-pointing orphan lines to the unknown sentinel'
UPDATE l
SET l.PriceCatalogId = @sentinelId,
    l.Note = LEFT(
        ISNULL(NULLIF(LTRIM(RTRIM(o.LegacyNote)), N''),
               N'(legacy ref #' + CAST(o.LegacyPcId AS nvarchar(20)) + N')'),
        300)
FROM dbo.PaymentDocumentLine l
INNER JOIN #orphans o ON o.LineId = l.Id;

DECLARE @moved int = @@ROWCOUNT;
PRINT '   Orphan lines moved: ' + CAST(@moved AS varchar);

DROP TABLE #orphans;

PRINT ''
PRINT '== fix complete =='
SELECT 'lines on PriceCatalog #1 (genuine PaymentItem #1)' AS metric, COUNT(*) AS rows_
  FROM dbo.PaymentDocumentLine WHERE PriceCatalogId = 1
UNION ALL
SELECT 'lines on unknown sentinel #999999', COUNT(*)
  FROM dbo.PaymentDocumentLine WHERE PriceCatalogId = 999999
UNION ALL
SELECT 'lines linked to real PaymentItems', COUNT(*)
  FROM dbo.PaymentDocumentLine WHERE PriceCatalogId NOT IN (1, 999999);
