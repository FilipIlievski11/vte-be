using Microsoft.AspNetCore.Identity;

namespace VTE.Domain.Identity;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() { }
    public ApplicationRole(string name) : base(name) { }
}

public static class Roles
{
    public const string Administrator = "Administrator";
    public const string Operator = "Operator";
}
