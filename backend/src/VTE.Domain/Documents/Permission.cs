namespace VTE.Domain.Documents;

public class Permission
{
    public long Id { get; set; }
    public int StationId { get; set; }
    public long CustomerVehicleRelationId { get; set; }
    public string? PermissionNumber { get; set; }
    public int? PermissionTypeId { get; set; }
    public DateOnly MadeDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Note { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class InternationalDrivingLicence
{
    public long Id { get; set; }
    public int StationId { get; set; }
    public long CustomerId { get; set; }
    public string LicenceNumber { get; set; } = string.Empty;
    public DateOnly IssuedDate { get; set; }
    public DateOnly ValidTill { get; set; }
    public int? IssuerId { get; set; }
    public string? Note { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
