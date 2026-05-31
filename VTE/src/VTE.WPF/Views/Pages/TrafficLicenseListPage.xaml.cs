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

public partial class TrafficLicenseListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public TrafficLicenseListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadItems();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.TrafficLicenses;
        BtnSearch.Content = Strings.Search;
        BtnNew.Content = Strings.NewTrafficLicense;
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColLicenseNumber.Header = Strings.LicenseNumber;
        ColPlateNumber.Header = Strings.PlateNumber;
        ColIssuedDate.Header = Strings.IssuedDate;
        ColValidUntil.Header = Strings.ValidUntil;
        ColCustomer.Header = Strings.Customer;
        ColVehicle.Header = Strings.VehicleLabel;
    }

    private async void LoadItems(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.TrafficLicenses
                    .Include(t => t.Document)
                        .ThenInclude(d => d.CustomerVehicleRelation)
                            .ThenInclude(r => r.Customer)
                    .Include(t => t.Document)
                        .ThenInclude(d => d.CustomerVehicleRelation)
                            .ThenInclude(r => r.Vehicle)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim().ToLower();
                    query = query.Where(t =>
                        t.LicenseNumber.ToLower().Contains(term) ||
                        (t.PlateNumber != null && t.PlateNumber.ToLower().Contains(term)) ||
                        t.Document.CustomerVehicleRelation.Customer.FirstName.ToLower().Contains(term) ||
                        t.Document.CustomerVehicleRelation.Customer.LastName.ToLower().Contains(term));
                }

                _totalRecords = query.Count();

                var items = query
                    .OrderByDescending(t => t.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(t => new TrafficLicenseListItem
                    {
                        Id = t.Id,
                        LicenseNumber = t.LicenseNumber,
                        PlateNumber = t.PlateNumber ?? "",
                        IssuedDate = t.IssuedDate,
                        ValidUntilDate = t.ValidUntilDate,
                        CustomerName = t.Document.CustomerVehicleRelation.Customer.FirstName + " " + t.Document.CustomerVehicleRelation.Customer.LastName,
                        VehicleRegNumber = t.Document.CustomerVehicleRelation.Vehicle.LastRegistrationNumber ?? ""
                    })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    ItemsGrid.ItemsSource = items;
                    UpdatePagination();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading traffic licenses:\n{ex.Message}", Strings.Error,
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
        if (_currentPage > 1) { _currentPage--; LoadItems(SearchBox.Text); }
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);
        if (_currentPage < totalPages) { _currentPage++; LoadItems(SearchBox.Text); }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = 1;
        LoadItems(SearchBox.Text);
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _searchTimer?.Stop();
            _currentPage = 1;
            LoadItems(SearchBox.Text);
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
            LoadItems(SearchBox.Text);
        };
        _searchTimer.Start();
    }

    private void New_Click(object sender, RoutedEventArgs e)
    {
        NavigateToDetail(null);
    }

    private void ItemsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ItemsGrid.SelectedItem is TrafficLicenseListItem item)
        {
            NavigateToDetail(item.Id);
        }
    }

    private void NavigateToDetail(long? id)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new TrafficLicenseDetailPage(id);
        }
    }
}

public class TrafficLicenseListItem
{
    public long Id { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string PlateNumber { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime? ValidUntilDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string VehicleRegNumber { get; set; } = string.Empty;
}
