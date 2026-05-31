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

public partial class ExamListPage : UserControl
{
    private int _currentPage = 1;
    private int _pageSize = 50;
    private int _totalRecords;
    private System.Windows.Threading.DispatcherTimer? _searchTimer;

    public ExamListPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadExams();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.TechnicalExams;
        BtnSearch.Content = Strings.Search;
        BtnNewExam.Content = Strings.NewExam;
        BtnPrevPage.Content = Strings.Prev;
        BtnNextPage.Content = Strings.Next;
        ColRegNum.Header = Strings.RegNumberShort;
        ColExamDate.Header = Strings.DateLabel;
        ColValidUntil.Header = Strings.ValidUntil;
        ColExamType.Header = Strings.TypeLabel;
        ColOrganization.Header = Strings.Organization;
        ColPassed.Header = Strings.Passed;
        ColController.Header = Strings.Controller;
    }

    private async void LoadExams(string? searchText = null)
    {
        try
        {
            await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var query = db.TechnicalExamReports
                    .Include(e => e.ExamType)
                    .Include(e => e.Organization)
                    .Include(e => e.FirstController)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var term = searchText.Trim().ToLower();
                    query = query.Where(e =>
                        e.RegistrationNumber.ToLower().Contains(term));
                }

                _totalRecords = query.Count();

                var exams = query
                    .OrderByDescending(e => e.Id)
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(e => new ExamListItem
                    {
                        Id = e.Id,
                        RegistrationNumber = e.RegistrationNumber,
                        ExamDate = e.ExamDate.ToString("dd.MM.yyyy"),
                        ValidUntilDate = e.ValidUntilDate.ToString("dd.MM.yyyy"),
                        ExamTypeName = e.ExamType != null ? e.ExamType.Name : "",
                        OrganizationName = e.Organization != null ? e.Organization.Name : "",
                        VehiclePassed = e.VehiclePassed,
                        ControllerUsername = e.FirstController != null ? e.FirstController.Username : ""
                    })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    ExamsGrid.ItemsSource = exams;
                    UpdatePagination();
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading exams:\n{ex.Message}", "Error",
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
        if (_currentPage > 1) { _currentPage--; LoadExams(SearchBox.Text); }
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
        var totalPages = (int)Math.Ceiling(_totalRecords / (double)_pageSize);
        if (_currentPage < totalPages) { _currentPage++; LoadExams(SearchBox.Text); }
    }

    private void Search_Click(object sender, RoutedEventArgs e)
    {
        _currentPage = 1;
        LoadExams(SearchBox.Text);
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            _searchTimer?.Stop();
            _currentPage = 1;
            LoadExams(SearchBox.Text);
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
            LoadExams(SearchBox.Text);
        };
        _searchTimer.Start();
    }

    private void NewExam_Click(object sender, RoutedEventArgs e)
    {
        NavigateToDetail(null);
    }

    private void ExamsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ExamsGrid.SelectedItem is ExamListItem item)
        {
            NavigateToDetail(item.Id);
        }
    }

    private void NavigateToDetail(long? examId)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new ExamDetailPage(examId);
        }
    }
}

public class ExamListItem
{
    public long Id { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string ExamDate { get; set; } = string.Empty;
    public string ValidUntilDate { get; set; } = string.Empty;
    public string ExamTypeName { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public bool VehiclePassed { get; set; }
    public string ControllerUsername { get; set; } = string.Empty;
}
