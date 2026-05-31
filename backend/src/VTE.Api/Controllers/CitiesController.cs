using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Reference;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// Cities CRUD that exposes everything the legacy uxCities form did:
// Name, postal code, owning Community and Country.
[ApiController]
[Route("api/cities")]
[Authorize]
public class CitiesController : ControllerBase
{
    private readonly VteDbContext _db;
    public CitiesController(VteDbContext db) => _db = db;

    public record CityDto(int Id, string Name, string? PostalCode, int? CommunityId, string? CommunityName,
                          int? CountryId, string? CountryName, bool IsActive);
    public record CityRequest(string Name, string? PostalCode, int? CommunityId, int? CountryId, bool IsActive);

    [HttpGet]
    public async Task<List<CityDto>> List()
        => await (from c in _db.Cities.AsNoTracking()
                  join cm in _db.Communities.AsNoTracking() on c.CommunityId equals cm.Id into cmj
                  from cm in cmj.DefaultIfEmpty()
                  join co in _db.Countries.AsNoTracking() on c.CountryId equals co.Id into coj
                  from co in coj.DefaultIfEmpty()
                  orderby c.Id
                  select new CityDto(c.Id, c.Name, c.PostalCode, c.CommunityId, cm != null ? cm.Name : null,
                                     c.CountryId, co != null ? co.Name : null, c.IsActive)
                 ).ToListAsync();

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<CityDto>> Create(CityRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var c = new City { Name = req.Name.Trim(), PostalCode = Trim(req.PostalCode), CommunityId = req.CommunityId, CountryId = req.CountryId, IsActive = req.IsActive };
        _db.Cities.Add(c);
        await _db.SaveChangesAsync();
        return await ToDto(c.Id);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, CityRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var c = await _db.Cities.FindAsync(id);
        if (c is null) return NotFound();
        c.Name = req.Name.Trim();
        c.PostalCode = Trim(req.PostalCode);
        c.CommunityId = req.CommunityId;
        c.CountryId = req.CountryId;
        c.IsActive = req.IsActive;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Cities.FindAsync(id);
        if (c is null) return NotFound();
        try { _db.Remove(c); await _db.SaveChangesAsync(); return NoContent(); }
        catch (DbUpdateException) { return BadRequest(new { error = "Cannot delete: city is referenced by other rows. Deactivate it instead." }); }
    }

    private async Task<ActionResult<CityDto>> ToDto(int id)
    {
        var dto = await (from c in _db.Cities.AsNoTracking()
                         join cm in _db.Communities.AsNoTracking() on c.CommunityId equals cm.Id into cmj
                         from cm in cmj.DefaultIfEmpty()
                         join co in _db.Countries.AsNoTracking() on c.CountryId equals co.Id into coj
                         from co in coj.DefaultIfEmpty()
                         where c.Id == id
                         select new CityDto(c.Id, c.Name, c.PostalCode, c.CommunityId, cm != null ? cm.Name : null,
                                            c.CountryId, co != null ? co.Name : null, c.IsActive)
                        ).FirstOrDefaultAsync();
        return dto is null ? NotFound() : Ok(dto);
    }

    private static string? Trim(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
