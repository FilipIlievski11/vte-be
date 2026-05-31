namespace VTE.Domain.Requests;

/// <summary>
/// Print template tied to a <see cref="RequestType"/>. Legacy app shipped three
/// coloured paper forms (Plav / Bel / Zelen). In v2 each row in this table
/// declares which PDF template renders when an operator hits Print.
/// </summary>
public class RequestDocumentPrint
{
    public byte Id { get; set; }
    public string Code { get; set; } = string.Empty;     // PLAV / BEL / ZELEN
    public string Name { get; set; } = string.Empty;     // Macedonian display label
    public string? TemplatePath { get; set; }            // relative path to the PDF template
    public bool Active { get; set; } = true;
}
