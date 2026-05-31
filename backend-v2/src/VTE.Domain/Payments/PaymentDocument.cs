using VTE.Domain.Common;

namespace VTE.Domain.Payments;

/// <summary>
/// A bill / receipt / invoice — the master record of a payment transaction. Lines
/// live in <see cref="PaymentDocumentLine"/>; installments in
/// <see cref="InstallmentSchedule"/>.
///
/// Legacy source: PaymentDocuments (244,192 rows). Improvements over legacy:
///   • Audit columns (CreatedBy/At, ModifiedBy/At) instead of just LastChanged.
///   • Tenant-scoped via the EF query filter.
///   • `FiscalPrintedAt` flag for the outbox / retry pattern (Phase 5 fiscal adapter)
///     — fiscal printing is no longer a UI-Save side-effect.
///   • Stornoed/StornoReason split: an explicit reason instead of a bare flag.
///   • Operator stored as the v2 user id when possible (CreatedByUserId);
///     OperatorLegacyId is kept as fallback for migrated historical rows.
/// </summary>
public class PaymentDocument : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    /// <summary>How paid (cash / card / installment / invoice). Legacy: IdPaymentType.</summary>
    public int PaymentTypeId { get; set; }

    /// <summary>Who/what is being billed — the customer + vehicle. Legacy: IdCustomerVehicleRelation.</summary>
    public long CustomerVehicleRelationId { get; set; }

    /// <summary>Legacy operator id, kept for migrated rows when the v2 user identity is unknown.
    /// New v2 records prefer <see cref="CreatedByUserId"/>. Legacy: IdOperator.</summary>
    public int? OperatorLegacyId { get; set; }

    /// <summary>Issuing station/organization. Legacy: IdOrganization.</summary>
    public int OrganizationId { get; set; }

    /// <summary>Bill number ({prefix}{seq}/{year}). Legacy: DocumentNumber.</summary>
    public string DocumentNumber { get; set; } = "";

    /// <summary>Date the bill was issued. Legacy: DatePay (the actual payment date for paid bills).</summary>
    public DateTime IssueDate { get; set; }

    /// <summary>Due date. Legacy: DateRequired.</summary>
    public DateTime DueDate { get; set; }

    /// <summary>Document-level discount % (0..100). Legacy: Discount (real).</summary>
    public double? Discount { get; set; }

    /// <summary>Has the customer settled this bill? Legacy: Payed.</summary>
    public bool Paid { get; set; }

    /// <summary>Bill was reversed (cancelled). Legacy: Storno.</summary>
    public bool Stornoed { get; set; }

    /// <summary>Why the bill was reversed (new in v2 — legacy left this implicit).</summary>
    public string? StornoReason { get; set; }

    public string? Note { get; set; }

    /// <summary>Installment plan, if this bill is paid on a schedule. Legacy: IdDogovor.</summary>
    public long? AgreementId { get; set; }

    /// <summary>For B2B billing — the company being invoiced (when different from the
    /// CustomerVehicleRelation owner). Legacy: IdFakturiraNa.</summary>
    public int? InvoicedToCompanyId { get; set; }

    /// <summary>Set when the fiscal device has confirmed the print (outbox marker).
    /// Null until the IFiscalPrinter adapter completes. New in v2.</summary>
    public DateTime? FiscalPrintedAt { get; set; }

    /// <summary>Legacy PaymentDocuments.Id, for migration traceability.</summary>
    public long? LegacyId { get; set; }

    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public string? CreatedByUserId { get; set; }
    public string? ModifiedByUserId { get; set; }

    /// <summary>EF concurrency token.</summary>
    public byte[] RowVersion { get; set; } = [];
}
