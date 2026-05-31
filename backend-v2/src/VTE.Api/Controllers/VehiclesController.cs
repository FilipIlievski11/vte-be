using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Identity;
using VTE.Domain.Vehicles;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/vehicles")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public VehiclesController(VteDbContext db, ITenantContext tenant)
    {
        _db = db;
        _tenant = tenant;
    }

    /// <summary>
    /// List driven by ClientVehicleRelation rows joined to Vehicle. One row per
    /// relation, so a vehicle with multiple active owners / authorized clients
    /// appears multiple times. RelationId is the row's dataKey. Tenant scoping is
    /// inherited from the joined Vehicle (which carries the EF tenant query filter).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedDto<VehicleListItem>>> List(
        [FromQuery] string? q = null,
        [FromQuery] byte? companyId = null,
        [FromQuery] int? bodyTypeId = null,
        [FromQuery] short? categoryId = null,
        [FromQuery] int? page = 1,
        [FromQuery] int? pageSize = 50,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDir = null)
    {
        var pg = page is null or < 1 ? 1 : page.Value;
        var ps = pageSize is null or < 1 or > 200 ? 50 : pageSize.Value;

        // Base: relations that have a vehicle, joined to that vehicle.
        var rq = _db.ClientVehicleRelations.AsNoTracking()
            .Where(r => r.VehicleId != null)
            .Join(_db.Vehicles.AsNoTracking(),
                r => r.VehicleId!.Value,
                v => v.Id,
                (r, v) => new { r, v });

        if (companyId.HasValue)  rq = rq.Where(x => x.v.CompanyId == companyId.Value);
        if (bodyTypeId.HasValue) rq = rq.Where(x => x.v.BodyTypeId == bodyTypeId.Value);
        if (categoryId.HasValue) rq = rq.Where(x => x.v.CategoryId == categoryId.Value);

        // Global free-text search — one input matches Vin, Plate, Maker name,
        // Model name, this relation's Client name parts, and Client MB (EMBG).
        //
        // Strategy: precompute the union of matching RelationIds in 4 small
        // independent queries (each indexable), then filter the main query by
        // r.Id IN (...). This is dramatically faster than one big OR with four
        // nested IN subqueries — the planner produces a clean index-seek-merge
        // instead of nested loops over the whole relation table.
        if (!string.IsNullOrWhiteSpace(q))
        {
            var s = q.Trim();
            var like = $"%{s}%";

            // 1) Relations whose vehicle's Vin or Plate matches.
            var hitByVehicleText = _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => r.VehicleId != null)
                .Join(_db.Vehicles.AsNoTracking()
                          .Where(v => EF.Functions.Like(v.Vin, like) ||
                                      (v.Plate != null && EF.Functions.Like(v.Plate, like))),
                      r => r.VehicleId!.Value, v => v.Id, (r, v) => r.Id);

            // 2) Relations whose vehicle's model name (or maker name) matches.
            //    We resolve the matching ModelIds first, then join.
            var matchingModelIds =
                _db.VehicleModels.AsNoTracking()
                    .Where(m => EF.Functions.Like(m.Name, like))
                    .Select(m => m.Id)
                .Union(
                    from m in _db.VehicleModels.AsNoTracking()
                    join mk in _db.VehicleMakers.AsNoTracking() on m.MakerId equals mk.Id
                    where EF.Functions.Like(mk.Name, like)
                    select m.Id);

            var hitByVehicleModel = _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => r.VehicleId != null)
                .Join(_db.Vehicles.AsNoTracking()
                          .Where(v => v.ModelId.HasValue && matchingModelIds.Contains(v.ModelId.Value)),
                      r => r.VehicleId!.Value, v => v.Id, (r, v) => r.Id);

            // 3) Relations whose Client name parts or MB match.
            var clientHitIds = _db.Clients.AsNoTracking()
                .Where(c =>
                    (c.FirstName  != null && EF.Functions.Like(c.FirstName,  like)) ||
                    (c.MiddleName != null && EF.Functions.Like(c.MiddleName, like)) ||
                    (c.LastName   != null && EF.Functions.Like(c.LastName,   like)) ||
                    (c.MB         != null && EF.Functions.Like(c.MB,         like)))
                .Select(c => c.Id);
            var hitByClient = _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => r.VehicleId != null && clientHitIds.Contains(r.ClientId))
                .Select(r => r.Id);

            // Union all matching relation IDs and apply as one IN.
            var hitRelationIds = hitByVehicleText
                .Union(hitByVehicleModel)
                .Union(hitByClient);

            rq = rq.Where(x => hitRelationIds.Contains(x.r.Id));
        }

        var total = await rq.CountAsync();
        var desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
        rq = (sortBy?.ToLowerInvariant()) switch
        {
            "vin"   => desc ? rq.OrderByDescending(x => x.v.Vin)   : rq.OrderBy(x => x.v.Vin),
            "plate" => desc ? rq.OrderByDescending(x => x.v.Plate) : rq.OrderBy(x => x.v.Plate),
            _       => desc ? rq.OrderByDescending(x => x.r.Id)    : rq.OrderByDescending(x => x.r.Id),
        };

        // Materialize the page as flat anonymous rows.
        var page1 = await rq.Skip((pg - 1) * ps).Take(ps)
            .Select(x => new
            {
                RelationId      = x.r.Id,
                RelationTypeId  = x.r.RelationTypeId,
                ClientId        = x.r.ClientId,
                RelationActive  = x.r.Active,
                VehicleId       = x.v.Id,
                x.v.CompanyId, x.v.Vin, x.v.Plate,
                x.v.ModelId, x.v.BodyTypeId, x.v.CategoryId, x.v.PrimaryColorId,
                x.v.EnginePowerKw, x.v.EngineWorkingCapacityCc,
                VehicleActive = x.v.Active
            }).ToListAsync();

        // Batch lookups for display fields
        var modelIds = page1.Where(v => v.ModelId.HasValue).Select(v => v.ModelId!.Value).Distinct().ToList();
        var btIds    = page1.Where(v => v.BodyTypeId.HasValue).Select(v => v.BodyTypeId!.Value).Distinct().ToList();
        var catIds   = page1.Where(v => v.CategoryId.HasValue).Select(v => v.CategoryId!.Value).Distinct().ToList();
        var colIds   = page1.Where(v => v.PrimaryColorId.HasValue).Select(v => v.PrimaryColorId!.Value).Distinct().ToList();
        var clientIds = page1.Select(v => v.ClientId).Distinct().ToList();
        var typeIds   = page1.Select(v => v.RelationTypeId).Distinct().ToList();

        var models   = await _db.VehicleModels.AsNoTracking().Where(m => modelIds.Contains(m.Id)).ToDictionaryAsync(m => m.Id);
        var makerIds = models.Values.Select(m => m.MakerId).Distinct().ToList();
        var makers   = await _db.VehicleMakers.AsNoTracking().Where(m => makerIds.Contains(m.Id)).ToDictionaryAsync(m => m.Id, m => m.Name);
        var bodies   = await _db.VehicleBodyTypes.AsNoTracking().Where(x => btIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x.Name);
        var cats     = await _db.VehicleCategories.AsNoTracking().Where(x => catIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x.Name);
        var cols     = await _db.VehicleColors.AsNoTracking().Where(x => colIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, x => x.Name);

        var clients = clientIds.Count == 0
            ? new Dictionary<long, (string? Name, string? Mb)>()
            : await _db.Clients.AsNoTracking()
                .Where(c => clientIds.Contains(c.Id))
                .Select(c => new { c.Id, c.FirstName, c.MiddleName, c.LastName, c.MB })
                .ToDictionaryAsync(
                    c => c.Id,
                    c => (
                        Name: string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }
                            .Where(p => !string.IsNullOrWhiteSpace(p))) is var n && n.Length > 0 ? n : null,
                        Mb: c.MB));

        var relTypes = typeIds.Count == 0
            ? new Dictionary<byte, string>()
            : await _db.ClientVehicleRelationTypes.AsNoTracking()
                .Where(t => typeIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, t => t.Name);

        var items = page1.Select(v =>
        {
            string? modelName = null, makerName = null;
            if (v.ModelId.HasValue && models.TryGetValue(v.ModelId.Value, out var m))
            {
                modelName = m.Name;
                makers.TryGetValue(m.MakerId, out makerName);
            }
            string? clientName = null, clientMb = null;
            if (clients.TryGetValue(v.ClientId, out var c))
            {
                clientName = c.Name;
                clientMb = c.Mb;
            }
            return new VehicleListItem(
                v.VehicleId, v.CompanyId, v.Vin, v.Plate,
                v.ModelId, modelName, makerName,
                v.BodyTypeId, v.BodyTypeId.HasValue && bodies.TryGetValue(v.BodyTypeId.Value, out var btN) ? btN : null,
                v.CategoryId, v.CategoryId.HasValue && cats.TryGetValue(v.CategoryId.Value, out var ctN) ? ctN : null,
                v.PrimaryColorId, v.PrimaryColorId.HasValue && cols.TryGetValue(v.PrimaryColorId.Value, out var clN) ? clN : null,
                v.EnginePowerKw, v.EngineWorkingCapacityCc,
                clientName, clientMb,
                v.VehicleActive,
                v.RelationId, v.RelationTypeId,
                relTypes.GetValueOrDefault(v.RelationTypeId));
        }).ToList();

        return Ok(new PagedDto<VehicleListItem>(pg, ps, total, items));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<VehicleReadDto>> Get(long id)
    {
        var v = await _db.Vehicles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (v == null) return NotFound();
        return Ok(ToReadDto(v));
    }

    [HttpPost]
    public async Task<ActionResult<VehicleReadDto>> Create([FromBody] VehicleWriteDto dto)
    {
        byte companyId;
        if (_tenant.IsAdmin && dto.CompanyId.HasValue)
        {
            if (!await _db.Companies.AnyAsync(c => c.Id == dto.CompanyId.Value))
                return BadRequest(new { error = $"Company {dto.CompanyId.Value} does not exist." });
            companyId = dto.CompanyId.Value;
        }
        else if (_tenant.CompanyId.HasValue) companyId = _tenant.CompanyId.Value;
        else
        {
            var first = await _db.Companies.AsNoTracking().OrderBy(c => c.Id).Select(c => (byte?)c.Id).FirstOrDefaultAsync();
            if (first == null) return BadRequest(new { error = "No Company exists yet." });
            companyId = first.Value;
        }

        var v = new Vehicle { CompanyId = companyId, CreatedAt = DateTime.UtcNow };
        ApplyWrite(v, dto);
        _db.Vehicles.Add(v);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = v.Id }, ToReadDto(v));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] VehicleWriteDto dto)
    {
        var v = await _db.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
        if (v == null) return NotFound();
        if (dto.CompanyId.HasValue && dto.CompanyId.Value != v.CompanyId)
            return BadRequest(new { error = "CompanyId is immutable after creation." });
        ApplyWrite(v, dto);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete (Active=false).</summary>
    [HttpDelete("{id:long}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(long id)
    {
        var v = await _db.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
        if (v == null) return NotFound();
        v.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static void ApplyWrite(Vehicle v, VehicleWriteDto d)
    {
        v.Vin = d.Vin;
        v.EngineNumber = d.EngineNumber;
        v.Plate = d.Plate;
        v.CategoryId = d.CategoryId;
        v.BodyTypeId = d.BodyTypeId;
        v.ModelId = d.ModelId;
        v.PrimaryColorId = d.PrimaryColorId;
        v.SecondaryColorId = d.SecondaryColorId;
        v.MadeCountryId = d.MadeCountryId;
        v.FuelId = d.FuelId;
        v.SecondFuelId = d.SecondFuelId;
        v.EngineTypeId = d.EngineTypeId;
        v.EcoProgramId = d.EcoProgramId;
        v.PaymentCategoryId = d.PaymentCategoryId;
        v.EnginePowerKw = d.EnginePowerKw;
        v.EngineWorkingCapacityCc = d.EngineWorkingCapacityCc;
        v.MaxRpm = d.MaxRpm;
        v.MaxSpeedKmh = d.MaxSpeedKmh;
        v.HasLpg = d.HasLpg;
        v.LengthMm = d.LengthMm; v.WidthMm = d.WidthMm; v.HeightMm = d.HeightMm;
        v.EmptyWeightKg = d.EmptyWeightKg; v.MaxAllowedWeightKg = d.MaxAllowedWeightKg;
        v.MaxLegalTotalMassKg = d.MaxLegalTotalMassKg;
        v.MaxConstructiveTotalMassKg = d.MaxConstructiveTotalMassKg;
        v.TrailerMassWithBrakesKg = d.TrailerMassWithBrakesKg;
        v.TrailerMassWithoutBrakesKg = d.TrailerMassWithoutBrakesKg;
        v.AxleCount = d.AxleCount; v.WheelCount = d.WheelCount;
        v.AxleLoad1Kg = d.AxleLoad1Kg; v.AxleLoad2Kg = d.AxleLoad2Kg;
        v.Seats = d.Seats; v.StandingSeats = d.StandingSeats;
        v.Co2GKm = d.Co2GKm; v.NoiseStaticDb = d.NoiseStaticDb; v.NoiseMovingDb = d.NoiseMovingDb;
        v.TypeText = d.TypeText; v.ModelVariant = d.ModelVariant; v.ApprovalMark = d.ApprovalMark;
        v.Note = d.Note;
        v.Active = d.Active ?? v.Active;
    }

    private static VehicleReadDto ToReadDto(Vehicle v) => new(
        v.Id, v.CompanyId, v.Vin, v.EngineNumber, v.Plate,
        v.CategoryId, v.BodyTypeId, v.ModelId, v.PrimaryColorId, v.SecondaryColorId, v.MadeCountryId,
        v.FuelId, v.SecondFuelId, v.EngineTypeId, v.EcoProgramId, v.PaymentCategoryId,
        v.EnginePowerKw, v.EngineWorkingCapacityCc, v.MaxRpm, v.MaxSpeedKmh, v.HasLpg,
        v.LengthMm, v.WidthMm, v.HeightMm,
        v.EmptyWeightKg, v.MaxAllowedWeightKg, v.MaxLegalTotalMassKg, v.MaxConstructiveTotalMassKg,
        v.TrailerMassWithBrakesKg, v.TrailerMassWithoutBrakesKg,
        v.AxleCount, v.WheelCount, v.AxleLoad1Kg, v.AxleLoad2Kg,
        v.Seats, v.StandingSeats,
        v.Co2GKm, v.NoiseStaticDb, v.NoiseMovingDb,
        v.TypeText, v.ModelVariant, v.ApprovalMark,
        v.Note, v.Active, v.CreatedAt);
}
