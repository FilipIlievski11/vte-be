using System.Text.Json;
using VTE.Domain.Common;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Services;

/// <summary>
/// Writes financial-audit rows. Log() only ADDS to the current DbContext — the
/// caller's SaveChangesAsync persists the audit row together with the mutation
/// itself (atomic: no change without its audit row and vice versa). For created
/// entities whose id is generated on save, log after the first save and save again.
/// </summary>
public class AuditLogger
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    private readonly IHttpContextAccessor _http;

    public AuditLogger(VteDbContext db, ITenantContext tenant, IHttpContextAccessor http)
    {
        _db = db; _tenant = tenant; _http = http;
    }

    public void Log(byte companyId, string action, string entityType, long entityId,
        string summary, object? details = null)
    {
        _db.AuditEntries.Add(new AuditEntry
        {
            CompanyId = companyId,
            AtUtc = DateTime.UtcNow,
            UserId = _tenant.UserId,
            UserName = _http.HttpContext?.User?.Identity?.Name,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Summary = summary.Length > 500 ? summary[..500] : summary,
            DetailsJson = details == null ? null : JsonSerializer.Serialize(details),
        });
    }
}
