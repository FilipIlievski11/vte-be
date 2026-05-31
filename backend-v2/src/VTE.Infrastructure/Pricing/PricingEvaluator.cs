using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Payments;
using VTE.Domain.Vehicles;
using VTE.Infrastructure.Persistence;

namespace VTE.Infrastructure.Pricing;

/// <summary>
/// Default <see cref="IPricingEvaluator"/> implementation.
///
/// Algorithm (mirrors legacy PaymentCataologList.GetPaymentForDepts, lines 40-86):
///   1. Pull all Active PriceCatalog rows whose Trigger matches the workflow
///      AND whose VehiclePaymentCategoryId matches the vehicle (or is NULL).
///   2. Filter by CommunityId match (or NULL = any).
///   3. For each candidate:
///        - VehicleField is NULL  → fixed fee, always passes.
///        - VehicleField is set   → read that property off the Vehicle via reflection
///          and check ParametarFrom ≤ value ≤ ParametarTo.
///   4. If the filtered set is empty AND we had a vehicle, fall back to "sentinel"
///      rules (ParametarFrom == 0 AND ParametarTo == 0) — legacy default tier.
///   5. Snapshot the VAT % from the linked VatRate.
/// </summary>
public class PricingEvaluator : IPricingEvaluator
{
    private readonly VteDbContext _db;

    public PricingEvaluator(VteDbContext db) => _db = db;

    /// <summary>Reflection cache: Vehicle property name → getter.</summary>
    private static readonly Dictionary<string, PropertyInfo?> _vehicleFieldCache = new(StringComparer.OrdinalIgnoreCase);
    private static PropertyInfo? GetVehicleField(string name)
    {
        if (_vehicleFieldCache.TryGetValue(name, out var pi)) return pi;
        pi = typeof(Vehicle).GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        _vehicleFieldCache[name] = pi;
        return pi;
    }

    public async Task<IReadOnlyList<MatchedPrice>> EvaluateAsync(
        PriceTrigger trigger,
        Vehicle? vehicle,
        int? communityId,
        byte? companyId,
        CancellationToken ct = default)
    {
        if (trigger == PriceTrigger.None) return Array.Empty<MatchedPrice>();

        // Step 1: candidate set (DB-side filter).
        //   - Trigger must match the workflow that fired.
        //   - VehiclePaymentCategoryId NULL means "any vehicle category".
        //   - CommunityId / PriceCompanyId NULL means "applies to every scope" — otherwise must match.
        //     PriceCompanyId is critical: without it, every legacy company's "Operating fee" rule
        //     fires at once and we get duplicate debt lines.
        var vehicleCategoryId = vehicle?.PaymentCategoryId;
        var candidates = await _db.PriceCatalogs.AsNoTracking()
            .Where(p => p.Active && p.Trigger == trigger
                     && (p.VehiclePaymentCategoryId == null
                         || p.VehiclePaymentCategoryId == vehicleCategoryId)
                     && (p.CommunityId == null || p.CommunityId == communityId)
                     && (p.PriceCompanyId == null || p.PriceCompanyId == companyId))
            .Select(p => new {
                p.Id, p.BasePrice, p.VatRateId, p.VehicleField, p.ParametarFrom, p.ParametarTo
            })
            .ToListAsync(ct);

        // Step 2: filter ranged rules client-side (need reflection over Vehicle).
        var matched = new List<(int Id, decimal Price, int VatRateId)>();
        var fallback = new List<(int Id, decimal Price, int VatRateId)>();

        foreach (var c in candidates)
        {
            if (string.IsNullOrWhiteSpace(c.VehicleField))
            {
                // Fixed-fee rule — always applies.
                matched.Add((c.Id, c.BasePrice, c.VatRateId));
                continue;
            }
            if (vehicle is null) continue; // ranged rule without a vehicle — skip.

            // Legacy sentinel: ParametarFrom=0 AND ParametarTo=0 means "default tier fallback".
            var isSentinel = c.ParametarFrom is null or 0 && c.ParametarTo is null or 0;

            var prop = GetVehicleField(c.VehicleField);
            if (prop is null) continue; // unknown property — skip rather than throw.

            double? value = TryReadAsDouble(prop.GetValue(vehicle));
            if (value is null)
            {
                if (isSentinel) fallback.Add((c.Id, c.BasePrice, c.VatRateId));
                continue;
            }

            var from = c.ParametarFrom ?? double.MinValue;
            var to   = c.ParametarTo   ?? double.MaxValue;
            if (value >= from && value <= to)
            {
                if (isSentinel) fallback.Add((c.Id, c.BasePrice, c.VatRateId));
                else            matched.Add((c.Id, c.BasePrice, c.VatRateId));
            }
        }

        // Step 4: use the explicit matches; fall back to sentinels only if nothing matched.
        var winners = matched.Count > 0 ? matched : fallback;
        if (winners.Count == 0) return Array.Empty<MatchedPrice>();

        // Step 5: snapshot VAT % for each.
        var vatIds = winners.Select(w => w.VatRateId).Distinct().ToList();
        var vatMap = await _db.VatRates.AsNoTracking()
            .Where(v => vatIds.Contains(v.Id))
            .ToDictionaryAsync(v => v.Id, v => v.Percent, ct);

        return winners.Select(w =>
            new MatchedPrice(w.Id, w.Price, vatMap.GetValueOrDefault(w.VatRateId, 0)))
            .ToList();
    }

    private static double? TryReadAsDouble(object? raw) => raw switch
    {
        null    => null,
        double d => d,
        float f  => f,
        decimal m => (double)m,
        int i    => i,
        long l   => l,
        short s  => s,
        byte b   => b,
        bool bo  => bo ? 1.0 : 0.0,
        _        => double.TryParse(raw.ToString(), out var v) ? v : null,
    };
}
