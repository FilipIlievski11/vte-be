using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Reference;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// Communities CRUD matching the legacy uxCommunities form: Code, Name, RegistrationCode.
[ApiController]
[Route("api/communities")]
[Authorize]
public class CommunitiesController : ControllerBase
{
    private readonly VteDbContext _db;
    public CommunitiesController(VteDbContext db) => _db = db;

    public record CommunityDto(int Id, string? CommunityCode, string Name, string? RegistrationCode, bool IsActive);
    public record CommunityRequest(string? CommunityCode, string Name, string? RegistrationCode, bool IsActive);

    [HttpGet]
    public async Task<List<CommunityDto>> List()
        => await _db.Communities.AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new CommunityDto(c.Id, c.CommunityCode, c.Name, c.RegistrationCode, c.IsActive))
            .ToListAsync();

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<CommunityDto>> Create(CommunityRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var c = new Community { Name = req.Name.Trim(), CommunityCode = Trim(req.CommunityCode), RegistrationCode = Trim(req.RegistrationCode), IsActive = req.IsActive };
        _db.Communities.Add(c);
        await _db.SaveChangesAsync();
        return Ok(new CommunityDto(c.Id, c.CommunityCode, c.Name, c.RegistrationCode, c.IsActive));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, CommunityRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var c = await _db.Communities.FindAsync(id);
        if (c is null) return NotFound();
        c.Name = req.Name.Trim();
        c.CommunityCode = Trim(req.CommunityCode);
        c.RegistrationCode = Trim(req.RegistrationCode);
        c.IsActive = req.IsActive;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Communities.FindAsync(id);
        if (c is null) return NotFound();
        try { _db.Remove(c); await _db.SaveChangesAsync(); return NoContent(); }
        catch (DbUpdateException) { return BadRequest(new { error = "Cannot delete: community is referenced by other rows. Deactivate it instead." }); }
    }

    private static string? Trim(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
