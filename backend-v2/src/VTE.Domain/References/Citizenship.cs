namespace VTE.Domain.References;

public class Citizenship
{
    public byte Id { get; set; }
    public short CountryId { get; set; }
    public string Name { get; set; } = string.Empty;
}
