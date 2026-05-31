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

public partial class VehicleListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public VehicleListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadVehicles();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.Vehicles;
        BtnSearch.Content = Strings.Search;
        BtnNewVehicle.Content = Strings.NewVehicle;
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColRegNum.Header = Strings.RegistrationNumber;
        ColShellNum.Header = Strings.ShellNumber;
        ColMakeModel.Header = Strings.VehicleModel;
        ColCategory.Header = Strings.VehicleCategory;
        ColYear.Header = Strings.Year;
        ColOwner.Header = Strings.Owner;
    }

    private async void LoadVehicles(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.Vehicles
                    .Include(v => v.VehicleModel)
                    .Include(v => v.Category)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim().ToLower();

                    // Find vehicle IDs owned by customers matching the search term
                    var ownerVehicleIds = db.CustomerVehicleRelations
                        .Where(r => r.EndDate == null &&
                            (r.Customer.FirstName.ToLower().Contains(term) ||
                             r.Customer.LastName.ToLower().Contains(term)))
                        .Select(r => r.VehicleId)
                        .Distinct()
                        .ToList();

                    query = query.Where(v =>
                        v.ShellNumber.ToLower().Contains(term) ||
                        (v.LastRegistrationNumber != null && v.LastRegistrationNumber.ToLower().Contains(term)) ||
                        (v.FirstRegistrationNumber != null && v.FirstRegistrationNumber.ToLower().Contains(term)) ||
                        (v.EngineNumber != null && v.EngineNumber.ToLower().Contains(term)) ||
                        (v.VehicleModel != null && v.VehicleModel.Name.ToLower().Contains(term)) ||
                        ownerVehicleIds.Contains(v.Id));
                }

                _totalRecords = query.Count();

                var pagedIds = query
                    .OrderByDescending(v => v.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(v => v.Id)
                    .ToList();

                var ownerMap = db.CustomerVehicleRelations
                    .Include(cvr => cvr.Customer)
                    .Where(cvr => pagedIds.Contains(cvr.VehicleId) && cvr.EndDate == null)
                    .GroupBy(cvr => cvr.VehicleId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.First().Customer.FirstName + " " + g.First().Customer.LastName);

                var vehicles = query
                    .OrderByDescending(v => v.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(v => new VehicleListItem
                    {
                        Id = v.Id,
                        LastRegistrationNumber = v.LastRegistrationNumber ?? "",
                        ShellNumber = v.ShellNumber,
                        MakeModel = v.VehicleModel != null ? v.VehicleModel.Name : "",
                        CategoryName = v.Category != null ? v.Category.Name : "",
                        Year = v.MakeDate.HasValue ? v.MakeDate.Value.Year.ToString() : ""
                    })
                    .ToList();

                foreach (var v in vehicles)
                {
                    if (ownerMap.TryGetValue(v.Id, out var ownerName))
                        v.OwnerName = ownerName;
                }

                Dispatcher.Invoke(() =>
                {
                    VehiclesGrid.ItemsSource = vehicles;
                    UpdatePagination();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading vehicles:\n{ex.Message}", "Error",
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
        if (_currentPage > 1) { _currentPage--; LoadVehicles(SearchBox.Text); }
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);
        if (_currentPage < totalPages) { _currentPage++; LoadVehicles(SearchBox.Text); }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = 1;
        LoadVehicles(SearchBox.Text);
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _searchTimer?.Stop();
            _currentPage = 1;
            LoadVehicles(SearchBox.Text);
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
            LoadVehicles(SearchBox.Text);
        };
        _searchTimer.Start();
    }

    private void NewVehicle_Click(object sender, RoutedEventArgs e)
    {
        NavigateToDetail(null);
    }

    private void VehiclesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (VehiclesGrid.SelectedItem is VehicleListItem item)
        {
            NavigateToDetail(item.Id);
        }
    }

    private void NavigateToDetail(long? vehicleId)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new VehicleDetailPage(vehicleId);
        }
    }
}

public class VehicleListItem
{
    public long Id { get; set; }
    public string LastRegistrationNumber { get; set; } = string.Empty;
    public string ShellNumber { get; set; } = string.Empty;
    public string MakeModel { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
}
