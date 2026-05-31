using System.Globalization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace VTE.Api.Pdf;

// Standalone PDFs for a PaymentDocument — printable as Faktura (invoice),
// Smetka (receipt), Rata (installment slip) or Dogovor (installment agreement).
// All four share the same header/parties/footer; the body adapts to the kind.
public static class PaymentDocumentPdf
{
    public enum Kind { Faktura, Smetka, Rata, Dogovor }

    public sealed record PaymentData(
        long Id, string DocumentNumber, string PaymentTypeName, string? Prefix,
        DateOnly DatePay, DateOnly DateRequired, decimal DiscountPercent,
        bool Payed, bool Storno, string? Note,
        string StationName, string? StationAddress, string? StationTaxNumber, string? StationPhone,
        string? CustomerName, string? CustomerAddress, string? CustomerEMBG,
        string? VehicleReg, string? VehicleVin, string? VehicleMakerModel,
        List<DetailLine> Details, List<InstallmentLine> Installments,
        string? GuarantorName, string? GuarantorAddress, string? GuarantorEMBG, int? NumberOfInstallments,
        string? CalculationItemName, string? BankAccount, string? Bank, string? PaymentForm);

    public sealed record DetailLine(int Index, string ItemName, decimal Price, decimal DDVRate,
        decimal DiscountPercent, bool PrePayed, decimal Net, decimal DDVAmount, decimal Gross, string? Note);

    public sealed record InstallmentLine(int Number, decimal Price, DateOnly? DueDate, bool Payed, DateOnly? DatePayed);

    public static byte[] Build(Kind kind, PaymentData d)
    {
        return Document.Create(c => c.Page(p =>
        {
            p.Size(PageSizes.A4);
            p.Margin(18, Unit.Millimetre);
            p.DefaultTextStyle(t => t.FontSize(10).FontFamily("Arial"));
            p.PageColor(Colors.White);

            p.Header().Element(e => Header(e, kind, d));
            p.Content().PaddingTop(8).Column(col =>
            {
                Parties(col, d);
                col.Item().PaddingTop(6);
                switch (kind)
                {
                    case Kind.Faktura: case Kind.Smetka: DetailsTable(col, d); break;
                    case Kind.Rata:    RataView(col, d); break;
                    case Kind.Dogovor: DogovorView(col, d); break;
                }
                if (kind == Kind.Faktura || kind == Kind.Smetka) Totals(col, d);
                if (!string.IsNullOrWhiteSpace(d.Note))
                    col.Item().PaddingTop(6).Text(t => { t.Span("Забелешка: ").SemiBold(); t.Span(d.Note!); });
            });
            p.Footer().Element(e => Footer(e, kind, d));
        })).GeneratePdf();
    }

    // -- sections --------------------------------------------------------------

    private static void Header(IContainer e, Kind kind, PaymentData d)
    {
        var title = kind switch
        {
            Kind.Faktura => "ФАКТУРА",
            Kind.Smetka  => "СМЕТКА",
            Kind.Rata    => "СМЕТКА ЗА РАТА",
            Kind.Dogovor => "ДОГОВОР ЗА РАТИ",
            _            => "ДОКУМЕНТ ЗА ПЛАЌАЊЕ",
        };
        e.Column(col =>
        {
            col.Item().Row(r =>
            {
                r.RelativeItem().Column(c2 =>
                {
                    c2.Item().Text(d.StationName).Bold().FontSize(13);
                    if (!string.IsNullOrEmpty(d.StationAddress)) c2.Item().Text(d.StationAddress!).FontSize(9);
                    var idLine = new List<string>();
                    if (!string.IsNullOrEmpty(d.StationTaxNumber)) idLine.Add("ДБ: " + d.StationTaxNumber);
                    if (!string.IsNullOrEmpty(d.StationPhone))     idLine.Add("Тел: " + d.StationPhone);
                    if (idLine.Count > 0) c2.Item().Text(string.Join("   ", idLine)).FontSize(9);
                });
                r.ConstantItem(180).AlignRight().Column(c2 =>
                {
                    c2.Item().Text(title).Bold().FontSize(14);
                    c2.Item().Text(d.DocumentNumber).SemiBold().FontSize(11);
                    if (d.Storno) c2.Item().Text("СТОРНО").Bold().FontColor(Colors.Red.Medium);
                    else if (d.Payed) c2.Item().Text("ПЛАТЕНО").Bold().FontColor(Colors.Green.Medium);
                });
            });
            col.Item().PaddingTop(4).LineHorizontal(0.5f).LineColor(Colors.Grey.Medium);
        });
    }

