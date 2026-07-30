-- fix-sovet-patna-seats60.sql — една грешна ќелија во „Републички совет за
-- безбедност од јавни патишта" (група 1086, нашата компанија 4).
--
-- Правилото за автобус со точно 60 седишта (PriceCatalog Id 2335) имаше 116,90 ден.
-- (иста вредност како редот за 61 седиште). Основната патна такса за 60 седишта е
-- 11.500 ден. (група 1, ред 2719), а советот е 1% од неа → 115,00 ден.
-- Потврдено 2026-07-30 со целосна спроверка 1086 vs група 1: сите останати редови
-- се точно 1%. (Групата 86 на компанија 3 има своја аномалија кај VehCat 14
-- 31-45 седишта = 57,4 наместо 67,4 — не ја допираме, не пука за компанија 4.)
--
-- Идемпотентна.

USE VTE;
GO

UPDATE dbo.PriceCatalog
SET BasePrice = 115.0
WHERE Id = 2335 AND PaymentCategoryGroupId = 1086 AND BasePrice = 116.9;

PRINT CONCAT('Поправени редови: ', @@ROWCOUNT);

SELECT Id, PaymentCategoryGroupId, VehiclePaymentCategoryId, ParametarFrom, ParametarTo, BasePrice
FROM dbo.PriceCatalog
WHERE Id = 2335;
GO
