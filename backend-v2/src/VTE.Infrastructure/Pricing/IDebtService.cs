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
    /// <param name="techExamScalePercent">
    /// Legacy <c>PercentOfFullExam</c> of the exam type, for non-РЕД-12М exams
    /// (типови &gt; 1): ≤ 0 создава НИШТО (пр. АТЕСТ/ОСЛОБОДЕН = бесплатно),
    /// инаку ставките од категоријата „Технички преглед" (групи 11/53/1011) се
    /// множат со процентот/100 (пр. ВОНРЕДЕН 70% → 1700 → 1190). Останатите
    /// ставки не се скалираат. Null = без скалирање (редовен преглед/друг извор).
    /// </param>
    /// <returns>How many debt rows were inserted.</returns>
    Task<int> CreateDebtsForSourceAsync(
        DebtOrigin origin,
        long originId,
        long customerVehicleRelationId,
        int organizationId,
        PriceTrigger trigger,
        int? communityId = null,
        string? note = null,
        int? techExamScalePercent = null,
        CancellationToken ct = default);
}
