-- fix-techexam-trigger1-twins.sql — редовните технички прегледи не ја наплаќаа
-- ГЛАВНАТА услуга „Технички преглед" (пр. 1603 „за Патнички возила M1" = 1700 ден).
--
-- Причина: во легаси категориите „Технички преглед" (групи 11/53/1011) носат ДВА
-- флага — TrigerdByTechnicalExam И TrigerdByIrregularTechnicalExam — истите ставки
-- се наплаќаат и за редовен и за нерегуларен преглед. v2 PriceCatalog има еден
-- Trigger по ред, а миграцијата ги класирала овие правила САМО како Trigger=6
-- (нерегуларен). Редовен преглед (Trigger=1) → главната услуга никогаш не се
-- појавуваше во долговите (фатено на прво в2-барање со авто-преглед, 2026-08-11;
-- засегнат само exam 10000004 — неплатен, никакви реални сметки).
--
-- Поправка: Trigger=1 БЛИЗНАК за секое АКТИВНО Trigger=6 правило во групите
-- 11/53/1011 (сите компании — легаси паритет). Идемпотентна: прескокнува ако
-- близнакот веќе постои. Id доделува identity (v2-бенд 1000000+).
--
-- Заб.: гоча #8 останува безбедна — неактивните sentinel-редови (1330/2510,
-- From=0/To=0/VehicleField set) се исклучени преку Active=1 филтерот.

USE VTE;
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

INSERT INTO dbo.PriceCatalog
    (Code, Name, BasePrice, VatRateId, [Trigger], VehicleCategoryFilter, BankAccount,
     PaymentForm, Active, CommunityId, ParametarFrom, ParametarTo, PaymentCategoryGroupId,
     VehicleField, VehiclePaymentCategoryId, PriceCompanyId, AgeFrom, AgeTo)
SELECT
    p.Code, p.Name, p.BasePrice, p.VatRateId, 1, p.VehicleCategoryFilter, p.BankAccount,
    p.PaymentForm, 1, p.CommunityId, p.ParametarFrom, p.ParametarTo, p.PaymentCategoryGroupId,
    p.VehicleField, p.VehiclePaymentCategoryId, p.PriceCompanyId, p.AgeFrom, p.AgeTo
FROM dbo.PriceCatalog p
WHERE p.PaymentCategoryGroupId IN (11, 53, 1011)
  AND p.[Trigger] = 6
  AND p.Active = 1
  AND NOT EXISTS (
      SELECT 1 FROM dbo.PriceCatalog t
      WHERE t.[Trigger] = 1
        AND t.Active = 1
        AND t.PaymentCategoryGroupId = p.PaymentCategoryGroupId
        AND t.Name = p.Name
        AND t.BasePrice = p.BasePrice
        AND ISNULL(t.VehiclePaymentCategoryId, -1) = ISNULL(p.VehiclePaymentCategoryId, -1)
        AND ISNULL(CAST(t.PriceCompanyId AS int), -1) = ISNULL(CAST(p.PriceCompanyId AS int), -1)
        AND ISNULL(t.VehicleField, N'')            = ISNULL(p.VehicleField, N'')
        AND ISNULL(t.ParametarFrom, -1)            = ISNULL(p.ParametarFrom, -1)
        AND ISNULL(t.ParametarTo, -1)              = ISNULL(p.ParametarTo, -1)
        AND ISNULL(t.CommunityId, -1)              = ISNULL(p.CommunityId, -1)
        AND ISNULL(t.AgeFrom, -1)                  = ISNULL(p.AgeFrom, -1)
        AND ISNULL(t.AgeTo, -1)                    = ISNULL(p.AgeTo, -1));

DECLARE @inserted int = @@ROWCOUNT;
PRINT CONCAT('Novi Trigger=1 bliznaci: ', @inserted);

COMMIT TRANSACTION;

-- Контрола: групите „Технички преглед" сега со двата тригери.
SELECT [Trigger], 'broj'=COUNT(*) FROM dbo.PriceCatalog
WHERE PaymentCategoryGroupId IN (11, 53, 1011) AND Active = 1
GROUP BY [Trigger];
GO
