namespace VTE.Domain.Identity;

// Per-station operator. PK = AspNetUsers.Id (1:1 with ApplicationUser).
// StationId is the tenant binding for the Operator role.
// Administrators do NOT have an Operator row.
public class Operator
{
    public string UserId { get; set; } = string.Empty;
    public int? StationId { get; set; }                   // null permitted for Administrators (defensive)
    public string FullName { get; set; } = string.Empty;
    public string? EMBG { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public ApplicationUser? User { get; set; }
}
