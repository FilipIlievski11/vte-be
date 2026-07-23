using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Payments;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

/// <summary>
/// API over <see cref="VTE.Domain.Payments.CustomerDebt"/> — the v2 mirror of
/// legacy <c>CustomerFinancialState</c>. Backs the dashboard "Наплата" panel:
/// open debts grouped by client → vehicle, manual add (Детали на сметка),
/// single + bulk delete. Settlement happens via payment-documents/from-debts.
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
        DateTime CreatedAt,
        // Full legacy bill-grid name: "{Категорија} {Ставка} {Параметар}" — used where the
        // row stands alone (детали на сметка, delete confirm), not in the dense panel.
        string? ComposedName,
        // Broad fee class (legacy PaymentCategories.Id) — lets the FE annotate derived
        // fees (совет 1,5% од ТП: групи 27/63/1063; 1% од патна такса: 86/1086).
        int? PaymentCategoryGroupId);

    /// <summary>Substitute the {0}/{1} parametar-range placeholders in a legacy price-item
    /// name with the rule's actual ParametarFrom/ParametarTo. Fractional bounds render
    /// with comma decimals ("од 66,1 до 84") — same as the legacy MK-culture grid.
    /// Uses Replace, not String.Format, so stray braces in the name can't throw.</summary>
    private static string? FormatPriceName(string? name, double? from, double? to)
    {
        static string num(double v) =>
            v % 1 == 0 ? ((long)v).ToString() : v.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture).Replace('.', ',');
        if (string.IsNullOrEmpty(name)) return name;
        if (name.Contains("{0}")) name = name.Replace("{0}", from.HasValue ? num(from.Value) : "");
        if (name.Contains("{1}")) name = name.Replace("{1}", to.HasValue ? num(to.Value) : "");
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
        var pcRaw = await _db.PriceCatalogs.AsNoTracking()
            .Where(p => priceCatalogIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Name, p.ParametarFrom, p.ParametarTo, p.PaymentCategoryGroupId })
            .ToListAsync();
        var priceCatalog = pcRaw.ToDictionary(p => p.Id, p => FormatPriceName(p.Name, p.ParametarFrom, p.ParametarTo));

        // Full legacy-composed variant ("{Категорија} {Ставка} {Параметар}") for standalone display.
        var debtGroupIds = pcRaw.Where(p => p.PaymentCategoryGroupId.HasValue)
            .Select(p => p.PaymentCategoryGroupId!.Value).Distinct().ToList();
        var debtGroupNames = await _db.PaymentCategoryGroups.AsNoTracking()
            .Where(g => debtGroupIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, g => g.Name);
        var composedNames = pcRaw.ToDictionary(p => p.Id, p =>
            PaymentDocumentsController.ComposeServiceName(
                p.PaymentCategoryGroupId.HasValue ? debtGroupNames.GetValueOrDefault(p.PaymentCategoryGroupId.Value) : null,
                p.Name, p.ParametarFrom, p.ParametarTo));

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
                r.OrganizationId, r.Paid, r.SettledByLineId, r.CreatedAt,
                composedNames.GetValueOrDefault(r.PriceCatalogId),
                pcRaw.FirstOrDefault(p => p.Id == r.PriceCatalogId)?.PaymentCategoryGroupId);
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

    public record CreateDebtDto(
        long CustomerVehicleRelationId,
        int PriceCatalogId,
        decimal? Price,      // null → PriceCatalog.BasePrice
        string? Note);

    /// <summary>Manually add an open debt (ставка) to a client's account — the dashboard
    /// "Детали на сметка" dialog. Price defaults to the catalog rule's BasePrice but the
    /// operator can override it; VAT % is always snapshotted from the rule's VatRate.
    /// Origin = Manual (legacy parity: operators could insert CustomerFinancialState rows
    /// by hand from the dashboard's payment-catalog picker).</summary>
    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] CreateDebtDto dto)
    {
        // Relation must exist and be visible in this tenant (query filter applies).
        var relExists = await _db.ClientVehicleRelations.AsNoTracking()
            .AnyAsync(r => r.Id == dto.CustomerVehicleRelationId);
        if (!relExists) return BadRequest(new { error = "Клиентската релација не постои." });

        var pc = await _db.PriceCatalogs.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == dto.PriceCatalogId);
        if (pc == null) return BadRequest(new { error = "Ставката од ценовникот не постои." });

        var price = dto.Price ?? pc.BasePrice;
        if (price < 0) return BadRequest(new { error = "Цената не може да биде негативна." });

        var vatPercent = await _db.VatRates.AsNoTracking()
            .Where(v => v.Id == pc.VatRateId)
            .Select(v => (double?)v.Percent)
            .FirstOrDefaultAsync() ?? 0d;

        // Station org: same as the tenant's debts so bill numbering (grouped per
        // OrganizationId in from-debts) keeps one sequence.
        var organizationId = await _db.CustomerDebts.AsNoTracking()
            .OrderByDescending(d => d.Id)
            .Select(d => (int?)d.OrganizationId)
            .FirstOrDefaultAsync();
        if (organizationId is null or 0)
            return BadRequest(new { error = "Не може да се одреди организација — нема постоечки ставки." });

        var note = string.IsNullOrWhiteSpace(dto.Note) ? null : dto.Note.Trim();
        if (note?.Length > 300) note = note[..300];

        var debt = new CustomerDebt
        {
            CompanyId = _tenant.CompanyId ?? (byte)4,
            CustomerVehicleRelationId = dto.CustomerVehicleRelationId,
            PriceCatalogId = pc.Id,
            Price = price,
            VatPercent = vatPercent,
            Note = note,
            Origin = DebtOrigin.Manual,
            OrganizationId = organizationId.Value,
            Paid = false,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = _tenant.UserId,
            Active = true,
        };
        _db.CustomerDebts.Add(debt);
        await _db.SaveChangesAsync();
        return Ok(new { id = debt.Id });
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

    public record DebtPriceDto(decimal Price);

    /// <summary>Operator price override on an unpaid debt — the legacy grid let the operator
    /// type the amount directly (e.g. надоместок за животна средина per the current tariff).
    /// The edited price flows into the bill when the debt gets billed.</summary>
    [HttpPut("{id:long}/price")]
    public async Task<IActionResult> UpdatePrice(long id, [FromBody] DebtPriceDto req)
    {
        if (req.Price < 0)
            return BadRequest(new { error = "Цената не може да биде негативна." });
        var d = await _db.CustomerDebts.FirstOrDefaultAsync(x => x.Id == id);
        if (d == null) return NotFound();
        if (!d.Active)
            return BadRequest(new { error = "Ставката е избришана — цената не може да се менува." });
        if (d.Paid)
            return BadRequest(new { error = "Веќе платена ставка не може да се менува — потребно е сторно." });
        d.Price = req.Price;
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
