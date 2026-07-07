using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Identity;
using VTE.Domain.Vehicles;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/vehicle-registrations")]
[Authorize]
public class VehicleRegistrationsController : ControllerBase
{
    private readonly VteDbContext _db;
    public VehicleRegistrationsController(VteDbContext db) => _db = db;

    public record VehicleRegistrationDto(
        long Id, long VehicleId, byte? IssuerId, string? IssuerName,
        string PlateNumber, DateTime RegisteredDate, DateTime ValidUntil,
        bool IsFirstRegistration, bool Active);

    public record VehicleRegistrationWriteDto(
        long VehicleId, byte IssuerId, string PlateNumber,
        DateTime RegisteredDate, DateTime ValidUntil,
        bool IsFirstRegistration, bool Active = true);

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VehicleRegistrationDto>>> List([FromQuery] long? vehicleId)
    {
        var q = _db.VehicleRegistrations.AsNoTracking().AsQueryable();
        if (vehicleId.HasValue) q = q.Where(r => r.VehicleId == vehicleId.Value);

        var rows = await q.OrderByDescending(r => r.RegisteredDate).Take(500).ToListAsync();
        var issuerIds = rows.Where(r => r.IssuerId.HasValue).Select(r => r.IssuerId!.Value).Distinct().ToList();
        var issuers = await _db.DocumentIssuers.AsNoTracking()
            .Where(d => issuerIds.Contains(d.Id))
            .ToDictionaryAsync(d => d.Id, d => d.Name);

        return Ok(rows.Select(r => new VehicleRegistrationDto(
            r.Id, r.VehicleId, r.IssuerId, r.IssuerId.HasValue ? issuers.GetValueOrDefault(r.IssuerId.Value) : null,
            r.PlateNumber, r.RegisteredDate, r.ValidUntil, r.IsFirstRegistration, r.Active)).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<VehicleRegistrationDto>> Create([FromBody] VehicleRegistrationWriteDto dto)
    {
        // Ensure Vehicle is visible (tenant filter applies).
        if (!await _db.Vehicles.AnyAsync(v => v.Id == dto.VehicleId))
            return BadRequest(new { error = "Vehicle not found or not visible." });

        var r = new VehicleRegistration
        {
            VehicleId = dto.VehicleId,
            IssuerId = dto.IssuerId,
            PlateNumber = dto.PlateNumber,
            RegisteredDate = dto.RegisteredDate,
            ValidUntil = dto.ValidUntil,
            IsFirstRegistration = dto.IsFirstRegistration,
            Active = dto.Active,
        };
        _db.VehicleRegistrations.Add(r);
        await _db.SaveChangesAsync();
        return Ok(new VehicleRegistrationDto(r.Id, r.VehicleId, r.IssuerId, null,
            r.PlateNumber, r.RegisteredDate, r.ValidUntil, r.IsFirstRegistration, r.Active));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] VehicleRegistrationWriteDto dto)
    {
        var r = await _db.VehicleRegistrations.FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();
        r.IssuerId = dto.IssuerId;
        r.PlateNumber = dto.PlateNumber;
        r.RegisteredDate = dto.RegisteredDate;
        r.ValidUntil = dto.ValidUntil;
        r.IsFirstRegistration = dto.IsFirstRegistration;
        r.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(long id)
    {
        var r = await _db.VehicleRegistrations.FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();
        r.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
