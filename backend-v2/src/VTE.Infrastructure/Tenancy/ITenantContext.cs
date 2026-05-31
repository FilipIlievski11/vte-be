namespace VTE.Infrastructure.Tenancy;

/// <summary>
/// Resolves the current tenant (Company) for the in-flight request.
/// Implemented in the Api layer over HttpContextAccessor; passed to the DbContext
/// so global query filters can scope tenant-owned entities by CompanyId.
/// </summary>
public interface ITenantContext
{
    /// <summary>CompanyId from the JWT/cookie claim, or null for Administrators / unauthenticated.</summary>
    byte? CompanyId { get; }

    /// <summary>True when the caller has the Administrator role (bypasses tenant filters).</summary>
    bool IsAdmin { get; }

    /// <summary>Current user id (AspNetUsers.Id), or null when unauthenticated.</summary>
    string? UserId { get; }
}

/// <summary>Inert tenant context used at design time (migrations) and in seeding.</summary>
public sealed class NullTenantContext : ITenantContext
{
    public byte? CompanyId => null;
    public bool IsAdmin => true;
    public string? UserId => null;
}
