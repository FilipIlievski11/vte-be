using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Payments;
using VTE.Infrastructure;

namespace VTE.Api.Migration;

public record MigrationStatus(long VteMaxId, long? LegacyMaxId, int Pending);

public record MigrationSkip(long Id, string Reason);

public record MigrationResult(int Inserted, int Skipped, long NewMaxId, int Total, IReadOnlyList<MigrationSkip> Skips);
public record BackfillResult(int Updated, int LegacyRowsFound, int LocalRowsScanned, IReadOnlyList<string> Errors);
public record PaymentCatalogMigrationResult(
    int PaymentTypesUpdated, int PaymentCategoriesInserted,
    int PaymentItemsInserted, int PaymentItemParametarsInserted,
    int PaymentDocumentNumbersInserted, IReadOnlyList<string> Errors);
public record PaymentItemIdBackfillResult(int Updated, int Scanned, IReadOnlyList<string> Errors);

// Delta migrates "newer" Customer rows from the legacy VTEZVV DB into VTE2.
// Mirrors what the one-off PowerShell did:
//   1. Find MAX(Id) in the local VTE2.Customers (per the seeded StationId).
//   2. Pull rows from legacy with Id > that, joining reference names for FK remap.
//   3. INSERT into VTE2 inside SET IDENTITY_INSERT so legacy Ids are preserved.
//   4. Skip rows that hit any constraint — log the reason, continue with the next.
public class LegacyMigrationService
{
    private readonly VteDbContext _db;
    private readonly string _legacyCs;
    private readonly int _stationId;

    public LegacyMigrationService(VteDbContext db, IConfiguration cfg)
    {
        _db = db;
        _legacyCs = cfg["Legacy:ConnectionString"]
            ?? throw new InvalidOperationException("Legacy:ConnectionString is not configured.");
        _stationId = cfg.GetValue<int?>("Legacy:DefaultStationId") ?? 1;
    }

    public async Task<MigrationStatus> GetStatusAsync(CancellationToken ct = default)
    {
        var vteMax = await _db.Customers.IgnoreQueryFilters()
            .Where(c => c.StationId == _stationId)
            .MaxAsync(c => (long?)c.Id, ct) ?? 0;
        var legacyMax = await GetLegacyMaxIdAsync("dbo.Customers", ct);
        return BuildStatus(vteMax, legacyMax);
    }

    public async Task<MigrationStatus> GetStreetsStatusAsync(CancellationToken ct = default)
    {
        var vteMax = await _db.Streets.MaxAsync(s => (long?)s.Id, ct) ?? 0;
        var legacyMax = await GetLegacyMaxIdAsync("dbo.Streets", ct);
        return BuildStatus(vteMax, legacyMax);
    }

    public async Task<MigrationStatus> GetVehiclesStatusAsync(CancellationToken ct = default)
    {
        var vteMax = await _db.Vehicles.IgnoreQueryFilters()
            .Where(v => v.StationId == _stationId)
            .MaxAsync(v => (long?)v.Id, ct) ?? 0;
        var legacyMax = await GetLegacyMaxIdAsync("dbo.Vehicles", ct);
        return BuildStatus(vteMax, legacyMax);
    }

    public async Task<MigrationStatus> GetRelationsStatusAsync(CancellationToken ct = default)
    {
        var vteMax = await _db.CustomerVehicleRelations.MaxAsync(r => (long?)r.Id, ct) ?? 0;
        var legacyMax = await GetLegacyMaxIdAsync("dbo.CustomerVehiclesRelations", ct);
        return BuildStatus(vteMax, legacyMax);
    }

    // For reference tables, the "pending" metric is "names in legacy not in VTE2"
    // (case-insensitive, trimmed). VteMaxId/LegacyMaxId carry row counts.
    public Task<MigrationStatus> GetVehicleMakersStatusAsync(CancellationToken ct = default)
        => GetRefDiffStatusAsync("dbo.VehicleMakers", "CompanyName", "dbo.VehicleMakers", ct);

    public async Task<MigrationStatus> GetVehicleModelsStatusAsync(CancellationToken ct = default)
    {
        var st = await GetRefDiffStatusAsync("dbo.VehicleModel", "ModelName", "dbo.VehicleModels", ct);
        // "Pending" for models = missing names + vehicles still waiting for a model binding.
        // The migrate action does both, so the user has a single actionable number.
        var nullsRebindable = await _db.Vehicles.IgnoreQueryFilters().CountAsync(v => v.VehicleModelId == null, ct);
        return new MigrationStatus(st.VteMaxId, st.LegacyMaxId, st.Pending + nullsRebindable);
    }

    private async Task<MigrationStatus> GetRefDiffStatusAsync(string legacyTable, string legacyNameCol, string vteTable, CancellationToken ct)
    {
        var legacyNames = await ReadNamesAsync(_legacyCs, $"SELECT {legacyNameCol} FROM {legacyTable}", ct);
        var vteNames    = await ReadNamesAsync(GetVteConnectionString(), $"SELECT Name FROM {vteTable}", ct);
        var missing = legacyNames.Count(n => !vteNames.Contains(n));
        return new MigrationStatus(vteNames.Count, legacyNames.Count, missing);
    }

    private string GetVteConnectionString() => _db.Database.GetConnectionString() ?? throw new InvalidOperationException("VTE2 connection string is not configured.");

