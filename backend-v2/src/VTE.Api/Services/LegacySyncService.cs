using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Services;

/// <summary>Per-table counts of one sync run. Property names are part of the JSON
/// contract consumed by LegacySyncView.vue and deploy/run-legacy-sync.ps1 — keep them.</summary>
public record LegacySyncResult(
    int Clients, int Vehicles, int Registrations, int Relations,
    int Requests, int OwnershipProofs, int PaymentProofs, int References,
    long DurationMs, long MaxClientId, long MaxVehicleId, long MaxRequestId,
    int TechnicalExamReports,
    int InternationalDrivingLicences = 0, int VehiclePermissions = 0);

/// <summary>Singleton snapshot of the most recent sync run (manual or scheduled),
/// surfaced on the admin Синхронизација screen. Also owns the one-at-a-time gate.</summary>
public class LegacySyncState
{
    public DateTime? LastStartedAtUtc { get; set; }
    public DateTime? LastFinishedAtUtc { get; set; }
    public bool? LastSucceeded { get; set; }
    public string? LastTrigger { get; set; }
    public string? LastError { get; set; }
    public LegacySyncResult? LastResult { get; set; }

    /// <summary>Held for the duration of a run — a second caller is refused, not queued.</summary>
    public SemaphoreSlim Gate { get; } = new(1, 1);
}

/// <summary>
/// The incremental legacy sync, callable from the admin endpoint AND the in-process
/// scheduler (<see cref="LegacySyncScheduler"/>). Pulls NEW records from the live
/// legacy DBs via the [VTEZVV_LIVE] linked server (which lives on the SQL Server
/// this API points at — in prod that is the mssql container, so the whole sync is
/// server-side) by running the same migrate-*.sql scripts embedded at build time.
/// </summary>
public class LegacySyncService
{
    private readonly VteDbContext _db;
    private readonly IConfiguration _cfg;
    private readonly ILogger<LegacySyncService> _log;
    private readonly LegacySyncState _state;

    public LegacySyncService(VteDbContext db, IConfiguration cfg, ILogger<LegacySyncService> log, LegacySyncState state)
    {
        _db = db; _cfg = cfg; _log = log; _state = state;
    }

    public bool Enabled => _cfg.GetValue("LegacySync:Enabled", false);

