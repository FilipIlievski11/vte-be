using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly VteDbContext _db;
    public DashboardController(VteDbContext db) => _db = db;

    public record DashboardStats(
        int Customers,
        int Vehicles,
        int OpenRequests,
        int UnpaidPaymentDocuments,
        int TrafficLicencesExpiringIn30Days,
        int TechExamReportsLast30Days,
        int FailedExamsLast30Days);

    [HttpGet]
    public async Task<ActionResult<DashboardStats>> Get()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var soon  = today.AddDays(30);
        var monthAgo = today.AddDays(-30);

        return new DashboardStats(
            Customers:                       await _db.Customers.CountAsync(),
            Vehicles:                        await _db.Vehicles.CountAsync(),
            OpenRequests:                    await _db.Requests.CountAsync(r => r.DateEnded == null),
            UnpaidPaymentDocuments:          await _db.PaymentDocuments.CountAsync(d => !d.Payed && !d.Storno),
            TrafficLicencesExpiringIn30Days: await _db.TrafficLicences.CountAsync(t => t.IsActive && t.EndDate >= today && t.EndDate <= soon),
            TechExamReportsLast30Days:       await _db.TechnicalExamReports.CountAsync(r => r.MadeDate >= monthAgo),
            FailedExamsLast30Days:           await _db.TechnicalExamReports.CountAsync(r => r.MadeDate >= monthAgo && !r.VehicleIsRight)
        );
    }
}
