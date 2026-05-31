using VTE.Domain.Common;

namespace VTE.Domain.Payments;

/// <summary>
/// One billable line on a <see cref="PaymentDocument"/>. Each line references a
/// <see cref="PriceCatalog"/> entry, snapshots its price + VAT at the moment of sale
/// (so historical bills stay reproducible even if the catalog changes), and may settle
/// a specific <see cref="CustomerDebt"/> (added in Phase 3).
///
/// Legacy source: PaymentDocumentsDetails (1,166,083 rows).
/// </summary>
public class PaymentDocumentLine : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    public long PaymentDocumentId { get; set; }

    /// <summary>What is being sold. Legacy: IdPriceCatalog.</summary>
    public int PriceCatalogId { get; set; }

    /// <summary>Snapshot of the unit price at sale time. Legacy: Price (money).</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Snapshot of the VAT percent at sale time. Legacy: DDV (real).</summary>
    public double VatPercent { get; set; }

    /// <summary>Per-line discount % (0..100). Legacy: Discount (real).</summary>
    public double Discount { get; set; }

    /// <summary>Quantity (legacy assumed 1; v2 supports multi-quantity).</summary>
    public int Quantity { get; set; } = 1;

    public string? Note { get; set; }

    /// <summary>True if the customer already paid this line up-front. Legacy: PrePayed.</summary>
    public bool PrePaid { get; set; }

    /// <summary>Note about how the line was pre-paid (deposit reference, etc.). Legacy: NotePrePayed.</summary>
    public string? PrePaidNote { get; set; }

    /// <summary>The CustomerDebt row this line settles (Phase 3). Nullable for ad-hoc
    /// sales that aren't backed by a pre-recorded debt. Legacy: IdCustomerFinancialState.</summary>
    public long? CustomerDebtId { get; set; }

    public bool Active { get; set; } = true;
}
