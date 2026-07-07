using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Companies;
using VTE.Domain.Identity;
using VTE.Domain.Payments;
using VTE.Domain.References;
using VTE.Domain.Requests;
using VTE.Domain.Stations;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;
        var cfg = sp.GetRequiredService<IConfiguration>();
        var db = sp.GetRequiredService<VteDbContext>();
        var users = sp.GetRequiredService<UserManager<ApplicationUser>>();
        var roles = sp.GetRequiredService<RoleManager<ApplicationRole>>();
        var log = sp.GetRequiredService<ILoggerFactory>().CreateLogger("DataSeeder");

        // 1. Roles
        foreach (var roleName in new[] { Roles.Administrator, Roles.Operator })
        {
            if (!await roles.RoleExistsAsync(roleName))
            {
                await roles.CreateAsync(new ApplicationRole(roleName));
                log.LogInformation("Seeded role: {Role}", roleName);
            }
        }

        // 2. Default Company (so newly-registered Operators have a tenant to belong to)
        var defaultCompanyName = cfg["Seed:DefaultCompanyName"] ?? "Default Company";
        var company = await db.Companies.IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Name == defaultCompanyName);
        if (company == null)
        {
            company = new Company { Name = defaultCompanyName, Active = true, CreatedAt = DateTime.UtcNow };
            db.Companies.Add(company);
            await db.SaveChangesAsync();
            log.LogInformation("Seeded default Company: {Name} (Id={Id})", company.Name, company.Id);
        }

        // 3. Default Station bound to the default Company
        var defaultStationName = cfg["Seed:DefaultStationName"] ?? "Default Station";
        var station = await db.Stations.IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.CompanyId == company.Id && s.Name == defaultStationName);
        if (station == null)
        {
            station = new Station { Name = defaultStationName, CompanyId = company.Id, Active = true };
            db.Stations.Add(station);
            await db.SaveChangesAsync();
            log.LogInformation("Seeded default Station: {Name} (Id={Id})", station.Name, station.Id);
        }

        // 4. Default Administrator
        var adminUserName = cfg["Seed:DefaultAdminUserName"] ?? "admin";
        var adminEmail = cfg["Seed:DefaultAdminEmail"] ?? "admin@local";
        var adminPassword = cfg["Seed:DefaultAdminPassword"] ?? "ChangeMe!Now1";

        var admin = await users.FindByNameAsync(adminUserName);
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true,
                FullName = "System Administrator",
                CompanyId = null,                          // Administrators are cross-tenant
                IsActive = true,
            };
            var created = await users.CreateAsync(admin, adminPassword);
            if (!created.Succeeded)
            {
                log.LogError("Failed to seed admin: {Errors}",
                    string.Join("; ", created.Errors.Select(e => e.Description)));
                return;
            }
            await users.AddToRoleAsync(admin, Roles.Administrator);
            log.LogInformation("Seeded default Administrator: {UserName}", adminUserName);
        }

        // 5. Request module catalogs
        await SeedRequestCatalogsAsync(db, log);

        // 6. International Driving Licence catalogs
        await SeedInternationalDrivingLicenceCatalogsAsync(db, log);
    }

    private static async Task SeedRequestCatalogsAsync(VteDbContext db, ILogger log)
    {
        // Skip seeding entirely if any RequestType already exists — this avoids
        // re-adding starter rows after the legacy data has been migrated in.
        // Empty DBs (fresh dev environments) still get the starter catalog.
        if (await db.RequestTypes.AnyAsync())
        {
            log.LogInformation("RequestType catalog already populated — skipping starter seed.");
            return;
        }

        // 5a. RequestDocumentPrint — the three legacy paper forms (Plav/Bel/Zelen)
        var prints = new[]
        {
            new RequestDocumentPrint { Code = "PLAV",  Name = "Барање за регистрација (плав образец)" },
            new RequestDocumentPrint { Code = "BEL",   Name = "Барање за пренос на сопственост (бел образец)" },
            new RequestDocumentPrint { Code = "ZELEN", Name = "Барање за одјава (зелен образец)" },
        };
        foreach (var p in prints)
        {
            if (!await db.RequestDocumentPrints.AnyAsync(x => x.Code == p.Code))
            {
                db.RequestDocumentPrints.Add(p);
                log.LogInformation("Seeded RequestDocumentPrint: {Code}", p.Code);
            }
        }
        await db.SaveChangesAsync();

        // Resolve seeded prints to their assigned Ids (for FK on RequestType)
        var plav  = await db.RequestDocumentPrints.FirstAsync(p => p.Code == "PLAV");
        var bel   = await db.RequestDocumentPrints.FirstAsync(p => p.Code == "BEL");
        var zelen = await db.RequestDocumentPrints.FirstAsync(p => p.Code == "ZELEN");

        // 5b. RequestType — common workflows used at every station
        var types = new[]
        {
            new RequestType
            {
                Name = "Прва регистрација",
                Description = "Прв пат регистрирано возило.",
                DocumentPrintId = plav.Id,
                TechnicalExamRequirement = TechnicalExamRequirement.Required,
                PaymentRequired = true,
                IssuesNewRegistration = true,
            },
            new RequestType
            {
                Name = "Продолжување на регистрација",
                Description = "Годишно продолжување на регистрација.",
                DocumentPrintId = plav.Id,
                TechnicalExamRequirement = TechnicalExamRequirement.Required,
                PaymentRequired = true,
                IssuesNewRegistration = true,
                PreviousRegistrationRequired = true,
            },
            new RequestType
            {
                Name = "Пренос на сопственост",
                Description = "Промена на сопственик на возилото.",
                DocumentPrintId = bel.Id,
                TechnicalExamRequirement = TechnicalExamRequirement.Optional,
                PaymentRequired = true,
                IssuesNewRegistration = true,
                TransfersOwnership = true,
            },
            new RequestType
            {
                Name = "Одјава на возило",
                Description = "Трајна одјава од регистар.",
                DocumentPrintId = zelen.Id,
                TechnicalExamRequirement = TechnicalExamRequirement.NotRequired,
                PaymentRequired = true,
                DeactivatesRelation = true,
            },
            new RequestType
            {
                Name = "Технички преглед",
                Description = "Само технички преглед без регистрација.",
                DocumentPrintId = plav.Id,
                TechnicalExamRequirement = TechnicalExamRequirement.Required,
                PaymentRequired = true,
            },
        };
        foreach (var t in types)
        {
            if (!await db.RequestTypes.AnyAsync(x => x.Name == t.Name))
            {
                db.RequestTypes.Add(t);
                log.LogInformation("Seeded RequestType: {Name}", t.Name);
            }
        }

        // 5c. Proof-type catalogs
        var ownershipProofs = new[]
        {
            "Купопродажен договор", "Договор за подарок", "Решение за наследство",
            "Сообраќајна дозвола", "Фактура / профактура", "Друго",
        };
        foreach (var name in ownershipProofs)
        {
            if (!await db.RequestOwnershipProofTypes.AnyAsync(x => x.Name == name))
                db.RequestOwnershipProofTypes.Add(new RequestOwnershipProofType { Name = name });
        }

        var paymentProofs = new[]
        {
            "Уплатница", "Фискална сметка", "Виримански налог",
            "Картичка (POS)", "Готовинска уплата", "Друго",
        };
        foreach (var name in paymentProofs)
        {
            if (!await db.RequestPaymentProofTypes.AnyAsync(x => x.Name == name))
                db.RequestPaymentProofTypes.Add(new RequestPaymentProofType { Name = name });
        }

        var attachmentTypes = new[]
        {
            "Лична карта", "Сообраќајна дозвола", "Технички преглед", "Договор", "Слика од возилото", "Друго",
        };
        foreach (var name in attachmentTypes)
        {
            if (!await db.RequestAttachmentTypes.AnyAsync(x => x.Name == name))
                db.RequestAttachmentTypes.Add(new RequestAttachmentType { Name = name });
        }

        await db.SaveChangesAsync();
    }

    /// <summary>
    /// 16 driving-licence categories (ids 3-18 preserved 1:1 with legacy
    /// DriveingLicenceCtegories) + one PriceCatalog row for the IDL flat fee.
    ///
    /// Note: the bulk historical pricing migration already carried over 3 legacy
    /// PaymentItemParametars rows with Trigger=InternationalDrivingLicence (ids 155/931/1632,
    /// one per legacy company) — but all 3 have VehiclePaymentCategoryId=11 set, which can
    /// never match this workflow's vehicle-less PricingEvaluator call (no vehicle ⇒
    /// VehiclePaymentCategoryId is always null on the evaluator side). Rather than mutate
    /// already-migrated historical-parity data, this seeds a NEW row scoped for the
    /// vehicle-less case (VehiclePaymentCategoryId=NULL) so the debt hook actually fires.
    /// </summary>
    private static async Task SeedInternationalDrivingLicenceCatalogsAsync(VteDbContext db, ILogger log)
    {
        var categories = new[]
        {
            (Id: 3,  Code: "A",   Description: "моторцикли"),
            (Id: 4,  Code: "B",   Description: "возила, освен возилата од категоријата А чија најголема дозволена тежина не преминува 3500кг (770 ливри) и кои освен седиштето за возачот имаат најмногу 8 седишта"),
            (Id: 5,  Code: "C",   Description: "возила, наменети за превоз на товар чија најголема дозволена тежина преминува 3500кг (7700 ливри)"),
            (Id: 6,  Code: "D",   Description: "АВТОБУСИ"),
            (Id: 7,  Code: "E",   Description: "ПРИКОЛИЦИ"),
            (Id: 8,  Code: "A1",  Description: (string?)null),
            (Id: 9,  Code: "C1",  Description: (string?)null),
            (Id: 10, Code: "D1",  Description: (string?)null),
            (Id: 11, Code: "BE",  Description: (string?)null),
            (Id: 12, Code: "C1E", Description: (string?)null),
            (Id: 13, Code: "CE",  Description: (string?)null),
            (Id: 14, Code: "D1E", Description: (string?)null),
            (Id: 15, Code: "DE",  Description: (string?)null),
            (Id: 16, Code: "G",   Description: (string?)null),
            (Id: 17, Code: "F",   Description: (string?)null),
            (Id: 18, Code: "M",   Description: (string?)null),
        };
        foreach (var c in categories)
        {
            if (!await db.DrivingLicenceCategories.AnyAsync(x => x.Id == c.Id))
            {
                db.DrivingLicenceCategories.Add(new DrivingLicenceCategory
                {
                    Id = c.Id, Code = c.Code, Description = c.Description, Active = true,
                });
            }
        }
        await db.SaveChangesAsync();

        const string idlCode = "IDL-ISSUE";
        if (!await db.PriceCatalogs.AnyAsync(x => x.Code == idlCode))
        {
            // VatRateId=3 (18%, standard rate) matches the legacy-migrated rows above.
            db.PriceCatalogs.Add(new PriceCatalog
            {
                Code = idlCode,
                Name = "Издавање на меѓународна возачка дозвола",
                BasePrice = 842.52m,
                VatRateId = 3,
                Trigger = PriceTrigger.InternationalDrivingLicence,
                VehicleField = null,
                VehiclePaymentCategoryId = null,
                CommunityId = null,
                PriceCompanyId = null,
                Active = true,
            });
            await db.SaveChangesAsync();
            log.LogInformation("Seeded PriceCatalog: {Code}", idlCode);
        }

        const string permCode = "PERM-ISSUE";
        if (!await db.PriceCatalogs.AnyAsync(x => x.Code == permCode))
        {
            // Legacy "Одобрение за туѓо возило" / "за Сите возила" — PaymentItemParametars
            // rows 271 (company 3) and 1690 (company 4), both 506.22 ден flat,
            // VehicleField=Null (matches unconditionally). Confirm against current
            // real-world pricing before go-live.
            db.PriceCatalogs.Add(new PriceCatalog
            {
                Code = permCode,
                Name = "Одобрение за туѓо возило",
                BasePrice = 506.22m,
                VatRateId = 3,
                Trigger = PriceTrigger.Permission,
                VehicleField = null,
                VehiclePaymentCategoryId = null,
                CommunityId = null,
                PriceCompanyId = null,
                Active = true,
            });
            await db.SaveChangesAsync();
            log.LogInformation("Seeded PriceCatalog: {Code}", permCode);
        }
    }
}