    /// <summary>Runs one full incremental sync. Throws when disabled, already running,
    /// or when the sync itself fails (after recording the failure in the state).</summary>
    public async Task<LegacySyncResult> RunAsync(string trigger, CancellationToken ct = default)
    {
        if (!Enabled)
            throw new InvalidOperationException("Legacy sync is disabled (LegacySync:Enabled=false).");
        if (!await _state.Gate.WaitAsync(TimeSpan.Zero, ct))
            throw new InvalidOperationException("Синхронизацијата е веќе во тек — почекај да заврши.");

        var sw = Stopwatch.StartNew();
        _state.LastStartedAtUtc = DateTime.UtcNow;
        _state.LastFinishedAtUtc = null;
        _state.LastTrigger = trigger;
        _state.LastSucceeded = null;
        _state.LastError = null;
        try
        {
            await EnsureLinkedServerAsync();

            // Capture "before" maxes so we can count exactly what this run added.
            // Ids >= 10,000,000 are v2-native (migrate/fix-v2-native-id-space.sql) — the
            // legacy mirror lives below that floor, so watermarks/counts ignore them.
            var before = await ReadRowAsync(@"SELECT
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Client                WHERE Id < 10000000),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Vehicle               WHERE Id < 10000000),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleRegistration   WHERE Id < 10000000),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.ClientVehicleRelation WHERE Id < 10000000),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Request               WHERE Id < 10000000),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestOwnershipProof WHERE Id < 10000000),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestPaymentProof   WHERE Id < 10000000),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.TechnicalExamReport   WHERE Id < 10000000)");

            // Run the embedded incremental migration (its own transaction + rollback).
            // Raw DbCommand, NOT EF's ExecuteSqlRaw, which would mis-parse "{...}" in
            // the script as {0}-style placeholders.
            await RunScriptAsync(LoadEmbeddedSql("migrate-incremental.sql"), ct);

            // The two LIVE registries in the station's OTHER legacy DB (VERTEST):
            // меѓународни возачки дозволи + полномошна. Both idempotent on LegacyId.
            var idlBefore  = await ReadRowAsync("SELECT COUNT_BIG(*) FROM dbo.InternationalDrivingLicence WHERE LegacyId IS NOT NULL");
            var permBefore = await ReadRowAsync("SELECT COUNT_BIG(*) FROM dbo.VehiclePermission WHERE LegacyId IS NOT NULL");
            await RunScriptAsync(LoadEmbeddedSql("migrate-idl-from-vertest.sql"), ct);
            await RunScriptAsync(LoadEmbeddedSql("migrate-permissions-from-vertest.sql"), ct);
            var idlAfter  = await ReadRowAsync("SELECT COUNT_BIG(*) FROM dbo.InternationalDrivingLicence WHERE LegacyId IS NOT NULL");
            var permAfter = await ReadRowAsync("SELECT COUNT_BIG(*) FROM dbo.VehiclePermission WHERE LegacyId IS NOT NULL");

            // Count what landed (still below the v2-native floor).
            var after = await ReadRowAsync(@"SELECT
                (SELECT COUNT_BIG(*) FROM dbo.Client                WHERE Id > @p0 AND Id < 10000000),
                (SELECT COUNT_BIG(*) FROM dbo.Vehicle               WHERE Id > @p1 AND Id < 10000000),
                (SELECT COUNT_BIG(*) FROM dbo.VehicleRegistration   WHERE Id > @p2 AND Id < 10000000),
                (SELECT COUNT_BIG(*) FROM dbo.ClientVehicleRelation WHERE Id > @p3 AND Id < 10000000),
                (SELECT COUNT_BIG(*) FROM dbo.Request               WHERE Id > @p4 AND Id < 10000000),
                (SELECT COUNT_BIG(*) FROM dbo.RequestOwnershipProof WHERE Id > @p5 AND Id < 10000000),
                (SELECT COUNT_BIG(*) FROM dbo.RequestPaymentProof   WHERE Id > @p6 AND Id < 10000000),
                (SELECT COUNT_BIG(*) FROM dbo.Request WHERE Id > @p4 AND Id < 10000000 AND LegacyReferenceNumber IS NOT NULL),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Client  WHERE Id < 10000000),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Vehicle WHERE Id < 10000000),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Request WHERE Id < 10000000),
                (SELECT COUNT_BIG(*) FROM dbo.TechnicalExamReport WHERE Id > @p7 AND Id < 10000000)",
                before[0], before[1], before[2], before[3], before[4], before[5], before[6], before[7]);

            sw.Stop();
            var result = new LegacySyncResult(
                (int)after[0], (int)after[1], (int)after[2], (int)after[3],
                (int)after[4], (int)after[5], (int)after[6], (int)after[7],
                sw.ElapsedMilliseconds, after[8], after[9], after[10],
                (int)after[11],
                (int)(idlAfter[0] - idlBefore[0]), (int)(permAfter[0] - permBefore[0]));

            _log.LogInformation("Legacy sync ({Trigger}) done in {Ms}ms: +{C} clients, +{V} vehicles, +{R} requests, +{T} tech-exam reports, +{I} IDLs, +{P} permissions",
                trigger, sw.ElapsedMilliseconds, result.Clients, result.Vehicles, result.Requests,
                result.TechnicalExamReports, result.InternationalDrivingLicences, result.VehiclePermissions);

            _state.LastFinishedAtUtc = DateTime.UtcNow;
            _state.LastSucceeded = true;
            _state.LastResult = result;
            return result;
        }
        catch (Exception ex)
        {
            _state.LastFinishedAtUtc = DateTime.UtcNow;
            _state.LastSucceeded = false;
            _state.LastError = ex.Message;
            _log.LogError(ex, "Legacy sync ({Trigger}) failed", trigger);
            throw;
        }
        finally
        {
            _state.Gate.Release();
        }
    }

    // ---- helpers (moved verbatim from LegacySyncController) ----

    private async Task EnsureLinkedServerAsync()
    {
        var name = _cfg["LegacySync:LinkedServerName"] ?? "VTEZVV_LIVE";
        var host = _cfg["LegacySync:Host"] ?? throw new InvalidOperationException("LegacySync:Host not configured.");
        var dbn  = _cfg["LegacySync:Database"] ?? "VTEZVV";
        var user = _cfg["LegacySync:User"] ?? throw new InvalidOperationException("LegacySync:User not configured.");
        var pwd  = _cfg["LegacySync:Password"] ?? throw new InvalidOperationException("LegacySync:Password not configured.");

        const string sql = @"
IF NOT EXISTS (SELECT 1 FROM sys.servers WHERE name = @name)
BEGIN
  EXEC sp_addlinkedserver @server=@name, @srvproduct=N'', @provider=N'MSOLEDBSQL',
       @datasrc=@host, @catalog=@db, @provstr=N'TrustServerCertificate=yes';
  EXEC sp_addlinkedsrvlogin @rmtsrvname=@name, @useself=N'false', @rmtuser=@user, @rmtpassword=@pwd;
  EXEC sp_serveroption @name, N'rpc out', N'true';
END";
        await _db.Database.ExecuteSqlRawAsync(sql,
            new SqlParameter("@name", name), new SqlParameter("@host", host),
            new SqlParameter("@db", dbn), new SqlParameter("@user", user), new SqlParameter("@pwd", pwd));
    }

    /// <summary>Embedded migration script with the leading USE/GO batch directives stripped.</summary>
    private static string LoadEmbeddedSql(string logicalName)
    {
        var asm = Assembly.GetExecutingAssembly();
        using var stream = asm.GetManifestResourceStream(logicalName)
            ?? throw new InvalidOperationException($"Embedded resource {logicalName} not found.");
        using var reader = new StreamReader(stream);
        var raw = reader.ReadToEnd();
        // Remove `USE VTE;` and standalone `GO` lines — invalid via ADO; we're already on VTE.
        return Regex.Replace(raw, @"(?im)^\s*(GO|USE\s+VTE;?)\s*$", "");
    }

    /// <summary>Runs a multi-statement script via a raw DbCommand (no EF placeholder parsing).</summary>
    private async Task RunScriptAsync(string sql, CancellationToken ct, int timeoutSec = 600)
    {
        var conn = (SqlConnection)_db.Database.GetDbConnection();
        var wasOpen = conn.State == ConnectionState.Open;
        if (!wasOpen) await conn.OpenAsync(ct);
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandTimeout = timeoutSec;
            await cmd.ExecuteNonQueryAsync(ct);
        }
        finally { if (!wasOpen) await conn.CloseAsync(); }
    }

    /// <summary>Runs a single-row query and returns the columns as longs.</summary>
    internal async Task<long[]> ReadRowAsync(string sql, params object[] args)
    {
        var conn = (SqlConnection)_db.Database.GetDbConnection();
        var wasOpen = conn.State == ConnectionState.Open;
        if (!wasOpen) await conn.OpenAsync();
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            for (var i = 0; i < args.Length; i++)
                cmd.Parameters.Add(new SqlParameter("@p" + i, args[i]));
            using var rd = await cmd.ExecuteReaderAsync();
            await rd.ReadAsync();
            var vals = new long[rd.FieldCount];
            for (var i = 0; i < rd.FieldCount; i++)
                vals[i] = rd.IsDBNull(i) ? 0 : Convert.ToInt64(rd.GetValue(i));
            return vals;
        }
        finally { if (!wasOpen) await conn.CloseAsync(); }
    }
}
