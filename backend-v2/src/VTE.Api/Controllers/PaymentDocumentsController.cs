using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Payments;
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

    // ---- Create (Phase 2): bill from selected open debts ----

    public record CreateFromDebtsRequest(
        IReadOnlyList<long> DebtIds,
        int PaymentTypeId,
        string? Note,
        DateTime? DueDate,
        // installment ("по договор") fields — required when the type IsInstallment
        int? Installments = null,
        decimal? FirstInstallmentAmount = null,
        string? GuarantorName = null,
        string? GuarantorAddress = null,
        string? GuarantorEmbg = null);

    public record CreateBillResponse(long Id, string DocumentNumber, decimal LinesTotal, int Lines);

    /// <summary>
    /// Legacy "Направи сметка": turns selected open <see cref="VTE.Domain.Payments.CustomerDebt"/>
    /// rows into one PaymentDocument with a line per debt, marks the debts settled, and
    /// assigns the next document number in the legacy format
    /// <c>[prefix-]{org}-{seq}/{year}</c> (sequence per organization + payment type + year,
    /// continuing the migrated numbering). Cash payment types are marked Paid immediately.
    /// </summary>
    [HttpPost("from-debts")]
    public async Task<ActionResult<CreateBillResponse>> CreateFromDebts([FromBody] CreateFromDebtsRequest req)
    {
        if (req?.DebtIds is null || req.DebtIds.Count == 0)
            return BadRequest(new { error = "Не се избрани ставки за наплата." });
        if (req.DebtIds.Count > 100)
            return BadRequest(new { error = "Премногу ставки (макс. 100)." });

        var type = await _db.PaymentTypes.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == req.PaymentTypeId && t.Active);
        if (type == null)
            return BadRequest(new { error = "Непостоечки начин на плаќање." });

        var debts = await _db.CustomerDebts
            .Where(d => req.DebtIds.Contains(d.Id))
            .ToListAsync();                                   // tenant filter applies

        if (debts.Count != req.DebtIds.Count)
            return BadRequest(new { error = "Некои од избраните ставки не постојат или не се достапни." });
        if (debts.Any(d => !d.Active))
            return BadRequest(new { error = "Некои од избраните ставки се избришани." });
        if (debts.Any(d => d.Paid))
            return BadRequest(new { error = "Некои од избраните ставки се веќе платени." });

        var relationId = debts[0].CustomerVehicleRelationId;
        if (debts.Any(d => d.CustomerVehicleRelationId != relationId))
            return BadRequest(new { error = "Сите ставки мора да бидат за ист клиент и возило (една сметка = еден клиент)." });

        var companyId = debts[0].CompanyId;
        var organizationId = debts[0].OrganizationId;

        // VAT snapshot per catalog entry (current rate at the moment of billing —
        // the line keeps the snapshot so history survives future rate changes).
        var pcIds = debts.Select(d => d.PriceCatalogId).Distinct().ToList();
        var vatByPc = await (
            from pc in _db.PriceCatalogs.AsNoTracking()
            join v in _db.VatRates.AsNoTracking() on pc.VatRateId equals v.Id into vv
            from v in vv.DefaultIfEmpty()
            where pcIds.Contains(pc.Id)
            select new { pc.Id, Percent = (double?)v.Percent }
        ).ToDictionaryAsync(x => x.Id, x => x.Percent ?? 0d);

        var now = DateTime.UtcNow;

        // Installment ("по договор") validation — legacy flow: the first installment is a
        // down payment collected on the spot; the remainder splits monthly over the rest.
        var billTotal = debts.Sum(d => Math.Round(d.Price, 0, MidpointRounding.ToEven));
        if (type.IsInstallment)
        {
            if (req.Installments is not (>= 2 and <= 36))
                return BadRequest(new { error = "Бројот на рати мора да биде помеѓу 2 и 36." });
            if (req.FirstInstallmentAmount is not > 0 || req.FirstInstallmentAmount >= billTotal)
                return BadRequest(new { error = "Првата рата мора да биде поголема од 0 и помала од вкупниот износ." });
        }

        // Serializable so two simultaneous bills can't draw the same sequence number.
        await using var tx = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        // Next sequence for (org, type, year). DocumentNumber format: [prefix-]org-seq/year —
        // seq is the segment between the last '-' and the '/'. TRY_CAST skips legacy oddballs.
        var yearSuffix = $"/{now.Year}";
        var seqRow = await _db.Database.SqlQuery<long?>($@"
            SELECT MAX(TRY_CAST(LEFT(tail, CHARINDEX('/', tail) - 1) AS bigint)) AS [Value]
            FROM (
                SELECT RIGHT(DocumentNumber, CHARINDEX('-', REVERSE(DocumentNumber)) - 1) AS tail
                FROM dbo.PaymentDocument
                WHERE OrganizationId = {organizationId}
                  AND PaymentTypeId = {req.PaymentTypeId}
                  AND CHARINDEX('-', DocumentNumber) > 0
                  AND CHARINDEX('/', DocumentNumber) > 0
                  AND DocumentNumber LIKE {"%" + yearSuffix}
            ) t
            WHERE CHARINDEX('/', tail) > 1").FirstOrDefaultAsync();
        var seq = (seqRow ?? 0) + 1;
        var prefixPart = string.IsNullOrWhiteSpace(type.Prefix) ? "" : type.Prefix + "-";
        var documentNumber = $"{prefixPart}{organizationId}-{seq}{yearSuffix}";

        var doc = new PaymentDocument
        {
            CompanyId = companyId,
            PaymentTypeId = type.Id,
            CustomerVehicleRelationId = relationId,
            OrganizationId = organizationId,
            DocumentNumber = documentNumber,
            IssueDate = now,
            DueDate = req.DueDate ?? (type.IsCash ? now : now.AddDays(15)),
            Paid = type.IsCash,                                // cash settles on the spot
            Note = string.IsNullOrWhiteSpace(req.Note) ? null : req.Note.Trim(),
            CreatedByUserId = _tenant.UserId,
        };
        _db.PaymentDocuments.Add(doc);
        await _db.SaveChangesAsync();

        var lines = debts.Select(d => new PaymentDocumentLine
        {
            CompanyId = companyId,
            PaymentDocumentId = doc.Id,
            PriceCatalogId = d.PriceCatalogId,
            UnitPrice = d.Price,
            VatPercent = vatByPc.GetValueOrDefault(d.PriceCatalogId, 0d),
            Discount = 0,
            Quantity = 1,
            Note = d.Note,
            CustomerDebtId = d.Id,
        }).ToList();
        _db.PaymentDocumentLines.AddRange(lines);
        await _db.SaveChangesAsync();

        foreach (var (debt, line) in debts.Zip(lines))
        {
            debt.Paid = true;
            debt.SettledByLineId = line.Id;
        }
        await _db.SaveChangesAsync();

        // Installment agreement + schedule (legacy DogovorZaRati + PaymentDocumentsRata).
        // Rata 1 = the down payment, settled immediately; the remainder splits into equal
        // whole-denar monthly installments, the last one absorbing the rounding remainder.
        if (type.IsInstallment)
        {
            var agreement = new InstallmentAgreement
            {
                CompanyId = companyId,
                Number = $"{seq}{yearSuffix}",
                Date = DateOnly.FromDateTime(now),
                TotalInstallments = req.Installments!.Value,
                GuarantorName = Trimmed(req.GuarantorName),
                GuarantorAddress = Trimmed(req.GuarantorAddress),
                GuarantorEmbg = Trimmed(req.GuarantorEmbg),
            };
            _db.InstallmentAgreements.Add(agreement);
            await _db.SaveChangesAsync();

            doc.AgreementId = agreement.Id;
            doc.Paid = false;

            var n = req.Installments.Value;
            var first = Math.Round(req.FirstInstallmentAmount!.Value, 0, MidpointRounding.ToEven);
            var remainder = billTotal - first;
            var per = Math.Floor(remainder / (n - 1));
            var today = DateOnly.FromDateTime(now);

            var schedule = new List<InstallmentSchedule>
            {
                new()
                {
                    CompanyId = companyId, PaymentDocumentId = doc.Id, SequenceNo = 1,
                    Amount = first, DueDate = today,
                    Paid = true, PaidAt = now, PaidAmount = first,
                    OrganizationId = organizationId,
                },
            };
            for (var i = 2; i <= n; i++)
            {
                var amount = i == n ? remainder - per * (n - 2) : per;
                schedule.Add(new InstallmentSchedule
                {
                    CompanyId = companyId, PaymentDocumentId = doc.Id, SequenceNo = i,
                    Amount = amount, DueDate = today.AddMonths(i - 1),
                });
            }
            _db.InstallmentSchedules.AddRange(schedule);
            doc.DueDate = schedule[^1].DueDate!.Value.ToDateTime(TimeOnly.MinValue);
            await _db.SaveChangesAsync();
        }

        await tx.CommitAsync();

        var total = lines.Sum(l => l.UnitPrice * l.Quantity);
        return CreatedAtAction(nameof(Get), new { id = doc.Id },
            new CreateBillResponse(doc.Id, doc.DocumentNumber, total, lines.Count));
    }

    private static string? Trimmed(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    /// <summary>Settle one scheduled installment (legacy "плати рата"). When the last
    /// open installment closes, the master document flips to Paid.</summary>
    [HttpPost("{id:long}/installments/{seq:int}/pay")]
    public async Task<ActionResult<object>> PayInstallment(long id, int seq)
    {
        var doc = await _db.PaymentDocuments.FirstOrDefaultAsync(x => x.Id == id);
        if (doc == null) return NotFound();
        if (doc.Stornoed) return BadRequest(new { error = "Сметката е сторнирана." });

        var rata = await _db.InstallmentSchedules
            .FirstOrDefaultAsync(s => s.PaymentDocumentId == id && s.SequenceNo == seq && s.Active);
        if (rata == null) return NotFound();
        if (rata.Paid) return BadRequest(new { error = "Оваа рата е веќе платена." });

        rata.Paid = true;
        rata.PaidAt = DateTime.UtcNow;
        rata.PaidAmount = rata.Amount;
        rata.OrganizationId ??= doc.OrganizationId;

        var stillOpen = await _db.InstallmentSchedules
            .CountAsync(s => s.PaymentDocumentId == id && s.Active && !s.Paid && s.Id != rata.Id);
        if (stillOpen == 0) doc.Paid = true;

        await _db.SaveChangesAsync();
        return Ok(new { documentPaid = doc.Paid, paidAt = rata.PaidAt });
    }

    // ---- Fiscal printing (Accent PF-500, file-exchange protocol) ----
    //
    // The legacy WinApp printed fiscal receipts by writing a command file into a
    // folder watched by the Accent PF-500 vendor driver (FiskalModule.vb:
    // PecatiFiskalnaSmetaAccentPF500). v2 keeps the same integration: this endpoint
    // composes the exact file content; the browser writes it into the operator's
    // configured fiscal folder via the File System Access API. The driver and folder
    // already exist on every operator PC — nothing new to install.

    public record FiscalFileDto(
        bool PrintsFiscal, string? SkipReason,
        string FileName, string ContentBase64,
        DateTime? FiscalPrintedAt);

    [HttpGet("{id:long}/fiscal-file")]
    public async Task<ActionResult<FiscalFileDto>> FiscalFile(long id, [FromQuery] int? installment = null)
    {
        var doc = await _db.PaymentDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (doc == null) return NotFound();

        // Installment receipt (legacy PecatiFiskalnaSmetaZaRataAccentPF500): a single
        // "Uplata po rata" line at 0% VAT for the paid amount of that installment.
        if (installment.HasValue)
        {
            var rata = await _db.InstallmentSchedules.AsNoTracking()
                .FirstOrDefaultAsync(s => s.PaymentDocumentId == id && s.SequenceNo == installment.Value && s.Active);
            if (rata == null) return NotFound();
            if (!rata.Paid)
                return Ok(new FiscalFileDto(false, "Ратата не е платена — нема што да се фискализира.", "", "", doc.FiscalPrintedAt));

            var rsb = new System.Text.StringBuilder();
            rsb.Append(doc.Stornoed ? " U1,0000,1" : " 01,0000,1").Append("\r\n");
            rsb.Append("'1").Append("Uplata po rata").Append('\t').Append((char)194)
               .Append(((double)(rata.PaidAmount ?? rata.Amount)).ToString("F2", System.Globalization.CultureInfo.InvariantCulture))
               .Append("\r\n");
            rsb.Append(" 5 Smetka\t\r\n");
            rsb.Append(doc.Stornoed ? "%V" : "%8").Append("\r\n");

            var rtext = rsb.ToString();
            var rbytes = new byte[rtext.Length];
            for (var i = 0; i < rtext.Length; i++) rbytes[i] = (byte)rtext[i];
            return Ok(new FiscalFileDto(true, null,
                $"smetkaID{doc.Id}rata{installment.Value}.txt",
                Convert.ToBase64String(rbytes), doc.FiscalPrintedAt));
        }

        var type = await _db.PaymentTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == doc.PaymentTypeId);
        // Legacy gate: only Fiskalna_kes (cash) payment types print a fiscal receipt.
        if (type is not { IsCash: true })
            return Ok(new FiscalFileDto(false, "Фискална сметка се печати само за готовинско плаќање.", "", "", doc.FiscalPrintedAt));

        var lines = await (
            from l in _db.PaymentDocumentLines.AsNoTracking()
            join pc in _db.PriceCatalogs.AsNoTracking() on l.PriceCatalogId equals pc.Id
            where l.PaymentDocumentId == id && l.Active
            orderby l.Id
            select new { l.UnitPrice, l.Discount, l.Quantity, l.VatPercent, CatalogName = pc.Name }
        ).ToListAsync();

        var total = lines.Sum(l => Math.Round((double)l.UnitPrice * l.Quantity * (1 - l.Discount / 100), 0));
        if (lines.Count == 0 || total == 0)
            return Ok(new FiscalFileDto(false, "Сметката нема износ за фискализација.", "", "", doc.FiscalPrintedAt));

        var sb = new System.Text.StringBuilder();
        // header: open receipt (storno opens a storno receipt)
        sb.Append(doc.Stornoed ? " U1,0000,1" : " 01,0000,1").Append("\r\n");

        for (var i = 0; i < lines.Count; i++)
        {
            var l = lines[i];
            // VAT class byte (CP1251 А/Б/В): 18% → 192, 5% → 193, 0% → 194.
            // Legacy silently truncated the receipt on an unknown rate; we refuse loudly.
            var vatByte = l.VatPercent switch
            {
                18 => (byte)192,
                5  => (byte)193,
                0  => (byte)194,
                _  => (byte)0,
            };
            if (vatByte == 0)
                return BadRequest(new { error = $"Ставка со непозната ДДВ стапка ({l.VatPercent}%) — не може да се фискализира." });

            // discount applied and rounded to whole denars (legacy Math.Round semantics)
            var price = Math.Round((double)l.UnitPrice * (1 - l.Discount / 100), 0) * l.Quantity;

            sb.Append(i % 2 == 0 ? "'1" : " 1")                       // legacy alternates the marker
              .Append(Left(ToLat(l.CatalogName), 24))
              .Append('\t')
              .Append((char)vatByte)
              .Append(price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture))
              .Append("\r\n");
        }

        sb.Append(" 5 Smetka\t\r\n");                                  // subtotal command
        sb.Append(doc.Stornoed ? "%V" : "%8").Append("\r\n");          // close receipt

        // CP1251-compatible bytes: everything is ASCII after transliteration except the
        // VAT class bytes (192/193/194), which must survive as single bytes.
        var text = sb.ToString();
        var bytes = new byte[text.Length];
        for (var i = 0; i < text.Length; i++) bytes[i] = (byte)text[i];

        return Ok(new FiscalFileDto(
            true, null,
            $"smetkaID{doc.Id}.txt",
            Convert.ToBase64String(bytes),
            doc.FiscalPrintedAt));
    }

    /// <summary>Outbox marker: the browser confirmed the command file was written into
    /// the fiscal folder. Idempotent — keeps the first timestamp.</summary>
    [HttpPost("{id:long}/fiscal-printed")]
    public async Task<IActionResult> FiscalPrinted(long id)
    {
        var doc = await _db.PaymentDocuments.FirstOrDefaultAsync(x => x.Id == id);
        if (doc == null) return NotFound();
        doc.FiscalPrintedAt ??= DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static string Left(string s, int n) => s.Length <= n ? s : s[..n];

    /// <summary>Macedonian Cyrillic → Latin transliteration, ported from the legacy
    /// KondnaTastaturaModule.ToLat (incl. digraphs); unmapped characters pass through.
    /// Fixes the legacy off-by-one that left capital "А" untransliterated.</summary>
    private static string ToLat(string input)
    {
        var sb = new System.Text.StringBuilder(input.Length + 8);
        foreach (var ch in input)
        {
            sb.Append(ch switch
            {
                'А' => "A", 'Б' => "B", 'В' => "V", 'Г' => "G", 'Д' => "D", 'Е' => "E",
                'З' => "Z", 'И' => "I", 'Ј' => "J", 'К' => "K", 'Л' => "L", 'М' => "M",
                'Н' => "N", 'О' => "O", 'П' => "P", 'Р' => "R", 'С' => "S", 'Т' => "T",
                'У' => "U", 'Ф' => "F", 'Х' => "H", 'Ц' => "C",
                'а' => "a", 'б' => "b", 'в' => "v", 'г' => "g", 'д' => "d", 'е' => "e",
                'з' => "z", 'и' => "i", 'ј' => "j", 'к' => "k", 'л' => "l", 'м' => "m",
                'н' => "n", 'о' => "o", 'п' => "p", 'р' => "r", 'с' => "s", 'т' => "t",
                'у' => "u", 'ф' => "f", 'х' => "h", 'ц' => "c",
                'Ѕ' => "DZ", 'ѕ' => "dz", 'Љ' => "LJ", 'љ' => "lj", 'Њ' => "NJ", 'њ' => "nj",
                'Ѓ' => "GJ", 'ѓ' => "gj", 'Ж' => "ZH", 'ж' => "zh", 'Ќ' => "KJ", 'ќ' => "kj",
                'Ч' => "CH", 'ч' => "ch", 'Ш' => "SH", 'ш' => "sh", 'Џ' => "DJ", 'џ' => "dj",
                _ => ch.ToString(),
            });
        }
        return sb.ToString();
    }

    // ---- Lookups (used by the create-bill UI) ----

    public record PaymentTypeDto(int Id, string? Code, string Name, bool IsCash, bool IsCard, bool IsInstallment, bool PrintsReceipt, bool PrintsInvoice, string? Prefix);
    public record VatRateDto(int Id, string? Code, string Name, double Percent);

    /// <summary>Payment types. The legacy table repeats each type once per company (no
    /// company column — just duplicated rows, and the same fee can carry a different
    /// doc-number Prefix per company), so <paramref name="usedOnly"/>=true keeps, per
    /// type NAME, only the id whose MOST RECENT bill is newest — i.e. the one the station
    /// is currently billing with. Grouping by Name alone (not Name+Prefix) is deliberate:
    /// "по договор" exists under several prefixes across companies and must collapse to a
    /// single choice. Most-recent beats most-total: an id can have more lifetime docs yet
    /// have been retired years ago (seen with "кредитна" 19 vs 24, "по договор" 18 vs 23).</summary>
    [HttpGet("payment-types")]
    public async Task<ActionResult<IReadOnlyList<PaymentTypeDto>>> PaymentTypes([FromQuery] bool usedOnly = false)
    {
        var q = _db.PaymentTypes.AsNoTracking().Where(p => p.Active);
        if (!usedOnly)
            return Ok(await q.OrderBy(p => p.Name)
                .Select(p => new PaymentTypeDto(p.Id, p.Code, p.Name, p.IsCash, p.IsCard, p.IsInstallment, p.PrintsReceipt, p.PrintsInvoice, p.Prefix))
                .ToListAsync());

        var withRecency = await q
            .Select(p => new
            {
                Type = p,
                LastDocId = _db.PaymentDocuments
                    .Where(d => d.PaymentTypeId == p.Id)
                    .Max(d => (long?)d.Id),
            })
            .Where(x => x.LastDocId != null)
            .ToListAsync();

        var deduped = withRecency
            .GroupBy(x => (x.Type.Name ?? "").Trim())   // legacy names carry stray leading spaces
            .Select(g => g.OrderByDescending(x => x.LastDocId).First().Type)
            .OrderBy(p => p.Name)
            .Select(p => new PaymentTypeDto(p.Id, p.Code, p.Name, p.IsCash, p.IsCard, p.IsInstallment, p.PrintsReceipt, p.PrintsInvoice, p.Prefix))
            .ToList();

        return Ok(deduped);
    }

    [HttpGet("vat-rates")]
    public async Task<ActionResult<IReadOnlyList<VatRateDto>>> VatRates() =>
        Ok(await _db.VatRates.AsNoTracking().Where(v => v.Active)
            .OrderBy(v => v.Percent)
            .Select(v => new VatRateDto(v.Id, v.Code, v.Name, v.Percent))
            .ToListAsync());
}
