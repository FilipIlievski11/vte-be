using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Identity;
using VTE.Domain.Vehicles;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/client-vehicle-relations")]
[Authorize]
public class ClientVehicleRelationsController : ControllerBase
{
    private readonly VteDbContext _db;
    public ClientVehicleRelationsController(VteDbContext db) => _db = db;

    public record RelationDto(
        long Id, long ClientId, long? VehicleId,
        byte RelationTypeId, string? RelationTypeName,
        string? ClientDisplayName, string? ClientMb,
        string? VehicleVin, string? VehiclePlate,
        string? VehicleMaker, string? VehicleModel,
        DateTime StartDate, DateTime? EndDate,
        string? StartNote, string? EndNote, bool Active);

    public record RelationWriteDto(
        long ClientId, long? VehicleId, byte RelationTypeId,
        DateTime StartDate, DateTime? EndDate,
        string? StartNote, string? EndNote, bool Active = true);

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RelationDto>>> List(
        [FromQuery] long? vehicleId = null,
        [FromQuery] long? clientId = null,
        [FromQuery] bool? activeOnly = null,
        [FromQuery] string? q = null)
    {
        var query = _db.ClientVehicleRelations.AsNoTracking().AsQueryable();
        if (vehicleId.HasValue) query = query.Where(r => r.VehicleId == vehicleId.Value);
        if (clientId.HasValue)  query = query.Where(r => r.ClientId  == clientId.Value);
        if (activeOnly == true) query = query.Where(r => r.Active);

        // Free-text search powering the New Request "find vehicle" picker: one
        // input matches the client (name parts / EMBG) and the vehicle (VIN /
        // plate / maker / model). Mirrors VehiclesController — precompute the
        // union of matching RelationIds in small indexable queries, then one IN.
        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q.Trim()}%";

