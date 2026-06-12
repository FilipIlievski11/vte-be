using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Identity;
using VTE.Domain.Payments;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// CRUD over <see cref="PriceCatalog"/> — the pricing rules that drive the
/// auto-debt engine (<c>PricingEvaluator</c> + <c>DebtService</c>). Currently
/// ~1000 rules migrated from the legacy 3-level hierarchy
/// (PaymentCategories → PaymentItems → PaymentItemParametars).
///
/// Reads: authenticated users (operators need them to render bills + the
/// price-pick widget).
/// Writes: Administrator only — a wrong rule edit can cascade into thousands
/// of mis-calculated debts on the next tech-exam or request.
/// </summary>
[ApiController]
[Route("api/price-catalogs")]
[Authorize]
public class PriceCatalogsController : ControllerBase
{
    private readonly VteDbContext _db;
    public PriceCatalogsController(VteDbContext db) => _db = db;

    public record PriceCatalogRow(
        int Id,
        string? Code,
        string Name,
        decimal BasePrice,
        int VatRateId,
        byte Trigger,                       // PriceTrigger enum value
        int? VehiclePaymentCategoryId,
        int? CommunityId,
        byte? PriceCompanyId,
        int? PaymentCategoryGroupId,
        string? VehicleField,
        double? ParametarFrom,
        double? ParametarTo,
        string? VehicleCategoryFilter,
        string? BankAccount,
        string? PaymentForm,
        bool Active);

    public record PriceCatalogWriteDto(
        string? Code,
        string Name,
        decimal BasePrice,
        int VatRateId,
        byte Trigger,
        int? VehiclePaymentCategoryId,
        int? CommunityId,
        byte? PriceCompanyId,
        int? PaymentCategoryGroupId,
        string? VehicleField,
        double? ParametarFrom,
        double? ParametarTo,
        string? VehicleCategoryFilter,
        string? BankAccount,
        string? PaymentForm,
        bool Active = true);

    private static PriceCatalogRow ToDto(PriceCatalog x) => new(
        x.Id, x.Code, x.Name, x.BasePrice, x.VatRateId,
        (byte)x.Trigger,
        x.VehiclePaymentCategoryId, x.CommunityId, x.PriceCompanyId,
        x.PaymentCategoryGroupId,
        x.VehicleField, x.ParametarFrom, x.ParametarTo,
        x.VehicleCategoryFilter, x.BankAccount, x.PaymentForm,
        x.Active);

