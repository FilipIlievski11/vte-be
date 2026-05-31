using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Identity;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/operators")]
[Authorize(Roles = Roles.Administrator)]
public class OperatorsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly VteDbContext _db;

    public OperatorsController(UserManager<ApplicationUser> users, VteDbContext db)
    {
        _users = users;
        _db = db;
    }

    public record OperatorDto(string UserId, string UserName, string? Email, int? StationId, string FullName, bool IsActive);
    public record CreateOperatorRequest(string UserName, string? Email, string Password, int StationId, string FullName, string? EMBG);

    [HttpGet]
    public async Task<ActionResult<List<OperatorDto>>> List([FromQuery] int? stationId)
        => await (
            from o in _db.Operators
            join u in _db.Users on o.UserId equals u.Id
            where stationId == null || o.StationId == stationId
            orderby o.FullName
            select new OperatorDto(o.UserId, u.UserName ?? string.Empty, u.Email, o.StationId, o.FullName, o.IsActive)
        ).ToListAsync();

    [HttpPost]
    public async Task<ActionResult<OperatorDto>> Create(CreateOperatorRequest req)
    {
        if (!await _db.Stations.AnyAsync(s => s.Id == req.StationId))
            return BadRequest(new { error = "Station not found." });

        var user = new ApplicationUser { UserName = req.UserName, Email = req.Email, EmailConfirmed = true };
        var createResult = await _users.CreateAsync(user, req.Password);
        if (!createResult.Succeeded)
            return BadRequest(new { errors = createResult.Errors.Select(e => e.Description) });

        await _users.AddToRoleAsync(user, Roles.Operator);

        var op = new Operator { UserId = user.Id, StationId = req.StationId, FullName = req.FullName, EMBG = req.EMBG };
        _db.Operators.Add(op);
        await _db.SaveChangesAsync();

        return Ok(new OperatorDto(user.Id, user.UserName!, user.Email, op.StationId, op.FullName, op.IsActive));
    }

    [HttpPost("{userId}/deactivate")]
    public async Task<IActionResult> Deactivate(string userId)
    {
        var op = await _db.Operators.FindAsync(userId);
        if (op is null) return NotFound();
        op.IsActive = false;

        var user = await _users.FindByIdAsync(userId);
        if (user is not null) await _users.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

        await _db.SaveChangesAsync();
        return NoContent();
    }
}
