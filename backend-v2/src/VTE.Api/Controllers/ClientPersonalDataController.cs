using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Clients;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/client-personal-data")]
[Authorize]
public class ClientPersonalDataController : ControllerBase
{
    private readonly VteDbContext _db;
    public ClientPersonalDataController(VteDbContext db) => _db = db;

    [HttpGet("by-client/{clientId:long}")]
    public async Task<ActionResult<IReadOnlyList<ClientPersonalDataReadDto>>> ListForClient(long clientId)
    {
        // Confirm the Client is visible to this caller (tenant filter applies).
        var clientExists = await _db.Clients.AnyAsync(c => c.Id == clientId);
        if (!clientExists) return NotFound();

        var rows = await _db.ClientPersonalData.AsNoTracking()
            .Where(x => x.ClientId == clientId)
            .OrderByDescending(x => x.Id)
            .Select(x => new ClientPersonalDataReadDto(
                x.Id, x.ClientId, x.PersonalDataTypeId, x.DocumentIssuerId, x.Number, x.CreatedAt, x.ExpiresAt, x.Active))
            .ToListAsync();
        return Ok(rows);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ClientPersonalDataReadDto>> Get(long id)
    {
        var x = await _db.ClientPersonalData.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (x == null) return NotFound();
        // Confirm the associated client is visible.
        var visible = await _db.Clients.AnyAsync(c => c.Id == x.ClientId);
        if (!visible) return NotFound();
        return Ok(new ClientPersonalDataReadDto(
            x.Id, x.ClientId, x.PersonalDataTypeId, x.DocumentIssuerId, x.Number, x.CreatedAt, x.ExpiresAt, x.Active));
    }

    [HttpPost]
    public async Task<ActionResult<ClientPersonalDataReadDto>> Create([FromBody] ClientPersonalDataWriteDto dto)
    {
        var clientExists = await _db.Clients.AnyAsync(c => c.Id == dto.ClientId);
        if (!clientExists) return BadRequest(new { error = "Client not found or not visible to this caller." });

        var e = new ClientPersonalData
        {
            ClientId = dto.ClientId,
            PersonalDataTypeId = dto.PersonalDataTypeId,
            DocumentIssuerId = dto.DocumentIssuerId,
            Number = dto.Number,
            // CreatedAt doubles as "date issued" — honour the operator-entered date when given.
            CreatedAt = dto.CreatedAt ?? DateTime.UtcNow,
            ExpiresAt = dto.ExpiresAt,
            Active = dto.Active,
        };
        _db.ClientPersonalData.Add(e);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = e.Id }, new ClientPersonalDataReadDto(
            e.Id, e.ClientId, e.PersonalDataTypeId, e.DocumentIssuerId, e.Number, e.CreatedAt, e.ExpiresAt, e.Active));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] ClientPersonalDataWriteDto dto)
    {
        var e = await _db.ClientPersonalData.FirstOrDefaultAsync(p => p.Id == id);
        if (e == null) return NotFound();
        var visible = await _db.Clients.AnyAsync(c => c.Id == e.ClientId);
        if (!visible) return NotFound();

        e.PersonalDataTypeId = dto.PersonalDataTypeId;
        e.DocumentIssuerId = dto.DocumentIssuerId;
        e.Number = dto.Number;
        if (dto.CreatedAt.HasValue) e.CreatedAt = dto.CreatedAt.Value;  // date issued, when edited
        e.ExpiresAt = dto.ExpiresAt;
        e.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false.</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var e = await _db.ClientPersonalData.FirstOrDefaultAsync(p => p.Id == id);
        if (e == null) return NotFound();
        var visible = await _db.Clients.AnyAsync(c => c.Id == e.ClientId);
        if (!visible) return NotFound();
        e.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
