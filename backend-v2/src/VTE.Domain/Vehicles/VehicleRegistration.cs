namespace VTE.Domain.Vehicles;

/// <summary>One row per registration cycle of a Vehicle (legacy Vehicle.Registrations).
/// Tenant scoping is inherited from the parent Vehicle.</summary>
public class VehicleRegistration
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public byte? IssuerId { get; set; }               // → DocumentIssuer (NULL: legacy rows with no issuer recorded)
    public string PlateNumber { get; set; } = string.Empty;
    public DateTime RegisteredDate { get; set; }
    public DateTime ValidUntil { get; set; }
    public bool IsFirstRegistration { get; set; }
    public bool Active { get; set; } = true;
}