    private static async Task<HashSet<string>> ReadNamesAsync(string cs, string sql, CancellationToken ct)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        await using var cn = new SqlConnection(cs); await cn.OpenAsync(ct);
        await using var cmd = cn.CreateCommand(); cmd.CommandText = sql;
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            if (rdr.IsDBNull(0)) continue;
            var n = rdr.GetString(0).Trim();
            if (n.Length > 0) set.Add(n);
        }
        return set;
    }

    private async Task<long?> GetLegacyMaxIdAsync(string table, CancellationToken ct)
    {
        await using var lcn = new SqlConnection(_legacyCs);
        try
        {
            await lcn.OpenAsync(ct);
            await using var cmd = lcn.CreateCommand();
            cmd.CommandText = $"SELECT MAX(Id) FROM {table}";
            var v = await cmd.ExecuteScalarAsync(ct);
            if (v is long l) return l;
            if (v is int i)  return i;
            if (v != DBNull.Value && v != null) return Convert.ToInt64(v);
            return null;
        }
        catch
        {
            // Legacy unreachable — surface as null so the UI can disable the button.
            return null;
        }
    }

    private static MigrationStatus BuildStatus(long vteMax, long? legacyMax)
    {
        var pending = legacyMax.HasValue && legacyMax > vteMax ? (int)(legacyMax.Value - vteMax) : 0;
        return new MigrationStatus(vteMax, legacyMax, pending);
    }

    public async Task<MigrationResult> MigrateNewerCustomersAsync(CancellationToken ct = default)
    {
        var status = await GetStatusAsync(ct);
        var vteMax = status.VteMaxId;

        // 1. Pull legacy rows (newer than our max) + ref names for FK remap.
        var legacyRows = new List<LegacyCustomer>();
        await using (var lcn = new SqlConnection(_legacyCs))
        {
            await lcn.OpenAsync(ct);
            await using var cmd = lcn.CreateCommand();
            cmd.CommandText = @"
SELECT c.Id, c.MB, c.CustomerSurname, c.CustomerFirstName, c.PhoneNumber, c.Fax, c.eMail,
       c.LivingAddressNumber, c.BrithAddressNumber, c.DateOfBirth, c.IsCompany,
       c.Occupation, c.WorksInCompany, c.ParentName, c.PassportNumber, c.BLK,
       c.CanSendNotifications, c.TaxNumber, c.BLKDateIssued, c.DriveingLicenceNumber,
       c.DriveingLicenceDateIssued, c.PassDateIssued, c.Active, c.Note, c.Status,
       ci.CityName    AS LivingCityName, st.StreetName AS LivingStreetName,
       cb.CityName    AS BirthCityName,  sb.StreetName AS BirthStreetName,
       bt.BusinessTypeDescription AS BusinessTypeName,
       co.CountryName AS CitizenshipName,
       blki.IssuerName AS IDCardIssuerName,
       pi2.IssuerName  AS PassportIssuerName,
       dli.IssuerName  AS DLIssuerName
FROM dbo.Customers c
LEFT JOIN dbo.Cities  ci ON ci.Id = c.IdLivingCity
LEFT JOIN dbo.Streets st ON st.Id = c.IdLivingAddress
LEFT JOIN dbo.Cities  cb ON cb.Id = c.IdBirhCity
LEFT JOIN dbo.Streets sb ON sb.Id = c.IdBirthAddress
LEFT JOIN dbo.BusinessTypes bt ON bt.Id = c.IdBusinessType
LEFT JOIN dbo.Countries co ON co.Id = c.IdCitizenship
LEFT JOIN dbo.RegistrationIssuers blki ON blki.Id = c.BLKIssuer
LEFT JOIN dbo.RegistrationIssuers pi2  ON pi2.Id  = c.PassIssuer
LEFT JOIN dbo.RegistrationIssuers dli  ON dli.Id  = c.DriveingLicenceIssuer
WHERE c.Id > @max
ORDER BY c.Id;";
            cmd.Parameters.AddWithValue("@max", vteMax);
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            while (await rdr.ReadAsync(ct))
                legacyRows.Add(LegacyCustomer.Read(rdr));
        }

        if (legacyRows.Count == 0)
            return new MigrationResult(0, 0, vteMax, await _db.Customers.IgnoreQueryFilters().CountAsync(ct), Array.Empty<MigrationSkip>());

        // 2. Build ref name -> id maps from VTE2 (case-insensitive; prefer active rows).
        var mCity = await BuildRefMapAsync("Cities", ct);
        var mStr  = await BuildRefMapAsync("Streets", ct);
        var mBT   = await BuildRefMapAsync("BusinessTypes", ct);
        var mCo   = await BuildRefMapAsync("Countries", ct);
        var mRI   = await BuildRefMapAsync("RegistrationIssuers", ct);

        int inserted = 0;
        var skips = new List<MigrationSkip>();

        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        await using (var idOn = conn.CreateCommand())
        {
            idOn.CommandText = "SET IDENTITY_INSERT dbo.Customers ON;";
            await idOn.ExecuteNonQueryAsync(ct);
        }

        try
        {
            foreach (var r in legacyRows)
            {
                try
                {
                    await using var ins = conn.CreateCommand();
                    ins.CommandText = InsertSql;
                    Bind(ins, r, _stationId, mCity, mStr, mBT, mCo, mRI);
                    await ins.ExecuteNonQueryAsync(ct);
                    inserted++;
                }
                catch (SqlException ex)
                {
                    skips.Add(new MigrationSkip(r.Id, FirstLine(ex.Message)));
                }
                catch (Exception ex)
                {
                    skips.Add(new MigrationSkip(r.Id, FirstLine(ex.Message)));
                }
            }
        }
        finally
        {
            await using var idOff = conn.CreateCommand();
            idOff.CommandText = "SET IDENTITY_INSERT dbo.Customers OFF;";
            await idOff.ExecuteNonQueryAsync(ct);
        }

        // Reseed identity so subsequent UI-driven inserts continue past the new max.
        await using (var reseed = conn.CreateCommand())
        {
            reseed.CommandText = "DBCC CHECKIDENT('dbo.Customers', RESEED);";
            await reseed.ExecuteNonQueryAsync(ct);
        }

        var newMax = await _db.Customers.IgnoreQueryFilters().MaxAsync(c => (long?)c.Id, ct) ?? 0;
        var total  = await _db.Customers.IgnoreQueryFilters().CountAsync(ct);
        return new MigrationResult(inserted, skips.Count, newMax, total, skips);
    }

    public async Task<MigrationResult> MigrateNewerStreetsAsync(CancellationToken ct = default)
    {
        var status = await GetStreetsStatusAsync(ct);
        var vteMax = status.VteMaxId;

        // Pull legacy rows newer than our max — minimal projection, no FK remap needed.
        var legacyRows = new List<(int Id, string Name, bool Active)>();
        await using (var lcn = new SqlConnection(_legacyCs))
        {
            await lcn.OpenAsync(ct);
            await using var cmd = lcn.CreateCommand();
            cmd.CommandText = "SELECT Id, StreetName, ISNULL(Active, 1) AS Active FROM dbo.Streets WHERE Id > @max ORDER BY Id;";
            cmd.Parameters.AddWithValue("@max", vteMax);
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            while (await rdr.ReadAsync(ct))
            {
                var id = rdr.GetInt32(0);
                var name = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1).Trim();
                var active = !rdr.IsDBNull(2) && rdr.GetBoolean(2);
                legacyRows.Add((id, name, active));
            }
        }

        if (legacyRows.Count == 0)
            return new MigrationResult(0, 0, vteMax, await _db.Streets.CountAsync(ct), Array.Empty<MigrationSkip>());

        int inserted = 0;
        var skips = new List<MigrationSkip>();

        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        await using (var idOn = conn.CreateCommand())
        {
            idOn.CommandText = "SET IDENTITY_INSERT dbo.Streets ON;";
            await idOn.ExecuteNonQueryAsync(ct);
        }

        try
        {
            foreach (var (id, name, active) in legacyRows)
            {
                if (string.IsNullOrWhiteSpace(name)) { skips.Add(new MigrationSkip(id, "Empty StreetName.")); continue; }
                try
                {
                    await using var ins = conn.CreateCommand();
                    ins.CommandText = "INSERT INTO dbo.Streets (Id, Name, IsActive) VALUES (@Id, @Name, @IsActive);";
                    var pId = ins.CreateParameter(); pId.ParameterName = "@Id";       pId.Value = id;       ins.Parameters.Add(pId);
                    var pNm = ins.CreateParameter(); pNm.ParameterName = "@Name";     pNm.Value = name;     ins.Parameters.Add(pNm);
                    var pAc = ins.CreateParameter(); pAc.ParameterName = "@IsActive"; pAc.Value = active;   ins.Parameters.Add(pAc);
                    await ins.ExecuteNonQueryAsync(ct);
                    inserted++;
                }
                catch (Exception ex) { skips.Add(new MigrationSkip(id, FirstLine(ex.Message))); }
            }
        }
        finally
        {
            await using var idOff = conn.CreateCommand();
            idOff.CommandText = "SET IDENTITY_INSERT dbo.Streets OFF;";
            await idOff.ExecuteNonQueryAsync(ct);
        }

        await using (var reseed = conn.CreateCommand())
        {
            reseed.CommandText = "DBCC CHECKIDENT('dbo.Streets', RESEED);";
            await reseed.ExecuteNonQueryAsync(ct);
        }

        var newMax = await _db.Streets.MaxAsync(s => (long?)s.Id, ct) ?? 0;
        var total  = await _db.Streets.CountAsync(ct);
        return new MigrationResult(inserted, skips.Count, newMax, total, skips);
    }

    public async Task<MigrationResult> MigrateVehicleMakersAsync(CancellationToken ct = default)
    {
        var legacyNames = await ReadNamesAsync(_legacyCs, "SELECT CompanyName FROM dbo.VehicleMakers", ct);
        var vteNames    = await ReadNamesAsync(GetVteConnectionString(), "SELECT Name FROM dbo.VehicleMakers", ct);
        var missing = legacyNames.Where(n => !vteNames.Contains(n)).ToList();

        int inserted = 0; var skips = new List<MigrationSkip>();
        foreach (var name in missing)
        {
            try
            {
                _db.VehicleMakers.Add(new VTE.Domain.Reference.VehicleMaker { Name = name, IsActive = true });
                await _db.SaveChangesAsync(ct);
                inserted++;
            }
            catch (Exception ex) { skips.Add(new MigrationSkip(0, $"{name}: {FirstLine(ex.Message)}")); }
        }
        var total = await _db.VehicleMakers.CountAsync(ct);
        return new MigrationResult(inserted, skips.Count, total, total, skips);
    }

    public async Task<MigrationResult> MigrateVehicleModelsAsync(CancellationToken ct = default)
    {
        // Pull legacy models + their maker name; insert any missing model into VTE2.
        var rows = new List<(string ModelName, string? MakerName)>();
        await using (var lcn = new SqlConnection(_legacyCs))
        {
            await lcn.OpenAsync(ct);
            await using var cmd = lcn.CreateCommand();
            cmd.CommandText = @"
SELECT LTRIM(RTRIM(m.ModelName)) AS ModelName, mk.CompanyName AS MakerName
FROM dbo.VehicleModel m
LEFT JOIN dbo.VehicleMakers mk ON mk.Id = m.IdVehicleMaker
WHERE m.ModelName IS NOT NULL AND LTRIM(RTRIM(m.ModelName)) <> ''";
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            while (await rdr.ReadAsync(ct))
                rows.Add((rdr.GetString(0), rdr.IsDBNull(1) ? null : rdr.GetString(1).Trim()));
        }

        var vteNames = await ReadNamesAsync(GetVteConnectionString(), "SELECT Name FROM dbo.VehicleModels", ct);
        var makerMap = await BuildRefMapAsync("VehicleMakers", ct);

        // Insert each missing legacy model. Dedupe within this batch — case-insensitive trimmed.
        int inserted = 0; var skips = new List<MigrationSkip>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (modelName, makerName) in rows)
        {
            if (vteNames.Contains(modelName)) continue;
            if (!seen.Add(modelName)) continue;
            try
            {
                _db.VehicleModels.Add(new VTE.Domain.Reference.VehicleModel
                {
                    Name = modelName,
                    VehicleMakerId = LookupId(makerMap, makerName) ?? 0,
                    IsActive = true,
                });
                await _db.SaveChangesAsync(ct);
                inserted++;
            }
            catch (Exception ex) { skips.Add(new MigrationSkip(0, $"{modelName}: {FirstLine(ex.Message)}")); }
        }

        // Re-bind VTE2.Vehicles with NULL VehicleModelId where a matching legacy ModelName now exists.
        int rebound = await RebindNullVehicleModelsAsync(ct);
        if (rebound > 0)
            skips.Add(new MigrationSkip(0, $"(info) re-bound {rebound} vehicles to their model"));

        var total = await _db.VehicleModels.CountAsync(ct);
        var maxId = await _db.VehicleModels.MaxAsync(m => (long?)m.Id, ct) ?? 0;
        return new MigrationResult(inserted, skips.Count, maxId, total, skips);
    }

    private async Task<int> RebindNullVehicleModelsAsync(CancellationToken ct)
    {
        // Pull (ShellNumber → trimmed ModelName) from legacy where model name is non-empty.
        var legacyShellToModel = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        await using (var lcn = new SqlConnection(_legacyCs))
        {
            await lcn.OpenAsync(ct);
            await using var cmd = lcn.CreateCommand();
            cmd.CommandText = @"
SELECT v.ShellNumber, LTRIM(RTRIM(m.ModelName)) AS ModelName
FROM dbo.Vehicles v
INNER JOIN dbo.VehicleModel m ON m.Id = v.IdVehicleModel
WHERE m.ModelName IS NOT NULL AND LTRIM(RTRIM(m.ModelName)) <> ''";
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            while (await rdr.ReadAsync(ct))
                if (!rdr.IsDBNull(0)) legacyShellToModel[rdr.GetString(0)] = rdr.GetString(1);
        }

        var modelMap = await BuildRefMapAsync("VehicleModels", ct);
        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        var nullRows = new List<(long Id, string Shell)>();
        await using (var sel = conn.CreateCommand())
        {
            sel.CommandText = "SELECT Id, ShellNumber FROM dbo.Vehicles WHERE VehicleModelId IS NULL";
            await using var rdr = await sel.ExecuteReaderAsync(ct);
            while (await rdr.ReadAsync(ct))
                nullRows.Add((rdr.GetInt64(0), rdr.IsDBNull(1) ? "" : rdr.GetString(1)));
        }

        int rebound = 0;
        foreach (var (id, shell) in nullRows)
        {
            if (!legacyShellToModel.TryGetValue(shell, out var name)) continue;
            if (!modelMap.TryGetValue(name, out var modelId)) continue;
            await using var upd = conn.CreateCommand();
            upd.CommandText = "UPDATE dbo.Vehicles SET VehicleModelId = @M WHERE Id = @Id";
            var pM = upd.CreateParameter(); pM.ParameterName = "@M"; pM.Value = modelId; upd.Parameters.Add(pM);
            var pI = upd.CreateParameter(); pI.ParameterName = "@Id"; pI.Value = id; upd.Parameters.Add(pI);
            rebound += await upd.ExecuteNonQueryAsync(ct);
        }
        return rebound;
    }

    public async Task<MigrationResult> MigrateNewerVehiclesAsync(CancellationToken ct = default)
    {
        var status = await GetVehiclesStatusAsync(ct);
        var vteMax = status.VteMaxId;

        var rows = new List<LegacyVehicle>();
        await using (var lcn = new SqlConnection(_legacyCs))
        {
            await lcn.OpenAsync(ct);
            await using var cmd = lcn.CreateCommand();
            // Pull just the columns we map, with joined ref names for FK remap.
            cmd.CommandText = @"
SELECT v.Id, v.ShellNumber, v.FirstRegistrationNumber, v.LastRegistratinNumber AS LastRegistrationNumber,
       v.VehicleModelAdding, v.EngineNumber, v.EnginePower, v.EngineWorkingCapacity, v.BrojNaVrtezi AS RPM,
       v.VehicleSizeHight AS H, v.VehicleSizeWidth AS W, v.VehicleSizeLength AS L,
       v.NumberOfDoors, v.NumberOfSeats, v.NumberOfStandingSeats, v.NumberOfLieingSeats AS LyingSeats,
       v.NumberOfAxis AS Axes, v.PropulsionAxis AS PropAxes, v.NumberOfWheels, v.NumberOfPropulsionWheels,
       v.EmptyWaight AS EmptyWeight, v.MaximunAllowedWaight AS MaxWeight,
       v.MakeDate,
       v.FirstRegistrationMakeDate, v.FirstRegistrationValidTill,
       v.LastRegistrationMakeDate,  v.LastRegistrationValidTill,
       v.ColorCode, ISNULL(v.Suffocation,0) AS Suffocation, ISNULL(v.Hook,0) AS Hook,
       ISNULL(v.Vitlo,0) AS Winch, ISNULL(v.TNG,0) AS TNG, v.Note, ISNULL(v.Active,1) AS Active,
       bt.BodytypeDescriprion AS BodyTypeName,
       cat.CategoryName     AS CategoryName,
       u.UseDescription     AS UseName,
       m.ModelName          AS ModelName,
       co.CountryName       AS MadeCountryName,
       cfp.Name             AS CategoryForPaymentsName,
       et.EngineTypeCode    AS EngineTypeName,
       eps.PowerSourceName  AS PowerSourceName,
       sps.PowerSourceName  AS SecondPowerSourceName,
       eep.EcoProgram       AS EcoProgramName,
       gb.GearBoxDescription AS GearBoxName,
       br.BreakesDescription AS BrakesName,
       sp.SupportingDescription AS SupportingName,
       pc.ColorDescription  AS PrimaryColorName,
       sc.ColorDescription  AS SecondaryColorName,
       fri.IssuerName       AS FirstIssuerName,
       lri.IssuerName       AS LastIssuerName,
       -- Print-overlay columns
       v.Tip, v.BrojEUPotvrda, v.OznakaNaOdobrenie, v.OznakaNaOdobrenieZaPriklucUred,
       v.IdentifikacijaNaMotorMestoMetod,
       v.TBrOdobrenieMehanPriklucok, v.TMarkaMehanPriklucok, v.TTipMehanPriklucok,
       v.TZastitnaKabina, v.TZastitnaRamka, v.NoiseTechnicalSpec, v.OdnosKwCcm,
       v.EnginePowerOutPut, v.NoiseStatic, v.CO2, v.MaxSpeed,
       v.MaxKonstVkMasa, v.MaxLegVkMasa, v.MaxLegVkMasaGrupa,
       v.MasaPoOska1, v.MasaPoOska2, v.MasaPoOska3, v.MasaPoOska4, v.MasaPoOska5,
       v.OsnoOptovaruvanje1, v.OsnoOptovaruvanje2, v.OsnoOptovaruvanje3, v.OsnoOptovaruvanje4, v.OsnoOptovaruvanje5,
       v.MaxKonstOptovaruvanjeVoPriklucok, v.TMaxHorVerOptovaruvanjePriklucok,
       v.TMaxKonstVkMasaNaKombinacija, v.TMaxKonstVkMasaPoluprikolka, v.TMaxKonstVkMasaPrikolka,
       v.TMaxKonstVkMasaPrikolkaSoCenOska, v.TMaxKonstVkMasaPrikolkaStoMozePrikluci, v.TMinMasa
FROM dbo.Vehicles v
LEFT JOIN dbo.VehicleBodytype                 bt  ON bt.Id  = v.IdVehicleBodyType
LEFT JOIN dbo.VehicleCategories               cat ON cat.Id = v.IdVehicleCategories
LEFT JOIN dbo.VehicleUse                      u   ON u.Id   = v.IdVehicleUse
LEFT JOIN dbo.VehicleModel                    m   ON m.Id   = v.IdVehicleModel
LEFT JOIN dbo.Countries                       co  ON co.Id  = v.IdMadeCountry
LEFT JOIN dbo.VehicleCategoryForPayments      cfp ON cfp.Id = v.IdVehicleCategoryForPayments
LEFT JOIN dbo.VehicleEngineTypes              et  ON et.Id  = v.IdEngineType
LEFT JOIN dbo.VehicleEnginePowerSourceTypes   eps ON eps.Id = v.IdEnginePowerSource
LEFT JOIN dbo.VehicleEnginePowerSourceTypes   sps ON sps.Id = v.IdEngineSecondPowerSource
LEFT JOIN dbo.VehicleEngineEcoProgram         eep ON eep.Id = v.IdEngineEcoProgram
LEFT JOIN dbo.VehicleGearBox                  gb  ON gb.Id  = v.IdGearBox
LEFT JOIN dbo.VehicleBrakes                   br  ON br.Id  = v.IdBreakes
LEFT JOIN dbo.VehicleSupporting               sp  ON sp.Id  = v.IdSupporting
LEFT JOIN dbo.Colors                          pc  ON pc.Id  = v.IdPrimaryColor
LEFT JOIN dbo.Colors                          sc  ON sc.Id  = v.IdSecondaryColor
LEFT JOIN dbo.RegistrationIssuers             fri ON fri.Id = v.IdFirstRegistrationIssuer
LEFT JOIN dbo.RegistrationIssuers             lri ON lri.Id = v.IdLastRegistrationIssuer
WHERE v.Id > @max
ORDER BY v.Id;";
            cmd.Parameters.AddWithValue("@max", vteMax);
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            while (await rdr.ReadAsync(ct)) rows.Add(LegacyVehicle.Read(rdr));
        }

        if (rows.Count == 0)
            return new MigrationResult(0, 0, vteMax, await _db.Vehicles.IgnoreQueryFilters().CountAsync(ct), Array.Empty<MigrationSkip>());

        var mBody = await BuildRefMapAsync("VehicleBodyTypes", ct);
        var mCat  = await BuildRefMapAsync("VehicleCategories", ct);
        var mUse  = await BuildRefMapAsync("VehicleUses", ct);
        var mModel= await BuildRefMapAsync("VehicleModels", ct);
        var mCo   = await BuildRefMapAsync("Countries", ct);
        var mCfp  = await BuildRefMapAsync("VehicleCategoriesForPayments", ct);
        var mEng  = await BuildRefMapAsync("VehicleEngineTypes", ct);
        var mPs   = await BuildRefMapAsync("VehicleEnginePowerSourceTypes", ct);
        var mEco  = await BuildRefMapAsync("VehicleEngineEcoPrograms", ct);
        var mGb   = await BuildRefMapAsync("VehicleGearBoxes", ct);
        var mBr   = await BuildRefMapAsync("VehicleBrakes", ct);
        var mSup  = await BuildRefMapAsync("VehicleSupportings", ct);
        var mClr  = await BuildRefMapAsync("Colors", ct);
        var mRI   = await BuildRefMapAsync("RegistrationIssuers", ct);

        int inserted = 0; var skips = new List<MigrationSkip>();
        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        await using (var idOn = conn.CreateCommand()) { idOn.CommandText = "SET IDENTITY_INSERT dbo.Vehicles ON;"; await idOn.ExecuteNonQueryAsync(ct); }
        try
        {
            foreach (var v in rows)
            {
                if (string.IsNullOrWhiteSpace(v.ShellNumber)) { skips.Add(new MigrationSkip(v.Id, "Empty ShellNumber.")); continue; }
                try
                {
                    await using var ins = conn.CreateCommand();
                    ins.CommandText = VehicleInsertSql;
                    BindVehicle(ins, v, _stationId,
                        mBody, mCat, mUse, mModel, mCo, mCfp, mEng, mPs, mEco, mGb, mBr, mSup, mClr, mRI);
                    await ins.ExecuteNonQueryAsync(ct);
                    inserted++;
                }
                catch (Exception ex) { skips.Add(new MigrationSkip(v.Id, FirstLine(ex.Message))); }
            }
        }
        finally
        {
            await using var idOff = conn.CreateCommand();
            idOff.CommandText = "SET IDENTITY_INSERT dbo.Vehicles OFF;";
            await idOff.ExecuteNonQueryAsync(ct);
        }

        await using (var reseed = conn.CreateCommand()) { reseed.CommandText = "DBCC CHECKIDENT('dbo.Vehicles', RESEED);"; await reseed.ExecuteNonQueryAsync(ct); }

        var newMax = await _db.Vehicles.IgnoreQueryFilters().MaxAsync(v => (long?)v.Id, ct) ?? 0;
        var total  = await _db.Vehicles.IgnoreQueryFilters().CountAsync(ct);
        return new MigrationResult(inserted, skips.Count, newMax, total, skips);
    }

    // Copies the 35 print-overlay columns from legacy VTEZVV.dbo.Vehicles into the local
    // Vehicles table for every row that already exists locally. Idempotent — re-running
    // overwrites the values with whatever's currently in legacy. Processes rows in chunks
    // of 1000 so we don't blow the SQL parameter limit.
    public async Task<BackfillResult> BackfillVehiclePrintColumnsAsync(CancellationToken ct = default)
    {
        var localIds = await _db.Vehicles.IgnoreQueryFilters().Select(v => v.Id).OrderBy(x => x).ToListAsync(ct);
        if (localIds.Count == 0) return new BackfillResult(0, 0, 0, Array.Empty<string>());

        var errors = new List<string>(); var updated = 0; var legacyFound = 0;
        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        const int chunkSize = 1000;
        for (var i = 0; i < localIds.Count; i += chunkSize)
        {
            var chunk = localIds.GetRange(i, Math.Min(chunkSize, localIds.Count - i));
            var inList = string.Join(",", chunk);

            await using var lcn = new SqlConnection(_legacyCs);
            await lcn.OpenAsync(ct);
            await using var cmd = lcn.CreateCommand();
            cmd.CommandText = $@"
SELECT Id, Tip, BrojEUPotvrda, OznakaNaOdobrenie, OznakaNaOdobrenieZaPriklucUred,
       IdentifikacijaNaMotorMestoMetod, TBrOdobrenieMehanPriklucok, TMarkaMehanPriklucok,
       TTipMehanPriklucok, TZastitnaKabina, TZastitnaRamka, NoiseTechnicalSpec, OdnosKwCcm,
       EnginePowerOutPut, NoiseStatic, CO2, MaxSpeed,
       MaxKonstVkMasa, MaxLegVkMasa, MaxLegVkMasaGrupa,
       MasaPoOska1, MasaPoOska2, MasaPoOska3, MasaPoOska4, MasaPoOska5,
       OsnoOptovaruvanje1, OsnoOptovaruvanje2, OsnoOptovaruvanje3, OsnoOptovaruvanje4, OsnoOptovaruvanje5,
       MaxKonstOptovaruvanjeVoPriklucok, TMaxHorVerOptovaruvanjePriklucok,
       TMaxKonstVkMasaNaKombinacija, TMaxKonstVkMasaPoluprikolka, TMaxKonstVkMasaPrikolka,
       TMaxKonstVkMasaPrikolkaSoCenOska, TMaxKonstVkMasaPrikolkaStoMozePrikluci, TMinMasa
FROM dbo.Vehicles WHERE Id IN ({inList})";
            cmd.CommandTimeout = 120;

            var rows = new List<Dictionary<string, object?>>();
            await using (var rdr = await cmd.ExecuteReaderAsync(ct))
            {
                while (await rdr.ReadAsync(ct))
                {
                    var dict = new Dictionary<string, object?>(rdr.FieldCount);
                    for (var f = 0; f < rdr.FieldCount; f++)
                        dict[rdr.GetName(f)] = rdr.IsDBNull(f) ? null : rdr.GetValue(f);
                    rows.Add(dict);
                }
            }
            legacyFound += rows.Count;

            foreach (var row in rows)
            {
                try
                {
                    await using var upd = conn.CreateCommand();
                    upd.CommandText = @"
UPDATE dbo.Vehicles SET
    Tip=@Tip, BrojEUPotvrda=@BrojEUPotvrda, OznakaNaOdobrenie=@OznakaNaOdobrenie,
    OznakaNaOdobrenieZaPriklucUred=@OznakaNaOdobrenieZaPriklucUred,
    IdentifikacijaNaMotorMestoMetod=@IdentifikacijaNaMotorMestoMetod,
    TBrOdobrenieMehanPriklucok=@TBrOdobrenieMehanPriklucok, TMarkaMehanPriklucok=@TMarkaMehanPriklucok,
    TTipMehanPriklucok=@TTipMehanPriklucok, TZastitnaKabina=@TZastitnaKabina, TZastitnaRamka=@TZastitnaRamka,
    NoiseTechnicalSpec=@NoiseTechnicalSpec, OdnosKwCcm=@OdnosKwCcm,
    EnginePowerOutPut=@EnginePowerOutPut, NoiseStatic=@NoiseStatic, CO2=@CO2, MaxSpeed=@MaxSpeed,
    MaxKonstVkMasa=@MaxKonstVkMasa, MaxLegVkMasa=@MaxLegVkMasa, MaxLegVkMasaGrupa=@MaxLegVkMasaGrupa,
    MasaPoOska1=@MasaPoOska1, MasaPoOska2=@MasaPoOska2, MasaPoOska3=@MasaPoOska3,
    MasaPoOska4=@MasaPoOska4, MasaPoOska5=@MasaPoOska5,
    OsnoOptovaruvanje1=@OsnoOptovaruvanje1, OsnoOptovaruvanje2=@OsnoOptovaruvanje2,
    OsnoOptovaruvanje3=@OsnoOptovaruvanje3, OsnoOptovaruvanje4=@OsnoOptovaruvanje4,
    OsnoOptovaruvanje5=@OsnoOptovaruvanje5,
    MaxKonstOptovaruvanjeVoPriklucok=@MaxKonstOptovaruvanjeVoPriklucok,
    TMaxHorVerOptovaruvanjePriklucok=@TMaxHorVerOptovaruvanjePriklucok,
    TMaxKonstVkMasaNaKombinacija=@TMaxKonstVkMasaNaKombinacija,
    TMaxKonstVkMasaPoluprikolka=@TMaxKonstVkMasaPoluprikolka,
    TMaxKonstVkMasaPrikolka=@TMaxKonstVkMasaPrikolka,
    TMaxKonstVkMasaPrikolkaSoCenOska=@TMaxKonstVkMasaPrikolkaSoCenOska,
    TMaxKonstVkMasaPrikolkaStoMozePrikluci=@TMaxKonstVkMasaPrikolkaStoMozePrikluci,
    TMinMasa=@TMinMasa
WHERE Id=@Id";
                    foreach (var kv in row)
                    {
                        var p = upd.CreateParameter();
                        p.ParameterName = "@" + kv.Key;
                        var v = kv.Value;
                        // legacy real/float -> decimal in local schema; clamp NaN/Infinity/overflow to null
                        if (v is float fv)  v = SafeDec(fv);
                        else if (v is double dv) v = SafeDec(dv);
                        p.Value = v ?? DBNull.Value;
                        upd.Parameters.Add(p);
                    }
                    var n = await upd.ExecuteNonQueryAsync(ct);
                    if (n > 0) updated++;
                }
                catch (Exception ex)
                {
                    errors.Add($"id={row["Id"]}: {FirstLine(ex.Message)}");
                }
            }
        }

        return new BackfillResult(updated, legacyFound, localIds.Count, errors);
    }

    public async Task<MigrationResult> MigrateNewerRelationsAsync(CancellationToken ct = default)
    {
        var status = await GetRelationsStatusAsync(ct);
        var vteMax = status.VteMaxId;

        var rows = new List<(long Id, long CustomerId, long VehicleId, string? RelTypeName, DateTime? Start, DateTime? End, string? Note, bool Active)>();
        await using (var lcn = new SqlConnection(_legacyCs))
        {
            await lcn.OpenAsync(ct);
            await using var cmd = lcn.CreateCommand();
            cmd.CommandText = @"
SELECT r.Id, r.IdCustomer, r.IdVehicle, rt.RelationTypeName, r.StartDate, r.EndDate, r.BeginNote, ISNULL(r.Active, 1) AS Active
FROM dbo.CustomerVehiclesRelations r
LEFT JOIN dbo.CustomerVehiclesRelationTypes rt ON rt.Id = r.IdRelationType
WHERE r.Id > @max
ORDER BY r.Id;";
            cmd.Parameters.AddWithValue("@max", vteMax);
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            while (await rdr.ReadAsync(ct))
            {
                rows.Add((
                    rdr.GetInt64(0),
                    rdr.GetInt64(1),
                    rdr.GetInt64(2),
                    rdr.IsDBNull(3) ? null : rdr.GetString(3),
                    rdr.IsDBNull(4) ? (DateTime?)null : rdr.GetDateTime(4),
                    rdr.IsDBNull(5) ? (DateTime?)null : rdr.GetDateTime(5),
                    rdr.IsDBNull(6) ? null : rdr.GetString(6),
                    !rdr.IsDBNull(7) && rdr.GetBoolean(7)));
            }
        }

        if (rows.Count == 0)
            return new MigrationResult(0, 0, vteMax, await _db.CustomerVehicleRelations.CountAsync(ct), Array.Empty<MigrationSkip>());

        var mType = await BuildRefMapAsync("CustomerVehicleRelationTypes", ct);
        // Load the set of valid Customer + Vehicle Ids so we can flag dangling FKs early.
        var custIds = new HashSet<long>(await _db.Customers.IgnoreQueryFilters().Select(c => c.Id).ToListAsync(ct));
        var vehIds  = new HashSet<long>(await _db.Vehicles.IgnoreQueryFilters().Select(v => v.Id).ToListAsync(ct));

        int inserted = 0; var skips = new List<MigrationSkip>();
        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        await using (var idOn = conn.CreateCommand()) { idOn.CommandText = "SET IDENTITY_INSERT dbo.CustomerVehicleRelations ON;"; await idOn.ExecuteNonQueryAsync(ct); }
        try
        {
            foreach (var r in rows)
            {
                if (!custIds.Contains(r.CustomerId)) { skips.Add(new MigrationSkip(r.Id, $"Customer {r.CustomerId} not in VTE2 — migrate Customers first.")); continue; }
                if (!vehIds.Contains(r.VehicleId))   { skips.Add(new MigrationSkip(r.Id, $"Vehicle {r.VehicleId} not in VTE2 — migrate Vehicles first.")); continue; }
                try
                {
                    await using var ins = conn.CreateCommand();
                    ins.CommandText = @"
INSERT INTO dbo.CustomerVehicleRelations (Id, CustomerId, VehicleId, RelationTypeId, ValidFrom, ValidTo, Note, IsActive)
VALUES (@Id, @Cust, @Veh, @Type, @From, @To, @Note, @Active);";
                    void P(string n, object? v) { var p = ins.CreateParameter(); p.ParameterName = n; p.Value = v ?? DBNull.Value; ins.Parameters.Add(p); }
                    P("@Id", r.Id);
                    P("@Cust", r.CustomerId);
                    P("@Veh", r.VehicleId);
                    P("@Type", (object?)LookupId(mType, r.RelTypeName));
                    P("@From", DateValue(r.Start));
                    P("@To",   DateValue(r.End));
                    P("@Note", NullIfBlank(r.Note));
                    P("@Active", r.Active);
                    await ins.ExecuteNonQueryAsync(ct);
                    inserted++;
                }
                catch (Exception ex) { skips.Add(new MigrationSkip(r.Id, FirstLine(ex.Message))); }
            }
        }
        finally
        {
            await using var idOff = conn.CreateCommand();
            idOff.CommandText = "SET IDENTITY_INSERT dbo.CustomerVehicleRelations OFF;";
            await idOff.ExecuteNonQueryAsync(ct);
        }

        await using (var reseed = conn.CreateCommand()) { reseed.CommandText = "DBCC CHECKIDENT('dbo.CustomerVehicleRelations', RESEED);"; await reseed.ExecuteNonQueryAsync(ct); }

        var newMax = await _db.CustomerVehicleRelations.MaxAsync(r => (long?)r.Id, ct) ?? 0;
        var total  = await _db.CustomerVehicleRelations.CountAsync(ct);
        return new MigrationResult(inserted, skips.Count, newMax, total, skips);
    }

    // ---------- helpers ----------

    private async Task<Dictionary<string, int>> BuildRefMapAsync(string table, CancellationToken ct)
    {
        // Prefer active rows; case-insensitive on the name. First-active-wins on conflict.
        var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT Id, Name, IsActive FROM dbo.{table} ORDER BY IsActive DESC, Id";
        await using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            if (rdr.IsDBNull(1)) continue;
            var name = rdr.GetString(1);
            if (!map.ContainsKey(name)) map[name] = rdr.GetInt32(0);
        }
        return map;
    }

    private static int? LookupId(IReadOnlyDictionary<string, int> map, string? name)
        => !string.IsNullOrWhiteSpace(name) && map.TryGetValue(name!, out var id) ? id : null;

    private static string FirstLine(string s) => s.Split('\n', 2)[0].Trim();

    // ---------- SQL + binding ----------

    private const string InsertSql = @"
