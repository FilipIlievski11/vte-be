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

public partial class RelationListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public RelationListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadRelations();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.Relations;
        BtnSearch.Content = Strings.Search;
        BtnNewRelation.Content = Strings.NewRelation;
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColCustomerName.Header = Strings.Customer;
        ColVehicle.Header = Strings.VehicleLabel;
        ColRelationType.Header = Strings.RelationTypeField;
        ColStartDate.Header = Strings.StartDate;
        ColEndDate.Header = Strings.EndDate;
        ColNote.Header = Strings.Note;
    }

    private async void LoadRelations(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.CustomerVehicleRelations
                    .Include(r => r.Customer)
                    .Include(r => r.Vehicle)
                    .Include(r => r.RelationType)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim().ToLower();
                    query = query.Where(r =>
                        r.Customer.FirstName.ToLower().Contains(term) ||
                        r.Customer.LastName.ToLower().Contains(term) ||
                        (r.Vehicle.LastRegistrationNumber != null && r.Vehicle.LastRegistrationNumber.ToLower().Contains(term)) ||
                        r.Vehicle.ShellNumber.ToLower().Contains(term));
                }

                _totalRecords = query.Count();

                var relations = query
                    .OrderByDescending(r => r.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(r => new RelationListItem
                    {
                        Id = r.Id,
                        CustomerName = r.Customer.FirstName + " " + r.Customer.LastName,
                        VehicleRegNumber = r.Vehicle.LastRegistrationNumber ?? r.Vehicle.ShellNumber,
                        RelationTypeName = r.RelationType.Name,
                        StartDate = r.StartDate,
                        EndDate = r.EndDate,
                        BeginNote = r.BeginNote ?? ""
                    })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    RelationsGrid.ItemsSource = relations;
                    UpdatePagination();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading relations:\n{ex.Message}", "Error",
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
        if (_currentPage > 1) { _currentPage--; LoadRelations(SearchBox.Text); }
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);
        if (_currentPage < totalPages) { _currentPage++; LoadRelations(SearchBox.Text); }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = 1;
        LoadRelations(SearchBox.Text);
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _searchTimer?.Stop();
            _currentPage = 1;
            LoadRelations(SearchBox.Text);
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
            LoadRelations(SearchBox.Text);
        };
        _searchTimer.Start();
    }

    private void NewRelation_Click(object sender, RoutedEventArgs e)
    {
        NavigateToDetail(null);
    }

    private void RelationsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (RelationsGrid.SelectedItem is RelationListItem item)
        {
            NavigateToDetail(item.Id);
        }
    }

    private void NavigateToDetail(long? relationId)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new RelationDetailPage(relationId);
        }
    }
}

public class RelationListItem
{
    public long Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string VehicleRegNumber { get; set; } = string.Empty;
    public string RelationTypeName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string BeginNote { get; set; } = string.Empty;
}
