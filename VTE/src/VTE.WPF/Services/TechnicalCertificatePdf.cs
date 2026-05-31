namespace VTE.WPF.Services;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VTE.Core.Entities;
using VTE.WPF.Resources;

public static class TechnicalCertificatePdf
{
    public static byte[] Generate(TechnicalExamReport exam, Customer customer, Vehicle vehicle,
        string organizationName, string? organizationAddress, string? organizationPhone,
        string controllerName, string? secondControllerName)
    {
        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));

                // Page border
                page.Background().Border(2).BorderColor(Colors.Grey.Darken1).Padding(6)
                    .Border(0.5f).BorderColor(Colors.Grey.Medium).Padding(10)
                    .Container();

                // Header
                page.Header().Column(col =>
                {
                    col.Item().AlignCenter().Text(organizationName).Bold().FontSize(16);
                    if (!string.IsNullOrWhiteSpace(organizationAddress))
                        col.Item().AlignCenter().Text(organizationAddress).FontSize(10);
                    if (!string.IsNullOrWhiteSpace(organizationPhone))
                        col.Item().AlignCenter().Text($"Тел: {organizationPhone}").FontSize(10);
                    col.Item().PaddingVertical(8).LineHorizontal(1.5f);
                });

                // Content
                page.Content().PaddingVertical(10).Column(col =>
                {
                    // Title
                    col.Item().PaddingTop(20).AlignCenter()
                        .Text(Strings.CertificateTitle).Bold().FontSize(18);

                    col.Item().PaddingTop(30);

                    // Certificate body text
                    var regNum = vehicle.LastRegistrationNumber ?? vehicle.FirstRegistrationNumber ?? vehicle.ShellNumber;
                    var customerName = customer.IsCompany && !string.IsNullOrEmpty(customer.CompanyName)
                        ? customer.CompanyName
                        : $"{customer.FirstName} {customer.LastName}";
                    var vehicleDesc = vehicle.VehicleModel != null
                        ? (vehicle.VehicleModel.VehicleMaker?.Name ?? "") + " " + vehicle.VehicleModel.Name
                        : "";

                    col.Item().Text(text =>
                    {
                        text.Span("Со оваа потврда се потврдува дека возилото со регистарски број ");
                        text.Span(regNum).Bold();
                        text.Span(", сопственост на ");
                        text.Span(customerName).Bold();
                        text.Span(", е технички исправно и го помина техничкиот преглед извршен на ");
                        text.Span(exam.ExamDate.ToString("dd.MM.yyyy")).Bold();
                        text.Span(".");
                    });

                    col.Item().PaddingTop(20);

                    // Vehicle details
                    col.Item().Text("Податоци за возилото:").SemiBold().FontSize(12);
                    col.Item().PaddingTop(8).PaddingLeft(20).Column(details =>
                    {
                        details.Item().Text($"Регистарски број: {regNum}");
                        details.Item().Text($"Број на шасија: {vehicle.ShellNumber}");
                        if (!string.IsNullOrEmpty(vehicleDesc))
                            details.Item().Text($"Марка/Модел: {vehicleDesc}");
                        if (vehicle.Category != null)
                            details.Item().Text($"Категорија: {vehicle.Category.Name}");
                    });

                    col.Item().PaddingTop(16);

                    // Owner details
                    col.Item().Text("Податоци за сопственикот:").SemiBold().FontSize(12);
                    col.Item().PaddingTop(8).PaddingLeft(20).Column(details =>
                    {
                        details.Item().Text($"Име и презиме: {customerName}");
                        if (!string.IsNullOrEmpty(customer.IdentificationNumber))
                            details.Item().Text($"Матичен број: {customer.IdentificationNumber}");
                        if (customer.IsCompany && !string.IsNullOrEmpty(customer.TaxNumber))
                            details.Item().Text($"ЕДБ: {customer.TaxNumber}");
                    });

                    col.Item().PaddingTop(16);

                    // Exam dates
                    col.Item().Text("Преглед:").SemiBold().FontSize(12);
                    col.Item().PaddingTop(8).PaddingLeft(20).Column(details =>
                    {
                        details.Item().Text($"Датум на преглед: {exam.ExamDate:dd.MM.yyyy}");
                        details.Item().Text($"Важи до: {exam.ValidUntilDate:dd.MM.yyyy}");
                    });

                    col.Item().PaddingTop(16);

                    // Controllers
                    col.Item().Text("Контролори:").SemiBold().FontSize(12);
                    col.Item().PaddingTop(8).PaddingLeft(20).Column(details =>
                    {
                        details.Item().Text($"Прв контролор: {controllerName}");
                        if (!string.IsNullOrEmpty(secondControllerName))
                            details.Item().Text($"Втор контролор: {secondControllerName}");
                    });

                    // Stamp area
                    col.Item().PaddingTop(60).Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignCenter().Text("М.П.").FontSize(10).Italic();
                            c.Item().PaddingTop(4).AlignCenter()
                                .Text("(Печат на организацијата)").FontSize(8).Italic();
                        });
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().AlignCenter().Text("_________________________");
                            c.Item().PaddingTop(4).AlignCenter()
                                .Text("(Потпис на контролор)").FontSize(8).Italic();
                        });
                    });
                });

                // Footer
                page.Footer().Column(col =>
                {
                    col.Item().LineHorizontal(0.5f);
                    col.Item().PaddingTop(4).Row(row =>
                    {
                        row.RelativeItem().Text($"Датум: {exam.ExamDate:dd.MM.yyyy}").FontSize(8);
                        row.RelativeItem().AlignRight().Text(organizationName).FontSize(8);
                    });
                });
            });
        });

        return document.GeneratePdf();
    }
}
