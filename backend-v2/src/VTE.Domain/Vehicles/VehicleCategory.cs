namespace VTE.Domain.Vehicles;

public class VehicleCategory
{
    public short Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
