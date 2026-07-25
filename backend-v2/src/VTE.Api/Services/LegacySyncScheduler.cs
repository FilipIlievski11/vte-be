namespace VTE.Api.Services;

/// <summary>
/// In-process scheduler for the legacy sync — runs ON THE SERVER, so the daily
/// sync happens regardless of whether anyone's laptop is on. Fire times come from
/// `LegacySync:AutoTimesUtc` (comma-separated 24h "HH:mm" values, UTC — the prod
/// host runs UTC; Macedonia is UTC+1/+2). Idle when the list is empty or the sync
/// is disabled, so local dev (which usually leaves AutoTimesUtc unset) is unaffected.
///
/// Failure policy: one retry 10 minutes after a failed slot (the live legacy server
/// occasionally drops), then wait for the next slot. Overlap with a manual run is
/// impossible — LegacySyncState.Gate refuses the second entrant.
/// </summary>
public class LegacySyncScheduler : BackgroundService
{
    private readonly IServiceScopeFactory _scopes;
    private readonly IConfiguration _cfg;
    private readonly ILogger<LegacySyncScheduler> _log;

    public LegacySyncScheduler(IServiceScopeFactory scopes, IConfiguration cfg, ILogger<LegacySyncScheduler> log)
    {
        _scopes = scopes; _cfg = cfg; _log = log;
    }

    internal static List<TimeOnly> ParseTimes(string? raw) =>
        (raw ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => TimeOnly.TryParseExact(s, "H:mm", out var t) ? t : (TimeOnly?)null)
            .Where(t => t.HasValue)
            .Select(t => t!.Value)
            .Distinct()
            .OrderBy(t => t)
            .ToList();

    /// <summary>Next UTC occurrence strictly after <paramref name="nowUtc"/>.</summary>
    internal static DateTime NextOccurrence(DateTime nowUtc, IReadOnlyList<TimeOnly> times)
    {
        var today = DateOnly.FromDateTime(nowUtc);
        foreach (var t in times)
        {
            var candidate = today.ToDateTime(t, DateTimeKind.Utc);
            if (candidate > nowUtc) return candidate;
        }
        return today.AddDays(1).ToDateTime(times[0], DateTimeKind.Utc);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var enabled = _cfg.GetValue("LegacySync:Enabled", false);
        var times = ParseTimes(_cfg["LegacySync:AutoTimesUtc"]);
        if (!enabled || times.Count == 0)
        {
            _log.LogInformation("Legacy auto-sync idle (enabled={Enabled}, AutoTimesUtc='{Times}')",
                enabled, _cfg["LegacySync:AutoTimesUtc"] ?? "");
            return;
        }
        _log.LogInformation("Legacy auto-sync active — UTC times: {Times}",
            string.Join(", ", times.Select(t => t.ToString("HH:mm"))));

        // Let the app finish migrations/seeding before the first possible run.
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var next = NextOccurrence(DateTime.UtcNow, times);
            var wait = next - DateTime.UtcNow;
            if (wait < TimeSpan.FromSeconds(1)) wait = TimeSpan.FromSeconds(1);
            _log.LogInformation("Next legacy auto-sync at {Next:yyyy-MM-dd HH:mm} UTC (in {Wait})",
                next, wait.ToString(@"hh\:mm\:ss"));
            try { await Task.Delay(wait, stoppingToken); }
            catch (OperationCanceledException) { return; }

            if (!await TryRunAsync("auto", stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                _log.LogWarning("Legacy auto-sync failed — retrying once in 10 minutes");
                try { await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken); }
                catch (OperationCanceledException) { return; }
                await TryRunAsync("auto-retry", stoppingToken);
            }
        }
    }

    private async Task<bool> TryRunAsync(string trigger, CancellationToken ct)
    {
        try
        {
            using var scope = _scopes.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<LegacySyncService>();
            await svc.RunAsync(trigger, ct);
            return true;
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return true; // shutting down — no retry
        }
        catch (Exception)
        {
            // Already logged (with the trigger) inside LegacySyncService.
            return false;
        }
    }
}
