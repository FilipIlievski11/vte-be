using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Payments;
using VTE.Domain.TechnicalExams;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Pricing;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

/// <summary>
/// Read API over the migrated technical-exam reports (Технички преглед).
/// Mirrors the legacy uxDocumentTehnicalExamReports form: a searchable register
/// (list) and a full single-report view (header + brake/emission measurements +
/// defective-parts lines). Tenant-scoped via the EF query filter on the report.
/// </summary>
[ApiController]
[Route("api/technical-exams")]
[Authorize]
public class TechnicalExamReportsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    private readonly IDebtService _debts;

    public TechnicalExamReportsController(VteDbContext db, ITenantContext tenant, IDebtService debts)
    {
        _db = db;
        _tenant = tenant;
        _debts = debts;
    }

    // ---- DTOs ----

    public record TechExamListItem(
        long Id, byte CompanyId,
        string? RegNumber, DateOnly MadeDate, DateOnly ValidTillDate,
        int TechnicalExamTypeId, string? TypeCode, string? TypeName,
        int OrganizationId, string? OrganizationName,
        long? CustomerVehicleRelationId, string? ClientName, string? VehiclePlate, string? VehicleVin,
        bool VehicleIsRight, bool Active);

    public record AxleReadingDto(int Axle, double? Left, double? Right, double? Gj, double? LeftRightDiff, double? Coefficient);

    public record TechExamDetailLineDto(
        long Id, int VehiclePartId, string? PartCode, string? PartName,
        int StatusId, string? StatusName,
        bool Front, bool Back, bool OnLeft, bool OnRight,
        DateTime EnteredAt, string? Note);

    public record TechExamReportFullDto(
        long Id, byte CompanyId,
        long? CustomerVehicleRelationId, string? ClientName, string? ClientMB,
        long? VehicleId, string? VehiclePlate, string? VehicleVin, string? VehicleMakerModel,
        int TechnicalExamTypeId, string? TypeCode, string? TypeName, int TypeValidDays,
        int OrganizationId, string? OrganizationName, string? OrganizationCode,
        string? RegNumber, DateOnly MadeDate, DateOnly ValidTillDate,
        int? FirstControllerLegacyId, int? SecondControllerLegacyId,
        string? FirstControllerName, string? SecondControllerName,
        bool VehicleIsRight,
        string? ExplanationNote, string? DriversWarning, string? Note, string? TechnicalChanges,
        IReadOnlyList<AxleReadingDto> Axles,
        double? Weight,
        double? EffectOfWorkingBrakeEmpty, double? EffectOfWorkingBrakeFull,
        double? EffectOfSecondaryBrake, double? EffectOfParkingBrake,
        double? Co, double? CoPlusTurns, double? EngineRpm, double? Lambda,
        double? Pinpoints, double? SpeedOfTurns, double? Noise, double? EngineOilTemp,
        DateTime CreatedAt, DateTime? ModifiedAt,
        IReadOnlyList<TechExamDetailLineDto> Details);

    public record TechExamTypeDto(int Id, string? Code, string Description, int ValidDays);

    // ---- List (register) ----

    [HttpGet]
    public async Task<ActionResult<PagedDto<TechExamListItem>>> List(
        [FromQuery] string? q = null,
        [FromQuery] string? result = "all",      // "all" | "pass" | "fail"
        [FromQuery] int? typeId = null,
        [FromQuery] long? customerVehicleRelationId = null,
        [FromQuery] long? vehicleId = null,
        [FromQuery] bool includeInactive = false,
        [FromQuery] string? sort = null,
        [FromQuery] string? dir = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 50;

        var query = _db.TechnicalExamReports.AsNoTracking().AsQueryable();

        if (!includeInactive) query = query.Where(r => r.Active);
        if (typeId.HasValue) query = query.Where(r => r.TechnicalExamTypeId == typeId.Value);
        if (customerVehicleRelationId.HasValue)
            query = query.Where(r => r.CustomerVehicleRelationId == customerVehicleRelationId.Value);
        // Vehicle history: exams hang off relations, so a vehicle's exams span ALL
        // of its relations (current + historical owners).
        if (vehicleId.HasValue)
        {
            var relIds = _db.ClientVehicleRelations.AsNoTracking()
                .Where(cvr => cvr.VehicleId == vehicleId.Value)
                .Select(cvr => (long?)cvr.Id);
            query = query.Where(r => relIds.Contains(r.CustomerVehicleRelationId));
        }

        var res = (result ?? "all").Trim().ToLowerInvariant();
        if (res == "pass")      query = query.Where(r => r.VehicleIsRight);
        else if (res == "fail") query = query.Where(r => !r.VehicleIsRight);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q.Trim()}%";

            var matchingClientIds = _db.Clients.AsNoTracking()
                .Where(c =>
                    (c.FirstName != null && EF.Functions.Like(c.FirstName, like)) ||
                    (c.MiddleName != null && EF.Functions.Like(c.MiddleName, like)) ||
                    (c.LastName != null && EF.Functions.Like(c.LastName, like)) ||
                    (c.MB != null && EF.Functions.Like(c.MB, like)))
                .Select(c => c.Id);
            var relationsByClient = _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => matchingClientIds.Contains(r.ClientId))
                .Select(r => r.Id);

            var matchingVehicleIds = _db.Vehicles.AsNoTracking()
                .Where(v => EF.Functions.Like(v.Vin, like) || (v.Plate != null && EF.Functions.Like(v.Plate, like)))
                .Select(v => v.Id);
            var relationsByVehicle = _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => r.VehicleId != null && matchingVehicleIds.Contains(r.VehicleId!.Value))
                .Select(r => r.Id);

            var unionRelationIds = relationsByClient.Union(relationsByVehicle);

            query = query.Where(r =>
                (r.RegNumber != null && EF.Functions.Like(r.RegNumber, like)) ||
                (r.CustomerVehicleRelationId != null && unionRelationIds.Contains(r.CustomerVehicleRelationId!.Value)));
        }

        var total = await query.CountAsync();

        var asc = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase);
        IQueryable<Domain.TechnicalExams.TechnicalExamReport> Order<TKey>(
            System.Linq.Expressions.Expression<Func<Domain.TechnicalExams.TechnicalExamReport, TKey>> key)
            => asc ? query.OrderBy(key) : query.OrderByDescending(key);
        var ordered = (sort ?? "").Trim().ToLowerInvariant() switch
        {
            "id"        => Order(r => r.Id),
            "regnumber" => Order(r => r.RegNumber),
            "validtill" => Order(r => r.ValidTillDate),
            "result"    => Order(r => r.VehicleIsRight),
            "type"      => Order(r => _db.TechnicalExamTypes.Where(t => t.Id == r.TechnicalExamTypeId).Select(t => (string?)t.Code).FirstOrDefault()),
            _           => Order(r => r.MadeDate),   // default: newest exam first
        };

        var pageRows = await ordered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new
            {
                r.Id, r.CompanyId, r.RegNumber, r.MadeDate, r.ValidTillDate,
                r.TechnicalExamTypeId, r.OrganizationId, r.CustomerVehicleRelationId,
                r.VehicleIsRight, r.Active,
            })
            .ToListAsync();

        // Batch-resolve joins
        var typeIds = pageRows.Select(r => r.TechnicalExamTypeId).Distinct().ToList();
        var orgIds = pageRows.Select(r => r.OrganizationId).Distinct().ToList();
        var relationIds = pageRows.Where(r => r.CustomerVehicleRelationId.HasValue)
            .Select(r => r.CustomerVehicleRelationId!.Value).Distinct().ToList();

        var types = await _db.TechnicalExamTypes.AsNoTracking()
            .Where(t => typeIds.Contains(t.Id))
            .Select(t => new { t.Id, t.Code, t.Description })
            .ToDictionaryAsync(t => t.Id);
        var orgs = await _db.TechnicalExamOrganizations.AsNoTracking()
            .Where(o => orgIds.Contains(o.Id))
            .Select(o => new { o.Id, o.Name })
            .ToDictionaryAsync(o => o.Id);

        var relations = await _db.ClientVehicleRelations.AsNoTracking()
            .Where(r => relationIds.Contains(r.Id))
            .Select(r => new { r.Id, r.ClientId, r.VehicleId })
            .ToDictionaryAsync(r => r.Id);
        var clientIds = relations.Values.Select(r => r.ClientId).Distinct().ToList();
        var vehicleIds = relations.Values.Where(r => r.VehicleId.HasValue).Select(r => r.VehicleId!.Value).Distinct().ToList();
        var clients = await _db.Clients.AsNoTracking()
            .Where(c => clientIds.Contains(c.Id))
            .Select(c => new { c.Id, c.FirstName, c.MiddleName, c.LastName })
            .ToDictionaryAsync(c => c.Id);
        var vehicles = await _db.Vehicles.AsNoTracking()
            .Where(v => vehicleIds.Contains(v.Id))
            .Select(v => new { v.Id, v.Vin, v.Plate })
            .ToDictionaryAsync(v => v.Id);

        string? clientName(long cid) =>
            clients.TryGetValue(cid, out var c)
                ? (string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }.Where(x => !string.IsNullOrWhiteSpace(x))) is var n && n.Length > 0 ? n : null)
                : null;

        var items = pageRows.Select(r =>
        {
            string? cName = null, plate = null, vin = null;
            if (r.CustomerVehicleRelationId.HasValue && relations.TryGetValue(r.CustomerVehicleRelationId.Value, out var rel))
            {
                cName = clientName(rel.ClientId);
                if (rel.VehicleId.HasValue && vehicles.TryGetValue(rel.VehicleId.Value, out var v))
                {
                    plate = v.Plate;
                    vin = v.Vin;
                }
            }
            types.TryGetValue(r.TechnicalExamTypeId, out var ty);
            orgs.TryGetValue(r.OrganizationId, out var og);
            return new TechExamListItem(
                r.Id, r.CompanyId, r.RegNumber, r.MadeDate, r.ValidTillDate,
                r.TechnicalExamTypeId, ty?.Code, ty?.Description,
                r.OrganizationId, og?.Name,
                r.CustomerVehicleRelationId, cName, plate, vin,
                r.VehicleIsRight, r.Active);
        }).ToList();

        return Ok(new PagedDto<TechExamListItem>(page, pageSize, total, items));
    }

    // ---- Detail (single report) ----

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TechExamReportFullDto>> Get(long id)
    {
        var r = await _db.TechnicalExamReports.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();

        var type = await _db.TechnicalExamTypes.AsNoTracking()
            .Where(t => t.Id == r.TechnicalExamTypeId)
            .Select(t => new { t.Code, t.Description, t.ValidDays }).FirstOrDefaultAsync();
        var org = await _db.TechnicalExamOrganizations.AsNoTracking()
            .Where(o => o.Id == r.OrganizationId)
            .Select(o => new { o.Name, o.Code }).FirstOrDefaultAsync();

        // Client + vehicle via the anchor relation.
        string? clientName = null, clientMB = null, plate = null, vin = null, makerModel = null;
        long? vehicleId = null;
        if (r.CustomerVehicleRelationId.HasValue)
        {
            var rel = await _db.ClientVehicleRelations.AsNoTracking()
                .Where(x => x.Id == r.CustomerVehicleRelationId.Value)
                .Select(x => new { x.ClientId, x.VehicleId }).FirstOrDefaultAsync();
            if (rel != null)
            {
                var c = await _db.Clients.AsNoTracking()
                    .Where(x => x.Id == rel.ClientId)
                    .Select(x => new { x.FirstName, x.MiddleName, x.LastName, x.MB }).FirstOrDefaultAsync();
                if (c != null)
                {
                    var n = string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
                    clientName = n.Length > 0 ? n : null;
                    clientMB = c.MB;
                }
                vehicleId = rel.VehicleId;
                if (rel.VehicleId.HasValue)
                {
                    var v = await _db.Vehicles.AsNoTracking()
                        .Where(x => x.Id == rel.VehicleId.Value)
                        .Select(x => new { x.Vin, x.Plate, x.ModelId }).FirstOrDefaultAsync();
                    if (v != null)
                    {
                        plate = v.Plate; vin = v.Vin;
                        if (v.ModelId.HasValue)
                        {
                            var m = await _db.VehicleModels.AsNoTracking()
                                .Where(x => x.Id == v.ModelId.Value)
                                .Select(x => new { x.Name, x.MakerId }).FirstOrDefaultAsync();
                            if (m != null)
                            {
                                var maker = await _db.VehicleMakers.AsNoTracking()
                                    .Where(x => x.Id == m.MakerId).Select(x => x.Name).FirstOrDefaultAsync();
                                makerModel = string.Join(' ', new[] { maker, m.Name }.Where(x => !string.IsNullOrWhiteSpace(x)));
                                if (makerModel.Length == 0) makerModel = null;
                            }
                        }
                    }
                }
            }
        }

        // Defect lines + part/status names.
        var linesRaw = await _db.TechnicalExamReportDetails.AsNoTracking()
            .Where(d => d.TechnicalExamReportId == r.Id)
            .OrderBy(d => d.Id)
            .Select(d => new
            {
                d.Id, d.VehiclePartId, d.StatusId,
                d.Front, d.Back, d.OnLeft, d.OnRight, d.EnteredAt, d.Note,
            })
            .ToListAsync();
        var partIds = linesRaw.Select(l => l.VehiclePartId).Distinct().ToList();
        var statusIds = linesRaw.Select(l => l.StatusId).Distinct().ToList();
        var parts = await _db.TechnicalExamVehicleParts.AsNoTracking()
            .Where(p => partIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Code, p.Description })
            .ToDictionaryAsync(p => p.Id);
        var statuses = await _db.TechnicalExamDetailStatuses.AsNoTracking()
            .Where(s => statusIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.Name);
        var lines = linesRaw.Select(l =>
        {
            parts.TryGetValue(l.VehiclePartId, out var p);
            return new TechExamDetailLineDto(
                l.Id, l.VehiclePartId, p?.Code, p?.Description,
                l.StatusId, statuses.GetValueOrDefault(l.StatusId),
                l.Front, l.Back, l.OnLeft, l.OnRight, l.EnteredAt, l.Note);
        }).ToList();

        // Controllers (inspectors) — recreated as v2 users keyed by their legacy id
        // (AspNetUsers.Id == legacy VTESecurity user id). Resolve to display names.
        var ctrlIds = new[] { r.FirstControllerLegacyId, r.SecondControllerLegacyId }
            .Where(x => x.HasValue).Select(x => x!.Value.ToString()).Distinct().ToList();
        var ctrlNames = ctrlIds.Count == 0
            ? new Dictionary<string, string?>()
            : await _db.Users.AsNoTracking()
                .Where(u => ctrlIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.FullName ?? u.UserName);
        string? ctrlName(int? legacyId) =>
            legacyId.HasValue && ctrlNames.TryGetValue(legacyId.Value.ToString(), out var n) ? n : null;

        // Brake-force grid → 5 axle rows (1-4 + parking=0).
        var axles = new List<AxleReadingDto>
        {
            new(1, r.Axis1Left, r.Axis1Right, r.Axis1Gj, r.Axis1LeftRightDiff, r.Axis1Coefficient),
            new(2, r.Axis2Left, r.Axis2Right, r.Axis2Gj, r.Axis2LeftRightDiff, r.Axis2Coefficient),
            new(3, r.Axis3Left, r.Axis3Right, r.Axis3Gj, r.Axis3LeftRightDiff, r.Axis3Coefficient),
            new(4, r.Axis4Left, r.Axis4Right, r.Axis4Gj, r.Axis4LeftRightDiff, r.Axis4Coefficient),
            new(0, r.AxisParkingLeft, r.AxisParkingRight, r.AxisParkingGj, r.AxisParkingLeftRightDiff, r.AxisParkingCoefficient),
        };

        return Ok(new TechExamReportFullDto(
            r.Id, r.CompanyId,
            r.CustomerVehicleRelationId, clientName, clientMB,
            vehicleId, plate, vin, makerModel,
            r.TechnicalExamTypeId, type?.Code, type?.Description, type?.ValidDays ?? 0,
            r.OrganizationId, org?.Name, org?.Code,
            r.RegNumber, r.MadeDate, r.ValidTillDate,
            r.FirstControllerLegacyId, r.SecondControllerLegacyId,
            ctrlName(r.FirstControllerLegacyId), ctrlName(r.SecondControllerLegacyId),
            r.VehicleIsRight,
            r.ExplanationNote, r.DriversWarning, r.Note, r.TechnicalChanges,
            axles,
            r.Weight,
            r.EffectOfWorkingBrakeEmpty, r.EffectOfWorkingBrakeFull,
            r.EffectOfSecondaryBrake, r.EffectOfParkingBrake,
            r.CO, r.COPlusTurns, r.EngineRpm, r.Lambda,
            r.Pinpoints, r.SpeedOfTurns, r.Noise, r.EngineOilTemp,
            r.CreatedAt, r.ModifiedAt,
            lines));
    }

    // ---- Lookups (for the list type filter) ----

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<TechExamTypeDto>>> Types()
    {
        var types = await _db.TechnicalExamTypes.AsNoTracking()
            .Where(t => t.Active)
            .OrderBy(t => t.Id)
            .Select(t => new TechExamTypeDto(t.Id, t.Code, t.Description, t.ValidDays))
            .ToListAsync();
        return Ok(types);
    }

    // ---- Certificate print bundle (legacy „Потврда за техничка исправност") ----

    public record CertOrgDto(string? Name, string? Address, string? CityLine, string? Phone, string? Fax);
    public record CertVehicleDto(string? Registration, string? Category, string? Maker, string? TypeText, string? Model, string? Vin);
    public record TechExamCertificateDto(
        long Id, string? RegNumber, DateOnly MadeDate, DateOnly ValidTillDate, bool VehicleIsRight,
        string? StationCity, string? ControllerName, CertOrgDto Organization, CertVehicleDto Vehicle);

    [HttpGet("{id:long}/print")]
    public async Task<ActionResult<TechExamCertificateDto>> Print(long id)
    {
        var r = await _db.TechnicalExamReports.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();

        // Issuing organization letterhead.
        var org = await _db.TechnicalExamOrganizations.AsNoTracking()
            .Where(o => o.Id == r.OrganizationId)
            .Select(o => new { o.Name, o.Address, o.Phone, o.Fax, o.CityId })
            .FirstOrDefaultAsync();
        string? cityLine = null, stationCity = null;
        if (org?.CityId != null)
        {
            var city = await _db.Cities.AsNoTracking()
                .Where(c => c.Id == org.CityId.Value)
                .Select(c => new { c.Name, c.PostalCode }).FirstOrDefaultAsync();
            if (city != null)
            {
                stationCity = city.Name;
                cityLine = string.Join(' ', new[] { city.PostalCode, city.Name }.Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }

        // Vehicle (A/Ј/D.1/D.2/D.3/E) via the anchor relation.
        string? registration = null, category = null, maker = null, typeText = null, model = null, vin = null;
        if (r.CustomerVehicleRelationId.HasValue)
        {
            var rel = await _db.ClientVehicleRelations.AsNoTracking()
                .Where(x => x.Id == r.CustomerVehicleRelationId.Value)
                .Select(x => new { x.VehicleId }).FirstOrDefaultAsync();
            if (rel?.VehicleId != null)
            {
                var v = await _db.Vehicles.AsNoTracking()
                    .Where(x => x.Id == rel.VehicleId.Value)
                    .Select(x => new { x.Plate, x.Vin, x.TypeText, x.ModelId, x.CategoryId })
                    .FirstOrDefaultAsync();
                if (v != null)
                {
                    registration = v.Plate; vin = v.Vin; typeText = v.TypeText;
                    if (v.CategoryId.HasValue)
                    {
                        var c = await _db.VehicleCategories.AsNoTracking()
                            .Where(x => x.Id == v.CategoryId.Value)
                            .Select(x => new { x.Code, x.Name }).FirstOrDefaultAsync();
                        if (c != null) category = string.IsNullOrWhiteSpace(c.Code) ? c.Name : $"{c.Code} {c.Name}";
                    }
                    if (v.ModelId.HasValue)
                    {
                        var m = await _db.VehicleModels.AsNoTracking()
                            .Where(x => x.Id == v.ModelId.Value)
                            .Select(x => new { x.Name, x.MakerId }).FirstOrDefaultAsync();
                        if (m != null)
                        {
                            model = m.Name;
                            maker = await _db.VehicleMakers.AsNoTracking()
                                .Where(x => x.Id == m.MakerId).Select(x => x.Name).FirstOrDefaultAsync();
                        }
                    }
                }
            }
        }

        // First inspector's name (recreated as a v2 user keyed by legacy id) for the signature.
        string? controllerName = null;
        if (r.FirstControllerLegacyId.HasValue)
        {
            var cid = r.FirstControllerLegacyId.Value.ToString();
            controllerName = await _db.Users.AsNoTracking()
                .Where(u => u.Id == cid).Select(u => u.FullName ?? u.UserName).FirstOrDefaultAsync();
        }

        return Ok(new TechExamCertificateDto(
            r.Id, r.RegNumber, r.MadeDate, r.ValidTillDate, r.VehicleIsRight,
            stationCity, controllerName,
            new CertOrgDto(org?.Name, org?.Address, cityLine, org?.Phone, org?.Fax),
            new CertVehicleDto(registration, category, maker, typeText, model, vin)));
    }

    // ---- Записник print bundle (legacy rptTehnickiPregledZapisnik, stamped on pre-printed paper) ----

    public record ZapisnikDto(
        long Id, string? RegNumber, DateOnly MadeDate, int TechnicalExamTypeId, bool IsSocial,
        string? OrganizationName,
        string? CustomerName, string? CityName, string? CommunityName, string? LivingAddress,
        string? Plate, string? Maker, string? ModelFull, int? MakeYear, string? MadeCountry,
        string? ColorFull, string? Vin, string? EngineTypeAndNum,
        double? EngineCapacityCc, double? EnginePowerKw, double? EmptyWeightKg, double? MaxAllowedWeightKg,
        int? AxleCount, int? PropulsionAxis, short? Seats);

    [HttpGet("{id:long}/zapisnik")]
    public async Task<ActionResult<ZapisnikDto>> Zapisnik(long id)
    {
        var r = await _db.TechnicalExamReports.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();

        var orgName = await _db.TechnicalExamOrganizations.AsNoTracking()
            .Where(o => o.Id == r.OrganizationId).Select(o => o.Name).FirstOrDefaultAsync();

        string? customerName = null, cityName = null, communityName = null, livingAddress = null;
        bool isSocial = false;
        string? plate = null, maker = null, modelFull = null, madeCountry = null, colorFull = null, vin = null, engineTypeAndNum = null;
        int? makeYear = null, axleCount = null;
        double? capCc = null, powKw = null, emptyKg = null, maxKg = null;
        short? seats = null;

        if (r.CustomerVehicleRelationId.HasValue)
        {
            var rel = await _db.ClientVehicleRelations.AsNoTracking()
                .Where(x => x.Id == r.CustomerVehicleRelationId.Value)
                .Select(x => new { x.ClientId, x.VehicleId }).FirstOrDefaultAsync();
            if (rel != null)
            {
                var c = await _db.Clients.AsNoTracking().Where(x => x.Id == rel.ClientId)
                    .Select(x => new { x.FirstName, x.MiddleName, x.LastName, x.Address, x.Business, x.CityId })
                    .FirstOrDefaultAsync();
                if (c != null)
                {
                    var n = string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
                    customerName = n.Length > 0 ? n : null;
                    livingAddress = c.Address;
                    isSocial = c.Business == true;   // best available: company → "social", individual → "private"
                    if (c.CityId.HasValue)
                    {
                        var cy = await _db.Cities.AsNoTracking().Where(x => x.Id == c.CityId.Value)
                            .Select(x => new { x.Name, x.CommunityId }).FirstOrDefaultAsync();
                        if (cy != null)
                        {
                            cityName = cy.Name;
                            communityName = await _db.Communities.AsNoTracking().Where(x => x.Id == cy.CommunityId).Select(x => x.Name).FirstOrDefaultAsync();
                        }
                    }
                }
                if (rel.VehicleId.HasValue)
                {
                    var v = await _db.Vehicles.AsNoTracking().Where(x => x.Id == rel.VehicleId.Value)
                        .Select(x => new
                        {
                            x.Plate, x.Vin, x.EngineNumber, x.ModelId, x.ModelVariant, x.HasLpg, x.ManufactureDate,
                            x.MadeCountryId, x.PrimaryColorId, x.EngineTypeId,
                            x.EngineWorkingCapacityCc, x.EnginePowerKw, x.EmptyWeightKg, x.MaxAllowedWeightKg, x.AxleCount, x.Seats
                        }).FirstOrDefaultAsync();
                    if (v != null)
                    {
                        plate = v.Plate; vin = v.Vin;
                        capCc = v.EngineWorkingCapacityCc; powKw = v.EnginePowerKw;
                        emptyKg = v.EmptyWeightKg; maxKg = v.MaxAllowedWeightKg;
                        axleCount = v.AxleCount; seats = v.Seats; makeYear = v.ManufactureDate?.Year;

                        string? modelName = null;
                        if (v.ModelId.HasValue)
                        {
                            var m = await _db.VehicleModels.AsNoTracking().Where(x => x.Id == v.ModelId.Value)
                                .Select(x => new { x.Name, x.MakerId }).FirstOrDefaultAsync();
                            if (m != null)
                            {
                                modelName = m.Name;
                                maker = await _db.VehicleMakers.AsNoTracking().Where(x => x.Id == m.MakerId).Select(x => x.Name).FirstOrDefaultAsync();
                            }
                        }
                        modelFull = string.Join(' ', new[] { modelName, v.ModelVariant, (v.HasLpg == true ? "ТНГ" : null) }.Where(s => !string.IsNullOrWhiteSpace(s)));
                        if (string.IsNullOrWhiteSpace(modelFull)) modelFull = null;

                        if (v.MadeCountryId.HasValue)
                            madeCountry = await _db.Countries.AsNoTracking().Where(x => x.Id == v.MadeCountryId.Value).Select(x => x.Name).FirstOrDefaultAsync();
                        if (v.PrimaryColorId.HasValue)
                        {
                            var col = await _db.VehicleColors.AsNoTracking().Where(x => x.Id == v.PrimaryColorId.Value).Select(x => new { x.Code, x.Name }).FirstOrDefaultAsync();
                            if (col != null) colorFull = string.IsNullOrWhiteSpace(col.Code) ? col.Name : $"{col.Code}-{col.Name}";
                        }
                        string? engCode = null;
                        if (v.EngineTypeId.HasValue)
                            engCode = await _db.VehicleEngineTypes.AsNoTracking().Where(x => x.Id == v.EngineTypeId.Value).Select(x => x.Code).FirstOrDefaultAsync();
                        engineTypeAndNum = (!string.IsNullOrWhiteSpace(engCode) && !string.IsNullOrWhiteSpace(v.EngineNumber))
                            ? $"{engCode}/{v.EngineNumber}" : (v.EngineNumber ?? engCode);
                    }
                }
            }
        }

        return Ok(new ZapisnikDto(
            r.Id, r.RegNumber, r.MadeDate, r.TechnicalExamTypeId, isSocial,
            orgName, customerName, cityName, communityName, livingAddress,
            plate, maker, modelFull, makeYear, madeCountry, colorFull, vin, engineTypeAndNum,
            capCc, powKw, emptyKg, maxKg, axleCount, null /* PropulsionAxis not in v2 */, seats));
    }

    // ---- Lookups for the create/edit form ----

    public record OrgLookupDto(int Id, string? Code, string? Name, byte? CompanyId);
    public record StatusLookupDto(int Id, string Name);
    public record PartLookupDto(int Id, int CategoryId, string Code, string Description);
    public record ControllerLookupDto(int Id, string FullName);

    [HttpGet("organizations")]
    public async Task<ActionResult<IReadOnlyList<OrgLookupDto>>> Organizations() =>
        Ok(await _db.TechnicalExamOrganizations.AsNoTracking().Where(o => o.Active)
            .OrderBy(o => o.Name)
            .Select(o => new OrgLookupDto(o.Id, o.Code, o.Name, o.CompanyId)).ToListAsync());

    [HttpGet("detail-statuses")]
    public async Task<ActionResult<IReadOnlyList<StatusLookupDto>>> DetailStatuses() =>
        Ok(await _db.TechnicalExamDetailStatuses.AsNoTracking().Where(s => s.Active)
            .OrderBy(s => s.Id)
            .Select(s => new StatusLookupDto(s.Id, s.Name)).ToListAsync());

    [HttpGet("vehicle-parts")]
    public async Task<ActionResult<IReadOnlyList<PartLookupDto>>> VehicleParts() =>
        Ok(await _db.TechnicalExamVehicleParts.AsNoTracking().Where(p => p.Active)
            .OrderBy(p => p.Description)
            .Select(p => new PartLookupDto(p.Id, p.CategoryId, p.Code, p.Description)).ToListAsync());

    /// <summary>Operator users that can act as inspectors. Only those with a numeric Id
    /// (the recreated legacy operators) — the report stores controllers as legacy int ids.</summary>
    [HttpGet("controllers")]
    public async Task<ActionResult<IReadOnlyList<ControllerLookupDto>>> Controllers()
    {
        var opRoleId = await _db.Roles.AsNoTracking()
            .Where(r => r.NormalizedName == "OPERATOR").Select(r => r.Id).FirstOrDefaultAsync();
        var users = await (from u in _db.Users.AsNoTracking()
                           join ur in _db.UserRoles on u.Id equals ur.UserId
                           where ur.RoleId == opRoleId && u.IsActive
                           select new { u.Id, u.FullName, u.UserName }).ToListAsync();
        var result = users
            .Select(u => new { ok = int.TryParse(u.Id, out var n), n, name = u.FullName ?? u.UserName })
            .Where(x => x.ok)
            .Select(x => new ControllerLookupDto(x.n, x.name ?? $"#{x.n}"))
            .OrderBy(x => x.FullName).ToList();
        return Ok(result);
    }

    // ---- Create / Update / Delete ----

    public record TechExamDetailWriteDto(int VehiclePartId, int StatusId, bool Front, bool Back, bool OnLeft, bool OnRight, string? Note);

    public record TechExamWriteDto(
        long? CustomerVehicleRelationId, int TechnicalExamTypeId, int OrganizationId,
        DateOnly MadeDate, int? FirstControllerLegacyId, int? SecondControllerLegacyId,
        string? ExplanationNote, string? DriversWarning, string? Note, string? TechnicalChanges,
        double? Axis1Left, double? Axis1Right, double? Axis1Gj, double? Axis1LeftRightDiff, double? Axis1Coefficient,
        double? Axis2Left, double? Axis2Right, double? Axis2Gj, double? Axis2LeftRightDiff, double? Axis2Coefficient,
        double? Axis3Left, double? Axis3Right, double? Axis3Gj, double? Axis3LeftRightDiff, double? Axis3Coefficient,
        double? Axis4Left, double? Axis4Right, double? Axis4Gj, double? Axis4LeftRightDiff, double? Axis4Coefficient,
        double? AxisParkingLeft, double? AxisParkingRight, double? AxisParkingGj, double? AxisParkingLeftRightDiff, double? AxisParkingCoefficient,
        double? Weight, double? EffectOfWorkingBrakeEmpty, double? EffectOfWorkingBrakeFull, double? EffectOfSecondaryBrake, double? EffectOfParkingBrake,
        double? SpeedOfTurns, double? Co, double? EngineRpm, double? CoPlusTurns, double? Lambda, double? Pinpoints, double? Noise, double? EngineOilTemp,
        IReadOnlyList<TechExamDetailWriteDto>? Details,
        // Explicit operator verdict (исправен/неисправен). Null → derive from the
        // detail statuses (DerivePass), the historical behavior.
        bool? VehicleIsRight = null);

    private static void ApplyMeasurements(TechnicalExamReport r, TechExamWriteDto d)
    {
        r.Axis1Left=d.Axis1Left; r.Axis1Right=d.Axis1Right; r.Axis1Gj=d.Axis1Gj; r.Axis1LeftRightDiff=d.Axis1LeftRightDiff; r.Axis1Coefficient=d.Axis1Coefficient;
        r.Axis2Left=d.Axis2Left; r.Axis2Right=d.Axis2Right; r.Axis2Gj=d.Axis2Gj; r.Axis2LeftRightDiff=d.Axis2LeftRightDiff; r.Axis2Coefficient=d.Axis2Coefficient;
        r.Axis3Left=d.Axis3Left; r.Axis3Right=d.Axis3Right; r.Axis3Gj=d.Axis3Gj; r.Axis3LeftRightDiff=d.Axis3LeftRightDiff; r.Axis3Coefficient=d.Axis3Coefficient;
        r.Axis4Left=d.Axis4Left; r.Axis4Right=d.Axis4Right; r.Axis4Gj=d.Axis4Gj; r.Axis4LeftRightDiff=d.Axis4LeftRightDiff; r.Axis4Coefficient=d.Axis4Coefficient;
        r.AxisParkingLeft=d.AxisParkingLeft; r.AxisParkingRight=d.AxisParkingRight; r.AxisParkingGj=d.AxisParkingGj; r.AxisParkingLeftRightDiff=d.AxisParkingLeftRightDiff; r.AxisParkingCoefficient=d.AxisParkingCoefficient;
        r.Weight=d.Weight; r.EffectOfWorkingBrakeEmpty=d.EffectOfWorkingBrakeEmpty; r.EffectOfWorkingBrakeFull=d.EffectOfWorkingBrakeFull; r.EffectOfSecondaryBrake=d.EffectOfSecondaryBrake; r.EffectOfParkingBrake=d.EffectOfParkingBrake;
        r.SpeedOfTurns=d.SpeedOfTurns; r.CO=d.Co; r.EngineRpm=d.EngineRpm; r.COPlusTurns=d.CoPlusTurns; r.Lambda=d.Lambda; r.Pinpoints=d.Pinpoints; r.Noise=d.Noise; r.EngineOilTemp=d.EngineOilTemp;
    }

    private async Task<string?> ValidateWriteAsync(TechExamWriteDto dto)
    {
        if (dto.TechnicalExamTypeId <= 0 || !await _db.TechnicalExamTypes.AnyAsync(t => t.Id == dto.TechnicalExamTypeId))
            return "Невалиден тип на технички преглед.";
        if (dto.OrganizationId <= 0 || !await _db.TechnicalExamOrganizations.AnyAsync(o => o.Id == dto.OrganizationId))
            return "Невалидна организација/станица.";
        if (dto.CustomerVehicleRelationId.HasValue && !await _db.ClientVehicleRelations.AnyAsync(r => r.Id == dto.CustomerVehicleRelationId.Value))
            return "Врската сопственик–возило не постои.";
        return null;
    }

    private static bool DerivePass(IReadOnlyList<TechExamDetailWriteDto>? details) =>
        details == null || details.Count == 0 || details.All(d => d.StatusId == 1);

    [HttpPost]
    public async Task<ActionResult<TechExamReportFullDto>> Create([FromBody] TechExamWriteDto dto)
    {
        if (_tenant.UserId is null) return Unauthorized();
        var err = await ValidateWriteAsync(dto);
        if (err != null) return BadRequest(new { error = err });

        var validDays = await _db.TechnicalExamTypes.Where(t => t.Id == dto.TechnicalExamTypeId).Select(t => t.ValidDays).FirstAsync();
        var validTill = dto.MadeDate.AddDays(validDays > 0 ? validDays : 365);

        // RegNumber = {org}-{nextSeq}/{year}; nextSeq continues the org's sequence.
        var prefix = $"{dto.OrganizationId}-";
        var lastRn = await _db.TechnicalExamReports.AsNoTracking()
            .Where(r => r.OrganizationId == dto.OrganizationId && r.RegNumber != null && r.RegNumber.StartsWith(prefix))
            .OrderByDescending(r => r.Id).Select(r => r.RegNumber).FirstOrDefaultAsync();
        int seq = 1;
        if (lastRn != null)
        {
            int dash = lastRn.IndexOf('-'), slash = lastRn.IndexOf('/');
            if (dash >= 0 && slash > dash && int.TryParse(lastRn.Substring(dash + 1, slash - dash - 1), out var n)) seq = n + 1;
        }
        var regNumber = $"{dto.OrganizationId}-{seq}/{DateTime.UtcNow.Year}";

        int? firstCtrl = dto.FirstControllerLegacyId;
        if (firstCtrl is null && int.TryParse(_tenant.UserId, out var uid)) firstCtrl = uid;

        var report = new TechnicalExamReport
        {
            CompanyId = _tenant.CompanyId ?? 4,
            CustomerVehicleRelationId = dto.CustomerVehicleRelationId,
            TechnicalExamTypeId = dto.TechnicalExamTypeId,
            OrganizationId = dto.OrganizationId,
            RegNumber = regNumber,
            MadeDate = dto.MadeDate,
            ValidTillDate = validTill,
            FirstControllerLegacyId = firstCtrl,
            SecondControllerLegacyId = dto.SecondControllerLegacyId,
            VehicleIsRight = dto.VehicleIsRight ?? DerivePass(dto.Details),
            ExplanationNote = dto.ExplanationNote,
            DriversWarning = dto.DriversWarning,
            Note = dto.Note,
            TechnicalChanges = dto.TechnicalChanges,
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };
        ApplyMeasurements(report, dto);
        _db.TechnicalExamReports.Add(report);
        await _db.SaveChangesAsync();

        if (dto.Details != null)
        {
            foreach (var d in dto.Details)
                _db.TechnicalExamReportDetails.Add(new TechnicalExamReportDetail
                {
                    TechnicalExamReportId = report.Id,
                    VehiclePartId = d.VehiclePartId, StatusId = d.StatusId,
                    Front = d.Front, Back = d.Back, OnLeft = d.OnLeft, OnRight = d.OnRight,
                    EnteredAt = DateTime.UtcNow, Note = d.Note, Active = true,
                });
            await _db.SaveChangesAsync();
        }

        // Phase 3: auto-debt creation. Mirrors legacy AddDeptsToCustomer on tech-exam save.
        // Fires only when a vehicle relation is anchored — orphan exams have no one to charge.
        // Trigger differs between regular and irregular exam types (legacy:
        // TrigerdByTechnicalExam vs TrigerdByIrregularTechnicalExam).
        if (report.CustomerVehicleRelationId.HasValue)
        {
            try
            {
                var isIrregular = report.TechnicalExamTypeId > 1;   // legacy convention: type=1 is regular
                await _debts.CreateDebtsForSourceAsync(
                    origin:                     isIrregular ? DebtOrigin.TechnicalExamIrregular : DebtOrigin.TechnicalExam,
                    originId:                   report.Id,
                    customerVehicleRelationId:  report.CustomerVehicleRelationId.Value,
                    organizationId:             report.OrganizationId,
                    trigger:                    isIrregular ? PriceTrigger.TechnicalExamIrregular : PriceTrigger.TechnicalExam,
                    communityId:                null,
                    note:                       $"технички преглед бр. {report.RegNumber}");
            }
            catch
            {
                // Debt creation is best-effort — failure here should not roll back the exam.
                // Phase 5 will move this to a transactional outbox for proper retry semantics.
            }
        }

        return await Get(report.Id);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] TechExamWriteDto dto)
    {
        var report = await _db.TechnicalExamReports.FirstOrDefaultAsync(x => x.Id == id);
        if (report == null) return NotFound();
        var err = await ValidateWriteAsync(dto);
        if (err != null) return BadRequest(new { error = err });

        var validDays = await _db.TechnicalExamTypes.Where(t => t.Id == dto.TechnicalExamTypeId).Select(t => t.ValidDays).FirstAsync();

        report.CustomerVehicleRelationId = dto.CustomerVehicleRelationId;
        report.TechnicalExamTypeId = dto.TechnicalExamTypeId;
        report.OrganizationId = dto.OrganizationId;
        report.MadeDate = dto.MadeDate;
        report.ValidTillDate = dto.MadeDate.AddDays(validDays > 0 ? validDays : 365);
        report.FirstControllerLegacyId = dto.FirstControllerLegacyId;
        report.SecondControllerLegacyId = dto.SecondControllerLegacyId;
        report.ExplanationNote = dto.ExplanationNote;
        report.DriversWarning = dto.DriversWarning;
        report.Note = dto.Note;
        report.TechnicalChanges = dto.TechnicalChanges;
        report.VehicleIsRight = dto.VehicleIsRight ?? DerivePass(dto.Details);
        report.ModifiedAt = DateTime.UtcNow;
        ApplyMeasurements(report, dto);

        // Replace detail lines.
        var old = await _db.TechnicalExamReportDetails.Where(d => d.TechnicalExamReportId == id).ToListAsync();
        _db.TechnicalExamReportDetails.RemoveRange(old);
        if (dto.Details != null)
            foreach (var d in dto.Details)
                _db.TechnicalExamReportDetails.Add(new TechnicalExamReportDetail
                {
                    TechnicalExamReportId = id,
                    VehiclePartId = d.VehiclePartId, StatusId = d.StatusId,
                    Front = d.Front, Back = d.Back, OnLeft = d.OnLeft, OnRight = d.OnRight,
                    EnteredAt = DateTime.UtcNow, Note = d.Note, Active = true,
                });

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var r = await _db.TechnicalExamReports.FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();
        r.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
