namespace VTE.Domain.Geography;

public class Country
{
    public short Id { get; set; }
    public string? Name { get; set; }
    public string? ShortName { get; set; }
    public bool? Active { get; set; } = true;
}
