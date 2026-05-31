using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VTE.Infrastructure.Tenancy;

namespace VTE.Infrastructure.Persistence;

/// <summary>
/// Used by `dotnet ef` at design time to construct a DbContext without going
/// through the API's DI container. Reads the connection string from the
/// V2_CONNECTION env var or falls back to the local-dev default.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VteDbContext>
{
    public VteDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("V2_CONNECTION")
                 ?? "Server=(localdb)\\MSSQLLocalDB;Database=VTE;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

        var options = new DbContextOptionsBuilder<VteDbContext>()
            .UseSqlServer(cs)
            .Options;

        return new VteDbContext(options, new NullTenantContext());
    }
}
