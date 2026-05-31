namespace VTE.WPF.Services;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VTE.Core.Entities;
using VTE.WPF.Resources;

public static class PaymentReceiptPdf
{
    public static byte[] Generate(PaymentDocument payment, Customer customer, Vehicle vehicle,
        List<PaymentLineItem> lineItems, string organizationName, string? organizationAddress,
        string? operatorName)
    {
        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A5);
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Content().Column(col =>
                {
                    // Top border
                    col.Item().PaddingBottom(4).Text("═══════════════════════════════════════")
                        .FontSize(8).AlignCenter();

                    // Organization header
                    col.Item().AlignCenter().Text(organizationName).Bold().FontSize(13);
                    if (!string.IsNullOrWhiteSpace(organizationAddress))
                        col.Item().AlignCenter().Text(organizationAddress).FontSize(9);

                    col.Item().PaddingTop(2).Text("═══════════════════════════════════════")
                        .FontSize(8).AlignCenter();

                    // Receipt title & date
                    col.Item().PaddingTop(6).AlignCenter()
                        .Text($"{Strings.ReceiptTitle} бр. {payment.DocumentNumber}").Bold().FontSize(12);
                    col.Item().AlignCenter()
                        .Text($"Датум: {payment.PaymentDate:dd.MM.yyyy}").FontSize(10);

                    col.Item().PaddingTop(4).Text("───────────────────────────────────────")
                        .FontSize(8).AlignCenter();

                    // Customer & vehicle
                    var regNum = vehicle.LastRegistrationNumber ?? vehicle.FirstRegistrationNumber ?? vehicle.ShellNumber;
                    var customerName = customer.IsCompany && !string.IsNullOrEmpty(customer.CompanyName)
                        ? customer.CompanyName
                        : $"{customer.FirstName} {customer.LastName}";

                    col.Item().PaddingTop(4).Text($"Клиент: {customerName}");
                    col.Item().Text($"Возило: {regNum}");

                    col.Item().PaddingTop(4).Text("───────────────────────────────────────")
                        .FontSize(8).AlignCenter();

                    // Line items
                    col.Item().PaddingTop(4);
                    foreach (var item in lineItems)
                    {
                        var total = item.UnitPrice * item.Quantity + item.VATAmount;
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text(item.Description).FontSize(10);
                            row.ConstantItem(80).AlignRight().Text($"{total:N2}").FontSize(10);
                        });
                    }

                    col.Item().PaddingTop(4).Text("───────────────────────────────────────")
                        .FontSize(8).AlignCenter();

                    // Totals
                    var subtotal = lineItems.Sum(li => li.UnitPrice * li.Quantity);
                    var totalVat = lineItems.Sum(li => li.VATAmount);
                    var grandTotal = subtotal + totalVat;
                    decimal discountAmount = 0;

                    if (payment.DiscountPercent > 0)
                    {
                        discountAmount = grandTotal * payment.DiscountPercent / 100m;
                        grandTotal -= discountAmount;

                        col.Item().PaddingTop(4).Row(row =>
                        {
                            row.RelativeItem().Text($"Попуст ({payment.DiscountPercent:N0}%):");
                            row.ConstantItem(80).AlignRight().Text($"-{discountAmount:N2}");
                        });
                    }

                    col.Item().PaddingTop(2).Row(row =>
                    {
                        row.RelativeItem().Text("ДДВ:");
                        row.ConstantItem(80).AlignRight().Text($"{totalVat:N2}");
                    });

                    col.Item().PaddingTop(4).Row(row =>
                    {
                        row.RelativeItem().Text("ВКУПНО:").Bold().FontSize(12);
                        row.ConstantItem(80).AlignRight().Text($"{grandTotal:N2}").Bold().FontSize(12);
                    });

                    col.Item().PaddingTop(4).Text("═══════════════════════════════════════")
                        .FontSize(8).AlignCenter();

                    // Operator & status
                    if (!string.IsNullOrWhiteSpace(operatorName))
                        col.Item().PaddingTop(4).Text($"{Strings.Operator}: {operatorName}");

                    col.Item().PaddingTop(2).Text(text =>
                    {
                        text.Span("Статус: ");
                        if (payment.IsPaid)
                            text.Span("ПЛАТЕНО").Bold().FontColor(Colors.Green.Darken2);
                        else
                            text.Span("НЕПЛАТЕНО").Bold().FontColor(Colors.Red.Darken2);
                    });
                });
            });
        });

        return document.GeneratePdf();
    }
}
