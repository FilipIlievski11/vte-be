namespace VTE.Domain.Payments;

/// <summary>
/// A single billable fee. Legacy stores pricing across 5 tables (PriceCatalog,
/// PaymentItems, PaymentCategories, PaymentItemParametars, VehicleCategoryForPayments);
/// v2 flattens to one row per fee with optional vehicle-category filter and trigger.
///
/// Cross-tenant catalog — fees are statutory in Macedonia, shared across companies.
/// (Company-specific overrides can be added later as a child table without changing
/// this entity.)
/// </summary>
public class PriceCatalog
{
    public int Id { get; set; }

    /// <summary>Stable short code (e.g. "TEH-IPR" for technical-exam pass).</summary>
    public string? Code { get; set; }

    /// <summary>Human label. Legacy: Name.</summary>
    public string Name { get; set; } = "";

    /// <summary>Base price (before discount + VAT). Legacy: Price (money).</summary>
    public decimal BasePrice { get; set; }

    /// <summary>VAT rate applied. Legacy: IdDDVCatalog.</summary>
    public int VatRateId { get; set; }

    /// <summary>Which workflow auto-creates a debt with this fee — replaces the legacy
    /// boolean cluster IsRequest/IsTehnicalExam/IsTrafficLicence/IsPermision/IsIDL.</summary>
    public PriceTrigger Trigger { get; set; }

    // ---- Rule-evaluator columns (added in Phase 3) — mirror legacy PaymentItemParametars ----

    /// <summary>Vehicle-payment-category this fee applies to (loose ref, no FK).
    /// Legacy: PaymentItems.IdVehicleCategoryForPayments. NULL = any.</summary>
    public int? VehiclePaymentCategoryId { get; set; }

    /// <summary>Optional municipality scope. Legacy: PaymentCategories.IdCommunity. NULL = any.</summary>
    public int? CommunityId { get; set; }

    /// <summary>Optional tenant scope. Legacy: PaymentCategories.IdCompany. NULL = applies to
    /// every company. Without this, multiple companies' "Operating fee" rules all fire at
    /// once for the same vehicle, producing duplicate debt lines.</summary>
    public byte? PriceCompanyId { get; set; }

    /// <summary>Forensic grouping: legacy PaymentCategories.Id (the broad fee class).</summary>
    public int? PaymentCategoryGroupId { get; set; }

    /// <summary>For ranged rules, the Vehicle property name to read at eval-time
    /// (e.g. "EngineWorkingCapacityCc", "AxleCount"). Legacy: PaymentItemParametars.VehicleField.
    /// NULL = fixed-fee rule (always applies once category matches).</summary>
    public string? VehicleField { get; set; }

    /// <summary>Lower bound (inclusive) on the vehicle property. Legacy: ParametarFrom.</summary>
    public double? ParametarFrom { get; set; }

    /// <summary>Upper bound (inclusive) on the vehicle property. Legacy: ParametarTo.</summary>
    public double? ParametarTo { get; set; }

    // ---- Second rule dimension: vehicle AGE (Сл. весник 89/2022 eco-fee tariff is
    // категорија × СТАРОСТ × зафатнина — the legacy model couldn't express two ranges,
    // which is why operators typed the eco fee manually). NULL/NULL = no age condition.

    /// <summary>Minimum vehicle age in years (inclusive), from ManufactureDate. NULL = 0.</summary>
    public int? AgeFrom { get; set; }

    /// <summary>Maximum vehicle age in years (inclusive). NULL = unbounded („над 30").</summary>
    public int? AgeTo { get; set; }

    /// <summary>Optional CSV of <see cref="VehicleCategory"/> codes the fee applies to
    /// (empty = all). Legacy used VehicleCategoryForPayments join table.</summary>
    public string? VehicleCategoryFilter { get; set; }

    /// <summary>Bank account fees of this kind are routed to. Legacy: CalculationItems.BankAccount.</summary>
    public string? BankAccount { get; set; }

    /// <summary>Bank payment-form code (e.g. "PP30"). Legacy: CalculationItems.Form.</summary>
    public string? PaymentForm { get; set; }

    public bool Active { get; set; } = true;
}
