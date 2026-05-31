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

public partial class PermissionListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public PermissionListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadPermissions();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.PermissionsTitle;
        BtnSearch.Content = Strings.Search;
        BtnNewPermission.Content = Strings.NewPermission;
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColPermissionNumber.Header = Strings.PermissionNumber;
        ColCustomer.Header = Strings.Customer;
        ColVehicle.Header = Strings.VehicleLabel;
        ColIssuedDate.Header = Strings.IssuedDate;
        ColValidUntil.Header = Strings.ValidUntil;
    }

    private async void LoadPermissions(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.Permissions
                    .Include(p => p.Document)
                        .ThenInclude(d => d.CustomerVehicleRelation)
                            .ThenInclude(r => r.Customer)
                    .Include(p => p.Document)
                        .ThenInclude(d => d.CustomerVehicleRelation)
                            .ThenInclude(r => r.Vehicle)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim().ToLower();
                    query = query.Where(p =>
                        p.PermissionNumber.ToLower().Contains(term) ||
                        (p.Document.CustomerVehicleRelation.Customer.FirstName + " " +
                         p.Document.CustomerVehicleRelation.Customer.LastName).ToLower().Contains(term));
                }

                _totalRecords = query.Count();

                var permissions = query
                    .OrderByDescending(p => p.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(p => new PermissionListItem
                    {
                        Id = p.Id,
                        PermissionNumber = p.PermissionNumber,
                        CustomerName = p.Document.CustomerVehicleRelation.Customer.FirstName + " " +
                                       p.Document.CustomerVehicleRelation.Customer.LastName,
                        VehicleRegNumber = p.Document.CustomerVehicleRelation.Vehicle.LastRegistrationNumber ?? "",
                        IssuedDate = p.IssuedDate,
                        ValidUntilDate = p.ValidUntilDate
                    })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    PermissionsGrid.ItemsSource = permissions;
                    UpdatePagination();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading permissions:\n{ex.Message}", "Error",
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
        if (_currentPage > 1) { _currentPage--; LoadPermissions(SearchBox.Text); }
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);
        if (_currentPage < totalPages) { _currentPage++; LoadPermissions(SearchBox.Text); }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = 1;
        LoadPermissions(SearchBox.Text);
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _searchTimer?.Stop();
            _currentPage = 1;
            LoadPermissions(SearchBox.Text);
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
            LoadPermissions(SearchBox.Text);
        };
        _searchTimer.Start();
    }

    private void NewPermission_Click(object sender, RoutedEventArgs e)
    {
        NavigateToDetail(null);
    }

    private void PermissionsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (PermissionsGrid.SelectedItem is PermissionListItem item)
        {
            NavigateToDetail(item.Id);
        }
    }

    private void NavigateToDetail(long? permissionId)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new PermissionDetailPage(permissionId);
        }
    }
}

public class PermissionListItem
{
    public long Id { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string VehicleRegNumber { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime? ValidUntilDate { get; set; }
}
