using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

/// <summary>
/// Read API over the migrated payment-document history (Phase 1: read-only).
///
/// Mirrors the legacy uxPaymentDocument list/detail screens: a paged searchable
/// register of bills + a full single-bill view with line items, installments and
/// optional installment agreement. Tenant-scoped via the EF query filter on
/// <see cref="VTE.Domain.Payments.PaymentDocument"/>.
///
/// Phase 2 will layer create/update onto this; Phase 5 will plug in fiscal printing.
/// </summary>
[ApiController]
[Route("api/payment-documents")]
[Authorize]
public class PaymentDocumentsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public PaymentDocumentsController(VteDbContext db, ITenantContext tenant)
    {
        _db = db;
        _tenant = tenant;
    }

    // ---- DTOs ----

    public record PaymentListItem(
        long Id, byte CompanyId,
        string DocumentNumber, DateTime IssueDate, DateTime DueDate,
        int PaymentTypeId, string? PaymentTypeName,
        long CustomerVehicleRelationId, string? ClientName,
        string? VehiclePlate, string? VehicleVin,
        decimal LinesTotal, double? Discount,
        bool Paid, bool Stornoed, bool Active);

    public record PaymentLineDto(
        long Id, int PriceCatalogId, string? PriceCatalogName,
        decimal UnitPrice, double VatPercent, double Discount, int Quantity,
        string? Note, bool PrePaid, string? PrePaidNote,
        long? CustomerDebtId, bool Active);

    public record InstallmentDto(
        long Id, int SequenceNo, decimal Amount, DateOnly? DueDate,
        bool Paid, DateTime? PaidAt, decimal? PaidAmount, string? Note);

    public record InstallmentAgreementDto(
        long Id, string Number, DateOnly Date, int TotalInstallments,
        string? GuarantorName, string? GuarantorAddress, string? GuarantorEmbg);

    public record PaymentDetailDto(
        long Id, byte CompanyId,
        string DocumentNumber, DateTime IssueDate, DateTime DueDate,
        int PaymentTypeId, string? PaymentTypeName,
        long CustomerVehicleRelationId, string? ClientName, string? ClientMB,
        long? VehicleId, string? VehiclePlate, string? VehicleVin, string? VehicleMakerModel,
        int OrganizationId, int? OperatorLegacyId,
        double? Discount, bool Paid, bool Stornoed, string? StornoReason, string? Note,
        long? AgreementId, InstallmentAgreementDto? Agreement,
        int? InvoicedToCompanyId,
        DateTime? FiscalPrintedAt, long? LegacyId,
        bool Active, DateTime CreatedAt, DateTime? ModifiedAt,
        decimal LinesTotal,
        IReadOnlyList<PaymentLineDto> Lines,
        IReadOnlyList<InstallmentDto> Installments);

    // ---- List (register) ----

    [HttpGet]
    public async Task<ActionResult<PagedDto<PaymentListItem>>> List(
        [FromQuery] string? q = null,
        [FromQuery] string? status = "all",          // "all" | "paid" | "unpaid" | "storno"
        [FromQuery] long? customerVehicleRelationId = null,
        [FromQuery] int? paymentTypeId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] bool includeInactive = false,
        [FromQuery] string? sort = null,             // "issue" | "due" | "doc" | "amount"
        [FromQuery] string? dir = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 50;

        var query = _db.PaymentDocuments.AsNoTracking().AsQueryable();

        if (!includeInactive) query = query.Where(x => x.Active);
        if (customerVehicleRelationId.HasValue) query = query.Where(x => x.CustomerVehicleRelationId == customerVehicleRelationId.Value);
        if (paymentTypeId.HasValue) query = query.Where(x => x.PaymentTypeId == paymentTypeId.Value);
        if (fromDate.HasValue) query = query.Where(x => x.IssueDate >= fromDate.Value);
        if (toDate.HasValue) query = query.Where(x => x.IssueDate <= toDate.Value);

        var st = (status ?? "all").Trim().ToLowerInvariant();
        if (st == "paid")        query = query.Where(x => x.Paid && !x.Stornoed);
        else if (st == "unpaid") query = query.Where(x => !x.Paid && !x.Stornoed);
        else if (st == "storno") query = query.Where(x => x.Stornoed);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q.Trim()}%";

            // Match by document number first (cheap, indexed), then fall back to client/vehicle search.
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

            query = query.Where(x =>
                EF.Functions.Like(x.DocumentNumber, like) ||
                unionRelationIds.Contains(x.CustomerVehicleRelationId));
        }

        var total = await query.CountAsync();

        var asc = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase);
        IQueryable<Domain.Payments.PaymentDocument> Order<TKey>(
            System.Linq.Expressions.Expression<Func<Domain.Payments.PaymentDocument, TKey>> key)
            => asc ? query.OrderBy(key) : query.OrderByDescending(key);
        var ordered = (sort ?? "").Trim().ToLowerInvariant() switch
        {
            "due"  => Order(x => x.DueDate),
            "doc"  => Order(x => x.DocumentNumber),
            _      => Order(x => x.IssueDate),    // default: newest first
        };

        var pageRows = await ordered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new
            {
                x.Id, x.CompanyId, x.DocumentNumber, x.IssueDate, x.DueDate,
                x.PaymentTypeId, x.CustomerVehicleRelationId,
                x.Discount, x.Paid, x.Stornoed, x.Active,
                // Sum lines per doc as a sub-query so we don't N+1
                LinesTotal = _db.PaymentDocumentLines
                    .Where(l => l.PaymentDocumentId == x.Id && l.Active)
                    .Sum(l => (decimal?)(l.UnitPrice * l.Quantity)) ?? 0m,
            })
            .ToListAsync();

        // Batch-resolve joins (payment types + client + vehicle)
        var typeIds = pageRows.Select(r => r.PaymentTypeId).Distinct().ToList();
        var relationIds = pageRows.Select(r => r.CustomerVehicleRelationId).Distinct().ToList();
        var ptDict = await _db.PaymentTypes.AsNoTracking()
            .Where(p => typeIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p.Name);
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
            if (relations.TryGetValue(r.CustomerVehicleRelationId, out var rel))
            {
                cName = clientName(rel.ClientId);
                if (rel.VehicleId.HasValue && vehicles.TryGetValue(rel.VehicleId.Value, out var v))
                {
                    plate = v.Plate;
                    vin = v.Vin;
                }
            }
            ptDict.TryGetValue(r.PaymentTypeId, out var ptName);
            return new PaymentListItem(
                r.Id, r.CompanyId,
                r.DocumentNumber, r.IssueDate, r.DueDate,
                r.PaymentTypeId, ptName,
                r.CustomerVehicleRelationId, cName, plate, vin,
                r.LinesTotal, r.Discount,
                r.Paid, r.Stornoed, r.Active);
        }).ToList();

        return Ok(new PagedDto<PaymentListItem>(page, pageSize, total, items));
    }

    // ---- Detail ----

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PaymentDetailDto>> Get(long id)
    {
        var d = await _db.PaymentDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (d == null) return NotFound();

        var ptName = await _db.PaymentTypes.AsNoTracking().Where(p => p.Id == d.PaymentTypeId).Select(p => p.Name).FirstOrDefaultAsync();

        // Anchor: client + vehicle via the relation.
        string? clientName = null, clientMB = null, plate = null, vin = null, makerModel = null;
        long? vehicleId = null;
        var rel = await _db.ClientVehicleRelations.AsNoTracking()
            .Where(x => x.Id == d.CustomerVehicleRelationId)
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

        // Lines with price-catalog name snapshotted.
        var linesRaw = await _db.PaymentDocumentLines.AsNoTracking()
            .Where(l => l.PaymentDocumentId == d.Id)
            .OrderBy(l => l.Id)
            .ToListAsync();
        var priceCatalogIds = linesRaw.Select(l => l.PriceCatalogId).Distinct().ToList();
        var priceCatalog = await _db.PriceCatalogs.AsNoTracking()
            .Where(p => priceCatalogIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p.Name);
        var lines = linesRaw.Select(l => new PaymentLineDto(
            l.Id, l.PriceCatalogId, priceCatalog.GetValueOrDefault(l.PriceCatalogId),
            l.UnitPrice, l.VatPercent, l.Discount, l.Quantity,
            l.Note, l.PrePaid, l.PrePaidNote,
            l.CustomerDebtId, l.Active)).ToList();
        var linesTotal = linesRaw.Where(l => l.Active).Sum(l => l.UnitPrice * l.Quantity);

        // Installments
        var insRaw = await _db.InstallmentSchedules.AsNoTracking()
            .Where(s => s.PaymentDocumentId == d.Id)
            .OrderBy(s => s.SequenceNo)
            .ToListAsync();
        var installments = insRaw.Select(s => new InstallmentDto(
            s.Id, s.SequenceNo, s.Amount, s.DueDate,
            s.Paid, s.PaidAt, s.PaidAmount, s.Note)).ToList();

        // Installment agreement (if any)
        InstallmentAgreementDto? agreement = null;
        if (d.AgreementId.HasValue)
        {
            var a = await _db.InstallmentAgreements.AsNoTracking().FirstOrDefaultAsync(x => x.Id == d.AgreementId.Value);
            if (a != null)
                agreement = new InstallmentAgreementDto(a.Id, a.Number, a.Date, a.TotalInstallments,
                    a.GuarantorName, a.GuarantorAddress, a.GuarantorEmbg);
        }

        return Ok(new PaymentDetailDto(
            d.Id, d.CompanyId,
            d.DocumentNumber, d.IssueDate, d.DueDate,
            d.PaymentTypeId, ptName,
            d.CustomerVehicleRelationId, clientName, clientMB,
            vehicleId, plate, vin, makerModel,
            d.OrganizationId, d.OperatorLegacyId,
            d.Discount, d.Paid, d.Stornoed, d.StornoReason, d.Note,
            d.AgreementId, agreement,
            d.InvoicedToCompanyId,
            d.FiscalPrintedAt, d.LegacyId,
            d.Active, d.CreatedAt, d.ModifiedAt,
            linesTotal, lines, installments));
    }

    // ---- Lookups (used by the future create-bill UI) ----

    public record PaymentTypeDto(int Id, string? Code, string Name, bool IsCash, bool IsCard, bool IsInstallment, bool PrintsReceipt, bool PrintsInvoice, string? Prefix);
    public record VatRateDto(int Id, string? Code, string Name, double Percent);

    [HttpGet("payment-types")]
    public async Task<ActionResult<IReadOnlyList<PaymentTypeDto>>> PaymentTypes() =>
        Ok(await _db.PaymentTypes.AsNoTracking().Where(p => p.Active)
            .OrderBy(p => p.Name)
            .Select(p => new PaymentTypeDto(p.Id, p.Code, p.Name, p.IsCash, p.IsCard, p.IsInstallment, p.PrintsReceipt, p.PrintsInvoice, p.Prefix))
            .ToListAsync());

    [HttpGet("vat-rates")]
    public async Task<ActionResult<IReadOnlyList<VatRateDto>>> VatRates() =>
        Ok(await _db.VatRates.AsNoTracking().Where(v => v.Active)
            .OrderBy(v => v.Percent)
            .Select(v => new VatRateDto(v.Id, v.Code, v.Name, v.Percent))
            .ToListAsync());
}
