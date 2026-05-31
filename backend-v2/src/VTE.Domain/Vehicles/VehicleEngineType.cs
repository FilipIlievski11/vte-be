namespace VTE.Domain.Vehicles;

/// <summary>Engine type / code (7k+ rows in legacy — kept normalized).</summary>
public class VehicleEngineType
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
