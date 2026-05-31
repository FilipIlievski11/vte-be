namespace VTE.WPF.Services;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Infrastructure.Data;
using VTE.WPF.Controls;

/// <summary>
/// Global lookup cache — loads all lookup data once at startup, reused by all forms.
/// </summary>
public static class LookupCache
{
    public static List<LookupItem> VehicleModels { get; private set; } = [];
    public static List<LookupItem> VehicleCategories { get; private set; } = [];
    public static List<LookupItem> VehiclePaymentCategories { get; private set; } = [];
    public static List<LookupItem> VehicleUseTypes { get; private set; } = [];
    public static List<LookupItem> VehicleBodyTypes { get; private set; } = [];
    public static List<LookupItem> EngineTypes { get; private set; } = [];
    public static List<LookupItem> Colors { get; private set; } = [];
    public static List<LookupItem> BrakeTypes { get; private set; } = [];
    public static List<LookupItem> GearBoxTypes { get; private set; } = [];
    public static List<LookupItem> SupportingTypes { get; private set; } = [];
    public static List<LookupItem> EcoPrograms { get; private set; } = [];
    public static List<LookupItem> EnginePowerSourceTypes { get; private set; } = [];
    public static List<LookupItem> TireTypes { get; private set; } = [];
    public static List<LookupItem> VehicleMakers { get; private set; } = [];
    public static List<LookupItem> RegistrationIssuers { get; private set; } = [];

    public static List<LookupItem> Countries { get; private set; } = [];
    public static List<LookupItem> Cities { get; private set; } = [];
    public static List<LookupItem> Communities { get; private set; } = [];
    public static List<LookupItem> Streets { get; private set; } = [];
    public static List<LookupItem> BusinessTypes { get; private set; } = [];

    public static List<LookupItem> DocumentTypes { get; private set; } = [];
    public static List<LookupItem> PaymentTypes { get; private set; } = [];
    public static List<LookupItem> PaymentCategories { get; private set; } = [];
    public static List<LookupItem> RequestTypes { get; private set; } = [];
    public static List<LookupItem> RelationTypes { get; private set; } = [];
    public static List<LookupItem> AttachmentTypes { get; private set; } = [];
    public static List<LookupItem> DrivingLicenseCategories { get; private set; } = [];

    public static List<LookupItem> TechnicalExamTypes { get; private set; } = [];
    public static List<LookupItem> ExamDetailStatuses { get; private set; } = [];
    public static List<LookupItem> Organizations { get; private set; } = [];
    public static List<LookupItem> Users { get; private set; } = [];
    public static List<LookupItem> Roles { get; private set; } = [];
    public static List<LookupItem> Customers { get; private set; } = [];
    public static List<LookupItem> Vehicles { get; private set; } = [];

    public static bool IsLoaded { get; private set; }

    public static void Load()
    {
        using var scope = App.Services!.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

        // Vehicle lookups
        VehicleModels = db.VehicleModels.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        VehicleCategories = db.VehicleCategories.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        VehiclePaymentCategories = db.VehiclePaymentCategories.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        VehicleUseTypes = db.VehicleUseTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        VehicleBodyTypes = db.VehicleBodyTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        EngineTypes = db.EngineTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        VehicleMakers = db.VehicleMakers.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        RegistrationIssuers = db.RegistrationIssuers.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        Colors = db.Colors.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        BrakeTypes = db.BrakeTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        GearBoxTypes = db.GearBoxTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        SupportingTypes = db.SupportingTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        EcoPrograms = db.EcoPrograms.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        EnginePowerSourceTypes = db.EnginePowerSourceTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        TireTypes = db.TireTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();

        // Geographic
        Countries = db.Countries.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        Cities = db.Cities.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        Communities = db.Communities.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        Streets = db.Streets.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        BusinessTypes = db.BusinessTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();

        // Document/Payment/Request
        DocumentTypes = db.DocumentTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        PaymentTypes = db.PaymentTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        PaymentCategories = db.PaymentCategories.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        RequestTypes = db.RequestTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        RelationTypes = db.RelationTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        AttachmentTypes = db.AttachmentTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        DrivingLicenseCategories = db.DrivingLicenseCategories.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();

        // Technical exam
        TechnicalExamTypes = db.TechnicalExamTypes.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        ExamDetailStatuses = db.ExamDetailStatuses.OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();
        Organizations = db.TechnicalExamOrganizations.Where(o => o.IsActive).OrderBy(x => x.Name).Select(x => new LookupItem { Id = x.Id, Name = x.Name }).ToList();

        // Users (active)
        Users = db.Users.Where(u => u.IsActive).OrderBy(u => u.FullName).Select(u => new LookupItem { Id = u.Id, Name = u.FullName }).ToList();
        Roles = db.Roles.OrderBy(r => r.Name).Select(r => new LookupItem { Id = r.Id, Name = r.Name }).ToList();

        // Customers & Vehicles (for pickers — ID + display name)
        Customers = db.Customers.OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
            .Select(c => new LookupItem { Id = c.Id, Name = c.FirstName + " " + c.LastName + (c.IdentificationNumber != "" ? " (" + c.IdentificationNumber + ")" : "") })
            .ToList();
        Vehicles = db.Vehicles.OrderByDescending(v => v.Id)
            .Select(v => new LookupItem { Id = v.Id, Name = (v.LastRegistrationNumber ?? v.FirstRegistrationNumber ?? "") + " - " + v.ShellNumber })
            .ToList();

        IsLoaded = true;
    }

    public static string GetName(List<LookupItem> list, long? id)
    {
        if (!id.HasValue) return "";
        return list.Find(i => i.Id == id.Value)?.Name ?? "";
    }
}
