using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// Specialised list endpoints for Invoices / Unpaid / Customer financial state.
// All read-only views over PaymentDocuments + CustomerFinancialState rows the
// tenant filter already restricts.
[ApiController]
[Route("api")]
[Authorize]
public class PaymentReportsController : ControllerBase
{
    private readonly VteDbContext _db;
    public PaymentReportsController(VteDbContext db) => _db = db;

    public record SummaryDocDto(long Id, string DocumentNumber, DateOnly DatePay, decimal TotalAmount, decimal TotalInstallments, bool Payed, bool Storno, long CustomerVehicleRelationId);

    [HttpGet("invoices")]
    public async Task<ActionResult<PagedResult<SummaryDocDto>>> Invoices([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var invoiceTypeIds = await _db.PaymentTypes.Where(pt => pt.IsInvoice && pt.IsActive).Select(pt => pt.Id).ToListAsync();
        var ordered =
            from d in _db.PaymentDocuments.AsNoTracking().Where(d => invoiceTypeIds.Contains(d.PaymentTypeId))
            orderby d.DatePay descending
            select new SummaryDocDto(
                d.Id, d.DocumentNumber, d.DatePay,
                _db.PaymentDocumentDetails.Where(x => x.PaymentDocumentId == d.Id)
                    .Sum(x => (decimal?)(x.Price * (1 - x.DiscountPercent / 100m))) * (1 - d.DiscountPercent / 100m) ?? 0m,
                _db.PaymentDocumentInstallments.Where(x => x.PaymentDocumentId == d.Id).Sum(x => (decimal?)x.Price) ?? 0m,
                d.Payed, d.Storno, d.CustomerVehicleRelationId);
        return Ok(await ordered.ToPagedAsync(page, pageSize));
    }

    [HttpGet("unpaid-deals")]
    public async Task<ActionResult<PagedResult<SummaryDocDto>>> Unpaid([FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var ordered =
            from d in _db.PaymentDocuments.AsNoTracking().Where(d => !d.Payed && !d.Storno)
            orderby d.DateRequired
            select new SummaryDocDto(
                d.Id, d.DocumentNumber, d.DatePay,
                _db.PaymentDocumentDetails.Where(x => x.PaymentDocumentId == d.Id)
                    .Sum(x => (decimal?)(x.Price * (1 - x.DiscountPercent / 100m))) * (1 - d.DiscountPercent / 100m) ?? 0m,
                _db.PaymentDocumentInstallments.Where(x => x.PaymentDocumentId == d.Id).Sum(x => (decimal?)x.Price) ?? 0m,
                d.Payed, d.Storno, d.CustomerVehicleRelationId);
        return Ok(await ordered.ToPagedAsync(page, pageSize));
    }

    public record CustomerFinancialStateRowDto(long Id, long CustomerId, DateOnly Date, string? Description, decimal DebitAmount, decimal CreditAmount, decimal RunningBalance);

    [HttpGet("customer-financial-state")]
    public async Task<ActionResult<PagedResult<CustomerFinancialStateRowDto>>> Cfs(
        [FromQuery] long? customerId, [FromQuery] int? page, [FromQuery] int? pageSize)
    {
        var q = _db.Set<VTE.Domain.Payments.CustomerFinancialState>().AsNoTracking().AsQueryable();
        if (customerId is not null) q = q.Where(x => x.CustomerId == customerId);
        var ordered = q.OrderByDescending(x => x.Date)
            .Select(x => new CustomerFinancialStateRowDto(x.Id, x.CustomerId, x.Date, x.Description, x.DebitAmount, x.CreditAmount, x.RunningBalance));
        return Ok(await ordered.ToPagedAsync(page, pageSize));
    }
}
