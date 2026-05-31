using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Vehicles;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/vehicles")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public VehiclesController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record VehicleListDto(long Id, string ShellNumber, string LastRegistrationNumber, int? VehicleCategoryId, bool IsActive,
        string? MakerName, string? ModelName, string? OwnerName, string? OwnerEMBG);

    // Child collection DTOs — match the actual VTE2 schema (richer than the legacy port).
    public record RegistrationDto(long? Id, int? IssuerId, string RegistrationNumber,
        DateOnly? MakeDate, DateOnly? ValidTill);
    public record AxleDto(long? Id, int AxleNumber, bool IsPropulsion, bool IsSteering, string? Note);
    public record AxleDistanceDto(long? Id, int FromAxleNumber, int ToAxleNumber, decimal? Distance);
    public record TyreDto(long? Id, int? TireTypeId, string? PositionNote, string? Dimensions,
        decimal? PressureFront, decimal? PressureRear);

    // Full detail wrapper used by GET /{id} and returned after Create.
    public record VehicleDetailDto(
        Vehicle Vehicle,
        IReadOnlyList<RegistrationDto> Registrations,
        IReadOnlyList<AxleDto> Axles,
        IReadOnlyList<AxleDistanceDto> AxleDistances,
        IReadOnlyList<TyreDto> Tyres);

    public record VehicleSaveRequest(
        string ShellNumber,
        string FirstRegistrationNumber, string LastRegistrationNumber,
        // classification
        int? BodyTypeId, int? VehicleCategoryId, int? VehicleUseId, int? VehicleModelId, string? VehicleModelAdding,
        int? MadeCountryId, int? VehicleCategoryForPaymentsId,
        // engine
        string? EngineNumber, int? EngineTypeId, int? EnginePowerSourceId, int? EngineSecondPowerSourceId, int? EngineEcoProgramId,
        decimal? EnginePowerKw, decimal? EngineWorkingCapacity, int? RPM, int? GearBoxId,
        // brakes / suspension
        int? BrakesId, int? SupportingId,
        // geometry
        decimal? VehicleHeight, decimal? VehicleWidth, decimal? VehicleLength,
        // doors / seats / wheels / axles
        int? NumberOfDoors, short? NumberOfSeats, short? NumberOfStandingSeats, short? NumberOfLyingSeats,
        int? NumberOfAxes, int? NumberOfPropulsionAxes, int? NumberOfWheels, int? NumberOfPropulsionWheels,
        // mass
        decimal? EmptyWeight, decimal? MaxAllowedWeight,
        // dates
        DateOnly? MakeDate,
        DateOnly? FirstRegistrationMakeDate, DateOnly? FirstRegistrationValidTill,
        DateOnly? LastRegistrationMakeDate,  DateOnly? LastRegistrationValidTill,
        int? FirstRegistrationIssuerId, int? LastRegistrationIssuerId,
        // color
        string? ColorCode, int? PrimaryColorId, int? SecondaryColorId,
        // equipment
        bool Suffocation, bool Hook, bool Winch, bool TNG,
        string? Note,
        // children — null = leave alone; non-null = replace wholesale
        IReadOnlyList<RegistrationDto>? Registrations = null,
        IReadOnlyList<AxleDto>? Axles = null,
        IReadOnlyList<AxleDistanceDto>? AxleDistances = null,
        IReadOnlyList<TyreDto>? Tyres = null);

    // Default ORDER BY mirrors the legacy `getVehiclesListShortALL` SP:
    // `ORDER BY Id DESC` — newest first.
    private static readonly SortMap<Vehicle> VehicleSort = new SortMap<Vehicle>(v => v.Id, defaultDescending: true)
        .Add("shellNumber",            v => v.ShellNumber)
        .Add("lastRegistrationNumber", v => v.LastRegistrationNumber)
        .Add("firstRegistrationNumber",v => v.FirstRegistrationNumber)
        .Add("id",                     v => v.Id);

    [HttpGet]
    public async Task<ActionResult<PagedResult<VehicleListDto>>> List(
        [FromQuery] string? q, [FromQuery] int? page, [FromQuery] int? pageSize,
        [FromQuery] string? sortBy, [FromQuery] string? sortDir, [FromQuery] int? take)
    {
        var query = _db.Vehicles.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            // Search hits VIN, registration numbers, plus the joined owner/maker/model
            // strings so the user can type "BMW", "АБДИОВ", or the EMBG and find a row.
            var like = $"%{q}%";
            query = query.Where(v =>
                EF.Functions.Like(v.ShellNumber, like) ||
                EF.Functions.Like(v.LastRegistrationNumber, like) ||
                EF.Functions.Like(v.FirstRegistrationNumber, like) ||
                _db.CustomerVehicleRelations.Any(r => r.VehicleId == v.Id &&
                    _db.Customers.Any(c => c.Id == r.CustomerId &&
                        (EF.Functions.Like(c.FirstName, like) ||
                         EF.Functions.Like(c.Surname ?? "", like) ||
                         EF.Functions.Like(c.EMBG ?? "", like)))) ||
                _db.VehicleModels.Any(m => m.Id == v.VehicleModelId &&
                    (EF.Functions.Like(m.Name, like) ||
                     _db.VehicleMakers.Any(mk => mk.Id == m.VehicleMakerId && EF.Functions.Like(mk.Name, like)))));
        }
        var ordered = VehicleSort.Apply(query, sortBy, sortDir)
            .Select(v => new VehicleListDto(
                v.Id, v.ShellNumber, v.LastRegistrationNumber, v.VehicleCategoryId, v.IsActive,
                _db.VehicleModels.Where(m => m.Id == v.VehicleModelId)
                    .Select(m => _db.VehicleMakers.Where(mk => mk.Id == m.VehicleMakerId).Select(mk => mk.Name).FirstOrDefault())
                    .FirstOrDefault(),
                _db.VehicleModels.Where(m => m.Id == v.VehicleModelId).Select(m => m.Name).FirstOrDefault(),
                _db.CustomerVehicleRelations.Where(r => r.VehicleId == v.Id).OrderBy(r => r.Id)
                    .Select(r => _db.Customers.Where(c => c.Id == r.CustomerId)
                        .Select(c => (c.FirstName + " " + (c.Surname ?? "")).Trim()).FirstOrDefault()).FirstOrDefault(),
                _db.CustomerVehicleRelations.Where(r => r.VehicleId == v.Id).OrderBy(r => r.Id)
                    .Select(r => _db.Customers.Where(c => c.Id == r.CustomerId).Select(c => c.EMBG).FirstOrDefault()).FirstOrDefault()));

        if (take is > 0)
            return Ok(new PagedResult<VehicleListDto>(1, take.Value, await ordered.CountAsync(), await ordered.Take(take.Value).ToListAsync()));
        return Ok(await ordered.ToPagedAsync(page, pageSize));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<VehicleDetailDto>> Get(long id)
    {
        var v = await _db.Vehicles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (v is null) return NotFound();
        return Ok(await BuildDetailAsync(v));
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDetailDto>> Create(VehicleSaveRequest req)
    {
        if (!Validate(req, out var error)) return BadRequest(new { error });

        var stationId = _tenant.StationId
            ?? await _db.Stations.Where(s => s.IsActive).OrderBy(s => s.Id).Select(s => (int?)s.Id).FirstOrDefaultAsync();
        if (stationId is null) return BadRequest(new { error = "No Station configured." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var v = new Vehicle
        {
            StationId = stationId.Value,
            CreatedByUserId = userId,
            LastModifiedByUserId = userId,
        };
        Apply(v, req);
        _db.Vehicles.Add(v);
        await _db.SaveChangesAsync();
        await ReplaceChildrenAsync(v.Id, req);
        var detail = await BuildDetailAsync(v);
        return CreatedAtAction(nameof(Get), new { id = v.Id }, detail);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<VehicleDetailDto>> Update(long id, VehicleSaveRequest req)
    {
        if (!Validate(req, out var error)) return BadRequest(new { error });
        var v = await _db.Vehicles.FindAsync(id);
        if (v is null) return NotFound();
        Apply(v, req);
        v.LastModifiedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _db.SaveChangesAsync();
        await ReplaceChildrenAsync(id, req);
        return Ok(await BuildDetailAsync(v));
    }

    // ---- child collection helpers ----

    private async Task<VehicleDetailDto> BuildDetailAsync(Vehicle v)
    {
        var regs = await _db.VehicleRegistrations.AsNoTracking()
            .Where(r => r.VehicleId == v.Id).OrderByDescending(r => r.Id)
            .Select(r => new RegistrationDto(r.Id, r.IssuerId, r.RegistrationNumber, r.MakeDate, r.ValidTill))
            .ToListAsync();
        var axes = await _db.VehicleAxles.AsNoTracking()
            .Where(a => a.VehicleId == v.Id).OrderBy(a => a.AxleNumber)
            .Select(a => new AxleDto(a.Id, a.AxleNumber, a.IsPropulsion, a.IsSteering, a.Note))
            .ToListAsync();
        var dists = await _db.VehicleAxleDistances.AsNoTracking()
            .Where(d => d.VehicleId == v.Id).OrderBy(d => d.FromAxleNumber)
            .Select(d => new AxleDistanceDto(d.Id, d.FromAxleNumber, d.ToAxleNumber, d.Distance))
            .ToListAsync();
        var tyres = await _db.VehicleTyres.AsNoTracking()
            .Where(t => t.VehicleId == v.Id).OrderBy(t => t.Id)
            .Select(t => new TyreDto(t.Id, t.TireTypeId, t.PositionNote, t.Dimensions, t.PressureFront, t.PressureRear))
            .ToListAsync();
        return new VehicleDetailDto(v, regs, axes, dists, tyres);
    }

    private async Task ReplaceChildrenAsync(long vehicleId, VehicleSaveRequest req)
    {
        if (req.Registrations is not null)
        {
            _db.VehicleRegistrations.RemoveRange(_db.VehicleRegistrations.Where(r => r.VehicleId == vehicleId));
            foreach (var r in req.Registrations.Where(x => !string.IsNullOrWhiteSpace(x.RegistrationNumber)))
                _db.VehicleRegistrations.Add(new VehicleRegistration
                {
                    VehicleId = vehicleId, IssuerId = r.IssuerId,
                    RegistrationNumber = r.RegistrationNumber.Trim(),
                    MakeDate = r.MakeDate, ValidTill = r.ValidTill,
                });
        }
        if (req.Axles is not null)
        {
            _db.VehicleAxles.RemoveRange(_db.VehicleAxles.Where(a => a.VehicleId == vehicleId));
            foreach (var a in req.Axles.Where(x => x.AxleNumber > 0))
                _db.VehicleAxles.Add(new VehicleAxle
                {
                    VehicleId = vehicleId, AxleNumber = a.AxleNumber,
                    IsPropulsion = a.IsPropulsion, IsSteering = a.IsSteering,
                    Note = string.IsNullOrWhiteSpace(a.Note) ? null : a.Note.Trim(),
                });
        }
        if (req.AxleDistances is not null)
        {
            _db.VehicleAxleDistances.RemoveRange(_db.VehicleAxleDistances.Where(d => d.VehicleId == vehicleId));
            foreach (var d in req.AxleDistances)
                _db.VehicleAxleDistances.Add(new VehicleAxleDistance
                {
                    VehicleId = vehicleId, FromAxleNumber = d.FromAxleNumber, ToAxleNumber = d.ToAxleNumber,
                    Distance = d.Distance,
                });
        }
        if (req.Tyres is not null)
        {
            _db.VehicleTyres.RemoveRange(_db.VehicleTyres.Where(t => t.VehicleId == vehicleId));
            foreach (var t in req.Tyres)
                _db.VehicleTyres.Add(new VehicleTyre
                {
                    VehicleId = vehicleId, TireTypeId = t.TireTypeId,
                    PositionNote = string.IsNullOrWhiteSpace(t.PositionNote) ? null : t.PositionNote.Trim(),
                    Dimensions = string.IsNullOrWhiteSpace(t.Dimensions) ? null : t.Dimensions.Trim(),
                    PressureFront = t.PressureFront, PressureRear = t.PressureRear,
                });
        }
        await _db.SaveChangesAsync();
    }

    private static bool Validate(VehicleSaveRequest r, out string error)
    {
        if (string.IsNullOrWhiteSpace(r.ShellNumber) || r.ShellNumber.Length < 4)
        { error = "BR-VEH-002: ShellNumber min length is 4."; return false; }
        if (r.ShellNumber.Length > 17)
        { error = "BR-VEH-001: ShellNumber max length is 17."; return false; }
        if (string.IsNullOrWhiteSpace(r.FirstRegistrationNumber))
        { error = "BR-VEH-004: FirstRegistrationNumber is required."; return false; }
        if (string.IsNullOrWhiteSpace(r.LastRegistrationNumber))
        { error = "BR-VEH-005: LastRegistrationNumber is required."; return false; }
        if (r.NumberOfPropulsionAxes is not null && r.NumberOfAxes is not null && r.NumberOfPropulsionAxes > r.NumberOfAxes)
        { error = "BR-VEH-006: Propulsion axes cannot exceed total axes."; return false; }
        if (r.NumberOfPropulsionWheels is not null && r.NumberOfWheels is not null && r.NumberOfPropulsionWheels > r.NumberOfWheels)
        { error = "BR-VEH-007: Propulsion wheels cannot exceed total wheels."; return false; }
        error = ""; return true;
    }

    private static void Apply(Vehicle v, VehicleSaveRequest r)
    {
        v.ShellNumber               = r.ShellNumber;
        v.FirstRegistrationNumber   = r.FirstRegistrationNumber;
        v.LastRegistrationNumber    = r.LastRegistrationNumber;
        v.BodyTypeId                = r.BodyTypeId;
        v.VehicleCategoryId         = r.VehicleCategoryId;
        v.VehicleUseId              = r.VehicleUseId;
        v.VehicleModelId            = r.VehicleModelId;
        v.VehicleModelAdding        = r.VehicleModelAdding;
        v.MadeCountryId             = r.MadeCountryId;
        v.VehicleCategoryForPaymentsId = r.VehicleCategoryForPaymentsId;
        v.EngineNumber              = r.EngineNumber;
        v.EngineTypeId              = r.EngineTypeId;
        v.EnginePowerSourceId       = r.EnginePowerSourceId;
        v.EngineSecondPowerSourceId = r.EngineSecondPowerSourceId;
        v.EngineEcoProgramId        = r.EngineEcoProgramId;
        v.EnginePowerKw             = r.EnginePowerKw;
        v.EngineWorkingCapacity     = r.EngineWorkingCapacity;
        v.RPM                       = r.RPM;
        v.GearBoxId                 = r.GearBoxId;
        v.BrakesId                  = r.BrakesId;
        v.SupportingId              = r.SupportingId;
        v.VehicleHeight             = r.VehicleHeight;
        v.VehicleWidth              = r.VehicleWidth;
        v.VehicleLength             = r.VehicleLength;
        v.NumberOfDoors             = r.NumberOfDoors;
        v.NumberOfSeats             = r.NumberOfSeats;
        v.NumberOfStandingSeats     = r.NumberOfStandingSeats;
        v.NumberOfLyingSeats        = r.NumberOfLyingSeats;
        v.NumberOfAxes              = r.NumberOfAxes;
        v.NumberOfPropulsionAxes    = r.NumberOfPropulsionAxes;
        v.NumberOfWheels            = r.NumberOfWheels;
        v.NumberOfPropulsionWheels  = r.NumberOfPropulsionWheels;
        v.EmptyWeight               = r.EmptyWeight;
        v.MaxAllowedWeight          = r.MaxAllowedWeight;
        v.MakeDate                  = r.MakeDate;
        v.FirstRegistrationMakeDate = r.FirstRegistrationMakeDate;
        v.FirstRegistrationValidTill = r.FirstRegistrationValidTill;
        v.LastRegistrationMakeDate  = r.LastRegistrationMakeDate;
        v.LastRegistrationValidTill = r.LastRegistrationValidTill;
        v.FirstRegistrationIssuerId = r.FirstRegistrationIssuerId;
        v.LastRegistrationIssuerId  = r.LastRegistrationIssuerId;
        v.ColorCode                 = r.ColorCode;
        v.PrimaryColorId            = r.PrimaryColorId;
        v.SecondaryColorId          = r.SecondaryColorId;
        v.Suffocation               = r.Suffocation;
        v.Hook                      = r.Hook;
        v.Winch                     = r.Winch;
        v.TNG                       = r.TNG;
        v.Note                      = r.Note;
    }
}