    /// <summary>Paged list with admin-style filters. Default sort: Name ASC.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedDto<PriceCatalogRow>>> List(
        [FromQuery] byte? trigger = null,
        [FromQuery] byte? companyId = null,
        [FromQuery] int? communityId = null,
        [FromQuery] int? vehiclePaymentCategoryId = null,
        [FromQuery] bool uncategorized = false,
        [FromQuery] int? paymentCategoryGroupId = null,
        [FromQuery] string? search = null,
        [FromQuery] bool activeOnly = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 500) pageSize = 50;

        var q = _db.PriceCatalogs.AsNoTracking().AsQueryable();

        if (activeOnly) q = q.Where(p => p.Active);
        if (trigger.HasValue) q = q.Where(p => (byte)p.Trigger == trigger.Value);
        if (companyId.HasValue) q = q.Where(p => p.PriceCompanyId == companyId.Value);
        if (communityId.HasValue) q = q.Where(p => p.CommunityId == communityId.Value);
        // "uncategorized" wins over an explicit category id — it's the NULL bucket.
        if (uncategorized)
            q = q.Where(p => p.VehiclePaymentCategoryId == null);
        else if (vehiclePaymentCategoryId.HasValue)
            q = q.Where(p => p.VehiclePaymentCategoryId == vehiclePaymentCategoryId.Value);
        if (paymentCategoryGroupId.HasValue)
            q = q.Where(p => p.PaymentCategoryGroupId == paymentCategoryGroupId.Value);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            q = q.Where(p => p.Name.Contains(s) || (p.Code != null && p.Code.Contains(s)));
        }

        var total = await q.CountAsync();
        var rows = await q
            .OrderBy(p => p.Name).ThenBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PagedDto<PriceCatalogRow>(
            page, pageSize, total,
            rows.Select(ToDto).ToList()));
    }

    public record PriceCategoryNode(
        byte? Id, string Name, byte? ZelenMap, bool Active,
        int RuleCount, int ActiveRuleCount);

    /// <summary>The vehicle-payment categories the price catalog is organised by, each
    /// with its rule counts. The first node (Id = null) is the synthetic "uncategorized"
    /// bucket — rules with no VehiclePaymentCategoryId. Backs the master rail on the
    /// price-catalog screen.</summary>
    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<PriceCategoryNode>>> Categories()
    {
        var counts = await _db.PriceCatalogs.AsNoTracking()
            .GroupBy(p => p.VehiclePaymentCategoryId)
            .Select(g => new
            {
                CatId = g.Key,
                Total = g.Count(),
                Active = g.Count(x => x.Active),
            })
            .ToListAsync();

        var byId = counts
            .Where(c => c.CatId != null)
            .ToDictionary(c => c.CatId!.Value, c => (c.Total, c.Active));

        var cats = await _db.VehiclePaymentCategories.AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

        var list = cats.Select(c =>
        {
            byId.TryGetValue(c.Id, out var cnt);
            return new PriceCategoryNode(c.Id, c.Name, c.ZelenMap, c.Active, cnt.Total, cnt.Active);
        }).ToList();

        // Synthetic "uncategorized" bucket first — empty Name, frontend localizes it.
        var nullCnt = counts.FirstOrDefault(c => c.CatId == null);
        list.Insert(0, new PriceCategoryNode(
            null, "", null, true,
            nullCnt?.Total ?? 0, nullCnt?.Active ?? 0));

        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PriceCatalogRow>> Get(int id)
    {
        var x = await _db.PriceCatalogs.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return x == null ? NotFound() : Ok(ToDto(x));
    }

    /// <summary>Distinct values seen in the catalog — handy for populating filter dropdowns
    /// on the admin screen without a separate round-trip per kind.</summary>
    [HttpGet("lookups")]
    public async Task<ActionResult<object>> Lookups()
    {
        var triggers = Enum.GetValues<PriceTrigger>()
            .Select(t => new { id = (byte)t, name = t.ToString() })
            .ToList();

        // Company ids actually referenced by price rules, resolved to names so the UI
        // can show "ВЕУС – Велес" instead of a bare "3".
        var usedCompanyIds = await _db.PriceCatalogs.AsNoTracking()
            .Where(p => p.PriceCompanyId != null)
            .Select(p => p.PriceCompanyId!.Value)
            .Distinct().ToListAsync();
        var companyNames = await _db.Companies.AsNoTracking()
            .Where(c => usedCompanyIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, c => c.Name);
        var companies = usedCompanyIds
            .OrderBy(id => id)
            .Select(id => new { id, name = companyNames.GetValueOrDefault(id, $"#{id}") })
            .ToList();

        var vehicleFields = await _db.PriceCatalogs.AsNoTracking()
            .Where(p => p.VehicleField != null && p.VehicleField != "")
            .Select(p => p.VehicleField!)
            .Distinct().OrderBy(x => x).ToListAsync();

        var categoryGroups = await _db.PriceCatalogs.AsNoTracking()
            .Where(p => p.PaymentCategoryGroupId != null)
            .Select(p => p.PaymentCategoryGroupId!.Value)
            .Distinct().OrderBy(x => x).ToListAsync();

        return Ok(new { triggers, companies, vehicleFields, categoryGroups });
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<PriceCatalogRow>> Create([FromBody] PriceCatalogWriteDto dto)
    {
        var err = Validate(dto);
        if (err != null) return BadRequest(new { error = err });

        var entity = new PriceCatalog();
        Apply(entity, dto);
        _db.PriceCatalogs.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, ToDto(entity));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] PriceCatalogWriteDto dto)
    {
        var entity = await _db.PriceCatalogs.FirstOrDefaultAsync(p => p.Id == id);
        if (entity == null) return NotFound();

        var err = Validate(dto);
        if (err != null) return BadRequest(new { error = err });

        Apply(entity, dto);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false. We never hard-delete a rule because
    /// historical debts may reference it as <c>SettledByLineId</c> or via the
    /// origin chain — losing the rule would break receipt reprints.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.PriceCatalogs.FirstOrDefaultAsync(p => p.Id == id);
        if (entity == null) return NotFound();
        entity.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // -------- helpers --------

    private static string? Validate(PriceCatalogWriteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return "Name is required.";
        if (dto.BasePrice < 0)
            return "BasePrice cannot be negative.";
        if (dto.VatRateId < 0)
            return "VatRateId is invalid.";
        if (!Enum.IsDefined(typeof(PriceTrigger), (int)dto.Trigger))
            return "Trigger is not a valid PriceTrigger value.";
        if (dto.ParametarFrom.HasValue && dto.ParametarTo.HasValue
            && dto.ParametarFrom.Value > dto.ParametarTo.Value)
            return "ParametarFrom must be ≤ ParametarTo.";
        return null;
    }

    private static void Apply(PriceCatalog entity, PriceCatalogWriteDto dto)
    {
        entity.Code = string.IsNullOrWhiteSpace(dto.Code) ? null : dto.Code.Trim();
        entity.Name = dto.Name.Trim();
        entity.BasePrice = dto.BasePrice;
        entity.VatRateId = dto.VatRateId;
        entity.Trigger = (PriceTrigger)dto.Trigger;
        entity.VehiclePaymentCategoryId = dto.VehiclePaymentCategoryId;
        entity.CommunityId = dto.CommunityId;
        entity.PriceCompanyId = dto.PriceCompanyId;
        entity.PaymentCategoryGroupId = dto.PaymentCategoryGroupId;
        entity.VehicleField = string.IsNullOrWhiteSpace(dto.VehicleField) ? null : dto.VehicleField.Trim();
        entity.ParametarFrom = dto.ParametarFrom;
        entity.ParametarTo = dto.ParametarTo;
        entity.VehicleCategoryFilter = string.IsNullOrWhiteSpace(dto.VehicleCategoryFilter)
            ? null : dto.VehicleCategoryFilter.Trim();
        entity.BankAccount = string.IsNullOrWhiteSpace(dto.BankAccount) ? null : dto.BankAccount.Trim();
        entity.PaymentForm = string.IsNullOrWhiteSpace(dto.PaymentForm) ? null : dto.PaymentForm.Trim();
        entity.Active = dto.Active;
    }
}
