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

public partial class RequestListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public RequestListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadRequests();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.Requests;
        BtnSearch.Content = Strings.Search;
        BtnNewRequest.Content = Strings.NewRequest;
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColReqType.Header = Strings.RequestType;
        ColCustomer.Header = Strings.Customer;
        ColVehicle.Header = Strings.VehicleLabel;
        ColCreated.Header = Strings.CreatedLabel;
        ColStatus.Header = Strings.Status;
    }

    private async void LoadRequests(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.Requests
                    .Include(r => r.RequestType)
                    .Include(r => r.CustomerVehicleRelation).ThenInclude(cvr => cvr.Customer)
                    .Include(r => r.CustomerVehicleRelation).ThenInclude(cvr => cvr.Vehicle)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim();
                    if (long.TryParse(term, out var id))
                    {
                        query = query.Where(r => r.Id == id);
                    }
                }

                _totalRecords = query.Count();

                var requests = query
                    .OrderByDescending(r => r.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(r => new RequestListItem
                    {
                        Id = r.Id,
                        RequestTypeName = r.RequestType.Name,
                        CustomerName = r.CustomerVehicleRelation.Customer.FirstName + " " + r.CustomerVehicleRelation.Customer.LastName,
                        VehicleRegNumber = r.CustomerVehicleRelation.Vehicle.LastRegistrationNumber ?? "",
                        CreatedDate = r.CreatedAt,
                        Status = r.DateEnded == null ? "Open" : "Closed"
                    })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    RequestsGrid.ItemsSource = requests;
                    UpdatePagination();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading requests:\n{ex.Message}", "Error",
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
        if (_currentPage > 1) { _currentPage--; LoadRequests(SearchBox.Text); }
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);
        if (_currentPage < totalPages) { _currentPage++; LoadRequests(SearchBox.Text); }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = 1;
        LoadRequests(SearchBox.Text);
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _searchTimer?.Stop();
            _currentPage = 1;
            LoadRequests(SearchBox.Text);
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
            LoadRequests(SearchBox.Text);
        };
        _searchTimer.Start();
    }

    private void RequestsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (RequestsGrid.SelectedItem is RequestListItem item)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
                mainWindow.ContentArea.Content = new RequestDetailPage(item.Id);
        }
    }

    private void NewRequest_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
            mainWindow.ContentArea.Content = new RequestWizardPage();
    }
}

public class RequestListItem
{
    public long Id { get; set; }
    public string RequestTypeName { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string VehicleRegNumber { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
