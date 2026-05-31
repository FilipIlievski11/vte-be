using System.Text.Json;
using System.Text.Json.Serialization;

namespace VTE.Api.Pdf.RequestPrint;

// Layout schema produced by tools/parse-print-designer.ps1.
// Coordinates are in TenthsOfAMillimeter (legacy DevExpress ReportUnit).
// Convert to mm by dividing by 10.
public sealed class PrintLayout
{
    public PageInfo Page { get; init; } = new();
    public List<PrintControl> Controls { get; init; } = new();

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    public static PrintLayout LoadEmbedded(string kind)
    {
        var asm = typeof(PrintLayout).Assembly;
        var name = $"VTE.Api.Pdf.RequestPrint.Layouts.{kind}.json";
        using var s = asm.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException($"Embedded layout '{name}' not found");
        return JsonSerializer.Deserialize<PrintLayout>(s, JsonOpts)
            ?? throw new InvalidOperationException($"Layout '{kind}' deserialized to null");
    }
}

public sealed class PageInfo
{
    public string Unit { get; init; } = "TenthsOfAMillimeter";
    public int Width { get; init; } = 2101;   // 210.1mm = A4 portrait
    public int Height { get; init; } = 2969;  // 296.9mm = A4 portrait
    public string PaperKind { get; init; } = "A4";
}

public sealed class PrintControl
{
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = "Label";  // Label, CheckBox, Panel
    public double? X { get; init; }
    public double? Y { get; init; }
    public double? W { get; init; }
    public double? H { get; init; }
    public string? Text { get; init; }
    public string? Binding { get; init; }
    public bool Visible { get; init; } = true;
    public bool Multiline { get; init; }
    public string? FontName { get; init; }
    public double? FontSize { get; init; }
    public string FontStyle { get; init; } = "Regular";
    public string? Align { get; init; }
    public string? Borders { get; init; }
    public List<string> Children { get; init; } = new();
    public string? Parent { get; init; }
}
