namespace VTE.WPF.Views;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class MainWindow : Window
{
    private Button? _activeNavButton;

    public MainWindow()
    {
        InitializeComponent();
        ContentArea.Content = new Pages.DashboardPage();
        SetActiveNav(BtnDashboard);

        var userService = App.Services?.GetRequiredService<VTE.Infrastructure.Services.CurrentUserService>();
        TxtCurrentUser.Text = $"User: {userService?.Username ?? "N/A"}";
        ApplyLanguage();
    }

    private void ApplyLanguage()
    {
        Title = Strings.AppTitle;
        TxtBrandingSub.Text = Strings.TechnicalExamination;
        BtnDashboard.Content = Strings.Dashboard;
        BtnCustomers.Content = Strings.Customers;
        BtnVehicles.Content = Strings.Vehicles;
        BtnRequests.Content = Strings.Requests;
        BtnExams.Content = Strings.TechnicalExams;
        BtnDocuments.Content = Strings.Documents;
        BtnPayments.Content = Strings.Payments;
        BtnRelations.Content = Strings.IsMacedonian ? "Врски клиент-возило" : "Relations";
        BtnPermissions.Content = Strings.IsMacedonian ? "Дозволи" : "Permissions";
        BtnTrafficLic.Content = Strings.IsMacedonian ? "Сообраќајни дозволи" : "Traffic Licenses";
        BtnIntlLic.Content = Strings.IsMacedonian ? "Меѓународни дозволи" : "Intl. Driving Licenses";
        BtnReports.Content = Strings.Reports;
        BtnLookups.Content = Strings.Lookups;
        BtnCompanies.Content = Strings.IsMacedonian ? "Организации" : "Organizations";
        BtnAdmin.Content = Strings.Administration;
        TxtNavOperations.Text = Strings.Operations;
        TxtNavAnalytics.Text = Strings.Analytics;
        TxtNavConfiguration.Text = Strings.Configuration;
        TxtStatus.Text = Strings.Ready;
        BtnLanguageToggle.Content = Strings.IsMacedonian ? Strings.SwitchToEnglish : Strings.SwitchToMacedonian;
        BtnThemeToggle.Content = Services.ThemeService.IsDark ? Strings.LightMode : Strings.DarkMode;
    }

    private void SetActiveNav(Button btn)
    {
        if (_activeNavButton != null)
            _activeNavButton.Background = Brushes.Transparent;
        _activeNavButton = btn;
        _activeNavButton.Background = new SolidColorBrush(
            (Color)ColorConverter.ConvertFromString("#313244"));
    }

    private async void OnSync(object s, RoutedEventArgs e)
    {
        BtnSync.IsEnabled = false;
        BtnSync.Content = "⏳ Синхронизација...";
        TxtStatus.Text = "Синхронизација со стара база...";

        var result = await Services.DataSyncService.SyncAsync(progress =>
        {
            Dispatcher.Invoke(() => TxtStatus.Text = progress);
        });

        BtnSync.IsEnabled = true;
        BtnSync.Content = Strings.IsMacedonian ? "🔄 Синхронизирај" : "🔄 Sync Data";

        if (result.Success)
        {
            TxtStatus.Text = Strings.IsMacedonian ? "Подготвено" : "Ready";
            MessageBox.Show(
                $"Синхронизација завршена!\n\nНови клиенти: +{result.NewCustomers}\nНови возила: +{result.NewVehicles}\n\nВкупно клиенти: {result.TotalCustomers:N0}\nВкупно возила: {result.TotalVehicles:N0}",
                "Синхронизација", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            TxtStatus.Text = "Грешка при синхронизација";
            MessageBox.Show($"Грешка:\n{result.Error}", "Синхронизација", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ShowContent(string title, string message)
    {
        ContentArea.Content = new StackPanel
        {
            Children =
            {
                new TextBlock
                {
                    Text = title,
                    FontSize = 28,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    Margin = new Thickness(0, 0, 0, 20)
                },
                new TextBlock
                {
                    Text = message,
                    FontSize = 16,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a6adc8")),
                    TextWrapping = TextWrapping.Wrap
                }
            }
        };
    }

    private void OnNavDashboard(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.DashboardPage(); }
    private void OnNavCustomers(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.CustomerListPage(); }
    private void OnNavVehicles(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.VehicleListPage(); }
    private void OnNavRequests(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.RequestListPage(); }
    private void OnNavExams(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.ExamListPage(); }
    private void OnNavDocuments(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.DocumentListPage(); }
    private void OnNavPayments(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.PaymentListPage(); }
    private void OnNavRelations(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.RelationListPage(); }
    private void OnNavPermissions(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.PermissionListPage(); }
    private void OnNavTrafficLicenses(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.TrafficLicenseListPage(); }
    private void OnNavIntlLicenses(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.IntlLicenseListPage(); }
    private void OnNavReports(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.ReportsPage(); }
    private void OnNavLookups(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.LookupsPage(); }
    private void OnNavCompanies(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.CompanyPage(); }
    private void OnNavAdmin(object s, RoutedEventArgs e) { SetActiveNav((Button)s); ContentArea.Content = new Pages.AdminPage(); }

    private void OnToggleTheme(object s, RoutedEventArgs e)
    {
        Services.ThemeService.Toggle();
        BtnThemeToggle.Content = Services.ThemeService.IsDark ? Strings.LightMode : Strings.DarkMode;

        // Refresh current page to pick up new colors
        ContentArea.Content = new Pages.DashboardPage();
        SetActiveNav(BtnDashboard);
    }

    private void OnToggleLanguage(object s, RoutedEventArgs e)
    {
        Strings.SetLanguage(!Strings.IsMacedonian);
        ApplyLanguage();
        // Refresh current content
        ContentArea.Content = new Pages.DashboardPage();
        SetActiveNav(BtnDashboard);
    }
}
