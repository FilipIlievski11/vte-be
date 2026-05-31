using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Geography;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/communities")]
[Authorize]
public class CommunitiesController : ControllerBase
{
    private readonly VteDbContext _db;
    public CommunitiesController(VteDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Community>>> List(
        [FromQuery] int? countryId = null, [FromQuery] string? search = null)
    {
        var q = _db.Communities.AsNoTracking().AsQueryable();
        if (countryId.HasValue) q = q.Where(c => c.CountryId == (short)countryId.Value);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(c => EF.Functions.Like(c.Name, $"%{search.Trim()}%"));
        return Ok(await q.OrderBy(c => c.Name).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Community>> Get(int id)
    {
        var c = await _db.Communities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<Community>> Create([FromBody] Community dto)
    {
        dto.Id = 0;
        _db.Communities.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] Community dto)
    {
        var c = await _db.Communities.FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound();
        c.Name = dto.Name;
        c.CountryId = dto.CountryId;
        c.Code = dto.Code;
        c.PlateNumberPrefix = dto.PlateNumberPrefix;
        c.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Communities.FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound();
        c.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
