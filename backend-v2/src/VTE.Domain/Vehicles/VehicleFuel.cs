namespace VTE.Domain.Vehicles;

/// <summary>Fuel / engine power source (petrol, diesel, LPG, electric, hybrid, ...).
/// Legacy table: VehicleEnginePowerSourceTypes.</summary>
public class VehicleFuel
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
