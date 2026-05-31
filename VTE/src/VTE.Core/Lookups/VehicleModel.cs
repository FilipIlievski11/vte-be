namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class VehicleModel : LookupEntity
{
    public long VehicleMakerId { get; set; }
    public VehicleMaker VehicleMaker { get; set; } = null!;
}
