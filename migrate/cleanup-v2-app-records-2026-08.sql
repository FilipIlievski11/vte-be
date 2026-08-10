-- cleanup-v2-app-records-2026-08.sql — бришење на записите ВНЕСЕНИ ОД НОВАТА
-- АПЛИКАЦИЈА што се дупликат/тест наспроти легаси продукцијата (одлука на Филип,
-- 2026-08-06: „izbrishi samo toa shto e dodadeno od app... mvd i polnomoshni ne cepkaj").
--
-- СЕ БРИШЕ (сè v2-native, Id >= 10M или LegacyId IS NULL):
--   • Тест-долгови мај–јули (Active=0) + денешните долгови од работниот тест
--   • Сметка 10000003 (01-37-48125/2026) + ставка — наплатата е прокнижена во легаси
--   • Барање 10000001 + автоматскиот технички преглед 10000001 (+ детали/докази)
--   • Тест-клиентите „ТЕСТ" (10000002) и „ЅЅ" (10000003) + нивни релации ако се осамени
--
-- НЕ СЕ ДОПИРА:
--   • InternationalDrivingLicence / VehiclePermission — НИШТО (ни тестовите)
--   • 619-те клиенти + 2023 релации >= 10M од vertest миграцијата (огледало на МВД/
--     полномошно носители) — легитимни податоци
--   • AuditEntry — останува како историја
--
-- Идемпотентна. Резерва: ноќен backup + offsite (docs/14).

USE VTE;
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- 0. Одврзи ги долговите од ставките на сметката (FK SettledByLineId).
UPDATE d SET d.SettledByLineId = NULL, d.Paid = 0
FROM dbo.CustomerDebt d
WHERE d.LegacyId IS NULL AND d.SettledByLineId IS NOT NULL;

-- 1. Сметката од 05.08 + ставки (+ евентуални рати/договор — нема, но идемпотентно).
DELETE FROM dbo.PaymentDocumentLine WHERE PaymentDocumentId >= 10000000;
DELETE FROM dbo.InstallmentSchedule WHERE PaymentDocumentId >= 10000000;
DELETE FROM dbo.PaymentDocument WHERE Id >= 10000000;
PRINT CONCAT('Smetki: ', @@ROWCOUNT);

-- 2. Сите v2-native долгови (тестовите + денешните; МВД регистарот не се допира —
--    долгот е наплата, не документ).
DELETE FROM dbo.CustomerDebt WHERE LegacyId IS NULL;
PRINT CONCAT('Dolgovi: ', @@ROWCOUNT);

-- 3. Технички преглед 10000001 (авто-креиран од барањето) + детали.
DELETE FROM dbo.TechnicalExamReportDetail WHERE TechnicalExamReportId >= 10000000;
DELETE FROM dbo.TechnicalExamReport WHERE Id >= 10000000;
PRINT CONCAT('Teh. pregledi: ', @@ROWCOUNT);

-- 4. Барање 10000001 + докази/прилози/референци.
DELETE FROM dbo.RequestOwnershipProof WHERE RequestId >= 10000000;
DELETE FROM dbo.RequestPaymentProof WHERE RequestId >= 10000000;
DELETE FROM dbo.RequestAttachment WHERE RequestId >= 10000000;
DELETE FROM dbo.Request WHERE Id >= 10000000;
PRINT CONCAT('Baranja: ', @@ROWCOUNT);

-- 5. Тест-клиенти „ТЕСТ" и „ЅЅ" — само ако не се врзани за ништо преостанато.
DECLARE @testClients TABLE (Id bigint);
INSERT INTO @testClients
SELECT c.Id FROM dbo.Client c
WHERE c.Id IN (10000002, 10000003)
  AND NOT EXISTS (SELECT 1 FROM dbo.InternationalDrivingLicence i WHERE i.ClientId = c.Id)
  AND NOT EXISTS (SELECT 1 FROM dbo.ClientVehicleRelation r
                  WHERE r.ClientId = c.Id
                    AND (EXISTS (SELECT 1 FROM dbo.CustomerDebt d WHERE d.CustomerVehicleRelationId = r.Id)
                      OR EXISTS (SELECT 1 FROM dbo.PaymentDocument p WHERE p.CustomerVehicleRelationId = r.Id)
                      OR EXISTS (SELECT 1 FROM dbo.Request q WHERE q.ClientVehicleRelationId = r.Id)
                      OR EXISTS (SELECT 1 FROM dbo.VehiclePermission vp WHERE vp.ClientVehicleRelationId = r.Id)));

DELETE FROM dbo.ClientPersonalData WHERE ClientId IN (SELECT Id FROM @testClients);
DELETE FROM dbo.ClientVehicleRelation WHERE ClientId IN (SELECT Id FROM @testClients);
DELETE FROM dbo.Client WHERE Id IN (SELECT Id FROM @testClients);
PRINT CONCAT('Test klienti: ', @@ROWCOUNT);

-- 6. Релации-сирачиња >= 10M: не се врзани за НИШТО (остатоци од избришани
--    барања/тестови). Vertest релациите се исклучени преку проверките (полномошно/
--    МВД/долг/сметка/барање врзани → не се фаќаат).
DELETE r FROM dbo.ClientVehicleRelation r
WHERE r.Id >= 10000000
  AND NOT EXISTS (SELECT 1 FROM dbo.VehiclePermission vp WHERE vp.ClientVehicleRelationId = r.Id)
  AND NOT EXISTS (SELECT 1 FROM dbo.InternationalDrivingLicence i WHERE i.ClientId = r.ClientId)
  AND NOT EXISTS (SELECT 1 FROM dbo.CustomerDebt d WHERE d.CustomerVehicleRelationId = r.Id)
  AND NOT EXISTS (SELECT 1 FROM dbo.PaymentDocument p WHERE p.CustomerVehicleRelationId = r.Id)
  AND NOT EXISTS (SELECT 1 FROM dbo.Request q WHERE q.ClientVehicleRelationId = r.Id OR q.NewClientVehicleRelationId = r.Id);
PRINT CONCAT('Sirak relacii: ', @@ROWCOUNT);

COMMIT TRANSACTION;

-- Контрола: што остана v2-native (МВД/полномошна и vertest огледалото ТРЕБА да стојат).
SELECT 'PaymentDocument >= 10M' = COUNT(*) FROM dbo.PaymentDocument WHERE Id >= 10000000;
SELECT 'CustomerDebt v2-native' = COUNT(*) FROM dbo.CustomerDebt WHERE LegacyId IS NULL;
SELECT 'Request >= 10M' = COUNT(*) FROM dbo.Request WHERE Id >= 10000000;
SELECT 'TechExam >= 10M' = COUNT(*) FROM dbo.TechnicalExamReport WHERE Id >= 10000000;
SELECT 'IDL vkupno (nedopreno)' = COUNT(*) FROM dbo.InternationalDrivingLicence;
SELECT 'VehiclePermission vkupno (nedopreno)' = COUNT(*) FROM dbo.VehiclePermission;
SELECT 'Klienti >= 10M (vertest ogledalo)' = COUNT(*) FROM dbo.Client WHERE Id >= 10000000;
GO
