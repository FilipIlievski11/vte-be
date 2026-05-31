namespace VTE.WPF.Services;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using VTE.Core.Entities;
using VTE.WPF.Resources;

public static class TechnicalExamPdf
{
    public static byte[] Generate(TechnicalExamReport exam, Customer customer, Vehicle vehicle,
        string organizationName, string? stationAddress, string examTypeName,
        string controllerName, string? secondControllerName)
    {
        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(9));

                // Header
                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text(organizationName).Bold().FontSize(14);
                            if (!string.IsNullOrWhiteSpace(stationAddress))
                                c.Item().Text(stationAddress).FontSize(9);
                        });
                        row.ConstantItem(160).Column(c =>
                        {
                            c.Item().AlignRight().Text($"Датум: {exam.ExamDate:dd.MM.yyyy}").FontSize(10);
                            c.Item().AlignRight().Text($"Рег. бр: {exam.RegistrationNumber}").FontSize(10).SemiBold();
                        });
                    });
                    col.Item().PaddingVertical(4).LineHorizontal(1);
                    col.Item().AlignCenter().Text("ИЗВЕШТАЈ ОД ТЕХНИЧКИ ПРЕГЛЕД").Bold().FontSize(14);
                    col.Item().PaddingBottom(6);
                });

                // Content
                page.Content().Column(col =>
                {
                    // Vehicle & Owner info
                    var regNum = vehicle.LastRegistrationNumber ?? vehicle.FirstRegistrationNumber ?? "";
                    var customerName = customer.IsCompany && !string.IsNullOrEmpty(customer.CompanyName)
                        ? customer.CompanyName
                        : $"{customer.FirstName} {customer.LastName}";
                    var vehicleDesc = vehicle.VehicleModel != null
                        ? (vehicle.VehicleModel.VehicleMaker?.Name ?? "") + " " + vehicle.VehicleModel.Name
                        : "";

                    col.Item().Text("ПОДАТОЦИ ЗА ВОЗИЛО И СОПСТВЕНИК").Bold().FontSize(10);
                    col.Item().PaddingVertical(4).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(140);
                            cols.RelativeColumn();
                            cols.ConstantColumn(140);
                            cols.RelativeColumn();
                        });

                        AddInfoRow(table, "Регистарски број:", regNum, "Шасија:", vehicle.ShellNumber);
                        AddInfoRow(table, "Марка/Модел:", vehicleDesc, "Категорија:", vehicle.Category?.Name ?? "");
                        AddInfoRow(table, "Сопственик:", customerName, "Тип на преглед:", examTypeName);
                        AddInfoRow(table, "Датум на преглед:", exam.ExamDate.ToString("dd.MM.yyyy"),
                            "Важи до:", exam.ValidUntilDate.ToString("dd.MM.yyyy"));
                        AddInfoRow(table, "Контролор:", controllerName,
                            "Втор контролор:", secondControllerName ?? "-");
                    });

                    col.Item().PaddingTop(8);

                    // Brake measurements table
                    col.Item().Text("МЕРЕЊА НА СОПИРАЧКИ").Bold().FontSize(10);
                    col.Item().PaddingVertical(4).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.ConstantColumn(90); // Axle label
                            cols.RelativeColumn(); // Left
                            cols.RelativeColumn(); // Right
                            cols.RelativeColumn(); // Gj
                            cols.RelativeColumn(); // Left Pj
                            cols.RelativeColumn(); // PN
                        });

                        // Header
                        table.Header(header =>
                        {
                            var bg = Colors.Grey.Lighten3;
                            header.Cell().Background(bg).Padding(3).Text("").SemiBold();
                            header.Cell().Background(bg).Padding(3).Text("Left KN").SemiBold().AlignCenter();
                            header.Cell().Background(bg).Padding(3).Text("Right KN").SemiBold().AlignCenter();
                            header.Cell().Background(bg).Padding(3).Text("Gj").SemiBold().AlignCenter();
                            header.Cell().Background(bg).Padding(3).Text("Left Pj").SemiBold().AlignCenter();
                            header.Cell().Background(bg).Padding(3).Text("PN").SemiBold().AlignCenter();
                        });

                        AddBrakeRow(table, "Оска 1", exam.Axle1BrakeLeftKN, exam.Axle1BrakeRightKN,
                            exam.Axle1BrakeGj, exam.Axle1BrakeLeftPj, exam.Axle1BrakePN, false);
                        AddBrakeRow(table, "Оска 2", exam.Axle2BrakeLeftKN, exam.Axle2BrakeRightKN,
                            exam.Axle2BrakeGj, exam.Axle2BrakeLeftPj, exam.Axle2BrakePN, true);
                        AddBrakeRow(table, "Оска 3", exam.Axle3BrakeLeftKN, exam.Axle3BrakeRightKN,
                            exam.Axle3BrakeGj, exam.Axle3BrakeLeftPj, exam.Axle3BrakePN, false);
                        AddBrakeRow(table, "Оска 4", exam.Axle4BrakeLeftKN, exam.Axle4BrakeRightKN,
                            exam.Axle4BrakeGj, exam.Axle4BrakeLeftPj, exam.Axle4BrakePN, true);
                        AddBrakeRow(table, "Паркинг", exam.ParkingBrakeLeftKN, exam.ParkingBrakeRightKN,
                            exam.ParkingBrakeGj, exam.ParkingBrakeLeftPj, exam.ParkingBrakePN, false);
                    });

                    col.Item().PaddingTop(8);

                    // Brake effectiveness
                    col.Item().Text("ЕФЕКТИВНОСТ НА СОПИРАЧКИ").Bold().FontSize(10);
                    col.Item().PaddingVertical(4).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn();
                            cols.ConstantColumn(80);
                        });

                        AddValueRow(table, "Работна сопирачка (празно):", exam.WorkingBrakeEffectivenessEmpty);
                        AddValueRow(table, "Работна сопирачка (полно):", exam.WorkingBrakeEffectivenessFull);
                        AddValueRow(table, "Помошна сопирачка:", exam.SecondaryBrakeEffectiveness);
                        AddValueRow(table, "Паркинг сопирачка:", exam.ParkingBrakeEffectiveness);
                        AddValueRow(table, "Маса на возило (KG):", exam.VehicleWeightKG);
                    });

                    col.Item().PaddingTop(8);

                    // Emissions
                    col.Item().Text("ЕМИСИИ").Bold().FontSize(10);
                    col.Item().PaddingVertical(4).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn();
                            cols.ConstantColumn(80);
                            cols.RelativeColumn();
                            cols.ConstantColumn(80);
                        });

                        var bg = Colors.White;
                        table.Cell().Background(bg).Padding(2).Text("Вртежи (RPM):");
                        table.Cell().Background(bg).Padding(2).AlignRight().Text(exam.EngineSpeedRPM?.ToString() ?? "-");
                        table.Cell().Background(bg).Padding(2).Text("CO:");
                        table.Cell().Background(bg).Padding(2).AlignRight().Text(Fmt(exam.CO));

                        bg = Colors.Grey.Lighten5;
                        table.Cell().Background(bg).Padding(2).Text("Вртежи на мотор:");
                        table.Cell().Background(bg).Padding(2).AlignRight().Text(exam.EngineTurns?.ToString() ?? "-");
                        table.Cell().Background(bg).Padding(2).Text("CO + Вртежи:");
                        table.Cell().Background(bg).Padding(2).AlignRight().Text(Fmt(exam.COPlusTurns));

                        bg = Colors.White;
                        table.Cell().Background(bg).Padding(2).Text("Ламбда:");
                        table.Cell().Background(bg).Padding(2).AlignRight().Text(Fmt(exam.Lambda));
                        table.Cell().Background(bg).Padding(2).Text("Точки:");
                        table.Cell().Background(bg).Padding(2).AlignRight().Text(Fmt(exam.Pinpoints));

                        bg = Colors.Grey.Lighten5;
                        table.Cell().Background(bg).Padding(2).Text("Бучава (dB):");
                        table.Cell().Background(bg).Padding(2).AlignRight().Text(Fmt(exam.NoiseDB));
                        table.Cell().Background(bg).Padding(2).Text("Температура масло (C):");
                        table.Cell().Background(bg).Padding(2).AlignRight().Text(Fmt(exam.EngineOilTemperatureC));
                    });

                    col.Item().PaddingTop(8);

                    // Notes
                    if (!string.IsNullOrWhiteSpace(exam.TechnicalChanges)
                        || !string.IsNullOrWhiteSpace(exam.ExplanationNote)
                        || !string.IsNullOrWhiteSpace(exam.DriverWarning)
                        || !string.IsNullOrWhiteSpace(exam.Note))
                    {
                        col.Item().Text("ЗАБЕЛЕШКИ").Bold().FontSize(10);
                        col.Item().PaddingTop(4);

                        if (!string.IsNullOrWhiteSpace(exam.TechnicalChanges))
                        {
                            col.Item().Text("Технички промени:").SemiBold();
                            col.Item().Text(exam.TechnicalChanges);
                        }
                        if (!string.IsNullOrWhiteSpace(exam.ExplanationNote))
                        {
                            col.Item().PaddingTop(2).Text("Образложение:").SemiBold();
                            col.Item().Text(exam.ExplanationNote);
                        }
                        if (!string.IsNullOrWhiteSpace(exam.DriverWarning))
                        {
                            col.Item().PaddingTop(2).Text("Предупредување за возач:").SemiBold();
                            col.Item().Text(exam.DriverWarning);
                        }
                        if (!string.IsNullOrWhiteSpace(exam.Note))
                        {
                            col.Item().PaddingTop(2).Text("Забелешка:").SemiBold();
                            col.Item().Text(exam.Note);
                        }
                        col.Item().PaddingTop(8);
                    }

                    // Result - large bold
                    col.Item().PaddingTop(10).AlignCenter().Text(text =>
                    {
                        text.Span("РЕЗУЛТАТ: ").Bold().FontSize(16);
                        if (exam.VehiclePassed)
                            text.Span(Strings.VehicleCorrect).Bold().FontSize(20).FontColor(Colors.Green.Darken2);
                        else
                            text.Span(Strings.VehicleIncorrect).Bold().FontSize(20).FontColor(Colors.Red.Darken2);
                    });
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

    private static string Fmt(decimal? val) => val.HasValue ? val.Value.ToString("N2") : "-";

    private static void AddInfoRow(TableDescriptor table, string label1, string value1, string label2, string value2)
    {
        table.Cell().Padding(2).Text(label1).SemiBold();
        table.Cell().Padding(2).Text(value1);
        table.Cell().Padding(2).Text(label2).SemiBold();
        table.Cell().Padding(2).Text(value2);
    }

    private static void AddBrakeRow(TableDescriptor table, string axleName,
        decimal? left, decimal? right, decimal? gj, decimal? leftPj, decimal? pn, bool alt)
    {
        var bg = alt ? Colors.Grey.Lighten5 : Colors.White;
        table.Cell().Background(bg).Padding(2).Text(axleName).SemiBold();
        table.Cell().Background(bg).Padding(2).AlignCenter().Text(Fmt(left));
        table.Cell().Background(bg).Padding(2).AlignCenter().Text(Fmt(right));
        table.Cell().Background(bg).Padding(2).AlignCenter().Text(Fmt(gj));
        table.Cell().Background(bg).Padding(2).AlignCenter().Text(Fmt(leftPj));
        table.Cell().Background(bg).Padding(2).AlignCenter().Text(Fmt(pn));
    }

    private static void AddValueRow(TableDescriptor table, string label, decimal? value)
    {
        table.Cell().Padding(2).Text(label);
        table.Cell().Padding(2).AlignRight().Text(Fmt(value));
    }
}