            var hitByVehicleText = _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => r.VehicleId != null)
                .Join(_db.Vehicles.AsNoTracking()
                          .Where(v => EF.Functions.Like(v.Vin, like) ||
                                      (v.Plate != null && EF.Functions.Like(v.Plate, like))),
                      r => r.VehicleId!.Value, v => v.Id, (r, v) => r.Id);

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

            var clientHitIds = _db.Clients.AsNoTracking()
                .Where(c =>
                    (c.FirstName  != null && EF.Functions.Like(c.FirstName,  like)) ||
                    (c.MiddleName != null && EF.Functions.Like(c.MiddleName, like)) ||
                    (c.LastName   != null && EF.Functions.Like(c.LastName,   like)) ||
                    (c.MB         != null && EF.Functions.Like(c.MB,         like)))
                .Select(c => c.Id);
            var hitByClient = _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => clientHitIds.Contains(r.ClientId))
                .Select(r => r.Id);

            var hitRelationIds = hitByVehicleText.Union(hitByVehicleModel).Union(hitByClient);
            query = query.Where(r => hitRelationIds.Contains(r.Id));
        }

        // Autocomplete needs only a short list; unfiltered callers (form pickers)
        // still expect the full set.
        var take = string.IsNullOrWhiteSpace(q) ? 500 : 25;
        var rows = await query.OrderByDescending(r => r.StartDate).Take(take).ToListAsync();

        var typeIds   = rows.Select(r => r.RelationTypeId).Distinct().ToList();
        var clientIds = rows.Select(r => r.ClientId).Distinct().ToList();
        var vIds      = rows.Where(r => r.VehicleId.HasValue).Select(r => r.VehicleId!.Value).Distinct().ToList();

        var types   = await _db.ClientVehicleRelationTypes.AsNoTracking()
            .Where(t => typeIds.Contains(t.Id)).ToDictionaryAsync(t => t.Id, t => t.Name);
        var clients = await _db.Clients.AsNoTracking()
            .Where(c => clientIds.Contains(c.Id))
            .Select(c => new { c.Id, c.FirstName, c.MiddleName, c.LastName, c.MB })
            .ToDictionaryAsync(c => c.Id);
        var vehicles = await _db.Vehicles.AsNoTracking()
            .Where(v => vIds.Contains(v.Id))
            .Select(v => new { v.Id, v.Vin, v.Plate, v.ModelId })
            .ToDictionaryAsync(v => v.Id);

        // Resolve model/maker names for the vehicles in this page.
        var modelIds = vehicles.Values.Where(v => v.ModelId.HasValue).Select(v => v.ModelId!.Value).Distinct().ToList();
        var models   = await _db.VehicleModels.AsNoTracking()
            .Where(m => modelIds.Contains(m.Id))
            .ToDictionaryAsync(m => m.Id);
        var makerIds = models.Values.Select(m => m.MakerId).Distinct().ToList();
        var makers   = await _db.VehicleMakers.AsNoTracking()
            .Where(m => makerIds.Contains(m.Id))
            .ToDictionaryAsync(m => m.Id, m => m.Name);

        string? joinName(long cid)
        {
            if (!clients.TryGetValue(cid, out var c)) return null;
            var parts = new[] { c.FirstName, c.MiddleName, c.LastName }
                .Where(p => !string.IsNullOrWhiteSpace(p));
            return string.Join(' ', parts);
        }
        string? clientMb(long cid) => clients.TryGetValue(cid, out var c) ? c.MB : null;

        return Ok(rows.Select(r =>
        {
            string? vin = null, plate = null, modelName = null, makerName = null;
            if (r.VehicleId.HasValue && vehicles.TryGetValue(r.VehicleId.Value, out var v))
            {
                vin = v.Vin;
                plate = v.Plate;
                if (v.ModelId.HasValue && models.TryGetValue(v.ModelId.Value, out var m))
                {
                    modelName = m.Name;
                    makers.TryGetValue(m.MakerId, out makerName);
                }
            }
            return new RelationDto(
                r.Id, r.ClientId, r.VehicleId,
                r.RelationTypeId, types.GetValueOrDefault(r.RelationTypeId),
                joinName(r.ClientId), clientMb(r.ClientId),
                vin, plate, makerName, modelName,
                r.StartDate, r.EndDate, r.StartNote, r.EndNote, r.Active);
        }).ToList());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<RelationDto>> Get(long id)
    {
        var r = await _db.ClientVehicleRelations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();

        string? typeName = await _db.ClientVehicleRelationTypes.AsNoTracking()
            .Where(t => t.Id == r.RelationTypeId).Select(t => t.Name).FirstOrDefaultAsync();

        var client = await _db.Clients.AsNoTracking()
            .Where(c => c.Id == r.ClientId)
            .Select(c => new { c.FirstName, c.MiddleName, c.LastName, c.MB })
            .FirstOrDefaultAsync();
        var clientName = client == null ? null
            : string.Join(' ', new[] { client.FirstName, client.MiddleName, client.LastName }
                .Where(x => !string.IsNullOrWhiteSpace(x)));
        var clientMb = client?.MB;

        string? vin = null, plate = null, makerName = null, modelName = null;
        if (r.VehicleId.HasValue)
        {
            var v = await _db.Vehicles.AsNoTracking()
                .Where(x => x.Id == r.VehicleId.Value)
                .Select(x => new { x.Vin, x.Plate, x.ModelId })
                .FirstOrDefaultAsync();
            if (v != null)
            {
                vin = v.Vin;
                plate = v.Plate;
                if (v.ModelId.HasValue)
                {
                    var m = await _db.VehicleModels.AsNoTracking()
                        .Where(x => x.Id == v.ModelId.Value)
                        .Select(x => new { x.Name, x.MakerId })
                        .FirstOrDefaultAsync();
                    if (m != null)
                    {
                        modelName = m.Name;
                        makerName = await _db.VehicleMakers.AsNoTracking()
                            .Where(x => x.Id == m.MakerId)
                            .Select(x => x.Name).FirstOrDefaultAsync();
                    }
                }
            }
        }

        return Ok(new RelationDto(r.Id, r.ClientId, r.VehicleId, r.RelationTypeId, typeName,
            clientName, clientMb, vin, plate, makerName, modelName,
            r.StartDate, r.EndDate, r.StartNote, r.EndNote, r.Active));
    }

    [HttpPost]
    public async Task<ActionResult<RelationDto>> Create([FromBody] RelationWriteDto dto)
    {
        if (!await _db.Clients.AnyAsync(c => c.Id == dto.ClientId))
            return BadRequest(new { error = "Client not found or not visible." });
        if (dto.VehicleId.HasValue && !await _db.Vehicles.AnyAsync(v => v.Id == dto.VehicleId.Value))
            return BadRequest(new { error = "Vehicle not found or not visible." });

        var r = new ClientVehicleRelation
        {
            ClientId = dto.ClientId,
            VehicleId = dto.VehicleId,
            RelationTypeId = dto.RelationTypeId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            StartNote = dto.StartNote,
            EndNote = dto.EndNote,
            Active = dto.Active,
        };
        _db.ClientVehicleRelations.Add(r);
        await _db.SaveChangesAsync();
        return Ok(new RelationDto(r.Id, r.ClientId, r.VehicleId, r.RelationTypeId, null,
            null, null, null, null, null, null, r.StartDate, r.EndDate, r.StartNote, r.EndNote, r.Active));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] RelationWriteDto dto)
    {
        var r = await _db.ClientVehicleRelations.FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();
        r.VehicleId = dto.VehicleId;
        r.RelationTypeId = dto.RelationTypeId;
        r.StartDate = dto.StartDate;
        r.EndDate = dto.EndDate;
        r.StartNote = dto.StartNote;
        r.EndNote = dto.EndNote;
        r.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(long id)
    {
        var r = await _db.ClientVehicleRelations.FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();
        r.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
