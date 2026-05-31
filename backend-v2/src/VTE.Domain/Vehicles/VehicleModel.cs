namespace VTE.Domain.Vehicles;

public class VehicleModel
{
    public int Id { get; set; }
    public int MakerId { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? ProductionStart { get; set; }
    public DateTime? ProductionEnd { get; set; }
    public bool Active { get; set; } = true;
}
