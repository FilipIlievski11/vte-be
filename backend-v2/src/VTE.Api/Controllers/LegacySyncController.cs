using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VTE.Api.Services;

namespace VTE.Api.Controllers;

/// <summary>
/// Admin-only "Sync from legacy" endpoint — a thin wrapper over
/// <see cref="LegacySyncService"/> (shared with the in-process scheduler
/// <see cref="LegacySyncScheduler"/>, which runs the same sync server-side on a
/// timetable). Status additionally reports the schedule and the last run.
/// </summary>
[ApiController]
[Route("api/admin/legacy-sync")]
[Authorize(Roles = "Administrator")]
public class LegacySyncController : ControllerBase
{
    private readonly LegacySyncService _sync;
    private readonly LegacySyncState _state;
    private readonly IConfiguration _cfg;

    public LegacySyncController(LegacySyncService sync, LegacySyncState state, IConfiguration cfg)
    {
        _sync = sync; _state = state; _cfg = cfg;
    }

    public record LastRunDto(
        DateTime? StartedAtUtc, DateTime? FinishedAtUtc, bool? Succeeded,
        string? Trigger, string? Error, LegacySyncResult? Result);

    public record SyncStatus(
        bool Enabled, long MaxClientId, long MaxVehicleId, long MaxRequestId,
        long Clients, long Vehicles, long Requests, long TechnicalExamReports,
        string? AutoTimesUtc, bool Running, LastRunDto? LastRun);

    /// <summary>Current high-water marks + schedule + last run, shown on the admin page.</summary>
    [HttpGet("status")]
    public async Task<ActionResult<SyncStatus>> Status()
    {
        var row = await _sync.ReadRowAsync(@"SELECT
            (SELECT ISNULL(MAX(Id),0) FROM dbo.Client),
            (SELECT ISNULL(MAX(Id),0) FROM dbo.Vehicle),
            (SELECT ISNULL(MAX(Id),0) FROM dbo.Request),
            (SELECT COUNT_BIG(*) FROM dbo.Client),
            (SELECT COUNT_BIG(*) FROM dbo.Vehicle),
            (SELECT COUNT_BIG(*) FROM dbo.Request),
            (SELECT COUNT_BIG(*) FROM dbo.TechnicalExamReport)");

        var lastRun = _state.LastStartedAtUtc == null ? null : new LastRunDto(
            _state.LastStartedAtUtc, _state.LastFinishedAtUtc, _state.LastSucceeded,
            _state.LastTrigger, _state.LastError, _state.LastResult);

        return new SyncStatus(
            _sync.Enabled, row[0], row[1], row[2], row[3], row[4], row[5], row[6],
            _cfg["LegacySync:AutoTimesUtc"], _state.Gate.CurrentCount == 0, lastRun);
    }

    /// <summary>Runs the incremental top-up and returns the per-table new-row counts.</summary>
    [HttpPost]
    public async Task<ActionResult<LegacySyncResult>> Sync()
    {
        try
        {
            return await _sync.RunAsync("manual", HttpContext.RequestAborted);
        }
        catch (InvalidOperationException ex)
        {
            // Disabled, or a run is already in progress.
            return Problem(ex.Message, statusCode: 400);
        }
        catch (Exception ex)
        {
            return Problem("Legacy sync failed: " + ex.Message, statusCode: 500);
        }
    }
}
