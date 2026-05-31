using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Documents;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/technical-exam-reports")]
[Authorize]
public class TechnicalExamReportsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public TechnicalExamReportsController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record ExamReportDto(long Id, string? RegNumber, DateOnly MadeDate, DateOnly ValidTillDate, bool VehicleIsRight,
        string? OwnerName, string? VehicleShellNumber, string? VehicleMaker, string? VehicleModel, string? ExamTypeName);
    public record FaultDto(int TechnicalExamVehiclePartId, int StatusId, bool Front, bool Back, bool OnLeft, bool OnRight, DateOnly DateEnter, string? Note);
    public record VisualErrorDto(string Description, string? Severity);

    public record CreateExamRequest(
        long CustomerVehicleRelationId,
        int TechnicalExamTypeId,
        int OrganizationForTechnicalExamId,
        string FirstInspectorOperatorUserId,
        string? SecondInspectorOperatorUserId,
        string? RegNumber,
        DateOnly MadeDate,
        DateOnly ValidTillDate,
        bool VehicleIsRight,
        string? ExplanationNote,
        string? DriversWarning,
        string? Note,
        List<FaultDto>? Faults,
        List<VisualErrorDto>? VisualErrors);

    [HttpGet]
    public async Task<ActionResult<PagedResult<ExamReportDto>>> List(
        [FromQuery] DateOnly? from, [FromQuery] DateOnly? to,
        [FromQuery] bool? vehicleIsRight,
        [FromQuery(Name = "q")] string? search,
        [FromQuery] int? page, [FromQuery] int? pageSize,
        [FromQuery] string? sortBy, [FromQuery] string? sortDir)
    {
        var rows =
            from r in _db.TechnicalExamReports.AsNoTracking()
            join cvr in _db.CustomerVehicleRelations on r.CustomerVehicleRelationId equals cvr.Id
            join cust in _db.Customers on cvr.CustomerId equals cust.Id
            join veh in _db.Vehicles on cvr.VehicleId equals veh.Id
            join et  in _db.TechnicalExamTypes on r.TechnicalExamTypeId equals et.Id into etj from et in etj.DefaultIfEmpty()
            join vm  in _db.VehicleModels on veh.VehicleModelId equals vm.Id into vmj from vm in vmj.DefaultIfEmpty()
            join mk  in _db.VehicleMakers on vm.VehicleMakerId equals mk.Id into mkj from mk in mkj.DefaultIfEmpty()
            select new
            {
                r.Id, r.RegNumber, r.MadeDate, r.ValidTillDate, r.VehicleIsRight,
                CustomerFirst = cust.FirstName, CustomerSurname = cust.Surname,
                veh.ShellNumber,
                MakerName = mk == null ? null : mk.Name,
                ModelName = vm == null ? null : vm.Name,
                ExamTypeName = et == null ? null : et.Name,
            };
        if (from is not null) rows = rows.Where(x => x.MadeDate >= from);
        if (to   is not null) rows = rows.Where(x => x.MadeDate <= to);
        if (vehicleIsRight is not null) rows = rows.Where(x => x.VehicleIsRight == vehicleIsRight);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var qt = $"%{search}%";
            rows = rows.Where(x =>
                EF.Functions.Like(x.RegNumber  ?? "", qt) ||
                EF.Functions.Like(x.ShellNumber ?? "", qt) ||
                EF.Functions.Like(x.CustomerFirst   ?? "", qt) ||
                EF.Functions.Like(x.CustomerSurname ?? "", qt));
        }

        var desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
        rows = (sortBy?.ToLowerInvariant()) switch {
            "regnumber"          => desc ? rows.OrderByDescending(x => x.RegNumber)        : rows.OrderBy(x => x.RegNumber),
            "madedate"           => desc ? rows.OrderByDescending(x => x.MadeDate)         : rows.OrderBy(x => x.MadeDate),
            "validtilldate"      => desc ? rows.OrderByDescending(x => x.ValidTillDate)    : rows.OrderBy(x => x.ValidTillDate),
            "ownername"          => desc ? rows.OrderByDescending(x => x.CustomerSurname).ThenByDescending(x => x.CustomerFirst)
                                         : rows.OrderBy(x => x.CustomerSurname).ThenBy(x => x.CustomerFirst),
            "vehicleshellnumber" => desc ? rows.OrderByDescending(x => x.ShellNumber)      : rows.OrderBy(x => x.ShellNumber),
            "vehiclemaker"       => desc ? rows.OrderByDescending(x => x.MakerName)        : rows.OrderBy(x => x.MakerName),
            "examtypename"       => desc ? rows.OrderByDescending(x => x.ExamTypeName)     : rows.OrderBy(x => x.ExamTypeName),
            _                    => rows.OrderByDescending(x => x.MadeDate),
        };

        var dtoQuery = rows.Select(x => new ExamReportDto(
            x.Id, x.RegNumber, x.MadeDate, x.ValidTillDate, x.VehicleIsRight,
            ((x.CustomerSurname ?? "") + " " + x.CustomerFirst).Trim(),
            x.ShellNumber, x.MakerName, x.ModelName, x.ExamTypeName));

        return Ok(await dtoQuery.ToPagedAsync(page, pageSize));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TechnicalExamReport>> Get(long id)
    {
        var r = await _db.TechnicalExamReports.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return r is null ? NotFound() : Ok(r);
    }

    public record DetailRow(long Id, int TechnicalExamVehiclePartId, int StatusId, bool Front, bool Back, bool OnLeft, bool OnRight, DateOnly DateEnter, string? Note);
    public record VisualErrorRow(long Id, string Description, string? Severity);
    public record CustomerOpt(long Id, string FirstName, string? Surname, string? EMBG, string? PhoneNumber);
    public record VehicleOpt(long Id, string ShellNumber, string LastRegistrationNumber);
    public record ExamReportSlim(
        long Id, long CustomerVehicleRelationId, int TechnicalExamTypeId, int OrganizationForTechnicalExamId,
        string? FirstInspectorOperatorUserId, string? SecondInspectorOperatorUserId,
        string? RegNumber, DateOnly MadeDate, DateOnly ValidTillDate, bool VehicleIsRight,
        string? ExplanationNote, string? DriversWarning, string? Note);
    public record ExamFullDto(ExamReportSlim Report, CustomerOpt? Customer, VehicleOpt? Vehicle,
        List<DetailRow> Details, List<VisualErrorRow> VisualErrors);

    [HttpGet("{id:long}/full")]
    public async Task<ActionResult<ExamFullDto>> GetFull(long id)
    {
        var r = await _db.TechnicalExamReports.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ExamReportSlim(
                x.Id, x.CustomerVehicleRelationId, x.TechnicalExamTypeId, x.OrganizationForTechnicalExamId,
                x.FirstInspectorOperatorUserId, x.SecondInspectorOperatorUserId,
                x.RegNumber, x.MadeDate, x.ValidTillDate, x.VehicleIsRight,
                x.ExplanationNote, x.DriversWarning, x.Note))
            .FirstOrDefaultAsync();
        if (r is null) return NotFound();

        var cvr = await _db.CustomerVehicleRelations.AsNoTracking()
            .Where(x => x.Id == r.CustomerVehicleRelationId)
            .Select(x => new { x.CustomerId, x.VehicleId })
            .FirstOrDefaultAsync();
        CustomerOpt? customer = null;
        VehicleOpt? vehicle = null;
        if (cvr is not null)
        {
            customer = await _db.Customers.AsNoTracking().Where(c => c.Id == cvr.CustomerId)
                .Select(c => new CustomerOpt(c.Id, c.FirstName, c.Surname, c.EMBG, c.PhoneNumber)).FirstOrDefaultAsync();
            if (cvr.VehicleId is long vehicleId)
            {
                vehicle = await _db.Vehicles.AsNoTracking().Where(v => v.Id == vehicleId)
                    .Select(v => new VehicleOpt(v.Id, v.ShellNumber, v.LastRegistrationNumber)).FirstOrDefaultAsync();
            }
        }

        var details = await _db.Set<TechnicalExamReportDetail>().AsNoTracking()
            .Where(d => d.TechnicalExamReportId == id)
            .Select(d => new DetailRow(d.Id, d.TechnicalExamVehiclePartId, d.StatusId, d.Front, d.Back, d.OnLeft, d.OnRight, d.DateEnter, d.Note))
            .ToListAsync();
        var visual = await _db.Set<TechnicalExamReportVisualError>().AsNoTracking()
            .Where(v => v.TechnicalExamReportId == id)
            .Select(v => new VisualErrorRow(v.Id, v.Description, v.Severity)).ToListAsync();

        return Ok(new ExamFullDto(r, customer, vehicle, details, visual));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var r = await _db.TechnicalExamReports.FirstOrDefaultAsync(x => x.Id == id);
        if (r is null) return NotFound();
        // Cascade delete the report's details + visual errors first
        await _db.Set<TechnicalExamReportDetail>().Where(d => d.TechnicalExamReportId == id).ExecuteDeleteAsync();
        await _db.Set<TechnicalExamReportVisualError>().Where(v => v.TechnicalExamReportId == id).ExecuteDeleteAsync();
        _db.TechnicalExamReports.Remove(r);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<ExamReportDto>> Create(CreateExamRequest req)
    {
        if (req.ValidTillDate < req.MadeDate)
            return BadRequest(new { error = "BR-DOC-403: ValidTillDate must be on or after MadeDate." });
        if (req.SecondInspectorOperatorUserId is not null && req.SecondInspectorOperatorUserId == req.FirstInspectorOperatorUserId)
            return BadRequest(new { error = "BR-DOC-404: first and second inspector must be different." });

        var stationId = _tenant.StationId
            ?? await _db.Stations.Where(s => s.IsActive).OrderBy(s => s.Id).Select(s => (int?)s.Id).FirstOrDefaultAsync();
        if (stationId is null) return BadRequest(new { error = "No Station configured." });

        var r = new TechnicalExamReport
        {
            StationId = stationId.Value,
            CustomerVehicleRelationId = req.CustomerVehicleRelationId,
            TechnicalExamTypeId = req.TechnicalExamTypeId,
            OrganizationForTechnicalExamId = req.OrganizationForTechnicalExamId,
            FirstInspectorOperatorUserId = req.FirstInspectorOperatorUserId,
            SecondInspectorOperatorUserId = req.SecondInspectorOperatorUserId,
            RegNumber = req.RegNumber,
            MadeDate = req.MadeDate,
            ValidTillDate = req.ValidTillDate,
            VehicleIsRight = req.VehicleIsRight,
            ExplanationNote = req.ExplanationNote,
            DriversWarning = req.DriversWarning,
            Note = req.Note,
        };
        _db.TechnicalExamReports.Add(r);
        await _db.SaveChangesAsync();

        // Save fault details (the parts checklist)
        if (req.Faults is { Count: > 0 })
        {
            foreach (var f in req.Faults)
            {
                _db.Set<TechnicalExamReportDetail>().Add(new TechnicalExamReportDetail
                {
                    TechnicalExamReportId = r.Id,
                    TechnicalExamVehiclePartId = f.TechnicalExamVehiclePartId,
                    StatusId = f.StatusId,
                    Front = f.Front, Back = f.Back, OnLeft = f.OnLeft, OnRight = f.OnRight,
                    DateEnter = f.DateEnter,
                    Note = f.Note,
                });
            }
            await _db.SaveChangesAsync();
        }

        // Save visual errors
        if (req.VisualErrors is { Count: > 0 })
        {
            foreach (var v in req.VisualErrors)
            {
                _db.Set<TechnicalExamReportVisualError>().Add(new TechnicalExamReportVisualError
                {
                    TechnicalExamReportId = r.Id,
                    Description = v.Description,
                    Severity = v.Severity,
                });
            }
            await _db.SaveChangesAsync();
        }

        return CreatedAtAction(nameof(Get), new { id = r.Id },
            new ExamReportDto(r.Id, r.RegNumber, r.MadeDate, r.ValidTillDate, r.VehicleIsRight, null, null, null, null, null));
    }
}
