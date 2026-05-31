namespace VTE.Core.Entities;

public class VehicleAxleDistance : BaseEntity
{
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public int FromAxle { get; set; }
    public int ToAxle { get; set; }
    public decimal DistanceMM { get; set; }
}
