-- ============================================================================
-- One-time backfill (2026-07-06): PaymentDocument / PaymentDocumentLine /
-- InstallmentAgreement / InstallmentSchedule rows missing below the current max.
--
-- Why the holes exist: the bulk lift (migrate-payments.sql) skipped every bill
-- whose ClientVehicleRelation didn't exist in v2 at the time — and 7,738
-- relations were missing until the 2026-07-06 gap-fill. Plus everything legacy
-- created after the 2026-06-13 lift. The embedded incremental sync only walks
-- forward (append watermarks), so holes are repaired HERE, manually, and this
-- script is safe to re-run (idempotent by Id).
--
-- Perf notes (linked server): id-lists are pulled single-table (fast); full
-- line rows are pulled in bounded Id chunks and filtered locally — NEVER via
-- a distributed semi-join (N+1 catastrophe, see CLAUDE.md).
-- ============================================================================
SET NOCOUNT ON;
SET XACT_ABORT ON;
DECLARE @rows int, @floor bigint = 10000000;

-- ----------------------------------------------------------------------------
-- 1. InstallmentAgreement (6.8k — full pull is trivial)
-- ----------------------------------------------------------------------------
PRINT '=== InstallmentAgreement holes ===';
SELECT Id, Broj, Datum, GarantNaziv, GarantAdresa, GartEMB, BrNaRati, Active
INTO #ia
FROM VTEZVV_LIVE.VTEZVV.dbo.DogovorZaRati;

SET IDENTITY_INSERT dbo.InstallmentAgreement ON;
INSERT INTO dbo.InstallmentAgreement (Id, CompanyId, Number, [Date], TotalInstallments,
                                      GuarantorName, GuarantorAddress, GuarantorEmbg,
                                      Active, CreatedAt, ModifiedAt)
SELECT i.Id, CONVERT(tinyint, 4), LEFT(ISNULL(i.Broj, N''), 40), CONVERT(date, i.Datum),
       ISNULL(i.BrNaRati, 0), LEFT(i.GarantNaziv, 200), LEFT(i.GarantAdresa, 300),
       LEFT(i.GartEMB, 13), ISNULL(i.Active, CONVERT(bit, 1)), GETUTCDATE(), NULL
