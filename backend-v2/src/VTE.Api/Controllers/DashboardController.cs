using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// Дневни бројки за KPI лентата на Контролната табла. Tenant-scoped автоматски
/// (глобалниот query filter); „денес" го праќа FE-то како локален датум на операторот
/// (from = локална полноќ), бидејќи серверот работи во UTC.
/// </summary>
[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly VteDbContext _db;
    public DashboardController(VteDbContext db) => _db = db;

    public record DashboardStats(int RequestsToday, int ExamsPassedToday);

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStats>> Stats([FromQuery] DateTime from, CancellationToken ct = default)
    {
        var fromDate = from.Date;
        var toDate = fromDate.AddDays(1);
        var day = DateOnly.FromDateTime(fromDate);

        var requestsToday = await _db.Requests.AsNoTracking()
            .CountAsync(r => r.Active && r.CreatedAt >= fromDate && r.CreatedAt < toDate, ct);
        var examsPassedToday = await _db.TechnicalExamReports.AsNoTracking()
            .CountAsync(x => x.Active && x.VehicleIsRight && x.MadeDate == day, ct);

        return Ok(new DashboardStats(requestsToday, examsPassedToday));
    }
}