    private static void Parties(ColumnDescriptor col, PaymentData d)
    {
        col.Item().Row(r =>
        {
            r.RelativeItem().Column(c2 =>
            {
                c2.Item().Text("Купувач / Сопственик").FontSize(8).FontColor(Colors.Grey.Darken2);
                c2.Item().Text(d.CustomerName ?? "—").SemiBold();
                if (!string.IsNullOrEmpty(d.CustomerAddress)) c2.Item().Text(d.CustomerAddress!).FontSize(9);
                if (!string.IsNullOrEmpty(d.CustomerEMBG))    c2.Item().Text("ЕМБГ: " + d.CustomerEMBG!).FontSize(9);
            });
            r.RelativeItem().Column(c2 =>
            {
                c2.Item().Text("Возило").FontSize(8).FontColor(Colors.Grey.Darken2);
                c2.Item().Text(d.VehicleMakerModel ?? "—").SemiBold();
                var line2 = new List<string>();
                if (!string.IsNullOrEmpty(d.VehicleReg)) line2.Add(d.VehicleReg!);
                if (!string.IsNullOrEmpty(d.VehicleVin)) line2.Add("VIN " + d.VehicleVin);
                if (line2.Count > 0) c2.Item().Text(string.Join("  ·  ", line2)).FontSize(9);
            });
            r.ConstantItem(140).Column(c2 =>
            {
                c2.Item().Text("Тип на плаќање").FontSize(8).FontColor(Colors.Grey.Darken2);
                c2.Item().Text(d.PaymentTypeName).SemiBold();
                c2.Item().Text("Датум: " + d.DatePay.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)).FontSize(9);
                c2.Item().Text("Досп: "  + d.DateRequired.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)).FontSize(9);
            });
        });
    }

    private static void DetailsTable(ColumnDescriptor col, PaymentData d)
    {
        col.Item().Table(t =>
        {
            t.ColumnsDefinition(c =>
            {
                c.ConstantColumn(22);   // #
                c.RelativeColumn(5);    // item
                c.ConstantColumn(70);   // price
                c.ConstantColumn(50);   // DDV %
                c.ConstantColumn(55);   // discount %
                c.ConstantColumn(70);   // net
                c.ConstantColumn(70);   // DDV
                c.ConstantColumn(80);   // gross
            });
            t.Header(h =>
            {
                static IContainer hcell(IContainer x) => x.Background(Colors.Grey.Lighten3).Padding(4).BorderBottom(0.5f).BorderColor(Colors.Grey.Medium);
                h.Cell().Element(hcell).Text("#").Bold();
                h.Cell().Element(hcell).Text("Ставка").Bold();
                h.Cell().Element(hcell).AlignRight().Text("Цена").Bold();
                h.Cell().Element(hcell).AlignRight().Text("ДДВ %").Bold();
                h.Cell().Element(hcell).AlignRight().Text("Поп. %").Bold();
                h.Cell().Element(hcell).AlignRight().Text("Нето").Bold();
                h.Cell().Element(hcell).AlignRight().Text("ДДВ").Bold();
                h.Cell().Element(hcell).AlignRight().Text("Бруто").Bold();
            });
            foreach (var line in d.Details)
            {
                static IContainer body(IContainer x) => x.Padding(3).BorderBottom(0.3f).BorderColor(Colors.Grey.Lighten2);
                t.Cell().Element(body).Text(line.Index.ToString());
                t.Cell().Element(body).Text(text => {
                    text.Span(line.ItemName);
                    if (line.PrePayed) text.Line("").Italic().FontColor(Colors.Grey.Medium);
                    if (line.PrePayed) text.Span(" (предплатено)").Italic().FontColor(Colors.Grey.Medium).FontSize(8);
                });
                t.Cell().Element(body).AlignRight().Text(Money(line.Price));
                t.Cell().Element(body).AlignRight().Text(line.DDVRate.ToString("0.##", CultureInfo.InvariantCulture));
                t.Cell().Element(body).AlignRight().Text(line.DiscountPercent.ToString("0.##", CultureInfo.InvariantCulture));
                t.Cell().Element(body).AlignRight().Text(Money(line.Net));
                t.Cell().Element(body).AlignRight().Text(Money(line.DDVAmount));
                t.Cell().Element(body).AlignRight().Text(Money(line.Gross)).SemiBold();
            }
        });
    }

    private static void Totals(ColumnDescriptor col, PaymentData d)
    {
        var subtotal = d.Details.Where(x => !x.PrePayed).Sum(x => x.Net);
        var ddv      = d.Details.Where(x => !x.PrePayed).Sum(x => x.DDVAmount);
        var preTotal = d.Details.Where(x => x.PrePayed).Sum(x => x.Gross);
        var beforeDiscount = subtotal + ddv;
        var docDiscount = beforeDiscount * (d.DiscountPercent / 100m);
        var grand = beforeDiscount - docDiscount;

        col.Item().PaddingTop(8).AlignRight().Column(c =>
        {
            c.Item().Text("Основица: " + Money(subtotal) + " ден.");
            c.Item().Text("ДДВ: "      + Money(ddv)      + " ден.");
            if (d.DiscountPercent > 0)
                c.Item().Text("Попуст (" + d.DiscountPercent.ToString("0.##", CultureInfo.InvariantCulture) + "%): -" + Money(docDiscount) + " ден.");
            if (preTotal > 0)
                c.Item().Text("Предплатено: " + Money(preTotal) + " ден.").FontColor(Colors.Grey.Medium);
            c.Item().PaddingTop(2).Text("Вкупно за плаќање: " + Money(grand) + " ден.").Bold().FontSize(12);
        });
    }

    private static void RataView(ColumnDescriptor col, PaymentData d)
    {
        col.Item().Table(t =>
        {
            t.ColumnsDefinition(c =>
            {
                c.ConstantColumn(40);
                c.ConstantColumn(120);
                c.ConstantColumn(120);
                c.RelativeColumn(1);
            });
            t.Header(h =>
            {
                static IContainer hcell(IContainer x) => x.Background(Colors.Grey.Lighten3).Padding(4).BorderBottom(0.5f);
                h.Cell().Element(hcell).Text("#").Bold();
                h.Cell().Element(hcell).AlignRight().Text("Износ").Bold();
                h.Cell().Element(hcell).Text("Доспева").Bold();
                h.Cell().Element(hcell).Text("Статус").Bold();
            });
            foreach (var i in d.Installments)
            {
                static IContainer body(IContainer x) => x.Padding(3).BorderBottom(0.3f).BorderColor(Colors.Grey.Lighten2);
                t.Cell().Element(body).Text(i.Number.ToString());
                t.Cell().Element(body).AlignRight().Text(Money(i.Price) + " ден.");
                t.Cell().Element(body).Text(i.DueDate?.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) ?? "—");
                t.Cell().Element(body).Text(i.Payed
                    ? "Платено " + (i.DatePayed?.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) ?? "")
                    : "Неплатено");
            }
        });
    }

    private static void DogovorView(ColumnDescriptor col, PaymentData d)
    {
        col.Item().Text("Гарант").FontSize(10).Bold();
        col.Item().Text(d.GuarantorName ?? "—");
        if (!string.IsNullOrEmpty(d.GuarantorAddress)) col.Item().Text(d.GuarantorAddress!).FontSize(9);
        if (!string.IsNullOrEmpty(d.GuarantorEMBG))    col.Item().Text("ЕМБГ: " + d.GuarantorEMBG!).FontSize(9);

        col.Item().PaddingTop(10).Text("Услови").Bold();
        col.Item().Text(
            "Должникот се обврзува должно да ги исплати " +
            (d.NumberOfInstallments?.ToString() ?? "—") + " рати според следната динамика. " +
            "Гарантот солидарно одговара за неисплатените рати.");

        col.Item().PaddingTop(8);
        RataView(col, d);
    }

    private static void Footer(IContainer e, Kind kind, PaymentData d)
    {
        e.Column(col =>
        {
            col.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Medium);
            col.Item().PaddingTop(4).Row(r =>
            {
                r.RelativeItem().Column(c =>
                {
                    if (!string.IsNullOrEmpty(d.Bank) || !string.IsNullOrEmpty(d.BankAccount))
                    {
                        c.Item().Text("Уплата на сметка").FontSize(8).FontColor(Colors.Grey.Darken2);
                        if (!string.IsNullOrEmpty(d.Bank))        c.Item().Text(d.Bank!).FontSize(9);
                        if (!string.IsNullOrEmpty(d.BankAccount)) c.Item().Text(d.BankAccount!).FontSize(9).SemiBold();
                        if (!string.IsNullOrEmpty(d.PaymentForm)) c.Item().Text("Образец: " + d.PaymentForm!).FontSize(9);
                    }
                });
                r.RelativeItem().AlignRight().Column(c =>
                {
                    c.Item().Text("_______________________").FontSize(9);
                    c.Item().Text("Печат и потпис").FontSize(8).FontColor(Colors.Grey.Darken2);
                });
            });
            col.Item().AlignCenter().Text(t =>
            {
                t.Span("страна ").FontSize(8).FontColor(Colors.Grey.Medium);
                t.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                t.Span(" / ").FontSize(8).FontColor(Colors.Grey.Medium);
                t.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
            });
        });
    }

    private static string Money(decimal v) => v.ToString("N2", CultureInfo.InvariantCulture);
}
