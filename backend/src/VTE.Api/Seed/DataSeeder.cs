using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Identity;
using VTE.Domain.Stations;
using VTE.Infrastructure;

namespace VTE.Api.Seed;

public static class DataSeeder
{
    // Runs on startup. Idempotent:
    //  - Creates the two roles if absent (already seeded by bootstrap-vte2.sql, but re-checked here for safety).
    //  - Creates a default Station if none exists.
    //  - Creates a default Administrator from config (Seed:DefaultAdminUserName / Email / Password).
    //    If no DefaultAdminPassword is configured, no admin is created.
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;

        var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var userMgr = sp.GetRequiredService<UserManager<ApplicationUser>>();
        var db      = sp.GetRequiredService<VteDbContext>();
        var cfg     = sp.GetRequiredService<IConfiguration>();
        var log     = sp.GetRequiredService<ILogger<Program>>();

        // Roles — also seeded by SQL bootstrap, but ensure idempotency.
        foreach (var role in Roles.All)
        {
            if (!await roleMgr.RoleExistsAsync(role))
            {
                await roleMgr.CreateAsync(new IdentityRole(role));
                log.LogInformation("Seeded role: {Role}", role);
            }
        }

        // Default Station
        var hasStation = await db.Stations.AnyAsync();
        if (!hasStation)
        {
            db.Stations.Add(new Station { Name = "Default Station", Code = "DEFAULT", IsActive = true });
            await db.SaveChangesAsync();
            log.LogInformation("Seeded default Station.");
        }

        // Default Administrator
        var adminUserName = cfg["Seed:DefaultAdminUserName"] ?? "admin";
        var adminEmail    = cfg["Seed:DefaultAdminEmail"]    ?? "admin@local";
        var adminPassword = cfg["Seed:DefaultAdminPassword"];
        if (string.IsNullOrEmpty(adminPassword))
        {
            log.LogInformation("No Seed:DefaultAdminPassword configured — skipping admin seed.");
            return;
        }

        var existing = await userMgr.FindByNameAsync(adminUserName);
        if (existing is null)
        {
            var user = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = await userMgr.CreateAsync(user, adminPassword);
            if (!result.Succeeded)
            {
                log.LogError("Failed to seed admin user: {Errors}",
                    string.Join("; ", result.Errors.Select(e => e.Description)));
                return;
            }
            await userMgr.AddToRoleAsync(user, Roles.Administrator);
            log.LogInformation("Seeded default Administrator: {UserName}", adminUserName);
        }
    }
}
