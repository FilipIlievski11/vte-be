namespace VTE.Core.Entities;

public class Permission : BaseEntity
{
    public long DocumentId { get; set; }
    public Document Document { get; set; } = null!;
    public string PermissionNumber { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime? ValidUntilDate { get; set; }
    public string? Note { get; set; }
}
