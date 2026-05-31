namespace VTE.Domain.Vehicles;

public class CustomerVehicleRelation
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public long VehicleId { get; set; }
    public int? RelationTypeId { get; set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public string? Note { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
