namespace VTE.WPF.Views.Pages;

using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class DashboardPage : UserControl
{
    public DashboardPage()
    {
        InitializeComponent();
        Loaded += (_, _) => LoadDashboard();
    }

    private void ApplyLanguage()
    {
        LblCustomers.Text = Strings.Customers.ToUpper();
        LblVehicles.Text = Strings.Vehicles.ToUpper();
        LblExamsToday.Text = Strings.ExamsToday;
        LblUnpaid.Text = Strings.Unpaid;
        LblCustomersSub.Text = Strings.Registered;
        LblVehiclesSub.Text = Strings.InRegistry;
        LblExamsTodaySub.Text = Strings.Inspections;
        LblUnpaidSub.Text = Strings.PendingPayments;
        LblQuickActions.Text = Strings.QuickActions;
        LblRecentActivity.Text = Strings.RecentActivity;
        TxtQNewCustomer.Text = Strings.NewCustomer.TrimStart('+').Trim();
        TxtQRegisterVehicle.Text = Strings.RegisterVehicle;
        TxtQNewExam.Text = Strings.NewTechnicalExam;
        TxtQNewRequest.Text = Strings.NewRequest.TrimStart('+').Trim();
        TxtQNewPayment.Text = Strings.NewPayment.TrimStart('+').Trim();
    }

    private async void LoadDashboard()
    {
        ApplyLanguage();

        // Greeting (no DB needed)
        var hour = DateTime.Now.Hour;
        var greeting = hour < 12 ? Strings.GoodMorning : hour < 18 ? Strings.GoodAfternoon : Strings.GoodEvening;
        var username = App.Services?.GetRequiredService<VTE.Infrastructure.Services.CurrentUserService>()?.Username ?? "User";
        TxtGreeting.Text = $"{greeting}, {username}";
        TxtDate.Text = DateTime.Now.ToString("dddd, MMMM d, yyyy");

        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                // Main stats
                var customers = db.Customers.Count();
                var vehicles = db.Vehicles.Count();
                var examsToday = db.TechnicalExamReports.Count(e => e.ExamDate.Date == DateTime.Today);
                var unpaid = db.PaymentDocuments.Count(p => !p.IsPaid && !p.IsCancelled);

                // (summary stats moved to payment tree)

                // Recent activity data
                var recentCustomers = db.Customers
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(3)
                    .Select(c => new { c.FirstName, c.LastName, c.CreatedAt })
                    .ToList();

                var recentExams = db.TechnicalExamReports
                    .OrderByDescending(e => e.ExamDate)
                    .Take(3)
                    .Select(e => new { e.RegistrationNumber, e.VehiclePassed, e.ExamDate })
                    .ToList();

                var recentPayments = db.PaymentDocuments
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(2)
                    .Select(p => new { p.DocumentNumber, p.IsPaid, p.CreatedAt })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    TxtCustomers.Text = customers.ToString("N0");
                    TxtVehicles.Text = vehicles.ToString("N0");
                    TxtExamsToday.Text = examsToday.ToString();
                    TxtUnpaid.Text = unpaid.ToString();

                    // Summary stats now in payment tree instead

                    // Recent activity
                    ActivityList.Children.Clear();

                    foreach (var c in recentCustomers)
                        AddActivityItem($"{Strings.CustomerRegistered} {c.FirstName} {c.LastName}", "#89b4fa", c.CreatedAt);

                    foreach (var e in recentExams)
                    {
                        var status = e.VehiclePassed ? Strings.Passed : Strings.Failed;
                        var color = e.VehiclePassed ? "#a6e3a1" : "#f38ba8";
                        AddActivityItem($"{Strings.ExamResult} {e.RegistrationNumber}: {status}", color, e.ExamDate);
                    }

                    foreach (var p in recentPayments)
                    {
                        var status = p.IsPaid ? Strings.IsPaid : Strings.PendingPayments;
                        AddActivityItem($"{Strings.PaymentStatus} {p.DocumentNumber}: {status}", "#f9e2af", p.CreatedAt);
                    }

                    if (ActivityList.Children.Count == 0)
                        AddActivityItem(Strings.NoRecentActivity, "#6c7086");

                    // Load payment tree
                    LoadPaymentTree(db);
                });
            });
        }
        catch (Exception ex)
        {
            AddActivityItem("Could not load data: " + ex.Message, "#f38ba8");
        }
    }

    private void LoadPaymentTree(VteDbContext db)
    {
        PaymentTreePanel.Children.Clear();

        // Old app logic: query PaymentInstallments (Rata) paid today
        // SP printShortPivotPaymentDocumentByDate filters by Rata.DatePayed
        var todayStart = DateTime.Today;
        var todayEnd = todayStart.AddDays(1);

        var rataQuery = db.PaymentInstallments
            .Where(r => r.IsPaid && r.PaidDate >= todayStart && r.PaidDate < todayEnd)
            .Include(r => r.PaymentDocument).ThenInclude(p => p.CustomerVehicleRelation).ThenInclude(rel => rel.Customer)
            .Include(r => r.PaymentDocument).ThenInclude(p => p.CustomerVehicleRelation).ThenInclude(rel => rel.Vehicle)
            .Include(r => r.PaymentDocument).ThenInclude(p => p.PaymentType)
            .Where(r => r.PaymentDocument != null && !r.PaymentDocument.IsCancelled)
            .OrderByDescending(r => r.Id)
            .Take(100)
            .ToList();

        if (rataQuery.Count == 0)
        {
            // Fallback: last 50 paid installments
            rataQuery = db.PaymentInstallments
                .Where(r => r.IsPaid)
                .Include(r => r.PaymentDocument).ThenInclude(p => p.CustomerVehicleRelation).ThenInclude(rel => rel.Customer)
                .Include(r => r.PaymentDocument).ThenInclude(p => p.CustomerVehicleRelation).ThenInclude(rel => rel.Vehicle)
                .Include(r => r.PaymentDocument).ThenInclude(p => p.PaymentType)
                .Where(r => r.PaymentDocument != null && !r.PaymentDocument.IsCancelled)
                .OrderByDescending(r => r.PaidDate)
                .Take(50)
                .ToList();
        }

        decimal grandTotal = 0;

        var byCustomer = rataQuery.GroupBy(r =>
        {
            var c = r.PaymentDocument?.CustomerVehicleRelation?.Customer;
            return c != null ? $"{c.LastName} {c.FirstName}" : "Непознат";
        });

        foreach (var custGroup in byCustomer)
        {
            var custTotal = custGroup.Sum(r => r.Amount);
            grandTotal += custTotal;

            var custHeader = new TextBlock
            {
                Text = $"⊞ Комитент: {custGroup.Key} (Total {custTotal:N2} ден.)",
                FontWeight = FontWeights.SemiBold, FontSize = 13,
                Margin = new Thickness(0, 8, 0, 4)
            };
            custHeader.SetResourceReference(TextBlock.ForegroundProperty, "TextPrimary");
            PaymentTreePanel.Children.Add(custHeader);

            var byVehicle = custGroup.GroupBy(r =>
            {
                var v = r.PaymentDocument?.CustomerVehicleRelation?.Vehicle;
                var model = v?.VehicleModelId != null ? Services.LookupCache.GetName(Services.LookupCache.VehicleModels, v.VehicleModelId) : "";
                return v != null ? $"{v.ShellNumber}({model}) {v.LastRegistrationNumber ?? ""}" : "";
            });

            foreach (var vehGroup in byVehicle)
            {
                var vehTotal = vehGroup.Sum(r => r.Amount);
                var vehHeader = new TextBlock
                {
                    Text = $"  ⊟ Возило: {vehGroup.Key} (Total {vehTotal:N2} ден.)",
                    FontSize = 12, Margin = new Thickness(16, 2, 0, 2)
                };
                vehHeader.SetResourceReference(TextBlock.ForegroundProperty, "AccentBlue");
                PaymentTreePanel.Children.Add(vehHeader);

                foreach (var r in vehGroup)
                {
                    var typeName = r.PaymentDocument?.PaymentType?.Name ?? "";
                    var docNum = r.PaymentDocument?.DocumentNumber ?? "";
                    var itemGrid = new Grid { Margin = new Thickness(32, 1, 0, 1) };
                    itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    itemGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    var desc = new TextBlock { Text = $"{typeName}  по барање бр.{docNum}", FontSize = 11.5 };
                    desc.SetResourceReference(TextBlock.ForegroundProperty, "TextMuted");
                    Grid.SetColumn(desc, 0);
                    var price = new TextBlock { Text = $"{r.Amount:N0}", FontSize = 11.5, Margin = new Thickness(16, 0, 0, 0) };
                    price.SetResourceReference(TextBlock.ForegroundProperty, "TextPrimary");
                    Grid.SetColumn(price, 1);
                    itemGrid.Children.Add(desc);
                    itemGrid.Children.Add(price);
                    PaymentTreePanel.Children.Add(itemGrid);
                }
            }
        }

        TxtPaymentTotal.Text = $"(Total {grandTotal:N2} ден.)";
    }

    private void AddActivityItem(string text, string color, DateTime? date = null)
    {
        var border = new Border
        {
            CornerRadius = new CornerRadius(6),
            Padding = new Thickness(12, 10, 12, 10),
            Margin = new Thickness(0, 0, 0, 6)
        };
        border.SetResourceReference(Border.BackgroundProperty, "ActivityRowBg");

        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        if (date.HasValue)
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        // Color dot
        var dot = new Border
        {
            Width = 8, Height = 8,
            CornerRadius = new CornerRadius(4),
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)),
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 10, 0)
        };
        Grid.SetColumn(dot, 0);
        grid.Children.Add(dot);

        // Text
        var txt = new TextBlock
        {
            Text = text,
            FontSize = 12,
            VerticalAlignment = VerticalAlignment.Center
        };
        txt.SetResourceReference(TextBlock.ForegroundProperty, "TextSecondary");
        Grid.SetColumn(txt, 1);
        grid.Children.Add(txt);

        // Date
        if (date.HasValue)
        {
            var dateTxt = new TextBlock
            {
                Text = FormatRelativeDate(date.Value),
                FontSize = 11,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(12, 0, 0, 0)
            };
            dateTxt.SetResourceReference(TextBlock.ForegroundProperty, "TextDimmed");
            Grid.SetColumn(dateTxt, 2);
            grid.Children.Add(dateTxt);
        }

        border.Child = grid;
        ActivityList.Children.Add(border);
    }

    private static string FormatRelativeDate(DateTime date)
    {
        var diff = DateTime.Now - date;
        if (diff.TotalMinutes < 1) return "just now";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes}m ago";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours}h ago";
        if (diff.TotalDays < 7) return $"{(int)diff.TotalDays}d ago";
        return date.ToString("MMM d");
    }

    // Quick action handlers
    private void QuickNewCustomer(object s, RoutedEventArgs e)
    {
        var mainWindow = Window.GetWindow(this) as VTE.WPF.Views.MainWindow;
        if (mainWindow != null) mainWindow.ContentArea.Content = new CustomerDetailPage(null);
    }

    private void QuickNewVehicle(object s, RoutedEventArgs e)
    {
        var mainWindow = Window.GetWindow(this) as VTE.WPF.Views.MainWindow;
        if (mainWindow != null) mainWindow.ContentArea.Content = new VehicleDetailPage(null);
    }

    private void QuickNewExam(object s, RoutedEventArgs e)
    {
        var mainWindow = Window.GetWindow(this) as VTE.WPF.Views.MainWindow;
        if (mainWindow != null) mainWindow.ContentArea.Content = new ExamDetailPage(null);
    }

    private void QuickNewRequest(object s, RoutedEventArgs e)
    {
        var mainWindow = Window.GetWindow(this) as VTE.WPF.Views.MainWindow;
        if (mainWindow != null) mainWindow.ContentArea.Content = new RequestDetailPage(null);
    }

    private void QuickNewPayment(object s, RoutedEventArgs e)
    {
        var mainWindow = Window.GetWindow(this) as VTE.WPF.Views.MainWindow;
        if (mainWindow != null) mainWindow.ContentArea.Content = new PaymentDetailPage(null);
    }
}
