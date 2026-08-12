using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Vehicles;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// Lookup endpoints for the Vehicle module. Reads populate the dropdowns on the
/// Vehicle form / list filters; the POSTs back the inline „Нов" buttons on that
/// form — legacy `uxVehicleEdit` lets the operator add a missing вид / каросерија /
/// марка / модел / боја / тип на мотор on the spot instead of blocking on an admin.
/// Every create de-duplicates on the trimmed name (the migrated legacy catalog is
/// already littered with „OPEL" ×7 — we don't add to that).
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

    // ---- Inline „Нов" creates (legacy parity: operator adds a missing lookup) ----

    public record LookupWriteDto(string? Code, string? Name);
    public record MakerWriteDto(string? Name, int? CountryId, string? Trademark);
    public record ModelWriteDto(int MakerId, string? Code, string? Name);

    private static string? Clean(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    /// <summary>Normalized name for the duplicate check — trimmed, case-insensitive.</summary>
    private static bool SameName(string a, string b) =>
        string.Equals(a.Trim(), b.Trim(), StringComparison.OrdinalIgnoreCase);

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] LookupWriteDto dto)
    {
        var name = Clean(dto.Name);
        if (name is null) return BadRequest(new { error = "Името е задолжително." });
        var all = await _db.VehicleCategories.AsNoTracking().ToListAsync();
        var dup = all.FirstOrDefault(x => SameName(x.Name, name));
        if (dup != null) return Ok(dup);
        var e = new VehicleCategory { Code = Clean(dto.Code), Name = name, Active = true };
        _db.VehicleCategories.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpPost("body-types")]
    public async Task<IActionResult> CreateBodyType([FromBody] LookupWriteDto dto)
    {
        var name = Clean(dto.Name);
        if (name is null) return BadRequest(new { error = "Името е задолжително." });
        var all = await _db.VehicleBodyTypes.AsNoTracking().ToListAsync();
        var dup = all.FirstOrDefault(x => SameName(x.Name, name));
        if (dup != null) return Ok(dup);
        var e = new VehicleBodyType { Code = Clean(dto.Code), Name = name, Active = true };
        _db.VehicleBodyTypes.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpPost("colors")]
    public async Task<IActionResult> CreateColor([FromBody] LookupWriteDto dto)
    {
        var name = Clean(dto.Name);
        if (name is null) return BadRequest(new { error = "Името е задолжително." });
        var all = await _db.VehicleColors.AsNoTracking().ToListAsync();
        var dup = all.FirstOrDefault(x => SameName(x.Name, name));
        if (dup != null) return Ok(dup);
        var e = new VehicleColor { Code = Clean(dto.Code), Name = name, Active = true };
        _db.VehicleColors.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpPost("engine-types")]
    public async Task<IActionResult> CreateEngineType([FromBody] LookupWriteDto dto)
    {
        var name = Clean(dto.Name);
        if (name is null) return BadRequest(new { error = "Името е задолжително." });
        var all = await _db.VehicleEngineTypes.AsNoTracking().ToListAsync();
        var dup = all.FirstOrDefault(x => SameName(x.Name, name));
        if (dup != null) return Ok(dup);
        var e = new VehicleEngineType { Code = Clean(dto.Code), Name = name, Active = true };
        _db.VehicleEngineTypes.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    [HttpPost("makers")]
    public async Task<IActionResult> CreateMaker([FromBody] MakerWriteDto dto)
    {
        var name = Clean(dto.Name);
        if (name is null) return BadRequest(new { error = "Името е задолжително." });
        var all = await _db.VehicleMakers.AsNoTracking().ToListAsync();
        var dup = all.FirstOrDefault(x => SameName(x.Name, name));
        if (dup != null) return Ok(dup);
        if (dto.CountryId is int cid && cid > 0
            && !await _db.Countries.AnyAsync(c => c.Id == (short)cid))
            return BadRequest(new { error = "Непостоечка држава." });
        var e = new VehicleMaker
        {
            Name = name,
            CountryId = dto.CountryId is int c2 && c2 > 0 ? (short)c2 : null,
            Trademark = Clean(dto.Trademark),
            Active = true,
        };
        _db.VehicleMakers.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    /// <summary>Models de-duplicate within their maker — „GOLF" exists under many brands.</summary>
    [HttpPost("models")]
    public async Task<IActionResult> CreateModel([FromBody] ModelWriteDto dto)
    {
        var name = Clean(dto.Name);
        if (name is null) return BadRequest(new { error = "Името е задолжително." });
        if (dto.MakerId <= 0 || !await _db.VehicleMakers.AnyAsync(m => m.Id == dto.MakerId))
            return BadRequest(new { error = "Марката е задолжителна." });
        var all = await _db.VehicleModels.AsNoTracking().Where(m => m.MakerId == dto.MakerId).ToListAsync();
        var dup = all.FirstOrDefault(x => SameName(x.Name, name));
        if (dup != null) return Ok(dup);
        var e = new VehicleModel { MakerId = dto.MakerId, Code = Clean(dto.Code), Name = name, Active = true };
        _db.VehicleModels.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e);
    }
}
