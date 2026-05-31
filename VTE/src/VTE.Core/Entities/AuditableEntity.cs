namespace VTE.Core.Entities;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long CreatedByUserId { get; set; }
    public long? ModifiedByUserId { get; set; }
}
