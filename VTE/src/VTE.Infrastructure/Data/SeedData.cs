namespace VTE.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using VTE.Core.Entities;

public static class SeedData
{
    public static async Task SeedAsync(VteDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var company = new Company { Name = "Default Company" };
        context.Companies.Add(company);
        await context.SaveChangesAsync();

        var org = new TechnicalExamOrganization
        {
            Name = "Main Station",
            CompanyId = company.Id,
            IsActive = true
        };
        context.TechnicalExamOrganizations.Add(org);
        await context.SaveChangesAsync();

        var role = new Role
        {
            Name = "Administrator",
            Privileges =
            [
                new RolePrivilege { EntityName = "Customer", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "Vehicle", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "Request", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "TechnicalExamReport", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "Document", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "PaymentDocument", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "User", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
            ]
        };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var admin = new User
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
            FullName = "System Administrator",
            FirstName = "System",
            LastName = "Administrator",
            RoleId = role.Id,
            OrganizationId = org.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
