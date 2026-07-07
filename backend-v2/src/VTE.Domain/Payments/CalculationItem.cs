namespace VTE.Domain.Payments;

/// <summary>
/// Уплатна сметка — WHERE the money of a billing category goes: the account number,
/// the bank / уплатна-сметка description and the payment-order form (ПП30/ПП50).
/// Referenced by <see cref="PaymentCategoryGroup.CalculationItemId"/>.
///
/// Legacy source: <c>CalculationItems</c> (13 rows, ids preserved). Imported once by
/// <c>migrate/import-billing-categories.sql</c>; v2 is the master afterwards — the
/// legacy sync intentionally does NOT touch these (admin edits would be overwritten).
/// </summary>
public class CalculationItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>Account number the money is paid into. Legacy: BankAccount.</summary>
    public string? BankAccount { get; set; }

    /// <summary>Bank / уплатна сметка + приходна шифра description. Legacy: Bank.</summary>
    public string? Bank { get; set; }

    /// <summary>Payment-order form (ПП30 / ПП50). Legacy: Form.</summary>
    public string? Form { get; set; }

    /// <summary>The station's OWN account — money collected into it stays with the
    /// station instead of being forwarded to an outside institution. Auto-detected at
    /// import (name matches a Company), admin-editable. Drives the daily distribution
    /// report's „останува за станицата" split.</summary>
    public bool IsOwnAccount { get; set; }

    public bool Active { get; set; } = true;
}
