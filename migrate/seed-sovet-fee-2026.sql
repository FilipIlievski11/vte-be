-- ============================================================================
-- seed-sovet-fee-2026.sql (2026-07-16)
--
-- „Републички совет за безбедност" = 1,5% од надоместокот за технички преглед
-- (Закон за БСП, чл. 374 ст. 1 ал. 1), пресметано врз НОВАТА ТП тарифа
-- (Правилник бр. 13.1.1-63380/1 од 31.03.2026), заокружено на цел денар:
--
--   ТП 1.200/1.220 (L1e, L2e, O1, трактори T*, приколки R*, S*)   → 18
--   ТП 1.460 (L3e–L7e)                                            → 22
--   ТП 1.700 (M1, M1G, N1, N1G, O2)                               → 26
--   ТП 2.300 (O3, O4)                                             → 35
--   ТП 2.900 (M2, M3, N2, N3)                                     → 44
--
-- Правилата се по регистрациска категорија (VehicleCategoryFilter, CSV со
-- кирилични двојници: L1е/L3е/Т1/Т43 постојат во податоците). Старото фиксно
-- правило (968: 20,50 ден., VehCat=Неодредено — кое и онака не палеше за
-- обични возила) се деактивира. Идемпотентно: Code='SOV26' → DELETE+INSERT.
-- ============================================================================

USE VTE;
GO
SET NOCOUNT ON;

UPDATE pc SET pc.Active = 0
FROM dbo.PriceCatalog pc
WHERE pc.PaymentCategoryGroupId IN (27, 63, 1063)
  AND pc.[Trigger] = 1
  AND pc.Active = 1
  AND ISNULL(pc.Code, '') <> 'SOV26';
PRINT CONCAT('Стари основен-совет правила деактивирани: ', @@ROWCOUNT);

DELETE FROM dbo.PriceCatalog WHERE Code = 'SOV26';
PRINT CONCAT('Постоечки SOV26 правила избришани (reseed): ', @@ROWCOUNT);

DECLARE @vatId int;
SELECT TOP 1 @vatId = Id FROM dbo.VatRate WHERE [Percent] = 0 ORDER BY Id;

INSERT INTO dbo.PriceCatalog
    (Code, Name, BasePrice, VatRateId, [Trigger], VehiclePaymentCategoryId, CommunityId,
     PriceCompanyId, PaymentCategoryGroupId, VehicleField, ParametarFrom, ParametarTo,
     AgeFrom, AgeTo, VehicleCategoryFilter, BankAccount, PaymentForm, Active)
SELECT 'SOV26', v.Name, v.Price, @vatId, 1, NULL, NULL,
       NULL, 1063, NULL, NULL, NULL, NULL, NULL, v.Filter, NULL, NULL, 1
FROM (VALUES
  (N'1,5% од технички преглед · мопеди, О1, трактори и приколки', 18,
   N'L1e,L1е,L2,L2e,O1,T1,Т1,T2,T3,T4,T4.1,T4.3,Т43,T5,R1,R2,R3,RA2,RA3,S1'),
  (N'1,5% од технички преглед · мотоцикли L3–L7', 22,
   N'L3e,L3е,L4e,L5e,L6e,L7e,L7е'),
  (N'1,5% од технички преглед · M1, N1, O2', 26,
   N'M1,M1G,N1,N1G,O2'),
  (N'1,5% од технички преглед · O3, O4', 35,
   N'O3,O4'),
  (N'1,5% од технички преглед · M2, M3, N2, N3', 44,
   N'M2,M3,N2,N3')
) v(Name, Price, Filter);
PRINT CONCAT('Нови SOV26 правила внесени: ', @@ROWCOUNT);

SELECT Id, LEFT(Name, 55) AS Name, BasePrice, LEFT(VehicleCategoryFilter, 70) AS Filter
FROM dbo.PriceCatalog WHERE Code = 'SOV26';
