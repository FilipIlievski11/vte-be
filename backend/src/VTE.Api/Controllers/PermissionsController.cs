using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Documents;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/permissions")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    public PermissionsController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record PermissionDto(long Id, string? PermissionNumber, DateOnly MadeDate, DateOnly? EndDate, bool IsActive);
    public record CreatePermissionRequest(long CustomerVehicleRelationId, string? PermissionNumber, DateOnly MadeDate, DateOnly? EndDate, string? Note);

    [HttpGet]
    public async Task<ActionResult<PagedResult<PermissionDto>>> List([FromQuery] int? page, [FromQuery] int? pageSize)
        => Ok(await _db.Permissions.AsNoTracking().OrderByDescending(p => p.MadeDate)
            .Select(p => new PermissionDto(p.Id, p.PermissionNumber, p.MadeDate, p.EndDate, p.IsActive))
            .ToPagedAsync(page, pageSize));

    [HttpPost]
    public async Task<ActionResult<PermissionDto>> Create(CreatePermissionRequest req)
    {
        if (req.EndDate is not null && req.EndDate < req.MadeDate)
            return BadRequest(new { error = "EndDate must be on or after MadeDate." });
        var stationId = _tenant.StationId
            ?? await _db.Stations.Where(s => s.IsActive).OrderBy(s => s.Id).Select(s => (int?)s.Id).FirstOrDefaultAsync();
        if (stationId is null) return BadRequest(new { error = "No Station configured." });

        var p = new Permission
        {
            StationId = stationId.Value,
            CustomerVehicleRelationId = req.CustomerVehicleRelationId,
            PermissionNumber = req.PermissionNumber,
            MadeDate = req.MadeDate,
            EndDate = req.EndDate,
            Note = req.Note,
        };
        _db.Permissions.Add(p);
        await _db.SaveChangesAsync();
        return Ok(new PermissionDto(p.Id, p.PermissionNumber, p.MadeDate, p.EndDate, p.IsActive));
    }
}

[ApiController]
[Route("api/intl-driving-licences")]
[Authorize]
public class IntlDrivingLicencesController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    public IntlDrivingLicencesController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record IDLDto(long Id, string LicenceNumber, DateOnly IssuedDate, DateOnly ValidTill, long CustomerId, bool IsActive);
    public record CreateIDLRequest(long CustomerId, string LicenceNumber, DateOnly IssuedDate, DateOnly ValidTill, int? IssuerId, string? Note);

    [HttpGet]
    public async Task<ActionResult<PagedResult<IDLDto>>> List([FromQuery] int? page, [FromQuery] int? pageSize)
        => Ok(await _db.InternationalDrivingLicences.AsNoTracking().OrderByDescending(l => l.IssuedDate)
            .Select(l => new IDLDto(l.Id, l.LicenceNumber, l.IssuedDate, l.ValidTill, l.CustomerId, l.IsActive))
            .ToPagedAsync(page, pageSize));

    [HttpPost]
    public async Task<ActionResult<IDLDto>> Create(CreateIDLRequest req)
    {
        if (req.ValidTill < req.IssuedDate)
            return BadRequest(new { error = "ValidTill must be on or after IssuedDate." });
        var stationId = _tenant.StationId
            ?? await _db.Stations.Where(s => s.IsActive).OrderBy(s => s.Id).Select(s => (int?)s.Id).FirstOrDefaultAsync();
        if (stationId is null) return BadRequest(new { error = "No Station configured." });

        var l = new InternationalDrivingLicence
        {
            StationId = stationId.Value,
            CustomerId = req.CustomerId,
            LicenceNumber = req.LicenceNumber,
            IssuedDate = req.IssuedDate,
            ValidTill = req.ValidTill,
            IssuerId = req.IssuerId,
            Note = req.Note,
        };
        _db.InternationalDrivingLicences.Add(l);
        await _db.SaveChangesAsync();
        return Ok(new IDLDto(l.Id, l.LicenceNumber, l.IssuedDate, l.ValidTill, l.CustomerId, l.IsActive));
    }
}
