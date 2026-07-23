-- ============================================================================
-- seed-eco-fee-rules-2022-bus.sql (2026-07-16)
--
-- Продолжение на seed-eco-fee-rules-2022.sql: „Надоместок за животна средина"
-- за М2 (минибуси) и М3 (автобуси) по Службен весник бр. 89/11.04.2022.
-- Двете делат payment-категорија 5 („Автобуси и комбибуси") — се разликуваат
-- преку регистрациската категорија (VehicleCategoryFilter = 'M2' / 'M3'),
-- која PricingEvaluator сега ја почитува.
--
-- Старите фиксни правила (974 меѓуградски 550 / 975 градски 380) се деактивираат.
-- Идемпотентно: маркер Code='ECO22B' — DELETE+INSERT.
-- ============================================================================

USE VTE;
GO
SET NOCOUNT ON;

UPDATE pc SET pc.Active = 0
FROM dbo.PriceCatalog pc
WHERE pc.PaymentCategoryGroupId = 8
  AND pc.VehiclePaymentCategoryId = 5
  AND pc.Active = 1
  AND ISNULL(pc.Code, '') NOT IN ('ECO22', 'ECO22B');
PRINT CONCAT('Стари автобуски еко-правила деактивирани: ', @@ROWCOUNT);

DELETE FROM dbo.PriceCatalog WHERE Code = 'ECO22B';
PRINT CONCAT('Постоечки ECO22B правила избришани (reseed): ', @@ROWCOUNT);

DECLARE @groupId int = 8, @vatId int, @companyId tinyint = NULL;
SELECT TOP 1 @vatId = Id FROM dbo.VatRate WHERE [Percent] = 0 ORDER BY Id;

DECLARE @rows TABLE (RegCat varchar(4), AgeFrom int, AgeTo int NULL, CcFrom float, CcTo float NULL, Price money, Label nvarchar(140));

-- M2 Минибуси (5 возрасти × 5 зафатнини)
INSERT INTO @rows (RegCat, AgeFrom, AgeTo, CcFrom, CcTo, Price, Label) VALUES
 ('M2', 0,5,    0,3000, 400, N''),('M2', 0,5,  3001,6000, 450, N''),('M2', 0,5,  6001,9000, 500, N''),('M2', 0,5,  9001,12000, 600, N''),('M2', 0,5,  12001,NULL, 700, N''),
 ('M2', 6,10,   0,3000, 450, N''),('M2', 6,10, 3001,6000, 600, N''),('M2', 6,10, 6001,9000, 700, N''),('M2', 6,10, 9001,12000, 800, N''),('M2', 6,10, 12001,NULL, 900, N''),
 ('M2',11,20,   0,3000, 600, N''),('M2',11,20, 3001,6000, 750, N''),('M2',11,20, 6001,9000, 900, N''),('M2',11,20, 9001,12000,1000, N''),('M2',11,20, 12001,NULL,1200, N''),
 ('M2',21,30,   0,3000, 850, N''),('M2',21,30, 3001,6000, 950, N''),('M2',21,30, 6001,9000,1000, N''),('M2',21,30, 9001,12000,1100, N''),('M2',21,30, 12001,NULL,1200, N''),
 ('M2',31,NULL, 0,3000, 950, N''),('M2',31,NULL,3001,6000,1050, N''),('M2',31,NULL,6001,9000,1200, N''),('M2',31,NULL,9001,12000,1300, N''),('M2',31,NULL,12001,NULL,1500, N'');

-- M3 Автобуси (5 возрасти × 6 зафатнини)
INSERT INTO @rows (RegCat, AgeFrom, AgeTo, CcFrom, CcTo, Price, Label) VALUES
 ('M3', 0,5,    0,3000, 550, N''),('M3', 0,5,  3001,6000, 650, N''),('M3', 0,5,  6001,9000, 700, N''),('M3', 0,5,  9001,12000, 800, N''),('M3', 0,5,  12001,16000, 900, N''),('M3', 0,5,  16001,NULL,1100, N''),
 ('M3', 6,10,   0,3000, 650, N''),('M3', 6,10, 3001,6000, 750, N''),('M3', 6,10, 6001,9000, 800, N''),('M3', 6,10, 9001,12000, 900, N''),('M3', 6,10, 12001,16000,1000, N''),('M3', 6,10, 16001,NULL,1200, N''),
 ('M3',11,20,   0,3000, 750, N''),('M3',11,20, 3001,6000, 850, N''),('M3',11,20, 6001,9000, 900, N''),('M3',11,20, 9001,12000,1000, N''),('M3',11,20, 12001,16000,1100, N''),('M3',11,20, 16001,NULL,1300, N''),
 ('M3',21,30,   0,3000, 850, N''),('M3',21,30, 3001,6000, 950, N''),('M3',21,30, 6001,9000,1000, N''),('M3',21,30, 9001,12000,1100, N''),('M3',21,30, 12001,16000,1200, N''),('M3',21,30, 16001,NULL,1400, N''),
 ('M3',31,NULL, 0,3000, 950, N''),('M3',31,NULL,3001,6000,1050, N''),('M3',31,NULL,6001,9000,1100, N''),('M3',31,NULL,9001,12000,1200, N''),('M3',31,NULL,12001,16000,1300, N''),('M3',31,NULL,16001,NULL,1500, N'');

UPDATE @rows SET Label = CONCAT(
    CASE WHEN AgeTo IS NULL THEN N'над 30 год.' ELSE CONCAT(N'од ', AgeFrom, N' до ', AgeTo, N' год.') END,
    N' · зафатнина ',
    CASE WHEN CcTo IS NULL THEN CONCAT(N'над ', CAST(CcFrom AS int), N' см3')
         ELSE CONCAT(CAST(CcFrom AS int), N'–', CAST(CcTo AS int), N' см3') END);

INSERT INTO dbo.PriceCatalog
    (Code, Name, BasePrice, VatRateId, [Trigger], VehiclePaymentCategoryId, CommunityId,
     PriceCompanyId, PaymentCategoryGroupId, VehicleField, ParametarFrom, ParametarTo,
     AgeFrom, AgeTo, VehicleCategoryFilter, BankAccount, PaymentForm, Active)
SELECT
    'ECO22B',
    CONCAT(CASE r.RegCat WHEN 'M2' THEN N'за Минибуси (M2)' ELSE N'за Автобуси (M3)' END, N' — ', r.Label),
    r.Price, @vatId, 1, 5, NULL,
    @companyId, @groupId, N'EngineWorkingCapacityCc', r.CcFrom, r.CcTo,
    r.AgeFrom, r.AgeTo, r.RegCat, NULL, NULL, 1
FROM @rows r;
PRINT CONCAT('Нови ECO22B правила внесени: ', @@ROWCOUNT);

SELECT VehicleCategoryFilter AS RegCat, COUNT(*) AS Rules, MIN(BasePrice) AS MinP, MAX(BasePrice) AS MaxP
FROM dbo.PriceCatalog WHERE Code = 'ECO22B' GROUP BY VehicleCategoryFilter;
