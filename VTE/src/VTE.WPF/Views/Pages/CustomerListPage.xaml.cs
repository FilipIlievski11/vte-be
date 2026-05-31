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

public partial class CustomerListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public CustomerListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadCustomers();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.Customers;
        BtnSearch.Content = Strings.Search;
        BtnNewCustomer.Content = Strings.NewCustomer;
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColName.Header = Strings.Customers;
        ColIdNum.Header = Strings.IdentificationNumber;
        ColCompany.Header = Strings.IsCompany;
        ColCity.Header = Strings.IsMacedonian ? "Адреса (на живеење)" : "Address (Living)";
    }

    private async void LoadCustomers(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.Customers
                    .Include(c => c.LivingCity)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim().ToLower();
                    query = query.Where(c =>
                        c.FirstName.ToLower().Contains(term) ||
                        c.LastName.ToLower().Contains(term) ||
                        c.IdentificationNumber.ToLower().Contains(term));
                }

                _totalRecords = query.Count();

                var customers = query
                    .OrderByDescending(c => c.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(c => new CustomerListItem
                    {
                        Id = c.Id,
                        FullName = c.FirstName + " " + c.LastName,
                        IdentificationNumber = c.IdentificationNumber,
                        PhoneNumber = c.PhoneNumber,
                        Email = c.Email,
                        IsCompany = c.IsCompany,
                        CityName = c.LivingCity != null ? c.LivingCity.Name : ""
                    })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    CustomersGrid.ItemsSource = customers;
                    UpdatePagination();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading customers:\n{ex.Message}", "Error",
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
        if (_currentPage > 1) { _currentPage--; LoadCustomers(SearchBox.Text); }
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);
        if (_currentPage < totalPages) { _currentPage++; LoadCustomers(SearchBox.Text); }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = 1;
        LoadCustomers(SearchBox.Text);
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _searchTimer?.Stop();
            _currentPage = 1;
            LoadCustomers(SearchBox.Text);
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
            LoadCustomers(SearchBox.Text);
        };
        _searchTimer.Start();
    }

    private void NewCustomer_Click(object sender, RoutedEventArgs e)
    {
        NavigateToDetail(null);
    }

    private void CustomersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (CustomersGrid.SelectedItem is CustomerListItem item)
        {
            NavigateToDetail(item.Id);
        }
    }

    private void NavigateToDetail(long? customerId)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new CustomerDetailPage(customerId);
        }
    }
}

public class CustomerListItem
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool IsCompany { get; set; }
    public string CityName { get; set; } = string.Empty;
}
