namespace VTE.Domain.Printing;

/// <summary>
/// A saved override layout for one print template, edited via the „Печатни обрасци"
/// admin screen. Global (station-wide) config — NOT tenant-owned: the legacy government
/// forms (Plav/Zelen/permissions/IDL) are the same paper for every company. Admin writes.
///
/// <see cref="LayoutJson"/> holds a per-field override map keyed by the template's stable
/// field keys, e.g. {"vin":{"x":123.4,"y":45.6,"size":9},"maker":{"y":50}}. Fields not
/// present keep the template's built-in default position — so a template renders correctly
/// even before anything is ever saved (empty map).
/// </summary>
public class PrintLayout
{
    /// <summary>Stable template code, e.g. "plav", "zelen", "idl-permit", "perm-cert".</summary>
    public string Code { get; set; } = "";

    /// <summary>Human-readable name shown in the admin list (Macedonian).</summary>
    public string Name { get; set; } = "";

    /// <summary>JSON override map: { fieldKey: { x?, y?, size? } }. Defaults to "{}".</summary>
    public string LayoutJson { get; set; } = "{}";

    public DateTime UpdatedAt { get; set; }
}
