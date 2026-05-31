namespace VTE.Domain.Vehicles;

/// <summary>Euro emission standard (Euro 0..6, …). Legacy table: VehicleEngineEcoProgram.</summary>
public class VehicleEcoProgram
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
