namespace VTE.WPF.Views.Pages;

using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class PaymentListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public PaymentListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadPayments();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.Payments;
        BtnSearch.Content = Strings.Search;
        BtnNewPayment.Content = Strings.NewPayment;
        BtnPrintSelected.Content = Strings.IsMacedonian ? "Печати" : "Print";
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColDocNum.Header = Strings.DocumentNumber;
        ColPayDate.Header = Strings.PaymentDate;
        ColCustomer.Header = Strings.Customers;
        ColVehicle.Header = Strings.Vehicles;
        ColAmount.Header = Strings.TotalAmount;
        ColPaid.Header = Strings.IsPaid;
        ColCancelled.Header = Strings.IsCancelled;
    }

    private async void LoadPayments(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.PaymentDocuments
                    .Include(p => p.CustomerVehicleRelation).ThenInclude(r => r.Customer)
                    .Include(p => p.CustomerVehicleRelation).ThenInclude(r => r.Vehicle)
                    .Include(p => p.PaymentType)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim().ToLower();
                    query = query.Where(p => p.DocumentNumber.ToLower().Contains(term));
                }

                _totalRecords = query.Count();

                var payments = query
                    .OrderByDescending(p => p.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(p => new PaymentListItem
                    {
                        Id = p.Id,
                        DocumentNumber = p.DocumentNumber,
                        PaymentDate = p.PaymentDate,
                        CustomerName = p.CustomerVehicleRelation.Customer.FirstName + " " + p.CustomerVehicleRelation.Customer.LastName,
                        VehicleRegNumber = p.CustomerVehicleRelation.Vehicle.LastRegistrationNumber ?? "",
                        Amount = p.LineItems.Sum(li => li.UnitPrice * li.Quantity).ToString("N2"),
                        IsPaid = p.IsPaid,
                        IsCancelled = p.IsCancelled
                    })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    PaymentsGrid.ItemsSource = payments;
                    UpdatePagination();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading payments:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UpdatePagination()
    {
        var totalPages = Math.Max(1, (int)Math.Ceiling(_totalRecords / (double)_pageSize));
        TxtPageInfo.Text = $"{Strings.Page} {_currentPage} {Strings.Of} {totalPages}";
        TxtRecordCount.Text = $"{_totalRecords:N0} {Strings.Records}";
        BtnPrevPage.IsEnabled = _currentPage > 1;
        BtnNextPage.IsEnabled = _currentPage < totalPages;
    }

    private void PrevPage_Click(object sender, RoutedEventArgs e)
    {
        if (_currentPage > 1) { _currentPage--; LoadPayments(SearchBox.Text); }
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);
        if (_currentPage < totalPages) { _currentPage++; LoadPayments(SearchBox.Text); }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = 1;
        LoadPayments(SearchBox.Text);
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _searchTimer?.Stop();
            _currentPage = 1;
            LoadPayments(SearchBox.Text);
        }
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _searchTimer?.Stop();
        _searchTimer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(400)
        };
        _searchTimer.Tick += (s, _) =>
        {
            _searchTimer.Stop();
            _currentPage = 1;
            LoadPayments(SearchBox.Text);
        };
        _searchTimer.Start();
    }

    private void PrintSelected_Click(object sender, RoutedEventArgs e)
    {
        if (PaymentsGrid.SelectedItem is not PaymentListItem selected)
        {
            MessageBox.Show(Strings.IsMacedonian ? "Изберете плаќање за печатење." : "Select a payment to print.",
                Strings.Error, MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var payment = db.PaymentDocuments
                .Include(p => p.LineItems)
                .Include(p => p.CustomerVehicleRelation).ThenInclude(r => r.Customer)
                .Include(p => p.CustomerVehicleRelation).ThenInclude(r => r.Vehicle)
                .FirstOrDefault(p => p.Id == selected.Id);

            if (payment == null) return;

            var customer = payment.CustomerVehicleRelation.Customer;
            var vehicle = payment.CustomerVehicleRelation.Vehicle;
            var orgName = db.TechnicalExamOrganizations.FirstOrDefault()?.Name ?? "VTE";

            var pdfBytes = VTE.WPF.Services.InvoicePdfGenerator.Generate(
                payment, customer, vehicle, payment.LineItems.ToList(), orgName);

            var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"Invoice_{payment.DocumentNumber.Replace("/", "_")}.pdf");
            System.IO.File.WriteAllBytes(tempPath, pdfBytes);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating PDF:\n{ex.Message}", Strings.Error);
        }
    }

    private void NewPayment_Click(object sender, RoutedEventArgs e)
    {
        NavigateToDetail(null);
    }

    private void PaymentsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (PaymentsGrid.SelectedItem is PaymentListItem item)
        {
            NavigateToDetail(item.Id);
        }
    }

    private void NavigateToDetail(long? paymentId)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new PaymentDetailPage(paymentId);
        }
    }
}

public class PaymentListItem
{
    public long Id { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string VehicleRegNumber { get; set; } = string.Empty;
    public string Amount { get; set; } = "N/A";
    public bool IsPaid { get; set; }
    public bool IsCancelled { get; set; }
}
