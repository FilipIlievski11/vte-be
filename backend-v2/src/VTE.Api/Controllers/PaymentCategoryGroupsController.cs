using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// Категории на наплата (<see cref="VTE.Domain.Payments.PaymentCategoryGroup"/>) —
/// the billing dimension PriceCatalog rules hang off (Јавни патишта, Комунална такса,
/// Црвен крст, …), each linked to the уплатна сметка its money goes to.
/// Backs the „Категории на наплата" screen.
/// </summary>
[ApiController]
[Route("api/payment-category-groups")]
[Authorize]
public class PaymentCategoryGroupsController : ControllerBase
{
    private readonly VteDbContext _db;
    public PaymentCategoryGroupsController(VteDbContext db) => _db = db;

    public record GroupDto(
        int Id, string Name, int? CalculationItemId, int CompanyScope, bool Active,
        int RuleCount, int ActiveRuleCount,
        string? AccountName, string? BankAccount, string? Bank, string? Form);

    public record GroupWriteDto(string Name, int? CalculationItemId, bool Active = true);

    /// <summary>All categories with rule counts + the resolved уплатна сметка. The
    /// synthetic id 0 entry is the "uncategorized" bucket (rules with NULL group).</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GroupDto>>> List()
    {
        var counts = await _db.PriceCatalogs.AsNoTracking()
            .GroupBy(p => p.PaymentCategoryGroupId)
            .Select(g => new { g.Key, Total = g.Count(), Active = g.Count(x => x.Active) })
            .ToListAsync();
        var byId = counts.Where(c => c.Key != null).ToDictionary(c => c.Key!.Value, c => (c.Total, c.Active));
        var nullBucket = counts.FirstOrDefault(c => c.Key == null);

        var calc = await _db.CalculationItems.AsNoTracking().ToDictionaryAsync(c => c.Id);
        var groups = await _db.PaymentCategoryGroups.AsNoTracking()
            .OrderBy(g => g.Name).ThenBy(g => g.Id)
            .ToListAsync();

        var list = groups.Select(g =>
        {
            byId.TryGetValue(g.Id, out var cnt);
            var ci = g.CalculationItemId.HasValue ? calc.GetValueOrDefault(g.CalculationItemId.Value) : null;
            return new GroupDto(g.Id, g.Name, g.CalculationItemId, g.CompanyScope, g.Active,
                cnt.Total, cnt.Active, ci?.Name, ci?.BankAccount, ci?.Bank, ci?.Form);
        }).ToList();

        // uncategorized bucket first (FE localizes the empty name)
        list.Insert(0, new GroupDto(0, "", null, 0, true,
            nullBucket?.Total ?? 0, nullBucket?.Active ?? 0, null, null, null, null));

        return Ok(list);
    }

    /// <summary>Rename a category, re-point its money destination (уплатна сметка),
    /// or (de)activate it. The rules themselves are edited on /api/price-catalogs.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] GroupWriteDto dto)
    {
        var g = await _db.PaymentCategoryGroups.FirstOrDefaultAsync(x => x.Id == id);
        if (g == null) return NotFound();
        if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest(new { error = "Називот е задолжителен." });
        if (dto.CalculationItemId.HasValue &&
            !await _db.CalculationItems.AnyAsync(c => c.Id == dto.CalculationItemId.Value))
            return BadRequest(new { error = "Уплатната сметка не постои." });

        g.Name = dto.Name.Trim();
        g.CalculationItemId = dto.CalculationItemId;
        g.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
