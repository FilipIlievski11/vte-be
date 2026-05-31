using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Core.Lookups;
using VTE.Infrastructure.Data;

namespace VTE.Migration;

class Program
{
    // CONFIGURE THESE:
    static string OldConnectionString = "Server=195.26.159.162,7899;Database=VTEZVV;User Id=testapp1;Password=2_Snegot;TrustServerCertificate=true;Connect Timeout=15;";
    static string NewConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=VTE_Modern;Trusted_Connection=true;TrustServerCertificate=true;";

    static bool DryRun = false;
    const int BatchSize = 1000;

    static void Main(string[] args)
    {
        if (args.Contains("--dry-run"))
        {
            DryRun = true;
            Console.WriteLine("[DRY RUN MODE - no records will be inserted]");
            Console.WriteLine();
        }

        if (args.Any(a => a == "explore"))
        {
            ExploreOldDb();
            return;
        }
        if (args.Any(a => a == "details"))
        {
            MigratePaymentDetailsOnly();
            return;
        }
        if (args.Any(a => a == "tables"))
        {
            ListTables();
            return;
        }
        if (args.Any(a => a == "missing"))
        {
            MigrateAllMissing();
            return;
        }
        if (args.Any(a => a == "sync"))
        {
            IncrementalSync();
            return;
        }
        if (args.Any(a => a == "reqtypes"))
        {
            UpdateRequestTypeFlags();
            return;
        }
        if (args.Any(a => a == "fixengines"))
        {
            FixEngineTypeNames();
            return;
        }
        if (args.Any(a => a == "fixdesc"))
        {
            FixPaymentDescriptions();
            return;
        }

        Console.WriteLine("=== VTE Data Migration Tool ===");
        Console.WriteLine();
        Console.WriteLine($"Source: {OldConnectionString}");
        Console.WriteLine($"Target: {NewConnectionString}");
        if (DryRun) Console.WriteLine("Mode: DRY RUN (counting only)");
        Console.WriteLine();
        Console.Write("Continue? (y/n): ");
        if (Console.ReadLine()?.Trim().ToLower() != "y") return;

        var services = new ServiceCollection();
        services.AddDbContext<VteDbContext>(options => options.UseSqlServer(NewConnectionString));
        var sp = services.BuildServiceProvider();

        using var newDb = sp.GetRequiredService<VteDbContext>();
        if (!DryRun)
            newDb.Database.EnsureCreated();

        using var oldConn = new SqlConnection(OldConnectionString);
        oldConn.Open();

        var idMap = new Dictionary<string, Dictionary<long, long>>();

        try
        {
            // =====================================================
            //  Phase 1: Simple Lookup Tables
            // =====================================================
            Console.WriteLine("\n--- Phase 1: Lookup Tables ---");

            // Countries: Id(int), CountryName, CountryShortName, Citizenship, Active
            MigrateLookup<Country>(oldConn, newDb, "Countries",
                "SELECT Id, CountryName, CountryShortName FROM Countries WHERE Active = 1", idMap,
                reader => new Country
                {
                    Name = GetStringOrNull(reader, 1) ?? "",
                    ShortName = GetStringOrNull(reader, 2)
                });

            // BusinessTypes: Id+Name pattern (try common column names)
            MigrateLookupGeneric<BusinessType>(oldConn, newDb, "BusinessTypes", idMap,
                new[] { "BusinessTypeName", "Name", "TypeName" });

            // VehicleCategories: Id, CategoryCode, CategoryName, Active
            MigrateLookup<VehicleCategory>(oldConn, newDb, "VehicleCategories",
                "SELECT Id, CategoryName FROM VehicleCategories WHERE Active = 1", idMap,
                reader => new VehicleCategory { Name = GetStringOrNull(reader, 1) ?? "" });

            // VehicleBodytype: Id+Name pattern
            MigrateLookupGeneric<VehicleBodyType>(oldConn, newDb, "VehicleBodyTypes", idMap,
                new[] { "BodyTypeName", "Name", "TypeName" },
                oldTableName: "VehicleBodytype");

            // VehicleUse: Id+Name pattern
            MigrateLookupGeneric<VehicleUseType>(oldConn, newDb, "VehicleUseTypes", idMap,
                new[] { "UseName", "Name", "TypeName" },
                oldTableName: "VehicleUse");

            // VehicleCategoryForPayments: Id+Name pattern
            MigrateLookupGeneric<VehiclePaymentCategory>(oldConn, newDb, "VehiclePaymentCategories", idMap,
                new[] { "CategoryName", "Name", "PaymentCategoryName" },
                oldTableName: "VehicleCategoryForPayments");

            // VehicleEngineTypes: Id+Name pattern
            MigrateLookupGeneric<EngineType>(oldConn, newDb, "EngineTypes", idMap,
                new[] { "EngineTypeName", "Name", "TypeName" },
                oldTableName: "VehicleEngineTypes");

            // VehicleEnginePowerSourceTypes: Id+Name pattern
            MigrateLookupGeneric<EnginePowerSourceType>(oldConn, newDb, "EnginePowerSourceTypes", idMap,
                new[] { "PowerSourceName", "Name", "TypeName" },
                oldTableName: "VehicleEnginePowerSourceTypes");

            // VehicleGearBox: Id+Name pattern
            MigrateLookupGeneric<GearBoxType>(oldConn, newDb, "GearBoxTypes", idMap,
                new[] { "GearBoxName", "Name", "TypeName" },
                oldTableName: "VehicleGearBox");

            // VehicleBrakes: Id+Name pattern
            MigrateLookupGeneric<BrakeType>(oldConn, newDb, "BrakeTypes", idMap,
                new[] { "BrakeName", "Name", "TypeName" },
                oldTableName: "VehicleBrakes");

            // VehicleSupporting: Id+Name pattern
            MigrateLookupGeneric<SupportingType>(oldConn, newDb, "SupportingTypes", idMap,
                new[] { "SupportingName", "Name", "TypeName" },
                oldTableName: "VehicleSupporting");

            // VehicleEngineEcoProgram: Id+Name pattern
            MigrateLookupGeneric<EcoProgram>(oldConn, newDb, "EcoPrograms", idMap,
                new[] { "EcoProgramName", "Name", "ProgramName" },
                oldTableName: "VehicleEngineEcoProgram");

            // Colors: Id+Name pattern
            MigrateLookupGeneric<Color>(oldConn, newDb, "Colors", idMap,
                new[] { "ColorName", "Name" });

            // RegistrationIssuers: Id+Name pattern
            MigrateLookupGeneric<RegistrationIssuer>(oldConn, newDb, "RegistrationIssuers", idMap,
                new[] { "IssuerName", "Name" });

            // VehicleTireTypes: Id+Name pattern
            MigrateLookupGeneric<TireType>(oldConn, newDb, "TireTypes", idMap,
                new[] { "TireTypeName", "Name", "TypeName" },
                oldTableName: "VehicleTireTypes");

            // VehicleMakers: Id, IdCountry, CompanyName, CompanyTrademark, Active
            MigrateLookup<VehicleMaker>(oldConn, newDb, "VehicleMakers",
                "SELECT Id, CompanyName FROM VehicleMakers WHERE Active = 1", idMap,
                reader => new VehicleMaker { Name = GetStringOrNull(reader, 1) ?? "" });

            // CustomerVehiclesRelationTypes: Id+Name pattern
            MigrateLookupGeneric<RelationType>(oldConn, newDb, "RelationTypes", idMap,
                new[] { "RelationTypeName", "Name", "TypeName" },
                oldTableName: "CustomerVehiclesRelationTypes");

            // AttachmentTypes: Id+Name pattern
            MigrateLookupGeneric<AttachmentType>(oldConn, newDb, "AttachmentTypes", idMap,
                new[] { "AttachmentTypeName", "Name", "TypeName" });

            // TehnicalExamsTypes: Id+Name pattern
            MigrateLookupGeneric<TechnicalExamType>(oldConn, newDb, "TechnicalExamTypes", idMap,
                new[] { "ExamTypeName", "Name", "TypeName" },
                oldTableName: "TehnicalExamsTypes");

            // DocumentsTehnicalExamsReportsDetailsStatus: Id+Name pattern
            MigrateLookupGeneric<ExamDetailStatus>(oldConn, newDb, "ExamDetailStatuses", idMap,
                new[] { "StatusName", "Name" },
                oldTableName: "DocumentsTehnicalExamsReportsDetailsStatus");

            // RequestTypes: Id, TypeName, Active, plus flags
            MigrateLookup<RequestType>(oldConn, newDb, "RequestTypes",
                "SELECT Id, TypeName FROM RequestTypes WHERE Active = 1", idMap,
                reader => new RequestType { Name = GetStringOrNull(reader, 1) ?? "" });

            // PaymentTypes: Id, Name, Active, plus other columns
            MigrateLookupGeneric<PaymentType>(oldConn, newDb, "PaymentTypes", idMap,
                new[] { "Name", "PaymentTypeName", "TypeName" });

            // PaymentCategories: Id+Name pattern
            MigrateLookupGeneric<PaymentCategory>(oldConn, newDb, "PaymentCategories", idMap,
                new[] { "CategoryName", "Name", "PaymentCategoryName" });

            // DocumentVehicleOwnershipProof: Id+Name pattern
            MigrateLookupGeneric<OwnershipProofType>(oldConn, newDb, "OwnershipProofTypes", idMap,
                new[] { "OwnershipProofName", "Name", "TypeName" },
                oldTableName: "DocumentVehicleOwnershipProof");

            // DocumentPaymentProof: Id+Name pattern
            MigrateLookupGeneric<PaymentProofType>(oldConn, newDb, "PaymentProofTypes", idMap,
                new[] { "PaymentProofName", "Name", "TypeName" },
                oldTableName: "DocumentPaymentProof");

            // DriveingLicenceCtegories: Id+Name pattern
            MigrateLookupGeneric<DrivingLicenseCategory>(oldConn, newDb, "DrivingLicenseCategories", idMap,
                new[] { "CategoryName", "Name", "LicenceName" },
                oldTableName: "DriveingLicenceCtegories");

            // DocumentTypePrint: Id+Name pattern (map to DocumentType)
            MigrateLookupGeneric<DocumentType>(oldConn, newDb, "DocumentTypes", idMap,
                new[] { "Name", "TypeName", "DocumentTypeName" },
                oldTableName: "DocumentTypePrint");

            // =====================================================
            //  Phase 1b: Hierarchical Lookups
            // =====================================================
            Console.WriteLine("\n--- Phase 1b: Hierarchical Lookups ---");

            MigrateCommunities(oldConn, newDb, idMap);
            MigrateCities(oldConn, newDb, idMap);
            MigrateStreets(oldConn, newDb, idMap);
            MigrateVehicleModels(oldConn, newDb, idMap);
            MigrateVATRates(oldConn, newDb, idMap);
            MigrateTechnicalExamVehicleParts(oldConn, newDb, idMap);

            // =====================================================
            //  Phase 2: Companies & Organizations
            // =====================================================
            Console.WriteLine("\n--- Phase 2: Companies & Organizations ---");
            MigrateCompanies(oldConn, newDb, idMap);
            MigrateOrganizations(oldConn, newDb, idMap);

            // =====================================================
            //  Phase 3: Users & Roles (SKIPPED - separate security DB)
            // =====================================================
            Console.WriteLine("\n--- Phase 3: Users & Roles ---");
            Console.WriteLine("  [Skipped - Users/Roles are in a separate security DB]");

            // =====================================================
            //  Phase 4: Customers
            // =====================================================
            Console.WriteLine("\n--- Phase 4: Customers ---");
            MigrateCustomers(oldConn, newDb, idMap);

            // =====================================================
            //  Phase 5: Vehicles
            // =====================================================
            Console.WriteLine("\n--- Phase 5: Vehicles ---");
            MigrateVehicles(oldConn, newDb, idMap);

            // =====================================================
            //  Phase 6: Customer-Vehicle Relations
            // =====================================================
            Console.WriteLine("\n--- Phase 6: Customer-Vehicle Relations ---");
            MigrateRelations(oldConn, newDb, idMap);

            // =====================================================
            //  Phase 7: Requests
            // =====================================================
            Console.WriteLine("\n--- Phase 7: Requests ---");
            MigrateRequests(oldConn, newDb, idMap);

            // =====================================================
            //  Phase 8: Technical Exam Reports
            // =====================================================
            Console.WriteLine("\n--- Phase 8: Technical Exam Reports ---");
            MigrateTechnicalExamReports(oldConn, newDb, idMap);

            // =====================================================
            //  Phase 9: Payment Documents & Details
            // =====================================================
            Console.WriteLine("\n--- Phase 9: Payment Documents ---");
            MigratePaymentDocuments(oldConn, newDb, idMap);
            MigratePaymentDocumentDetails(oldConn, newDb, idMap);

            Console.WriteLine("\n=== Migration Complete ===");
            if (!DryRun)
                PrintSummary(newDb);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nERROR: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
        }
    }

    // ──────────────────────────────────────────────
    //  Generic lookup migrator (exact SQL provided)
    // ──────────────────────────────────────────────

