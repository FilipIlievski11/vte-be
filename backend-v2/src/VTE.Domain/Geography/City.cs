namespace VTE.Domain.Geography;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CommunityId { get; set; }
    public string PostalCode { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
