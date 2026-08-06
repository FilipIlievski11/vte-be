namespace VTE.Infrastructure.Pricing;

/// <summary>
/// Whole-denar rounding shared by pricing and billing. Same semantics as the legacy
/// VB RoundHelper.FicalRound (the rule the fiscal printer applies): the fraction goes
/// DOWN at ≤ 0.49 and UP from 0.50 — e.g. 842.52 → 843, 842.49 → 842, 66.10 → 66.
/// Applied when CREATING v2-native debts so the Наплата panel, the bill and the
/// fiscal receipt all show the same whole-denar amount (operators were rounding by
/// hand — audit 2026-08-05, debt 10288). Mirrored legacy debts stay as synced.
/// </summary>
public static class MoneyRounding
{
    public static decimal FicalRound(decimal value)
    {
        var whole = decimal.Truncate(value);
        var frac = decimal.Round(value - whole, 2);
        if (frac >= 0 && frac <= 0.49m) return whole;
        if (frac > 0.49m) return whole + 1;
        return 0; // negative fractions fell through the legacy Select Case → 0
    }
}