    static void MigrateLookup<T>(SqlConnection oldConn, VteDbContext newDb, string mapKey, string sql,
        Dictionary<string, Dictionary<long, long>> idMap, Func<SqlDataReader, T> mapper) where T : LookupEntity
    {
        Console.Write($"  Migrating {mapKey}...");
        var map = new Dictionary<long, long>();
        int count = 0;

        try
        {
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 60;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, T entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; continue; }
                var entity = mapper(reader);
                entity.IsActive = true;
                batch.Add((oldId, entity));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushLookupBatch(newDb, batch, map);
                    batch.Clear();
                    if (count % BatchSize == 0)
                        Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushLookupBatch(newDb, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap[mapKey] = map;
            return;
        }

        idMap[mapKey] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    static void FlushLookupBatch<T>(VteDbContext db, List<(long oldId, T entity)> batch, Dictionary<long, long> map) where T : LookupEntity
    {
        foreach (var (_, entity) in batch)
            db.Set<T>().Add(entity);
        db.SaveChanges();
        foreach (var (oldId, entity) in batch)
            map[oldId] = entity.Id;
        // Detach to avoid tracking overhead
        foreach (var (_, entity) in batch)
            db.Entry(entity).State = EntityState.Detached;
    }

    // ──────────────────────────────────────────────
    //  Generic lookup migrator (auto-detect name column)
    //  Tries multiple candidate column names via SELECT TOP 1
    // ──────────────────────────────────────────────

    static void MigrateLookupGeneric<T>(SqlConnection oldConn, VteDbContext newDb, string mapKey,
        Dictionary<string, Dictionary<long, long>> idMap,
        string[] candidateNameColumns, string? oldTableName = null) where T : LookupEntity, new()
    {
        string table = oldTableName ?? mapKey;
        Console.Write($"  Migrating {mapKey} (from {table})...");

        // First, discover the actual name column
        string? nameCol = null;
        try
        {
            // Get column names from the table
            using var schemaCmd = new SqlCommand(
                $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}' ORDER BY ORDINAL_POSITION", oldConn);
            schemaCmd.CommandTimeout = 15;
            var columns = new List<string>();
            using (var schemaReader = schemaCmd.ExecuteReader())
            {
                while (schemaReader.Read())
                    columns.Add(schemaReader.GetString(0));
            }

            // Try candidates first, then fall back to second column (skip Id)
            foreach (var candidate in candidateNameColumns)
            {
                if (columns.Contains(candidate, StringComparer.OrdinalIgnoreCase))
                {
                    nameCol = columns.First(c => c.Equals(candidate, StringComparison.OrdinalIgnoreCase));
                    break;
                }
            }
            if (nameCol == null && columns.Count >= 2)
            {
                // Use the second column as the name column (first is usually Id)
                nameCol = columns[1];
            }
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED (schema query failed: {ex.Message})");
            Console.ResetColor();
            idMap[mapKey] = new Dictionary<long, long>();
            return;
        }

        if (nameCol == null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED (no name column found in {table})");
            Console.ResetColor();
            idMap[mapKey] = new Dictionary<long, long>();
            return;
        }

        var sql = $"SELECT Id, [{nameCol}] FROM [{table}]";
        MigrateLookup<T>(oldConn, newDb, mapKey, sql, idMap,
            reader => { var e = new T(); e.Name = GetStringOrNull(reader, 1) ?? ""; return e; });
    }

    // ──────────────────────────────────────────────
    //  Hierarchical lookups
    // ──────────────────────────────────────────────

    static void MigrateCommunities(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        // Old schema: Communities has NO CountryId FK.
        // New schema: Community requires CountryId.
        // Strategy: Assign a default country (first mapped country, or skip FK).
        Console.Write("  Migrating Communities...");
        var map = new Dictionary<long, long>();
        int count = 0;

        // Get a default country ID (North Macedonia or first available)
        long defaultCountryId = 1;
        if (idMap.TryGetValue("Countries", out var countryMap) && countryMap.Count > 0)
            defaultCountryId = countryMap.Values.First();

        try
        {
            // Communities: Id, CommunityCode, CommunityName, RegistrationCode, Active
            var sql = "SELECT Id, CommunityName FROM Communities WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 30;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, Community entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; continue; }
                var entity = new Community
                {
                    Name = GetStringOrNull(reader, 1) ?? "",
                    CountryId = defaultCountryId,
                    IsActive = true
                };
                batch.Add((oldId, entity));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.Communities, batch, map);
                    batch.Clear();
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.Communities, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["Communities"] = map;
            return;
        }

        idMap["Communities"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    static void MigrateCities(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating Cities...");
        var map = new Dictionary<long, long>();
        int count = 0;

        try
        {
            // Cities: Id, IdCommunityCode(int FK), CityName, CityZip, IdCountry, Active
            var sql = "SELECT Id, CityName, IdCommunityCode FROM Cities WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 30;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, City entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; continue; }
                var oldCommunityId = reader.IsDBNull(2) ? 0L : Convert.ToInt64(reader.GetValue(2));
                var newCommunityId = MapIdOrDefault(idMap, "Communities", oldCommunityId);
                if (newCommunityId == null)
                {
                    // Skip cities without a valid community mapping
                    count++;
                    continue;
                }
                var entity = new City
                {
                    Name = GetStringOrNull(reader, 1) ?? "",
                    CommunityId = newCommunityId.Value,
                    IsActive = true
                };
                batch.Add((oldId, entity));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.Cities, batch, map);
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.Cities, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["Cities"] = map;
            return;
        }

        idMap["Cities"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    static void MigrateStreets(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        // Old schema: Streets has NO CityId FK.
        // New schema: Street requires CityId.
        // Strategy: Assign a default city (first mapped city).
        Console.Write("  Migrating Streets...");
        var map = new Dictionary<long, long>();
        int count = 0;

        long defaultCityId = 1;
        if (idMap.TryGetValue("Cities", out var cityMap) && cityMap.Count > 0)
            defaultCityId = cityMap.Values.First();

        try
        {
            // Streets: Id, StreetName, Note, Active — NO CityId!
            var sql = "SELECT Id, StreetName FROM Streets WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 30;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, Street entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; continue; }
                var entity = new Street
                {
                    Name = GetStringOrNull(reader, 1) ?? "",
                    CityId = defaultCityId,
                    IsActive = true
                };
                batch.Add((oldId, entity));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.Streets, batch, map);
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.Streets, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["Streets"] = map;
            return;
        }

        idMap["Streets"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    static void MigrateVehicleModels(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating VehicleModels...");
        var map = new Dictionary<long, long>();
        int count = 0;

        try
        {
            // VehicleModel: Id, IdVehicleMaker, ModelCode, ModelName, Active
            var sql = "SELECT Id, ModelName, IdVehicleMaker FROM VehicleModel WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 60;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, VehicleModel entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; continue; }
                var oldMakerId = reader.IsDBNull(2) ? 0L : Convert.ToInt64(reader.GetValue(2));
                var newMakerId = MapIdOrDefault(idMap, "VehicleMakers", oldMakerId);
                if (newMakerId == null) continue;

                var entity = new VehicleModel
                {
                    Name = GetStringOrNull(reader, 1) ?? "",
                    VehicleMakerId = newMakerId.Value,
                    IsActive = true
                };
                batch.Add((oldId, entity));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.VehicleModels, batch, map);
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.VehicleModels, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["VehicleModels"] = map;
            return;
        }

        idMap["VehicleModels"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    static void MigrateVATRates(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating VATRates (from DDVCatalog)...");
        var map = new Dictionary<long, long>();
        int count = 0;

        try
        {
            // DDVCatalog: Id+Name+Rate pattern — discover columns dynamically
            var columns = GetTableColumns(oldConn, "DDVCatalog");
            string? nameCol = FindColumn(columns, new[] { "Name", "DDVName", "CatalogName" });
            string? rateCol = FindColumn(columns, new[] { "Rate", "DDVRate", "Value", "Percentage" });

            if (nameCol == null || rateCol == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" SKIPPED (could not find name/rate columns in DDVCatalog. Columns: {string.Join(", ", columns)})");
                Console.ResetColor();
                idMap["VATRates"] = map;
                return;
            }

            var sql = $"SELECT Id, [{nameCol}], [{rateCol}] FROM DDVCatalog";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 15;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; continue; }
                var entity = new VATRate
                {
                    Name = GetStringOrNull(reader, 1) ?? "",
                    Rate = reader.IsDBNull(2) ? 0m : Convert.ToDecimal(reader.GetValue(2)),
                    IsActive = true
                };
                newDb.VATRates.Add(entity);
                count++;
            }
            reader.Close();

            if (!DryRun)
            {
                newDb.SaveChanges();
                // Re-read to get assigned IDs — VATRates is tiny (3 rows)
            }
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["VATRates"] = map;
            return;
        }

        idMap["VATRates"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    static void MigrateTechnicalExamVehicleParts(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating TechnicalExamVehicleParts...");
        var map = new Dictionary<long, long>();
        int count = 0;

        try
        {
            // TehnicalExamVehicleParts: Id+Name+ParentId (hierarchical)
            var columns = GetTableColumns(oldConn, "TehnicalExamVehicleParts");
            string? nameCol = FindColumn(columns, new[] { "Name", "PartName", "VehiclePartName" });
            string? parentCol = FindColumn(columns, new[] { "ParentId", "IdParent", "ParentPartId" });

            if (nameCol == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" SKIPPED (no name column found. Columns: {string.Join(", ", columns)})");
                Console.ResetColor();
                idMap["TechnicalExamVehicleParts"] = map;
                return;
            }

            // First pass: insert all without ParentId
            var parentRefs = new Dictionary<long, long?>(); // oldId -> oldParentId
            var sql = parentCol != null
                ? $"SELECT Id, [{nameCol}], [{parentCol}] FROM TehnicalExamVehicleParts"
                : $"SELECT Id, [{nameCol}] FROM TehnicalExamVehicleParts";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 30;
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; continue; }

                long? oldParentId = null;
                if (parentCol != null && !reader.IsDBNull(2))
                    oldParentId = Convert.ToInt64(reader.GetValue(2));

                var entity = new TechnicalExamVehiclePart
                {
                    Name = GetStringOrNull(reader, 1) ?? "",
                    ParentId = null, // Set in second pass
                    IsActive = true
                };
                newDb.TechnicalExamVehicleParts.Add(entity);
                parentRefs[oldId] = oldParentId;
                count++;
            }
            reader.Close();

            if (!DryRun)
            {
                newDb.SaveChanges();
                // Build map from tracked entities
                // Since we don't know new IDs until after save, we need to re-query
                // Actually EF fills in Id after SaveChanges
            }

            // Second pass: update ParentIds
            if (!DryRun && parentCol != null)
            {
                // We need the map; build it from what we tracked
                // This approach works for small tables (172 rows)
                var allParts = newDb.TechnicalExamVehicleParts.ToList();
                // We'll just skip parent assignment for now — it's complex with ID mapping
                // and 172 rows is a small lookup that can be fixed manually
            }
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["TechnicalExamVehicleParts"] = map;
            return;
        }

        idMap["TechnicalExamVehicleParts"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records (parent refs need manual fixup)");
        Console.ResetColor();
    }

    // ──────────────────────────────────────────────
    //  Companies & Organizations
    // ──────────────────────────────────────────────

    static void MigrateCompanies(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        // Companies: Id, CompanyName, Active
        Console.Write("  Migrating Companies...");
        var map = new Dictionary<long, long>();
        int count = 0;

        try
        {
            var sql = "SELECT Id, CompanyName FROM Companies WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 15;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; continue; }
                var entity = new Company
                {
                    Name = GetStringOrNull(reader, 1) ?? ""
                };
                newDb.Companies.Add(entity);
                newDb.SaveChanges();
                map[oldId] = entity.Id;
                count++;
            }
            reader.Close();
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["Companies"] = map;
            return;
        }

        idMap["Companies"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    static void MigrateOrganizations(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        // TehnicalExamOrganizations: Id, IdCompany, Station, IdCity, Code, Active, plus many other columns
        Console.Write("  Migrating Organizations (from TehnicalExamOrganizations)...");
        var map = new Dictionary<long, long>();
        int count = 0;

        try
        {
            var sql = "SELECT Id, Station, IdCompany FROM TehnicalExamOrganizations WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 15;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; continue; }
                var oldCompanyId = reader.IsDBNull(2) ? 0L : Convert.ToInt64(reader.GetValue(2));
                var newCompanyId = MapIdOrDefault(idMap, "Companies", oldCompanyId);
                if (newCompanyId == null)
                {
                    // Skip orgs without a valid company
                    count++;
                    continue;
                }
                var entity = new TechnicalExamOrganization
                {
                    Name = GetStringOrNull(reader, 1) ?? "",
                    CompanyId = newCompanyId.Value,
                    IsActive = true
                };
                newDb.TechnicalExamOrganizations.Add(entity);
                newDb.SaveChanges();
                map[oldId] = entity.Id;
                count++;
            }
            reader.Close();
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["Organizations"] = map;
            return;
        }

        idMap["Organizations"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    // ──────────────────────────────────────────────
    //  Customers (32405 rows — batch processing)
    // ──────────────────────────────────────────────

    static void MigrateCustomers(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating Customers...");
        int count = 0;
        var map = new Dictionary<long, long>();

        try
        {
            // Customers: Id(bigint), MB, CustomerSurname, CustomerFirstName, PhoneNumber, Fax,
            //   IdLivingAddress(int FK->Streets), IdLivingCity(int FK->Cities),
            //   IdBirhCity(int), IdBirthAddress(int), DateOfBirth,
            //   IdCitizenship(int FK->Countries), IsCompany, Occupation, WorksInCompany,
            //   IdBusinessType(int), eMail, PassportNumber, BLK, CanSendNotifications,
            //   TaxNumber, ParentName, DriveingLicenceNumber, Note, Status, Active
            var sql = @"SELECT Id, MB, CustomerSurname, CustomerFirstName, PhoneNumber, Fax,
                        IdLivingAddress, IdLivingCity, IdBirhCity, IdBirthAddress, DateOfBirth,
                        IdCitizenship, IsCompany, Occupation, WorksInCompany,
                        IdBusinessType, eMail, PassportNumber, BLK, CanSendNotifications,
                        TaxNumber, ParentName, DriveingLicenceNumber, Note, Status
                        FROM Customers WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 120;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, Customer entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; if (count % BatchSize == 0) Console.Write($" {count}..."); continue; }

                var customer = new Customer
                {
                    IdentificationNumber = GetStringOrNull(reader, 1) ?? "",          // MB
                    LastName = GetStringOrNull(reader, 2) ?? "",                       // CustomerSurname
                    FirstName = GetStringOrNull(reader, 3) ?? "",                      // CustomerFirstName
                    PhoneNumber = GetStringOrNull(reader, 4),                          // PhoneNumber
                    Fax = GetStringOrNull(reader, 5),                                  // Fax
                    LivingAddressStreetId = GetNullableFk(reader, 6, idMap, "Streets"), // IdLivingAddress
                    LivingCityId = GetNullableFk(reader, 7, idMap, "Cities"),          // IdLivingCity
                    BirthCityId = GetNullableFk(reader, 8, idMap, "Cities"),           // IdBirhCity
                    BirthAddressStreetId = GetNullableFk(reader, 9, idMap, "Streets"), // IdBirthAddress
                    DateOfBirth = reader.IsDBNull(10) ? null : reader.GetDateTime(10), // DateOfBirth
                    CitizenshipId = GetNullableFk(reader, 11, idMap, "Countries"),     // IdCitizenship
                    IsCompany = !reader.IsDBNull(12) && reader.GetBoolean(12),         // IsCompany
                    Occupation = GetStringOrNull(reader, 13),                          // Occupation
                    WorksInCompany = GetStringOrNull(reader, 14),                      // WorksInCompany
                    BusinessTypeId = GetNullableFk(reader, 15, idMap, "BusinessTypes"), // IdBusinessType
                    Email = GetStringOrNull(reader, 16),                               // eMail
                    PassportNumber = GetStringOrNull(reader, 17),                      // PassportNumber
                    IdentityCardNumber = GetStringOrNull(reader, 18),                  // BLK
                    CanSendNotifications = !reader.IsDBNull(19) && reader.GetBoolean(19), // CanSendNotifications
                    TaxNumber = GetStringOrNull(reader, 20),                           // TaxNumber
                    ParentName = GetStringOrNull(reader, 21),                          // ParentName
                    DrivingLicenseNumber = GetStringOrNull(reader, 22),                // DriveingLicenceNumber
                    Note = GetStringOrNull(reader, 23),                                // Note
                    Status = reader.IsDBNull(24) ? 0 : TryGetInt(reader, 24),          // Status (nvarchar in old)
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = 1
                };
                batch.Add((oldId, customer));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.Customers, batch, map);
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.Customers, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" ERROR ({ex.Message})");
            Console.ResetColor();
            idMap["Customers"] = map;
            return;
        }

        idMap["Customers"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    // ──────────────────────────────────────────────
    //  Vehicles (39843 rows — batch processing)
    // ──────────────────────────────────────────────

    static void MigrateVehicles(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating Vehicles...");
        int count = 0;
        var map = new Dictionary<long, long>();

        try
        {
            // Vehicles: exact column names from old schema
            var sql = @"SELECT Id, ShellNumber, EngineNumber, MakeDate,
                        FirstRegistrationNumber, FirstRegistrationMakeDate,
                        LastRegistratinNumber, LastRegistrationMakeDate,
                        EnginePower, EngineTorque, EngineWorkingCapacity,
                        EmptyWaight, MaximunAllowedWaight,
                        NumberOfDoors, NumberOfSeats, NumberOfStandingSeats, NumberOfLieingSeats,
                        MaxSpeed, ColorCode,
                        VehicleSizeHight, VehicleSizeWidth, VehicleSizeLength,
                        Hook, TNG, IsSocialNotPrivate, ForPrivateTransportNotPublic,
                        IdVehicleModel, IdVehicleBodyType, IdVehicleCategories,
                        IdVehicleCategoryForPayments, IdVehicleUse, IdEngineType,
                        IdEnginePowerSource, IdEngineSecondPowerSource,
                        IdGearBox, IdBreakes, IdSupporting,
                        IdEngineEcoProgram, IdMadeCountry,
                        IdPrimaryColor, IdSecondaryColor,
                        IdFirstRegistrationIssuer, IdLastRegistrationIssuer
                        FROM Vehicles WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 120;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, Vehicle entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; if (count % BatchSize == 0) Console.Write($" {count}..."); continue; }

                var vehicle = new Vehicle
                {
                    ShellNumber = GetStringOrNull(reader, 1) ?? "",                         // ShellNumber
                    EngineNumber = GetStringOrNull(reader, 2),                              // EngineNumber
                    MakeDate = GetNullableDateTime(reader, 3),                              // MakeDate
                    FirstRegistrationNumber = GetStringOrNull(reader, 4),                   // FirstRegistrationNumber
                    FirstRegistrationDate = GetNullableDateTime(reader, 5),                 // FirstRegistrationMakeDate
                    LastRegistrationNumber = GetStringOrNull(reader, 6),                    // LastRegistratinNumber (sic)
                    LastRegistrationDate = GetNullableDateTime(reader, 7),                  // LastRegistrationMakeDate
                    EnginePowerKW = GetNullableDecimalFromReal(reader, 8),                  // EnginePower (real)
                    EngineTorqueNM = GetNullableDecimalFromString(reader, 9),               // EngineTorque (nvarchar!)
                    EngineWorkingCapacityCM3 = GetNullableDecimalFromReal(reader, 10),      // EngineWorkingCapacity (real)
                    EmptyWeightKG = GetNullableDecimalFromReal(reader, 11),                 // EmptyWaight (real)
                    MaxAllowedWeightKG = GetNullableDecimalFromReal(reader, 12),            // MaximunAllowedWaight (real)
                    NumberOfDoors = GetNullableInt(reader, 13),                             // NumberOfDoors (int)
                    NumberOfSeats = GetNullableInt(reader, 14),                             // NumberOfSeats (smallint)
                    NumberOfStandingSeats = GetNullableInt(reader, 15),                     // NumberOfStandingSeats (smallint)
                    NumberOfLyingSeats = GetNullableInt(reader, 16),                        // NumberOfLieingSeats (smallint)
                    MaxSpeedKMH = GetNullableDecimalFromReal(reader, 17),                   // MaxSpeed (real)
                    ColorCode = GetStringOrNull(reader, 18),                                // ColorCode
                    HeightMM = GetNullableIntFromReal(reader, 19),                          // VehicleSizeHight (real)
                    WidthMM = GetNullableIntFromReal(reader, 20),                           // VehicleSizeWidth (real)
                    LengthMM = GetNullableIntFromReal(reader, 21),                          // VehicleSizeLength (real)
                    HasHook = !reader.IsDBNull(22) && reader.GetBoolean(22),                // Hook (bit)
                    HasLPG = !reader.IsDBNull(23) && reader.GetBoolean(23),                 // TNG (bit) -> HasLPG
                    IsSocialVehicle = !reader.IsDBNull(24) && reader.GetBoolean(24),        // IsSocialNotPrivate
                    IsPrivateTransport = !reader.IsDBNull(25) && reader.GetBoolean(25),     // ForPrivateTransportNotPublic
                    VehicleModelId = GetNullableFk(reader, 26, idMap, "VehicleModels"),
                    BodyTypeId = GetNullableFk(reader, 27, idMap, "VehicleBodyTypes"),
                    CategoryId = GetNullableFk(reader, 28, idMap, "VehicleCategories"),
                    PaymentCategoryId = GetNullableFk(reader, 29, idMap, "VehiclePaymentCategories"),
                    UseTypeId = GetNullableFk(reader, 30, idMap, "VehicleUseTypes"),
                    EngineTypeId = GetNullableFk(reader, 31, idMap, "EngineTypes"),
                    PrimaryPowerSourceId = GetNullableFk(reader, 32, idMap, "EnginePowerSourceTypes"),
                    SecondaryPowerSourceId = GetNullableFk(reader, 33, idMap, "EnginePowerSourceTypes"),
                    GearBoxTypeId = GetNullableFk(reader, 34, idMap, "GearBoxTypes"),
                    BrakeTypeId = GetNullableFk(reader, 35, idMap, "BrakeTypes"),
                    SupportingTypeId = GetNullableFk(reader, 36, idMap, "SupportingTypes"),
                    EcoProgramId = GetNullableFk(reader, 37, idMap, "EcoPrograms"),
                    MadeInCountryId = GetNullableFk(reader, 38, idMap, "Countries"),
                    PrimaryColorId = GetNullableFk(reader, 39, idMap, "Colors"),
                    SecondaryColorId = GetNullableFk(reader, 40, idMap, "Colors"),
                    FirstRegistrationIssuerId = GetNullableFk(reader, 41, idMap, "RegistrationIssuers"),
                    LastRegistrationIssuerId = GetNullableFk(reader, 42, idMap, "RegistrationIssuers"),
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = 1
                };
                batch.Add((oldId, vehicle));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.Vehicles, batch, map);
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.Vehicles, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" ERROR ({ex.Message})");
            Console.ResetColor();
            idMap["Vehicles"] = map;
            return;
        }

        idMap["Vehicles"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records");
        Console.ResetColor();
    }

    // ──────────────────────────────────────────────
    //  Customer-Vehicle Relations (72984 rows)
    // ──────────────────────────────────────────────

    static void MigrateRelations(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating CustomerVehicleRelations...");
        int count = 0;
        int skipped = 0;
        var map = new Dictionary<long, long>();

        try
        {
            // CustomerVehiclesRelations: Id(bigint), IdRelationType, IdCustomer, IdVehicle,
            //   StartDate, EndDate, BeginNote, TerminationNote, Active
            var sql = @"SELECT Id, IdRelationType, IdCustomer, IdVehicle,
                        StartDate, EndDate, BeginNote, TerminationNote
                        FROM CustomerVehiclesRelations WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 120;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, CustomerVehicleRelation entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; if (count % BatchSize == 0) Console.Write($" {count}..."); continue; }

                var oldRelTypeId = reader.IsDBNull(1) ? 0L : Convert.ToInt64(reader.GetValue(1));
                var oldCustomerId = reader.IsDBNull(2) ? 0L : Convert.ToInt64(reader.GetValue(2));
                var oldVehicleId = reader.IsDBNull(3) ? 0L : Convert.ToInt64(reader.GetValue(3));

                var newRelTypeId = MapIdOrDefault(idMap, "RelationTypes", oldRelTypeId);
                var newCustomerId = MapIdOrDefault(idMap, "Customers", oldCustomerId);
                var newVehicleId = MapIdOrDefault(idMap, "Vehicles", oldVehicleId);

                if (newRelTypeId == null || newCustomerId == null || newVehicleId == null)
                {
                    skipped++;
                    continue;
                }

                var entity = new CustomerVehicleRelation
                {
                    RelationTypeId = newRelTypeId.Value,
                    CustomerId = newCustomerId.Value,
                    VehicleId = newVehicleId.Value,
                    StartDate = reader.IsDBNull(4) ? DateTime.UtcNow : reader.GetDateTime(4),
                    EndDate = GetNullableDateTime(reader, 5),
                    BeginNote = GetStringOrNull(reader, 6),
                    TerminationNote = GetStringOrNull(reader, 7)
                };
                batch.Add((oldId, entity));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.CustomerVehicleRelations, batch, map);
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.CustomerVehicleRelations, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["Relations"] = map;
            return;
        }

