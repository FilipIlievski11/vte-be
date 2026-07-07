namespace VTE.Domain.Payments;

/// <summary>
/// Категорија на наплата (Јавни патишта, Комунална такса, Црвен крст, Технички
/// преглед, …) — the billing dimension <see cref="PriceCatalog.PaymentCategoryGroupId"/>
/// points at (that column predates this table and stays a loose int: legacy data
/// contains ids that were deleted upstream). Each category resolves WHERE its money
/// goes via <see cref="CalculationItemId"/>.
///
/// Legacy source: <c>PaymentCategories</c> (ids preserved 1:1, including the per-company
/// duplicates — e.g. 86 vs 1086 are the same name for different legacy companies;
/// <see cref="CompanyScope"/> keeps that legacy IdCompany for display). Imported once by
/// <c>migrate/import-billing-categories.sql</c>; v2 is the master afterwards.
/// </summary>
public class PaymentCategoryGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>Каде одат парите — the уплатна сметка of this category. Legacy: IdCalculationItem.</summary>
    public int? CalculationItemId { get; set; }

    /// <summary>Legacy IdCompany (0 = global). Informational — lookup is global in v2.</summary>
    public int CompanyScope { get; set; }

    /// <summary>Legacy VisibleOrder — display ordering in the legacy UI.</summary>
    public int? VisibleOrder { get; set; }

    public bool Active { get; set; } = true;
}
