using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.References;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// NOTE: PK is not IDENTITY in the schema; Create requires a caller-supplied Id.
/// </summary>
[ApiController]
[Route("api/personal-data-types")]
[Authorize]
public class PersonalDataTypesController : ControllerBase
{
    private readonly VteDbContext _db;
    public PersonalDataTypesController(VteDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PersonalDataType>>> List() =>
        Ok(await _db.PersonalDataTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonalDataType>> Get(int id)
    {
        var c = await _db.PersonalDataTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == (byte)id);
        return c == null ? NotFound() : Ok(c);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<PersonalDataType>> Create([FromBody] PersonalDataType dto)
    {
        if (dto.Id == 0) return BadRequest(new { error = "Id is required (PK is not auto-incremented)." });
        if (await _db.PersonalDataTypes.AnyAsync(x => x.Id == dto.Id))
            return Conflict(new { error = $"Id {dto.Id} already exists." });
        _db.PersonalDataTypes.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] PersonalDataType dto)
    {
        var c = await _db.PersonalDataTypes.FirstOrDefaultAsync(x => x.Id == (byte)id);
        if (c == null) return NotFound();
        c.Name = dto.Name;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.PersonalDataTypes.FirstOrDefaultAsync(x => x.Id == (byte)id);
        if (c == null) return NotFound();
        _db.PersonalDataTypes.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
