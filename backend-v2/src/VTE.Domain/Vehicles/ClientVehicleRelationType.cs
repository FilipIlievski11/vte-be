namespace VTE.Domain.Vehicles;

/// <summary>How a Client relates to a Vehicle. Three legacy values:
/// 1 = Owner ("сопственик"), 2 = Authorized ("полномошно лице"),
/// 3 = Client-only (no vehicle bound).</summary>
public class ClientVehicleRelationType
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsOwner { get; set; }
    public bool IsAuthorized { get; set; }
    public bool IsCustomerOnly { get; set; }
    public string? Description { get; set; }
    public bool Active { get; set; } = true;
}
