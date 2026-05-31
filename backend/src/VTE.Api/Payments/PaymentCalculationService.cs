using System.Globalization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Payments;
using VTE.Domain.Vehicles;
using VTE.Infrastructure;

namespace VTE.Api.Payments;

// Ports the legacy `PaymentCataologList.GetPaymentForDepts(trigerdBy, vehicle)` flow:
//   1. Walk every PaymentCategory whose corresponding TrigerdBy* flag is set.
//   2. For each category find the matching PaymentItem (by VehicleCategoryForPaymentsId
//      or fall back to a category-only item).
//   3. If the item has PaymentItemParametars rows:
//        - VehicleField=null  -> fixed price, always applies
//        - VehicleField=X     -> read Vehicle.X via reflection and pick the parametar row
//                                whose [From, To] range contains the value.
//      Optional parametars (IsOptional=true) are returned for manual selection only.
//   4. Resolve DDV rate from PaymentCategory.DDVId → DDVCatalog.
// Returns a list of proposed PaymentDocumentDetail rows; the controller persists them
// when the user clicks "Save" on the payment form.
public enum PaymentTrigger { Request, TechnicalExam, TrafficLicence, PermissionForVehicle, InternationalDriverLicence, IrregularTechnicalExam }

public sealed record CalculationProposedLine(
    int PaymentCategoryId, string CategoryName,
    int PaymentItemId, string ItemName,
    int? PaymentItemParametarId, string? ParametarName,
    decimal Price, decimal DDVRate,
    bool AllowDiscount, bool IsOptional,
    string? MatchedVehicleField, decimal? MatchedVehicleValue);

public sealed record CalculationResult(
    long VehicleId, int? VehicleCategoryForPaymentsId,
    List<CalculationProposedLine> Lines, List<string> Warnings);

public sealed class PaymentCalculationService
{
    private readonly VteDbContext _db;
    public PaymentCalculationService(VteDbContext db) => _db = db;

    public async Task<CalculationResult> CalculateForVehicleAsync(long vehicleId, PaymentTrigger trigger, CancellationToken ct = default)
    {
        var warnings = new List<string>();
        var lines = new List<CalculationProposedLine>();

        var vehicle = await _db.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Id == vehicleId, ct);
        if (vehicle is null)
        {
            warnings.Add($"Vehicle id={vehicleId} not found.");
            return new CalculationResult(vehicleId, null, lines, warnings);
        }

        var categories = await CategoriesForTriggerAsync(trigger, ct);
        var ddvRates = await _db.DDVCatalogs.AsNoTracking().Where(d => d.IsActive)
            .ToDictionaryAsync(d => d.Id, d => d.Rate, ct);

        foreach (var cat in categories)
        {
            // Prefer the item whose VehicleCategoryForPayments matches the vehicle.
            // Fall back to a category-only item (VehicleCategoryForPaymentsId IS NULL).
            var item = await _db.PaymentItems.AsNoTracking()
                .Where(i => i.IsActive && i.PaymentCategoryId == cat.Id &&
                       (i.VehicleCategoryForPaymentsId == vehicle.VehicleCategoryForPaymentsId
                        || i.VehicleCategoryForPaymentsId == null))
                .OrderByDescending(i => i.VehicleCategoryForPaymentsId != null)
                .FirstOrDefaultAsync(ct);
            if (item is null) continue;

            var parametars = await _db.PaymentItemParametars.AsNoTracking()
                .Where(p => p.IsActive && p.PaymentItemId == item.Id)
                .ToListAsync(ct);

            var ddvRate = cat.DDVId.HasValue && ddvRates.TryGetValue(cat.DDVId.Value, out var r) ? r : 0m;

            if (parametars.Count == 0)
            {
                warnings.Add($"Category '{cat.CategoryName}' item '{item.ItemName}' has no parametars — skipped.");
                continue;
            }

            foreach (var p in parametars)
            {
                if (p.IsOptional)
                {
                    lines.Add(new CalculationProposedLine(cat.Id, cat.CategoryName, item.Id, item.ItemName,
                        p.Id, p.ParametarName, p.Price, ddvRate, cat.AllowDiscount, true,
                        p.VehicleField, null));
                    continue;
                }

                // Fixed-price (no VehicleField) → always applies
                if (string.IsNullOrWhiteSpace(p.VehicleField) || string.Equals(p.VehicleField, "Null", StringComparison.OrdinalIgnoreCase))
                {
                    lines.Add(new CalculationProposedLine(cat.Id, cat.CategoryName, item.Id, item.ItemName,
                        p.Id, p.ParametarName, p.Price, ddvRate, cat.AllowDiscount, false,
                        null, null));
                    continue;
                }

                // Range match against a Vehicle property
                var value = ReadVehicleField(vehicle, p.VehicleField);
                if (value is null) continue;
                if (InRange(value.Value, p.ParametarFrom, p.ParametarTo))
                {
                    lines.Add(new CalculationProposedLine(cat.Id, cat.CategoryName, item.Id, item.ItemName,
                        p.Id, p.ParametarName, p.Price, ddvRate, cat.AllowDiscount, false,
                        p.VehicleField, value));
                }
            }
        }

        return new CalculationResult(vehicleId, vehicle.VehicleCategoryForPaymentsId, lines, warnings);
    }

    private Task<List<PaymentCategory>> CategoriesForTriggerAsync(PaymentTrigger trigger, CancellationToken ct)
    {
        IQueryable<PaymentCategory> q = _db.PaymentCategories.AsNoTracking().Where(c => c.IsActive);
        q = trigger switch
        {
            PaymentTrigger.Request                    => q.Where(c => c.TriggerdByRequest),
            PaymentTrigger.TechnicalExam              => q.Where(c => c.TriggerdByTechnicalExam),
            PaymentTrigger.TrafficLicence             => q.Where(c => c.TriggerdByTrafficLicence),
            PaymentTrigger.PermissionForVehicle       => q.Where(c => c.TriggerdByPermissionForVehicle),
            PaymentTrigger.InternationalDriverLicence => q.Where(c => c.TriggerdByInternationalDriverLicence),
            PaymentTrigger.IrregularTechnicalExam     => q.Where(c => c.TriggerdByIrregularTechnicalExam),
            _ => q.Where(c => false),
        };
        return q.OrderBy(c => c.VisibleOrder ?? 0).ThenBy(c => c.CategoryName).ToListAsync(ct);
    }

    // Reflection over the Vehicle entity. The legacy app used VB CallByName.
    // Accepts numeric, decimal, int, short, double, float properties; date/string
    // properties produce null (no range match possible).
    private static decimal? ReadVehicleField(Vehicle v, string field)
    {
        var prop = typeof(Vehicle).GetProperty(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (prop is null) return null;
        var raw = prop.GetValue(v);
        if (raw is null) return null;
        return raw switch
        {
            decimal d => d,
            double dd => (decimal)dd,
            float f   => (decimal)f,
            int i     => i,
            short s   => s,
            long l    => l,
            bool b    => b ? 1m : 0m,
            string str when decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var p) => p,
            _ => null,
        };
    }

    private static bool InRange(decimal value, decimal? from, decimal? to)
    {
        if (from.HasValue && value < from.Value) return false;
        if (to.HasValue   && value > to.Value)   return false;
        return true;
    }
}
