-- fix-installment-duedate.sql — поправка на огледалните рати (Id < 10M).
--
-- Легаси PaymentDocumentsRata.DatePayed е ПЛАНИРАН датум (рок) додека ратата не се
-- плати, а потоа датум на плаќање. Старото мапирање го ставаше безусловно во PaidAt
-- и оставаше DueDate NULL — па неплатена рата покажуваше „ПЛАТЕНО НА <иден датум>"
-- со празен РОК. Оваа скрипта:
--   1. DueDate ← легаси DatePayed (за сите огледални рати без DueDate).
--   2. PaidAt/PaidAmount ← NULL за неплатените огледални рати.
-- Идемпотентна; migrate-incremental.sql е веќе поправен за новите редови.

USE VTE;
GO

DECLARE @fixedDue int, @fixedPaid int;

UPDATE s SET s.DueDate = CONVERT(date, r.DatePayed)
FROM dbo.InstallmentSchedule s
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsRata r ON r.Id = s.Id
WHERE s.Id < 10000000 AND s.DueDate IS NULL AND r.DatePayed IS NOT NULL;
SET @fixedDue = @@ROWCOUNT;

UPDATE dbo.InstallmentSchedule
SET PaidAt = NULL, PaidAmount = NULL
WHERE Id < 10000000 AND Paid = 0 AND (PaidAt IS NOT NULL OR PaidAmount IS NOT NULL);
SET @fixedPaid = @@ROWCOUNT;

PRINT CONCAT('DueDate пополнет на ', @fixedDue, ' рати; PaidAt исчистен на ', @fixedPaid, ' неплатени.');

-- Контрола: неплатена рата со PaidAt или платена без PaidAt не смее да постои (огледални).
SELECT ostanati_gresni = COUNT(*)
FROM dbo.InstallmentSchedule
WHERE Id < 10000000 AND ((Paid = 0 AND PaidAt IS NOT NULL) OR (Paid = 1 AND PaidAt IS NULL));
GO
