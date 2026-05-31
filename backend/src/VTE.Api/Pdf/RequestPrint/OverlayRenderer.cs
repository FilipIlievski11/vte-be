using System.Globalization;
using System.Reflection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace VTE.Api.Pdf.RequestPrint;

// Pixel-faithful overlay renderer:
//   - Reads PrintLayout JSON (parsed from legacy DevExpress XtraReport designer)
//   - Pulls values for each binding from PrintRequestData via reflection
//   - Applies per-kind show/hide overrides from ConditionEngine
//   - Emits a QuestPDF Document that places every visible Label/CheckBox at the
//     exact mm coordinate of the legacy report (TenthsOfAMillimeter / 10).
//
// Margins map to the legacy objOpcii.{Plav,Bel,Zelen}{Top,Left,Right,Bottom}Margin —
// passed in by the controller (per-station settings) and added to the layout x/y
// so the overlay aligns with the physical pre-printed stationery.
public sealed class OverlayRenderer
{
    public enum Kind { Plav, Bel, Zelen }

    public byte[] Render(Kind kind, PrintRequestData data, OverlayMargins margins)
    {
        var layout = PrintLayout.LoadEmbedded(kind.ToString().ToLowerInvariant());
        var overrides = kind switch
        {
            Kind.Plav  => ConditionEngine.Plav(data),
            Kind.Bel   => ConditionEngine.Bel(data),
            Kind.Zelen => ConditionEngine.Zelen(data),
            _ => new Dictionary<string, bool>(),
        };

        // Build absolute positions by walking the panel hierarchy.
        var abs = ComputeAbsolutePositions(layout);

        return Document.Create(c => c.Page(p =>
        {
            p.Size(PageSizes.A4);
            p.Margin(0); // we honor the legacy margins by offsetting each element
            p.DefaultTextStyle(t => t.FontFamily("Arial").FontSize(9.75f));
            p.PageColor(Colors.White);

            p.Content().Layers(layers =>
            {
                layers.PrimaryLayer().Container(); // empty base layer fills page

                foreach (var ctrl in layout.Controls)
                {
                    if (!IsEffectivelyVisible(ctrl, overrides)) continue;
                    if (ctrl.Type == "Panel") continue;
                    if (!abs.TryGetValue(ctrl.Name, out var pos)) continue;
                    if (pos.W <= 0 || pos.H <= 0) continue;

                    var xMm = (float)(pos.X / 10.0 + margins.LeftMm);
                    var yMm = (float)(pos.Y / 10.0 + margins.TopMm);
                    var wMm = (float)(pos.W / 10.0);
                    var hMm = (float)(pos.H / 10.0);
                    var text = ResolveText(ctrl, data);
                    var size = (float)(ctrl.FontSize ?? 9.75);
                    var fontName = ctrl.FontName ?? "Arial";
                    var bold = IsBold(ctrl.FontStyle);
                    var italic = IsItalic(ctrl.FontStyle);
                    var align = ctrl.Align;
                    var isCheck = ctrl.Type == "CheckBox";

                    layers.Layer().AlignTop().AlignLeft()
                          .PaddingLeft(xMm, Unit.Millimetre).PaddingTop(yMm, Unit.Millimetre)
                          .Width(wMm, Unit.Millimetre).Height(hMm, Unit.Millimetre)
                          .Element(box =>
                          {
                              var aligned = ApplyAlignment(box, align);
                              aligned.Text(t =>
                              {
                                  t.DefaultTextStyle(s =>
                                  {
                                      s = s.FontFamily(fontName).FontSize(size);
                                      if (bold)   s = s.Bold();
                                      if (italic) s = s.Italic();
                                      return s;
                                  });
                                  t.Span(isCheck ? "X" : text);
                              });
                          });
                }
            });
        })).GeneratePdf();
    }

    private static bool IsEffectivelyVisible(PrintControl c, IReadOnlyDictionary<string, bool> overrides)
    {
        if (overrides.TryGetValue(c.Name, out var ov)) return ov;
        if (c.Type == "CheckBox" && !overrides.ContainsKey(c.Name)) return false; // legacy default-hides checkboxes
        return c.Visible;
    }

    private static string ResolveText(PrintControl c, PrintRequestData data)
    {
        if (c.Type == "CheckBox") return "X";
        if (!string.IsNullOrEmpty(c.Binding))
        {
            var val = ReadByPath(data, c.Binding);
            return val ?? string.Empty;
        }
        // Designer placeholder text starts with the control name; collapse to blank.
        var t = c.Text ?? string.Empty;
        if (string.Equals(t, c.Name, StringComparison.Ordinal)) return string.Empty;
        return t;
    }

    private static string? ReadByPath(object obj, string path)
    {
        object? cur = obj;
        foreach (var seg in path.Split('.'))
        {
            if (cur is null) return null;
            var p = cur.GetType().GetProperty(seg, BindingFlags.Public | BindingFlags.Instance);
            if (p is null) return null;
            cur = p.GetValue(cur);
        }
        return cur switch
        {
            null        => string.Empty,
            string s    => s,
            DateOnly d  => d.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
            DateTime dt => dt.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
            IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
            _ => cur.ToString(),
        };
    }

    private static Dictionary<string, AbsPos> ComputeAbsolutePositions(PrintLayout layout)
    {
        var byName = layout.Controls.ToDictionary(c => c.Name, StringComparer.Ordinal);
        var result = new Dictionary<string, AbsPos>(StringComparer.Ordinal);
        foreach (var c in layout.Controls)
        {
            var x = 0.0; var y = 0.0;
            var node = c;
            while (node != null)
            {
                x += node.X ?? 0;
                y += node.Y ?? 0;
                if (node.Parent is null || node.Parent == "Detail") break;
                if (!byName.TryGetValue(node.Parent, out var parent)) break;
                node = parent;
            }
            result[c.Name] = new AbsPos(x, y, c.W ?? 0, c.H ?? 0);
        }
        return result;
    }

    private static bool IsBold(string s)    => s.Contains("Bold",   StringComparison.OrdinalIgnoreCase);
    private static bool IsItalic(string s)  => s.Contains("Italic", StringComparison.OrdinalIgnoreCase);

    // Translates a DevExpress TextAlignment (e.g. "MiddleLeft", "TopRight", "BottomCenter")
    // into the corresponding QuestPDF AlignTop/AlignMiddle/.../AlignLeft/Center/Right chain.
    private static IContainer ApplyAlignment(IContainer box, string? align)
    {
        var vert = box;
        if (align?.StartsWith("Top",    StringComparison.OrdinalIgnoreCase) == true)      vert = box.AlignTop();
        else if (align?.StartsWith("Bottom", StringComparison.OrdinalIgnoreCase) == true) vert = box.AlignBottom();
        else                                                                              vert = box.AlignMiddle();

        if (align?.EndsWith("Right",  StringComparison.OrdinalIgnoreCase) == true)  return vert.AlignRight();
        if (align?.EndsWith("Center", StringComparison.OrdinalIgnoreCase) == true)  return vert.AlignCenter();
        return vert.AlignLeft();
    }

    private readonly record struct AbsPos(double X, double Y, double W, double H);
}

public sealed record OverlayMargins(double TopMm, double LeftMm, double RightMm, double BottomMm)
{
    public static OverlayMargins Zero { get; } = new(0, 0, 0, 0);
}
