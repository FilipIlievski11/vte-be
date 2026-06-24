using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

/// <summary>
/// Read API over <see cref="VTE.Domain.Payments.CustomerDebt"/> — the v2 mirror of
/// legacy <c>CustomerFinancialState</c>. Backs the dashboard "Наплата" panel:
/// open debts grouped by client → vehicle.
///
/// Phase 2 will add POST / settle endpoints; for now read-only.
/// </summary>
[ApiController]
[Route("api/customer-debts")]
[Authorize]
public class CustomerDebtsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public CustomerDebtsController(VteDbContext db, ITenantContext tenant)
    {
        _db = db;
        _tenant = tenant;
    }

    public record CustomerDebtRow(
        long Id, byte CompanyId,
        long CustomerVehicleRelationId,
        string? ClientName, string? ClientMB,
        long? VehicleId, string? VehiclePlate, string? VehicleVin, string? VehicleMakerModel,
        int PriceCatalogId, string? PriceCatalogName,
        decimal Price, double VatPercent,
        string? Note,
        byte Origin, long? OriginRequestId, long? OriginTechnicalExamId,
        int OrganizationId,
        bool Paid, long? SettledByLineId,
        DateTime CreatedAt);

    /// <summary>Substitute the {0}/{1} parametar-range placeholders in a legacy price-item
    /// name with the rule's actual ParametarFrom/ParametarTo (rendered as whole numbers).
    /// Uses Replace, not String.Format, so stray braces in the name can't throw.</summary>
    private static string? FormatPriceName(string? name, double? from, double? to)
    {
        if (string.IsNullOrEmpty(name)) return name;
        if (name.Contains("{0}")) name = name.Replace("{0}", from.HasValue ? ((long)from.Value).ToString() : "");
        if (name.Contains("{1}")) name = name.Replace("{1}", to.HasValue ? ((long)to.Value).ToString() : "");
        return name;
    }

    /// <summary>
    /// Paged list of debts. Defaults to unpaid + active (the "Наплата" view).
    /// Sorted by client → vehicle → debt id so the frontend can group adjacent rows
    /// in a single pass without re-sorting.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedDto<CustomerDebtRow>>> List(
        [FromQuery] long? customerVehicleRelationId = null,
        [FromQuery] bool unpaidOnly = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 100)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 500) pageSize = 100;

        var query = _db.CustomerDebts.AsNoTracking().Where(d => d.Active);
        if (unpaidOnly) query = query.Where(d => !d.Paid);
        if (customerVehicleRelationId.HasValue)
            query = query.Where(d => d.CustomerVehicleRelationId == customerVehicleRelationId.Value);

        var total = await query.CountAsync();

        var pageRows = await query
            .OrderBy(d => d.CustomerVehicleRelationId)
            .ThenBy(d => d.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new
            {
                d.Id, d.CompanyId, d.CustomerVehicleRelationId, d.PriceCatalogId,
                d.Price, d.VatPercent, d.Note,
                d.Origin, d.OriginRequestId, d.OriginTechnicalExamId,
                d.OrganizationId, d.Paid, d.SettledByLineId, d.CreatedAt,
            })
            .ToListAsync();

        // Batch-resolve joins (relation → client + vehicle, price-catalog name).
        var relationIds = pageRows.Select(r => r.CustomerVehicleRelationId).Distinct().ToList();
        var priceCatalogIds = pageRows.Select(r => r.PriceCatalogId).Distinct().ToList();

        var relations = await _db.ClientVehicleRelations.AsNoTracking()
            .Where(r => relationIds.Contains(r.Id))
            .Select(r => new { r.Id, r.ClientId, r.VehicleId })
            .ToDictionaryAsync(r => r.Id);
        var clientIds = relations.Values.Select(r => r.ClientId).Distinct().ToList();
        var vehicleIds = relations.Values.Where(r => r.VehicleId.HasValue).Select(r => r.VehicleId!.Value).Distinct().ToList();

        var clients = await _db.Clients.AsNoTracking()
            .Where(c => clientIds.Contains(c.Id))
            .Select(c => new { c.Id, c.FirstName, c.MiddleName, c.LastName, c.MB })
            .ToDictionaryAsync(c => c.Id);
        var vehicles = await _db.Vehicles.AsNoTracking()
            .Where(v => vehicleIds.Contains(v.Id))
            .Select(v => new { v.Id, v.Vin, v.Plate, v.ModelId })
            .ToDictionaryAsync(v => v.Id);

        // Maker/model labels (lazy two-step join — most pages have <50 vehicles so it stays cheap).
        var modelIds = vehicles.Values.Where(v => v.ModelId.HasValue).Select(v => v.ModelId!.Value).Distinct().ToList();
        var models = await _db.VehicleModels.AsNoTracking()
            .Where(m => modelIds.Contains(m.Id))
            .Select(m => new { m.Id, m.Name, m.MakerId })
            .ToDictionaryAsync(m => m.Id);
        var makerIds = models.Values.Select(m => m.MakerId).Distinct().ToList();
        var makers = await _db.VehicleMakers.AsNoTracking()
            .Where(m => makerIds.Contains(m.Id))
            .ToDictionaryAsync(m => m.Id, m => m.Name);

        // Resolve the displayed price name. Legacy item names are String.Format templates
        // with {0}/{1} placeholders for the matched parametar range (e.g.
        // "за носивост од {0} до {1}"). Substitute the rule's ParametarFrom/ParametarTo so
        // the Наплата panel shows the real range ("…од 3001 до 5000"), like the legacy app.
        var priceCatalog = (await _db.PriceCatalogs.AsNoTracking()
            .Where(p => priceCatalogIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Name, p.ParametarFrom, p.ParametarTo })
            .ToListAsync())
            .ToDictionary(p => p.Id, p => FormatPriceName(p.Name, p.ParametarFrom, p.ParametarTo));

        string? makerModel(long vehicleId)
        {
            if (!vehicles.TryGetValue(vehicleId, out var v) || !v.ModelId.HasValue) return null;
            if (!models.TryGetValue(v.ModelId.Value, out var m)) return null;
            var maker = makers.GetValueOrDefault(m.MakerId);
            var label = string.Join(' ', new[] { maker, m.Name }.Where(x => !string.IsNullOrWhiteSpace(x)));
            return string.IsNullOrWhiteSpace(label) ? null : label;
        }
        string? clientName(long cid) =>
            clients.TryGetValue(cid, out var c)
                ? (string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }.Where(x => !string.IsNullOrWhiteSpace(x))) is var n && n.Length > 0 ? n : null)
                : null;

        var items = pageRows.Select(r =>
        {
            string? cName = null, cMB = null, plate = null, vin = null, mm = null;
            long? vId = null;
            if (relations.TryGetValue(r.CustomerVehicleRelationId, out var rel))
            {
                if (clients.TryGetValue(rel.ClientId, out var c)) { cName = clientName(rel.ClientId); cMB = c.MB; }
                vId = rel.VehicleId;
                if (rel.VehicleId.HasValue && vehicles.TryGetValue(rel.VehicleId.Value, out var v))
                {
                    plate = v.Plate; vin = v.Vin; mm = makerModel(v.Id);
                }
            }
            return new CustomerDebtRow(
                r.Id, r.CompanyId,
                r.CustomerVehicleRelationId, cName, cMB,
                vId, plate, vin, mm,
                r.PriceCatalogId, priceCatalog.GetValueOrDefault(r.PriceCatalogId),
                r.Price, r.VatPercent, r.Note,
                (byte)r.Origin, r.OriginRequestId, r.OriginTechnicalExamId,
                r.OrganizationId, r.Paid, r.SettledByLineId, r.CreatedAt);
        }).ToList();

        return Ok(new PagedDto<CustomerDebtRow>(page, pageSize, total, items));
    }

    /// <summary>Quick counters for the dashboard pill: how many open debts + their sum.</summary>
    [HttpGet("summary")]
    public async Task<ActionResult<object>> Summary()
    {
        var q = _db.CustomerDebts.AsNoTracking().Where(d => d.Active && !d.Paid);
        var count = await q.CountAsync();
        var sum = await q.SumAsync(d => (decimal?)d.Price) ?? 0m;
        return Ok(new { count, total = sum });
    }

    /// <summary>Soft-delete a debt row (sets Active=false). Tenant-scoped via the EF query
    /// filter — you can only delete debts in your own company. A settled debt (one already
    /// paid via a PaymentDocumentLine) is refused; storno the line instead.</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var d = await _db.CustomerDebts.FirstOrDefaultAsync(x => x.Id == id);
        if (d == null) return NotFound();
        if (d.Paid) return BadRequest(new { error = "Веќе платена ставка не може да се избрише — потребно е сторно." });
        d.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    public record DeleteDebtsRequest(IReadOnlyList<long> Ids);
    public record DeleteDebtsResponse(int Requested, int Deleted, int SkippedPaid, int SkippedMissing);

    /// <summary>Bulk soft-delete: takes a list of debt ids, sets Active=false on every one
    /// that's still unpaid + visible in the current tenant. Paid debts are skipped (storno
    /// the line instead). Returns a summary of how many actually got deleted vs. skipped.</summary>
    [HttpPost("delete-batch")]
    public async Task<ActionResult<DeleteDebtsResponse>> DeleteBatch([FromBody] DeleteDebtsRequest req)
    {
        if (req?.Ids is null || req.Ids.Count == 0)
            return BadRequest(new { error = "Не се испратени ставки за бришење." });
        if (req.Ids.Count > 500)
            return BadRequest(new { error = "Премногу ставки (макс. 500)." });

        var rows = await _db.CustomerDebts
            .Where(d => req.Ids.Contains(d.Id))   // tenant query filter applies automatically
            .ToListAsync();
        var skippedPaid = rows.Count(r => r.Paid);
        var toDelete = rows.Where(r => !r.Paid).ToList();
        foreach (var r in toDelete) r.Active = false;
        await _db.SaveChangesAsync();

        return Ok(new DeleteDebtsResponse(
            Requested: req.Ids.Count,
            Deleted: toDelete.Count,
            SkippedPaid: skippedPaid,
            SkippedMissing: req.Ids.Count - rows.Count));
    }
}
