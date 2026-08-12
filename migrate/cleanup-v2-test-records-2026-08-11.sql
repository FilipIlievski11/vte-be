-- cleanup-v2-test-records-2026-08-11.sql — бришење на ТЕСТ записите внесени од
-- новата апликација (одлука на Филип, 2026-08-11: „izbrishi se sho imame
-- dodadeno od ovaa app kako test").
--
-- СЕ БРИШЕ:
--   • Барање >= 10M + авто-креираниот технички преглед + сите v2-native долгови
--     (тестот од 11.08 — неплатен, без сметка)
--   • МВД тестовите од развојот (01–02.07): „TEST-CLAUDE-99999" (smoke-test),
--     „TEST-IDL-SNAP-1" (snapshot test), 2222222222, 22333, 11111 — сите на
--     Филип/тест-баратели, четири од нив веќе Active=0
--   • Полномошна бр. 1 и 2 (03.07, Филип / СТОЈАН СТОЈАНОВ) — Active=0
--
-- НЕ СЕ ДОПИРА (ПРОВЕРЕНО, НЕ Е ТЕСТ):
--   • МВД Id 12170 — бр. 2700, ИВАН ИВАНОВСКИ, издадено 05.08 низ v2. Бројот се
--     вклопува во вистинската легаси нумерација (2706/2707 на 07.08, 2710–2713 на
--     10.08) и клиентот нема друго МВД → ВИСТИНСКИ издаден документ.
--   • Клиентите „ТЕСТ" (10000002) и „ЅЅ" (10000003) — огледало на легаси vertest
--     записи (IDL LegacyId 11/12, броеви „едде"/„тетгфергфе"). Мора прво да се
--     избришат во легаси, инаку следниот sync ги враќа.
--   • 624 клиенти и 2028 релации >= 10M од vertest синхронизацијата (создадени
--     во 03:02/10:02 од scheduler-от) — легитимни податоци.
--   • AuditEntry — останува како историја.
--
-- Идемпотентна. Резерва: ноќен backup + offsite (docs/14).

USE VTE;
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @testIdl TABLE (Id bigint);
INSERT INTO @testIdl (Id)
SELECT Id FROM dbo.InternationalDrivingLicence
WHERE LegacyId IS NULL
  AND Id <> 12170                                   -- вистинското МВД бр. 2700
  AND (NumberOfLicence LIKE 'TEST%'
    OR NumberOfLicence IN ('2222222222', '22333', '11111'));

DECLARE @testPerm TABLE (Id bigint);
INSERT INTO @testPerm (Id)
SELECT Id FROM dbo.VehiclePermission
WHERE LegacyId IS NULL AND PermissionNumber IN ('1', '2');

-- 0. Одврзи ги долговите од ставки на сметка (FK), потоа сите v2-native долгови.
UPDATE d SET d.SettledByLineId = NULL, d.Paid = 0
FROM dbo.CustomerDebt d
WHERE d.LegacyId IS NULL AND d.SettledByLineId IS NOT NULL;

DELETE FROM dbo.CustomerDebt WHERE LegacyId IS NULL;
PRINT CONCAT('Dolgovi: ', @@ROWCOUNT);

-- 1. Сметки >= 10M (нема, но идемпотентно).
DELETE FROM dbo.PaymentDocumentLine WHERE PaymentDocumentId >= 10000000;
DELETE FROM dbo.InstallmentSchedule WHERE PaymentDocumentId >= 10000000;
DELETE FROM dbo.PaymentDocument WHERE Id >= 10000000;
PRINT CONCAT('Smetki: ', @@ROWCOUNT);

-- 2. Барања >= 10M — прво одврзи го тех. прегледот (FK), па децата.
UPDATE dbo.Request SET TechnicalExamReportId = NULL WHERE Id >= 10000000;
DELETE FROM dbo.RequestOwnershipProof WHERE RequestId >= 10000000;
DELETE FROM dbo.RequestPaymentProof   WHERE RequestId >= 10000000;
DELETE FROM dbo.RequestAttachment     WHERE RequestId >= 10000000;
DELETE FROM dbo.Request WHERE Id >= 10000000;
PRINT CONCAT('Baranja: ', @@ROWCOUNT);

-- 3. Технички прегледи >= 10M (авто-креирани од барањата).
DELETE FROM dbo.TechnicalExamReportDetail WHERE TechnicalExamReportId >= 10000000;
DELETE FROM dbo.TechnicalExamReport WHERE Id >= 10000000;
PRINT CONCAT('Teh. pregledi: ', @@ROWCOUNT);

-- 4. МВД тестови од развојот.
DELETE FROM dbo.InternationalDrivingLicence WHERE Id IN (SELECT Id FROM @testIdl);
PRINT CONCAT('MVD testovi: ', @@ROWCOUNT);

-- 5. Полномошна тестови од развојот.
DELETE FROM dbo.VehiclePermission WHERE Id IN (SELECT Id FROM @testPerm);
PRINT CONCAT('Polnomosna testovi: ', @@ROWCOUNT);

COMMIT TRANSACTION;

-- Контрола: што остана (десната колона е она што ТРЕБА да стои).
SELECT 'Baranja >= 10M'          = COUNT(*) FROM dbo.Request WHERE Id >= 10000000;
SELECT 'Teh. pregledi >= 10M'    = COUNT(*) FROM dbo.TechnicalExamReport WHERE Id >= 10000000;
SELECT 'Smetki >= 10M'           = COUNT(*) FROM dbo.PaymentDocument WHERE Id >= 10000000;
SELECT 'Dolgovi v2-native'       = COUNT(*) FROM dbo.CustomerDebt WHERE LegacyId IS NULL;
SELECT 'MVD v2-native (1 = 2700)' = COUNT(*) FROM dbo.InternationalDrivingLicence WHERE LegacyId IS NULL;
SELECT 'Polnomosna v2-native (0)' = COUNT(*) FROM dbo.VehiclePermission WHERE LegacyId IS NULL;
SELECT 'MVD vkupno'              = COUNT(*) FROM dbo.InternationalDrivingLicence;
SELECT 'Polnomosna vkupno'       = COUNT(*) FROM dbo.VehiclePermission;
SELECT 'Klienti >= 10M (vertest)' = COUNT(*) FROM dbo.Client WHERE Id >= 10000000;
GO
