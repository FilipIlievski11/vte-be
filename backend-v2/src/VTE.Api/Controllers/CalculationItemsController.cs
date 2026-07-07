using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Identity;
using VTE.Domain.Payments;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// Уплатни сметки (<see cref="CalculationItem"/>) — WHERE the money of a billing
/// category goes: account number, bank / уплатна-сметка description, payment-order
/// form (ПП30/ПП50). Referenced by <see cref="PaymentCategoryGroup"/>.
/// Reads: any operator (shown on the Категории на наплата screen).
/// Writes: Administrator — a wrong account number misdirects real payments.
/// </summary>
[ApiController]
[Route("api/calculation-items")]
[Authorize]
public class CalculationItemsController : ControllerBase
{
    private readonly VteDbContext _db;
    public CalculationItemsController(VteDbContext db) => _db = db;

    public record CalcItemDto(int Id, string Name, string? BankAccount, string? Bank, string? Form, bool Active, int CategoryCount);
    public record CalcItemWriteDto(string Name, string? BankAccount, string? Bank, string? Form, bool Active = true);

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CalcItemDto>>> List()
    {
        var usage = await _db.PaymentCategoryGroups.AsNoTracking()
            .Where(g => g.CalculationItemId != null)
            .GroupBy(g => g.CalculationItemId!.Value)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count);

        var rows = await _db.CalculationItems.AsNoTracking()
            .OrderBy(x => x.Name).ThenBy(x => x.Id)
            .ToListAsync();
        return Ok(rows.Select(x => new CalcItemDto(
            x.Id, x.Name, x.BankAccount, x.Bank, x.Form, x.Active,
            usage.GetValueOrDefault(x.Id))).ToList());
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<CalcItemDto>> Create([FromBody] CalcItemWriteDto dto)
    {
        var err = Validate(dto);
        if (err != null) return BadRequest(new { error = err });
        var e = new CalculationItem();
        Apply(e, dto);
        _db.CalculationItems.Add(e);
        await _db.SaveChangesAsync();
        return Ok(new CalcItemDto(e.Id, e.Name, e.BankAccount, e.Bank, e.Form, e.Active, 0));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] CalcItemWriteDto dto)
    {
        var e = await _db.CalculationItems.FirstOrDefaultAsync(x => x.Id == id);
        if (e == null) return NotFound();
        var err = Validate(dto);
        if (err != null) return BadRequest(new { error = err });
        Apply(e, dto);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete. Refused while categories still point at the account.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.CalculationItems.FirstOrDefaultAsync(x => x.Id == id);
        if (e == null) return NotFound();
        var used = await _db.PaymentCategoryGroups.AsNoTracking()
            .CountAsync(g => g.CalculationItemId == id && g.Active);
        if (used > 0)
            return BadRequest(new { error = $"Сметката е врзана за {used} категории — прво одврзи ги." });
        e.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static string? Validate(CalcItemWriteDto dto) =>
        string.IsNullOrWhiteSpace(dto.Name) ? "Називот е задолжителен." : null;

    private static void Apply(CalculationItem e, CalcItemWriteDto dto)
    {
        e.Name = dto.Name.Trim();
        e.BankAccount = string.IsNullOrWhiteSpace(dto.BankAccount) ? null : dto.BankAccount.Trim();
        e.Bank = string.IsNullOrWhiteSpace(dto.Bank) ? null : dto.Bank.Trim();
        e.Form = string.IsNullOrWhiteSpace(dto.Form) ? null : dto.Form.Trim();
        e.Active = dto.Active;
    }
}
