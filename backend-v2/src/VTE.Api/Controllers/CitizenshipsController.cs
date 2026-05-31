using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.References;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/citizenships")]
[Authorize]
public class CitizenshipsController : ControllerBase
{
    private readonly VteDbContext _db;
    public CitizenshipsController(VteDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Citizenship>>> List([FromQuery] int? countryId = null)
    {
        var q = _db.Citizenships.AsNoTracking().AsQueryable();
        if (countryId.HasValue) q = q.Where(c => c.CountryId == (short)countryId.Value);
        return Ok(await q.OrderBy(c => c.Name).ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Citizenship>> Get(int id)
    {
        var c = await _db.Citizenships.AsNoTracking().FirstOrDefaultAsync(x => x.Id == (byte)id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<Citizenship>> Create([FromBody] Citizenship dto)
    {
        dto.Id = 0;
        _db.Citizenships.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] Citizenship dto)
    {
        var c = await _db.Citizenships.FirstOrDefaultAsync(x => x.Id == (byte)id);
        if (c == null) return NotFound();
        c.Name = dto.Name;
        c.CountryId = dto.CountryId;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Citizenships.FirstOrDefaultAsync(x => x.Id == (byte)id);
        if (c == null) return NotFound();
        _db.Citizenships.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
