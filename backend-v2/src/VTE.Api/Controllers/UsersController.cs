using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Identity;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// Administrator-only user management (operators + other administrators).
/// Distinct from /api/auth/* which handles the current caller's own session.
/// </summary>
[ApiController]
[Route("api/users")]
[Authorize(Roles = Roles.Administrator)]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly RoleManager<ApplicationRole> _roles;
    private readonly VteDbContext _db;

    public UsersController(
        UserManager<ApplicationUser> users,
        RoleManager<ApplicationRole> roles,
        VteDbContext db)
    {
        _users = users;
        _roles = roles;
        _db = db;
    }

    /// <summary>Paged list, with optional search (UserName/FullName/Email), companyId, role filters.
    /// Inactive users (historical/recreated legacy operators) are hidden by default — pass
    /// includeInactive=true to surface them for cleanup or audit.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedDto<UserListItem>>> List(
        [FromQuery] string? q = null,
        [FromQuery] byte? companyId = null,
        [FromQuery] string? role = null,
        [FromQuery] bool includeInactive = false,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 50;

        // Join AspNetUsers → AspNetUserRoles → AspNetRoles in one shot.
        var query =
            from u in _db.Users.AsNoTracking()
            join ur in _db.UserRoles on u.Id equals ur.UserId into urs
            from ur in urs.DefaultIfEmpty()
            join r in _db.Roles on ur.RoleId equals r.Id into rs
            from r in rs.DefaultIfEmpty()
            select new
            {
                u.Id, u.UserName, u.FullName, u.Email, u.CompanyId, u.IsActive, u.CreatedAt,
                Role = r.Name ?? ""
            };

        if (!includeInactive) query = query.Where(x => x.IsActive);
        if (companyId.HasValue) query = query.Where(x => x.CompanyId == companyId.Value);
        if (!string.IsNullOrWhiteSpace(role)) query = query.Where(x => x.Role == role);
        if (!string.IsNullOrWhiteSpace(q))
        {
            var s = q.Trim();
            query = query.Where(x =>
                EF.Functions.Like(x.UserName!, $"%{s}%") ||
                (x.FullName != null && EF.Functions.Like(x.FullName, $"%{s}%")) ||
                (x.Email    != null && EF.Functions.Like(x.Email!,    $"%{s}%")));
        }

        var total = await query.CountAsync();
        var rows = await query
            .OrderBy(x => x.UserName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Resolve company names in one batch lookup.
        var companyIds = rows.Where(r => r.CompanyId.HasValue).Select(r => r.CompanyId!.Value).Distinct().ToList();
        var companies = companyIds.Count == 0
            ? new Dictionary<byte, string>()
            : await _db.Companies.IgnoreQueryFilters().AsNoTracking()
                .Where(c => companyIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.Name);

        var items = rows.Select(x => new UserListItem(
            x.Id, x.UserName!, x.FullName, x.Email, x.CompanyId,
            x.CompanyId.HasValue ? companies.GetValueOrDefault(x.CompanyId.Value) : null,
            x.Role, x.IsActive, x.CreatedAt)).ToList();

        return Ok(new PagedDto<UserListItem>(page, pageSize, total, items));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserListItem>> Get(string id)
    {
        var u = await _users.FindByIdAsync(id);
        if (u == null) return NotFound();
        var userRoles = await _users.GetRolesAsync(u);
        var companyName = u.CompanyId.HasValue
            ? await _db.Companies.IgnoreQueryFilters().AsNoTracking()
                .Where(c => c.Id == u.CompanyId.Value)
                .Select(c => c.Name).FirstOrDefaultAsync()
            : null;
        return Ok(new UserListItem(
            u.Id, u.UserName!, u.FullName, u.Email, u.CompanyId, companyName,
            userRoles.FirstOrDefault() ?? "", u.IsActive, u.CreatedAt));
    }

    [HttpPost]
    public async Task<ActionResult<UserListItem>> Create([FromBody] UserCreateRequest req)
    {
        if (!await _roles.RoleExistsAsync(req.Role))
            return BadRequest(new { error = $"Unknown role '{req.Role}'." });

        if (await _users.FindByNameAsync(req.UserName) != null)
            return Conflict(new { error = "User name already taken." });

        if (req.CompanyId.HasValue && !await _db.Companies.AnyAsync(c => c.Id == req.CompanyId.Value))
            return BadRequest(new { error = $"Company {req.CompanyId.Value} does not exist." });

        var user = new ApplicationUser
        {
            UserName = req.UserName,
            Email = req.Email,
            EmailConfirmed = true,
            FullName = req.FullName,
            CompanyId = req.CompanyId,
            IsActive = true,
        };

        var created = await _users.CreateAsync(user, req.Password);
        if (!created.Succeeded)
            return BadRequest(new { error = string.Join("; ", created.Errors.Select(e => e.Description)) });

        var added = await _users.AddToRoleAsync(user, req.Role);
        if (!added.Succeeded)
            return BadRequest(new { error = string.Join("; ", added.Errors.Select(e => e.Description)) });

        var companyName = user.CompanyId.HasValue
            ? await _db.Companies.IgnoreQueryFilters()
                .Where(c => c.Id == user.CompanyId.Value)
                .Select(c => c.Name).FirstOrDefaultAsync()
            : null;

        return CreatedAtAction(nameof(Get), new { id = user.Id }, new UserListItem(
            user.Id, user.UserName!, user.FullName, user.Email, user.CompanyId, companyName,
            req.Role, user.IsActive, user.CreatedAt));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UserUpdateRequest req)
    {
        var u = await _users.FindByIdAsync(id);
        if (u == null) return NotFound();

        if (req.CompanyId.HasValue && !await _db.Companies.AnyAsync(c => c.Id == req.CompanyId.Value))
            return BadRequest(new { error = $"Company {req.CompanyId.Value} does not exist." });

        if (req.FullName != null) u.FullName = req.FullName;
        if (req.Email    != null) u.Email    = req.Email;
        if (req.CompanyId.HasValue) u.CompanyId = req.CompanyId;
        if (req.IsActive.HasValue)  u.IsActive  = req.IsActive.Value;

        var result = await _users.UpdateAsync(u);
        if (!result.Succeeded)
            return BadRequest(new { error = string.Join("; ", result.Errors.Select(e => e.Description)) });

        return NoContent();
    }

    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(string id, [FromBody] ResetPasswordRequest req)
    {
        var u = await _users.FindByIdAsync(id);
        if (u == null) return NotFound();

        var token = await _users.GeneratePasswordResetTokenAsync(u);
        var result = await _users.ResetPasswordAsync(u, token, req.NewPassword);
        if (!result.Succeeded)
            return BadRequest(new { error = string.Join("; ", result.Errors.Select(e => e.Description)) });

        return NoContent();
    }

    /// <summary>Soft-delete: sets IsActive=false. The row stays so audit trails / FKs remain intact.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var u = await _users.FindByIdAsync(id);
        if (u == null) return NotFound();
        u.IsActive = false;
        await _users.UpdateAsync(u);
        return NoContent();
    }
}
