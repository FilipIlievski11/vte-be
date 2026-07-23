-- ============================================================================
-- seed-eco-fee-rules-2022.sql (2026-07-16)
--
-- „Надоместок за животна средина" по Службен весник бр. 89 од 11.04.2022:
-- категорија × СТАРОСТ на возилото × ЗАФАТНИНА на моторот. Дотогашните правила
-- (по сила/носивост, стара тарифа 70–710 ден.) се деактивираат за опфатените
-- категории; операторите оттогаш ја куцаа тарифата рачно — ова ја автоматизира.
--
-- Опфат: M1 → Патнички (VehCat 2), L → Мотоцикли (3), N → Товарни (4) и
-- Комбинирани (10). НЕ се допираат: автобуси/комбибуси (5 — M2/M3 не се
-- разликуваат во податоците), влечни/специјални/работни (стари фиксни правила
-- остануваат), олдтајмери (нема флаг во v2 — рачно).
--
-- Старост = тековна година − година на производство (ManufactureDate).
-- Возило без датум/зафатнина → ниедно правило не пали → операторот внесува рачно.
--
-- Идемпотентно: новите правила носат Code='ECO22' — DELETE+INSERT по тој маркер.
-- ============================================================================

USE VTE;
GO
SET NOCOUNT ON;

-- 1) Деактивирај ги старите правила за животна средина на опфатените категории.
UPDATE pc SET pc.Active = 0
FROM dbo.PriceCatalog pc
JOIN dbo.PaymentCategoryGroup g ON g.Id = pc.PaymentCategoryGroupId
WHERE g.Name LIKE N'%животна средина%'
  AND pc.VehiclePaymentCategoryId IN (2, 3, 4, 10)
  AND pc.Active = 1
  AND ISNULL(pc.Code, '') <> 'ECO22';
PRINT CONCAT('Стари еко-правила деактивирани: ', @@ROWCOUNT);

-- 2) Обнови ги новите (idempotent преку маркерот).
DELETE FROM dbo.PriceCatalog WHERE Code = 'ECO22';
PRINT CONCAT('Постоечки ECO22 правила избришани (reseed): ', @@ROWCOUNT);

-- Група + ДДВ (0%) + компаниски опсег: од старото патничко правило (VehCat 2).
DECLARE @groupId int, @vatId int, @companyId tinyint;
SELECT TOP 1 @groupId = pc.PaymentCategoryGroupId, @vatId = v.Id, @companyId = pc.PriceCompanyId
FROM dbo.PriceCatalog pc
JOIN dbo.PaymentCategoryGroup g ON g.Id = pc.PaymentCategoryGroupId
CROSS APPLY (SELECT TOP 1 Id FROM dbo.VatRate WHERE [Percent] = 0 ORDER BY Id) v(Id)
WHERE g.Name LIKE N'%животна средина%' AND pc.VehiclePaymentCategoryId = 2
ORDER BY pc.Id;
IF @groupId IS NULL BEGIN RAISERROR('Нема стара еко-група за референца!', 16, 1); RETURN; END
PRINT CONCAT('Група: ', @groupId, ' · ДДВ(0%): ', @vatId, ' · Компанија: ', ISNULL(CAST(@companyId AS varchar), 'NULL'));

-- 3) Тарифни редови: (возр.од, возр.до, cc-од, cc-до, цена) по категорија.
--    cc-до = NULL значи „над"; возр.до = NULL значи „над 30".
DECLARE @rows TABLE (VehCat int, AgeFrom int, AgeTo int NULL, CcFrom float, CcTo float NULL, Price money, Label nvarchar(120));

-- M1 Патнички (VehCat 2)
INSERT INTO @rows VALUES
 (2, 0,5,    0, 750, 150, N''),(2, 0,5,  751,1400, 250, N''),(2, 0,5, 1401,2000, 300, N''),(2, 0,5, 2001,NULL, 400, N''),
 (2, 6,10,   0, 750, 250, N''),(2, 6,10, 751,1400, 350, N''),(2, 6,10,1401,2000, 400, N''),(2, 6,10,2001,NULL, 500, N''),
 (2,11,20,   0, 750, 350, N''),(2,11,20, 751,1400, 450, N''),(2,11,20,1401,2000, 500, N''),(2,11,20,2001,NULL, 600, N''),
 (2,21,30,   0, 750, 450, N''),(2,21,30, 751,1400, 550, N''),(2,21,30,1401,2000, 600, N''),(2,21,30,2001,NULL, 700, N''),
 (2,31,NULL, 0, 750, 450, N''),(2,31,NULL,751,1400,550, N''),(2,31,NULL,1401,2000,600, N''),(2,31,NULL,2001,NULL,700, N'');

