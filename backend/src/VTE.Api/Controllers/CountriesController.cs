using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Reference;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// Dedicated CRUD for Countries because the legacy form exposes more fields
// than the generic ref grid (Iso2 / short-name + Citizenship noun).
[ApiController]
[Route("api/countries")]
[Authorize]
public class CountriesController : ControllerBase
{
    private readonly VteDbContext _db;
    public CountriesController(VteDbContext db) => _db = db;

    public record CountryDto(int Id, string Name, string? Iso2, string? Iso3, string? Citizenship, bool IsActive);
    public record CountryRequest(string Name, string? Iso2, string? Iso3, string? Citizenship, bool IsActive);

    // Any authenticated user can list — same access level as /ref/countries.
    [HttpGet]
    public async Task<List<CountryDto>> List()
        => await _db.Countries.AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CountryDto(c.Id, c.Name, c.Iso2, c.Iso3, c.Citizenship, c.IsActive))
            .ToListAsync();

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<CountryDto>> Create(CountryRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var c = new Country { Name = req.Name.Trim(), Iso2 = Trim(req.Iso2), Iso3 = Trim(req.Iso3), Citizenship = Trim(req.Citizenship), IsActive = req.IsActive };
        _db.Countries.Add(c);
        await _db.SaveChangesAsync();
        return Ok(new CountryDto(c.Id, c.Name, c.Iso2, c.Iso3, c.Citizenship, c.IsActive));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, CountryRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var c = await _db.Countries.FindAsync(id);
        if (c is null) return NotFound();
        c.Name = req.Name.Trim();
        c.Iso2 = Trim(req.Iso2);
        c.Iso3 = Trim(req.Iso3);
        c.Citizenship = Trim(req.Citizenship);
        c.IsActive = req.IsActive;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Countries.FindAsync(id);
        if (c is null) return NotFound();
        try { _db.Remove(c); await _db.SaveChangesAsync(); return NoContent(); }
        catch (DbUpdateException) { return BadRequest(new { error = "Cannot delete: country is referenced by other rows. Deactivate it instead." }); }
    }

    private static string? Trim(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
