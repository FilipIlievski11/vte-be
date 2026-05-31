namespace VTE.WPF.Services;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VTE.Core.Entities;

public static class InvoicePdfGenerator
{
    public static byte[] Generate(PaymentDocument payment, Customer customer, Vehicle vehicle,
        List<PaymentLineItem> lineItems, string organizationName, string? organizationAddress = null)
    {
        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                // Header
                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text(organizationName).Bold().FontSize(14);
                            if (!string.IsNullOrWhiteSpace(organizationAddress))
                                c.Item().Text(organizationAddress).FontSize(9);
                            c.Item().Text("Фактура / Invoice").FontSize(12).SemiBold();
                        });
                        row.ConstantItem(150).Column(c =>
                        {
                            c.Item().Text($"Број: {payment.DocumentNumber}").AlignRight();
                            c.Item().Text($"Датум: {payment.PaymentDate:dd.MM.yyyy}").AlignRight();
                        });
                    });
                    col.Item().PaddingVertical(5).LineHorizontal(1);
                });

                // Content
                page.Content().PaddingVertical(10).Column(col =>
                {
                    // Customer info
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Клиент:").SemiBold();
                            c.Item().Text($"{customer.FirstName} {customer.LastName}");
                            if (customer.IsCompany && customer.CompanyName != null)
                                c.Item().Text(customer.CompanyName);
                            if (customer.LivingCity != null)
                                c.Item().Text(customer.LivingCity.Name);
                            if (customer.LivingAddressStreet != null)
                                c.Item().Text(customer.LivingAddressStreet.Name);
                            if (customer.TaxNumber != null)
                                c.Item().Text($"ЕДБ: {customer.TaxNumber}");
                        });
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("Возило:").SemiBold();
                            c.Item().Text($"Рег. бр: {vehicle.LastRegistrationNumber ?? vehicle.FirstRegistrationNumber}");
                            c.Item().Text($"Шасија: {vehicle.ShellNumber}");
                        });
                    });

                    col.Item().PaddingVertical(10);

                    // Line items table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(30);  // #
                            cols.RelativeColumn(4);    // Description
                            cols.ConstantColumn(50);   // Qty
                            cols.ConstantColumn(80);   // Unit Price
                            cols.ConstantColumn(60);   // VAT %
                            cols.ConstantColumn(80);   // VAT Amount
                            cols.ConstantColumn(90);   // Total
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("#").SemiBold();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("Опис").SemiBold();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("Кол.").SemiBold().AlignRight();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("Цена").SemiBold().AlignRight();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("ДДВ%").SemiBold().AlignRight();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("ДДВ").SemiBold().AlignRight();
                            header.Cell().Background(Colors.Grey.Lighten3).Padding(4).Text("Вкупно").SemiBold().AlignRight();
                        });

                        // Rows
                        int i = 1;
                        foreach (var item in lineItems)
                        {
                            var vatAmount = item.UnitPrice * item.Quantity * item.VATPercent / 100m;
                            var total = item.UnitPrice * item.Quantity + vatAmount;

                            var bg = i % 2 == 0 ? Colors.Grey.Lighten5 : Colors.White;
                            table.Cell().Background(bg).Padding(3).Text($"{i}");
                            table.Cell().Background(bg).Padding(3).Text(item.Description);
                            table.Cell().Background(bg).Padding(3).Text($"{item.Quantity}").AlignRight();
                            table.Cell().Background(bg).Padding(3).Text($"{item.UnitPrice:N2}").AlignRight();
                            table.Cell().Background(bg).Padding(3).Text($"{item.VATPercent:N0}%").AlignRight();
                            table.Cell().Background(bg).Padding(3).Text($"{vatAmount:N2}").AlignRight();
                            table.Cell().Background(bg).Padding(3).Text($"{total:N2}").AlignRight();
                            i++;
                        }
                    });

                    col.Item().PaddingVertical(5);

                    // Totals
                    var subtotal = lineItems.Sum(li => li.UnitPrice * li.Quantity);
                    var totalVat = lineItems.Sum(li => li.UnitPrice * li.Quantity * li.VATPercent / 100m);
                    var grandTotal = subtotal + totalVat;

                    if (payment.DiscountPercent > 0)
                    {
                        var discount = grandTotal * payment.DiscountPercent / 100m;
                        col.Item().AlignRight().Text($"Попуст ({payment.DiscountPercent:N0}%): -{discount:N2}");
                        grandTotal -= discount;
                    }

                    col.Item().AlignRight().Text($"Основица: {subtotal:N2}");
                    col.Item().AlignRight().Text($"ДДВ: {totalVat:N2}");
                    col.Item().AlignRight().Text($"ВКУПНО: {grandTotal:N2}").Bold().FontSize(12);

                    col.Item().PaddingVertical(10);
                    col.Item().Text($"Статус: {(payment.IsPaid ? "ПЛАТЕНО" : "НЕПЛАТЕНО")}").SemiBold();

                    if (!string.IsNullOrEmpty(payment.Note))
                    {
                        col.Item().PaddingTop(10);
                        col.Item().Text($"Забелешка: {payment.Note}");
                    }
                });

                // Footer
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Страна ");
                    text.CurrentPageNumber();
                    text.Span(" од ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }
}
