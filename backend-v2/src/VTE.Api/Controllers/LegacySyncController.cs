using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// Admin-only "Sync from legacy" endpoint. Pulls NEW records (Id &gt; current max)
/// from the live legacy DB via the [VTEZVV_LIVE] linked server and inserts them
/// into VTE, additively. Runs the same migrate-incremental.sql embedded at build
/// time, so the button and the manual sqlcmd workflow stay in lockstep.
/// </summary>
[ApiController]
[Route("api/admin/legacy-sync")]
[Authorize(Roles = "Administrator")]
public class LegacySyncController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly IConfiguration _cfg;
    private readonly ILogger<LegacySyncController> _log;

    public LegacySyncController(VteDbContext db, IConfiguration cfg, ILogger<LegacySyncController> log)
    {
        _db = db; _cfg = cfg; _log = log;
    }

    public record SyncStatus(bool Enabled, long MaxClientId, long MaxVehicleId, long MaxRequestId,
        long Clients, long Vehicles, long Requests, long TechnicalExamReports);

    public record SyncResult(
        int Clients, int Vehicles, int Registrations, int Relations,
        int Requests, int OwnershipProofs, int PaymentProofs, int References,
        long DurationMs, long MaxClientId, long MaxVehicleId, long MaxRequestId,
        int TechnicalExamReports);

    /// <summary>Current high-water marks, shown on the admin page before syncing.</summary>
    [HttpGet("status")]
    public async Task<ActionResult<SyncStatus>> Status()
    {
        var enabled = _cfg.GetValue("LegacySync:Enabled", false);
        var row = await ReadRowAsync(@"SELECT
            (SELECT ISNULL(MAX(Id),0) FROM dbo.Client),
            (SELECT ISNULL(MAX(Id),0) FROM dbo.Vehicle),
            (SELECT ISNULL(MAX(Id),0) FROM dbo.Request),
            (SELECT COUNT_BIG(*) FROM dbo.Client),
            (SELECT COUNT_BIG(*) FROM dbo.Vehicle),
            (SELECT COUNT_BIG(*) FROM dbo.Request),
            (SELECT COUNT_BIG(*) FROM dbo.TechnicalExamReport)");
        return new SyncStatus(enabled, row[0], row[1], row[2], row[3], row[4], row[5], row[6]);
    }

    /// <summary>Runs the incremental top-up and returns the per-table new-row counts.</summary>
    [HttpPost]
    public async Task<ActionResult<SyncResult>> Sync()
    {
        if (!_cfg.GetValue("LegacySync:Enabled", false))
            return Problem("Legacy sync is disabled (LegacySync:Enabled=false).", statusCode: 400);

        var sw = Stopwatch.StartNew();
        try
        {
            await EnsureLinkedServerAsync();

            // Capture "before" maxes so we can count exactly what this run added.
            var before = await ReadRowAsync(@"SELECT
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Client),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Vehicle),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.VehicleRegistration),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.ClientVehicleRelation),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Request),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestOwnershipProof),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.RequestPaymentProof),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.TechnicalExamReport)");

            // Run the embedded incremental migration (its own transaction + rollback).
            // Use a raw DbCommand, NOT EF's ExecuteSqlRaw, which would mis-parse any
            // "{...}" in the script as a {0}-style placeholder.
            await RunScriptAsync(LoadMigrationSql());

            // Count what landed.
            var after = await ReadRowAsync(@"SELECT
                (SELECT COUNT_BIG(*) FROM dbo.Client                WHERE Id > @p0),
                (SELECT COUNT_BIG(*) FROM dbo.Vehicle               WHERE Id > @p1),
                (SELECT COUNT_BIG(*) FROM dbo.VehicleRegistration   WHERE Id > @p2),
                (SELECT COUNT_BIG(*) FROM dbo.ClientVehicleRelation WHERE Id > @p3),
                (SELECT COUNT_BIG(*) FROM dbo.Request               WHERE Id > @p4),
                (SELECT COUNT_BIG(*) FROM dbo.RequestOwnershipProof WHERE Id > @p5),
                (SELECT COUNT_BIG(*) FROM dbo.RequestPaymentProof   WHERE Id > @p6),
                (SELECT COUNT_BIG(*) FROM dbo.Request WHERE Id > @p4 AND LegacyReferenceNumber IS NOT NULL),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Client),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Vehicle),
                (SELECT ISNULL(MAX(Id),0) FROM dbo.Request),
                (SELECT COUNT_BIG(*) FROM dbo.TechnicalExamReport WHERE Id > @p7)",
                before[0], before[1], before[2], before[3], before[4], before[5], before[6], before[7]);

            sw.Stop();
            _log.LogInformation("Legacy sync done in {Ms}ms: +{C} clients, +{V} vehicles, +{R} requests, +{T} tech-exam reports",
                sw.ElapsedMilliseconds, after[0], after[1], after[4], after[11]);

            return new SyncResult(
                (int)after[0], (int)after[1], (int)after[2], (int)after[3],
                (int)after[4], (int)after[5], (int)after[6], (int)after[7],
                sw.ElapsedMilliseconds, after[8], after[9], after[10],
                (int)after[11]);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Legacy sync failed");
            return Problem("Legacy sync failed: " + ex.Message, statusCode: 500);
        }
    }

    // ---- helpers ----

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

    /// <summary>Embedded migrate-incremental.sql with the leading USE/GO batch directive stripped.</summary>
    private static string LoadMigrationSql()
    {
        var asm = Assembly.GetExecutingAssembly();
        using var stream = asm.GetManifestResourceStream("migrate-incremental.sql")
            ?? throw new InvalidOperationException("Embedded resource migrate-incremental.sql not found.");
        using var reader = new StreamReader(stream);
        var raw = reader.ReadToEnd();
        // Remove `USE VTE;` and standalone `GO` lines — invalid via ADO; we're already on VTE.
        return Regex.Replace(raw, @"(?im)^\s*(GO|USE\s+VTE;?)\s*$", "");
    }

    /// <summary>Runs a multi-statement script via a raw DbCommand (no EF placeholder parsing).</summary>
    private async Task RunScriptAsync(string sql, int timeoutSec = 600)
    {
        var conn = (SqlConnection)_db.Database.GetDbConnection();
        var wasOpen = conn.State == ConnectionState.Open;
        if (!wasOpen) await conn.OpenAsync();
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandTimeout = timeoutSec;
            await cmd.ExecuteNonQueryAsync();
        }
        finally { if (!wasOpen) await conn.CloseAsync(); }
    }

    /// <summary>Runs a single-row query and returns the columns as longs.</summary>
    private async Task<long[]> ReadRowAsync(string sql, params object[] args)
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
