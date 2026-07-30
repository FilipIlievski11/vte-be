using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VTE.Api.Services;

namespace VTE.Api.Controllers;

/// <summary>
/// Admin-only health snapshot of the two unattended night jobs, so the app itself
/// can raise a warning banner (no external alerting service / no credentials):
///   • Backup — from the status JSON the server-side cron (deploy/backup-db.sh)
///     writes into the attachments volume (/data/attachments/ops/backup-status.json).
///   • Auto-sync — from the in-process <see cref="LegacySyncState"/>.
/// Each part is null when not applicable (dev has no backup file; sync disabled),
/// so the frontend simply shows nothing.
/// </summary>
[ApiController]
[Route("api/admin/ops-health")]
[Authorize(Roles = "Administrator")]
public class OpsHealthController : ControllerBase
{
    /// <summary>A job is considered late when its last success is older than this.</summary>
    private const double StaleHours = 26;

    private readonly IConfiguration _cfg;
    private readonly LegacySyncState _syncState;

    public OpsHealthController(IConfiguration cfg, LegacySyncState syncState)
    {
        _cfg = cfg; _syncState = syncState;
    }

    public record JobHealth(bool Ok, string? Note, DateTime? LastSuccessUtc, double? AgeHours);
    public record OpsHealthDto(JobHealth? Backup, JobHealth? Sync, DateTime CheckedAtUtc);

    [HttpGet]
    public ActionResult<OpsHealthDto> Get()
    {
        return Ok(new OpsHealthDto(ReadBackupHealth(), ReadSyncHealth(), DateTime.UtcNow));
    }

    private JobHealth? ReadBackupHealth()
    {
        var path = _cfg["Ops:BackupStatusPath"] ?? "/data/attachments/ops/backup-status.json";
        if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path)) return null;
        try
        {
            using var doc = JsonDocument.Parse(System.IO.File.ReadAllText(path));
            var root = doc.RootElement;
            DateTime? lastSuccess = null;
            if (root.TryGetProperty("lastSuccessUtc", out var ls) &&
                DateTime.TryParse(ls.GetString(), CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var d))
                lastSuccess = d;
            var attemptOk = root.TryGetProperty("lastAttemptOk", out var ao) && ao.ValueKind == JsonValueKind.True;

            var age = lastSuccess.HasValue ? (DateTime.UtcNow - lastSuccess.Value).TotalHours : (double?)null;
            var ok = attemptOk && age is < StaleHours;
            string? note = ok ? null
                : !attemptOk ? "последниот обид НЕ успеа (backup.log на серверот)"
                : age == null ? "нема успешен backup"
                : $"последен успешен пред {age:0} часа";
            return new JobHealth(ok, note, lastSuccess, age);
        }
        catch
        {
            return new JobHealth(false, "статус фајлот е нечитлив", null, null);
        }
    }

    private JobHealth? ReadSyncHealth()
    {
        var enabled = _cfg.GetValue("LegacySync:Enabled", false);
        var times = LegacySyncScheduler.ParseTimes(_cfg["LegacySync:AutoTimesUtc"]);
        if (!enabled || times.Count == 0) return null;

        var s = _syncState;
        if (s.Gate.CurrentCount == 0)
            return new JobHealth(true, null, null, null);                    // work in progress

        if (s.LastStartedAtUtc == null)
        {
            // State is in-memory: right after a (re)start there is legitimately no run yet.
            var uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalHours;
            var ok = uptime < StaleHours;
            return new JobHealth(ok, ok ? null : "нема ниту едно извршување по стартот на серверот", null, null);
        }

        if (s.LastSucceeded == false)
            return new JobHealth(false, $"последниот sync НЕ успеа: {s.LastError}", null, null);

        var last = s.LastFinishedAtUtc ?? s.LastStartedAtUtc.Value;
        var age2 = (DateTime.UtcNow - last).TotalHours;
        return new JobHealth(age2 < StaleHours,
            age2 < StaleHours ? null : $"последен успешен sync пред {age2:0} часа",
            s.LastSucceeded == true ? last : null, age2);
    }
}
