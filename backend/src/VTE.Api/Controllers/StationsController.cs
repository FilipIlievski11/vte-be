using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Stations;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/stations")]
[Authorize(Roles = Roles.Administrator)]   // only Administrators manage stations
public class StationsController : ControllerBase
{
    private readonly VteDbContext _db;

    public StationsController(VteDbContext db) => _db = db;

    public record StationDto(int Id, string Name, string Code, bool IsActive);
    public record CreateStationRequest(string Name, string Code);
    public record UpdateStationRequest(string Name, string Code, bool IsActive);

    [HttpGet]
    public async Task<ActionResult<List<StationDto>>> List()
        => await _db.Stations
            .OrderBy(s => s.Name)
            .Select(s => new StationDto(s.Id, s.Name, s.Code, s.IsActive))
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StationDto>> Get(int id)
    {
        var s = await _db.Stations.FindAsync(id);
        return s is null
            ? NotFound()
            : Ok(new StationDto(s.Id, s.Name, s.Code, s.IsActive));
    }

    [HttpPost]
    public async Task<ActionResult<StationDto>> Create(CreateStationRequest req)
    {
        var s = new Station { Name = req.Name, Code = req.Code };
        _db.Stations.Add(s);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = s.Id }, new StationDto(s.Id, s.Name, s.Code, s.IsActive));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateStationRequest req)
    {
        var s = await _db.Stations.FindAsync(id);
        if (s is null) return NotFound();
        s.Name = req.Name;
        s.Code = req.Code;
        s.IsActive = req.IsActive;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
