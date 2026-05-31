using System.Security.Claims;
using VTE.Domain;
using VTE.Infrastructure;

namespace VTE.Api.Tenancy;

// Resolves the tenant (StationId) from the JWT claims of the current request.
// Administrators have IsAdministrator = true and StationId = null (cross-tenant).
// Operators have IsAdministrator = false and StationId set.
public class TenantContext : ITenantContext
{
    public TenantContext(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            IsAdministrator = user.IsInRole(Roles.Administrator);
            var stationClaim = user.FindFirst(VteClaims.StationId)?.Value;
            if (int.TryParse(stationClaim, out var stationId))
            {
                StationId = stationId;
            }
        }
    }

    public int? StationId { get; }
    public bool IsAdministrator { get; }
}
