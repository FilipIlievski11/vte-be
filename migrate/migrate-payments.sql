/*
  migrate-payments.sql  —  Phase 1 historical-data import for the v2 payment module.

  Source: VTEZVV_LIVE.VTEZVV.dbo.*  (linked server to legacy SQL Server)
  Target: VTE (current v2 DB)

  Strategy:
    • Catalog tables (VatRate, PaymentType): legacy Id-preserving via IDENTITY_INSERT.
    • PriceCatalog: rebuilt from `PaymentItemParametars JOIN PaymentItems` because
      legacy `PaymentDocumentsDetails.IdPriceCatalog` references PaymentItemParametars.Id
      (NOT PaymentItems.Id — that was a 7-character coincidence that bit us earlier).
      Each PriceCatalog row carries `ItemName — ParametarName` (e.g.
      "за Патнички возила — Оперативни трошоци") + the parametar Price.
    • A sentinel row at PriceCatalog #999999 ("Непозната ставка (мигрирана)") catches
      any rare legacy line whose IdPriceCatalog still doesn't match — defensive only;
      a clean migration should produce zero rows on it.
    • InstallmentAgreement / PaymentDocument / PaymentDocumentLine / InstallmentSchedule
      are Id-preserving, so FK joins between them are trivial.
    • Bills whose IdCustomerVehicleRelation no longer exists in v2 (≈ 6 % / 15 327 rows)
      are SKIPPED — they would otherwise violate the FK.
    • Idempotent: re-runnable. Wipes v2 payment tables first, then re-imports.

  Estimated volumes (one-time): ~244k bills · 1.17M lines · 254k installments.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

PRINT '== migrate-payments.sql start =='
PRINT 'Pre-clean v2 tables…'

DELETE FROM dbo.InstallmentSchedule;
DELETE FROM dbo.PaymentDocumentLine;
DELETE FROM dbo.PaymentDocument;
DELETE FROM dbo.InstallmentAgreement;
DELETE FROM dbo.PriceCatalog;
DELETE FROM dbo.PaymentType;
DELETE FROM dbo.VatRate;

DBCC CHECKIDENT('dbo.InstallmentSchedule',   RESEED) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.PaymentDocumentLine',   RESEED) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.PaymentDocument',       RESEED) WITH NO_INFOMSGS;
DBCC CHECKIDENT('dbo.InstallmentAgreement',  RESEED) WITH NO_INFOMSGS;
-- PriceCatalog / PaymentType / VatRate use Id-preserving inserts below; no reseed needed.
GO

------------------------------------------------------------------
-- 1. VatRate ← DDVCatalog
------------------------------------------------------------------
PRINT '1. VatRate ← DDVCatalog'

SET IDENTITY_INSERT dbo.VatRate ON;
INSERT INTO dbo.VatRate (Id, Code, Name, [Percent], Active)
SELECT Id,
       CONVERT(varchar(20), DDVValue),
       CASE WHEN DDVName IS NULL OR DDVName = N'' THEN N'ДДВ ' + CONVERT(nvarchar(20), DDVValue) + N' %' ELSE DDVName END,
       CONVERT(float, DDVValue),
       CONVERT(bit, Active)
FROM VTEZVV_LIVE.VTEZVV.dbo.DDVCatalog;
SET IDENTITY_INSERT dbo.VatRate OFF;
GO

------------------------------------------------------------------
-- 2. PaymentType ← PaymentTypes
------------------------------------------------------------------
PRINT '2. PaymentType ← PaymentTypes'

-- Linked-server COALESCE on bit columns is parser-fragile; stage via a temp table.
SELECT Id, Name, Fiskalna_kes, Fiskalna_karticka, Rati, Smetka, Faktura, Prefix, Active
  INTO #pt
  FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentTypes;

SET IDENTITY_INSERT dbo.PaymentType ON;
INSERT INTO dbo.PaymentType (Id, Code, Name, IsCash, IsCard, IsInstallment, PrintsReceipt, PrintsInvoice, Prefix, Active)
SELECT Id, NULL, Name,
       ISNULL(Fiskalna_kes,       CONVERT(bit, 0)),
       ISNULL(Fiskalna_karticka,  CONVERT(bit, 0)),
       ISNULL(Rati,               CONVERT(bit, 0)),
       ISNULL(Smetka,             CONVERT(bit, 0)),
       ISNULL(Faktura,            CONVERT(bit, 0)),
       Prefix,
       ISNULL(Active,             CONVERT(bit, 1))
FROM #pt;
SET IDENTITY_INSERT dbo.PaymentType OFF;
DROP TABLE #pt;
GO

------------------------------------------------------------------
-- 3. PriceCatalog ← PaymentItemParametars JOIN PaymentItems  (+ unknown sentinel)
------------------------------------------------------------------
PRINT '3. PriceCatalog ← PaymentItemParametars (real fees + unknown sentinel)'

DECLARE @defaultVatId int;
SET @defaultVatId = (SELECT TOP 1 Id FROM dbo.VatRate WHERE [Percent] = 18 ORDER BY Id);
IF @defaultVatId IS NULL SET @defaultVatId = (SELECT TOP 1 Id FROM dbo.VatRate ORDER BY Id);

-- Stage the real catalog rows.
SELECT
    pip.Id                                                                                AS Id,
    LEFT(CONCAT(ISNULL(pi.ItemName, N'(no item)'), N' — ', pip.PrametarName), 250)        AS Name,
    CONVERT(decimal(18,4), ISNULL(pip.Price, 0))                                          AS BasePrice,
    ISNULL(pip.Active, CONVERT(bit, 1))                                                   AS Active
INTO #pc
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentItemParametars pip
LEFT JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentItems pi ON pi.Id = pip.IdPaymentItem;
CREATE UNIQUE CLUSTERED INDEX IX_pc_id ON #pc(Id);

SET IDENTITY_INSERT dbo.PriceCatalog ON;

-- Real fees
INSERT INTO dbo.PriceCatalog (Id, Code, Name, BasePrice, VatRateId, [Trigger],
                              VehicleCategoryFilter, BankAccount, PaymentForm, Active)
SELECT Id, NULL, Name, BasePrice, @defaultVatId, CONVERT(tinyint, 0),
       NULL, NULL, NULL, Active
FROM #pc;

-- Unknown-sentinel catch-all for lines whose IdPriceCatalog doesn't resolve.
INSERT INTO dbo.PriceCatalog (Id, Code, Name, BasePrice, VatRateId, [Trigger],
                              VehicleCategoryFilter, BankAccount, PaymentForm, Active)
VALUES (999999, 'UNKNOWN', N'Непозната ставка (мигрирана)', 0, @defaultVatId, 0, NULL, NULL, NULL, 1);

SET IDENTITY_INSERT dbo.PriceCatalog OFF;
DROP TABLE #pc;
GO

------------------------------------------------------------------
-- 4. InstallmentAgreement ← DogovorZaRati
------------------------------------------------------------------
PRINT '4. InstallmentAgreement ← DogovorZaRati'

-- Stage so we can LEFT() over the non-canonical EMBG values (legacy stored arbitrary text there).
SELECT Id, Broj, Datum, GarantNaziv, GarantAdresa, GartEMB, BrNaRati, Active
  INTO #ia
  FROM VTEZVV_LIVE.VTEZVV.dbo.DogovorZaRati;

SET IDENTITY_INSERT dbo.InstallmentAgreement ON;
INSERT INTO dbo.InstallmentAgreement (Id, CompanyId, Number, [Date], TotalInstallments,
                                      GuarantorName, GuarantorAddress, GuarantorEmbg,
                                      Active, CreatedAt, ModifiedAt)
SELECT
    Id,
    CONVERT(tinyint, 4)             AS CompanyId,
    LEFT(ISNULL(Broj, N''),  40)    AS Number,
    CONVERT(date, Datum)            AS [Date],
    ISNULL(BrNaRati, 0)             AS TotalInstallments,
    LEFT(GarantNaziv,  200)         AS GuarantorName,
    LEFT(GarantAdresa, 300)         AS GuarantorAddress,
    LEFT(GartEMB,       13)         AS GuarantorEmbg,    -- legacy occasionally stuffed text here; truncate
    ISNULL(Active, CONVERT(bit, 1)) AS Active,
    GETUTCDATE(), NULL
FROM #ia;
SET IDENTITY_INSERT dbo.InstallmentAgreement OFF;
DROP TABLE #ia;
GO

------------------------------------------------------------------
-- 5. PaymentDocument ← PaymentDocuments
--    SKIP any bill whose IdCustomerVehicleRelation no longer exists in v2,
--    and any IdPaymentType / IdDogovor that doesn't map.
------------------------------------------------------------------
PRINT '5. PaymentDocument ← PaymentDocuments (skipping orphans)'

;WITH valid_pd AS (
    SELECT pd.*
    FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocuments pd
    WHERE EXISTS (SELECT 1 FROM dbo.ClientVehicleRelation r WHERE r.Id = pd.IdCustomerVehicleRelation)
      AND EXISTS (SELECT 1 FROM dbo.PaymentType pt WHERE pt.Id = pd.IdPaymentType)
)
SELECT * INTO #pd FROM valid_pd;

SET IDENTITY_INSERT dbo.PaymentDocument ON;
INSERT INTO dbo.PaymentDocument
    (Id, CompanyId, PaymentTypeId, CustomerVehicleRelationId, OperatorLegacyId, OrganizationId,
     DocumentNumber, IssueDate, DueDate, Discount, Paid, Stornoed, StornoReason, Note,
     AgreementId, InvoicedToCompanyId, FiscalPrintedAt,
     LegacyId, Active, CreatedAt, ModifiedAt, CreatedByUserId, ModifiedByUserId)
SELECT
    p.Id,
    CONVERT(tinyint, 4) AS CompanyId,
    p.IdPaymentType,
    p.IdCustomerVehicleRelation,
    NULLIF(p.IdOperator, 0),
    p.IdOrganization,
    COALESCE(p.DocumentNumber, ''),
    p.DatePay,
    p.DateRequired,
    p.Discount,
    CONVERT(bit, COALESCE(p.Payed, 0)),
    CONVERT(bit, COALESCE(p.Storno, 0)),
    NULL,                                          -- StornoReason — legacy didn't store
    p.Note,
    -- AgreementId only when the IdDogovor still exists in v2 InstallmentAgreement
    CASE WHEN EXISTS (SELECT 1 FROM dbo.InstallmentAgreement ia WHERE ia.Id = p.IdDogovor)
         THEN p.IdDogovor ELSE NULL END,
    NULLIF(p.IdFakturiraNa, 0),
    NULL,                                          -- FiscalPrintedAt — not in legacy
    p.Id,                                          -- LegacyId
    CONVERT(bit, COALESCE(p.Active, 1)),
    COALESCE(p.DatePay, GETUTCDATE()),             -- CreatedAt — best guess from legacy
    NULL,                                          -- ModifiedAt
    NULL, NULL                                     -- CreatedByUserId / ModifiedByUserId
FROM #pd p;
SET IDENTITY_INSERT dbo.PaymentDocument OFF;

DROP TABLE #pd;
GO

------------------------------------------------------------------
-- 6. PaymentDocumentLine ← PaymentDocumentsDetails
--    PriceCatalogId = legacy IdPriceCatalog directly (now matches real PaymentItemParametars).
--    Falls back to #999999 sentinel if a row references a deleted parametar.
--    Note is the legacy d.Note as-is (no synthetic prefix).
------------------------------------------------------------------
PRINT '6. PaymentDocumentLine ← PaymentDocumentsDetails'

SET IDENTITY_INSERT dbo.PaymentDocumentLine ON;
INSERT INTO dbo.PaymentDocumentLine
    (Id, CompanyId, PaymentDocumentId, PriceCatalogId, UnitPrice, VatPercent,
     Discount, Quantity, Note, PrePaid, PrePaidNote, CustomerDebtId, Active)
SELECT
    d.Id,
    CONVERT(tinyint, 4) AS CompanyId,
    d.IdPaymentDocuments,
    CASE WHEN EXISTS (SELECT 1 FROM dbo.PriceCatalog pc WHERE pc.Id = d.IdPriceCatalog)
         THEN d.IdPriceCatalog
         ELSE 999999                                     -- unknown sentinel
    END                                                AS PriceCatalogId,
    CONVERT(decimal(18,4), d.Price)                    AS UnitPrice,
    CONVERT(float, d.DDV)                              AS VatPercent,
    CONVERT(float, COALESCE(d.Discount, 0))            AS Discount,
    1                                                  AS Quantity,
    LEFT(NULLIF(LTRIM(RTRIM(d.Note)), N''), 300)       AS Note,
    CONVERT(bit, COALESCE(d.PrePayed, 0))              AS PrePaid,
    d.NotePrePayed                                      AS PrePaidNote,
    NULL                                                AS CustomerDebtId,
    CONVERT(bit, COALESCE(d.Active, 1))                 AS Active
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsDetails d
WHERE EXISTS (SELECT 1 FROM dbo.PaymentDocument pd WHERE pd.Id = d.IdPaymentDocuments);
SET IDENTITY_INSERT dbo.PaymentDocumentLine OFF;
GO

------------------------------------------------------------------
-- 7. InstallmentSchedule ← PaymentDocumentsRata
------------------------------------------------------------------
PRINT '7. InstallmentSchedule ← PaymentDocumentsRata'

SET IDENTITY_INSERT dbo.InstallmentSchedule ON;
INSERT INTO dbo.InstallmentSchedule
    (Id, CompanyId, PaymentDocumentId, SequenceNo, Amount, DueDate,
     Paid, PaidAt, PaidAmount, OrganizationId, OperatorLegacyId, Note, Active)
SELECT
    r.Id,
    CONVERT(tinyint, 4) AS CompanyId,
    r.IdPaymentDocument,
    CAST(ROW_NUMBER() OVER (PARTITION BY r.IdPaymentDocument ORDER BY r.Id) AS int) AS SequenceNo,
    CONVERT(decimal(18,4), r.Price) AS Amount,
    NULL AS DueDate,                               -- legacy didn't store the schedule
    CONVERT(bit, COALESCE(r.Payed, 0)) AS Paid,
    r.DatePayed AS PaidAt,
    CASE WHEN COALESCE(r.Payed,0) = 1 THEN CONVERT(decimal(18,4), r.Price) ELSE NULL END AS PaidAmount,
    NULLIF(r.IdOrganization, 0) AS OrganizationId,
    NULLIF(r.IdOperator, 0)     AS OperatorLegacyId,
    r.Note,
    CONVERT(bit, COALESCE(r.Active, 1))
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentDocumentsRata r
WHERE EXISTS (SELECT 1 FROM dbo.PaymentDocument pd WHERE pd.Id = r.IdPaymentDocument);
SET IDENTITY_INSERT dbo.InstallmentSchedule OFF;
GO

------------------------------------------------------------------
-- Summary
------------------------------------------------------------------
PRINT ''
PRINT '== migrate-payments.sql complete =='
SELECT 'VatRate'                                                 AS Table_, COUNT(*) AS Rows_ FROM dbo.VatRate UNION ALL
SELECT 'PaymentType',                                            COUNT(*)         FROM dbo.PaymentType UNION ALL
SELECT 'PriceCatalog (real parametars + 1 sentinel)',            COUNT(*)         FROM dbo.PriceCatalog UNION ALL
SELECT 'InstallmentAgreement',                                   COUNT(*)         FROM dbo.InstallmentAgreement UNION ALL
SELECT 'PaymentDocument',                                        COUNT(*)         FROM dbo.PaymentDocument UNION ALL
SELECT 'PaymentDocumentLine',                                    COUNT(*)         FROM dbo.PaymentDocumentLine UNION ALL
SELECT 'PaymentDocumentLine on unknown sentinel (#999999)',      COUNT(*)         FROM dbo.PaymentDocumentLine WHERE PriceCatalogId = 999999 UNION ALL
SELECT 'InstallmentSchedule',                                    COUNT(*)         FROM dbo.InstallmentSchedule;
