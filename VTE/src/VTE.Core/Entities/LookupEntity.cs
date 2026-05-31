namespace VTE.Core.Entities;

public abstract class LookupEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
