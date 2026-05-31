using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Geography;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/countries")]
[Authorize]
public class CountriesController : ControllerBase
{
    private readonly VteDbContext _db;
    public CountriesController(VteDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Country>>> List([FromQuery] string? search = null)
    {
        var q = _db.Countries.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(c => c.Name != null && EF.Functions.Like(c.Name, $"%{search.Trim()}%"));
        return Ok(await q.OrderBy(c => c.Name).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Country>> Get(int id)
    {
        var c = await _db.Countries.AsNoTracking().FirstOrDefaultAsync(x => x.Id == (short)id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<Country>> Create([FromBody] Country dto)
    {
        dto.Id = 0;
        _db.Countries.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] Country dto)
    {
        var c = await _db.Countries.FirstOrDefaultAsync(x => x.Id == (short)id);
        if (c == null) return NotFound();
        c.Name = dto.Name;
        c.ShortName = dto.ShortName;
        c.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Countries.FirstOrDefaultAsync(x => x.Id == (short)id);
        if (c == null) return NotFound();
        c.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
