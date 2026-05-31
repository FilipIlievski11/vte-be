using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Payments;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/payment-documents")]
[Authorize]
public class PaymentDocumentsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    private readonly VTE.Api.Payments.PaymentCalculationService _calc;

    public PaymentDocumentsController(VteDbContext db, ITenantContext tenant, VTE.Api.Payments.PaymentCalculationService calc)
    { _db = db; _tenant = tenant; _calc = calc; }

    public record CalculateRequest(long VehicleId, string Trigger);
    public record CalculateLineDto(int PaymentCategoryId, string CategoryName, int PaymentItemId, string ItemName,
        int? PaymentItemParametarId, string? ParametarName, decimal Price, decimal DDVRate,
        bool AllowDiscount, bool IsOptional, string? MatchedVehicleField, decimal? MatchedVehicleValue);
    public record CalculateResultDto(long VehicleId, int? VehicleCategoryForPaymentsId,
        List<CalculateLineDto> Lines, List<string> Warnings);

    // POST /api/payment-documents/calculate
    // Runs the legacy auto-calc engine for a vehicle + a trigger (Request, TechnicalExam, ...).
    // Returns the lines the UI should pre-fill in the PaymentDocument form.
    [HttpPost("calculate")]
    public async Task<ActionResult<CalculateResultDto>> Calculate([FromBody] CalculateRequest req, CancellationToken ct)
    {
        if (!Enum.TryParse<VTE.Api.Payments.PaymentTrigger>(req.Trigger, true, out var trig))
            return BadRequest(new { error = $"Unknown trigger '{req.Trigger}'. Use Request, TechnicalExam, TrafficLicence, PermissionForVehicle, InternationalDriverLicence, IrregularTechnicalExam." });
        var r = await _calc.CalculateForVehicleAsync(req.VehicleId, trig, ct);
        return Ok(new CalculateResultDto(r.VehicleId, r.VehicleCategoryForPaymentsId,
            r.Lines.Select(x => new CalculateLineDto(x.PaymentCategoryId, x.CategoryName, x.PaymentItemId, x.ItemName,
                x.PaymentItemParametarId, x.ParametarName, x.Price, x.DDVRate,
                x.AllowDiscount, x.IsOptional, x.MatchedVehicleField, x.MatchedVehicleValue)).ToList(),
            r.Warnings));
    }

    public record PaymentDocumentDto(long Id, string DocumentNumber, DateOnly DatePay, decimal TotalAmount, decimal TotalInstallments, bool Payed, bool Storno);
    public record DetailLineRequest(int PriceCatalogId, decimal Price, decimal DDVRate, decimal DiscountPercent, bool PrePayed, string? Note, int? PaymentItemId = null, string? NotePrePayed = null);
    public record InstallmentRequest(int InstallmentNumber, decimal Price, DateOnly? DueDate);
    public record GuarantorRequest(
        string ContractNumber, DateOnly ContractDate, int NumberOfInstallments,
        decimal FirstInstallment, DateOnly FirstInstallmentDate,
        string? GuarantorName, string? GuarantorAddress, string? GuarantorEMBG, string? Note);
    public record CreatePaymentDocumentRequest(
        int PaymentTypeId,
        long CustomerVehicleRelationId,
        long? InstallmentContractId,
        long? BillToCustomerId,
        int? TechnicalExamOrganizationId,
        DateOnly DatePay,
        DateOnly DateRequired,
        decimal DiscountPercent,
        string? Note,
        List<DetailLineRequest> Details,
        List<InstallmentRequest>? Installments,
        GuarantorRequest? Guarantor = null,        // when set + PaymentType.IsInstallments,
                                                   // creates an InstallmentContract +
                                                   // auto-computes the Installments list.
        bool IsTechExamReport = false,
        string? DocumentNumber = null);            // optional override; auto-generated when null

    public record PaymentDocumentRow(long Id, string DocumentNumber, DateOnly DatePay, DateOnly DateRequired,
        decimal TotalAmount, decimal TotalInstallments, bool Payed, bool Storno,
        int? PaymentTypeId, string? PaymentTypeName,
        long CustomerVehicleRelationId, string? CustomerName, string? VehicleReg);

    [HttpGet]
    public async Task<ActionResult<PagedResult<PaymentDocumentRow>>> List(
        [FromQuery] bool? unpaid, [FromQuery] bool? storno,
        [FromQuery] DateOnly? from, [FromQuery] DateOnly? to,
        [FromQuery] int? paymentTypeId, [FromQuery] string? q,
        [FromQuery] int? page, [FromQuery] int? pageSize,
        [FromQuery] string? sortBy, [FromQuery] string? sortDir)
    {
        var src = _db.PaymentDocuments.AsNoTracking().AsQueryable();
        if (unpaid == true) src = src.Where(d => !d.Payed && !d.Storno);
        if (storno == true) src = src.Where(d => d.Storno);
        if (storno == false) src = src.Where(d => !d.Storno);
        if (from is not null) src = src.Where(d => d.DatePay >= from);
        if (to   is not null) src = src.Where(d => d.DatePay <= to);
        if (paymentTypeId is not null) src = src.Where(d => d.PaymentTypeId == paymentTypeId);
        if (!string.IsNullOrWhiteSpace(q)) src = src.Where(d => EF.Functions.Like(d.DocumentNumber, $"%{q}%"));

        var joined =
            from d in src
            join pt in _db.PaymentTypes on d.PaymentTypeId equals pt.Id into ptj from pt in ptj.DefaultIfEmpty()
            join cvr in _db.CustomerVehicleRelations on d.CustomerVehicleRelationId equals cvr.Id into cvrj from cvr in cvrj.DefaultIfEmpty()
            join c in _db.Customers on cvr.CustomerId equals c.Id into cj from c in cj.DefaultIfEmpty()
            join v in _db.Vehicles on cvr.VehicleId equals v.Id into vj from v in vj.DefaultIfEmpty()
            select new {
                d.Id, d.DocumentNumber, d.DatePay, d.DateRequired, d.DiscountPercent, d.Payed, d.Storno,
                d.PaymentTypeId, PtName = pt == null ? null : pt.Name,
                d.CustomerVehicleRelationId,
                CustomerName = c == null ? null : ((c.Surname ?? "") + " " + c.FirstName).Trim(),
                VehicleReg   = v == null ? null : v.LastRegistrationNumber,
            };

        var desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
        joined = (sortBy?.ToLowerInvariant()) switch {
            "documentnumber" => desc ? joined.OrderByDescending(x => x.DocumentNumber) : joined.OrderBy(x => x.DocumentNumber),
            "datepay"        => desc ? joined.OrderByDescending(x => x.DatePay)        : joined.OrderBy(x => x.DatePay),
            "customername"   => desc ? joined.OrderByDescending(x => x.CustomerName)   : joined.OrderBy(x => x.CustomerName),
            "vehiclereg"     => desc ? joined.OrderByDescending(x => x.VehicleReg)     : joined.OrderBy(x => x.VehicleReg),
            "paymenttypename"=> desc ? joined.OrderByDescending(x => x.PtName)         : joined.OrderBy(x => x.PtName),
            _                => joined.OrderByDescending(x => x.DatePay),
        };

        var ordered = joined.Select(x => new PaymentDocumentRow(
            x.Id, x.DocumentNumber, x.DatePay, x.DateRequired,
            _db.PaymentDocumentDetails.Where(d => d.PaymentDocumentId == x.Id)
                .Sum(d => (decimal?)(d.Price * (1 - d.DiscountPercent / 100m))) * (1 - x.DiscountPercent / 100m) ?? 0m,
            _db.PaymentDocumentInstallments.Where(d => d.PaymentDocumentId == x.Id)
                .Sum(d => (decimal?)d.Price) ?? 0m,
            x.Payed, x.Storno, x.PaymentTypeId, x.PtName,
            x.CustomerVehicleRelationId, x.CustomerName, x.VehicleReg));

        return Ok(await ordered.ToPagedAsync(page, pageSize));
    }

    public record DetailLine(long Id, int PriceCatalogId, int? PaymentItemId, string? ItemName,
        decimal Price, decimal DDVRate, decimal DiscountPercent, bool PrePayed, string? Note, string? NotePrePayed);
    public record InstallmentLine(long Id, int InstallmentNumber, decimal Price, DateOnly? DueDate,
        bool Payed, DateOnly? DatePayed, string? Note);
    public record DocumentDetailDto(
        long Id, string DocumentNumber, int PaymentTypeId, string? PaymentTypeName, bool TypeIsInstallments,
        long CustomerVehicleRelationId, long? CustomerId, string? CustomerName, long? VehicleId,
        string? VehicleReg, string? VehicleVin,
        long? InstallmentContractId, long? BillToCustomerId, int? TechnicalExamOrganizationId,
        DateOnly DatePay, DateOnly DateRequired, decimal DiscountPercent,
        bool Payed, bool Storno, decimal? PolicyNumber, string? Note,
        decimal Total, decimal TotalInstallments,
        List<DetailLine> Details, List<InstallmentLine> Installments);

    [HttpGet("{id:long}")]
    public async Task<ActionResult<DocumentDetailDto>> Get(long id)
    {
        var d = await _db.PaymentDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (d is null) return NotFound();
        var typeRow = await _db.PaymentTypes.Where(t => t.Id == d.PaymentTypeId)
            .Select(t => new { t.Name, t.IsInstallments }).FirstOrDefaultAsync();
        var rel = await _db.CustomerVehicleRelations.AsNoTracking().FirstOrDefaultAsync(r => r.Id == d.CustomerVehicleRelationId);
        var customer = rel is null ? null : await _db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == rel.CustomerId);
        var vehicle  = rel is null ? null : await _db.Vehicles .AsNoTracking().FirstOrDefaultAsync(v => v.Id == rel.VehicleId);

        var details = await _db.PaymentDocumentDetails.AsNoTracking()
            .Where(x => x.PaymentDocumentId == id).OrderBy(x => x.Id)
            .Select(x => new DetailLine(x.Id, x.PriceCatalogId, x.PaymentItemId,
                x.PaymentItemId == null ? null : _db.PaymentItems.Where(p => p.Id == x.PaymentItemId).Select(p => p.ItemName).FirstOrDefault(),
                x.Price, x.DDVRate, x.DiscountPercent, x.PrePayed, x.Note, x.NotePrePayed))
            .ToListAsync();

        var installments = await _db.PaymentDocumentInstallments.AsNoTracking()
            .Where(x => x.PaymentDocumentId == id).OrderBy(x => x.InstallmentNumber)
            .Select(x => new InstallmentLine(x.Id, x.InstallmentNumber, x.Price, x.DueDate, x.Payed, x.DatePayed, x.Note))
            .ToListAsync();

        var total = details.Where(x => !x.PrePayed).Sum(x => x.Price * (1 - x.DiscountPercent / 100m))
                    * (1 - d.DiscountPercent / 100m);
        var totalInstallments = installments.Sum(x => x.Price);

        return Ok(new DocumentDetailDto(
            d.Id, d.DocumentNumber, d.PaymentTypeId, typeRow?.Name, typeRow?.IsInstallments ?? false,
            d.CustomerVehicleRelationId, customer?.Id,
            customer == null ? null : ((customer.Surname ?? "") + " " + customer.FirstName).Trim(),
            vehicle?.Id, vehicle?.LastRegistrationNumber, vehicle?.ShellNumber,
            d.InstallmentContractId, d.BillToCustomerId, d.TechnicalExamOrganizationId,
            d.DatePay, d.DateRequired, d.DiscountPercent, d.Payed, d.Storno, d.PolicyNumber, d.Note,
            total, totalInstallments, details, installments));
    }

    public record PaymentSummaryDto(int Total, decimal TotalPaid, decimal TotalUnpaid, decimal TotalStorno);

    [HttpGet("summary")]
    public async Task<ActionResult<PaymentSummaryDto>> Summary(
        [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var q = _db.PaymentDocuments.AsNoTracking().AsQueryable();
        if (from is not null) q = q.Where(d => d.DatePay >= from);
        if (to   is not null) q = q.Where(d => d.DatePay <= to);
        var totals = await q.Select(d => new {
            Amount = _db.PaymentDocumentDetails.Where(x => x.PaymentDocumentId == d.Id)
                .Sum(x => (decimal?)(x.Price * (1 - x.DiscountPercent / 100m))) * (1 - d.DiscountPercent / 100m) ?? 0m,
            d.Payed, d.Storno
        }).ToListAsync();
        return Ok(new PaymentSummaryDto(
            totals.Count,
            totals.Where(t => t.Payed && !t.Storno).Sum(t => t.Amount),
            totals.Where(t => !t.Payed && !t.Storno).Sum(t => t.Amount),
            totals.Where(t => t.Storno).Sum(t => t.Amount)));
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDocumentDto>> Create(CreatePaymentDocumentRequest req)
    {
        if (req.Details is null || req.Details.Count == 0)
            return BadRequest(new { error = "BR-PAY-006: at least one PaymentDocumentDetail is required." });
        if (req.DiscountPercent < 0 || req.DiscountPercent >= 100)
            return BadRequest(new { error = "BR-PAY-012: document discount must be in [0, 100)." });
        foreach (var d in req.Details)
        {
            if (d.DiscountPercent < 0 || d.DiscountPercent >= 100)
                return BadRequest(new { error = "BR-PAY-012: line discount must be in [0, 100)." });
            if (d.Price < 0) return BadRequest(new { error = "Line price must be >= 0." });
        }

        // BR-PAY-007: sum of installments <= sum of details
        var totalDetails = req.Details.Sum(x => x.Price);
        var totalInstallments = req.Installments?.Sum(x => x.Price) ?? 0m;
        if (totalInstallments > totalDetails)
            return BadRequest(new { error = "BR-PAY-007: sum of installments cannot exceed sum of bill line items (Збирот на ратите не смее да биде поголем од вкупната сметка)." });

        if (_tenant.StationId is null) return BadRequest(new { error = "StationId could not be resolved." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Auto-generate DocumentNumber when client doesn't pre-assign one.
        // Format mirrors legacy: {Prefix}-{StationId}-{Number}/{Year}
        var docNumber = req.DocumentNumber;
        if (string.IsNullOrWhiteSpace(docNumber))
        {
            docNumber = await AllocateDocumentNumberAsync(_tenant.StationId.Value, req.PaymentTypeId, req.IsTechExamReport, req.DatePay.Year);
        }

        // Guarantor → InstallmentContract + auto-computed installment rows.
        // Legacy: first installment is set by the user (FirstInstallment), the
        // remaining (NumberOfInstallments-1) split the remainder equally, each
        // due one month after the previous. First installment auto-marked Payed.
        long? installmentContractId = req.InstallmentContractId;
        var generatedInstallments = new List<InstallmentRequest>();
        if (req.Guarantor is not null)
        {
            var g = req.Guarantor;
            if (g.NumberOfInstallments < 2 || g.NumberOfInstallments > 60)
                return BadRequest(new { error = "BR-PAY-034: NumberOfInstallments must be in [2, 60]." });
            var totalDue = req.Details.Where(d => !d.PrePayed).Sum(d => d.Price * (1 - d.DiscountPercent / 100m))
                           * (1 - req.DiscountPercent / 100m);
            if (g.FirstInstallment <= 0 || g.FirstInstallment >= totalDue)
                return BadRequest(new { error = "Guarantor.FirstInstallment must be > 0 and < grand total." });

            var contract = new InstallmentContract
            {
                StationId = _tenant.StationId.Value,
                ContractNumber = g.ContractNumber,
                ContractDate = g.ContractDate,
                NumberOfInstallments = g.NumberOfInstallments,
                GuarantorName = g.GuarantorName,
                GuarantorAddress = g.GuarantorAddress,
                GuarantorEMBG = g.GuarantorEMBG,
                Note = g.Note,
            };
            _db.InstallmentContracts.Add(contract);
            await _db.SaveChangesAsync();
            installmentContractId = contract.Id;

            var remaining = totalDue - g.FirstInstallment;
            var per = Math.Round(remaining / (g.NumberOfInstallments - 1), 2, MidpointRounding.AwayFromZero);
            // First installment (marked paid below via Installments table when persisted)
            generatedInstallments.Add(new InstallmentRequest(1, g.FirstInstallment, g.FirstInstallmentDate));
            for (var i = 2; i <= g.NumberOfInstallments; i++)
                generatedInstallments.Add(new InstallmentRequest(i, per, g.FirstInstallmentDate.AddMonths(i - 1)));
        }

        var doc = new PaymentDocument
        {
            StationId = _tenant.StationId.Value,
            DocumentNumber = docNumber,
            PaymentTypeId = req.PaymentTypeId,
            CustomerVehicleRelationId = req.CustomerVehicleRelationId,
            InstallmentContractId = installmentContractId,
            BillToCustomerId = req.BillToCustomerId,
            TechnicalExamOrganizationId = req.TechnicalExamOrganizationId,
            DatePay = req.DatePay,
            DateRequired = req.DateRequired,
            DiscountPercent = req.DiscountPercent,
            Note = req.Note,
            CreatedByOperatorUserId = userId,
            Details = req.Details.Select(d => new PaymentDocumentDetail
            {
                PriceCatalogId = d.PriceCatalogId,
                PaymentItemId  = d.PaymentItemId,
                Price = d.Price,
                DDVRate = d.DDVRate,
                DiscountPercent = d.DiscountPercent,
                PrePayed = d.PrePayed,
                Note = d.Note,
                NotePrePayed = d.NotePrePayed,
            }).ToList(),
            Installments = (req.Installments ?? generatedInstallments).Select(i => new PaymentDocumentInstallment
            {
                InstallmentNumber = i.InstallmentNumber,
                Price = i.Price,
                DueDate = i.DueDate,
                Payed = req.Guarantor is not null && i.InstallmentNumber == 1,
                DatePayed = req.Guarantor is not null && i.InstallmentNumber == 1 ? req.Guarantor.FirstInstallmentDate : null,
            }).ToList()
        };

        _db.PaymentDocuments.Add(doc);
        await _db.SaveChangesAsync();

        // Emit a CustomerFinancialState debit row per non-pre-paid detail so the
        // running-balance / unpaid-deals views stay in sync.
        await EmitFinancialStateAsync(doc);

        return Ok(new PaymentDocumentDto(doc.Id, doc.DocumentNumber, doc.DatePay,
            doc.Details.Sum(x => x.Price * (1 - x.DiscountPercent / 100m)) * (1 - doc.DiscountPercent / 100m),
            doc.Installments.Sum(x => x.Price), doc.Payed, doc.Storno));
    }

    [HttpPost("{id:long}/storno")]
    public async Task<IActionResult> Storno(long id)
    {
        var d = await _db.PaymentDocuments.FindAsync(id);
        if (d is null) return NotFound();
        d.Storno = true;
        await _db.SaveChangesAsync();
        await EmitFinancialStateCreditAsync(d, "Сторнирано");
        return NoContent();
    }

    [HttpPost("installments/{installmentId:long}/pay")]
    public async Task<IActionResult> PayInstallment(long installmentId)
    {
        var i = await _db.PaymentDocumentInstallments.FindAsync(installmentId);
        if (i is null) return NotFound();
        if (i.Payed) return BadRequest(new { error = "Installment already paid." });
        i.Payed = true;
        i.DatePayed = DateOnly.FromDateTime(DateTime.UtcNow);
        i.CollectedByOperatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id:long}/pay")]
    public async Task<IActionResult> PayDocument(long id)
    {
        var d = await _db.PaymentDocuments.FindAsync(id);
        if (d is null) return NotFound();
        if (d.Storno) return BadRequest(new { error = "Cannot mark a Storno document as paid." });
        d.Payed = true;
        await _db.SaveChangesAsync();
        await EmitFinancialStateCreditAsync(d, "Платено");
        return NoContent();
    }

    public record UpdatePaymentDocumentRequest(
        DateOnly DatePay, DateOnly DateRequired, decimal DiscountPercent, string? Note,
        long? BillToCustomerId, int? TechnicalExamOrganizationId,
        List<DetailLineRequest> Details);

    // Edit allowed only while not Storno. Replaces details wholesale (legacy parity).
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UpdatePaymentDocumentRequest req)
    {
        var doc = await _db.PaymentDocuments.FirstOrDefaultAsync(x => x.Id == id);
        if (doc is null) return NotFound();
        if (doc.Storno) return BadRequest(new { error = "Cannot edit a Storno document." });

        if (req.DiscountPercent < 0 || req.DiscountPercent >= 100)
            return BadRequest(new { error = "BR-PAY-012: document discount must be in [0, 100)." });
        foreach (var d in req.Details)
        {
            if (d.DiscountPercent < 0 || d.DiscountPercent >= 100)
                return BadRequest(new { error = "BR-PAY-012: line discount must be in [0, 100)." });
            if (d.Price < 0) return BadRequest(new { error = "Line price must be >= 0." });
        }

        doc.DatePay = req.DatePay;
        doc.DateRequired = req.DateRequired;
        doc.DiscountPercent = req.DiscountPercent;
        doc.Note = req.Note;
        doc.BillToCustomerId = req.BillToCustomerId;
        doc.TechnicalExamOrganizationId = req.TechnicalExamOrganizationId;

        var existing = await _db.PaymentDocumentDetails.Where(d => d.PaymentDocumentId == id).ToListAsync();
        _db.PaymentDocumentDetails.RemoveRange(existing);
        foreach (var d in req.Details)
        {
            _db.PaymentDocumentDetails.Add(new PaymentDocumentDetail
            {
                PaymentDocumentId = id,
                PriceCatalogId = d.PriceCatalogId,
                PaymentItemId = d.PaymentItemId,
                Price = d.Price, DDVRate = d.DDVRate, DiscountPercent = d.DiscountPercent,
                PrePayed = d.PrePayed, Note = d.Note, NotePrePayed = d.NotePrePayed,
            });
        }
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // PDF output for a payment document.
    //   /print/invoice  -> faktura
    //   /print/smetka   -> cash receipt
    //   /print/rata     -> installment table
    //   /print/dogovor  -> installment agreement (with guarantor)
    [HttpGet("{id:long}/print/{kind}")]
    public async Task<IActionResult> Print(long id, string kind, CancellationToken ct)
    {
        var normalized = (kind ?? "").ToLowerInvariant() switch
        {
            "invoice" or "faktura" => "Faktura",
            "smetka"  or "receipt" => "Smetka",
            "rata"    or "installment" => "Rata",
            "dogovor" or "agreement" or "contract" => "Dogovor",
            _ => kind ?? "",
        };
        if (!Enum.TryParse<VTE.Api.Pdf.PaymentDocumentPdf.Kind>(normalized, ignoreCase: true, out var k))
            return BadRequest(new { error = "Unknown print kind. Use invoice/smetka/rata/dogovor." });

        var d = await _db.PaymentDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (d is null) return NotFound();
        var ptype  = await _db.PaymentTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == d.PaymentTypeId, ct);
        var rel    = await _db.CustomerVehicleRelations.AsNoTracking().FirstOrDefaultAsync(r => r.Id == d.CustomerVehicleRelationId, ct);
        var cust   = rel == null ? null : await _db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == rel.CustomerId, ct);
        var veh    = rel == null ? null : await _db.Vehicles .AsNoTracking().FirstOrDefaultAsync(v => v.Id == rel.VehicleId,  ct);
        var maker  = veh?.VehicleModelId.HasValue == true
            ? await (from m in _db.VehicleModels.AsNoTracking()
                     join mk in _db.VehicleMakers.AsNoTracking() on m.VehicleMakerId equals mk.Id
                     where m.Id == veh.VehicleModelId
                     select mk.Name + " " + m.Name).FirstOrDefaultAsync(ct)
            : null;
        var station = _tenant.StationId.HasValue
            ? await _db.Stations.AsNoTracking().FirstOrDefaultAsync(s => s.Id == _tenant.StationId.Value, ct)
            : null;
        var contract = d.InstallmentContractId.HasValue
            ? await _db.InstallmentContracts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == d.InstallmentContractId.Value, ct)
            : null;
        var details = await _db.PaymentDocumentDetails.AsNoTracking()
            .Where(x => x.PaymentDocumentId == id).OrderBy(x => x.Id).ToListAsync(ct);
        var detailItemIds = details.Where(x => x.PaymentItemId.HasValue).Select(x => x.PaymentItemId!.Value).Distinct().ToList();
        var itemNames = await _db.PaymentItems.AsNoTracking()
            .Where(p => detailItemIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p.ItemName, ct);
        var installments = await _db.PaymentDocumentInstallments.AsNoTracking()
            .Where(x => x.PaymentDocumentId == id).OrderBy(x => x.InstallmentNumber).ToListAsync(ct);

        // CalculationItem (bank account) for the first detail's category
        VTE.Domain.Reference.CalculationItem? calc = null;
        if (details.Count > 0 && details[0].PaymentItemId.HasValue)
        {
            var firstItem = await _db.PaymentItems.AsNoTracking().FirstOrDefaultAsync(p => p.Id == details[0].PaymentItemId!.Value, ct);
            if (firstItem is not null)
            {
                var cat = await _db.PaymentCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == firstItem.PaymentCategoryId, ct);
                if (cat?.CalculationItemId is int ciid)
                    calc = await _db.CalculationItems.AsNoTracking().FirstOrDefaultAsync(c => c.Id == ciid, ct);
            }
        }

        var detailLines = details.Select((x, idx) =>
        {
            var net   = x.Price * (1 - x.DiscountPercent / 100m);
            var ddv   = net * (x.DDVRate / 100m);
            var gross = net + ddv;
            return new VTE.Api.Pdf.PaymentDocumentPdf.DetailLine(
                idx + 1,
                (itemNames.TryGetValue(x.PaymentItemId ?? 0, out var n) ? n : x.Note) ?? "",
                x.Price, x.DDVRate, x.DiscountPercent, x.PrePayed,
                net, ddv, gross, x.Note);
        }).ToList();

        var data = new VTE.Api.Pdf.PaymentDocumentPdf.PaymentData(
            d.Id, d.DocumentNumber, ptype?.Name ?? "", ptype?.Prefix,
            d.DatePay, d.DateRequired, d.DiscountPercent,
            d.Payed, d.Storno, d.Note,
            station?.Name ?? "VTE", null, null, null,
            cust == null ? null : ((cust.Surname ?? "") + " " + cust.FirstName).Trim(),
            null, cust?.EMBG,
            veh?.LastRegistrationNumber, veh?.ShellNumber, maker,
            detailLines,
            installments.Select(x => new VTE.Api.Pdf.PaymentDocumentPdf.InstallmentLine(
                x.InstallmentNumber, x.Price, x.DueDate, x.Payed, x.DatePayed)).ToList(),
            contract?.GuarantorName, contract?.GuarantorAddress, contract?.GuarantorEMBG,
            contract?.NumberOfInstallments,
            calc?.ItemName, calc?.BankAccount, calc?.Bank, calc?.Form);

        var bytes = VTE.Api.Pdf.PaymentDocumentPdf.Build(k, data);
        return File(bytes, "application/pdf", $"payment-{id}-{kind.ToLowerInvariant()}.pdf");
    }

    // Adds one CustomerFinancialState row per non-pre-paid detail: a debit against
    // the relation owner. The legacy SP `addPaymentDocumentsDetailFinace` does the
    // same on the legacy side. Running balance is maintained as the sum of all
    // debits minus credits for that customer at the report-read time.
    private async Task EmitFinancialStateAsync(PaymentDocument doc)
    {
        var rel = await _db.CustomerVehicleRelations.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == doc.CustomerVehicleRelationId);
        if (rel is null) return;
        foreach (var d in doc.Details.Where(x => !x.PrePayed))
        {
            var gross = d.Price * (1 - d.DiscountPercent / 100m);
            _db.CustomerFinancialStates.Add(new VTE.Domain.Payments.CustomerFinancialState
            {
                CustomerId = rel.CustomerId,
                StationId = doc.StationId,
                Date = doc.DatePay,
                Description = doc.DocumentNumber + (d.Note is null ? "" : " · " + d.Note),
                DebitAmount = gross,
                CreditAmount = 0,
                RunningBalance = 0,                 // recomputed lazily on report read
                PaymentDocumentId = doc.Id,
            });
        }
        await _db.SaveChangesAsync();
    }

    // Adds a single credit row matching the document's gross when the document is
    // marked Payed (or a reversal credit on Storno).
    private async Task EmitFinancialStateCreditAsync(PaymentDocument doc, string description)
    {
        var rel = await _db.CustomerVehicleRelations.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == doc.CustomerVehicleRelationId);
        if (rel is null) return;
        var grossNet = await _db.PaymentDocumentDetails
            .Where(d => d.PaymentDocumentId == doc.Id && !d.PrePayed)
            .SumAsync(d => (decimal?)(d.Price * (1 - d.DiscountPercent / 100m))) ?? 0m;
        var amount = grossNet * (1 - doc.DiscountPercent / 100m);
        _db.CustomerFinancialStates.Add(new VTE.Domain.Payments.CustomerFinancialState
        {
            CustomerId = rel.CustomerId,
            StationId = doc.StationId,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            Description = doc.DocumentNumber + " · " + description,
            DebitAmount = 0,
            CreditAmount = amount,
            RunningBalance = 0,
            PaymentDocumentId = doc.Id,
        });
        await _db.SaveChangesAsync();
    }

    // Allocates next DocumentNumber via the PaymentDocumentNumbers row keyed by
    // (StationId, PaymentTypeId, IsTechExamReport). Atomic UPDATE...OUTPUT pattern
    // avoids race conditions between concurrent saves on the same station.
    private async Task<string> AllocateDocumentNumberAsync(int stationId, int paymentTypeId, bool isTechExam, int year)
    {
        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync();

        // Make sure the row exists
        await using (var ins = conn.CreateCommand())
        {
            ins.CommandText = @"
IF NOT EXISTS (SELECT 1 FROM dbo.PaymentDocumentNumbers
               WHERE StationId=@s AND PaymentTypeId=@t AND IsTechExamReport=@x)
    INSERT INTO dbo.PaymentDocumentNumbers (StationId, PaymentTypeId, IsTechExamReport, Number)
    VALUES (@s, @t, @x, 0);";
            AddParam(ins, "@s", stationId); AddParam(ins, "@t", paymentTypeId); AddParam(ins, "@x", isTechExam);
            await ins.ExecuteNonQueryAsync();
        }

        int newNumber;
        await using (var upd = conn.CreateCommand())
        {
            upd.CommandText = @"
UPDATE dbo.PaymentDocumentNumbers SET Number = Number + 1
OUTPUT inserted.Number
WHERE StationId=@s AND PaymentTypeId=@t AND IsTechExamReport=@x;";
            AddParam(upd, "@s", stationId); AddParam(upd, "@t", paymentTypeId); AddParam(upd, "@x", isTechExam);
            newNumber = (int)(await upd.ExecuteScalarAsync() ?? 0);
        }

        var prefix = await _db.PaymentTypes.Where(t => t.Id == paymentTypeId).Select(t => t.Prefix).FirstOrDefaultAsync();
        prefix = string.IsNullOrWhiteSpace(prefix) ? "PD" : prefix;
        return $"{prefix}-{stationId}-{newNumber}/{year}";

        static void AddParam(System.Data.Common.DbCommand cmd, string name, object value)
        {
            var p = cmd.CreateParameter(); p.ParameterName = name; p.Value = value; cmd.Parameters.Add(p);
        }
    }
}
