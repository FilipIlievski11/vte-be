namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class VehicleTyre : BaseEntity
{
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public long TireTypeId { get; set; }
    public TireType TireType { get; set; } = null!;
    public int AxleNumber { get; set; }
}
