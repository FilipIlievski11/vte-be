using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Documents;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/traffic-licences")]
[Authorize]
public class TrafficLicencesController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public TrafficLicencesController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record TrafficLicenceDto(long Id, string? TrafficLicenceNumber, DateOnly MadeDate, DateOnly EndDate, bool IsActive);
    public record CreateTrafficLicenceRequest(long CustomerVehicleRelationId, int? IssuingOrganizationId, string? TrafficLicenceNumber, DateOnly MadeDate, DateOnly EndDate, string? Note);

    [HttpGet]
    public async Task<ActionResult<PagedResult<TrafficLicenceDto>>> List(
        [FromQuery] bool? expiringOnly, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var q = _db.TrafficLicences.AsNoTracking().AsQueryable();
        if (expiringOnly == true)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var soon = today.AddDays(30);
            q = q.Where(t => t.IsActive && t.EndDate >= today && t.EndDate <= soon);
        }
        var ordered = q.OrderByDescending(t => t.MadeDate)
            .Select(t => new TrafficLicenceDto(t.Id, t.TrafficLicenceNumber, t.MadeDate, t.EndDate, t.IsActive));
        return Ok(await ordered.ToPagedAsync(page, pageSize));
    }

    [HttpPost]
    public async Task<ActionResult<TrafficLicenceDto>> Create(CreateTrafficLicenceRequest req)
    {
        if (req.EndDate < req.MadeDate)
            return BadRequest(new { error = "EndDate must be on or after MadeDate." });
        if (_tenant.StationId is null) return BadRequest(new { error = "StationId could not be resolved." });

        var t = new TrafficLicence
        {
            StationId = _tenant.StationId.Value,
            CustomerVehicleRelationId = req.CustomerVehicleRelationId,
            IssuingOrganizationId = req.IssuingOrganizationId,
            TrafficLicenceNumber = req.TrafficLicenceNumber,
            MadeDate = req.MadeDate,
            EndDate = req.EndDate,
            Note = req.Note
        };
        _db.TrafficLicences.Add(t);
        await _db.SaveChangesAsync();
        return Ok(new TrafficLicenceDto(t.Id, t.TrafficLicenceNumber, t.MadeDate, t.EndDate, t.IsActive));
    }
}
