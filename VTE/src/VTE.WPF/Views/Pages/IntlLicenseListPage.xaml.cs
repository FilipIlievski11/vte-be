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

public partial class IntlLicenseListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public IntlLicenseListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadItems();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.IntlDrivingLicenses;
        BtnSearch.Content = Strings.Search;
        BtnNew.Content = Strings.NewIntlLicense;
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColLicenseNumber.Header = Strings.LicenseNumber;
        ColCustomer.Header = Strings.Customer;
        ColIssuedDate.Header = Strings.IssuedDate;
        ColValidUntil.Header = Strings.ValidUntil;
    }

    private async void LoadItems(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.InternationalDrivingLicenses
                    .Include(l => l.Customer)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim().ToLower();
                    query = query.Where(l =>
                        l.LicenseNumber.ToLower().Contains(term) ||
                        l.Customer.FirstName.ToLower().Contains(term) ||
                        l.Customer.LastName.ToLower().Contains(term));
                }

                _totalRecords = query.Count();

                var items = query
                    .OrderByDescending(l => l.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(l => new IntlLicenseListItem
                    {
                        Id = l.Id,
                        LicenseNumber = l.LicenseNumber,
                        CustomerName = l.Customer.FirstName + " " + l.Customer.LastName,
                        IssuedDate = l.IssuedDate,
                        ValidUntilDate = l.ValidUntilDate
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
            MessageBox.Show($"Error loading international driving licenses:\n{ex.Message}", Strings.Error,
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
        if (ItemsGrid.SelectedItem is IntlLicenseListItem item)
        {
            NavigateToDetail(item.Id);
        }
    }

    private void NavigateToDetail(long? id)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new IntlLicenseDetailPage(id);
        }
    }
}

public class IntlLicenseListItem
{
    public long Id { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime ValidUntilDate { get; set; }
}
