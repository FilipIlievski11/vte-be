using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VTE.Domain.Documents;
using VTE.Domain.Payments;

namespace VTE.Api.Pdf;

// Three core regulatory PDF outputs. Implementations are deliberately compact —
// the output matches the legacy structure (header / body / footer) without trying
// to pixel-match the Macedonian state forms (R-5 in audit). For pixel-accurate
// regulatory output, sample print-outs from real stations are required.
public static class PdfReports
{
    static PdfReports()
    {
        // Community licence — legitimate for in-house & non-commercial use up to a revenue cap.
        // Commercial deployments must purchase a QuestPDF licence and replace this line.
        QuestPDF.Settings.License = LicenseType.Community;
    }

    // ===== Technical Exam Report — pass/fail certificate =====
    public static byte[] ExamReportCertificate(TechnicalExamReport r, string stationName, string? customerName)
    {
        return Document.Create(c =>
        {
            c.Page(p =>
            {
                p.Size(PageSizes.A4);
                p.Margin(20);
                p.DefaultTextStyle(t => t.FontSize(10));

                p.Header().Element(e =>
                {
                    e.Column(col =>
                    {
                        col.Item().Text(stationName).Bold().FontSize(14);
                        col.Item().Text("Извештај за технички преглед — Technical Exam Report").FontSize(10);
                        col.Item().PaddingBottom(6).LineHorizontal(0.5f);
                    });
                });

                p.Content().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c2 =>
                        {
                            c2.Item().Text(t => { t.Span("Клиент / Customer: ").SemiBold(); t.Span(customerName ?? "—"); });
                            c2.Item().Text(t => { t.Span("Регистрација / Reg.: ").SemiBold(); t.Span(r.RegNumber ?? "—"); });
                            c2.Item().Text(t => { t.Span("Извршен / Made on: ").SemiBold(); t.Span(r.MadeDate.ToString("yyyy-MM-dd")); });
                            c2.Item().Text(t => { t.Span("Важи до / Valid till: ").SemiBold(); t.Span(r.ValidTillDate.ToString("yyyy-MM-dd")); });
                        });
                        row.ConstantItem(140).Background(r.VehicleIsRight ? Colors.Green.Lighten4 : Colors.Red.Lighten4)
                           .AlignCenter().AlignMiddle().Padding(8)
                           .Text(r.VehicleIsRight ? "ИСПРАВНО / PASS" : "НЕИСПРАВНО / FAIL")
                           .Bold().FontSize(14);
                    });

                    col.Item().PaddingTop(10).Text("Кочни сили / Brake forces (kN/daN — confirm regulatory unit)").Bold();
                    col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c2 => { c2.RelativeColumn(); c2.RelativeColumn(); c2.RelativeColumn(); c2.RelativeColumn(); c2.RelativeColumn(); c2.RelativeColumn(); });
                        t.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Оска").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Лево").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Десно").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Гj").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Разлика").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Коеф.").SemiBold();
                        });
                        void AddRow(string label, decimal? l, decimal? r2, decimal? gj, decimal? diff, decimal? coef)
                        {
                            t.Cell().Padding(3).Text(label);
                            t.Cell().Padding(3).Text(l?.ToString("0.##") ?? "—");
                            t.Cell().Padding(3).Text(r2?.ToString("0.##") ?? "—");
                            t.Cell().Padding(3).Text(gj?.ToString("0.##") ?? "—");
                            t.Cell().Padding(3).Text(diff?.ToString("0.##") ?? "—");
                            t.Cell().Padding(3).Text(coef?.ToString("0.##") ?? "—");
                        }
                        AddRow("Оска 1",    r.Axis1Left, r.Axis1Right, r.Axis1Gj, r.Axis1LeftRightDiff, r.Axis1Coefficient);
                        AddRow("Оска 2",    r.Axis2Left, r.Axis2Right, r.Axis2Gj, r.Axis2LeftRightDiff, r.Axis2Coefficient);
                        AddRow("Оска 3",    r.Axis3Left, r.Axis3Right, r.Axis3Gj, r.Axis3LeftRightDiff, r.Axis3Coefficient);
                        AddRow("Оска 4",    r.Axis4Left, r.Axis4Right, r.Axis4Gj, r.Axis4LeftRightDiff, r.Axis4Coefficient);
                        AddRow("Паркинг",   r.AxisParkingLeft, r.AxisParkingRight, r.AxisParkingGj, r.AxisParkingLeftRightDiff, r.AxisParkingCoefficient);
                    });

                    col.Item().PaddingTop(10).Text("Емисии / Emissions").Bold();
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text(t => { t.Span("CO: ").SemiBold(); t.Span(r.CO?.ToString("0.000") ?? "—"); });
                        row.RelativeItem().Text(t => { t.Span("Lambda: ").SemiBold(); t.Span(r.Lambda?.ToString("0.000") ?? "—"); });
                        row.RelativeItem().Text(t => { t.Span("Pinpoints: ").SemiBold(); t.Span(r.Pinpoints?.ToString("0.000") ?? "—"); });
                        row.RelativeItem().Text(t => { t.Span("Noise: ").SemiBold(); t.Span(r.Noise?.ToString("0.0") ?? "—"); });
                    });

                    if (!string.IsNullOrWhiteSpace(r.Note))
                    {
                        col.Item().PaddingTop(10).Text("Забелешка / Note").Bold();
                        col.Item().Text(r.Note);
                    }
                    if (!string.IsNullOrWhiteSpace(r.DriversWarning))
                    {
                        col.Item().PaddingTop(6).Text("Предупредување / Warning").Bold();
                        col.Item().Text(r.DriversWarning);
                    }
                });

                p.Footer().AlignCenter().Text(t =>
                {
                    t.Span("Страна ");
                    t.CurrentPageNumber();
                    t.Span(" / ");
                    t.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    // ===== Faktura (invoice) =====
    public static byte[] Invoice(PaymentDocument d, string stationName, string? customerName)
    {
        var totalGross = d.Details.Sum(x => x.Price * (1 - x.DiscountPercent / 100m));
        var afterDocDiscount = totalGross * (1 - d.DiscountPercent / 100m);
        var totalVat = d.Details.Sum(x => x.Price * (1 - x.DiscountPercent / 100m) * (x.DDVRate / 100m));

        return Document.Create(c =>
        {
            c.Page(p =>
            {
                p.Size(PageSizes.A4);
                p.Margin(20);
                p.DefaultTextStyle(t => t.FontSize(10));

                p.Header().Column(col =>
                {
                    col.Item().Text(stationName).Bold().FontSize(14);
                    col.Item().Text($"Фактура / Invoice бр. {d.DocumentNumber}").FontSize(12).Bold();
                    col.Item().PaddingBottom(6).LineHorizontal(0.5f);
                });

                p.Content().Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text(t => { t.Span("Клиент / Customer: ").SemiBold(); t.Span(customerName ?? "—"); });
                        r.ConstantItem(160).Column(c2 =>
                        {
                            c2.Item().Text($"Датум: {d.DatePay:yyyy-MM-dd}");
                            c2.Item().Text($"Доспева: {d.DateRequired:yyyy-MM-dd}");
                            if (d.Storno) c2.Item().Text("СТОРНИРАНО / VOIDED").Bold().FontColor(Colors.Red.Darken2);
                        });
                    });

                    col.Item().PaddingTop(10).Table(t =>
                    {
                        t.ColumnsDefinition(c2 => { c2.RelativeColumn(4); c2.RelativeColumn(); c2.RelativeColumn(); c2.RelativeColumn(); c2.RelativeColumn(); });
                        t.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Опис").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).AlignRight().Text("Цена").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).AlignRight().Text("Поп. %").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).AlignRight().Text("ДДВ %").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).AlignRight().Text("Износ").SemiBold();
                        });
                        foreach (var line in d.Details)
                        {
                            var lineGross = line.Price * (1 - line.DiscountPercent / 100m);
                            t.Cell().Padding(3).Text(line.Note ?? $"PriceCatalog #{line.PriceCatalogId}");
                            t.Cell().Padding(3).AlignRight().Text(line.Price.ToString("0.00"));
                            t.Cell().Padding(3).AlignRight().Text(line.DiscountPercent.ToString("0.##"));
                            t.Cell().Padding(3).AlignRight().Text(line.DDVRate.ToString("0.##"));
                            t.Cell().Padding(3).AlignRight().Text(lineGross.ToString("0.00"));
                        }
                    });

                    col.Item().PaddingTop(10).AlignRight().Column(c2 =>
                    {
                        c2.Item().Text($"Збир / Subtotal: {totalGross:0.00}");
                        c2.Item().Text($"Документски попуст / Doc discount: {d.DiscountPercent:0.##} %");
                        c2.Item().Text($"ДДВ / VAT: {totalVat:0.00}");
                        c2.Item().Text($"Вкупно за плаќање / Total due: {afterDocDiscount:0.00}").Bold().FontSize(12);
                    });

                    if (d.Installments.Count > 0)
                    {
                        col.Item().PaddingTop(10).Text("Рати / Installments").Bold();
                        col.Item().Table(t =>
                        {
                            t.ColumnsDefinition(c2 => { c2.RelativeColumn(); c2.RelativeColumn(); c2.RelativeColumn(); c2.RelativeColumn(); });
                            t.Header(h =>
                            {
                                h.Cell().Padding(3).Text("Бр.").SemiBold();
                                h.Cell().Padding(3).AlignRight().Text("Износ").SemiBold();
                                h.Cell().Padding(3).Text("Доспева").SemiBold();
                                h.Cell().Padding(3).Text("Платена").SemiBold();
                            });
                            foreach (var i in d.Installments.OrderBy(x => x.InstallmentNumber))
                            {
                                t.Cell().Padding(3).Text(i.InstallmentNumber.ToString());
                                t.Cell().Padding(3).AlignRight().Text(i.Price.ToString("0.00"));
                                t.Cell().Padding(3).Text(i.DueDate?.ToString("yyyy-MM-dd") ?? "—");
                                t.Cell().Padding(3).Text(i.Payed ? (i.DatePayed?.ToString("yyyy-MM-dd") ?? "Да") : "Не");
                            }
                        });
                    }
                });

                p.Footer().AlignCenter().Text(t => { t.Span("Страна "); t.CurrentPageNumber(); t.Span(" / "); t.TotalPages(); });
            });
        }).GeneratePdf();
    }

    // ===== Сообраќајна дозвола / Traffic Licence =====
    public static byte[] TrafficLicenceCertificate(TrafficLicence l, string stationName, string? customerName, string? regNumber, string? vehicleShell, string? issuingOrg)
    {
        return Document.Create(c =>
        {
            c.Page(p =>
            {
                p.Size(PageSizes.A4); p.Margin(20);
                p.DefaultTextStyle(t => t.FontSize(10));
                p.Header().Column(col =>
                {
                    col.Item().Text(stationName).Bold().FontSize(14);
                    col.Item().Text("Сообраќајна дозвола / Traffic Licence").FontSize(12).Bold();
                    col.Item().PaddingBottom(6).LineHorizontal(0.5f);
                });
                p.Content().Column(col =>
                {
                    col.Item().PaddingTop(4).Text(t => { t.Span("Број / Number: ").SemiBold(); t.Span(l.TrafficLicenceNumber ?? "—").FontSize(12); });
                    col.Item().Text(t => { t.Span("Сопственик / Customer: ").SemiBold(); t.Span(customerName ?? "—"); });
                    col.Item().Text(t => { t.Span("Регистрација / Reg.: ").SemiBold(); t.Span(regNumber ?? "—"); });
                    col.Item().Text(t => { t.Span("VIN / Шасија: ").SemiBold(); t.Span(vehicleShell ?? "—"); });
                    col.Item().PaddingTop(8).Text(t => { t.Span("Датум на издавање / Issued: ").SemiBold(); t.Span(l.MadeDate.ToString("yyyy-MM-dd")); });
                    col.Item().Text(t => { t.Span("Важи до / Valid till: ").SemiBold(); t.Span(l.EndDate.ToString("yyyy-MM-dd")); });
                    col.Item().Text(t => { t.Span("Издавач / Issuer: ").SemiBold(); t.Span(issuingOrg ?? "—"); });
                    if (!string.IsNullOrWhiteSpace(l.Note))
                    {
                        col.Item().PaddingTop(10).Text("Забелешка / Note").Bold();
                        col.Item().Text(l.Note);
                    }
                });
                p.Footer().AlignCenter().Text(t => { t.Span("Страна "); t.CurrentPageNumber(); t.Span(" / "); t.TotalPages(); });
            });
        }).GeneratePdf();
    }

    // ===== Одобрение / Permission =====
    public static byte[] PermissionCertificate(Permission per, string stationName, string? customerName, string? regNumber, string? typeName)
    {
        return Document.Create(c =>
        {
            c.Page(p =>
            {
                p.Size(PageSizes.A4); p.Margin(20);
                p.DefaultTextStyle(t => t.FontSize(10));
                p.Header().Column(col =>
                {
                    col.Item().Text(stationName).Bold().FontSize(14);
                    col.Item().Text("Одобрение / Permission").FontSize(12).Bold();
                    col.Item().PaddingBottom(6).LineHorizontal(0.5f);
                });
                p.Content().Column(col =>
                {
                    col.Item().PaddingTop(4).Text(t => { t.Span("Број / Number: ").SemiBold(); t.Span(per.PermissionNumber ?? "—").FontSize(12); });
                    col.Item().Text(t => { t.Span("Сопственик / Customer: ").SemiBold(); t.Span(customerName ?? "—"); });
                    col.Item().Text(t => { t.Span("Регистрација / Reg.: ").SemiBold(); t.Span(regNumber ?? "—"); });
                    col.Item().Text(t => { t.Span("Тип / Type: ").SemiBold(); t.Span(typeName ?? "—"); });
                    col.Item().PaddingTop(8).Text(t => { t.Span("Издадено / Issued: ").SemiBold(); t.Span(per.MadeDate.ToString("yyyy-MM-dd")); });
                    col.Item().Text(t => { t.Span("Важи до / Valid till: ").SemiBold(); t.Span(per.EndDate?.ToString("yyyy-MM-dd") ?? "—"); });
                    if (!string.IsNullOrWhiteSpace(per.Note))
                    {
                        col.Item().PaddingTop(10).Text("Забелешка / Note").Bold();
                        col.Item().Text(per.Note);
                    }
                });
                p.Footer().AlignCenter().Text(t => { t.Span("Страна "); t.CurrentPageNumber(); t.Span(" / "); t.TotalPages(); });
            });
        }).GeneratePdf();
    }

    // ===== Меѓународна возачка / International Driving Licence =====
    public static byte[] InternationalDrivingLicenceCertificate(InternationalDrivingLicence idl, string stationName, string? customerName, string? issuerName, IReadOnlyList<string> categoryCodes)
    {
        return Document.Create(c =>
        {
            c.Page(p =>
            {
                p.Size(PageSizes.A4); p.Margin(20);
                p.DefaultTextStyle(t => t.FontSize(10));
                p.Header().Column(col =>
                {
                    col.Item().Text(stationName).Bold().FontSize(14);
                    col.Item().Text("Меѓународна возачка дозвола / International Driving Licence").FontSize(12).Bold();
                    col.Item().PaddingBottom(6).LineHorizontal(0.5f);
                });
                p.Content().Column(col =>
                {
                    col.Item().PaddingTop(4).Text(t => { t.Span("Број / Number: ").SemiBold(); t.Span(idl.LicenceNumber).FontSize(12); });
                    col.Item().Text(t => { t.Span("Сопственик / Holder: ").SemiBold(); t.Span(customerName ?? "—"); });
                    col.Item().Text(t => { t.Span("Издавач / Issuer: ").SemiBold(); t.Span(issuerName ?? "—"); });
                    col.Item().PaddingTop(8).Text(t => { t.Span("Издадена на / Issued: ").SemiBold(); t.Span(idl.IssuedDate.ToString("yyyy-MM-dd")); });
                    col.Item().Text(t => { t.Span("Важи до / Valid till: ").SemiBold(); t.Span(idl.ValidTill.ToString("yyyy-MM-dd")); });
                    if (categoryCodes.Count > 0)
                    {
                        col.Item().PaddingTop(10).Text("Важи за категории / Valid for categories").Bold();
                        col.Item().Text(string.Join(", ", categoryCodes));
                    }
                    if (!string.IsNullOrWhiteSpace(idl.Note))
                    {
                        col.Item().PaddingTop(10).Text("Забелешка / Note").Bold();
                        col.Item().Text(idl.Note);
                    }
                });
                p.Footer().AlignCenter().Text(t => { t.Span("Страна "); t.CurrentPageNumber(); t.Span(" / "); t.TotalPages(); });
            });
        }).GeneratePdf();
    }

    // ===== Kasov izvestaj — daily cash report =====
    public static byte[] CashReport(DateOnly day, string stationName, IReadOnlyList<PaymentDocument> docs)
    {
        var cashDocs = docs.Where(d => !d.Storno && d.DatePay == day).ToList();
        var total = cashDocs.Sum(d => d.Details.Sum(x => x.Price * (1 - x.DiscountPercent / 100m)) * (1 - d.DiscountPercent / 100m));

        return Document.Create(c =>
        {
            c.Page(p =>
            {
                p.Size(PageSizes.A4);
                p.Margin(20);
                p.DefaultTextStyle(t => t.FontSize(10));
                p.Header().Column(col =>
                {
                    col.Item().Text(stationName).Bold().FontSize(14);
                    col.Item().Text($"Касов извештај / Cash report — {day:yyyy-MM-dd}").FontSize(12).Bold();
                    col.Item().PaddingBottom(6).LineHorizontal(0.5f);
                });
                p.Content().Column(col =>
                {
                    col.Item().Table(t =>
                    {
                        t.ColumnsDefinition(c2 => { c2.RelativeColumn(); c2.RelativeColumn(2); c2.RelativeColumn(); });
                        t.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Бр. документ").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).Text("Забелешка").SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(3).AlignRight().Text("Износ").SemiBold();
                        });
                        foreach (var d in cashDocs)
                        {
                            var amt = d.Details.Sum(x => x.Price * (1 - x.DiscountPercent / 100m)) * (1 - d.DiscountPercent / 100m);
                            t.Cell().Padding(3).Text(d.DocumentNumber);
                            t.Cell().Padding(3).Text(d.Note ?? string.Empty);
                            t.Cell().Padding(3).AlignRight().Text(amt.ToString("0.00"));
                        }
                    });
                    col.Item().PaddingTop(10).AlignRight()
                        .Text($"Вкупно / Total: {total:0.00}").Bold().FontSize(12);
                    col.Item().PaddingTop(20).Text($"Број на документи / Doc count: {cashDocs.Count}");
                });
                p.Footer().AlignCenter().Text(t => { t.Span("Страна "); t.CurrentPageNumber(); t.Span(" / "); t.TotalPages(); });
            });
        }).GeneratePdf();
    }
}
