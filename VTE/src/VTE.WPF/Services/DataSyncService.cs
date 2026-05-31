namespace VTE.WPF.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;

public static class DataSyncService
{
    private const string OldConnectionString = "Server=195.26.159.162,7899;Database=VTEZVV;User Id=testapp1;Password=2_Snegot;TrustServerCertificate=true;Connect Timeout=15;";

    public static async Task<SyncResult> SyncAsync(Action<string>? onProgress = null)
    {
        var result = new SyncResult();

        await Task.Run(() =>
        {
            try
            {
                using var oldConn = new SqlConnection(OldConnectionString);
                oldConn.Open();
                onProgress?.Invoke("Поврзано со старата база");

                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                // Get existing keys
                var existingMBs = new HashSet<string>(db.Customers.Select(c => c.IdentificationNumber).ToList());
                var existingShells = new HashSet<string>(db.Vehicles.Select(v => v.ShellNumber).ToList());

                onProgress?.Invoke($"Постоечки: {existingMBs.Count:N0} клиенти, {existingShells.Count:N0} возила");

                // Sync Customers
                onProgress?.Invoke("Синхронизација на клиенти...");
                using (var cmd = new SqlCommand(
                    @"SELECT MB, CustomerSurname, CustomerFirstName, ParentName,
                      DateOfBirth, IsCompany, PhoneNumber, Fax, eMail,
                      TaxNumber, PassportNumber, DriveingLicenceNumber, BLK,
                      CanSendNotifications, Note, Occupation
                      FROM Customers WHERE Active = 1", oldConn))
                {
                    cmd.CommandTimeout = 300;
                    using var reader = cmd.ExecuteReader();
                    int batch = 0;
                    while (reader.Read())
                    {
                        var mb = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();
                        if (existingMBs.Contains(mb)) continue;
                        existingMBs.Add(mb);

                        db.Customers.Add(new Customer
                        {
                            IdentificationNumber = mb,
                            LastName = reader.IsDBNull(1) ? "" : reader.GetString(1).Trim(),
                            FirstName = reader.IsDBNull(2) ? "" : reader.GetString(2).Trim(),
                            ParentName = reader.IsDBNull(3) ? null : reader.GetString(3).Trim(),
                            DateOfBirth = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                            IsCompany = !reader.IsDBNull(5) && reader.GetBoolean(5),
                            PhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6).Trim(),
                            Fax = reader.IsDBNull(7) ? null : reader.GetString(7).Trim(),
                            Email = reader.IsDBNull(8) ? null : reader.GetString(8).Trim(),
                            TaxNumber = reader.IsDBNull(9) ? null : reader.GetString(9).Trim(),
                            PassportNumber = reader.IsDBNull(10) ? null : reader.GetString(10).Trim(),
                            DrivingLicenseNumber = reader.IsDBNull(11) ? null : reader.GetString(11).Trim(),
                            IdentityCardNumber = reader.IsDBNull(12) ? null : reader.GetString(12).Trim(),
                            CanSendNotifications = !reader.IsDBNull(13) && reader.GetBoolean(13),
                            Note = reader.IsDBNull(14) ? null : reader.GetString(14).Trim(),
                            Occupation = reader.IsDBNull(15) ? null : reader.GetString(15).Trim(),
                            CreatedAt = DateTime.UtcNow,
                            CreatedByUserId = 1
                        });
                        result.NewCustomers++;
                        batch++;
                        if (batch % 100 == 0) db.SaveChanges();
                    }
                }
                if (result.NewCustomers > 0) db.SaveChanges();
                onProgress?.Invoke($"Клиенти: +{result.NewCustomers} нови");

                // Sync Vehicles
                onProgress?.Invoke("Синхронизација на возила...");
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
                    int batch = 0;
                    while (reader.Read())
                    {
                        var shell = reader.IsDBNull(0) ? "" : reader.GetString(0).Trim();
                        if (string.IsNullOrEmpty(shell) || existingShells.Contains(shell)) continue;
                        existingShells.Add(shell);

                        db.Vehicles.Add(new Vehicle
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
                        });
                        result.NewVehicles++;
                        batch++;
                        if (batch % 100 == 0) db.SaveChanges();
                    }
                }
                if (result.NewVehicles > 0) db.SaveChanges();
                onProgress?.Invoke($"Возила: +{result.NewVehicles} нови");

                result.TotalCustomers = db.Customers.Count();
                result.TotalVehicles = db.Vehicles.Count();
                result.Success = true;
                onProgress?.Invoke("Синхронизацијата заврши успешно!");
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                onProgress?.Invoke($"Грешка: {ex.Message}");
            }
        });

        // Refresh lookup cache
        if (result.Success && (result.NewCustomers > 0 || result.NewVehicles > 0))
        {
            try { LookupCache.Load(); } catch { }
        }

        return result;
    }
}

public class SyncResult
{
    public bool Success { get; set; }
    public int NewCustomers { get; set; }
    public int NewVehicles { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalVehicles { get; set; }
    public string? Error { get; set; }
}
