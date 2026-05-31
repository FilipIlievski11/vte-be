namespace VTE.WPF.Services;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VTE.Core.Entities;
using VTE.WPF.Resources;

public static class TrafficLicensePdf
{
    public static byte[] Generate(TrafficLicense license, Customer customer, Vehicle vehicle,
        string organizationName)
    {
        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(35);
                page.DefaultTextStyle(x => x.FontSize(10));

                // Header
                page.Header().Column(col =>
                {
                    col.Item().AlignCenter().Text(organizationName).Bold().FontSize(14);
                    col.Item().PaddingVertical(6).LineHorizontal(1.5f);
                    col.Item().PaddingTop(8).AlignCenter()
                        .Text(Strings.TrafficLicenseTitle).Bold().FontSize(16);
                    col.Item().PaddingTop(4).AlignCenter()
                        .Text($"Број: {license.LicenseNumber}").FontSize(11).SemiBold();
                    col.Item().PaddingBottom(10);
                });

                // Content
                page.Content().PaddingVertical(10).Row(mainRow =>
                {
                    // Left column - Vehicle data
                    mainRow.RelativeItem().PaddingRight(12).Column(col =>
                    {
                        col.Item().Text("ПОДАТОЦИ ЗА ВОЗИЛОТО").Bold().FontSize(11);
                        col.Item().PaddingVertical(4).LineHorizontal(0.5f);
                        col.Item().PaddingTop(6);

                        var vehicleDesc = vehicle.VehicleModel != null
                            ? (vehicle.VehicleModel.VehicleMaker?.Name ?? "") + " " + vehicle.VehicleModel.Name
                            : "";

                        AddField(col, "Марка/Модел:", vehicleDesc);
                        AddField(col, "Категорија:", vehicle.Category?.Name ?? "");
                        AddField(col, "Форма на каросерија:", vehicle.BodyType?.Name ?? "");
                        AddField(col, "Тип на мотор:", vehicle.EngineType?.Name ?? "");
                        AddField(col, "Моќност (KW):", vehicle.EnginePowerKW?.ToString("N1") ?? "-");
                        AddField(col, "Зафатнина (CM3):", vehicle.EngineWorkingCapacityCM3?.ToString("N0") ?? "-");
                        AddField(col, "Празна маса (KG):", vehicle.EmptyWeightKG?.ToString("N0") ?? "-");
                        AddField(col, "Макс. маса (KG):", vehicle.MaxAllowedWeightKG?.ToString("N0") ?? "-");
                        AddField(col, "Број на седишта:", vehicle.NumberOfSeats?.ToString() ?? "-");
                        AddField(col, "Димензии (mm):",
                            $"{vehicle.LengthMM?.ToString() ?? "-"} x {vehicle.WidthMM?.ToString() ?? "-"} x {vehicle.HeightMM?.ToString() ?? "-"}");

                        if (vehicle.PrimaryColor != null)
                            AddField(col, "Боја:", vehicle.PrimaryColor.Name);

                        AddField(col, "Број на шасија:", vehicle.ShellNumber);
                        AddField(col, "Број на мотор:", vehicle.EngineNumber ?? "-");

                        if (vehicle.HasLPG)
                            AddField(col, "ТНГ:", "Да");
                        if (vehicle.HasHook)
                            AddField(col, "Кука:", "Да");
                    });

                    // Right column - Owner & Registration data
                    mainRow.RelativeItem().PaddingLeft(12).Column(col =>
                    {
                        col.Item().Text("ПОДАТОЦИ ЗА СОПСТВЕНИКОТ").Bold().FontSize(11);
                        col.Item().PaddingVertical(4).LineHorizontal(0.5f);
                        col.Item().PaddingTop(6);

                        var customerName = customer.IsCompany && !string.IsNullOrEmpty(customer.CompanyName)
                            ? customer.CompanyName
                            : $"{customer.FirstName} {customer.LastName}";

                        AddField(col, "Име:", customerName);

                        if (!string.IsNullOrEmpty(customer.IdentificationNumber))
                            AddField(col, "Матичен број:", customer.IdentificationNumber);
                        if (customer.IsCompany && !string.IsNullOrEmpty(customer.TaxNumber))
                            AddField(col, "ЕДБ:", customer.TaxNumber);
                        if (customer.LivingCity != null)
                            AddField(col, "Град:", customer.LivingCity.Name);
                        if (customer.LivingAddressStreet != null)
                            AddField(col, "Адреса:", customer.LivingAddressStreet.Name);

                        col.Item().PaddingTop(20);

                        col.Item().Text("РЕГИСТРАЦИЈА").Bold().FontSize(11);
                        col.Item().PaddingVertical(4).LineHorizontal(0.5f);
                        col.Item().PaddingTop(6);

                        var regNum = vehicle.LastRegistrationNumber ?? vehicle.FirstRegistrationNumber ?? license.PlateNumber ?? "";
                        AddField(col, "Регистарски број:", regNum);
                        if (!string.IsNullOrEmpty(license.PlateNumber))
                            AddField(col, "Табличка:", license.PlateNumber);
                        AddField(col, "Датум на издавање:", license.IssuedDate.ToString("dd.MM.yyyy"));
                        if (license.ValidUntilDate.HasValue)
                            AddField(col, "Важи до:", license.ValidUntilDate.Value.ToString("dd.MM.yyyy"));
                    });
                });

                // Footer
                page.Footer().Column(col =>
                {
                    col.Item().LineHorizontal(0.5f);
                    col.Item().PaddingTop(4).Row(row =>
                    {
                        row.RelativeItem().Text($"Издадено: {license.IssuedDate:dd.MM.yyyy}").FontSize(8);
                        row.RelativeItem().AlignRight().Text(organizationName).FontSize(8);
                    });
                });
            });
        });

        return document.GeneratePdf();
    }

    private static void AddField(ColumnDescriptor col, string label, string value)
    {
        col.Item().PaddingBottom(4).Row(row =>
        {
            row.ConstantItem(140).Text(label).SemiBold().FontSize(9);
            row.RelativeItem().Text(value).FontSize(10);
        });
    }
}
