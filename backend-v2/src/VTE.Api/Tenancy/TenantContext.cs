using System.Security.Claims;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Tenancy;

public class TenantContext : ITenantContext
{
    public const string CompanyIdClaim = "companyId";

    private readonly IHttpContextAccessor _http;

    public TenantContext(IHttpContextAccessor http) => _http = http;

    public byte? CompanyId
    {
        get
        {
            var raw = _http.HttpContext?.User?.FindFirst(CompanyIdClaim)?.Value;
            return byte.TryParse(raw, out var v) ? v : (byte?)null;
        }
    }

    public bool IsAdmin =>
        _http.HttpContext?.User?.IsInRole("Administrator") ?? false;

    public string? UserId =>
        _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