-- L Мотоцикли (VehCat 3)
INSERT INTO @rows VALUES
 (3, 0,5,    0,  50, 100, N''),(3, 0,5,   51, 100, 150, N''),(3, 0,5,  101, 175, 200, N''),(3, 0,5,  176, 250, 300, N''),(3, 0,5,  251, 500, 400, N''),(3, 0,5,  501, 750, 500, N''),(3, 0,5,  751,NULL, 600, N''),
 (3, 6,10,   0,  50, 200, N''),(3, 6,10,  51, 100, 250, N''),(3, 6,10, 101, 175, 300, N''),(3, 6,10, 176, 250, 400, N''),(3, 6,10, 251, 500, 500, N''),(3, 6,10, 501, 750, 600, N''),(3, 6,10, 751,NULL, 700, N''),
 (3,11,20,   0,  50, 300, N''),(3,11,20,  51, 100, 350, N''),(3,11,20, 101, 175, 400, N''),(3,11,20, 176, 250, 500, N''),(3,11,20, 251, 500, 600, N''),(3,11,20, 501, 750, 700, N''),(3,11,20, 751,NULL, 800, N''),
 (3,21,30,   0,  50, 400, N''),(3,21,30,  51, 100, 450, N''),(3,21,30, 101, 175, 500, N''),(3,21,30, 176, 250, 600, N''),(3,21,30, 251, 500, 700, N''),(3,21,30, 501, 750, 800, N''),(3,21,30, 751,NULL, 900, N''),
 (3,31,NULL, 0,  50, 500, N''),(3,31,NULL, 51, 100, 550, N''),(3,31,NULL,101, 175, 600, N''),(3,31,NULL,176, 250, 700, N''),(3,31,NULL,251, 500, 800, N''),(3,31,NULL,501, 750, 900, N''),(3,31,NULL,751,NULL,1000, N'');

-- N Товарни (VehCat 4) + истата тарифа за Комбинирани (VehCat 10)
INSERT INTO @rows
SELECT v.VehCat, x.AgeFrom, x.AgeTo, x.CcFrom, x.CcTo, x.Price, N''
FROM (VALUES
 (0,5,      0, 3000, 1050),(0,5,   3001, 6000, 1150),(0,5,   6001, 9000, 1200),(0,5,   9001,12000, 1300),(0,5,  12001,16000, 1400),(0,5,  16001,NULL, 1600),
 (6,10,     0, 3000, 1150),(6,10,  3001, 6000, 1250),(6,10,  6001, 9000, 1300),(6,10,  9001,12000, 1400),(6,10, 12001,16000, 1500),(6,10, 16001,NULL, 1700),
 (11,20,    0, 3000, 1250),(11,20, 3001, 6000, 1350),(11,20, 6001, 9000, 1400),(11,20, 9001,12000, 1500),(11,20,12001,16000, 1600),(11,20,16001,NULL, 1800),
 (21,30,    0, 3000, 1350),(21,30, 3001, 6000, 1450),(21,30, 6001, 9000, 1500),(21,30, 9001,12000, 1600),(21,30,12001,16000, 1700),(21,30,16001,NULL, 1900),
 (31,NULL,  0, 3000, 1450),(31,NULL,3001, 6000, 1550),(31,NULL,6001, 9000, 1600),(31,NULL,9001,12000, 1700),(31,NULL,12001,16000, 1800),(31,NULL,16001,NULL, 2000)
) x(AgeFrom, AgeTo, CcFrom, CcTo, Price)
CROSS JOIN (VALUES (4), (10)) v(VehCat);

-- Читлива етикета: „од X до Y год. · зафатнина A–B см3"
UPDATE @rows SET Label = CONCAT(
    CASE WHEN AgeTo IS NULL THEN N'над 30 год.' ELSE CONCAT(N'од ', AgeFrom, N' до ', AgeTo, N' год.') END,
    N' · зафатнина ',
    CASE WHEN CcTo IS NULL THEN CONCAT(N'над ', CAST(CcFrom AS int), N' см3')
         ELSE CONCAT(CAST(CcFrom AS int), N'–', CAST(CcTo AS int), N' см3') END);

-- 4) Внеси како PriceCatalog правила (Trigger 1 = технички преглед, како старите).
INSERT INTO dbo.PriceCatalog
    (Code, Name, BasePrice, VatRateId, [Trigger], VehiclePaymentCategoryId, CommunityId,
     PriceCompanyId, PaymentCategoryGroupId, VehicleField, ParametarFrom, ParametarTo,
     AgeFrom, AgeTo, VehicleCategoryFilter, BankAccount, PaymentForm, Active)
SELECT
    'ECO22',
    CONCAT(CASE r.VehCat WHEN 2 THEN N'за Патнички возила' WHEN 3 THEN N'за Мотоцикли'
                         WHEN 4 THEN N'за Товарни возила' ELSE N'за Комбинирани возила' END,
           N' — ', r.Label),
    r.Price, @vatId, 1, r.VehCat, NULL,
    @companyId, @groupId, N'EngineWorkingCapacityCc', r.CcFrom, r.CcTo,
    r.AgeFrom, r.AgeTo, NULL, NULL, NULL, 1
FROM @rows r;
PRINT CONCAT('Нови ECO22 правила внесени: ', @@ROWCOUNT);

-- 5) Преглед
SELECT VehiclePaymentCategoryId AS VehCat, COUNT(*) AS Rules, MIN(BasePrice) AS MinP, MAX(BasePrice) AS MaxP
FROM dbo.PriceCatalog WHERE Code = 'ECO22' GROUP BY VehiclePaymentCategoryId ORDER BY VehCat;
