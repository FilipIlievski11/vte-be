namespace VTE.Domain.Vehicles;

public class VehicleMaker
{
    public int Id { get; set; }
    public short? CountryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Trademark { get; set; }
    public bool Active { get; set; } = true;
}
