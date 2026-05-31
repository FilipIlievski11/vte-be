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

public partial class DocumentListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public DocumentListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadDocuments();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.Documents;
        BtnSearch.Content = Strings.Search;
        BtnNewDocument.Content = Strings.NewDocument;
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColDocType.Header = Strings.DocumentType;
        ColCustomer.Header = Strings.Customer;
        ColVehicle.Header = Strings.VehicleLabel;
        ColCreated.Header = Strings.CreatedLabel;
        ColEnded.Header = Strings.EndedLabel;
    }

    private async void LoadDocuments(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.Documents
                    .Include(d => d.DocumentType)
                    .Include(d => d.CustomerVehicleRelation).ThenInclude(cvr => cvr.Customer)
                    .Include(d => d.CustomerVehicleRelation).ThenInclude(cvr => cvr.Vehicle)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim();
                    if (long.TryParse(term, out var id))
                    {
                        query = query.Where(d => d.Id == id);
                    }
                }

                _totalRecords = query.Count();

                var documents = query
                    .OrderByDescending(d => d.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(d => new DocumentListItem
                    {
                        Id = d.Id,
                        DocumentTypeName = d.DocumentType.Name,
                        CustomerName = d.CustomerVehicleRelation.Customer.FirstName + " " + d.CustomerVehicleRelation.Customer.LastName,
                        VehicleRegNumber = d.CustomerVehicleRelation.Vehicle.LastRegistrationNumber ?? "",
                        CreatedDate = d.CreatedAt,
                        EndedDate = d.DateEnded
                    })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    DocumentsGrid.ItemsSource = documents;
                    UpdatePagination();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading documents:\n{ex.Message}", "Error",
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
        if (_currentPage > 1) { _currentPage--; LoadDocuments(SearchBox.Text); }
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);
        if (_currentPage < totalPages) { _currentPage++; LoadDocuments(SearchBox.Text); }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = 1;
        LoadDocuments(SearchBox.Text);
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _searchTimer?.Stop();
            _currentPage = 1;
            LoadDocuments(SearchBox.Text);
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
            LoadDocuments(SearchBox.Text);
        };
        _searchTimer.Start();
    }

    private void DocumentsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DocumentsGrid.SelectedItem is DocumentListItem item)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
                mainWindow.ContentArea.Content = new DocumentDetailPage(item.Id);
        }
    }

    private void NewDocument_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
            mainWindow.ContentArea.Content = new DocumentDetailPage(null);
    }
}

public class DocumentListItem
{
    public long Id { get; set; }
    public string DocumentTypeName { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string VehicleRegNumber { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? EndedDate { get; set; }
}
