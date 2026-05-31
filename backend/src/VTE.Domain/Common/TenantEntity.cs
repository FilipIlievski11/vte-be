namespace VTE.Domain.Common;

public abstract class TenantEntity : AuditableEntity
{
    public int StationId { get; set; }
    public bool IsActive { get; set; } = true;
}
