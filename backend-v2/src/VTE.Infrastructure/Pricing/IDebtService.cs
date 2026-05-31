using VTE.Domain.Payments;

namespace VTE.Infrastructure.Pricing;

/// <summary>
/// Persists <see cref="CustomerDebt"/> rows from a triggering workflow.
/// Wraps the evaluator + EF insert in one transactional unit, mirroring the
/// legacy <c>AddDeptsToCustomer</c> path that fired on every document save.
/// </summary>
public interface IDebtService
{
    /// <summary>
    /// Create one debt row per matched PriceCatalog rule. Idempotent for the
    /// same (origin, originId) pair — skips if any unpaid debt already exists
    /// for that source document.
    /// </summary>
    /// <returns>How many debt rows were inserted.</returns>
    Task<int> CreateDebtsForSourceAsync(
        DebtOrigin origin,
        long originId,
        long customerVehicleRelationId,
        int organizationId,
        PriceTrigger trigger,
        int? communityId = null,
        string? note = null,
        CancellationToken ct = default);
}
