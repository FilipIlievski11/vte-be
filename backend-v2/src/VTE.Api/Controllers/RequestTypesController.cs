using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Identity;
using VTE.Domain.Requests;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// CRUD for <see cref="RequestType"/> — the workflow catalog that drives
/// validation and side-effects of each <see cref="Request"/>. Reads are
/// available to all authenticated users (the operator's "New request" form
/// picks from this list). Writes are Administrator-only.
/// </summary>
[ApiController]
[Route("api/request-types")]
[Authorize]
public class RequestTypesController : ControllerBase
{
    private readonly VteDbContext _db;
    public RequestTypesController(VteDbContext db) => _db = db;

    public record RequestTypeDto(
        byte Id,
        byte? ParentRequestTypeId,
        byte DocumentPrintId,
        string Name,
        string? Description,
        byte TechnicalExamRequirement,          // 0=No, 1=Yes, 2=Optional
        bool PaymentRequired,
        bool IssuesNewRegistration,
        bool DeactivatesRelation,
        bool DeactivatesVehicle,
        bool TransfersOwnership,
        bool MutatesVehicleData,
        bool MutatesClientData,
        bool IsSufficient,
        bool PreviousRegistrationRequired,
        bool Active);

    public record RequestTypeWriteDto(
        byte? ParentRequestTypeId,
        byte DocumentPrintId,
        string Name,
        string? Description,
        byte TechnicalExamRequirement,
        bool PaymentRequired,
        bool IssuesNewRegistration,
        bool DeactivatesRelation,
        bool DeactivatesVehicle,
        bool TransfersOwnership,
        bool MutatesVehicleData,
        bool MutatesClientData,
        bool IsSufficient,
        bool PreviousRegistrationRequired,
        bool Active = true);

    private static RequestTypeDto ToDto(RequestType x) => new(
        x.Id, x.ParentRequestTypeId, x.DocumentPrintId,
        x.Name, x.Description,
        (byte)x.TechnicalExamRequirement,
        x.PaymentRequired, x.IssuesNewRegistration,
        x.DeactivatesRelation, x.DeactivatesVehicle,
        x.TransfersOwnership,
        x.MutatesVehicleData, x.MutatesClientData,
        x.IsSufficient, x.PreviousRegistrationRequired,
        x.Active);

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RequestTypeDto>>> List([FromQuery] bool? activeOnly = null)
    {
        var q = _db.RequestTypes.AsNoTracking().AsQueryable();
        if (activeOnly == true) q = q.Where(t => t.Active);
        var rows = await q.OrderBy(t => t.Name).ToListAsync();
        return Ok(rows.Select(ToDto).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RequestTypeDto>> Get(int id)
    {
        var x = await _db.RequestTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == (byte)id);
        return x == null ? NotFound() : Ok(ToDto(x));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<RequestTypeDto>> Create([FromBody] RequestTypeWriteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Name is required." });
        if (!await _db.RequestDocumentPrints.AnyAsync(p => p.Id == dto.DocumentPrintId))
            return BadRequest(new { error = "DocumentPrintId not found." });
        if (dto.ParentRequestTypeId.HasValue &&
            !await _db.RequestTypes.AnyAsync(t => t.Id == dto.ParentRequestTypeId.Value))
            return BadRequest(new { error = "ParentRequestTypeId not found." });

        var entity = new RequestType
        {
            ParentRequestTypeId = dto.ParentRequestTypeId,
            DocumentPrintId = dto.DocumentPrintId,
            Name = dto.Name.Trim(),
            Description = dto.Description,
            TechnicalExamRequirement = (TechnicalExamRequirement)dto.TechnicalExamRequirement,
            PaymentRequired = dto.PaymentRequired,
            IssuesNewRegistration = dto.IssuesNewRegistration,
            DeactivatesRelation = dto.DeactivatesRelation,
            DeactivatesVehicle = dto.DeactivatesVehicle,
            TransfersOwnership = dto.TransfersOwnership,
            MutatesVehicleData = dto.MutatesVehicleData,
            MutatesClientData = dto.MutatesClientData,
            IsSufficient = dto.IsSufficient,
            PreviousRegistrationRequired = dto.PreviousRegistrationRequired,
            Active = dto.Active,
        };
        _db.RequestTypes.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = entity.Id }, ToDto(entity));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, [FromBody] RequestTypeWriteDto dto)
    {
        var entity = await _db.RequestTypes.FirstOrDefaultAsync(t => t.Id == (byte)id);
        if (entity == null) return NotFound();

        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest(new { error = "Name is required." });
        if (!await _db.RequestDocumentPrints.AnyAsync(p => p.Id == dto.DocumentPrintId))
            return BadRequest(new { error = "DocumentPrintId not found." });
        if (dto.ParentRequestTypeId.HasValue)
        {
            if (dto.ParentRequestTypeId.Value == entity.Id)
                return BadRequest(new { error = "Parent cannot be self." });
            if (!await _db.RequestTypes.AnyAsync(t => t.Id == dto.ParentRequestTypeId.Value))
                return BadRequest(new { error = "ParentRequestTypeId not found." });
        }

        entity.ParentRequestTypeId = dto.ParentRequestTypeId;
        entity.DocumentPrintId = dto.DocumentPrintId;
        entity.Name = dto.Name.Trim();
        entity.Description = dto.Description;
        entity.TechnicalExamRequirement = (TechnicalExamRequirement)dto.TechnicalExamRequirement;
        entity.PaymentRequired = dto.PaymentRequired;
        entity.IssuesNewRegistration = dto.IssuesNewRegistration;
        entity.DeactivatesRelation = dto.DeactivatesRelation;
        entity.DeactivatesVehicle = dto.DeactivatesVehicle;
        entity.TransfersOwnership = dto.TransfersOwnership;
        entity.MutatesVehicleData = dto.MutatesVehicleData;
        entity.MutatesClientData = dto.MutatesClientData;
        entity.IsSufficient = dto.IsSufficient;
        entity.PreviousRegistrationRequired = dto.PreviousRegistrationRequired;
        entity.Active = dto.Active;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.RequestTypes.FirstOrDefaultAsync(t => t.Id == (byte)id);
        if (entity == null) return NotFound();
        entity.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
