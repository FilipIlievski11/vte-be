using VTE.Domain.Payments;
using VTE.Domain.Vehicles;

namespace VTE.Infrastructure.Pricing;

/// <summary>
/// Replicates the legacy <c>PaymentCataologList.GetPaymentForDepts</c> logic:
/// given a triggering workflow + the vehicle being processed, return every
/// PriceCatalog row whose rule applies (right trigger, right vehicle category,
/// vehicle property within the parametar range, etc.).
/// </summary>
public interface IPricingEvaluator
{
    /// <summary>
    /// Find every PriceCatalog rule that matches the given vehicle for the
    /// given workflow trigger. Stateless / pure: doesn't touch CustomerDebt.
    /// </summary>
    /// <param name="trigger">The workflow that's causing the evaluation.</param>
    /// <param name="vehicle">The vehicle being charged (nullable for fixed-fee-only rules).</param>
    /// <param name="communityId">Optional municipality scope for community-specific fees.</param>
    Task<IReadOnlyList<MatchedPrice>> EvaluateAsync(
        PriceTrigger trigger,
        Vehicle? vehicle,
        int? communityId,
        byte? companyId,
        CancellationToken ct = default);
}

/// <summary>A matched PriceCatalog row + the price snapshot to use.</summary>
public record MatchedPrice(int PriceCatalogId, decimal Price, double VatPercent);
