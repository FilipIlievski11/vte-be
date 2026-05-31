using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Geography;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/cities")]
[Authorize]
public class CitiesController : ControllerBase
{
    private readonly VteDbContext _db;
    public CitiesController(VteDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<City>>> List(
        [FromQuery] int? communityId = null, [FromQuery] string? search = null)
    {
        var q = _db.Cities.AsNoTracking().AsQueryable();
        if (communityId.HasValue) q = q.Where(c => c.CommunityId == communityId.Value);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(c => EF.Functions.Like(c.Name, $"%{search.Trim()}%"));
        return Ok(await q.OrderBy(c => c.Name).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<City>> Get(int id)
    {
        var c = await _db.Cities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<City>> Create([FromBody] City dto)
    {
        dto.Id = 0;
        _db.Cities.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] City dto)
    {
        var c = await _db.Cities.FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound();
        c.Name = dto.Name;
        c.CommunityId = dto.CommunityId;
        c.PostalCode = dto.PostalCode;
        c.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Cities.FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound();
        c.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
