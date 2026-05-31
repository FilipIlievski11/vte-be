namespace VTE.Core.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<RolePrivilege> Privileges { get; set; } = [];
}
