using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// Admin-only read API over the financial audit log (<see cref="VTE.Domain.Common.AuditEntry"/>).
/// Rows are written by AuditLogger at every money-touching mutation — see the call
/// sites in PaymentDocumentsController / CustomerDebtsController.
/// </summary>
[ApiController]
[Route("api/admin/audit")]
[Authorize(Roles = "Administrator")]
public class AuditController : ControllerBase
{
    private readonly VteDbContext _db;
    public AuditController(VteDbContext db) => _db = db;

    public record AuditRow(
        long Id, DateTime AtUtc, string? UserName, string Action,
        string EntityType, long EntityId, string Summary, string? DetailsJson);

    [HttpGet]
    public async Task<ActionResult<PagedDto<AuditRow>>> List(
        [FromQuery] string? action = null,
        [FromQuery] string? q = null,
        [FromQuery] long? entityId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 50;

        var query = _db.AuditEntries.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(action))
            query = query.Where(a => a.Action.StartsWith(action));
        if (entityId.HasValue)
            query = query.Where(a => a.EntityId == entityId.Value);
        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(a =>
                a.Summary.Contains(term) ||
                (a.UserName != null && a.UserName.Contains(term)));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(a => a.AtUtc).ThenByDescending(a => a.Id)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(a => new AuditRow(a.Id, a.AtUtc, a.UserName, a.Action,
                a.EntityType, a.EntityId, a.Summary, a.DetailsJson))
            .ToListAsync();

        return Ok(new PagedDto<AuditRow>(page, pageSize, total, items));
    }
}
