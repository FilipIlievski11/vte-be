using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Stations;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/stations")]
[Authorize]
public class StationsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    public StationsController(VteDbContext db, ITenantContext tenant)
    {
        _db = db;
        _tenant = tenant;
    }

    // Tenant filter on Station: Operators see only their company's stations; Admins see all.
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Station>>> List() =>
        Ok(await _db.Stations.AsNoTracking().OrderBy(s => s.Name).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Station>> Get(int id)
    {
        var s = await _db.Stations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == (short)id);
        return s == null ? NotFound() : Ok(s);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<Station>> Create([FromBody] Station dto)
    {
        dto.Id = 0;
        _db.Stations.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] Station dto)
    {
        var s = await _db.Stations.FirstOrDefaultAsync(x => x.Id == (short)id);
        if (s == null) return NotFound();
        s.Name = dto.Name;
        s.CompanyId = dto.CompanyId;
        s.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await _db.Stations.FirstOrDefaultAsync(x => x.Id == (short)id);
        if (s == null) return NotFound();
        s.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
