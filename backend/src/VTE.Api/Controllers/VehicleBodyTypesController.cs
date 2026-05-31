using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Reference;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// Dedicated CRUD for VehicleBodyTypes — mirrors the legacy uxVehicleBodytypes form
// which exposes Code, Name (Description), and OldName.
[ApiController]
[Route("api/vehicle-body-types")]
[Authorize]
public class VehicleBodyTypesController : ControllerBase
{
    private readonly VteDbContext _db;
    public VehicleBodyTypesController(VteDbContext db) => _db = db;

    public record BodyTypeDto(int Id, string? Code, string Name, string? OldName, bool IsActive);
    public record BodyTypeRequest(string? Code, string Name, string? OldName, bool IsActive);

    [HttpGet]
    public async Task<List<BodyTypeDto>> List()
        => await _db.VehicleBodyTypes.AsNoTracking()
            .OrderBy(b => b.Id)
            .Select(b => new BodyTypeDto(b.Id, b.Code, b.Name, b.OldName, b.IsActive))
            .ToListAsync();

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<BodyTypeDto>> Create(BodyTypeRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var b = new VehicleBodyType { Name = req.Name.Trim(), Code = Trim(req.Code), OldName = Trim(req.OldName), IsActive = req.IsActive };
        _db.VehicleBodyTypes.Add(b);
        await _db.SaveChangesAsync();
        return Ok(new BodyTypeDto(b.Id, b.Code, b.Name, b.OldName, b.IsActive));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, BodyTypeRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var b = await _db.VehicleBodyTypes.FindAsync(id);
        if (b is null) return NotFound();
        b.Name = req.Name.Trim();
        b.Code = Trim(req.Code);
        b.OldName = Trim(req.OldName);
        b.IsActive = req.IsActive;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var b = await _db.VehicleBodyTypes.FindAsync(id);
        if (b is null) return NotFound();
        try { _db.Remove(b); await _db.SaveChangesAsync(); return NoContent(); }
        catch (DbUpdateException) { return BadRequest(new { error = "Cannot delete: body type is referenced. Deactivate it instead." }); }
    }

    private static string? Trim(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
