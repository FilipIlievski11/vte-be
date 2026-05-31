using VTE.Domain.Common;

namespace VTE.Domain.Payments;

/// <summary>
/// One scheduled installment on a <see cref="PaymentDocument"/>. The PaymentDocument
/// is the master bill; the schedule rows are the calendar of payments under an
/// <see cref="InstallmentAgreement"/>. Legacy: PaymentDocumentsRata (254,348 rows).
///
/// Improvements over legacy:
///   • Explicit SequenceNo + DueDate columns (legacy stored only DatePayed once paid).
///   • PaidAmount separate from scheduled Amount (legacy used the same column).
/// </summary>
public class InstallmentSchedule : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    public long PaymentDocumentId { get; set; }

    /// <summary>1-based ordinal in the schedule (1, 2, 3 …). New in v2.</summary>
    public int SequenceNo { get; set; }

    /// <summary>Scheduled installment amount. Legacy: Price (money).</summary>
    public decimal Amount { get; set; }

    /// <summary>When this installment is due. New in v2 (legacy didn't store it explicitly).</summary>
    public DateOnly? DueDate { get; set; }

    /// <summary>Has this installment been settled? Legacy: Payed.</summary>
    public bool Paid { get; set; }

    /// <summary>When the customer actually paid. Legacy: DatePayed.</summary>
    public DateTime? PaidAt { get; set; }

    /// <summary>Actual amount paid (≤ Amount in case of partial). Legacy: not stored separately.</summary>
    public decimal? PaidAmount { get; set; }

    /// <summary>Which station collected the payment. Legacy: IdOrganization.</summary>
    public int? OrganizationId { get; set; }

    /// <summary>Legacy operator id who registered the payment, or null when v2-issued. Legacy: IdOperator.</summary>
    public int? OperatorLegacyId { get; set; }

    public string? Note { get; set; }
    public bool Active { get; set; } = true;
}
