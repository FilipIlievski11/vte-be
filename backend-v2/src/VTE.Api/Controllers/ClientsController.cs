using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Clients;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/clients")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public ClientsController(VteDbContext db, ITenantContext tenant)
    {
        _db = db;
        _tenant = tenant;
    }

    // Operators see only their tenant's clients (enforced by DbContext query filter).
    // Administrators bypass.
    /// <summary>
    /// List clients (paged envelope: { page, pageSize, total, items }).
    /// For Administrators, optionally narrow to a single Company via ?companyId=.
    /// For Operators, the tenant query filter on the DbContext already scopes results to their
    /// own company.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedDto<ClientReadDto>>> List(
        [FromQuery] string? q = null,
        [FromQuery] string? search = null,
        [FromQuery] byte? companyId = null,
        [FromQuery] string? business = null,   // null/'all' | 'true' | 'false'
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDir = null)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 50;
        // Backwards compat: older callers used "search"; v1 uses "q".
        var term = !string.IsNullOrWhiteSpace(q) ? q : search;

        var query = _db.Clients.AsNoTracking().AsQueryable();

        if (companyId.HasValue)
            query = query.Where(c => c.CompanyId == companyId.Value);

        if (!string.IsNullOrWhiteSpace(business) && business != "all")
        {
            if (bool.TryParse(business, out var b))
                query = query.Where(c => c.Business == b);
        }

        if (!string.IsNullOrWhiteSpace(term))
        {
            // Tokenized search: EVERY whitespace-separated word must match some field, so a
            // full name works in ANY word order — FirstName=surname (Презиме) and
            // LastName=given (Име) are stored separately, so "Филип Илиевски" and
            // "Илиевски Филип" both hit. A lone EMBG/tax number is a single token and matches too.
            foreach (var token in term.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Take(6))
            {
                var like = $"%{token}%";
                query = query.Where(c =>
                    (c.FirstName != null && EF.Functions.Like(c.FirstName, like)) ||
                    (c.MiddleName != null && EF.Functions.Like(c.MiddleName, like)) ||
                    (c.LastName != null && EF.Functions.Like(c.LastName, like)) ||
                    (c.MB != null && EF.Functions.Like(c.MB, like)) ||
                    (c.TaxNumber != null && EF.Functions.Like(c.TaxNumber, like)));
            }
        }

        var total = await query.CountAsync();

        // Sort — default to id desc (most-recent first).
        var desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
        query = (sortBy?.ToLowerInvariant()) switch
        {
            "customer" or "name" or "lastname" => desc ? query.OrderByDescending(c => c.LastName).ThenByDescending(c => c.FirstName)
                                                       : query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName),
            "embg" or "mb"  => desc ? query.OrderByDescending(c => c.MB) : query.OrderBy(c => c.MB),
            _                => desc ? query.OrderByDescending(c => c.Id) : query.OrderByDescending(c => c.Id),
        };

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClientReadDto(
                c.Id, c.CompanyId, c.CityId, c.CitizenshipId, c.Business,
                c.FirstName, c.MiddleName, c.LastName, c.MB, c.Address,
                c.TaxNumber, c.PhoneNumber, c.Email, c.DateOfBirth, c.Note,
                c.Active, c.CreatedAt))
            .ToListAsync();

        return Ok(new PagedDto<ClientReadDto>(page, pageSize, total, items));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ClientReadDto>> Get(long id)
    {
        var c = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound();
        return Ok(new ClientReadDto(
            c.Id, c.CompanyId, c.CityId, c.CitizenshipId, c.Business,
            c.FirstName, c.MiddleName, c.LastName, c.MB, c.Address,
            c.TaxNumber, c.PhoneNumber, c.Email, c.DateOfBirth, c.Note,
            c.Active, c.CreatedAt));
    }

    [HttpPost]
    public async Task<ActionResult<ClientReadDto>> Create([FromBody] ClientWriteDto dto)
    {
        // Resolve tenant:
        //   - Operators always write to their own JWT-scoped CompanyId (any dto.CompanyId is ignored).
        //   - Administrators can explicitly choose a CompanyId via dto.CompanyId. If they don't
        //     supply one, fall back to the first Company in the table.
        byte companyId;
        if (_tenant.IsAdmin && dto.CompanyId.HasValue)
        {
            var exists = await _db.Companies.AnyAsync(co => co.Id == dto.CompanyId.Value);
            if (!exists) return BadRequest(new { error = $"Company {dto.CompanyId.Value} does not exist." });
            companyId = dto.CompanyId.Value;
        }
        else if (_tenant.CompanyId.HasValue)
        {
            companyId = _tenant.CompanyId.Value;
        }
        else
        {
            var first = await _db.Companies.AsNoTracking().OrderBy(c => c.Id).Select(c => (byte?)c.Id).FirstOrDefaultAsync();
            if (first == null) return BadRequest(new { error = "No Company exists yet." });
            companyId = first.Value;
        }

        var entity = new Client
        {
            CompanyId = companyId,
            CityId = dto.CityId,
            CitizenshipId = dto.CitizenshipId,
            Business = dto.Business,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            MB = dto.MB,
            Address = dto.Address,
            TaxNumber = dto.TaxNumber,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            Note = dto.Note,
            Active = dto.Active ?? true,
            CreatedAt = DateTime.UtcNow,
        };
        _db.Clients.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = entity.Id }, new ClientReadDto(
            entity.Id, entity.CompanyId, entity.CityId, entity.CitizenshipId, entity.Business,
            entity.FirstName, entity.MiddleName, entity.LastName, entity.MB, entity.Address,
            entity.TaxNumber, entity.PhoneNumber, entity.Email, entity.DateOfBirth, entity.Note,
            entity.Active, entity.CreatedAt));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] ClientWriteDto dto)
    {
        var c = await _db.Clients.FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound();

        // CompanyId is locked after creation. The UI sends the current CompanyId
        // in the payload (so a round-trip PUT keeps working), but a *change* is rejected.
        if (dto.CompanyId.HasValue && dto.CompanyId.Value != c.CompanyId)
        {
            return BadRequest(new { error = "CompanyId is immutable after creation." });
        }

        c.CityId = dto.CityId;
        c.CitizenshipId = dto.CitizenshipId;
        c.Business = dto.Business;
        c.FirstName = dto.FirstName;
        c.MiddleName = dto.MiddleName;
        c.LastName = dto.LastName;
        c.MB = dto.MB;
        c.Address = dto.Address;
        c.TaxNumber = dto.TaxNumber;
        c.PhoneNumber = dto.PhoneNumber;
        c.Email = dto.Email;
        c.DateOfBirth = dto.DateOfBirth;
        c.Note = dto.Note;
        c.Active = dto.Active ?? c.Active;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false. Row stays in the DB; can be reactivated via PUT.</summary>
    [HttpDelete("{id:long}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(long id)
    {
        var c = await _db.Clients.FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound();
        c.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
