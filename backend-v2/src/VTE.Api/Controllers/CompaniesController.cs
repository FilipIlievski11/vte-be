using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Companies;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/companies")]
[Authorize(Roles = Roles.Administrator)]
public class CompaniesController : ControllerBase
{
    private readonly VteDbContext _db;
    public CompaniesController(VteDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Company>>> List() =>
        Ok(await _db.Companies.AsNoTracking().OrderBy(c => c.Name).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Company>> Get(int id)
    {
        var c = await _db.Companies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == (byte)id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpPost]
    public async Task<ActionResult<Company>> Create([FromBody] Company dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.Companies.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Company dto)
    {
        var c = await _db.Companies.FirstOrDefaultAsync(x => x.Id == (byte)id);
        if (c == null) return NotFound();
        c.Name = dto.Name;
        c.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Companies.FirstOrDefaultAsync(x => x.Id == (byte)id);
        if (c == null) return NotFound();
        c.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
