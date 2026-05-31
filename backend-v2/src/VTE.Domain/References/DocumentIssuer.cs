namespace VTE.Domain.References;

public class DocumentIssuer
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
