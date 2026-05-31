using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// The 7 Прегледи / pivot reports from the legacy menu.
// Each is a server-side aggregation that returns small JSON the frontend can render.
[ApiController]
[Route("api/reports")]
[Authorize]
public class PivotReportsController : ControllerBase
{
    private readonly VteDbContext _db;
    public PivotReportsController(VteDbContext db) => _db = db;

    public record GroupRow(string Group, int Count);
    public record GroupSumRow(string Group, int Count, decimal Sum);

    // 1. Преглед на комитенти — customers grouped by city + by type
    [HttpGet("pivot/customers")]
    public async Task<ActionResult<object>> Customers()
    {
        var byCity = await (from c in _db.Customers
                            join city in _db.Cities on c.LivingCityId equals city.Id into cj
                            from city in cj.DefaultIfEmpty()
                            group c by (city != null ? city.Name : "—") into g
                            orderby g.Count() descending
                            select new GroupRow(g.Key, g.Count())).Take(50).ToListAsync();

        var byType = await _db.Customers
            .GroupBy(c => c.IsCompany ? "Правни лица" : "Физички лица")
            .Select(g => new GroupRow(g.Key, g.Count())).ToListAsync();

        return new {
            total = await _db.Customers.CountAsync(),
            active = await _db.Customers.CountAsync(c => c.IsActive),
            byCity, byType,
        };
    }

    // 2. Преглед на возила — vehicles by category + by body type + by maker
    [HttpGet("pivot/vehicles")]
    public async Task<ActionResult<object>> Vehicles()
    {
        var byCategory = await (from v in _db.Vehicles
                                join c in _db.VehicleCategories on v.VehicleCategoryId equals c.Id into cj
                                from c in cj.DefaultIfEmpty()
                                group v by (c != null ? c.Name : "—") into g
                                orderby g.Count() descending
                                select new GroupRow(g.Key, g.Count())).ToListAsync();

        var byBodyType = await (from v in _db.Vehicles
                                join b in _db.VehicleBodyTypes on v.BodyTypeId equals b.Id into bj
                                from b in bj.DefaultIfEmpty()
                                group v by (b != null ? b.Name : "—") into g
                                orderby g.Count() descending
                                select new GroupRow(g.Key, g.Count())).Take(20).ToListAsync();

        return new {
            total = await _db.Vehicles.CountAsync(),
            active = await _db.Vehicles.CountAsync(v => v.IsActive),
            byCategory, byBodyType,
        };
    }

    // 3. Преглед на возила и комитенти — joined view of customer-vehicle pairs
    public record CustomerVehicleRow(string Customer, string? VIN, string? RegNumber, string? RelationType);
    [HttpGet("pivot/customer-vehicle")]
    public async Task<ActionResult<List<CustomerVehicleRow>>> CustomerVehicle([FromQuery] int take = 200)
    {
        return await (from r in _db.CustomerVehicleRelations
                      join c in _db.Customers on r.CustomerId equals c.Id
                      join v in _db.Vehicles  on r.VehicleId  equals v.Id
                      join rt in _db.CustomerVehicleRelationTypes on r.RelationTypeId equals rt.Id into rj
                      from rt in rj.DefaultIfEmpty()
                      where r.IsActive
                      orderby c.Surname, c.FirstName
                      select new CustomerVehicleRow(
                          (c.Surname ?? "") + " " + c.FirstName,
                          v.ShellNumber, v.LastRegistrationNumber,
                          rt != null ? rt.Name : "—"))
                      .Take(take).ToListAsync();
    }

    // 4. Преглед на барања — by type and by status
    [HttpGet("pivot/requests")]
    public async Task<ActionResult<object>> Requests()
    {
        var byType = await (from r in _db.Requests
                            join t in _db.RequestTypes on r.RequestTypeId equals t.Id
                            group r by t.TypeName into g
                            orderby g.Count() descending
                            select new GroupRow(g.Key, g.Count())).ToListAsync();
        var byStatus = await _db.Requests
            .GroupBy(r => r.DateEnded == null ? "Отворени" : "Завршени")
            .Select(g => new GroupRow(g.Key, g.Count())).ToListAsync();

        return new {
            total = await _db.Requests.CountAsync(),
            open  = await _db.Requests.CountAsync(r => r.DateEnded == null),
            byType, byStatus,
        };
    }

    // 5. Преглед на бр. тех. прегледи — count of tech-exams by month (last 12) and by type
    [HttpGet("pivot/exams-count")]
    public async Task<ActionResult<object>> ExamsCount()
    {
        var monthAgo12 = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12));
        var byMonthRows = await _db.TechnicalExamReports
            .Where(r => r.MadeDate >= monthAgo12)
            .Select(r => new { Year = r.MadeDate.Year, Month = r.MadeDate.Month })
            .ToListAsync();
        var byMonth = byMonthRows
            .GroupBy(x => x.Year * 100 + x.Month)
            .OrderBy(g => g.Key)
            .Select(g => new GroupRow($"{g.Key / 100}-{g.Key % 100:D2}", g.Count())).ToList();

        var byType = await (from r in _db.TechnicalExamReports
                            join t in _db.TechnicalExamTypes on r.TechnicalExamTypeId equals t.Id into tj
                            from t in tj.DefaultIfEmpty()
                            group r by (t != null ? t.Name : "—") into g
                            orderby g.Count() descending
                            select new GroupRow(g.Key, g.Count())).ToListAsync();

        return new {
            total       = await _db.TechnicalExamReports.CountAsync(),
            passedTotal = await _db.TechnicalExamReports.CountAsync(r => r.VehicleIsRight),
            failedTotal = await _db.TechnicalExamReports.CountAsync(r => !r.VehicleIsRight),
            byMonth, byType,
        };
    }

    // 6. Преглед на тех. прегледи со неисправни возила — failed exams
    public record FailedExamRow(long Id, DateOnly MadeDate, string? RegNumber, string? Note);
    [HttpGet("pivot/exams-failed")]
    public async Task<ActionResult<List<FailedExamRow>>> ExamsFailed()
    {
        return await _db.TechnicalExamReports
            .Where(r => !r.VehicleIsRight)
            .OrderByDescending(r => r.MadeDate)
            .Take(500)
            .Select(r => new FailedExamRow(r.Id, r.MadeDate, r.RegNumber, r.Note))
            .ToListAsync();
    }

    // 7. Извештај за плаќања — payments by date (last 12 months) and by status
    [HttpGet("pivot/payments")]
    public async Task<ActionResult<object>> Payments()
    {
        var monthAgo12 = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12));
        var docs = await _db.PaymentDocuments.Include(d => d.Details)
            .Where(d => !d.Storno && d.DatePay >= monthAgo12)
            .Select(d => new {
                d.DatePay,
                d.Payed, d.Storno,
                Total = d.Details.Sum(x => x.Price * (1 - x.DiscountPercent / 100m)) * (1 - d.DiscountPercent / 100m)
            }).ToListAsync();

        var byMonth = docs
            .GroupBy(x => x.DatePay.Year * 100 + x.DatePay.Month)
            .OrderBy(g => g.Key)
            .Select(g => new GroupSumRow($"{g.Key / 100}-{g.Key % 100:D2}", g.Count(), g.Sum(x => x.Total))).ToList();

        var byStatus = docs
            .GroupBy(x => x.Payed ? "Платени" : "Неплатени")
            .Select(g => new GroupSumRow(g.Key, g.Count(), g.Sum(x => x.Total))).ToList();

        return new {
            total      = docs.Count,
            sumTotal   = docs.Sum(x => x.Total),
            byMonth, byStatus,
        };
    }
}