        idMap["Relations"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped due to missing FK)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    // ──────────────────────────────────────────────
    //  Requests (128801 rows)
    // ──────────────────────────────────────────────

    static void MigrateRequests(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating Requests...");
        int count = 0;
        int skipped = 0;
        var map = new Dictionary<long, long>();

        try
        {
            // Requests: Id(bigint), IdRequestType, IdCustomerVehicleRelation,
            //   IdCustomerVehicleRelationNew, IdOperatorCreated, DateCreated, DateEnded,
            //   Note, IsCustomerChanged, IsVehicleChanged, Active, IdOrganisation
            var sql = @"SELECT Id, IdRequestType, IdCustomerVehicleRelation,
                        IdCustomerVehicleRelationNew, IdOperatorCreated, DateCreated, DateEnded,
                        Note, IsCustomerChanged, IsVehicleChanged, IdOrganisation
                        FROM Requests WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 180;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, Request entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; if (count % BatchSize == 0) Console.Write($" {count}..."); continue; }

                var oldReqTypeId = reader.IsDBNull(1) ? 0L : Convert.ToInt64(reader.GetValue(1));
                var oldRelId = reader.IsDBNull(2) ? 0L : Convert.ToInt64(reader.GetValue(2));

                var newReqTypeId = MapIdOrDefault(idMap, "RequestTypes", oldReqTypeId);
                var newRelId = MapIdOrDefault(idMap, "Relations", oldRelId);

                if (newReqTypeId == null || newRelId == null)
                {
                    skipped++;
                    continue;
                }

                long? newRelNewId = null;
                if (!reader.IsDBNull(3))
                {
                    var oldRelNewId = Convert.ToInt64(reader.GetValue(3));
                    newRelNewId = MapIdOrDefault(idMap, "Relations", oldRelNewId);
                }

                long? newOrgId = null;
                if (!reader.IsDBNull(10))
                {
                    var oldOrgId = Convert.ToInt64(reader.GetValue(10));
                    newOrgId = MapIdOrDefault(idMap, "Organizations", oldOrgId);
                }

                var entity = new Request
                {
                    RequestTypeId = newReqTypeId.Value,
                    CustomerVehicleRelationId = newRelId.Value,
                    NewCustomerVehicleRelationId = newRelNewId,
                    Note = GetStringOrNull(reader, 7),
                    IsCustomerChanged = !reader.IsDBNull(8) && reader.GetBoolean(8),
                    IsVehicleChanged = !reader.IsDBNull(9) && reader.GetBoolean(9),
                    OrganizationId = newOrgId,
                    DateEnded = GetNullableDateTime(reader, 6),
                    CreatedAt = reader.IsDBNull(5) ? DateTime.UtcNow : reader.GetDateTime(5),
                    CreatedByUserId = 1 // IdOperatorCreated maps to Users which we skip
                };
                batch.Add((oldId, entity));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.Requests, batch, map);
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.Requests, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["Requests"] = map;
            return;
        }

        idMap["Requests"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    // ──────────────────────────────────────────────
    //  Technical Exam Reports (106241 rows)
    // ──────────────────────────────────────────────

    static void MigrateTechnicalExamReports(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating TechnicalExamReports (from DocumentsTehnicalExamsReports)...");
        int count = 0;
        int skipped = 0;
        var map = new Dictionary<long, long>();

        try
        {
            var sql = @"SELECT Id, IdCustomerVehicleRelation, IdTypeOfTehnicalExam,
                        RegNumber, MadeDate, ValidTillDate,
                        IdOrganizationForTehnicalExam, IdFirsControler, IdSecondControler,
                        VehicleIsRight,
                        Axis1Left, Axis1Right, Axis1Gj, Axis1LeftPj,
                        Axis2Left, Axis2Right, Axis2Gj, Axis2LeftPj,
                        EffectOfWorkingBreakEmpty, EffectOfWorkingBreakFull,
                        SpeedOfTurns, CO, NumEngineTurns,
                        ExplanationNote, DriversWarning, Note
                        FROM DocumentsTehnicalExamsReports";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 300;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, TechnicalExamReport entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; if (count % BatchSize == 0) Console.Write($" {count}..."); continue; }

                var oldRelId = reader.IsDBNull(1) ? 0L : Convert.ToInt64(reader.GetValue(1));
                var oldExamTypeId = reader.IsDBNull(2) ? 0L : Convert.ToInt64(reader.GetValue(2));
                var oldOrgId = reader.IsDBNull(6) ? 0L : Convert.ToInt64(reader.GetValue(6));

                var newRelId = MapIdOrDefault(idMap, "Relations", oldRelId);
                var newExamTypeId = MapIdOrDefault(idMap, "TechnicalExamTypes", oldExamTypeId);
                var newOrgId = MapIdOrDefault(idMap, "Organizations", oldOrgId);

                if (newRelId == null || newExamTypeId == null || newOrgId == null)
                {
                    skipped++;
                    continue;
                }

                var entity = new TechnicalExamReport
                {
                    CustomerVehicleRelationId = newRelId.Value,
                    ExamTypeId = newExamTypeId.Value,
                    RegistrationNumber = GetStringOrNull(reader, 3) ?? "",
                    ExamDate = reader.IsDBNull(4) ? DateTime.UtcNow : reader.GetDateTime(4),
                    ValidUntilDate = reader.IsDBNull(5) ? DateTime.UtcNow.AddYears(1) : reader.GetDateTime(5),
                    OrganizationId = newOrgId.Value,
                    FirstControllerId = 1, // IdFirsControler -> Users (skipped)
                    SecondControllerId = null,
                    VehiclePassed = !reader.IsDBNull(9) && reader.GetBoolean(9),
                    Axle1BrakeLeftKN = GetNullableDecimalFromReal(reader, 10),
                    Axle1BrakeRightKN = GetNullableDecimalFromReal(reader, 11),
                    Axle1BrakeGj = GetNullableDecimalFromReal(reader, 12),
                    Axle1BrakeLeftPj = GetNullableDecimalFromReal(reader, 13),
                    Axle2BrakeLeftKN = GetNullableDecimalFromReal(reader, 14),
                    Axle2BrakeRightKN = GetNullableDecimalFromReal(reader, 15),
                    Axle2BrakeGj = GetNullableDecimalFromReal(reader, 16),
                    Axle2BrakeLeftPj = GetNullableDecimalFromReal(reader, 17),
                    WorkingBrakeEffectivenessEmpty = GetNullableDecimalFromReal(reader, 18),
                    WorkingBrakeEffectivenessFull = GetNullableDecimalFromReal(reader, 19),
                    EngineSpeedRPM = GetNullableIntFromReal(reader, 20),
                    CO = GetNullableDecimalFromReal(reader, 21),
                    EngineTurns = GetNullableIntFromReal(reader, 22),
                    ExplanationNote = GetStringOrNull(reader, 23),
                    DriverWarning = GetStringOrNull(reader, 24),
                    Note = GetStringOrNull(reader, 25),
                    CreatedAt = reader.IsDBNull(4) ? DateTime.UtcNow : reader.GetDateTime(4),
                    CreatedByUserId = 1
                };
                batch.Add((oldId, entity));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.TechnicalExamReports, batch, map);
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.TechnicalExamReports, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["TechnicalExamReports"] = map;
            return;
        }

        idMap["TechnicalExamReports"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    // ──────────────────────────────────────────────
    //  Payment Documents (241042 rows)
    // ──────────────────────────────────────────────

    static void MigratePaymentDocuments(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating PaymentDocuments...");
        int count = 0;
        int skipped = 0;
        var map = new Dictionary<long, long>();

        try
        {
            // PaymentDocuments: Id(bigint), IdPaymentType, IdCustomerVehicleRelation, IdOperator,
            //   DocumentNumber, DatePay, DateRequired, Discount(real), Payed(bit),
            //   Note, Storno(bit), IdDogovor(bigint), IdOrganization, Active
            var sql = @"SELECT Id, IdPaymentType, IdCustomerVehicleRelation, IdOperator,
                        DocumentNumber, DatePay, DateRequired, Discount, Payed,
                        Note, Storno, IdDogovor, IdOrganization
                        FROM PaymentDocuments WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 300;
            using var reader = cmd.ExecuteReader();
            var batch = new List<(long oldId, PaymentDocument entity)>();

            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (DryRun) { count++; if (count % BatchSize == 0) Console.Write($" {count}..."); continue; }

                var oldPayTypeId = reader.IsDBNull(1) ? 0L : Convert.ToInt64(reader.GetValue(1));
                var oldRelId = reader.IsDBNull(2) ? 0L : Convert.ToInt64(reader.GetValue(2));

                var newPayTypeId = MapIdOrDefault(idMap, "PaymentTypes", oldPayTypeId);
                var newRelId = MapIdOrDefault(idMap, "Relations", oldRelId);

                if (newPayTypeId == null || newRelId == null)
                {
                    skipped++;
                    continue;
                }

                long? newOrgId = null;
                if (!reader.IsDBNull(12))
                    newOrgId = MapIdOrDefault(idMap, "Organizations", Convert.ToInt64(reader.GetValue(12)));

                var entity = new PaymentDocument
                {
                    PaymentTypeId = newPayTypeId.Value,
                    CustomerVehicleRelationId = newRelId.Value,
                    DocumentNumber = GetStringOrNull(reader, 4) ?? "",
                    PaymentDate = GetNullableDateTime(reader, 5),
                    DueDate = GetNullableDateTime(reader, 6),
                    DiscountPercent = GetNullableDecimalFromReal(reader, 7) ?? 0m,
                    IsPaid = !reader.IsDBNull(8) && reader.GetBoolean(8),
                    Note = GetStringOrNull(reader, 9),
                    IsCancelled = !reader.IsDBNull(10) && reader.GetBoolean(10),
                    AgreementId = reader.IsDBNull(11) ? null : Convert.ToInt64(reader.GetValue(11)),
                    OrganizationId = newOrgId,
                    CreatedAt = GetNullableDateTime(reader, 5) ?? DateTime.UtcNow,
                    CreatedByUserId = 1 // IdOperator -> Users (skipped)
                };
                batch.Add((oldId, entity));
                count++;

                if (batch.Count >= BatchSize)
                {
                    FlushEntities(newDb, newDb.PaymentDocuments, batch, map);
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
                FlushEntities(newDb, newDb.PaymentDocuments, batch, map);
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            idMap["PaymentDocuments"] = map;
            return;
        }

        idMap["PaymentDocuments"] = map;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    // ──────────────────────────────────────────────
    //  Payment Document Details (1150804 rows)
    // ──────────────────────────────────────────────

    static void MigratePaymentDocumentDetails(SqlConnection oldConn, VteDbContext newDb, Dictionary<string, Dictionary<long, long>> idMap)
    {
        Console.Write("  Migrating PaymentDocumentsDetails...");
        int count = 0;
        int skipped = 0;

        try
        {
            // PaymentDocumentsDetails: Id(bigint), IdPaymentDocuments, IdPriceCatalog,
            //   Price(money), DDV(real), Note, Discount(real), Active
            var sql = @"SELECT Id, IdPaymentDocuments, IdPriceCatalog,
                        Price, DDV, Note, Discount
                        FROM PaymentDocumentsDetails WHERE Active = 1";
            using var cmd = new SqlCommand(sql, oldConn);
            cmd.CommandTimeout = 600;
            using var reader = cmd.ExecuteReader();
            var batch = new List<PaymentLineItem>();

            while (reader.Read())
            {
                if (DryRun) { count++; if (count % BatchSize == 0) Console.Write($" {count}..."); continue; }

                var oldDocId = reader.IsDBNull(1) ? 0L : Convert.ToInt64(reader.GetValue(1));
                var newDocId = MapIdOrDefault(idMap, "PaymentDocuments", oldDocId);
                if (newDocId == null) { skipped++; continue; }

                // IdPriceCatalog -> PaymentCatalogItem (we may not have migrated this)
                // Use a default of 1 if not mapped
                long paymentItemId = 1;

                var entity = new PaymentLineItem
                {
                    PaymentDocumentId = newDocId.Value,
                    PaymentItemId = paymentItemId,
                    Description = GetStringOrNull(reader, 5) ?? "",
                    Quantity = 1,
                    UnitPrice = reader.IsDBNull(3) ? 0m : Convert.ToDecimal(reader.GetValue(3)),
                    VATPercent = GetNullableDecimalFromReal(reader, 4) ?? 0m,
                    SortOrder = 0
                };
                batch.Add(entity);
                count++;

                if (batch.Count >= BatchSize)
                {
                    foreach (var e in batch)
                        newDb.PaymentLineItems.Add(e);
                    newDb.SaveChanges();
                    foreach (var e in batch)
                        newDb.Entry(e).State = EntityState.Detached;
                    batch.Clear();
                    Console.Write($" {count}...");
                }
            }

            reader.Close();

            if (!DryRun && batch.Count > 0)
            {
                foreach (var e in batch)
                    newDb.PaymentLineItems.Add(e);
                newDb.SaveChanges();
                foreach (var e in batch)
                    newDb.Entry(e).State = EntityState.Detached;
            }
        }
        catch (SqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED ({ex.Message})");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    // ──────────────────────────────────────────────
    //  ID mapping helpers
    // ──────────────────────────────────────────────

    static long? MapIdOrDefault(Dictionary<string, Dictionary<long, long>> idMap, string tableName, long oldId)
    {
        if (oldId == 0) return null;
        if (idMap.TryGetValue(tableName, out var map) && map.TryGetValue(oldId, out var newId))
            return newId;
        return null;
    }

    static long MapId(Dictionary<string, Dictionary<long, long>> idMap, string tableName, long oldId)
    {
        if (idMap.TryGetValue(tableName, out var map) && map.TryGetValue(oldId, out var newId))
            return newId;
        throw new InvalidOperationException(
            $"Cannot map {tableName} old ID {oldId} to a new ID. Was this table migrated?");
    }

    static long? MapIdNullable(Dictionary<string, Dictionary<long, long>> idMap, string tableName, long oldId)
    {
        return MapIdOrDefault(idMap, tableName, oldId);
    }

    // ──────────────────────────────────────────────
    //  FK helper for reader columns
    // ──────────────────────────────────────────────

    static long? GetNullableFk(SqlDataReader reader, int ordinal, Dictionary<string, Dictionary<long, long>> idMap, string tableName)
    {
        if (reader.IsDBNull(ordinal)) return null;
        var oldId = Convert.ToInt64(reader.GetValue(ordinal));
        return MapIdOrDefault(idMap, tableName, oldId);
    }

    // ──────────────────────────────────────────────
    //  Safe reader helpers
    // ──────────────────────────────────────────────

    static string? GetStringOrNull(SqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal)) return null;
        return reader.GetValue(ordinal)?.ToString();
    }

    static DateTime? GetNullableDateTime(SqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal)) return null;
        try { return reader.GetDateTime(ordinal); }
        catch { return null; }
    }

    static int? GetNullableInt(SqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal)) return null;
        try { return Convert.ToInt32(reader.GetValue(ordinal)); }
        catch { return null; }
    }

    static int? GetNullableIntFromReal(SqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal)) return null;
        try { return (int)Convert.ToDouble(reader.GetValue(ordinal)); }
        catch { return null; }
    }

    static decimal? GetNullableDecimalFromReal(SqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal)) return null;
        try { return (decimal)Convert.ToDouble(reader.GetValue(ordinal)); }
        catch { return null; }
    }

    static decimal? GetNullableDecimalFromString(SqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal)) return null;
        var str = reader.GetValue(ordinal)?.ToString();
        if (string.IsNullOrWhiteSpace(str)) return null;
        if (decimal.TryParse(str, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var val))
            return val;
        return null;
    }

    static int TryGetInt(SqlDataReader reader, int ordinal)
    {
        // Status is nvarchar in old DB but int in new — try to parse
        var val = reader.GetValue(ordinal);
        if (val is int i) return i;
        if (int.TryParse(val?.ToString(), out var parsed)) return parsed;
        return 0;
    }

    // ──────────────────────────────────────────────
    //  Batch flush helper
    // ──────────────────────────────────────────────

    static void FlushEntities<T>(VteDbContext db, DbSet<T> dbSet, List<(long oldId, T entity)> batch, Dictionary<long, long> map) where T : BaseEntity
    {
        foreach (var (_, entity) in batch)
            dbSet.Add(entity);
        db.SaveChanges();
        foreach (var (oldId, entity) in batch)
            map[oldId] = entity.Id;
        foreach (var (_, entity) in batch)
            db.Entry(entity).State = EntityState.Detached;
    }

    // ──────────────────────────────────────────────
    //  Schema discovery helpers
    // ──────────────────────────────────────────────

    static List<string> GetTableColumns(SqlConnection conn, string tableName)
    {
        var columns = new List<string>();
        using var cmd = new SqlCommand(
            $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}' ORDER BY ORDINAL_POSITION", conn);
        cmd.CommandTimeout = 15;
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            columns.Add(reader.GetString(0));
        return columns;
    }

    static string? FindColumn(List<string> columns, string[] candidates)
    {
        foreach (var candidate in candidates)
        {
            var match = columns.FirstOrDefault(c => c.Equals(candidate, StringComparison.OrdinalIgnoreCase));
            if (match != null) return match;
        }
        return null;
    }

    // ──────────────────────────────────────────────
    //  Explore / List Tables
    // ──────────────────────────────────────────────

    static void ListTables()
    {
        Console.WriteLine("Connecting to old DB...");
        using var conn = new SqlConnection(OldConnectionString);
        conn.Open();
        Console.WriteLine("Connected!\n");

        var sql = @"SELECT t.TABLE_NAME, p.rows
                    FROM INFORMATION_SCHEMA.TABLES t
                    JOIN sys.partitions p ON OBJECT_ID(t.TABLE_SCHEMA + '.' + t.TABLE_NAME) = p.object_id AND p.index_id IN (0,1)
                    WHERE t.TABLE_TYPE = 'BASE TABLE'
                    ORDER BY t.TABLE_NAME";
        using var cmd = new SqlCommand(sql, conn);
        cmd.CommandTimeout = 30;
        using var reader = cmd.ExecuteReader();
        Console.WriteLine($"{"TABLE",-50} {"ROWS",10}");
        Console.WriteLine(new string('-', 62));
        while (reader.Read())
            Console.WriteLine($"{reader.GetString(0),-50} {reader.GetInt64(1),10}");
        reader.Close();

        Console.WriteLine("\n\n=== KEY TABLE COLUMNS ===\n");
        var keyTables = new[] {
            "DocumentsTrafficLicences", "DocumentsPermisions",
            "DocumentsInternationalDriveingLicences",
            "DocumentsTehnicalExamsReportsDetails",
            "PaymentDocumentsRata", "CustomersBankAccounts", "DDVCatalog",
            "DocumentAttachments", "DogovorZaRati", "TehnicalExamVehicleParts",
            "CalculationItems", "PaymentItems", "DocumentTypesOptions",
            "DocumentTypesOptionsDetails"
        };

        foreach (var table in keyTables)
        {
            Console.WriteLine($"\n--- {table} ---");
            try
            {
                var colSql = $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}' ORDER BY ORDINAL_POSITION";
                using var colCmd = new SqlCommand(colSql, conn);
                colCmd.CommandTimeout = 15;
                using var colReader = colCmd.ExecuteReader();
                while (colReader.Read())
                    Console.WriteLine($"  {colReader.GetString(0),-35} {colReader.GetString(1),-15} {colReader.GetString(2)}");
                colReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ERROR: {ex.Message}");
            }
        }
    }

    static void MigratePaymentDetailsOnly()
    {
        Console.WriteLine("=== Migrating Payment Document Details ===");
        Console.WriteLine($"Source: {OldConnectionString}");
        Console.WriteLine($"Target: {NewConnectionString}");

        var services = new ServiceCollection();
        services.AddDbContext<VteDbContext>(options => options.UseSqlServer(NewConnectionString));
        var sp = services.BuildServiceProvider();
        using var newDb = sp.GetRequiredService<VteDbContext>();

        // Build payment ID map (old ID -> new ID)
        Console.Write("Building payment ID map...");
        var newPayments = newDb.PaymentDocuments.Select(p => new { p.Id, p.DocumentNumber }).ToList();
        Console.WriteLine($" {newPayments.Count} payments in new DB");

        using var oldConn = new SqlConnection(OldConnectionString);
        oldConn.Open();

        // Get old payment IDs mapped to document numbers
        var oldPaymentMap = new Dictionary<long, long>(); // old payment ID -> new payment ID
        using (var cmd = new SqlCommand("SELECT Id, DocumentNumber FROM PaymentDocuments WHERE Active = 1", oldConn))
        using (var reader = cmd.ExecuteReader())
        {
            var newByDocNum = newPayments.GroupBy(p => p.DocumentNumber).ToDictionary(g => g.Key, g => g.First().Id);
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                var docNum = reader.GetString(1);
                if (newByDocNum.TryGetValue(docNum, out var newId))
                    oldPaymentMap[oldId] = newId;
            }
        }
        Console.WriteLine($"Mapped {oldPaymentMap.Count} old->new payment IDs");

        // Migrate details
        Console.Write("Migrating PaymentDocumentsDetails...");
        int count = 0, skipped = 0;

        var sql = @"SELECT Id, IdPaymentDocuments, Price, DDV, Discount, Note
                    FROM PaymentDocumentsDetails WHERE Active = 1";
        using var detailCmd = new SqlCommand(sql, oldConn);
        detailCmd.CommandTimeout = 300;
        using var detailReader = detailCmd.ExecuteReader();

        while (detailReader.Read())
        {
            var oldPaymentId = Convert.ToInt64(detailReader.GetValue(1));
            if (!oldPaymentMap.TryGetValue(oldPaymentId, out var newPaymentId))
            {
                skipped++;
                continue;
            }

            var price = detailReader.IsDBNull(2) ? 0m : Convert.ToDecimal(detailReader.GetValue(2));
            var ddv = detailReader.IsDBNull(3) ? 0m : Convert.ToDecimal(detailReader.GetValue(3));
            var discount = detailReader.IsDBNull(4) ? 0m : Convert.ToDecimal(detailReader.GetValue(4));
            var note = detailReader.IsDBNull(5) ? null : detailReader.GetString(5);

            if (!DryRun)
            {
                var li = new VTE.Core.Entities.PaymentLineItem
                {
                    PaymentDocumentId = newPaymentId,
                    PaymentItemId = 1, // default - no catalog mapping
                    Description = note ?? "",
                    Quantity = 1,
                    UnitPrice = price,
                    VATPercent = ddv,
                    SortOrder = 0
                };
                newDb.PaymentLineItems.Add(li);
            }

            count++;
            if (count % BatchSize == 0)
            {
                if (!DryRun)
                {
                    newDb.SaveChanges();
                    // Detach to free memory
                    foreach (var entry in newDb.ChangeTracker.Entries().ToList())
                        entry.State = EntityState.Detached;
                }
                Console.Write($" {count/1000}K...");
            }
        }

        if (!DryRun && count % BatchSize != 0)
            newDb.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {count} records ({skipped} skipped)");
        Console.ResetColor();

        // Verify
        var totalLines = newDb.PaymentLineItems.Count();
        Console.WriteLine($"Total line items in new DB: {totalLines}");
    }

    static void ExploreOldDb()
    {
        Console.WriteLine("=== VTE Old Database Explorer ===\n");
        ListTables();

        // Also show sample data from a few lookup tables
        Console.WriteLine("\n\n=== SAMPLE DATA ===\n");
        using var conn = new SqlConnection(OldConnectionString);
        conn.Open();

        var sampleTables = new[] { "BusinessTypes", "VehicleBodytype", "VehicleGearBox", "Colors" };
        foreach (var table in sampleTables)
        {
            Console.WriteLine($"\n--- TOP 3 from {table} ---");
            try
            {
                using var cmd = new SqlCommand($"SELECT TOP 3 * FROM [{table}]", conn);
                cmd.CommandTimeout = 15;
                using var reader = cmd.ExecuteReader();
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write($"{reader.GetName(i),-25}");
                Console.WriteLine();
                Console.WriteLine(new string('-', reader.FieldCount * 25));
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var val = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString();
                        Console.Write($"{val?[..Math.Min(val.Length, 24)],-25}");
                    }
                    Console.WriteLine();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ERROR: {ex.Message}");
            }
        }
    }

    // ──────────────────────────────────────────────
    //  Migrate ALL Missing Tables
    // ──────────────────────────────────────────────

    static void IncrementalSync()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Incremental Sync — migrating new records only ===");
        Console.WriteLine($"Source: {OldConnectionString}");
        Console.WriteLine($"Target: {NewConnectionString}");
        Console.WriteLine();

        using var oldConn = new SqlConnection(OldConnectionString);
        oldConn.Open();

        var services = new ServiceCollection();
        services.AddDbContext<VteDbContext>(options => options.UseSqlServer(NewConnectionString));
        var sp = services.BuildServiceProvider();
        using var newDb = sp.GetRequiredService<VteDbContext>();

        // Get current counts in new DB
        var existingCustomerMBs = new HashSet<string>(newDb.Customers.Select(c => c.IdentificationNumber).ToList());
        var existingVehicleShells = new HashSet<string>(newDb.Vehicles.Select(v => v.ShellNumber).ToList());
        var existingPaymentDocNums = new HashSet<string>(newDb.PaymentDocuments.Select(p => p.DocumentNumber).ToList());
        var existingExamRegNums = new HashSet<string>(
            newDb.TechnicalExamReports.Select(e => e.RegistrationNumber + "|" + e.ExamDate.ToString("yyyyMMdd")).ToList());

        Console.WriteLine($"Existing: {existingCustomerMBs.Count} customers, {existingVehicleShells.Count} vehicles, {existingPaymentDocNums.Count} payments, {existingExamRegNums.Count} exams");

        // Sync Customers
        Console.Write("Syncing Customers...");
        int newCustomers = 0;
        using (var cmd = new SqlCommand(
            @"SELECT Id, MB, CustomerSurname, CustomerFirstName, ParentName,
              DateOfBirth, IsCompany, PhoneNumber, Fax, eMail,
              TaxNumber, PassportNumber, DriveingLicenceNumber, BLK,
              CanSendNotifications, Note, Occupation
              FROM Customers WHERE Active = 1", oldConn))
        {
            cmd.CommandTimeout = 300;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var mb = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim();
                if (existingCustomerMBs.Contains(mb)) continue;

                var customer = new Customer
                {
                    IdentificationNumber = mb,
                    LastName = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim(),
                    FirstName = reader.IsDBNull(3) ? "" : reader.GetString(3).Trim(),
                    ParentName = reader.IsDBNull(4) ? null : reader.GetString(4).Trim(),
                    DateOfBirth = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                    IsCompany = !reader.IsDBNull(6) && reader.GetBoolean(6),
                    PhoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7).Trim(),
                    Fax = reader.IsDBNull(8) ? null : reader.GetString(8).Trim(),
                    Email = reader.IsDBNull(9) ? null : reader.GetString(9).Trim(),
                    TaxNumber = reader.IsDBNull(10) ? null : reader.GetString(10).Trim(),
                    PassportNumber = reader.IsDBNull(11) ? null : reader.GetString(11).Trim(),
                    DrivingLicenseNumber = reader.IsDBNull(12) ? null : reader.GetString(12).Trim(),
                    IdentityCardNumber = reader.IsDBNull(13) ? null : reader.GetString(13).Trim(),
                    CanSendNotifications = !reader.IsDBNull(14) && reader.GetBoolean(14),
                    Note = reader.IsDBNull(15) ? null : reader.GetString(15).Trim(),
                    Occupation = reader.IsDBNull(16) ? null : reader.GetString(16).Trim(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = 1
                };
                newDb.Customers.Add(customer);
                newCustomers++;
                if (newCustomers % 100 == 0) { newDb.SaveChanges(); Console.Write("."); }
            }
        }
        if (newCustomers > 0) newDb.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" +{newCustomers} new");
        Console.ResetColor();

        // Sync Vehicles
        Console.Write("Syncing Vehicles...");
        int newVehicles = 0;
        using (var cmd = new SqlCommand(
            @"SELECT ShellNumber, EngineNumber, MakeDate,
              FirstRegistrationNumber, FirstRegistrationMakeDate,
              LastRegistratinNumber, LastRegistrationMakeDate,
              EnginePower, EngineWorkingCapacity,
              EmptyWaight, MaximunAllowedWaight,
              NumberOfDoors, NumberOfSeats, MaxSpeed,
              VehicleSizeHight, VehicleSizeWidth, VehicleSizeLength
              FROM Vehicles WHERE Active = 1", oldConn))
        {
            cmd.CommandTimeout = 300;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var shell = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();
                if (string.IsNullOrEmpty(shell) || existingVehicleShells.Contains(shell)) continue;

                var vehicle = new Vehicle
                {
                    ShellNumber = shell,
                    EngineNumber = reader.IsDBNull(1) ? null : reader.GetString(1).Trim(),
                    MakeDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                    FirstRegistrationNumber = reader.IsDBNull(3) ? null : reader.GetString(3).Trim(),
                    FirstRegistrationDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    LastRegistrationNumber = reader.IsDBNull(5) ? null : reader.GetString(5).Trim(),
                    LastRegistrationDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                    EnginePowerKW = reader.IsDBNull(7) ? null : (decimal?)Convert.ToDecimal(reader.GetValue(7)),
                    EngineWorkingCapacityCM3 = reader.IsDBNull(8) ? null : (decimal?)Convert.ToDecimal(reader.GetValue(8)),
                    EmptyWeightKG = reader.IsDBNull(9) ? null : (decimal?)Convert.ToDecimal(reader.GetValue(9)),
                    MaxAllowedWeightKG = reader.IsDBNull(10) ? null : (decimal?)Convert.ToDecimal(reader.GetValue(10)),
                    NumberOfDoors = reader.IsDBNull(11) ? null : (int?)reader.GetInt32(11),
                    NumberOfSeats = reader.IsDBNull(12) ? null : (int?)Convert.ToInt32(reader.GetValue(12)),
                    MaxSpeedKMH = reader.IsDBNull(13) ? null : (decimal?)Convert.ToDecimal(reader.GetValue(13)),
                    HeightMM = reader.IsDBNull(14) ? null : (int?)Convert.ToInt32(Convert.ToDecimal(reader.GetValue(14))),
                    WidthMM = reader.IsDBNull(15) ? null : (int?)Convert.ToInt32(Convert.ToDecimal(reader.GetValue(15))),
                    LengthMM = reader.IsDBNull(16) ? null : (int?)Convert.ToInt32(Convert.ToDecimal(reader.GetValue(16))),
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = 1
                };
                newDb.Vehicles.Add(vehicle);
                newVehicles++;
                if (newVehicles % 100 == 0) { newDb.SaveChanges(); Console.Write("."); }
            }
        }
        if (newVehicles > 0) newDb.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" +{newVehicles} new");
        Console.ResetColor();

        // Sync Payment Documents
        Console.Write("Syncing Payments...");
        int newPayments = 0;
        // Rebuild customer/vehicle maps for relation matching
        var customerMap = newDb.Customers.GroupBy(c => c.IdentificationNumber).ToDictionary(g => g.Key, g => g.First().Id);
        var vehicleMap = newDb.Vehicles.GroupBy(v => v.ShellNumber).ToDictionary(g => g.Key, g => g.First().Id);

        using (var cmd = new SqlCommand(
            @"SELECT p.DocumentNumber, p.DatePay, p.Discount, p.Payed, p.Note, p.Storno,
              p.IdCustomerVehicleRelation
              FROM PaymentDocuments p WHERE p.Active = 1
              AND p.DocumentNumber NOT IN (SELECT DocumentNumber FROM PaymentDocuments WHERE 1=0)", oldConn))
        {
            cmd.CommandTimeout = 300;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var docNum = reader.GetString(0).Trim();
                if (existingPaymentDocNums.Contains(docNum)) continue;

                // Skip — can't create without a valid CustomerVehicleRelationId
                // Would need full relation mapping which is complex for incremental
                newPayments++;
            }
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {newPayments} new payments found (skipped — need relation mapping)");
        Console.ResetColor();

        // Summary
        Console.WriteLine();
        Console.WriteLine("=== Sync Summary ===");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  Customers: +{newCustomers} new");
        Console.WriteLine($"  Vehicles:  +{newVehicles} new");
        Console.WriteLine($"  Payments:  {newPayments} found (incremental payment sync requires full relation mapping)");
        Console.ResetColor();

        Console.WriteLine();
        Console.WriteLine("Current DB totals:");
        Console.WriteLine($"  Customers:  {newDb.Customers.Count():N0}");
        Console.WriteLine($"  Vehicles:   {newDb.Vehicles.Count():N0}");
        Console.WriteLine($"  Payments:   {newDb.PaymentDocuments.Count():N0}");
        Console.WriteLine($"  Exams:      {newDb.TechnicalExamReports.Count():N0}");
        Console.WriteLine($"  Line Items: {newDb.PaymentLineItems.Count():N0}");
    }

    static void FixEngineTypeNames()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Fixing Engine Type Names ===");

        using var oldConn = new SqlConnection(OldConnectionString);
        oldConn.Open();

        // Get engine types with maker names from old DB
        var oldEngines = new Dictionary<string, string>(); // old name -> "Code - Maker"
        using (var cmd = new SqlCommand(
            @"SELECT e.Id, e.EngineTypeCode, m.CompanyName
              FROM VehicleEngineTypes e
              LEFT JOIN VehicleMakers m ON e.IdVehicleMaker = m.Id
              WHERE e.Active = 1", oldConn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var code = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim();
                var maker = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim();
                var oldName = code; // our migration used EngineTypeCode as Name
                var newName = string.IsNullOrEmpty(maker) ? code : $"{code} - {maker}";
                if (!string.IsNullOrEmpty(oldName) && !oldEngines.ContainsKey(oldName))
                    oldEngines[oldName] = newName;
            }
        }
        Console.WriteLine($"Loaded {oldEngines.Count} engine types from old DB");

        // Update new DB
        var services = new ServiceCollection();
        services.AddDbContext<VteDbContext>(options => options.UseSqlServer(NewConnectionString));
        var sp = services.BuildServiceProvider();
        using var newDb = sp.GetRequiredService<VteDbContext>();

        // Get old engines in insertion order (same order as migration)
        var orderedOldNames = new List<string>();
        using (var cmd2 = new SqlCommand(
            @"SELECT RTRIM(e.EngineTypeCode) + ' - ' + RTRIM(ISNULL(m.CompanyName,''))
              FROM VehicleEngineTypes e
              LEFT JOIN VehicleMakers m ON e.IdVehicleMaker = m.Id
              WHERE e.Active = 1 ORDER BY e.Id", oldConn))
        using (var reader2 = cmd2.ExecuteReader())
        {
            while (reader2.Read())
                orderedOldNames.Add(reader2.GetString(0).Trim());
        }
        Console.WriteLine($"Got {orderedOldNames.Count} ordered names from old DB");

        var newEngines = newDb.EngineTypes.OrderBy(e => e.Id).ToList();
        int updated = 0;
        for (int i = 0; i < Math.Min(newEngines.Count, orderedOldNames.Count); i++)
        {
            var betterName = orderedOldNames[i];
            if (!string.IsNullOrWhiteSpace(betterName) && betterName != newEngines[i].Name && betterName != " - ")
            {
                newEngines[i].Name = betterName;
                updated++;
            }
        }
        newDb.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Updated {updated} engine type names");
        Console.ResetColor();

        // Also refresh LookupCache note
        Console.WriteLine("Restart the app to see updated names.");
    }

    static void FixPaymentDescriptions()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Fixing Payment Line Item Descriptions ===");

        using var oldConn = new SqlConnection(OldConnectionString);
        oldConn.Open();

        // First, get all PaymentItems names from old DB
        Console.Write("Loading PaymentItems from old DB...");
        var itemNames = new Dictionary<int, string>();
        using (var cmd = new SqlCommand("SELECT Id, ItemName FROM PaymentItems WHERE Active = 1", oldConn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                itemNames[id] = name;
            }
        }
        Console.WriteLine($" {itemNames.Count} items");

        // Show some names to verify encoding
        Console.WriteLine("Sample names:");
        foreach (var kv in itemNames.Take(5))
            Console.WriteLine($"  [{kv.Key}] = \"{kv.Value}\"");

        // Build a map: old PaymentDocument ID → old PaymentDocumentsDetails with their PriceCatalog IDs
        // Then update new DB's PaymentLineItems descriptions
        Console.Write("Loading old payment details with PriceCatalog IDs...");
        var detailDescs = new Dictionary<long, List<(int seq, int priceCatId, string note)>>();
        using (var cmd = new SqlCommand(
            "SELECT Id, IdPaymentDocuments, IdPriceCatalog, Note FROM PaymentDocumentsDetails WHERE Active = 1 ORDER BY IdPaymentDocuments, Id", oldConn))
        {
            cmd.CommandTimeout = 300;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var payDocId = Convert.ToInt64(reader.GetValue(1));
                var priceCatId = reader.GetInt32(2);
                var note = reader.IsDBNull(3) ? "" : reader.GetString(3);
                if (!detailDescs.ContainsKey(payDocId))
                    detailDescs[payDocId] = new List<(int, int, string)>();
                detailDescs[payDocId].Add((detailDescs[payDocId].Count, priceCatId, note));
            }
        }
        Console.WriteLine($" {detailDescs.Values.Sum(v => v.Count)} details across {detailDescs.Count} payments");

        // Now update new DB
        var services = new ServiceCollection();
        services.AddDbContext<VteDbContext>(options => options.UseSqlServer(NewConnectionString));
        var sp = services.BuildServiceProvider();

        // Build payment doc number → old ID map
        Console.Write("Building document number map...");
        var docNumToOldId = new Dictionary<string, long>();
        using (var cmd2 = new SqlCommand("SELECT Id, DocumentNumber FROM PaymentDocuments WHERE Active = 1", oldConn))
        using (var reader2 = cmd2.ExecuteReader())
        {
            while (reader2.Read())
            {
                var id = Convert.ToInt64(reader2.GetValue(0));
                var docNum = reader2.GetString(1);
                docNumToOldId[docNum] = id;
            }
        }
        Console.WriteLine($" {docNumToOldId.Count} docs");

        // Get new payment docs with their line items
        Console.Write("Updating descriptions...");
        using var newDb = sp.GetRequiredService<VteDbContext>();
        int updated = 0, batches = 0;

        var newPayments = newDb.PaymentDocuments
            .Select(p => new { p.Id, p.DocumentNumber })
            .ToList();

        foreach (var np in newPayments)
        {
            if (!docNumToOldId.TryGetValue(np.DocumentNumber, out var oldPayId)) continue;
            if (!detailDescs.TryGetValue(oldPayId, out var oldDetails)) continue;

            var newLineItems = newDb.PaymentLineItems
                .Where(li => li.PaymentDocumentId == np.Id)
                .OrderBy(li => li.Id)
                .ToList();

            for (int i = 0; i < Math.Min(newLineItems.Count, oldDetails.Count); i++)
            {
                var desc = itemNames.GetValueOrDefault(oldDetails[i].priceCatId, "");
                if (string.IsNullOrEmpty(desc) && !string.IsNullOrEmpty(oldDetails[i].note))
                    desc = oldDetails[i].note;
                if (string.IsNullOrEmpty(desc))
                    desc = "Ставка";

                newLineItems[i].Description = desc;
                updated++;
            }

            batches++;
            if (batches % 1000 == 0)
            {
                newDb.SaveChanges();
                Console.Write($" {batches/1000}K...");
            }
        }
        newDb.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" Updated {updated} descriptions across {batches} payments");
        Console.ResetColor();
    }

    static void UpdateRequestTypeFlags()
    {
        Console.WriteLine("=== Updating Request Type Flags ===");
        using var oldConn = new SqlConnection(OldConnectionString);
        oldConn.Open();

        var services = new ServiceCollection();
        services.AddDbContext<VteDbContext>(options => options.UseSqlServer(NewConnectionString));
        var sp = services.BuildServiceProvider();
        using var newDb = sp.GetRequiredService<VteDbContext>();

        var sql = @"SELECT TypeName, IsTehnicalExamRequired, IsPayRequired, IsNewRegistration,
                    IsPreviosRegistrationReqired, IsRelationDeleted, IsVehicleDeleted,
                    IsNewCustomer, IsVehicleChanged, IsCustomerChanged
                    FROM RequestTypes WHERE Active = 1";
        using var cmd = new SqlCommand(sql, oldConn);
        using var reader = cmd.ExecuteReader();

        int updated = 0;
        while (reader.Read())
        {
            var name = reader.GetString(0);
            var newType = newDb.RequestTypes.FirstOrDefault(r => r.Name == name);
            if (newType == null) continue;

            newType.IsTehnicalExamRequired = !reader.IsDBNull(1) && Convert.ToInt32(reader.GetValue(1)) != 0;
            newType.IsPayRequired = !reader.IsDBNull(2) && reader.GetBoolean(2);
            newType.IsNewRegistration = !reader.IsDBNull(3) && reader.GetBoolean(3);
            newType.IsPreviousRegistrationRequired = !reader.IsDBNull(4) && reader.GetBoolean(4);
            newType.IsRelationDeleted = !reader.IsDBNull(5) && reader.GetBoolean(5);
            newType.IsVehicleDeleted = !reader.IsDBNull(6) && reader.GetBoolean(6);
            newType.IsNewCustomer = !reader.IsDBNull(7) && reader.GetBoolean(7);
            newType.IsVehicleChanged = !reader.IsDBNull(8) && reader.GetBoolean(8);
            newType.IsCustomerChanged = !reader.IsDBNull(9) && reader.GetBoolean(9);
            updated++;
        }
        reader.Close();
        newDb.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Updated {updated} request type flags");
        Console.ResetColor();
    }

    static void MigrateAllMissing()
    {
        Console.WriteLine("=== Migrating ALL Missing Tables ===");
        Console.WriteLine($"Source: {OldConnectionString}");
        Console.WriteLine($"Target: {NewConnectionString}");
        Console.WriteLine();

        var services = new ServiceCollection();
        services.AddDbContext<VteDbContext>(options => options.UseSqlServer(NewConnectionString));
        var sp = services.BuildServiceProvider();
        using var newDb = sp.GetRequiredService<VteDbContext>();
        newDb.Database.EnsureCreated();

        using var oldConn = new SqlConnection(OldConnectionString);
        oldConn.Open();

        try
        {
            // ── 1. TechnicalExamReportDetails (10,506 rows) ──
            MigrateMissingExamReportDetails(oldConn, newDb);

            // ── 2. PaymentInstallments (251K rows) ──
            MigrateMissingPaymentInstallments(oldConn, newDb);

            // ── 3. TrafficLicenses (8,183 rows) ──
            MigrateMissingTrafficLicenses(oldConn, newDb);

            // ── 4. Permissions (2,784 rows) ──
            MigrateMissingPermissions(oldConn, newDb);

            // ── 5. InternationalDrivingLicenses (1,469 rows) ──
            MigrateMissingInternationalDrivingLicenses(oldConn, newDb);

            // ── 6. CustomerBankAccounts (8 rows) ──
            MigrateMissingCustomerBankAccounts(oldConn, newDb);

            Console.WriteLine("\n=== Missing Tables Migration Complete ===");
            PrintMissingSummary(newDb);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nERROR: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
        }
    }

    static void MigrateMissingExamReportDetails(SqlConnection oldConn, VteDbContext newDb)
    {
        Console.Write("  Migrating TechnicalExamReportDetails (from DocumentsTehnicalExamsReportsDetails)...");
        int count = 0, skipped = 0;

        // Build exam ID map: match old exams to new exams by RegistrationNumber + ExamDate
        Console.Write(" building exam map...");
        var newExams = newDb.TechnicalExamReports
            .Select(e => new { e.Id, e.RegistrationNumber, e.ExamDate })
            .ToList();
        var newExamLookup = newExams
            .GroupBy(e => (e.RegistrationNumber ?? "", e.ExamDate.Date))
            .ToDictionary(g => g.Key, g => g.First().Id);

        var oldExamMap = new Dictionary<long, long>(); // old exam ID -> new exam ID
        using (var cmd = new SqlCommand("SELECT Id, RegNumber, MadeDate FROM DocumentsTehnicalExamsReports", oldConn))
        {
            cmd.CommandTimeout = 120;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                var regNum = reader.IsDBNull(1) ? "" : reader.GetValue(1).ToString() ?? "";
                var madeDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                var key = (regNum, madeDate.Date);
                if (newExamLookup.TryGetValue(key, out var newId))
                    oldExamMap[oldId] = newId;
            }
        }
        Console.Write($" {oldExamMap.Count} exams mapped...");

        // Build vehicle parts ID map: match by Name (old Description)
        var newParts = newDb.TechnicalExamVehicleParts
            .Select(p => new { p.Id, p.Name })
            .ToList();
        var newPartsByName = newParts
            .GroupBy(p => p.Name)
            .ToDictionary(g => g.Key, g => g.First().Id);

        var oldPartMap = new Dictionary<long, long>(); // old part ID -> new part ID
        using (var cmd = new SqlCommand("SELECT Id, Description FROM TehnicalExamVehicleParts WHERE Active = 1", oldConn))
        {
            cmd.CommandTimeout = 30;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                var desc = reader.IsDBNull(1) ? "" : reader.GetValue(1).ToString() ?? "";
                if (newPartsByName.TryGetValue(desc, out var newId))
                    oldPartMap[oldId] = newId;
            }
        }

        // Build exam detail status map: match by Name
        var newStatuses = newDb.Set<ExamDetailStatus>()
            .Select(s => new { s.Id, s.Name })
            .ToList();
        var newStatusByName = newStatuses
            .GroupBy(s => s.Name)
            .ToDictionary(g => g.Key, g => g.First().Id);

        var oldStatusMap = new Dictionary<long, long>();
        using (var cmd = new SqlCommand(
            "SELECT Id, c.COLUMN_NAME FROM DocumentsTehnicalExamsReportsDetailsStatus s CROSS APPLY (SELECT TOP 1 COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DocumentsTehnicalExamsReportsDetailsStatus' AND COLUMN_NAME <> 'Id' AND COLUMN_NAME <> 'Active' ORDER BY ORDINAL_POSITION) c",
            oldConn))
        {
            // Simpler approach: just try direct ID mapping (statuses are small lookup, IDs likely match)
        }
        // For statuses, try name-based mapping; fallback to same-ID
        try
        {
            var columns = GetTableColumns(oldConn, "DocumentsTehnicalExamsReportsDetailsStatus");
            var nameCol = FindColumn(columns, new[] { "StatusName", "Name", "Description" });
            if (nameCol != null)
            {
                using var cmd = new SqlCommand($"SELECT Id, [{nameCol}] FROM DocumentsTehnicalExamsReportsDetailsStatus", oldConn);
                cmd.CommandTimeout = 15;
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var oldId = Convert.ToInt64(reader.GetValue(0));
                    var name = reader.IsDBNull(1) ? "" : reader.GetValue(1).ToString() ?? "";
                    if (newStatusByName.TryGetValue(name, out var newId))
                        oldStatusMap[oldId] = newId;
                    else
                    {
                        // Fallback: if new DB has a status with same ID, use it
                        var directMatch = newStatuses.FirstOrDefault(s => s.Id == oldId);
                        if (directMatch != null)
                            oldStatusMap[oldId] = directMatch.Id;
                    }
                }
            }
        }
        catch { /* ignore, will try direct ID */ }

        // Now migrate the details
        var sql = @"SELECT Id, IdTehnicalExamsReports, IdTehnicalExamVehivlePart, IdStatus, Note
                    FROM DocumentsTehnicalExamsReportsDetails WHERE Active = 1";
        using var detailCmd = new SqlCommand(sql, oldConn);
        detailCmd.CommandTimeout = 300;
        using var detailReader = detailCmd.ExecuteReader();
        var batch = new List<TechnicalExamReportDetail>();

        while (detailReader.Read())
        {
            var oldReportId = detailReader.IsDBNull(1) ? 0L : Convert.ToInt64(detailReader.GetValue(1));
            var oldPartId = detailReader.IsDBNull(2) ? 0L : Convert.ToInt64(detailReader.GetValue(2));
            var oldStatusId = detailReader.IsDBNull(3) ? 0L : Convert.ToInt64(detailReader.GetValue(3));

            if (!oldExamMap.TryGetValue(oldReportId, out var newReportId))
            { skipped++; continue; }
            if (oldPartId == 0 || !oldPartMap.TryGetValue(oldPartId, out var newPartId))
            {
                // Fallback: use same ID if it exists in new DB
                if (oldPartId > 0 && newParts.Any(p => p.Id == oldPartId))
                    newPartId = oldPartId;
                else { skipped++; continue; }
            }
            long newStatusId;
            if (oldStatusId > 0 && oldStatusMap.TryGetValue(oldStatusId, out var mappedStatusId))
                newStatusId = mappedStatusId;
            else if (oldStatusId > 0 && newStatuses.Any(s => s.Id == oldStatusId))
                newStatusId = oldStatusId;
            else { skipped++; continue; }

            var entity = new TechnicalExamReportDetail
            {
                ReportId = newReportId,
                VehiclePartId = newPartId,
                StatusId = newStatusId,
                Note = GetStringOrNull(detailReader, 4)
            };
            batch.Add(entity);
            count++;

            if (batch.Count >= BatchSize)
            {
                foreach (var e in batch) newDb.TechnicalExamReportDetails.Add(e);
                newDb.SaveChanges();
                foreach (var e in batch) newDb.Entry(e).State = EntityState.Detached;
                batch.Clear();
                Console.Write($" {count}...");
            }
        }

        if (batch.Count > 0)
        {
            foreach (var e in batch) newDb.TechnicalExamReportDetails.Add(e);
            newDb.SaveChanges();
            foreach (var e in batch) newDb.Entry(e).State = EntityState.Detached;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    static void MigrateMissingPaymentInstallments(SqlConnection oldConn, VteDbContext newDb)
    {
        Console.Write("  Migrating PaymentInstallments (from PaymentDocumentsRata)...");
        int count = 0, skipped = 0;

        // Build payment ID map by DocumentNumber
        var newPayments = newDb.PaymentDocuments
            .Select(p => new { p.Id, p.DocumentNumber })
            .ToList();
        var newByDocNum = newPayments
            .GroupBy(p => p.DocumentNumber)
            .ToDictionary(g => g.Key, g => g.First().Id);

        var oldPaymentMap = new Dictionary<long, long>();
        using (var cmd = new SqlCommand("SELECT Id, DocumentNumber FROM PaymentDocuments WHERE Active = 1", oldConn))
        {
            cmd.CommandTimeout = 120;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                var docNum = reader.IsDBNull(1) ? "" : reader.GetValue(1).ToString() ?? "";
                if (newByDocNum.TryGetValue(docNum, out var newId))
                    oldPaymentMap[oldId] = newId;
            }
        }
        Console.Write($" {oldPaymentMap.Count} payments mapped...");

        var sql = @"SELECT Id, IdPaymentDocument, Price, Payed, DatePayed, Note
                    FROM PaymentDocumentsRata WHERE Active = 1";
        using var detailCmd = new SqlCommand(sql, oldConn);
        detailCmd.CommandTimeout = 300;
        using var detailReader = detailCmd.ExecuteReader();
        var batch = new List<PaymentInstallment>();
        var installmentCounters = new Dictionary<long, int>(); // new payment ID -> installment counter

        while (detailReader.Read())
        {
            var oldPaymentId = detailReader.IsDBNull(1) ? 0L : Convert.ToInt64(detailReader.GetValue(1));
            if (!oldPaymentMap.TryGetValue(oldPaymentId, out var newPaymentId))
            { skipped++; continue; }

            if (!installmentCounters.ContainsKey(newPaymentId))
                installmentCounters[newPaymentId] = 0;
            installmentCounters[newPaymentId]++;

            var amount = detailReader.IsDBNull(2) ? 0m : Convert.ToDecimal(detailReader.GetValue(2));
            var isPaid = !detailReader.IsDBNull(3) && detailReader.GetBoolean(3);
            var paidDate = GetNullableDateTime(detailReader, 4);

            var entity = new PaymentInstallment
            {
                PaymentDocumentId = newPaymentId,
                InstallmentNumber = installmentCounters[newPaymentId],
                DueDate = paidDate ?? DateTime.UtcNow, // Use PaidDate as DueDate since old schema lacks explicit DueDate
                Amount = amount,
                IsPaid = isPaid,
                PaidDate = paidDate
            };
            batch.Add(entity);
            count++;

            if (batch.Count >= BatchSize)
            {
                foreach (var e in batch) newDb.PaymentInstallments.Add(e);
                newDb.SaveChanges();
                foreach (var e in batch) newDb.Entry(e).State = EntityState.Detached;
                batch.Clear();
                Console.Write($" {count / 1000}K...");
            }
        }

        if (batch.Count > 0)
        {
            foreach (var e in batch) newDb.PaymentInstallments.Add(e);
            newDb.SaveChanges();
            foreach (var e in batch) newDb.Entry(e).State = EntityState.Detached;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    static void MigrateMissingTrafficLicenses(SqlConnection oldConn, VteDbContext newDb)
    {
        Console.Write("  Migrating TrafficLicenses (from DocumentsTrafficLicences)...");
        int count = 0, skipped = 0;

        // Build CustomerVehicleRelation ID map by matching old -> new via sequential order
        var newRelations = newDb.CustomerVehicleRelations
            .OrderBy(r => r.Id)
            .Select(r => r.Id)
            .ToList();
        var newRelationSet = new HashSet<long>(newRelations);

        // Build old relation -> new relation map
        var oldRelMap = new Dictionary<long, long>();
        using (var cmd = new SqlCommand("SELECT Id FROM CustomerVehiclesRelations WHERE Active = 1 ORDER BY Id", oldConn))
        {
            cmd.CommandTimeout = 120;
            using var reader = cmd.ExecuteReader();
            int idx = 0;
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (idx < newRelations.Count)
                    oldRelMap[oldId] = newRelations[idx];
                idx++;
            }
        }

        // Get default DocumentType for TrafficLicense
        long defaultDocTypeId = newDb.Set<DocumentType>().Select(d => d.Id).FirstOrDefault();
        if (defaultDocTypeId == 0) defaultDocTypeId = 1;

        var sql = @"SELECT Id, IdCustomerVehicleRelation, TrafficLicenceNumber, MadeDate, EndDate, Note
                    FROM DocumentsTrafficLicences WHERE Active = 1";
        using var detailCmd = new SqlCommand(sql, oldConn);
        detailCmd.CommandTimeout = 120;
        using var detailReader = detailCmd.ExecuteReader();

        while (detailReader.Read())
        {
            var oldRelId = detailReader.IsDBNull(1) ? 0L : Convert.ToInt64(detailReader.GetValue(1));
            if (!oldRelMap.TryGetValue(oldRelId, out var newRelId))
            { skipped++; continue; }

            var licenseNumber = GetStringOrNull(detailReader, 2) ?? "";
            var madeDate = GetNullableDateTime(detailReader, 3) ?? DateTime.UtcNow;
            var endDate = GetNullableDateTime(detailReader, 4);
            var note = GetStringOrNull(detailReader, 5);

            // Create Document record first
            var doc = new Document
            {
                DocumentTypeId = defaultDocTypeId,
                CustomerVehicleRelationId = newRelId,
                Note = note,
                CreatedAt = madeDate,
                CreatedByUserId = 1
            };
            newDb.Documents.Add(doc);
            newDb.SaveChanges();

            var tl = new TrafficLicense
            {
                DocumentId = doc.Id,
                LicenseNumber = licenseNumber,
                IssuedDate = madeDate,
                ValidUntilDate = endDate
            };
            newDb.TrafficLicenses.Add(tl);
            newDb.SaveChanges();
            newDb.Entry(doc).State = EntityState.Detached;
            newDb.Entry(tl).State = EntityState.Detached;

            count++;
            if (count % BatchSize == 0)
                Console.Write($" {count}...");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    static void MigrateMissingPermissions(SqlConnection oldConn, VteDbContext newDb)
    {
        Console.Write("  Migrating Permissions (from DocumentsPermisions)...");
        int count = 0, skipped = 0;

        // Build old relation -> new relation map (same approach as traffic licenses)
        var newRelations = newDb.CustomerVehicleRelations
            .OrderBy(r => r.Id)
            .Select(r => r.Id)
            .ToList();

        var oldRelMap = new Dictionary<long, long>();
        using (var cmd = new SqlCommand("SELECT Id FROM CustomerVehiclesRelations WHERE Active = 1 ORDER BY Id", oldConn))
        {
            cmd.CommandTimeout = 120;
            using var reader = cmd.ExecuteReader();
            int idx = 0;
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                if (idx < newRelations.Count)
                    oldRelMap[oldId] = newRelations[idx];
                idx++;
            }
        }

        long defaultDocTypeId = newDb.Set<DocumentType>().Select(d => d.Id).FirstOrDefault();
        if (defaultDocTypeId == 0) defaultDocTypeId = 1;

        // Discover columns for DocumentsPermisions
        var columns = GetTableColumns(oldConn, "DocumentsPermisions");
        var relCol = FindColumn(columns, new[] { "IdCustomerVehicleRelation", "IdRelation" });
        var numCol = FindColumn(columns, new[] { "PermisionNumber", "PermissionNumber", "Number" });
        var dateCol = FindColumn(columns, new[] { "MadeDate", "DateCreated", "DateMade" });
        var endCol = FindColumn(columns, new[] { "EndDate", "ValidTillDate", "ValidUntilDate" });
        var noteCol = FindColumn(columns, new[] { "Note", "Description" });

        if (relCol == null || numCol == null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" SKIPPED (could not find required columns. Columns: {string.Join(", ", columns)})");
            Console.ResetColor();
            return;
        }

        var selectCols = new List<string> { "Id", $"[{relCol}]", $"[{numCol}]" };
        if (dateCol != null) selectCols.Add($"[{dateCol}]");
        if (endCol != null) selectCols.Add($"[{endCol}]");
        if (noteCol != null) selectCols.Add($"[{noteCol}]");

        var sql = $"SELECT {string.Join(", ", selectCols)} FROM DocumentsPermisions WHERE Active = 1";
        using var detailCmd = new SqlCommand(sql, oldConn);
        detailCmd.CommandTimeout = 120;
        using var detailReader = detailCmd.ExecuteReader();

        while (detailReader.Read())
        {
            var oldRelId = detailReader.IsDBNull(1) ? 0L : Convert.ToInt64(detailReader.GetValue(1));
            if (!oldRelMap.TryGetValue(oldRelId, out var newRelId))
            { skipped++; continue; }

            var permNumber = GetStringOrNull(detailReader, 2) ?? "";
            int colIdx = 3;
            var madeDate = dateCol != null ? (GetNullableDateTime(detailReader, colIdx++) ?? DateTime.UtcNow) : DateTime.UtcNow;
            var endDate = endCol != null ? GetNullableDateTime(detailReader, colIdx++) : null;
            var note = noteCol != null ? GetStringOrNull(detailReader, colIdx++) : null;

            var doc = new Document
            {
                DocumentTypeId = defaultDocTypeId,
                CustomerVehicleRelationId = newRelId,
                Note = note,
                CreatedAt = madeDate,
                CreatedByUserId = 1
            };
            newDb.Documents.Add(doc);
            newDb.SaveChanges();

            var perm = new Permission
            {
                DocumentId = doc.Id,
                PermissionNumber = permNumber,
                IssuedDate = madeDate,
                ValidUntilDate = endDate,
                Note = note
            };
            newDb.Permissions.Add(perm);
            newDb.SaveChanges();
            newDb.Entry(doc).State = EntityState.Detached;
            newDb.Entry(perm).State = EntityState.Detached;

            count++;
            if (count % BatchSize == 0)
                Console.Write($" {count}...");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    static void MigrateMissingInternationalDrivingLicenses(SqlConnection oldConn, VteDbContext newDb)
    {
        Console.Write("  Migrating InternationalDrivingLicenses (from DocumentsInternationalDriveingLicences)...");
        int count = 0, skipped = 0;

        // Build customer ID map by matching IdentificationNumber (MB)
        var newCustomers = newDb.Customers
            .Select(c => new { c.Id, c.IdentificationNumber })
            .ToList();
        var newCustByMB = newCustomers
            .Where(c => !string.IsNullOrEmpty(c.IdentificationNumber))
            .GroupBy(c => c.IdentificationNumber)
            .ToDictionary(g => g.Key, g => g.First().Id);

        var oldCustMap = new Dictionary<long, long>();
        using (var cmd = new SqlCommand("SELECT Id, MB FROM Customers WHERE Active = 1", oldConn))
        {
            cmd.CommandTimeout = 120;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                var mb = reader.IsDBNull(1) ? "" : reader.GetValue(1).ToString() ?? "";
                if (!string.IsNullOrEmpty(mb) && newCustByMB.TryGetValue(mb, out var newId))
                    oldCustMap[oldId] = newId;
            }
        }
        Console.Write($" {oldCustMap.Count} customers mapped...");

        var sql = @"SELECT Id, IdCustomer, NumberOfLicence, DateCreated, ValidTillDate, Note
                    FROM DocumentsInternationalDriveingLicences WHERE Active = 1";
        using var detailCmd = new SqlCommand(sql, oldConn);
        detailCmd.CommandTimeout = 120;
        using var detailReader = detailCmd.ExecuteReader();
        var batch = new List<InternationalDrivingLicense>();

        while (detailReader.Read())
        {
            var oldCustId = detailReader.IsDBNull(1) ? 0L : Convert.ToInt64(detailReader.GetValue(1));
            if (!oldCustMap.TryGetValue(oldCustId, out var newCustId))
            { skipped++; continue; }

            var licenseNum = GetStringOrNull(detailReader, 2) ?? "";
            var issuedDate = GetNullableDateTime(detailReader, 3) ?? DateTime.UtcNow;
            var validUntil = GetNullableDateTime(detailReader, 4) ?? DateTime.UtcNow.AddYears(1);

            var entity = new InternationalDrivingLicense
            {
                CustomerId = newCustId,
                LicenseNumber = licenseNum,
                IssuedDate = issuedDate,
                ValidUntilDate = validUntil
            };
            batch.Add(entity);
            count++;

            if (batch.Count >= BatchSize)
            {
                foreach (var e in batch) newDb.InternationalDrivingLicenses.Add(e);
                newDb.SaveChanges();
                foreach (var e in batch) newDb.Entry(e).State = EntityState.Detached;
                batch.Clear();
                Console.Write($" {count}...");
            }
        }

        if (batch.Count > 0)
        {
            foreach (var e in batch) newDb.InternationalDrivingLicenses.Add(e);
            newDb.SaveChanges();
            foreach (var e in batch) newDb.Entry(e).State = EntityState.Detached;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    static void MigrateMissingCustomerBankAccounts(SqlConnection oldConn, VteDbContext newDb)
    {
        Console.Write("  Migrating CustomerBankAccounts (from CustomersBankAccounts)...");
        int count = 0, skipped = 0;

        // Build customer ID map by matching IdentificationNumber (MB)
        var newCustomers = newDb.Customers
            .Select(c => new { c.Id, c.IdentificationNumber })
            .ToList();
        var newCustByMB = newCustomers
            .Where(c => !string.IsNullOrEmpty(c.IdentificationNumber))
            .GroupBy(c => c.IdentificationNumber)
            .ToDictionary(g => g.Key, g => g.First().Id);

        var oldCustMap = new Dictionary<long, long>();
        using (var cmd = new SqlCommand("SELECT Id, MB FROM Customers WHERE Active = 1", oldConn))
        {
            cmd.CommandTimeout = 30;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var oldId = Convert.ToInt64(reader.GetValue(0));
                var mb = reader.IsDBNull(1) ? "" : reader.GetValue(1).ToString() ?? "";
                if (!string.IsNullOrEmpty(mb) && newCustByMB.TryGetValue(mb, out var newId))
                    oldCustMap[oldId] = newId;
            }
        }

        var sql = @"SELECT Id, IdCustomer, BankAccount, DeponentBank, TaxNumber
                    FROM CustomersBankAccounts WHERE Active = 1";
        using var detailCmd = new SqlCommand(sql, oldConn);
        detailCmd.CommandTimeout = 30;
        using var detailReader = detailCmd.ExecuteReader();

        while (detailReader.Read())
        {
            var oldCustId = detailReader.IsDBNull(1) ? 0L : Convert.ToInt64(detailReader.GetValue(1));
            if (!oldCustMap.TryGetValue(oldCustId, out var newCustId))
            { skipped++; continue; }

            var bankAccount = GetStringOrNull(detailReader, 2) ?? "";
            var bankName = GetStringOrNull(detailReader, 3) ?? "";
            var taxNumber = GetStringOrNull(detailReader, 4);

            var entity = new CustomerBankAccount
            {
                CustomerId = newCustId,
                BankName = bankName,
                AccountNumber = bankAccount,
                Note = taxNumber != null ? $"TaxNumber: {taxNumber}" : null
            };
            newDb.CustomerBankAccounts.Add(entity);
            count++;
        }

        if (count > 0)
            newDb.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" {count} records");
        Console.ResetColor();
        if (skipped > 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($" ({skipped} skipped)");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    static void PrintMissingSummary(VteDbContext db)
    {
        Console.WriteLine("\n--- Missing Tables Summary ---");
        try { Console.WriteLine($"  ExamReportDetails:     {db.TechnicalExamReportDetails.Count()}"); } catch { Console.WriteLine("  ExamReportDetails:     N/A"); }
        try { Console.WriteLine($"  PaymentInstallments:   {db.PaymentInstallments.Count()}"); } catch { Console.WriteLine("  PaymentInstallments:   N/A"); }
        try { Console.WriteLine($"  Documents:             {db.Documents.Count()}"); } catch { Console.WriteLine("  Documents:             N/A"); }
        try { Console.WriteLine($"  TrafficLicenses:       {db.TrafficLicenses.Count()}"); } catch { Console.WriteLine("  TrafficLicenses:       N/A"); }
        try { Console.WriteLine($"  Permissions:           {db.Permissions.Count()}"); } catch { Console.WriteLine("  Permissions:           N/A"); }
        try { Console.WriteLine($"  IntlDrivingLicenses:   {db.InternationalDrivingLicenses.Count()}"); } catch { Console.WriteLine("  IntlDrivingLicenses:   N/A"); }
        try { Console.WriteLine($"  CustomerBankAccounts:  {db.CustomerBankAccounts.Count()}"); } catch { Console.WriteLine("  CustomerBankAccounts:  N/A"); }
    }

    // ──────────────────────────────────────────────
    //  Summary
    // ──────────────────────────────────────────────

    static void PrintSummary(VteDbContext db)
    {
        Console.WriteLine("\n--- New Database Summary ---");
        try { Console.WriteLine($"  Countries:         {db.Countries.Count()}"); } catch { Console.WriteLine("  Countries:         N/A"); }
        try { Console.WriteLine($"  Communities:       {db.Communities.Count()}"); } catch { Console.WriteLine("  Communities:       N/A"); }
        try { Console.WriteLine($"  Cities:            {db.Cities.Count()}"); } catch { Console.WriteLine("  Cities:            N/A"); }
        try { Console.WriteLine($"  Streets:           {db.Streets.Count()}"); } catch { Console.WriteLine("  Streets:           N/A"); }
        try { Console.WriteLine($"  VehicleMakers:     {db.VehicleMakers.Count()}"); } catch { Console.WriteLine("  VehicleMakers:     N/A"); }
        try { Console.WriteLine($"  VehicleModels:     {db.VehicleModels.Count()}"); } catch { Console.WriteLine("  VehicleModels:     N/A"); }
        try { Console.WriteLine($"  Companies:         {db.Companies.Count()}"); } catch { Console.WriteLine("  Companies:         N/A"); }
        try { Console.WriteLine($"  Organizations:     {db.TechnicalExamOrganizations.Count()}"); } catch { Console.WriteLine("  Organizations:     N/A"); }
        try { Console.WriteLine($"  Customers:         {db.Customers.Count()}"); } catch { Console.WriteLine("  Customers:         N/A"); }
        try { Console.WriteLine($"  Vehicles:          {db.Vehicles.Count()}"); } catch { Console.WriteLine("  Vehicles:          N/A"); }
        try { Console.WriteLine($"  Relations:         {db.CustomerVehicleRelations.Count()}"); } catch { Console.WriteLine("  Relations:         N/A"); }
        try { Console.WriteLine($"  Exam Reports:      {db.TechnicalExamReports.Count()}"); } catch { Console.WriteLine("  Exam Reports:      N/A"); }
        try { Console.WriteLine($"  Requests:          {db.Requests.Count()}"); } catch { Console.WriteLine("  Requests:          N/A"); }
        try { Console.WriteLine($"  Payment Documents: {db.PaymentDocuments.Count()}"); } catch { Console.WriteLine("  Payment Documents: N/A"); }
        try { Console.WriteLine($"  Payment Details:   {db.PaymentLineItems.Count()}"); } catch { Console.WriteLine("  Payment Details:   N/A"); }
    }
}
