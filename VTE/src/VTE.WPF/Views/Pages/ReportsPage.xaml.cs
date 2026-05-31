namespace VTE.WPF.Views.Pages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class ReportsPage : UserControl
{
    private int _reportPage = 1;
    private int _reportPageSize = 50;
    private int _reportTotalRecords;
    private string _reportSearchText = "";

    public ReportsPage()
    {
        InitializeComponent();

        DateFrom.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTo.SelectedDate = DateTime.Today;

        ApplyLanguage();
    }

    public void ApplyLanguage()
    {
        TxtReportTypes.Text = Strings.Reports;
        ReportTitle.Text = Strings.Reports;
        TxtFrom.Text = Strings.From;
        TxtTo.Text = Strings.To;
        BtnGenerate.Content = Strings.Generate;
        BtnReportSearch.Content = Strings.Search;
        BtnReportPrev.Content = Strings.Prev;
        BtnReportNext.Content = Strings.Next;

        var selectedIndex = ReportTypeList.SelectedIndex;
        ReportTypeList.Items.Clear();
        ReportTypeList.Items.Add(Strings.CashRegister);
        ReportTypeList.Items.Add(Strings.PaymentByCategory);
        ReportTypeList.Items.Add(Strings.TechnicalExamsReport);
        ReportTypeList.Items.Add(Strings.UnpaidPayments);
        ReportTypeList.Items.Add(Strings.VehicleRegistry);
        ReportTypeList.Items.Add(Strings.CustomerSummary);
        ReportTypeList.Items.Add(Strings.ExamsByOrganization);
        ReportTypeList.Items.Add(Strings.MonthlyActivity);

        if (selectedIndex >= 0 && selectedIndex < ReportTypeList.Items.Count)
            ReportTypeList.SelectedIndex = selectedIndex;
    }

    private void OnReportTypeSelected(object sender, SelectionChangedEventArgs e)
    {
        if (ReportTypeList.SelectedItem is not string selected) return;

        ReportTitle.Text = selected;
        SummaryText.Text = string.Empty;
        ReportGrid.ItemsSource = null;
        ReportGrid.Columns.Clear();

        // Reset search and pagination
        _reportPage = 1;
        _reportSearchText = "";
        TxtReportSearch.Text = "";

        // Show/hide date filter based on report type
        var needsDateFilter = selected != Strings.CustomerSummary
                           && selected != Strings.UnpaidPayments
                           && selected != Strings.ExamsByOrganization
                           && selected != Strings.MonthlyActivity;
        DateFilterPanel.Visibility = needsDateFilter ? Visibility.Visible : Visibility.Collapsed;

        // Show/hide search and pagination for reports with many rows
        var needsSearch = selected == Strings.VehicleRegistry
                       || selected == Strings.TechnicalExamsReport
                       || selected == Strings.PaymentByCategory
                       || selected == Strings.UnpaidPayments;
        var needsPagination = needsSearch;
        SearchPanel.Visibility = needsSearch ? Visibility.Visible : Visibility.Collapsed;
        PaginationPanel.Visibility = needsPagination ? Visibility.Visible : Visibility.Collapsed;

        GenerateReport(selected);
    }

    private void OnGenerate(object sender, RoutedEventArgs e)
    {
        _reportPage = 1;
        if (ReportTypeList.SelectedItem is string selected)
            GenerateReport(selected);
    }

    private void GenerateReport(string reportType)
    {
        if (reportType == Strings.CashRegister) GenerateCashRegister();
        else if (reportType == Strings.PaymentByCategory) GeneratePaymentByCategory();
        else if (reportType == Strings.TechnicalExamsReport) GenerateTechnicalExamsReport();
        else if (reportType == Strings.UnpaidPayments) GenerateUnpaidPayments();
        else if (reportType == Strings.VehicleRegistry) GenerateVehicleRegistry();
        else if (reportType == Strings.CustomerSummary) GenerateCustomerSummary();
        else if (reportType == Strings.ExamsByOrganization) GenerateExamsByOrganization();
        else if (reportType == Strings.MonthlyActivity) GenerateMonthlyActivity();
    }

    // ── 1. Cash Register (Касов извештај) ──────────────────────────────

    private async void GenerateCashRegister()
    {
        ReportTitle.Text = Strings.CashRegister;
        SummaryText.Text = Strings.Loading;
        var from = DateFrom.SelectedDate;
        var to = DateTo.SelectedDate?.AddDays(1);
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.PaymentDocuments.AsNoTracking().AsQueryable();

                if (from.HasValue) query = query.Where(p => p.PaymentDate >= from.Value);
                if (to.HasValue) query = query.Where(p => p.PaymentDate < to.Value);
                query = query.Where(p => !p.IsCancelled);

                var items = query
                    .SelectMany(p => p.LineItems)
                    .GroupBy(li => li.PaymentItem != null ? li.PaymentItem.Name : "Unknown")
                    .Select(g => new CashRegisterItem
                    {
                        Category = g.Key,
                        Count = g.Count(),
                        TotalAmount = g.Sum(li => li.UnitPrice * li.Quantity),
                        VATAmount = g.Sum(li => li.UnitPrice * li.Quantity * li.VATPercent / 100m)
                    })
                    .OrderBy(x => x.Category)
                    .ToList();

                // Compute NetAmount client-side (simple arithmetic)
                foreach (var item in items)
                    item.NetAmount = item.TotalAmount - item.VATAmount;

                var grandTotal = items.Sum(x => x.TotalAmount);
                var grandVAT = items.Sum(x => x.VATAmount);
                var grandNet = items.Sum(x => x.NetAmount);

                Dispatcher.Invoke(() =>
                {
                    ReportGrid.ItemsSource = items;
                    SummaryText.Text = $"{Strings.GrandTotal} {Strings.TotalAmount}: {grandTotal:N2} | {Strings.VATAmount}: {grandVAT:N2} | {Strings.NetAmount}: {grandNet:N2}";
                });
            });
        }
        catch (Exception ex)
        {
            SummaryText.Text = $"{Strings.Error}: {ex.Message}";
        }
    }

    // ── 2. Payment by Category (Извештај по категорија на плаќање) ─────

    private async void GeneratePaymentByCategory()
    {
        ReportTitle.Text = Strings.PaymentByCategory;
        SummaryText.Text = Strings.Loading;
        var from = DateFrom.SelectedDate;
        var to = DateTo.SelectedDate?.AddDays(1);
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.PaymentDocuments.AsNoTracking().AsQueryable();

                if (from.HasValue) query = query.Where(p => p.PaymentDate >= from.Value);
                if (to.HasValue) query = query.Where(p => p.PaymentDate < to.Value);

                var items = query
                    .GroupBy(p => p.PaymentType != null ? p.PaymentType.Name : "Unknown")
                    .Select(g => new PaymentByCategoryItem
                    {
                        PaymentType = g.Key,
                        TotalDocuments = g.Count(),
                        PaidCount = g.Count(p => p.IsPaid),
                        UnpaidCount = g.Count(p => !p.IsPaid && !p.IsCancelled),
                        CancelledCount = g.Count(p => p.IsCancelled),
                        TotalAmount = g.SelectMany(p => p.LineItems).Sum(li => li.UnitPrice * li.Quantity)
                    })
                    .OrderBy(x => x.PaymentType)
                    .ToList();

                var grandDocs = items.Sum(x => x.TotalDocuments);
                var grandPaid = items.Sum(x => x.PaidCount);
                var grandUnpaid = items.Sum(x => x.UnpaidCount);
                var grandCancelled = items.Sum(x => x.CancelledCount);
                var grandAmount = items.Sum(x => x.TotalAmount);

                Dispatcher.Invoke(() =>
                {
                    ReportGrid.ItemsSource = items;
                    SummaryText.Text = $"{Strings.GrandTotal} {Strings.Total}: {grandDocs:N0} | {Strings.PaidDocuments}: {grandPaid:N0} | {Strings.UnpaidDocuments}: {grandUnpaid:N0} | {Strings.CancelledDocuments}: {grandCancelled:N0} | {Strings.TotalAmount}: {grandAmount:N2}";
                });
            });
        }
        catch (Exception ex)
        {
            SummaryText.Text = $"{Strings.Error}: {ex.Message}";
        }
    }

    // ── 3. Technical Exam Report ───────────────────────────────────────

    private async void GenerateTechnicalExamsReport()
    {
        ReportTitle.Text = Strings.TechnicalExamsReport;
        SummaryText.Text = Strings.Loading;
        var from = DateFrom.SelectedDate;
        var to = DateTo.SelectedDate?.AddDays(1);
        var searchText = _reportSearchText;
        var page = _reportPage;
        var pageSize = _reportPageSize;
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.TechnicalExamReports.AsNoTracking().AsQueryable();

                if (from.HasValue) query = query.Where(e => e.ExamDate >= from.Value);
                if (to.HasValue) query = query.Where(e => e.ExamDate < to.Value);
                if (!string.IsNullOrEmpty(searchText))
                    query = query.Where(e => e.RegistrationNumber.Contains(searchText) ||
                        (e.Organization != null && e.Organization.Name.Contains(searchText)) ||
                        (e.FirstController != null && e.FirstController.FullName.Contains(searchText)));

                var totalRecords = query.Count();
                var passed = query.Count(e => e.VehiclePassed);
                var failed = totalRecords - passed;

                var items = query
                    .OrderByDescending(e => e.ExamDate)
                    .Skip((page - 1) * pageSize).Take(pageSize)
                    .Select(e => new ExamReportItem
                    {
                        ExamDate = e.ExamDate,
                        RegNumber = e.RegistrationNumber,
                        Type = e.ExamType != null ? e.ExamType.Name : "",
                        Organization = e.Organization != null ? e.Organization.Name : "",
                        Controller = e.FirstController != null ? e.FirstController.FullName : "",
                        Result = e.VehiclePassed ? "P" : "F"
                    }).ToList();

                // Translate results client-side
                foreach (var item in items)
                    item.Result = item.Result == "P" ? Strings.Passed : Strings.Failed;

                Dispatcher.Invoke(() =>
                {
                    _reportTotalRecords = totalRecords;
                    ReportGrid.ItemsSource = items;
                    SummaryText.Text = $"{Strings.Total}: {totalRecords:N0} | {Strings.PassedCount}: {passed:N0} | {Strings.FailedCount}: {failed:N0}";
                    UpdateReportPagination();
                });
            });
        }
        catch (Exception ex)
        {
            SummaryText.Text = $"{Strings.Error}: {ex.Message}";
        }
    }

    // ── 4. Unpaid Payments ─────────────────────────────────────────────

    private async void GenerateUnpaidPayments()
    {
        ReportTitle.Text = Strings.UnpaidPayments;
        SummaryText.Text = Strings.Loading;
        var searchText = _reportSearchText;
        var page = _reportPage;
        var pageSize = _reportPageSize;
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.PaymentDocuments
                    .Where(p => !p.IsPaid && !p.IsCancelled)
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(searchText))
                    query = query.Where(p =>
                        p.DocumentNumber.Contains(searchText) ||
                        p.CustomerVehicleRelation.Customer.FirstName.Contains(searchText) ||
                        p.CustomerVehicleRelation.Customer.LastName.Contains(searchText) ||
                        (p.CustomerVehicleRelation.Vehicle.LastRegistrationNumber != null &&
                         p.CustomerVehicleRelation.Vehicle.LastRegistrationNumber.Contains(searchText)));

                var totalRecords = query.Count();
                var totalAmount = query.SelectMany(p => p.LineItems).Sum(li => li.UnitPrice * li.Quantity);

                var items = query
                    .OrderByDescending(p => p.Id)
                    .Skip((page - 1) * pageSize).Take(pageSize)
                    .Select(p => new PaymentReportItem
                    {
                        DocumentNumber = p.DocumentNumber,
                        Date = p.PaymentDate,
                        Customer = p.CustomerVehicleRelation.Customer.FirstName + " " + p.CustomerVehicleRelation.Customer.LastName,
                        Vehicle = p.CustomerVehicleRelation.Vehicle.LastRegistrationNumber ?? "",
                        Amount = p.LineItems.Sum(li => li.UnitPrice * li.Quantity),
                        Type = p.PaymentType != null ? p.PaymentType.Name : "",
                        Status = "U"
                    }).ToList();

                foreach (var item in items)
                    item.Status = Strings.UnpaidPayments;

                Dispatcher.Invoke(() =>
                {
                    _reportTotalRecords = totalRecords;
                    ReportGrid.ItemsSource = items;
                    SummaryText.Text = $"{Strings.Total}: {totalRecords:N0} | {Strings.TotalAmount}: {totalAmount:N2}";
                    UpdateReportPagination();
                });
            });
        }
        catch (Exception ex)
        {
            SummaryText.Text = $"{Strings.Error}: {ex.Message}";
        }
    }

    // ── 5. Vehicle Registry ────────────────────────────────────────────

    private async void GenerateVehicleRegistry()
    {
        ReportTitle.Text = Strings.VehicleRegistry;
        SummaryText.Text = Strings.Loading;
        var from = DateFrom.SelectedDate;
        var to = DateTo.SelectedDate?.AddDays(1);
        var searchText = _reportSearchText;
        var page = _reportPage;
        var pageSize = _reportPageSize;
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.Vehicles.AsNoTracking().AsQueryable();

                if (from.HasValue) query = query.Where(v => v.LastRegistrationDate >= from.Value);
                if (to.HasValue) query = query.Where(v => v.LastRegistrationDate < to.Value);
                if (!string.IsNullOrEmpty(searchText))
                    query = query.Where(v => v.ShellNumber.Contains(searchText) ||
                        (v.LastRegistrationNumber != null && v.LastRegistrationNumber.Contains(searchText)) ||
                        (v.EngineNumber != null && v.EngineNumber.Contains(searchText)) ||
                        (v.VehicleModel != null && v.VehicleModel.Name.Contains(searchText)));

                var totalRecords = query.Count();

                var items = query
                    .OrderByDescending(v => v.Id)
                    .Skip((page - 1) * pageSize).Take(pageSize)
                    .Select(v => new VehicleRegistryItem
                    {
                        RegistrationNumber = v.LastRegistrationNumber ?? v.FirstRegistrationNumber ?? "",
                        ShellNumber = v.ShellNumber,
                        MakeModel = v.VehicleModel != null ? v.VehicleModel.Name : "",
                        Category = v.Category != null ? v.Category.Name : "",
                        Owner = "",
                        RegistrationDate = v.LastRegistrationDate ?? v.FirstRegistrationDate
                    }).ToList();

                Dispatcher.Invoke(() =>
                {
                    _reportTotalRecords = totalRecords;
                    ReportGrid.ItemsSource = items;
                    SummaryText.Text = $"{Strings.Total}: {totalRecords:N0}";
                    UpdateReportPagination();
                });
            });
        }
        catch (Exception ex)
        {
            SummaryText.Text = $"{Strings.Error}: {ex.Message}";
        }
    }

    // ── 6. Customer Summary ────────────────────────────────────────────

    private async void GenerateCustomerSummary()
    {
        ReportTitle.Text = Strings.CustomerSummary;
        SummaryText.Text = Strings.Loading;
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var customers = db.Customers.AsNoTracking().ToList();

                var item = new CustomerSummaryItem
                {
                    TotalCustomers = customers.Count,
                    Companies = customers.Count(c => c.IsCompany),
                    Individuals = customers.Count(c => !c.IsCompany),
                    WithEmail = customers.Count(c => !string.IsNullOrWhiteSpace(c.Email)),
                    WithPhone = customers.Count(c => !string.IsNullOrWhiteSpace(c.PhoneNumber))
                };

                Dispatcher.Invoke(() =>
                {
                    ReportGrid.ItemsSource = new List<CustomerSummaryItem> { item };
                    SummaryText.Text = $"{Strings.Total}: {item.TotalCustomers}";
                });
            });
        }
        catch (Exception ex)
        {
            SummaryText.Text = $"{Strings.Error}: {ex.Message}";
        }
    }

    // ── 7. Exams by Organization ───────────────────────────────────────

    private async void GenerateExamsByOrganization()
    {
        ReportTitle.Text = Strings.ExamsByOrganization;
        SummaryText.Text = Strings.Loading;
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var exams = db.TechnicalExamReports
                    .Include(e => e.Organization)
                    .AsNoTracking()
                    .ToList();

                var items = exams
                    .GroupBy(e => e.Organization?.Name ?? "Unknown")
                    .Select(g => new ExamsByOrganizationItem
                    {
                        OrganizationName = g.Key,
                        TotalExams = g.Count(),
                        Passed = g.Count(e => e.VehiclePassed),
                        Failed = g.Count(e => !e.VehiclePassed),
                        PassRate = g.Count() > 0
                            ? Math.Round(g.Count(e => e.VehiclePassed) * 100.0 / g.Count(), 1)
                            : 0
                    })
                    .OrderByDescending(x => x.TotalExams)
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    ReportGrid.ItemsSource = items;
                    SummaryText.Text = $"{Strings.Total}: {items.Count} | {Strings.ExamsCount}: {items.Sum(x => x.TotalExams)}";
                });
            });
        }
        catch (Exception ex)
        {
            SummaryText.Text = $"{Strings.Error}: {ex.Message}";
        }
    }

    // ── 8. Monthly Activity ────────────────────────────────────────────

    private async void GenerateMonthlyActivity()
    {
        ReportTitle.Text = Strings.MonthlyActivity;
        SummaryText.Text = Strings.Loading;
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var today = DateTime.Today;
                var startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-11);

                var customers = db.Customers.AsNoTracking()
                    .Where(c => c.CreatedAt >= startDate)
                    .ToList();
                var vehicles = db.Vehicles.AsNoTracking()
                    .Where(v => v.CreatedAt >= startDate)
                    .ToList();
                var exams = db.TechnicalExamReports.AsNoTracking()
                    .Where(e => e.ExamDate >= startDate)
                    .ToList();
                var payments = db.PaymentDocuments.AsNoTracking()
                    .Where(p => p.CreatedAt >= startDate)
                    .ToList();

                var items = new List<MonthlyActivityItem>();

                for (int i = 0; i < 12; i++)
                {
                    var monthStart = startDate.AddMonths(i);
                    var monthEnd = monthStart.AddMonths(1);

                    items.Add(new MonthlyActivityItem
                    {
                        Month = monthStart.ToString("yyyy-MM"),
                        NewCustomers = customers.Count(c => c.CreatedAt >= monthStart && c.CreatedAt < monthEnd),
                        NewVehicles = vehicles.Count(v => v.CreatedAt >= monthStart && v.CreatedAt < monthEnd),
                        Exams = exams.Count(e => e.ExamDate >= monthStart && e.ExamDate < monthEnd),
                        Payments = payments.Count(p => p.CreatedAt >= monthStart && p.CreatedAt < monthEnd)
                    });
                }

                Dispatcher.Invoke(() =>
                {
                    ReportGrid.ItemsSource = items;
                    SummaryText.Text = $"{Strings.MonthlyActivity} (12)";
                });
            });
        }
        catch (Exception ex)
        {
            SummaryText.Text = $"{Strings.Error}: {ex.Message}";
        }
    }

    // -- Pagination & Search --

    private void RegenerateCurrentReport()
    {
        if (ReportTypeList.SelectedItem is string selected)
            GenerateReport(selected);
    }

    private void UpdateReportPagination()
    {
        var totalPages = Math.Max(1, (int)Math.Ceiling(_reportTotalRecords / (double)_reportPageSize));
        TxtReportPageInfo.Text = $"{Strings.Page} {_reportPage} {Strings.Of} {totalPages}";
        TxtReportRecordCount.Text = $"{_reportTotalRecords:N0} {Strings.Records}";
        BtnReportPrev.IsEnabled = _reportPage > 1;
        BtnReportNext.IsEnabled = _reportPage < totalPages;
    }

    private void ReportPrev_Click(object sender, RoutedEventArgs e)
    {
        if (_reportPage > 1) { _reportPage--; RegenerateCurrentReport(); }
    }

    private void ReportNext_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_reportTotalRecords / (double)_reportPageSize);
        if (_reportPage < totalPages) { _reportPage++; RegenerateCurrentReport(); }
    }

    private void ReportSearch_Click(object sender, RoutedEventArgs e)
    {
        _reportSearchText = TxtReportSearch.Text.Trim();
        _reportPage = 1;
        RegenerateCurrentReport();
    }

    private void ReportSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) ReportSearch_Click(sender, e);
    }

    private void ReportGrid_AutoGenerated(object sender, EventArgs e)
    {
        TranslateGridHeaders();
    }

    // -- Column header translation --

    private void TranslateGridHeaders()
    {
        var map = new Dictionary<string, string>
        {
            // Exam report
            {"ExamDate", Strings.ExamDate}, {"RegNumber", Strings.RegistrationNumber},
            {"Type", Strings.ExamType}, {"Organization", Strings.Organization},
            {"Controller", Strings.Controller}, {"Result", Strings.Status},
            // Payment report
            {"DocumentNumber", Strings.DocumentNumber}, {"Date", Strings.PaymentDate},
            {"Customer", Strings.Customers}, {"Vehicle", Strings.Vehicles},
            {"Amount", Strings.TotalAmount}, {"Status", Strings.Status},
            // Vehicle registry
            {"RegistrationNumber", Strings.RegistrationNumber}, {"ShellNumber", Strings.ShellNumber},
            {"MakeModel", Strings.VehicleModel}, {"Category", Strings.Category},
            {"Owner", Strings.Owner}, {"RegistrationDate", Strings.MakeDate},
            // Cash register
            {"Count", Strings.Count}, {"TotalAmount", Strings.TotalAmount},
            {"VATAmount", Strings.VATAmount}, {"NetAmount", Strings.NetAmount},
            // Payment by category
            {"PaymentType", Strings.PaymentType}, {"TotalDocuments", Strings.Total},
            {"PaidCount", Strings.PaidCount}, {"UnpaidCount", Strings.UnpaidCount},
            {"CancelledCount", Strings.CancelledDocuments},
            // Customer summary
            {"TotalCustomers", Strings.Total}, {"Companies", Strings.Companies},
            {"Individuals", Strings.Individuals}, {"WithEmail", Strings.WithEmail},
            {"WithPhone", Strings.WithPhone},
            // Exams by org
            {"OrganizationName", Strings.Organization}, {"TotalExams", Strings.Total},
            {"Passed", Strings.PassedCount}, {"Failed", Strings.FailedCount},
            {"PassRate", Strings.PassRate},
            // Monthly
            {"Month", Strings.Month}, {"NewCustomers", Strings.NewCustomersCount},
            {"NewVehicles", Strings.NewVehiclesCount}, {"Exams", Strings.ExamsCount},
            {"Payments", Strings.PaymentsCount},
        };

        foreach (var col in ReportGrid.Columns)
        {
            if (col is DataGridBoundColumn bc && bc.Header is string header)
            {
                if (map.TryGetValue(header, out var translated))
                    bc.Header = translated;
            }
        }
    }

    // -- Helper classes --

    private class CashRegisterItem
    {
        public string Category { get; set; } = "";
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal NetAmount { get; set; }
    }

    private class PaymentByCategoryItem
    {
        public string PaymentType { get; set; } = "";
        public int TotalDocuments { get; set; }
        public int PaidCount { get; set; }
        public int UnpaidCount { get; set; }
        public int CancelledCount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    private class CustomerSummaryItem
    {
        public int TotalCustomers { get; set; }
        public int Companies { get; set; }
        public int Individuals { get; set; }
        public int WithEmail { get; set; }
        public int WithPhone { get; set; }
    }

    private class VehicleRegistryItem
    {
        public string RegistrationNumber { get; set; } = "";
        public string ShellNumber { get; set; } = "";
        public string MakeModel { get; set; } = "";
        public string Category { get; set; } = "";
        public string Owner { get; set; } = "";
        public DateTime? RegistrationDate { get; set; }
    }

    private class ExamReportItem
    {
        public DateTime ExamDate { get; set; }
        public string RegNumber { get; set; } = "";
        public string Type { get; set; } = "";
        public string Organization { get; set; } = "";
        public string Controller { get; set; } = "";
        public string Result { get; set; } = "";
    }

    private class PaymentReportItem
    {
        public string DocumentNumber { get; set; } = "";
        public DateTime? Date { get; set; }
        public string Customer { get; set; } = "";
        public string Vehicle { get; set; } = "";
        public decimal Amount { get; set; }
        public string Type { get; set; } = "";
        public string Status { get; set; } = "";
    }

    private class ExamsByOrganizationItem
    {
        public string OrganizationName { get; set; } = "";
        public int TotalExams { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public double PassRate { get; set; }
    }

    private class MonthlyActivityItem
    {
        public string Month { get; set; } = "";
        public int NewCustomers { get; set; }
        public int NewVehicles { get; set; }
        public int Exams { get; set; }
        public int Payments { get; set; }
    }
}