FROM #ia i
WHERE i.Id < @floor
  AND NOT EXISTS (SELECT 1 FROM dbo.InstallmentAgreement x WHERE x.Id = i.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.InstallmentAgreement OFF;
PRINT CONCAT('  -> ', @rows, ' agreements backfilled.');
DROP TABLE #ia;

-- ----------------------------------------------------------------------------
-- 2. PaymentDocument (full row pull ~250k once; local NOT EXISTS diff)
-- ----------------------------------------------------------------------------
PRINT '=== PaymentDocument holes ===';
SELECT p.Id, p.IdPaymentType, p.IdCustomerVehicleRelation, p.IdOperator, p.IdOrganization,
       p.DocumentNumber, p.DatePay, p.DateRequired, p.Discount, p.Payed, p.Storno,
       p.Note, p.IdDogovor, p.IdFakturiraNa, p.Active
INTO #pd
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocuments p
WHERE p.Id < 10000000;

SET IDENTITY_INSERT dbo.PaymentDocument ON;
INSERT INTO dbo.PaymentDocument
    (Id, CompanyId, PaymentTypeId, CustomerVehicleRelationId, OperatorLegacyId, OrganizationId,
     DocumentNumber, IssueDate, DueDate, Discount, Paid, Stornoed, StornoReason, Note,
     AgreementId, InvoicedToCompanyId, FiscalPrintedAt,
     LegacyId, Active, CreatedAt, ModifiedAt, CreatedByUserId, ModifiedByUserId)
SELECT
    p.Id, CONVERT(tinyint, 4), p.IdPaymentType, p.IdCustomerVehicleRelation,
    NULLIF(p.IdOperator, 0), p.IdOrganization,
    COALESCE(p.DocumentNumber, ''), p.DatePay, p.DateRequired, p.Discount,
    CONVERT(bit, COALESCE(p.Payed, 0)), CONVERT(bit, COALESCE(p.Storno, 0)), NULL, p.Note,
    CASE WHEN EXISTS (SELECT 1 FROM dbo.InstallmentAgreement ia WHERE ia.Id = p.IdDogovor)
         THEN p.IdDogovor ELSE NULL END,
    NULLIF(p.IdFakturiraNa, 0), NULL,
    p.Id, CONVERT(bit, COALESCE(p.Active, 1)), COALESCE(p.DatePay, GETUTCDATE()), NULL, NULL, NULL
FROM #pd p
INNER JOIN dbo.ClientVehicleRelation r ON r.Id = p.IdCustomerVehicleRelation
INNER JOIN dbo.PaymentType          pt ON pt.Id = p.IdPaymentType
WHERE NOT EXISTS (SELECT 1 FROM dbo.PaymentDocument x WHERE x.Id = p.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.PaymentDocument OFF;
PRINT CONCAT('  -> ', @rows, ' documents backfilled.');

DECLARE @stillMissingDocs int = (SELECT COUNT(*) FROM #pd p WHERE NOT EXISTS (SELECT 1 FROM dbo.PaymentDocument x WHERE x.Id = p.Id));
PRINT CONCAT('  (', @stillMissingDocs, ' legacy docs remain unimportable — orphan relation/type)');
DROP TABLE #pd;

-- ----------------------------------------------------------------------------
-- 3. PaymentDocumentLine — id-diff via id-list pull, then bounded Id-chunk
--    full-row pulls filtered locally (~1.6M ids, chunks of 200k)
-- ----------------------------------------------------------------------------
PRINT '=== PaymentDocumentLine holes ===';
SELECT d.Id INTO #lid FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsDetails d WHERE d.Id < 10000000;
CREATE UNIQUE CLUSTERED INDEX ix ON #lid (Id);

SELECT l.Id INTO #missing
FROM #lid l
WHERE NOT EXISTS (SELECT 1 FROM dbo.PaymentDocumentLine x WHERE x.Id = l.Id);
DECLARE @missingLines int = (SELECT COUNT(*) FROM #missing);
PRINT CONCAT('  missing line ids: ', @missingLines);

IF @missingLines > 0
BEGIN
    DECLARE @lo bigint = (SELECT MIN(Id) FROM #missing),
            @maxId bigint = (SELECT MAX(Id) FROM #missing),
            @chunk bigint = 200000, @inserted int = 0;

    WHILE @lo <= @maxId
    BEGIN
        -- narrow the chunk to spans that actually contain missing ids
        SET @lo = (SELECT MIN(Id) FROM #missing WHERE Id >= @lo);
        IF @lo IS NULL BREAK;
        DECLARE @hi bigint = @lo + @chunk - 1;

        SELECT d.Id, d.IdPaymentDocuments, d.IdPriceCatalog, d.Price, d.DDV, d.Discount,
               d.Note, d.PrePayed, d.NotePrePayed, d.Active
        INTO #chunkRows
        FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsDetails d
        WHERE d.Id BETWEEN @lo AND @hi;

        SET IDENTITY_INSERT dbo.PaymentDocumentLine ON;
        INSERT INTO dbo.PaymentDocumentLine
            (Id, CompanyId, PaymentDocumentId, PriceCatalogId, UnitPrice, VatPercent,
             Discount, Quantity, Note, PrePaid, PrePaidNote, CustomerDebtId, Active)
        SELECT
            c.Id, CONVERT(tinyint, 4), c.IdPaymentDocuments,
            CASE WHEN EXISTS (SELECT 1 FROM dbo.PriceCatalog pc WHERE pc.Id = c.IdPriceCatalog)
                 THEN c.IdPriceCatalog ELSE 999999 END,
            CONVERT(decimal(18,4), c.Price), CONVERT(float, c.DDV),
            CONVERT(float, COALESCE(c.Discount, 0)), 1,
            LEFT(NULLIF(LTRIM(RTRIM(c.Note)), N''), 300),
            CONVERT(bit, COALESCE(c.PrePayed, 0)), c.NotePrePayed, NULL,
            CONVERT(bit, COALESCE(c.Active, 1))
        FROM #chunkRows c
        INNER JOIN #missing m ON m.Id = c.Id
        INNER JOIN dbo.PaymentDocument pd ON pd.Id = c.IdPaymentDocuments;
        SET @inserted = @inserted + @@ROWCOUNT;
        SET IDENTITY_INSERT dbo.PaymentDocumentLine OFF;

        DROP TABLE #chunkRows;
        SET @lo = @hi + 1;
    END
    PRINT CONCAT('  -> ', @inserted, ' lines backfilled.');
END
DROP TABLE #missing; DROP TABLE #lid;

-- ----------------------------------------------------------------------------
-- 4. InstallmentSchedule (344k ids — id-diff, then chunked like lines)
-- ----------------------------------------------------------------------------
PRINT '=== InstallmentSchedule holes ===';
SELECT r.Id INTO #rid FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsRata r WHERE r.Id < 10000000;
CREATE UNIQUE CLUSTERED INDEX ix ON #rid (Id);
SELECT l.Id INTO #rmiss FROM #rid l
WHERE NOT EXISTS (SELECT 1 FROM dbo.InstallmentSchedule x WHERE x.Id = l.Id);
DECLARE @missR int = (SELECT COUNT(*) FROM #rmiss);
PRINT CONCAT('  missing schedule ids: ', @missR);

IF @missR > 0
BEGIN
    DECLARE @rlo bigint = (SELECT MIN(Id) FROM #rmiss),
            @rmax bigint = (SELECT MAX(Id) FROM #rmiss), @rins int = 0;
    WHILE @rlo <= @rmax
    BEGIN
        SET @rlo = (SELECT MIN(Id) FROM #rmiss WHERE Id >= @rlo);
        IF @rlo IS NULL BREAK;
        DECLARE @rhi bigint = @rlo + 199999;

        SELECT r.Id, r.IdPaymentDocument, r.Price, r.Payed, r.DatePayed,
               r.IdOrganization, r.IdOperator, r.Note, r.Active
        INTO #rchunk
        FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsRata r
        WHERE r.Id BETWEEN @rlo AND @rhi;

        SET IDENTITY_INSERT dbo.InstallmentSchedule ON;
        INSERT INTO dbo.InstallmentSchedule
            (Id, CompanyId, PaymentDocumentId, SequenceNo, Amount, DueDate,
             Paid, PaidAt, PaidAmount, OrganizationId, OperatorLegacyId, Note, Active)
        SELECT
            c.Id, CONVERT(tinyint, 4), c.IdPaymentDocument,
            -- sequence appended after existing rows of the same document
            CAST((SELECT COUNT(*) FROM dbo.InstallmentSchedule s2
                  WHERE s2.PaymentDocumentId = c.IdPaymentDocument AND s2.Id < c.Id) +
                 ROW_NUMBER() OVER (PARTITION BY c.IdPaymentDocument ORDER BY c.Id) AS int),
            CONVERT(decimal(18,4), c.Price), NULL,
            CONVERT(bit, COALESCE(c.Payed, 0)), c.DatePayed,
            CASE WHEN COALESCE(c.Payed,0) = 1 THEN CONVERT(decimal(18,4), c.Price) ELSE NULL END,
            NULLIF(c.IdOrganization, 0), NULLIF(c.IdOperator, 0), c.Note,
            CONVERT(bit, COALESCE(c.Active, 1))
        FROM #rchunk c
        INNER JOIN #rmiss m ON m.Id = c.Id
        INNER JOIN dbo.PaymentDocument pd ON pd.Id = c.IdPaymentDocument;
        SET @rins = @rins + @@ROWCOUNT;
        SET IDENTITY_INSERT dbo.InstallmentSchedule OFF;

        DROP TABLE #rchunk;
        SET @rlo = @rhi + 1;
    END
    PRINT CONCAT('  -> ', @rins, ' schedules backfilled.');
END
DROP TABLE #rmiss; DROP TABLE #rid;

PRINT '';
PRINT '=== after backfill ===';
SELECT 'PaymentDocument' t, COUNT(*) c FROM dbo.PaymentDocument
UNION ALL SELECT 'PaymentDocumentLine', COUNT(*) FROM dbo.PaymentDocumentLine
UNION ALL SELECT 'InstallmentAgreement', COUNT(*) FROM dbo.InstallmentAgreement
UNION ALL SELECT 'InstallmentSchedule', COUNT(*) FROM dbo.InstallmentSchedule;
