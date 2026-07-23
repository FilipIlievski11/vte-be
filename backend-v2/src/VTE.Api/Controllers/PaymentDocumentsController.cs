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

        // Lines with the LEGACY-composed service name: "{Category} {Item} {Parametar}" —
        // e.g. "Црвен крст за Патнички возила Црвен крст" — exactly what the legacy
        // bill grid displayed (item names already start with "за ...").
        var linesRaw = await _db.PaymentDocumentLines.AsNoTracking()
            .Where(l => l.PaymentDocumentId == d.Id)
            .OrderBy(l => l.Id)
            .ToListAsync();
        var priceCatalogIds = linesRaw.Select(l => l.PriceCatalogId).Distinct().ToList();
        var priceCatalog = await _db.PriceCatalogs.AsNoTracking()
            .Where(p => priceCatalogIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Name, p.PaymentCategoryGroupId, p.ParametarFrom, p.ParametarTo })
            .ToDictionaryAsync(p => p.Id);
        var groupIds = priceCatalog.Values
            .Where(p => p.PaymentCategoryGroupId.HasValue)
            .Select(p => p.PaymentCategoryGroupId!.Value).Distinct().ToList();
        var groupNames = await _db.PaymentCategoryGroups.AsNoTracking()
            .Where(g => groupIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, g => g.Name);
        string? composedName(int pcId)
        {
            if (!priceCatalog.TryGetValue(pcId, out var p)) return null;
            var group = p.PaymentCategoryGroupId.HasValue ? groupNames.GetValueOrDefault(p.PaymentCategoryGroupId.Value) : null;
            return ComposeServiceName(group, p.Name, p.ParametarFrom, p.ParametarTo);
        }
        var lines = linesRaw.Select(l => new PaymentLineDto(
            l.Id, l.PriceCatalogId, composedName(l.PriceCatalogId),
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
               .Append(FormatFiscal(FicalRound((double)(rata.PaidAmount ?? rata.Amount))))
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

        // Legacy SP GetPaymentDocumentForFiscalPrintByIdDocument: active lines only,
        // PrePayed excluded, and the line's NAME on the receipt is the payment CATEGORY
        // (PaymentCategories.CategoryName), resolved through the catalog. The inner joins
        // silently drop lines whose category chain is broken — kept identical here.
        var lines = await (
            from l in _db.PaymentDocumentLines.AsNoTracking()
            join pc in _db.PriceCatalogs.AsNoTracking() on l.PriceCatalogId equals pc.Id
            join g in _db.PaymentCategoryGroups.AsNoTracking() on pc.PaymentCategoryGroupId equals g.Id
            where l.PaymentDocumentId == id && l.Active && !l.PrePaid
            orderby l.Id
            select new { l.UnitPrice, l.Discount, l.Quantity, l.VatPercent, CategoryName = g.Name }
        ).ToListAsync();

        // Legacy folds same-category rows into ONE receipt line (PaymentDocumentFiscalPrintList
        // .ContainsP/AddPrice): the first row keeps its raw price, discount and VAT; every
        // subsequent same-name row adds its price PRE-ROUNDED through FicalRound. Folding keys
        // on the category NAME, so per-company duplicate category ids merge — as in legacy.
        var folded = new List<FiscalFold>();
        foreach (var l in lines)
        {
            var raw = (double)l.UnitPrice * l.Quantity;
            var f = folded.Find(x => string.Equals(x.Name, l.CategoryName, StringComparison.Ordinal));
            if (f == null)
                folded.Add(new FiscalFold { Name = l.CategoryName, Accum = raw, Discount = l.Discount, VatPercent = l.VatPercent });
            else
                f.Accum += FicalRound(raw);
        }

        // Zero gate — legacy GetTotalAmmount over the folded rows (FicalRound'ed price,
        // first-row discount, no final whole-denar rounding).
        var total = folded.Sum(f => { var p = FicalRound(f.Accum); return p - p * f.Discount / 100; });
        if (folded.Count == 0 || total == 0)
            return Ok(new FiscalFileDto(false, "Сметката нема износ за фискализација.", "", "", doc.FiscalPrintedAt));

        var sb = new System.Text.StringBuilder();
        // header: open receipt (storno opens a storno receipt)
        sb.Append(doc.Stornoed ? " U1,0000,1" : " 01,0000,1").Append("\r\n");

        for (var i = 0; i < folded.Count; i++)
        {
            var f = folded[i];
            // VAT class byte (CP1251 А/Б/В): 18% → 192, 5% → 193, 0% → 194.
            // Legacy silently truncated the receipt on an unknown rate; we refuse loudly.
            var vatByte = f.VatPercent switch
            {
                18 => (byte)192,
                5  => (byte)193,
                0  => (byte)194,
                _  => (byte)0,
            };
            if (vatByte == 0)
                return BadRequest(new { error = $"Ставка со непозната ДДВ стапка ({f.VatPercent}%) — не може да се фискализира." });

            // Legacy: cenaSoPopust = Math.Round(P − P·d/100, 0) where P = FicalRound(folded sum)
            // and d = the first row's discount — banker's rounding, matching VB Math.Round.
            var p = FicalRound(f.Accum);
            var price = Math.Round(p - p * f.Discount / 100, 0);

            sb.Append(i % 2 == 0 ? "'1" : " 1")                       // legacy alternates the marker
              .Append(Left(ToLat(f.Name), 24))
              .Append('\t')
              .Append((char)vatByte)
              .Append(FormatFiscal(price))
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

    /// <summary>Legacy bill-grid service name: "{CategoryName} {ItemName} {PrametarName}"
    /// with the {0}/{1} range placeholders substituted (66.1 renders as "66,1" — comma
    /// decimals, like the legacy MK-culture grid). PriceCatalog.Name was imported as
    /// "ItemName — PrametarName", so it splits on the em-dash separator.</summary>
    internal static string? ComposeServiceName(string? groupName, string? catalogName, double? from, double? to)
    {
        static string num(double v) =>
            v % 1 == 0 ? ((long)v).ToString() : v.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture).Replace('.', ',');
        string fill(string s)
        {
            if (s.Contains("{0}")) s = s.Replace("{0}", from.HasValue ? num(from.Value) : "");
            if (s.Contains("{1}")) s = s.Replace("{1}", to.HasValue ? num(to.Value) : "");
            return s;
        }
        var idx = (catalogName ?? "").IndexOf(" — ", StringComparison.Ordinal);
        var item = idx >= 0 ? catalogName![..idx] : (catalogName ?? "");
        var param = idx >= 0 ? catalogName![(idx + 3)..] : "";
        if (item == "(no item)") item = "";
        var parts = new[] { groupName?.Trim(), item.Trim(), fill(param).Trim() }
            .Where(s => !string.IsNullOrEmpty(s));
        var composed = string.Join(' ', parts);
        return composed.Length > 0 ? composed : catalogName;
    }

    public record LinePriceDto(decimal UnitPrice);

    public record PaidDto(bool Paid);

    /// <summary>Operator toggle платено/неплатено on a bill (legacy Payed flag).</summary>
    [HttpPut("{id:long}/paid")]
    public async Task<IActionResult> SetPaid(long id, [FromBody] PaidDto req)
    {
        var doc = await _db.PaymentDocuments.FirstOrDefaultAsync(x => x.Id == id);
        if (doc == null) return NotFound();
        if (doc.Stornoed)
            return BadRequest(new { error = "Сметката е сторнирана — статусот не може да се менува." });
        doc.Paid = req.Paid;
        doc.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---- СМЕТКОПОТВРДА (legacy A4-landscape receipt, two copies side by side) ----

    public record ReceiptLineDto(string? Name, double BezDdv, double Popust, double Ddv, double Cena);
    public record ReceiptPrintDto(
        long Id, string DocumentNumber, DateTime IssueDate, string? PaymentTypeName,
        string? OrgName, string? OrgTaxNumber, string? OrgAddress, string? OrgPhone,
        string? ClientName, string? ClientAddress, string? ClientCityName,
        string? VehicleCategoryLabel, string? Plate, string? MakerModel, double? WorkingCapacityCc,
        string? Vin, double? PowerKw, string? EngineNumber, double CarryKg,
        IReadOnlyList<ReceiptLineDto> Lines,
        double TotalBezDdv, double TotalDdv, double Total,
        string? ReferentName, string? Note, bool Paid, bool Stornoed);

    /// <summary>Print bundle for СМЕТКОПОТВРДА — the legacy landscape receipt. Per-line
    /// money mirrors legacy PrintPaymentDocumetnByIdDocumetnInfo: everything runs through
    /// FicalRound; ЦЕНА БЕЗ ДДВ = FicalRound(soPopust/(1+ddv/100)), ДДВ = FicalRound(
    /// soPopust·ddv/(100+ddv)); line names are the payment CATEGORY (as on the fiscal).</summary>
    [HttpGet("{id:long}/receipt-print")]
    public async Task<ActionResult<ReceiptPrintDto>> ReceiptPrint(long id)
    {
        var d = await _db.PaymentDocuments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (d == null) return NotFound();

        var type = await _db.PaymentTypes.AsNoTracking()
            .Where(t => t.Id == d.PaymentTypeId).Select(t => t.Name).FirstOrDefaultAsync();
        var org = await _db.TechnicalExamOrganizations.AsNoTracking()
            .Where(o => o.Id == d.OrganizationId)
            .Select(o => new { o.Name, o.TaxNumber, o.Address, o.Phone })
            .FirstOrDefaultAsync();

        // Client + vehicle through the relation.
        string? clientName = null, clientAddress = null, clientCity = null;
        string? catLabel = null, plate = null, makerModel = null, vin = null, engineNo = null;
        double? capacity = null, power = null; double carry = 0;
        var rel = await _db.ClientVehicleRelations.AsNoTracking()
            .Where(r => r.Id == d.CustomerVehicleRelationId)
            .Select(r => new { r.ClientId, r.VehicleId })
            .FirstOrDefaultAsync();
        if (rel != null)
        {
            var c = await _db.Clients.AsNoTracking().Where(x => x.Id == rel.ClientId)
                .Select(x => new { x.FirstName, x.MiddleName, x.LastName, x.Address, x.CityId })
                .FirstOrDefaultAsync();
            if (c != null)
            {
                var n = string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
                clientName = n.Length > 0 ? n : null;
                clientAddress = c.Address;
                if (c.CityId.HasValue)
                    clientCity = await _db.Cities.AsNoTracking().Where(x => x.Id == c.CityId.Value)
                        .Select(x => x.Name).FirstOrDefaultAsync();
            }
            if (rel.VehicleId.HasValue)
            {
                var v = await _db.Vehicles.AsNoTracking().Where(x => x.Id == rel.VehicleId.Value)
                    .Select(x => new
                    {
                        x.Plate, x.Vin, x.EngineNumber, x.EnginePowerKw, x.EngineWorkingCapacityCc,
                        x.MaxAllowedWeightKg, x.CategoryId, x.ModelId,
                    })
                    .FirstOrDefaultAsync();
                if (v != null)
                {
                    plate = v.Plate; vin = v.Vin; engineNo = v.EngineNumber;
                    power = v.EnginePowerKw; capacity = v.EngineWorkingCapacityCc;
                    carry = v.MaxAllowedWeightKg ?? 0;
                    if (v.CategoryId.HasValue)
                    {
                        var vc = await _db.VehicleCategories.AsNoTracking()
                            .Where(x => x.Id == v.CategoryId.Value)
                            .Select(x => new { x.Code, x.Name }).FirstOrDefaultAsync();
                        if (vc != null)
                            catLabel = string.Join(' ', new[] { vc.Code, vc.Name?.ToUpperInvariant() }.Where(s => !string.IsNullOrWhiteSpace(s)));
                    }
                    if (v.ModelId.HasValue)
                    {
                        var m = await _db.VehicleModels.AsNoTracking().Where(x => x.Id == v.ModelId.Value)
                            .Select(x => new { x.Name, x.MakerId }).FirstOrDefaultAsync();
                        if (m != null)
                        {
                            var maker = await _db.VehicleMakers.AsNoTracking()
                                .Where(x => x.Id == m.MakerId).Select(x => x.Name).FirstOrDefaultAsync();
                            makerModel = string.Join(", ", new[] { maker, m.Name }.Where(s => !string.IsNullOrWhiteSpace(s)));
                        }
                    }
                }
            }
        }

        // Lines: category name + FicalRound money (legacy PrintPaymentDocumetnByIdDocumetnInfo).
        var linesRaw = await (
            from l in _db.PaymentDocumentLines.AsNoTracking()
            join pc in _db.PriceCatalogs.AsNoTracking() on l.PriceCatalogId equals pc.Id
            where l.PaymentDocumentId == d.Id && l.Active
            orderby l.Id
            select new { l.UnitPrice, l.Quantity, l.Discount, l.VatPercent, pc.Name, pc.PaymentCategoryGroupId }
        ).ToListAsync();
        var rGroupIds = linesRaw.Where(x => x.PaymentCategoryGroupId.HasValue)
            .Select(x => x.PaymentCategoryGroupId!.Value).Distinct().ToList();
        var rGroups = await _db.PaymentCategoryGroups.AsNoTracking()
            .Where(g => rGroupIds.Contains(g.Id)).ToDictionaryAsync(g => g.Id, g => g.Name);

        var lines = new List<ReceiptLineDto>();
        foreach (var l in linesRaw)
        {
            var soPopust = (double)l.UnitPrice * l.Quantity * (1 - l.Discount / 100);
            var cena = FicalRound(soPopust);
            var bezDdv = FicalRound(soPopust / (1 + l.VatPercent / 100));
            var ddv = FicalRound(soPopust * l.VatPercent / (100 + l.VatPercent));
            var popust = FicalRound((double)l.UnitPrice * l.Quantity * l.Discount / 100);
            var name = l.PaymentCategoryGroupId.HasValue
                ? rGroups.GetValueOrDefault(l.PaymentCategoryGroupId.Value)?.Trim() ?? l.Name
                : l.Name;
            lines.Add(new ReceiptLineDto(name, bezDdv, popust, ddv, cena));
        }

        string? referent = null;
        if (!string.IsNullOrEmpty(d.CreatedByUserId))
            referent = await _db.Users.AsNoTracking()
                .Where(u => u.Id == d.CreatedByUserId).Select(u => u.FullName).FirstOrDefaultAsync();

        return Ok(new ReceiptPrintDto(
            d.Id, d.DocumentNumber, d.IssueDate, type?.Trim(),
            org?.Name, org?.TaxNumber, org?.Address, org?.Phone,
            clientName, clientAddress, clientCity,
            catLabel, plate, makerModel, capacity,
            vin, power, engineNo, carry,
            lines,
            lines.Sum(x => x.BezDdv), lines.Sum(x => x.Ddv), lines.Sum(x => x.Cena),
            referent, d.Note, d.Paid, d.Stornoed));
    }

    /// <summary>Operator price override on a bill line — legacy allowed editing the price
    /// cell directly in the bill grid. Touches ONLY this bill: the originating debt keeps
    /// its historic price, and the fiscal file (composed at print time) picks up the new
    /// value automatically.</summary>
    [HttpPut("{id:long}/lines/{lineId:long}/price")]
    public async Task<IActionResult> UpdateLinePrice(long id, long lineId, [FromBody] LinePriceDto req)
    {
        if (req.UnitPrice < 0)
            return BadRequest(new { error = "Цената не може да биде негативна." });
        var doc = await _db.PaymentDocuments.FirstOrDefaultAsync(x => x.Id == id);
        if (doc == null) return NotFound();
        if (doc.Stornoed)
            return BadRequest(new { error = "Сметката е сторнирана — цената не може да се менува." });
        var line = await _db.PaymentDocumentLines.FirstOrDefaultAsync(l => l.Id == lineId && l.PaymentDocumentId == id);
        if (line == null) return NotFound();
        if (!line.Active)
            return BadRequest(new { error = "Ставката е избришана — цената не може да се менува." });

        line.UnitPrice = req.UnitPrice;
        doc.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>One folded fiscal-receipt line — legacy PaymentDocumentFiscalPrintInfo:
    /// price accumulates per category name; discount and VAT stay from the FIRST row.</summary>
    private sealed class FiscalFold
    {
        public string Name = string.Empty;
        public double Accum;
        public double Discount;
        public double VatPercent;
    }

    /// <summary>Legacy VTE.Library RoundHelper.FicalRound, ported verbatim: the fraction
    /// (pre-rounded to 2 decimals, banker's) goes DOWN at ≤ 0.49 and UP above — unlike
    /// banker's whole-denar rounding, x.50 always rounds up. Negative fractions fell
    /// through the legacy Select Case and returned 0; prices here are never negative.</summary>
    private static double FicalRound(double value)
    {
        var frac = Math.Round(value - Math.Truncate(value), 2);
        if (frac >= 0 && frac <= 0.49) return Math.Truncate(value);
        if (frac > 0.49) return Math.Truncate(value) + 1;
        return 0;
    }

    /// <summary>Legacy VB FormatNumber(x, 2, IncludeLeadingDigit:=False, GroupDigits:=False)
    /// with the "," → "." replace: two decimals, no thousands separator, and values below 1
    /// print WITHOUT the leading zero (0 → ".00").</summary>
    private static string FormatFiscal(double value)
    {
        var s = value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        return s.StartsWith("0.", StringComparison.Ordinal) ? s[1..] : s;
    }

    /// <summary>Macedonian Cyrillic → Latin transliteration, ported from the legacy
    /// KondnaTastaturaModule.ToLat (incl. digraphs). Fixes the legacy off-by-one that
    /// left capital "А" untransliterated. Unicode dashes (— –) fold to a plain "-", and
    /// any other non-printable-ASCII char becomes "?" — the fiscal file is emitted by a
    /// raw (byte)char cast, so a stray char > 126 would otherwise truncate to a garbage
    /// (often control) byte in the Accent receipt (legacy relied on CP1251 here).</summary>
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
                '—' or '–' or '―' or '‒' => "-",   // Unicode dashes → ASCII hyphen (legacy printed these via CP1251)
                _ => ch is >= ' ' and <= '~' ? ch.ToString() : "?",  // keep printable ASCII; never emit a stray > 126 byte
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
