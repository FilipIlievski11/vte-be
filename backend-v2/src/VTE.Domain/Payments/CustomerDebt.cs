using VTE.Domain.Common;

namespace VTE.Domain.Payments;

/// <summary>
/// An open debt against a customer–vehicle relation. Created automatically when a
/// source document (Request, TechnicalExamReport, …) completes and the pricing
/// evaluator finds catalogue rules that apply to the vehicle. Picked up later by
/// the "create bill" flow which collects unpaid debts into a PaymentDocument.
///
/// Legacy source: <c>CustomerFinancialState</c>.
/// </summary>
public class CustomerDebt : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    /// <summary>Anchor: who owes for what vehicle. Legacy: IdCustomerVehicleRelation.</summary>
    public long CustomerVehicleRelationId { get; set; }

    /// <summary>What is owed (the fee rule that triggered). Legacy: IdPriceCatalog.</summary>
    public int PriceCatalogId { get; set; }

    /// <summary>Snapshot of the fee price at the moment the debt was created
    /// (catalogue can change later without retroactively affecting this debt).
    /// Legacy: Price.</summary>
    public decimal Price { get; set; }

    /// <summary>Snapshot of the VAT % at creation time.</summary>
    public double VatPercent { get; set; }

    public string? Note { get; set; }

    /// <summary>Which workflow created this debt.</summary>
    public DebtOrigin Origin { get; set; }

    /// <summary>If Origin = Request, the request id. Legacy: IdDocument.</summary>
    public long? OriginRequestId { get; set; }

    /// <summary>If Origin = TechnicalExam(Irregular), the tech-exam id. Legacy: IdDocumentTehnicalExam.</summary>
    public long? OriginTechnicalExamId { get; set; }

    /// <summary>Station/organization that owns this debt. Legacy: IdOrganization.</summary>
    public int OrganizationId { get; set; }

    /// <summary>True once the debt is fully settled by a PaymentDocumentLine.</summary>
    public bool Paid { get; set; }

    /// <summary>FK to the <see cref="PaymentDocumentLine"/> that settled this debt
    /// (when paid). Null while unpaid.</summary>
    public long? SettledByLineId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedByUserId { get; set; }
    public bool Active { get; set; } = true;
}
