namespace VTE.Core.Entities;

public class RolePrivilege : BaseEntity
{
    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public string EntityName { get; set; } = string.Empty;
    public bool CanCreate { get; set; }
    public bool CanRead { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
}