INSERT INTO dbo.Customers (
    Id, StationId, IsCompany, EMBG, FirstName, Surname, ParentName, DateOfBirth,
    CitizenshipId, BusinessTypeId,
    LivingAddressId, LivingAddressNumber, LivingCityId,
    BirthCityId, BirthAddressId, BirthAddressNumber,
    Occupation, WorksInCompany,
    PhoneNumber, Fax, Email, CanSendNotifications,
    IDCardNumber, IDCardDateIssued, IDCardIssuerId,
    PassportNumber, PassportDateIssued, PassportIssuerId,
    DrivingLicenceNumber, DrivingLicenceDateIssued, DrivingLicenceIssuerId,
    TaxNumber, Status, Note, IsActive)
VALUES (
    @Id, @StationId, @IsCompany, @EMBG, @FirstName, @Surname, @ParentName, @DateOfBirth,
    @CitizenshipId, @BusinessTypeId,
    @LivingAddressId, @LivingAddressNumber, @LivingCityId,
    @BirthCityId, @BirthAddressId, @BirthAddressNumber,
    @Occupation, @WorksInCompany,
    @PhoneNumber, @Fax, @Email, @CanSendNotifications,
    @IDCardNumber, @IDCardDateIssued, @IDCardIssuerId,
    @PassportNumber, @PassportDateIssued, @PassportIssuerId,
    @DrivingLicenceNumber, @DrivingLicenceDateIssued, @DrivingLicenceIssuerId,
    @TaxNumber, @Status, @Note, @IsActive);";

    private static void Bind(System.Data.Common.DbCommand cmd, LegacyCustomer r, int stationId,
        IReadOnlyDictionary<string, int> mCity, IReadOnlyDictionary<string, int> mStr,
        IReadOnlyDictionary<string, int> mBT,   IReadOnlyDictionary<string, int> mCo,
        IReadOnlyDictionary<string, int> mRI)
    {
        void P(string name, object? value)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        P("@Id",                       r.Id);
        P("@StationId",                stationId);
        P("@IsCompany",                r.IsCompany);
        P("@EMBG",                     NullIfBlank(r.MB));
        P("@FirstName",                r.CustomerFirstName ?? string.Empty);
        P("@Surname",                  NullIfBlank(r.CustomerSurname));
        P("@ParentName",               NullIfBlank(r.ParentName));
        P("@DateOfBirth",              DateValue(r.DateOfBirth));
        P("@CitizenshipId",            (object?)LookupId(mCo, r.CitizenshipName));
        P("@BusinessTypeId",           (object?)LookupId(mBT, r.BusinessTypeName));
        P("@LivingAddressId",          (object?)LookupId(mStr, r.LivingStreetName));
        P("@LivingAddressNumber",      NullIfBlank(r.LivingAddressNumber));
        P("@LivingCityId",             (object?)LookupId(mCity, r.LivingCityName));
        P("@BirthCityId",              (object?)LookupId(mCity, r.BirthCityName));
        P("@BirthAddressId",           (object?)LookupId(mStr, r.BirthStreetName));
        P("@BirthAddressNumber",       NullIfBlank(r.BirthAddressNumber));
        P("@Occupation",               NullIfBlank(r.Occupation));
        P("@WorksInCompany",           NullIfBlank(r.WorksInCompany));
        P("@PhoneNumber",              NullIfBlank(r.PhoneNumber));
        P("@Fax",                      NullIfBlank(r.Fax));
        P("@Email",                    NullIfBlank(r.Email));
        P("@CanSendNotifications",     r.CanSendNotifications);
        P("@IDCardNumber",             NullIfBlank(r.BLK));
        P("@IDCardDateIssued",         DateValue(r.BLKDateIssued));
        P("@IDCardIssuerId",           (object?)LookupId(mRI, r.IDCardIssuerName));
        P("@PassportNumber",           NullIfBlank(r.PassportNumber));
        P("@PassportDateIssued",       DateValue(r.PassDateIssued));
        P("@PassportIssuerId",         (object?)LookupId(mRI, r.PassportIssuerName));
        P("@DrivingLicenceNumber",     NullIfBlank(r.DriveingLicenceNumber));
        P("@DrivingLicenceDateIssued", DateValue(r.DriveingLicenceDateIssued));
        P("@DrivingLicenceIssuerId",   (object?)LookupId(mRI, r.DLIssuerName));
        P("@TaxNumber",                NullIfBlank(r.TaxNumber));
        P("@Status",                   NullIfBlank(r.Status));
        P("@Note",                     NullIfBlank(r.Note));
        P("@IsActive",                 r.Active);
    }

    private static object? NullIfBlank(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
    private static object? DateValue(DateTime? d) => d is null || d.Value.Year <= 1900 ? null : DateOnly.FromDateTime(d.Value);

    // POCO that maps the joined SELECT — kept private to this service.
    private sealed class LegacyCustomer
    {
        public long Id;
        public string? MB;
        public string? CustomerSurname;
        public string? CustomerFirstName;
        public string? PhoneNumber;
        public string? Fax;
        public string? Email;
        public string? LivingAddressNumber;
        public string? BirthAddressNumber;
        public DateTime? DateOfBirth;
        public bool IsCompany;
        public string? Occupation;
        public string? WorksInCompany;
        public string? ParentName;
        public string? PassportNumber;
        public string? BLK;
        public bool CanSendNotifications;
        public string? TaxNumber;
        public DateTime? BLKDateIssued;
        public string? DriveingLicenceNumber;
        public DateTime? DriveingLicenceDateIssued;
        public DateTime? PassDateIssued;
        public bool Active;
        public string? Note;
        public string? Status;
        public string? LivingCityName;
        public string? LivingStreetName;
        public string? BirthCityName;
        public string? BirthStreetName;
        public string? BusinessTypeName;
        public string? CitizenshipName;
        public string? IDCardIssuerName;
        public string? PassportIssuerName;
        public string? DLIssuerName;

        public static LegacyCustomer Read(SqlDataReader r)
        {
            string? S(string col) { var i = r.GetOrdinal(col); return r.IsDBNull(i) ? null : r.GetString(i); }
            DateTime? D(string col) { var i = r.GetOrdinal(col); return r.IsDBNull(i) ? (DateTime?)null : r.GetDateTime(i); }
            bool B(string col) { var i = r.GetOrdinal(col); return !r.IsDBNull(i) && r.GetBoolean(i); }
            long L(string col) { var i = r.GetOrdinal(col); return r.GetInt64(i); }

            return new LegacyCustomer
            {
                Id                        = L("Id"),
                MB                        = S("MB"),
                CustomerSurname           = S("CustomerSurname"),
                CustomerFirstName         = S("CustomerFirstName"),
                PhoneNumber               = S("PhoneNumber"),
                Fax                       = S("Fax"),
                Email                     = S("eMail"),
                LivingAddressNumber       = S("LivingAddressNumber"),
                BirthAddressNumber        = S("BrithAddressNumber"),
                DateOfBirth               = D("DateOfBirth"),
                IsCompany                 = B("IsCompany"),
                Occupation                = S("Occupation"),
                WorksInCompany            = S("WorksInCompany"),
                ParentName                = S("ParentName"),
                PassportNumber            = S("PassportNumber"),
                BLK                       = S("BLK"),
                CanSendNotifications      = B("CanSendNotifications"),
                TaxNumber                 = S("TaxNumber"),
                BLKDateIssued             = D("BLKDateIssued"),
                DriveingLicenceNumber     = S("DriveingLicenceNumber"),
                DriveingLicenceDateIssued = D("DriveingLicenceDateIssued"),
                PassDateIssued            = D("PassDateIssued"),
                Active                    = B("Active"),
                Note                      = S("Note"),
                Status                    = S("Status"),
                LivingCityName            = S("LivingCityName"),
                LivingStreetName          = S("LivingStreetName"),
                BirthCityName             = S("BirthCityName"),
                BirthStreetName           = S("BirthStreetName"),
                BusinessTypeName          = S("BusinessTypeName"),
                CitizenshipName           = S("CitizenshipName"),
                IDCardIssuerName          = S("IDCardIssuerName"),
                PassportIssuerName        = S("PassportIssuerName"),
                DLIssuerName              = S("DLIssuerName"),
            };
        }
    }

    // Vehicle insert SQL + POCO + binder — kept together with the customer ones above.
    private const string VehicleInsertSql = @"
INSERT INTO dbo.Vehicles (
    Id, StationId, ShellNumber,
    BodyTypeId, VehicleCategoryId, VehicleUseId, VehicleModelId, VehicleModelAdding,
    MadeCountryId, VehicleCategoryForPaymentsId,
    EngineNumber, EngineTypeId, EnginePowerSourceId, EngineSecondPowerSourceId, EngineEcoProgramId,
    EnginePowerKw, EngineWorkingCapacity, RPM, GearBoxId,
    BrakesId, SupportingId,
    VehicleHeight, VehicleWidth, VehicleLength,
    NumberOfDoors, NumberOfSeats, NumberOfStandingSeats, NumberOfLyingSeats,
    NumberOfAxes, NumberOfPropulsionAxes, NumberOfWheels, NumberOfPropulsionWheels,
    EmptyWeight, MaxAllowedWeight,
    MakeDate,
    FirstRegistrationNumber, LastRegistrationNumber,
    FirstRegistrationMakeDate, FirstRegistrationValidTill,
    LastRegistrationMakeDate, LastRegistrationValidTill,
    FirstRegistrationIssuerId, LastRegistrationIssuerId,
    ColorCode, PrimaryColorId, SecondaryColorId,
    Suffocation, Hook, Winch, TNG,
    Note, IsActive,
    Tip, BrojEUPotvrda, OznakaNaOdobrenie, OznakaNaOdobrenieZaPriklucUred, IdentifikacijaNaMotorMestoMetod,
    TBrOdobrenieMehanPriklucok, TMarkaMehanPriklucok, TTipMehanPriklucok,
    TZastitnaKabina, TZastitnaRamka, NoiseTechnicalSpec, OdnosKwCcm,
    EnginePowerOutPut, NoiseStatic, CO2, MaxSpeed,
    MaxKonstVkMasa, MaxLegVkMasa, MaxLegVkMasaGrupa,
    MasaPoOska1, MasaPoOska2, MasaPoOska3, MasaPoOska4, MasaPoOska5,
    OsnoOptovaruvanje1, OsnoOptovaruvanje2, OsnoOptovaruvanje3, OsnoOptovaruvanje4, OsnoOptovaruvanje5,
    MaxKonstOptovaruvanjeVoPriklucok, TMaxHorVerOptovaruvanjePriklucok,
    TMaxKonstVkMasaNaKombinacija, TMaxKonstVkMasaPoluprikolka, TMaxKonstVkMasaPrikolka,
    TMaxKonstVkMasaPrikolkaSoCenOska, TMaxKonstVkMasaPrikolkaStoMozePrikluci, TMinMasa)
VALUES (
    @Id, @StationId, @ShellNumber,
    @BodyTypeId, @VehicleCategoryId, @VehicleUseId, @VehicleModelId, @VehicleModelAdding,
    @MadeCountryId, @VehicleCategoryForPaymentsId,
    @EngineNumber, @EngineTypeId, @EnginePowerSourceId, @EngineSecondPowerSourceId, @EngineEcoProgramId,
    @EnginePowerKw, @EngineWorkingCapacity, @RPM, @GearBoxId,
    @BrakesId, @SupportingId,
    @VehicleHeight, @VehicleWidth, @VehicleLength,
    @NumberOfDoors, @NumberOfSeats, @NumberOfStandingSeats, @NumberOfLyingSeats,
    @NumberOfAxes, @NumberOfPropulsionAxes, @NumberOfWheels, @NumberOfPropulsionWheels,
    @EmptyWeight, @MaxAllowedWeight,
    @MakeDate,
    @FirstRegistrationNumber, @LastRegistrationNumber,
    @FirstRegistrationMakeDate, @FirstRegistrationValidTill,
    @LastRegistrationMakeDate, @LastRegistrationValidTill,
    @FirstRegistrationIssuerId, @LastRegistrationIssuerId,
    @ColorCode, @PrimaryColorId, @SecondaryColorId,
    @Suffocation, @Hook, @Winch, @TNG,
    @Note, @IsActive,
    @Tip, @BrojEUPotvrda, @OznakaNaOdobrenie, @OznakaNaOdobrenieZaPriklucUred, @IdentifikacijaNaMotorMestoMetod,
    @TBrOdobrenieMehanPriklucok, @TMarkaMehanPriklucok, @TTipMehanPriklucok,
    @TZastitnaKabina, @TZastitnaRamka, @NoiseTechnicalSpec, @OdnosKwCcm,
    @EnginePowerOutPut, @NoiseStatic, @CO2, @MaxSpeed,
    @MaxKonstVkMasa, @MaxLegVkMasa, @MaxLegVkMasaGrupa,
    @MasaPoOska1, @MasaPoOska2, @MasaPoOska3, @MasaPoOska4, @MasaPoOska5,
    @OsnoOptovaruvanje1, @OsnoOptovaruvanje2, @OsnoOptovaruvanje3, @OsnoOptovaruvanje4, @OsnoOptovaruvanje5,
    @MaxKonstOptovaruvanjeVoPriklucok, @TMaxHorVerOptovaruvanjePriklucok,
    @TMaxKonstVkMasaNaKombinacija, @TMaxKonstVkMasaPoluprikolka, @TMaxKonstVkMasaPrikolka,
    @TMaxKonstVkMasaPrikolkaSoCenOska, @TMaxKonstVkMasaPrikolkaStoMozePrikluci, @TMinMasa);";

    private static void BindVehicle(System.Data.Common.DbCommand cmd, LegacyVehicle v, int stationId,
        IReadOnlyDictionary<string, int> mBody, IReadOnlyDictionary<string, int> mCat,
        IReadOnlyDictionary<string, int> mUse,  IReadOnlyDictionary<string, int> mModel,
        IReadOnlyDictionary<string, int> mCo,   IReadOnlyDictionary<string, int> mCfp,
        IReadOnlyDictionary<string, int> mEng,  IReadOnlyDictionary<string, int> mPs,
        IReadOnlyDictionary<string, int> mEco,  IReadOnlyDictionary<string, int> mGb,
        IReadOnlyDictionary<string, int> mBr,   IReadOnlyDictionary<string, int> mSup,
        IReadOnlyDictionary<string, int> mClr,  IReadOnlyDictionary<string, int> mRI)
    {
        void P(string n, object? val) { var p = cmd.CreateParameter(); p.ParameterName = n; p.Value = val ?? DBNull.Value; cmd.Parameters.Add(p); }

        P("@Id",                          v.Id);
        P("@StationId",                   stationId);
        P("@ShellNumber",                 v.ShellNumber);
        P("@BodyTypeId",                  (object?)LookupId(mBody, v.BodyTypeName));
        P("@VehicleCategoryId",           (object?)LookupId(mCat,  v.CategoryName));
        P("@VehicleUseId",                (object?)LookupId(mUse,  v.UseName));
        P("@VehicleModelId",              (object?)LookupId(mModel,v.ModelName));
        P("@VehicleModelAdding",          NullIfBlank(v.VehicleModelAdding));
        P("@MadeCountryId",               (object?)LookupId(mCo,   v.MadeCountryName));
        P("@VehicleCategoryForPaymentsId",(object?)LookupId(mCfp,  v.CategoryForPaymentsName));
        P("@EngineNumber",                NullIfBlank(v.EngineNumber));
        P("@EngineTypeId",                (object?)LookupId(mEng,  v.EngineTypeName));
        P("@EnginePowerSourceId",         (object?)LookupId(mPs,   v.PowerSourceName));
        P("@EngineSecondPowerSourceId",   (object?)LookupId(mPs,   v.SecondPowerSourceName));
        P("@EngineEcoProgramId",          (object?)LookupId(mEco,  v.EcoProgramName));
        P("@EnginePowerKw",               DecValue(v.EnginePower));
        P("@EngineWorkingCapacity",       DecValue(v.EngineWorkingCapacity));
        P("@RPM",                         v.RPM);
        P("@GearBoxId",                   (object?)LookupId(mGb,   v.GearBoxName));
        P("@BrakesId",                    (object?)LookupId(mBr,   v.BrakesName));
        P("@SupportingId",                (object?)LookupId(mSup,  v.SupportingName));
        P("@VehicleHeight",               DecValue(v.H));
        P("@VehicleWidth",                DecValue(v.W));
        P("@VehicleLength",               DecValue(v.L));
        P("@NumberOfDoors",               v.NumberOfDoors);
        P("@NumberOfSeats",               v.NumberOfSeats);
        P("@NumberOfStandingSeats",       v.NumberOfStandingSeats);
        P("@NumberOfLyingSeats",          v.LyingSeats);
        P("@NumberOfAxes",                v.Axes);
        P("@NumberOfPropulsionAxes",      v.PropAxes);
        P("@NumberOfWheels",              v.NumberOfWheels);
        P("@NumberOfPropulsionWheels",    v.NumberOfPropulsionWheels);
        P("@EmptyWeight",                 DecValue(v.EmptyWeight));
        P("@MaxAllowedWeight",            DecValue(v.MaxWeight));
        P("@MakeDate",                    DateValue(v.MakeDate));
        P("@FirstRegistrationNumber",     v.FirstRegistrationNumber ?? string.Empty);
        P("@LastRegistrationNumber",      v.LastRegistrationNumber ?? string.Empty);
        P("@FirstRegistrationMakeDate",   DateValue(v.FirstRegistrationMakeDate));
        P("@FirstRegistrationValidTill",  DateValue(v.FirstRegistrationValidTill));
        P("@LastRegistrationMakeDate",    DateValue(v.LastRegistrationMakeDate));
        P("@LastRegistrationValidTill",   DateValue(v.LastRegistrationValidTill));
        P("@FirstRegistrationIssuerId",   (object?)LookupId(mRI, v.FirstIssuerName));
        P("@LastRegistrationIssuerId",    (object?)LookupId(mRI, v.LastIssuerName));
        P("@ColorCode",                   NullIfBlank(v.ColorCode));
        P("@PrimaryColorId",              (object?)LookupId(mClr, v.PrimaryColorName));
        P("@SecondaryColorId",            (object?)LookupId(mClr, v.SecondaryColorName));
        P("@Suffocation",                 v.Suffocation);
        P("@Hook",                        v.Hook);
        P("@Winch",                       v.Winch);
        P("@TNG",                         v.TNG);
        P("@Note",                        NullIfBlank(v.Note));
        P("@IsActive",                    v.Active);

        // Print-overlay columns
        P("@Tip",                                              NullIfBlank(v.Tip));
        P("@BrojEUPotvrda",                                    NullIfBlank(v.BrojEUPotvrda));
        P("@OznakaNaOdobrenie",                                NullIfBlank(v.OznakaNaOdobrenie));
        P("@OznakaNaOdobrenieZaPriklucUred",                   NullIfBlank(v.OznakaNaOdobrenieZaPriklucUred));
        P("@IdentifikacijaNaMotorMestoMetod",                  NullIfBlank(v.IdentifikacijaNaMotorMestoMetod));
        P("@TBrOdobrenieMehanPriklucok",                       NullIfBlank(v.TBrOdobrenieMehanPriklucok));
        P("@TMarkaMehanPriklucok",                             NullIfBlank(v.TMarkaMehanPriklucok));
        P("@TTipMehanPriklucok",                               NullIfBlank(v.TTipMehanPriklucok));
        P("@TZastitnaKabina",                                  NullIfBlank(v.TZastitnaKabina));
        P("@TZastitnaRamka",                                   NullIfBlank(v.TZastitnaRamka));
        P("@NoiseTechnicalSpec",                               NullIfBlank(v.NoiseTechnicalSpec));
        P("@OdnosKwCcm",                                       NullIfBlank(v.OdnosKwCcm));
        P("@EnginePowerOutPut",                                DecValue(v.EnginePowerOutPut));
        P("@NoiseStatic",                                      DecValue(v.NoiseStatic));
        P("@CO2",                                              DecValue(v.CO2));
        P("@MaxSpeed",                                         DecValue(v.MaxSpeed));
        P("@MaxKonstVkMasa",                                   DecValue(v.MaxKonstVkMasa));
        P("@MaxLegVkMasa",                                     DecValue(v.MaxLegVkMasa));
        P("@MaxLegVkMasaGrupa",                                DecValue(v.MaxLegVkMasaGrupa));
        P("@MasaPoOska1",                                      v.MasaPoOska1);
        P("@MasaPoOska2",                                      v.MasaPoOska2);
        P("@MasaPoOska3",                                      v.MasaPoOska3);
        P("@MasaPoOska4",                                      v.MasaPoOska4);
        P("@MasaPoOska5",                                      v.MasaPoOska5);
        P("@OsnoOptovaruvanje1",                               v.OsnoOptovaruvanje1);
        P("@OsnoOptovaruvanje2",                               v.OsnoOptovaruvanje2);
        P("@OsnoOptovaruvanje3",                               v.OsnoOptovaruvanje3);
        P("@OsnoOptovaruvanje4",                               v.OsnoOptovaruvanje4);
        P("@OsnoOptovaruvanje5",                               v.OsnoOptovaruvanje5);
        P("@MaxKonstOptovaruvanjeVoPriklucok",                 v.MaxKonstOptovaruvanjeVoPriklucok);
        P("@TMaxHorVerOptovaruvanjePriklucok",                 v.TMaxHorVerOptovaruvanjePriklucok);
        P("@TMaxKonstVkMasaNaKombinacija",                     v.TMaxKonstVkMasaNaKombinacija);
        P("@TMaxKonstVkMasaPoluprikolka",                      v.TMaxKonstVkMasaPoluprikolka);
        P("@TMaxKonstVkMasaPrikolka",                          v.TMaxKonstVkMasaPrikolka);
        P("@TMaxKonstVkMasaPrikolkaSoCenOska",                 v.TMaxKonstVkMasaPrikolkaSoCenOska);
        P("@TMaxKonstVkMasaPrikolkaStoMozePrikluci",           v.TMaxKonstVkMasaPrikolkaStoMozePrikluci);
        P("@TMinMasa",                                         v.TMinMasa);
    }

    // Clamp NaN/Infinity/out-of-range float|double to null so a couple of garbage rows in
    // the legacy DB don't blow up an otherwise good batch insert/backfill.
    private static object? DecValue(float? f)  => f.HasValue ? SafeDec(f.Value)  : null;
    private static object? DecValue(double? d) => d.HasValue ? SafeDec(d.Value)  : null;
    private static object? SafeDec(double v)
    {
        if (double.IsNaN(v) || double.IsInfinity(v)) return null;
        // Round to 4 decimals: the legacy `real` columns hold ~7 significant float digits,
        // which when cast to decimal explodes the scale and trips SQL Server's
        // arithmetic-overflow check against decimal(18,2|4) target columns.
        try { return Math.Round((decimal)v, 4, MidpointRounding.AwayFromZero); } catch (OverflowException) { return null; }
    }

    // ===========================================================================
    // Payment catalog migration. One-shot import of PaymentTypes flag columns,
    // PaymentCategories, PaymentItems, PaymentItemParametars, PaymentDocumentNumbers.
    // The first four are reference data; PaymentDocumentNumbers is the per-station
    // sequence table. All idempotent — safe to re-run.
    // ===========================================================================
    public async Task<PaymentCatalogMigrationResult> MigratePaymentCatalogAsync(CancellationToken ct = default)
    {
        var errors = new List<string>();
        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        // ---- 1. PaymentTypes flag columns (UPDATE only — rows already exist) ----
        var paymentTypesUpdated = 0;
        await using (var lcn = new SqlConnection(_legacyCs))
        {
            await lcn.OpenAsync(ct);
            await using var cmd = lcn.CreateCommand();
            cmd.CommandText = @"SELECT Id, Name, ISNULL(Fiskalna_kes,0) AS IsCash, ISNULL(Fiskalna_karticka,0) AS IsFiscalCard,
                                       ISNULL(Rati,0) AS IsInstallments, ISNULL(Smetka,0) AS IsAccount,
                                       ISNULL(Faktura,0) AS IsInvoice, PrintText, Prefix, ISNULL(PayedAmount,0) AS PayedAmount,
                                       ISNULL(Active,1) AS Active
                                FROM dbo.PaymentTypes";
            await using var rdr = await cmd.ExecuteReaderAsync(ct);
            while (await rdr.ReadAsync(ct))
            {
                var id = rdr.GetInt32(0);
                await using var upd = conn.CreateCommand();
                upd.CommandText = @"UPDATE dbo.PaymentTypes SET
                                        IsCash=@IsCash, IsFiscalCard=@IsFiscalCard, IsInstallments=@IsInst,
                                        IsAccount=@IsAccount, IsInvoice=@IsInvoice, PrintText=@PrintText,
                                        Prefix=@Prefix, PayedAmount=@Payed
                                    WHERE Id=@Id";
                Add(upd, "@Id", id);
                Add(upd, "@IsCash", rdr.GetBoolean(2));
                Add(upd, "@IsFiscalCard", rdr.GetBoolean(3));
                Add(upd, "@IsInst", rdr.GetBoolean(4));
                Add(upd, "@IsAccount", rdr.GetBoolean(5));
                Add(upd, "@IsInvoice", rdr.GetBoolean(6));
                Add(upd, "@PrintText", rdr.IsDBNull(7) ? (object)DBNull.Value : rdr.GetString(7));
                Add(upd, "@Prefix", rdr.IsDBNull(8) ? (object)DBNull.Value : rdr.GetString(8));
                Add(upd, "@Payed", rdr.GetBoolean(9));
                if (await upd.ExecuteNonQueryAsync(ct) > 0) paymentTypesUpdated++;
            }
        }

        // ---- 2. PaymentCategories ----
        var categoriesInserted = await BulkImportAsync(conn, "PaymentCategories", ct,
            "SELECT Id, IdDDV, IdCalculationItem, CategoryName, ISNULL(AllowDiscount,0) AS AllowDiscount, " +
            "       ISNULL(TrigerdByRequest,0) AS TriggerdByRequest, ISNULL(TrigerdByTechnicalExam,0) AS TriggerdByTechnicalExam, " +
            "       ISNULL(TrigerdByTrafficLicence,0) AS TriggerdByTrafficLicence, " +
            "       ISNULL(TrigerdByPremisionForVehicle,0) AS TriggerdByPermissionForVehicle, " +
            "       ISNULL(TrigerdByInternationalDrivierLicence,0) AS TriggerdByInternationalDriverLicence, " +
            "       ISNULL(TrigerdByIrregularTechnicalExam,0) AS TriggerdByIrregularTechnicalExam, " +
            "       VisibleOrder, IdCommunity, ISNULL(Active,1) AS IsActive " +
            "FROM dbo.PaymentCategories",
            new[] {
                "Id","DDVId","CalculationItemId","CategoryName","AllowDiscount",
                "TriggerdByRequest","TriggerdByTechnicalExam","TriggerdByTrafficLicence",
                "TriggerdByPermissionForVehicle","TriggerdByInternationalDriverLicence","TriggerdByIrregularTechnicalExam",
                "VisibleOrder","CommunityId","IsActive"
            },
            errors, identityInsert: false);

        // ---- 3. PaymentItems ----
        var itemsInserted = await BulkImportAsync(conn, "PaymentItems", ct,
            "SELECT Id, IdPymentCategory AS PaymentCategoryId, IdVehicleCategoryForPayments AS VehicleCategoryForPaymentsId, " +
            "       ItemName, ISNULL(Active,1) AS IsActive " +
            "FROM dbo.PaymentItems",
            new[] { "Id","PaymentCategoryId","VehicleCategoryForPaymentsId","ItemName","IsActive" },
            errors, identityInsert: false);

        // ---- 4. PaymentItemParametars ----
        var paramsInserted = await BulkImportAsync(conn, "PaymentItemParametars", ct,
            "SELECT Id, IdPaymentItem AS PaymentItemId, PrametarName AS ParametarName, VehicleField, " +
            "       ParametarFrom, ParametarTo, ISNULL(Price,0) AS Price, ISNULL(IsOptional,0) AS IsOptional, " +
            "       ISNULL(Active,1) AS IsActive " +
            "FROM dbo.PaymentItemParametars",
            new[] { "Id","PaymentItemId","ParametarName","VehicleField","ParametarFrom","ParametarTo","Price","IsOptional","IsActive" },
            errors, identityInsert: false);

        // ---- 5. PaymentDocumentNumbers (per-station-per-type sequence) ----
        var numbersInserted = await BulkImportAsync(conn, "PaymentDocumentNumbers", ct,
            "SELECT IdStation AS StationId, IdTypeOfPayment AS PaymentTypeId, ISNULL(Number,0) AS Number, " +
            "       ISNULL(IsTehExamReport,0) AS IsTechExamReport " +
            "FROM dbo.PaymentDocumentsNumbers",
            new[] { "StationId","PaymentTypeId","Number","IsTechExamReport" },
            errors, identityInsert: false);

        return new PaymentCatalogMigrationResult(
            paymentTypesUpdated, categoriesInserted, itemsInserted, paramsInserted, numbersInserted, errors);
    }

    // Back-fills PaymentDocumentDetails.PaymentItemId by copying legacy
    // PaymentDocumentsDetails.IdPriceCatalog (misleadingly named — actually a
    // PaymentItems FK). Joins on Id (preserved during the original bulk import).
    // Chunked to stay within the SQL Server parameter / packet limits.
    public async Task<PaymentItemIdBackfillResult> BackfillPaymentItemIdAsync(CancellationToken ct = default)
    {
        var errors = new List<string>(); var updated = 0;
        var localIds = await _db.Set<PaymentDocumentDetail>()
            .Where(d => d.PaymentItemId == null)
            .Select(d => d.Id)
            .OrderBy(x => x)
            .ToListAsync(ct);
        if (localIds.Count == 0) return new PaymentItemIdBackfillResult(0, 0, Array.Empty<string>());

        var conn = _db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        const int chunkSize = 2000;
        for (var i = 0; i < localIds.Count; i += chunkSize)
        {
            var chunk = localIds.GetRange(i, Math.Min(chunkSize, localIds.Count - i));
            var inList = string.Join(",", chunk);

            // Pull legacy mappings for this chunk
            var legacyMap = new Dictionary<long, int>(chunkSize);
            try
            {
                await using var lcn = new SqlConnection(_legacyCs);
                await lcn.OpenAsync(ct);
                await using var sel = lcn.CreateCommand();
                sel.CommandText = $"SELECT Id, IdPriceCatalog FROM dbo.PaymentDocumentsDetails WHERE Id IN ({inList})";
                sel.CommandTimeout = 120;
                await using var rdr = await sel.ExecuteReaderAsync(ct);
                while (await rdr.ReadAsync(ct))
                {
                    if (rdr.IsDBNull(1)) continue;
                    legacyMap[rdr.GetInt64(0)] = rdr.GetInt32(1);
                }
            }
            catch (Exception ex) { errors.Add($"chunk@{i}: {FirstLine(ex.Message)}"); continue; }

            foreach (var (id, paymentItemId) in legacyMap)
            {
                try
                {
                    await using var upd = conn.CreateCommand();
                    upd.CommandText = "UPDATE dbo.PaymentDocumentDetails SET PaymentItemId=@PaymentItemId WHERE Id=@Id";
                    Add(upd, "@PaymentItemId", paymentItemId);
                    Add(upd, "@Id", id);
                    if (await upd.ExecuteNonQueryAsync(ct) > 0) updated++;
                }
                catch (Exception ex) { errors.Add($"id={id}: {FirstLine(ex.Message)}"); }
            }
        }

        return new PaymentItemIdBackfillResult(updated, localIds.Count, errors);
    }

    // Reusable: SELECT from legacy, INSERT into local. Truncates local table first so
    // re-runs replace the catalog cleanly. identityInsert toggles SET IDENTITY_INSERT.
    private async Task<int> BulkImportAsync(
        System.Data.Common.DbConnection conn, string tableName, CancellationToken ct,
        string legacySelect, string[] localColumns, List<string> errors, bool identityInsert = true)
    {
        try
        {
            // Wipe local table
            await using (var del = conn.CreateCommand())
            {
                del.CommandText = $"DELETE FROM dbo.{tableName};";
                await del.ExecuteNonQueryAsync(ct);
            }

            // Pull legacy rows
            var rows = new List<object?[]>();
            await using (var lcn = new SqlConnection(_legacyCs))
            {
                await lcn.OpenAsync(ct);
                await using var sel = lcn.CreateCommand();
                sel.CommandText = legacySelect;
                sel.CommandTimeout = 120;
                await using var rdr = await sel.ExecuteReaderAsync(ct);
                while (await rdr.ReadAsync(ct))
                {
                    var arr = new object?[rdr.FieldCount];
                    for (var i = 0; i < rdr.FieldCount; i++) arr[i] = rdr.IsDBNull(i) ? null : rdr.GetValue(i);
                    rows.Add(arr);
                }
            }
            if (rows.Count == 0) return 0;

            var cols = string.Join(",", localColumns);
            var prms = string.Join(",", localColumns.Select(c => "@" + c));
            var sql = $"INSERT INTO dbo.{tableName} ({cols}) VALUES ({prms});";

            if (identityInsert)
            {
                await using var on = conn.CreateCommand();
                on.CommandText = $"SET IDENTITY_INSERT dbo.{tableName} ON;";
                await on.ExecuteNonQueryAsync(ct);
            }
            try
            {
                foreach (var row in rows)
                {
                    await using var ins = conn.CreateCommand();
                    ins.CommandText = sql;
                    for (var i = 0; i < localColumns.Length; i++)
                    {
                        var p = ins.CreateParameter();
                        p.ParameterName = "@" + localColumns[i];
                        var v = row[i];
                        if (v is float f) v = SafeDec(f);
                        else if (v is double d) v = SafeDec(d);
                        p.Value = v ?? DBNull.Value;
                        ins.Parameters.Add(p);
                    }
                    await ins.ExecuteNonQueryAsync(ct);
                }
            }
            finally
            {
                if (identityInsert)
                {
                    await using var off = conn.CreateCommand();
                    off.CommandText = $"SET IDENTITY_INSERT dbo.{tableName} OFF;";
                    await off.ExecuteNonQueryAsync(ct);
                }
            }
            return rows.Count;
        }
        catch (Exception ex)
        {
            errors.Add($"{tableName}: {FirstLine(ex.Message)}");
            return 0;
        }
    }

    private static void Add(System.Data.Common.DbCommand cmd, string name, object? value)
    {
        var p = cmd.CreateParameter(); p.ParameterName = name; p.Value = value ?? DBNull.Value; cmd.Parameters.Add(p);
    }

    private sealed class LegacyVehicle
    {
        public long Id;
        public string? ShellNumber;
        public string? FirstRegistrationNumber;
        public string? LastRegistrationNumber;
        public string? VehicleModelAdding;
        public string? EngineNumber;
        public float? EnginePower;
        public float? EngineWorkingCapacity;
        public int? RPM;
        public float? H; public float? W; public float? L;
        public int? NumberOfDoors;
        public short? NumberOfSeats;
        public short? NumberOfStandingSeats;
        public short? LyingSeats;
        public int? Axes; public int? PropAxes; public int? NumberOfWheels; public int? NumberOfPropulsionWheels;
        public float? EmptyWeight; public float? MaxWeight;
        public DateTime? MakeDate;
        public DateTime? FirstRegistrationMakeDate;
        public DateTime? FirstRegistrationValidTill;
        public DateTime? LastRegistrationMakeDate;
        public DateTime? LastRegistrationValidTill;
        public string? ColorCode;
        public bool Suffocation; public bool Hook; public bool Winch; public bool TNG;
        public string? Note; public bool Active;
        public string? BodyTypeName; public string? CategoryName; public string? UseName; public string? ModelName;
        public string? MadeCountryName; public string? CategoryForPaymentsName;
        public string? EngineTypeName; public string? PowerSourceName; public string? SecondPowerSourceName; public string? EcoProgramName;
        public string? GearBoxName; public string? BrakesName; public string? SupportingName;
        public string? PrimaryColorName; public string? SecondaryColorName;
        public string? FirstIssuerName; public string? LastIssuerName;

        // Print-overlay
        public string? Tip; public string? BrojEUPotvrda; public string? OznakaNaOdobrenie;
        public string? OznakaNaOdobrenieZaPriklucUred; public string? IdentifikacijaNaMotorMestoMetod;
        public string? TBrOdobrenieMehanPriklucok; public string? TMarkaMehanPriklucok; public string? TTipMehanPriklucok;
        public string? TZastitnaKabina; public string? TZastitnaRamka; public string? NoiseTechnicalSpec; public string? OdnosKwCcm;
        public float? EnginePowerOutPut; public float? NoiseStatic; public float? CO2; public float? MaxSpeed;
        public float? MaxKonstVkMasa; public double? MaxLegVkMasa; public double? MaxLegVkMasaGrupa;
        public int? MasaPoOska1; public int? MasaPoOska2; public int? MasaPoOska3; public int? MasaPoOska4; public int? MasaPoOska5;
        public int? OsnoOptovaruvanje1; public int? OsnoOptovaruvanje2; public int? OsnoOptovaruvanje3; public int? OsnoOptovaruvanje4; public int? OsnoOptovaruvanje5;
        public int? MaxKonstOptovaruvanjeVoPriklucok; public int? TMaxHorVerOptovaruvanjePriklucok;
        public int? TMaxKonstVkMasaNaKombinacija; public int? TMaxKonstVkMasaPoluprikolka; public int? TMaxKonstVkMasaPrikolka;
        public int? TMaxKonstVkMasaPrikolkaSoCenOska; public int? TMaxKonstVkMasaPrikolkaStoMozePrikluci; public int? TMinMasa;

        public static LegacyVehicle Read(SqlDataReader r)
        {
            string? S(string c) { var i = r.GetOrdinal(c); return r.IsDBNull(i) ? null : r.GetString(i); }
            DateTime? D(string c) { var i = r.GetOrdinal(c); return r.IsDBNull(i) ? (DateTime?)null : r.GetDateTime(i); }
            bool B(string c) { var i = r.GetOrdinal(c); return !r.IsDBNull(i) && r.GetBoolean(i); }
            int? I(string c) { var i = r.GetOrdinal(c); return r.IsDBNull(i) ? (int?)null : r.GetInt32(i); }
            short? Sh(string c) { var i = r.GetOrdinal(c); return r.IsDBNull(i) ? (short?)null : r.GetInt16(i); }
            float? F(string c) { var i = r.GetOrdinal(c); if (r.IsDBNull(i)) return null;
                                 var fv = r.GetFieldType(i); return fv == typeof(double) ? (float?)r.GetDouble(i) : r.GetFloat(i); }
            double? Dbl(string c) { var i = r.GetOrdinal(c); return r.IsDBNull(i) ? (double?)null : r.GetDouble(i); }

            return new LegacyVehicle
            {
                Id                          = r.GetInt64(r.GetOrdinal("Id")),
                ShellNumber                 = S("ShellNumber"),
                FirstRegistrationNumber     = S("FirstRegistrationNumber"),
                LastRegistrationNumber      = S("LastRegistrationNumber"),
                VehicleModelAdding          = S("VehicleModelAdding"),
                EngineNumber                = S("EngineNumber"),
                EnginePower                 = F("EnginePower"),
                EngineWorkingCapacity       = F("EngineWorkingCapacity"),
                RPM                         = I("RPM"),
                H                           = F("H"),
                W                           = F("W"),
                L                           = F("L"),
                NumberOfDoors               = I("NumberOfDoors"),
                NumberOfSeats               = Sh("NumberOfSeats"),
                NumberOfStandingSeats       = Sh("NumberOfStandingSeats"),
                LyingSeats                  = Sh("LyingSeats"),
                Axes                        = I("Axes"),
                PropAxes                    = I("PropAxes"),
                NumberOfWheels              = I("NumberOfWheels"),
                NumberOfPropulsionWheels    = I("NumberOfPropulsionWheels"),
                EmptyWeight                 = F("EmptyWeight"),
                MaxWeight                   = F("MaxWeight"),
                MakeDate                    = D("MakeDate"),
                FirstRegistrationMakeDate   = D("FirstRegistrationMakeDate"),
                FirstRegistrationValidTill  = D("FirstRegistrationValidTill"),
                LastRegistrationMakeDate    = D("LastRegistrationMakeDate"),
                LastRegistrationValidTill   = D("LastRegistrationValidTill"),
                ColorCode                   = S("ColorCode"),
                Suffocation                 = B("Suffocation"),
                Hook                        = B("Hook"),
                Winch                       = B("Winch"),
                TNG                         = B("TNG"),
                Note                        = S("Note"),
                Active                      = B("Active"),
                BodyTypeName                = S("BodyTypeName"),
                CategoryName                = S("CategoryName"),
                UseName                     = S("UseName"),
                ModelName                   = S("ModelName"),
                MadeCountryName             = S("MadeCountryName"),
                CategoryForPaymentsName     = S("CategoryForPaymentsName"),
                EngineTypeName              = S("EngineTypeName"),
                PowerSourceName             = S("PowerSourceName"),
                SecondPowerSourceName       = S("SecondPowerSourceName"),
                EcoProgramName              = S("EcoProgramName"),
                GearBoxName                 = S("GearBoxName"),
                BrakesName                  = S("BrakesName"),
                SupportingName              = S("SupportingName"),
                PrimaryColorName            = S("PrimaryColorName"),
                SecondaryColorName          = S("SecondaryColorName"),
                FirstIssuerName             = S("FirstIssuerName"),
                LastIssuerName              = S("LastIssuerName"),

                // Print-overlay columns
                Tip                                              = S("Tip"),
                BrojEUPotvrda                                    = S("BrojEUPotvrda"),
                OznakaNaOdobrenie                                = S("OznakaNaOdobrenie"),
                OznakaNaOdobrenieZaPriklucUred                   = S("OznakaNaOdobrenieZaPriklucUred"),
                IdentifikacijaNaMotorMestoMetod                  = S("IdentifikacijaNaMotorMestoMetod"),
                TBrOdobrenieMehanPriklucok                       = S("TBrOdobrenieMehanPriklucok"),
                TMarkaMehanPriklucok                             = S("TMarkaMehanPriklucok"),
                TTipMehanPriklucok                               = S("TTipMehanPriklucok"),
                TZastitnaKabina                                  = S("TZastitnaKabina"),
                TZastitnaRamka                                   = S("TZastitnaRamka"),
                NoiseTechnicalSpec                               = S("NoiseTechnicalSpec"),
                OdnosKwCcm                                       = S("OdnosKwCcm"),
                EnginePowerOutPut                                = F("EnginePowerOutPut"),
                NoiseStatic                                      = F("NoiseStatic"),
                CO2                                              = F("CO2"),
                MaxSpeed                                         = F("MaxSpeed"),
                MaxKonstVkMasa                                   = F("MaxKonstVkMasa"),
                MaxLegVkMasa                                     = Dbl("MaxLegVkMasa"),
                MaxLegVkMasaGrupa                                = Dbl("MaxLegVkMasaGrupa"),
                MasaPoOska1                                      = I("MasaPoOska1"),
                MasaPoOska2                                      = I("MasaPoOska2"),
                MasaPoOska3                                      = I("MasaPoOska3"),
                MasaPoOska4                                      = I("MasaPoOska4"),
                MasaPoOska5                                      = I("MasaPoOska5"),
                OsnoOptovaruvanje1                               = I("OsnoOptovaruvanje1"),
                OsnoOptovaruvanje2                               = I("OsnoOptovaruvanje2"),
                OsnoOptovaruvanje3                               = I("OsnoOptovaruvanje3"),
                OsnoOptovaruvanje4                               = I("OsnoOptovaruvanje4"),
                OsnoOptovaruvanje5                               = I("OsnoOptovaruvanje5"),
                MaxKonstOptovaruvanjeVoPriklucok                 = I("MaxKonstOptovaruvanjeVoPriklucok"),
                TMaxHorVerOptovaruvanjePriklucok                 = I("TMaxHorVerOptovaruvanjePriklucok"),
                TMaxKonstVkMasaNaKombinacija                     = I("TMaxKonstVkMasaNaKombinacija"),
                TMaxKonstVkMasaPoluprikolka                      = I("TMaxKonstVkMasaPoluprikolka"),
                TMaxKonstVkMasaPrikolka                          = I("TMaxKonstVkMasaPrikolka"),
                TMaxKonstVkMasaPrikolkaSoCenOska                 = I("TMaxKonstVkMasaPrikolkaSoCenOska"),
                TMaxKonstVkMasaPrikolkaStoMozePrikluci           = I("TMaxKonstVkMasaPrikolkaStoMozePrikluci"),
                TMinMasa                                         = I("TMinMasa"),
            };
        }
    }
}
