using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// Read-only endpoints for the Vehicle module lookup tables. Used to populate
/// dropdowns on the Vehicle form / list filters. Admin-write endpoints can be
/// added later if needed.
/// </summary>
[ApiController]
[Route("api/vehicles/ref")]
[Authorize]
public class VehicleLookupsController : ControllerBase
{
    private readonly VteDbContext _db;
    public VehicleLookupsController(VteDbContext db) => _db = db;

    [HttpGet("body-types")]
    public async Task<IActionResult> BodyTypes() =>
        Ok(await _db.VehicleBodyTypes.AsNoTracking().OrderBy(x => x.Name).ToListAsync());

    [HttpGet("categories")]
    public async Task<IActionResult> Categories() =>
        Ok(await _db.VehicleCategories.AsNoTracking().OrderBy(x => x.Code).ToListAsync());

    [HttpGet("makers")]
    public async Task<IActionResult> Makers() =>
        Ok(await _db.VehicleMakers.AsNoTracking().OrderBy(x => x.Name).ToListAsync());

    [HttpGet("models")]
    public async Task<IActionResult> Models([FromQuery] int? makerId = null)
    {
        var q = _db.VehicleModels.AsNoTracking().AsQueryable();
        if (makerId.HasValue) q = q.Where(m => m.MakerId == makerId.Value);
        return Ok(await q.OrderBy(m => m.Name).ToListAsync());
    }

    [HttpGet("colors")]
    public async Task<IActionResult> Colors() =>
        Ok(await _db.VehicleColors.AsNoTracking().OrderBy(x => x.Name).ToListAsync());

    [HttpGet("fuels")]
    public async Task<IActionResult> Fuels() =>
        Ok(await _db.VehicleFuels.AsNoTracking().OrderBy(x => x.Name).ToListAsync());

    [HttpGet("eco-programs")]
    public async Task<IActionResult> EcoPrograms() =>
        Ok(await _db.VehicleEcoPrograms.AsNoTracking().OrderBy(x => x.Name).ToListAsync());

    [HttpGet("engine-types")]
    public async Task<IActionResult> EngineTypes([FromQuery] string? q = null)
    {
        var query = _db.VehicleEngineTypes.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var s = q.Trim();
            query = query.Where(e =>
                (e.Code != null && EF.Functions.Like(e.Code, $"%{s}%")) ||
                EF.Functions.Like(e.Name, $"%{s}%"));
        }
        return Ok(await query.OrderBy(x => x.Name).Take(500).ToListAsync());
    }

    [HttpGet("payment-categories")]
    public async Task<IActionResult> PaymentCategories() =>
        Ok(await _db.VehiclePaymentCategories.AsNoTracking().OrderBy(x => x.Name).ToListAsync());

    [HttpGet("relation-types")]
    public async Task<IActionResult> RelationTypes() =>
        Ok(await _db.ClientVehicleRelationTypes.AsNoTracking().OrderBy(x => x.Id).ToListAsync());
}
