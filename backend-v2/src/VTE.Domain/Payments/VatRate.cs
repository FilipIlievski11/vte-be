namespace VTE.Domain.Payments;

/// <summary>
/// Macedonian VAT rate catalog. Legacy: DDVCatalog (3 rows: 0%, 5%, 18%).
/// Cross-tenant — the rates apply company-wide.
/// </summary>
public class VatRate
{
    public int Id { get; set; }

    /// <summary>Short code for the rate (e.g. "0", "5", "18"). Legacy: not present; introduced for typed lookups.</summary>
    public string? Code { get; set; }

    /// <summary>Display name (e.g. "ДДВ 18%"). Legacy: Name.</summary>
    public string Name { get; set; } = "";

    /// <summary>Percent value, e.g. 18.0. Legacy: DDVValue (real).</summary>
    public double Percent { get; set; }

    public bool Active { get; set; } = true;
}
