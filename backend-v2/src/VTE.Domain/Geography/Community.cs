namespace VTE.Domain.Geography;

public class Community
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public short CountryId { get; set; }
    public string? Code { get; set; }
    public string? PlateNumberPrefix { get; set; }
    public bool? Active { get; set; } = true;
}
