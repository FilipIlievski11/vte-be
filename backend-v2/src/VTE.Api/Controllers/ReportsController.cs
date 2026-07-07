using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

/// <summary>
/// Monthly financial reports. First report: „Детален месечен извештај по ставки од
/// регистрација на возила" — the public-roads fee (Надоместок за јавни патишта,
/// legacy PaymentCategories id 1 → PriceCatalog.PaymentCategoryGroupId) collected per
/// bill in a month. Semantics verified 1:1 against the legacy SP
/// <c>ReportByCategoryForPayment</c> + the station's real May-2026 XLS export
/// (683 rows / 1,577,400.00 ден — exact match on v2 data):
///   • one row per PaymentDocument with at least one category line;
///   • amount = SUM of that document's matching lines (Active, not pre-paid);
///   • document must be Active, not stornoed (flag or the two legacy Note markers),
///     payment type not 'пп' (вирман pro-forma), IssueDate (legacy DatePay) in month.
/// </summary>
[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    public ReportsController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record ReportRow(long DocumentId, DateTime DatePay, string Payer,
                            string? VehicleCategory, string? Plate, decimal Amount);
    public record ReportDto(int Year, int Month, string From, string To,
                            string CompanyName, decimal Total, IReadOnlyList<ReportRow> Rows);

    private async Task<ReportDto?> BuildAsync(int year, int month, int categoryGroupId, CancellationToken ct)
    {
        if (year is < 2000 or > 2100 || month is < 1 or > 12) return null;
        var fromDate = new DateTime(year, month, 1);
        var toDate = fromDate.AddMonths(1);

        // Line-level pull (grouping per document happens locally so we can also
        // resolve payer/vehicle labels in one pass).
        var lines = await (
            from d in _db.PaymentDocuments.AsNoTracking()
            join t in _db.PaymentTypes.AsNoTracking() on d.PaymentTypeId equals t.Id
            join l in _db.PaymentDocumentLines.AsNoTracking() on d.Id equals l.PaymentDocumentId
            join pc in _db.PriceCatalogs.AsNoTracking() on l.PriceCatalogId equals pc.Id
            where d.Active && !d.Stornoed
                  && d.IssueDate >= fromDate && d.IssueDate < toDate
                  && (d.Note == null || (!d.Note.StartsWith("Автоматски генерирана")
                                         && !d.Note.StartsWith("Сторнирана во сметка")))
                  && (t.Prefix == null || t.Prefix.Trim() != "пп")
                  && l.Active && !l.PrePaid
                  && pc.PaymentCategoryGroupId == categoryGroupId
            select new
            {
                d.Id,
                d.IssueDate,
                d.CustomerVehicleRelationId,
                LineTotal = l.UnitPrice * l.Quantity,
            }).ToListAsync(ct);

        // Batch-resolve relation → client + vehicle (+ payment category name).
        var relIds = lines.Select(x => x.CustomerVehicleRelationId).Distinct().ToList();
        var rels = await _db.ClientVehicleRelations.AsNoTracking()
            .Where(r => relIds.Contains(r.Id))
            .Select(r => new { r.Id, r.ClientId, r.VehicleId })
            .ToDictionaryAsync(r => r.Id, ct);
        var clientIds = rels.Values.Select(r => r.ClientId).Distinct().ToList();
        var clients = await _db.Clients.AsNoTracking()
            .Where(c => clientIds.Contains(c.Id))
            .Select(c => new { c.Id, c.FirstName, c.LastName })
            .ToDictionaryAsync(c => c.Id, ct);
        var vehIds = rels.Values.Where(r => r.VehicleId.HasValue).Select(r => r.VehicleId!.Value).Distinct().ToList();
        var vehicles = await _db.Vehicles.AsNoTracking()
            .Where(v => vehIds.Contains(v.Id))
            .Select(v => new { v.Id, v.Plate, v.PaymentCategoryId })
            .ToDictionaryAsync(v => v.Id, ct);
        var catNames = await _db.VehiclePaymentCategories.AsNoTracking()
            .ToDictionaryAsync(c => c.Id, c => c.Name, ct);

        var rows = lines
            .GroupBy(x => new { x.Id, x.IssueDate, x.CustomerVehicleRelationId })
            .Select(g =>
            {
                string payer = "—"; string? plate = null, vehCat = null;
                if (rels.TryGetValue(g.Key.CustomerVehicleRelationId, out var rel))
                {
                    if (clients.TryGetValue(rel.ClientId, out var c))
                        payer = string.Join(' ', new[] { c.LastName, c.FirstName }
                            .Where(s => !string.IsNullOrWhiteSpace(s))).Trim();
                    if (rel.VehicleId.HasValue && vehicles.TryGetValue(rel.VehicleId.Value, out var v))
                    {
                        plate = v.Plate;
                        if (v.PaymentCategoryId.HasValue)
                            vehCat = catNames.GetValueOrDefault(v.PaymentCategoryId.Value);
                    }
                }
                return new ReportRow(g.Key.Id, g.Key.IssueDate, payer, vehCat, plate, g.Sum(x => x.LineTotal));
            })
            .OrderBy(r => r.DatePay).ThenBy(r => r.DocumentId)
            .ToList();

        var companyName = await _db.Companies.AsNoTracking()
            .Where(c => c.Id == (_tenant.CompanyId ?? 4))
            .Select(c => c.Name)
            .FirstOrDefaultAsync(ct) ?? "";

        return new ReportDto(year, month,
            fromDate.ToString("dd.MM.yyyy"), toDate.AddDays(-1).ToString("dd.MM.yyyy"),
            companyName, rows.Sum(r => r.Amount), rows);
    }

    /// <summary>Report data as JSON — backs the Извештаи screen.</summary>
    [HttpGet("public-roads")]
    public async Task<ActionResult<ReportDto>> PublicRoads(
        [FromQuery] int year, [FromQuery] int month,
        [FromQuery] int categoryGroupId = 1, CancellationToken ct = default)
    {
        var dto = await BuildAsync(year, month, categoryGroupId, ct);
        return dto == null ? BadRequest(new { error = "Невалиден период." }) : Ok(dto);
    }

    /// <summary>The same report as a downloadable .xlsx, replicating the legacy layout
    /// (title block, columns, ВКУПНО row, М.П. + signature footer).</summary>
    [HttpGet("public-roads/xlsx")]
    public async Task<IActionResult> PublicRoadsXlsx(
        [FromQuery] int year, [FromQuery] int month,
        [FromQuery] int categoryGroupId = 1, CancellationToken ct = default)
    {
        var dto = await BuildAsync(year, month, categoryGroupId, ct);
        if (dto == null) return BadRequest(new { error = "Невалиден период." });

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Извештај");

        ws.Cell(1, 1).Value = dto.CompanyName;
        ws.Range(1, 1, 1, 6).Merge().Style
            .Font.SetBold().Font.SetFontSize(12)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        ws.Cell(3, 1).Value = "Детален месечен извештај по ставки од регистрација на возила";
        ws.Range(3, 1, 3, 6).Merge().Style
            .Font.SetBold()
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        ws.Cell(4, 1).Value = $"за период од {dto.From} до {dto.To}";
        ws.Range(4, 1, 4, 6).Merge().Style
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        string[] heads = ["Ред. Бр.", "Дата на уплата", "Назив на уплатувач",
                          "Категорија на возило", "Рег. број на возило", "Уплатен износ"];
        const int headRow = 6;
        for (var i = 0; i < heads.Length; i++)
        {
            var c = ws.Cell(headRow, i + 1);
            c.Value = heads[i];
            c.Style.Font.SetBold()
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Fill.SetBackgroundColor(XLColor.FromArgb(0xEF, 0xEF, 0xEF));
        }

        var r = headRow + 1;
        var n = 1;
        foreach (var row in dto.Rows)
        {
            ws.Cell(r, 1).Value = n++;
            ws.Cell(r, 2).Value = row.DatePay;
            ws.Cell(r, 2).Style.DateFormat.Format = "dd.MM.yyyy";
            ws.Cell(r, 3).Value = row.Payer;
            ws.Cell(r, 4).Value = row.VehicleCategory ?? "";
            ws.Cell(r, 5).Value = row.Plate ?? "";
            ws.Cell(r, 6).Value = row.Amount;
            ws.Cell(r, 6).Style.NumberFormat.Format = "#,##0.00 \"ден\"";
            r++;
        }

        ws.Cell(r, 5).Value = "ВКУПНО :";
        ws.Cell(r, 5).Style.Font.SetBold();
        ws.Cell(r, 6).Value = dto.Total;
        ws.Cell(r, 6).Style.Font.SetBold().NumberFormat.SetFormat("#,##0.00 \"ден\"");

        ws.Cell(r + 3, 3).Value = "М.П.";
        ws.Cell(r + 4, 5).Value = "____________________________";
        ws.Cell(r + 5, 5).Value = "потпис на одговорно лице";

        ws.Column(1).Width = 8;
        ws.Column(2).Width = 14;
        ws.Column(3).Width = 34;
        ws.Column(4).Width = 30;
        ws.Column(5).Width = 18;
        ws.Column(6).Width = 16;

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        var mkMonths = new[] { "", "ЈАНУАРИ", "ФЕВРУАРИ", "МАРТ", "АПРИЛ", "МАЈ", "ЈУНИ",
                               "ЈУЛИ", "АВГУСТ", "СЕПТЕМВРИ", "ОКТОМВРИ", "НОЕМВРИ", "ДЕКЕМВРИ" };
        return File(ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Извештај {mkMonths[dto.Month]} {dto.Year}.xlsx");
    }

    // ========================================================================
    // Дневна распределба: how much of the day's collection must be forwarded to
    // each outside institution (уплатна сметка) vs. how much stays with the
    // station. Same line-filter semantics as the public-roads report (active,
    // not stornoed, payment type not 'пп', line active + not pre-paid, the two
    // Note markers excluded) so the two reports reconcile to the same base.
    //
    // Every billable line → PriceCatalog → PaymentCategoryGroup → CalculationItem
    // (the destination account). Lines whose account IsOwnAccount, plus lines with
    // no linked account (own admin services), stay with the station; the rest are
    // grouped per institution to forward.
    // ========================================================================
    public record InstitutionRow(int CalcId, string Name, string? BankAccount, string? Bank, string? Form,
                                 int LineCount, decimal Total);
    public record DistributionDto(
        string From, string To, string CompanyName,
        IReadOnlyList<InstitutionRow> Institutions,   // to forward
        decimal InstitutionsTotal,
        decimal StationOwnAccount,                     // lines on the station's own account
        decimal StationUnassigned,                     // lines with no linked account (own services)
        decimal StationTotal,                          // = own + unassigned (stays with the station)
        decimal GrandTotal,
        int LineCount);

    [HttpGet("institution-distribution")]
    public async Task<ActionResult<DistributionDto>> InstitutionDistribution(
        [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct = default)
    {
        var fromDate = from.Date;
        var toDate = to.Date.AddDays(1);      // inclusive of the 'to' day
        if (toDate <= fromDate) return BadRequest(new { error = "Невалиден период." });

        // Aggregate per destination account in the DB. calcId null = no linked
        // account; IsOwnAccount separates the station's own account from institutions.
        var groups = await (
            from d in _db.PaymentDocuments.AsNoTracking()
            join t in _db.PaymentTypes.AsNoTracking() on d.PaymentTypeId equals t.Id
            join l in _db.PaymentDocumentLines.AsNoTracking() on d.Id equals l.PaymentDocumentId
            join pc in _db.PriceCatalogs.AsNoTracking() on l.PriceCatalogId equals pc.Id
            join g in _db.PaymentCategoryGroups.AsNoTracking() on pc.PaymentCategoryGroupId equals g.Id into gj
            from g in gj.DefaultIfEmpty()
            join ci in _db.CalculationItems.AsNoTracking() on g.CalculationItemId equals ci.Id into cij
            from ci in cij.DefaultIfEmpty()
            where d.Active && !d.Stornoed
                  && d.IssueDate >= fromDate && d.IssueDate < toDate
                  && (d.Note == null || (!d.Note.StartsWith("Автоматски генерирана")
                                         && !d.Note.StartsWith("Сторнирана во сметка")))
                  && (t.Prefix == null || t.Prefix.Trim() != "пп")
                  && l.Active && !l.PrePaid
            group new { l.UnitPrice, l.Quantity } by new
            {
                CalcId = (int?)(ci != null ? ci.Id : (int?)null),
                Name = ci != null ? ci.Name : null,
                ci!.BankAccount, ci.Bank, ci.Form, IsOwn = ci != null && ci.IsOwnAccount,
            } into grp
            select new
            {
                grp.Key.CalcId, grp.Key.Name, grp.Key.BankAccount, grp.Key.Bank, grp.Key.Form, grp.Key.IsOwn,
                LineCount = grp.Count(),
                Total = grp.Sum(x => x.UnitPrice * x.Quantity),
            }).ToListAsync(ct);

        var institutions = groups
            .Where(x => x.CalcId != null && !x.IsOwn)
            .OrderByDescending(x => x.Total)
            .Select(x => new InstitutionRow(x.CalcId!.Value, x.Name ?? "", x.BankAccount, x.Bank, x.Form, x.LineCount, x.Total))
            .ToList();

        var ownAccount = groups.Where(x => x.CalcId != null && x.IsOwn).Sum(x => x.Total);
        var unassigned = groups.Where(x => x.CalcId == null).Sum(x => x.Total);
        var institutionsTotal = institutions.Sum(x => x.Total);
        var grand = groups.Sum(x => x.Total);
        var lineCount = groups.Sum(x => x.LineCount);

        var companyName = await _db.Companies.AsNoTracking()
            .Where(c => c.Id == (_tenant.CompanyId ?? 4))
            .Select(c => c.Name).FirstOrDefaultAsync(ct) ?? "";

        return Ok(new DistributionDto(
            fromDate.ToString("dd.MM.yyyy"), toDate.AddDays(-1).ToString("dd.MM.yyyy"), companyName,
            institutions, institutionsTotal, ownAccount, unassigned, ownAccount + unassigned, grand, lineCount));
    }

    [HttpGet("institution-distribution/xlsx")]
    public async Task<IActionResult> InstitutionDistributionXlsx(
        [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct = default)
    {
        var result = await InstitutionDistribution(from, to, ct);
        if (result.Result is BadRequestObjectResult bad) return bad;
        var dto = (result.Value ?? ((OkObjectResult)result.Result!).Value) as DistributionDto;
        if (dto is null) return BadRequest(new { error = "Невалиден период." });

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Распределба");

        ws.Cell(1, 1).Value = dto.CompanyName;
        ws.Range(1, 1, 1, 4).Merge().Style.Font.SetBold().Font.SetFontSize(12)
            .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        ws.Cell(3, 1).Value = "Дневна распределба на наплата по институции";
        ws.Range(3, 1, 3, 4).Merge().Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        ws.Cell(4, 1).Value = dto.From == dto.To ? $"за {dto.From}" : $"за период од {dto.From} до {dto.To}";
        ws.Range(4, 1, 4, 4).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        ws.Cell(6, 1).Value = "ТРЕБА ДА СЕ УПЛАТИ НА ИНСТИТУЦИИ";
        ws.Range(6, 1, 6, 4).Merge().Style.Font.SetBold();

        string[] heads = ["Институција", "Уплатна сметка", "Образец", "Износ"];
        const int headRow = 7;
        for (var i = 0; i < heads.Length; i++)
            ws.Cell(headRow, i + 1).Value = heads[i];
        ws.Range(headRow, 1, headRow, 4).Style.Font.SetBold()
            .Fill.SetBackgroundColor(XLColor.FromArgb(0xEF, 0xEF, 0xEF))
            .Border.SetOutsideBorder(XLBorderStyleValues.Thin);

        var r = headRow + 1;
        foreach (var inst in dto.Institutions)
        {
            ws.Cell(r, 1).Value = inst.Name;
            ws.Cell(r, 2).Value = inst.BankAccount ?? "";
            ws.Cell(r, 2).Style.NumberFormat.Format = "@";   // keep account number as text
            ws.Cell(r, 3).Value = inst.Form ?? "";
            ws.Cell(r, 4).Value = inst.Total;
            ws.Cell(r, 4).Style.NumberFormat.Format = "#,##0.00 \"ден\"";
            r++;
        }
        ws.Cell(r, 3).Value = "Вкупно за уплата:";
        ws.Cell(r, 3).Style.Font.SetBold().Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        ws.Cell(r, 4).Value = dto.InstitutionsTotal;
        ws.Cell(r, 4).Style.Font.SetBold().NumberFormat.SetFormat("#,##0.00 \"ден\"");

        r += 2;
        ws.Cell(r, 1).Value = "ОСТАНУВА ЗА СТАНИЦАТА";
        ws.Cell(r, 1).Style.Font.SetBold();
        ws.Cell(r, 4).Value = dto.StationTotal;
        ws.Cell(r, 4).Style.Font.SetBold().NumberFormat.SetFormat("#,##0.00 \"ден\"");
        ws.Cell(r + 1, 1).Value = "  – сопствена сметка";
        ws.Cell(r + 1, 4).Value = dto.StationOwnAccount;
        ws.Cell(r + 1, 4).Style.NumberFormat.Format = "#,##0.00 \"ден\"";
        ws.Cell(r + 2, 1).Value = "  – административни / останати услуги";
        ws.Cell(r + 2, 4).Value = dto.StationUnassigned;
        ws.Cell(r + 2, 4).Style.NumberFormat.Format = "#,##0.00 \"ден\"";

        r += 4;
        ws.Cell(r, 1).Value = "ВКУПНО НАПЛАТЕНО";
        ws.Cell(r, 1).Style.Font.SetBold();
        ws.Cell(r, 4).Value = dto.GrandTotal;
        ws.Cell(r, 4).Style.Font.SetBold().NumberFormat.SetFormat("#,##0.00 \"ден\"");
        ws.Range(r, 1, r, 4).Style.Border.SetTopBorder(XLBorderStyleValues.Medium);

        ws.Column(1).Width = 44;
        ws.Column(2).Width = 20;
        ws.Column(3).Width = 10;
        ws.Column(4).Width = 18;

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        var fname = dto.From == dto.To ? $"Распределба {dto.From}.xlsx" : $"Распределба {dto.From}-{dto.To}.xlsx";
        return File(ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fname);
    }
}
