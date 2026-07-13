using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Identity;
using VTE.Domain.Printing;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// Saved print-template layouts (<see cref="PrintLayout"/>) — the per-field position/font
/// overrides edited on the „Печатни обрасци" admin screen. Global config (not tenant-owned):
/// the government forms are the same paper for every station.
/// Reads: any operator (each print view fetches its own saved layout on open).
/// Writes: Administrator only.
/// </summary>
[ApiController]
[Route("api/print-layouts")]
[Authorize]
public class PrintLayoutsController : ControllerBase
{
    private readonly VteDbContext _db;
    public PrintLayoutsController(VteDbContext db) => _db = db;

    public record PrintLayoutRow(string Code, string Name, DateTime UpdatedAt);
    public record PrintLayoutDto(string Code, string Name, string LayoutJson, DateTime? UpdatedAt);
    public record PrintLayoutWriteDto(string? Name, string LayoutJson);

    /// <summary>All saved layouts (for the admin list's „изменето" column). Templates with
    /// no saved override simply don't appear here — the FE knows the full catalog.</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PrintLayoutRow>>> List()
    {
        var rows = await _db.PrintLayouts.AsNoTracking()
            .OrderBy(x => x.Code)
            .Select(x => new PrintLayoutRow(x.Code, x.Name, x.UpdatedAt))
            .ToListAsync();
        return Ok(rows);
    }

    /// <summary>The saved override map for one template. Returns an EMPTY map (not 404) when
    /// nothing has been saved yet, so print views always get a usable response.</summary>
    [HttpGet("{code}")]
    public async Task<ActionResult<PrintLayoutDto>> Get(string code)
    {
        code = (code ?? "").Trim().ToLowerInvariant();
        var e = await _db.PrintLayouts.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code);
        if (e == null) return Ok(new PrintLayoutDto(code, "", "{}", null));
        return Ok(new PrintLayoutDto(e.Code, e.Name, e.LayoutJson, e.UpdatedAt));
    }

    /// <summary>Upsert the override map for a template.</summary>
    [HttpPut("{code}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Save(string code, [FromBody] PrintLayoutWriteDto dto)
    {
        code = (code ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(code) || code.Length > 40)
            return BadRequest(new { error = "Невалиден код на образец." });

        var json = string.IsNullOrWhiteSpace(dto.LayoutJson) ? "{}" : dto.LayoutJson.Trim();
        try { using var _ = JsonDocument.Parse(json); }
        catch { return BadRequest(new { error = "Невалиден JSON за распоред." }); }

        var e = await _db.PrintLayouts.FirstOrDefaultAsync(x => x.Code == code);
        if (e == null)
        {
            e = new PrintLayout { Code = code };
            _db.PrintLayouts.Add(e);
        }
        e.Name = string.IsNullOrWhiteSpace(dto.Name) ? (string.IsNullOrEmpty(e.Name) ? code : e.Name) : dto.Name.Trim();
        e.LayoutJson = json;
        e.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Reset a template to its built-in defaults (drops the saved override row).</summary>
    [HttpDelete("{code}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Reset(string code)
    {
        code = (code ?? "").Trim().ToLowerInvariant();
        var e = await _db.PrintLayouts.FirstOrDefaultAsync(x => x.Code == code);
        if (e != null) { _db.PrintLayouts.Remove(e); await _db.SaveChangesAsync(); }
        return NoContent();
    }
}
