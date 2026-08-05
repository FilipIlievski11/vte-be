-- fix-idl-permission-category-groups.sql — фискалната прескокнуваше МВД/полномошно.
--
-- Двете v2-seeded правила (создадени за долг-куките на МВД и полномошно) немаа
-- PaymentCategoryGroupId. Фискалната композиција (легаси паритет со
-- GetPaymentDocumentForFiscalPrintByIdDocument) тивко ги испушта редовите без
-- категорија → првата вистинска МВД сметка (01-37-48125/2026, 2026-08-05) заврши
-- со „Сметката нема износ за фискализација".
--
-- Мапирање по легаси (блок 1000+ = компанија 4, како 1086/1003):
--   1000000 „Издавање на меѓународна возачка дозвола"  → група 1004 „Меѓународна возачка дозвола"
--   1000001 „Одобрение за туѓо возило"                  → група 1003 „Одобрение за туѓо возило"
-- Со ова и СМЕТКОПОТВРДА/Договор ги печатат правилните категориски имиња.
-- Идемпотентна.

USE VTE;
GO

UPDATE dbo.PriceCatalog SET PaymentCategoryGroupId = 1004
WHERE Id = 1000000 AND PaymentCategoryGroupId IS NULL;
PRINT CONCAT('MVD pravilo: ', @@ROWCOUNT);

UPDATE dbo.PriceCatalog SET PaymentCategoryGroupId = 1003
WHERE Id = 1000001 AND PaymentCategoryGroupId IS NULL;
PRINT CONCAT('Polnomosno pravilo: ', @@ROWCOUNT);

-- Контрола: не смее да остане активно правило со тригер без валидна група.
SELECT PreostanatiBezGrupa = COUNT(*)
FROM dbo.PriceCatalog pc
WHERE pc.Active = 1 AND pc.[Trigger] > 0
  AND (pc.PaymentCategoryGroupId IS NULL
       OR NOT EXISTS (SELECT 1 FROM dbo.PaymentCategoryGroup g WHERE g.Id = pc.PaymentCategoryGroupId));
GO
