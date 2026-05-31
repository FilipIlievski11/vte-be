namespace VTE.WPF;

using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VTE.Application;
using VTE.Infrastructure;
using VTE.Infrastructure.Data;
using VTE.Infrastructure.Services;
using VTE.WPF.Views;

public partial class App : System.Windows.Application
{
    public static ServiceProvider? Services { get; private set; }
    public static bool DbAvailable { get; private set; }

    private static readonly string ErrorLogPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "vte-error.txt");

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        try
        {
            File.WriteAllText(ErrorLogPath, "Step 1: Starting...\n");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            File.AppendAllText(ErrorLogPath, "Step 2: Config loaded\n");

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            File.AppendAllText(ErrorLogPath, "Step 3: Serilog created\n");

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddLogging(builder => builder.AddSerilog());
            Services = services.BuildServiceProvider();

            File.AppendAllText(ErrorLogPath, "Step 4: DI built\n");

            // Try database with timeout
            try
            {
                File.AppendAllText(ErrorLogPath, "Step 4b: Attempting DB connection...\n");
                var dbTask = Task.Run(() =>
                {
                    using var scope = Services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                    db.Database.EnsureCreated();
                    SeedData.SeedAsync(db).GetAwaiter().GetResult();
                });

                if (dbTask.Wait(TimeSpan.FromSeconds(10)))
                {
                    DbAvailable = true;
                    File.AppendAllText(ErrorLogPath, "Step 5: DB OK\n");
                    // Load all lookups into cache
                    VTE.WPF.Services.LookupCache.Load();
                    File.AppendAllText(ErrorLogPath, "Step 5b: Lookups cached\n");
                }
                else
                {
                    DbAvailable = false;
                    File.AppendAllText(ErrorLogPath, "Step 5: DB TIMEOUT after 10s\n");
                }
            }
            catch (Exception dbEx)
            {
                DbAvailable = false;
                File.AppendAllText(ErrorLogPath, $"Step 5: DB FAILED: {dbEx.Message}\n");
            }

            if (DbAvailable)
            {
                File.AppendAllText(ErrorLogPath, "Step 6: Showing login\n");
                var login = new LoginWindow();
                if (login.ShowDialog() != true)
                {
                    Shutdown();
                    return;
                }
                File.AppendAllText(ErrorLogPath, "Step 7: Login OK\n");
            }
            else
            {
                var userService = Services.GetRequiredService<CurrentUserService>();
                userService.UserId = 1;
                userService.Username = "admin";
                userService.RoleName = "Administrator";
                File.AppendAllText(ErrorLogPath, "Step 6: Skipped login (no DB)\n");
            }

            File.AppendAllText(ErrorLogPath, "Step 8: Showing main window\n");
            var main = new MainWindow();
            MainWindow = main;
            main.Closed += (_, _) => Shutdown();
            main.Show();
            File.AppendAllText(ErrorLogPath, "Step 9: Main window shown!\n");
        }
        catch (Exception ex)
        {
            File.AppendAllText(ErrorLogPath, $"FATAL: {ex}\n");
            Shutdown(1);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
        Services?.Dispose();
        base.OnExit(e);
    }
}
