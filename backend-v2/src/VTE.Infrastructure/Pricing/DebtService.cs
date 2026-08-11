using Microsoft.EntityFrameworkCore;
using VTE.Domain.Payments;
using VTE.Domain.Vehicles;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Infrastructure.Pricing;

public class DebtService : IDebtService
{
    private readonly VteDbContext _db;
    private readonly IPricingEvaluator _evaluator;
    private readonly ITenantContext _tenant;

    public DebtService(VteDbContext db, IPricingEvaluator evaluator, ITenantContext tenant)
    {
        _db = db;
        _evaluator = evaluator;
        _tenant = tenant;
    }

    /// <summary>Групите „Технички преглед" (легаси PaymentCategories 11/53/1011) —
    /// само нивните ставки се скалираат со PercentOfFullExam (легаси
    /// AddDeptsToCustomer: PaymentName почнува со името на категоријата).
    /// List, не int[] — на .NET 10 array.Contains се врзува за span-overload
    /// што EF funcletizer-от не може да го евалуира (TypeLoadException).</summary>
    private static readonly List<int> TechExamFeeGroups = new() { 11, 53, 1011 };

    public async Task<int> CreateDebtsForSourceAsync(
        DebtOrigin origin,
        long originId,
        long customerVehicleRelationId,
        int organizationId,
        PriceTrigger trigger,
        int? communityId = null,
        string? note = null,
        int? techExamScalePercent = null,
        CancellationToken ct = default)
    {
        // Легаси: `If delitel > 0` — тип со 0% (АТЕСТ, ПОВТ-РЕГ, ИЗД-ГАСОВИ,
        // ОСЛОБОДЕН) не создава никакви долгови.
        if (techExamScalePercent is <= 0) return 0;

        // Idempotency: if any debt already exists for this source, skip.
        // (We don't try to reconcile changes — that's a Phase 4+ concern.)
        var isRequest    = origin == DebtOrigin.Request;
        var isTechExam   = origin == DebtOrigin.TechnicalExam || origin == DebtOrigin.TechnicalExamIrregular;
        var isIdl        = origin == DebtOrigin.InternationalDrivingLicence;
        var isPermission = origin == DebtOrigin.Permission;
        var existing = await _db.CustomerDebts.AsNoTracking().AnyAsync(d =>
            d.Origin == origin
            && ((isRequest    && d.OriginRequestId                       == originId)
             || (isTechExam   && d.OriginTechnicalExamId                 == originId)
             || (isIdl        && d.OriginInternationalDrivingLicenceId   == originId)
             || (isPermission && d.OriginPermissionId                    == originId)),
            ct);
        if (existing) return 0;

        // Load the relation + its vehicle to feed the evaluator.
        var rel = await _db.ClientVehicleRelations.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == customerVehicleRelationId, ct);
        if (rel is null) return 0;

        Vehicle? vehicle = null;
        if (rel.VehicleId.HasValue)
            vehicle = await _db.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Id == rel.VehicleId.Value, ct);

        var companyId = _tenant.CompanyId ?? (byte)4;
        var matches = await _evaluator.EvaluateAsync(trigger, vehicle, communityId, companyId, ct);
        if (matches.Count == 0) return 0;
        var userId = _tenant.UserId;
        var now = DateTime.UtcNow;

        // Легаси pomDelitel: процентот важи САМО за ставките од категоријата
        // „Технички преглед"; останатите (ако некогаш се појават во иста
        // евалуација) одат со полна цена.
        var scaledCatalogIds = new HashSet<int>();
        if (techExamScalePercent is int pct && pct != 100)
        {
            var ids = matches.Select(m => m.PriceCatalogId).ToList();
            scaledCatalogIds = (await _db.PriceCatalogs.AsNoTracking()
                .Where(p => ids.Contains(p.Id)
                         && p.PaymentCategoryGroupId != null
                         && TechExamFeeGroups.Contains(p.PaymentCategoryGroupId.Value))
                .Select(p => p.Id)
                .ToListAsync(ct)).ToHashSet();
        }

        foreach (var m in matches)
        {
            var price = scaledCatalogIds.Contains(m.PriceCatalogId)
                ? m.Price * techExamScalePercent!.Value / 100m
                : m.Price;
            _db.CustomerDebts.Add(new CustomerDebt
            {
                CompanyId = companyId,
                CustomerVehicleRelationId = customerVehicleRelationId,
                PriceCatalogId = m.PriceCatalogId,
                // Whole denars from day one (легаси фискално правило: ≤.49 ↓, ≥.50 ↑) —
                // operators were rounding 842.52 → 843 by hand before billing.
                Price = MoneyRounding.FicalRound(price),
                VatPercent = m.VatPercent,
                Note = note,
                Origin = origin,
                OriginRequestId                     = origin == DebtOrigin.Request ? originId : null,
                OriginTechnicalExamId               = origin is DebtOrigin.TechnicalExam or DebtOrigin.TechnicalExamIrregular ? originId : null,
                OriginInternationalDrivingLicenceId = origin == DebtOrigin.InternationalDrivingLicence ? originId : null,
                OriginPermissionId                  = origin == DebtOrigin.Permission ? originId : null,
                OrganizationId = organizationId,
                Paid = false,
                CreatedAt = now,
                CreatedByUserId = userId,
                Active = true,
            });
        }
        await _db.SaveChangesAsync(ct);
        return matches.Count;
    }
}
