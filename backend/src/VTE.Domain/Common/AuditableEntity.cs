namespace VTE.Domain.Common;

public abstract class AuditableEntity
{
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public string? CreatedByUserId { get; set; }
    public string? LastModifiedByUserId { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
