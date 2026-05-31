namespace VTE.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Interfaces;
using VTE.Infrastructure.Data;
using VTE.Infrastructure.Data.Repositories;
using VTE.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VteDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("VTE")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<CurrentUserService>();
        services.AddSingleton<ICurrentUserService>(sp => sp.GetRequiredService<CurrentUserService>());

        return services;
    }
}
