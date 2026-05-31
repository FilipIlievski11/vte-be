using Microsoft.AspNetCore.Identity;

namespace VTE.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    /// <summary>Tenant the operator is bound to. Null for system Administrators.</summary>
    public byte? CompanyId { get; set; }

    public string? FullName { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
