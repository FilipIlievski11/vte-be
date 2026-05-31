using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.References;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/document-issuers")]
[Authorize]
public class DocumentIssuersController : ControllerBase
{
    private readonly VteDbContext _db;
    public DocumentIssuersController(VteDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DocumentIssuer>>> List() =>
        Ok(await _db.DocumentIssuers.AsNoTracking().OrderBy(x => x.Name).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DocumentIssuer>> Get(int id)
    {
        var c = await _db.DocumentIssuers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == (byte)id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<DocumentIssuer>> Create([FromBody] DocumentIssuer dto)
    {
        dto.Id = 0;
        _db.DocumentIssuers.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] DocumentIssuer dto)
    {
        var c = await _db.DocumentIssuers.FirstOrDefaultAsync(x => x.Id == (byte)id);
        if (c == null) return NotFound();
        c.Name = dto.Name;
        c.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.DocumentIssuers.FirstOrDefaultAsync(x => x.Id == (byte)id);
        if (c == null) return NotFound();
        c.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
