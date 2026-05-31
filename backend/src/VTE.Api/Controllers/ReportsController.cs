using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Pdf;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly VteDbContext _db;
    public ReportsController(VteDbContext db) => _db = db;

    [HttpGet("technical-exam-reports/{id:long}/pdf")]
    public async Task<IActionResult> ExamReportPdf(long id)
    {
        var r = await _db.TechnicalExamReports.FindAsync(id);
        if (r is null) return NotFound();

        // Resolve station + customer name for the header
        var station = await _db.Stations.FirstOrDefaultAsync(s => s.Id == r.StationId);
        var customer = await (from rel in _db.CustomerVehicleRelations
                              join c in _db.Customers on rel.CustomerId equals c.Id
                              where rel.Id == r.CustomerVehicleRelationId
                              select new { c.FirstName, c.Surname }).FirstOrDefaultAsync();

        var name = customer is null ? null : $"{customer.FirstName} {customer.Surname}".Trim();
        var pdf = PdfReports.ExamReportCertificate(r, station?.Name ?? "VTE", name);
        return File(pdf, "application/pdf", $"exam-report-{id}.pdf");
    }

    [HttpGet("payment-documents/{id:long}/invoice")]
    public async Task<IActionResult> InvoicePdf(long id)
    {
        var d = await _db.PaymentDocuments.Include(x => x.Details).Include(x => x.Installments)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (d is null) return NotFound();

        var station = await _db.Stations.FirstOrDefaultAsync(s => s.Id == d.StationId);
        var customer = await (from rel in _db.CustomerVehicleRelations
                              join c in _db.Customers on rel.CustomerId equals c.Id
                              where rel.Id == d.CustomerVehicleRelationId
                              select new { c.FirstName, c.Surname }).FirstOrDefaultAsync();
        var name = customer is null ? null : $"{customer.FirstName} {customer.Surname}".Trim();

        var pdf = PdfReports.Invoice(d, station?.Name ?? "VTE", name);
        return File(pdf, "application/pdf", $"invoice-{d.DocumentNumber}.pdf");
    }

    [HttpGet("traffic-licences/{id:long}/pdf")]
    public async Task<IActionResult> TrafficLicencePdf(long id)
    {
        var l = await _db.TrafficLicences.FirstOrDefaultAsync(x => x.Id == id);
        if (l is null) return NotFound();

        var station = await _db.Stations.FirstOrDefaultAsync(s => s.Id == l.StationId);
        var ctx = await (from rel in _db.CustomerVehicleRelations
                         join c in _db.Customers on rel.CustomerId equals c.Id
                         join v in _db.Vehicles on rel.VehicleId equals v.Id
                         where rel.Id == l.CustomerVehicleRelationId
                         select new { c.FirstName, c.Surname, v.LastRegistrationNumber, v.ShellNumber }).FirstOrDefaultAsync();
        var name = ctx is null ? null : $"{ctx.FirstName} {ctx.Surname}".Trim();
        var org = l.IssuingOrganizationId is null ? null
            : (await _db.TechnicalExamOrganizations.FirstOrDefaultAsync(o => o.Id == l.IssuingOrganizationId))?.Name;

        var pdf = PdfReports.TrafficLicenceCertificate(l, station?.Name ?? "VTE", name, ctx?.LastRegistrationNumber, ctx?.ShellNumber, org);
        return File(pdf, "application/pdf", $"traffic-licence-{l.TrafficLicenceNumber ?? id.ToString()}.pdf");
    }

    [HttpGet("permissions/{id:long}/pdf")]
    public async Task<IActionResult> PermissionPdf(long id)
    {
        var per = await _db.Permissions.FirstOrDefaultAsync(x => x.Id == id);
        if (per is null) return NotFound();

        var station = await _db.Stations.FirstOrDefaultAsync(s => s.Id == per.StationId);
        var ctx = await (from rel in _db.CustomerVehicleRelations
                         join c in _db.Customers on rel.CustomerId equals c.Id
                         join v in _db.Vehicles on rel.VehicleId equals v.Id
                         where rel.Id == per.CustomerVehicleRelationId
                         select new { c.FirstName, c.Surname, v.LastRegistrationNumber }).FirstOrDefaultAsync();
        var name = ctx is null ? null : $"{ctx.FirstName} {ctx.Surname}".Trim();

        var pdf = PdfReports.PermissionCertificate(per, station?.Name ?? "VTE", name, ctx?.LastRegistrationNumber, null);
        return File(pdf, "application/pdf", $"permission-{per.PermissionNumber ?? id.ToString()}.pdf");
    }

    [HttpGet("intl-driving-licences/{id:long}/pdf")]
    public async Task<IActionResult> IdlPdf(long id)
    {
        var idl = await _db.InternationalDrivingLicences.FirstOrDefaultAsync(x => x.Id == id);
        if (idl is null) return NotFound();

        var station = await _db.Stations.FirstOrDefaultAsync(s => s.Id == idl.StationId);
        var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == idl.CustomerId);
        var name = customer is null ? null : $"{customer.FirstName} {customer.Surname}".Trim();
        var issuer = idl.IssuerId is null ? null
            : (await _db.RegistrationIssuers.FirstOrDefaultAsync(r => r.Id == idl.IssuerId))?.Name;

        // IDL category junction has no EF entity yet — query raw, fail-safe to empty
        var cats = await _db.Database.SqlQueryRaw<string>(@"
            SELECT d.Name
            FROM dbo.InternationalDrivingLicenceCategories j
            JOIN dbo.DrivingLicenceCategories d ON d.Id = j.DrivingLicenceCategoryId
            WHERE j.InternationalDrivingLicenceId = {0}
            ORDER BY d.Name", idl.Id).ToListAsync();

        var pdf = PdfReports.InternationalDrivingLicenceCertificate(idl, station?.Name ?? "VTE", name, issuer, cats);
        return File(pdf, "application/pdf", $"idl-{idl.LicenceNumber}.pdf");
    }

    [HttpGet("cash-report")]
    public async Task<IActionResult> CashReportPdf([FromQuery] DateOnly? day)
    {
        var d = day ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var docs = await _db.PaymentDocuments.Include(x => x.Details).Include(x => x.Installments)
            .Where(x => x.DatePay == d).ToListAsync();

        var anyStationId = docs.FirstOrDefault()?.StationId;
        var station = anyStationId is null ? null
            : await _db.Stations.FirstOrDefaultAsync(s => s.Id == anyStationId);

        var pdf = PdfReports.CashReport(d, station?.Name ?? "VTE", docs);
        return File(pdf, "application/pdf", $"cash-report-{d:yyyy-MM-dd}.pdf");
    }
}
