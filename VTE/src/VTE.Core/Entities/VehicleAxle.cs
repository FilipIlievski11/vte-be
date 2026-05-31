namespace VTE.Core.Entities;

public class VehicleAxle : BaseEntity
{
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public int AxleNumber { get; set; }
    public decimal? MaxWeightKG { get; set; }
}
