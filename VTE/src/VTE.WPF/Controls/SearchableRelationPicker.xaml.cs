namespace VTE.WPF.Controls;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Infrastructure.Data;

public partial class SearchableRelationPicker : UserControl
{
    public long? SelectedRelationId { get; private set; }
    public string SelectedDisplay { get; private set; } = "";

    public event Action<long>? RelationSelected;

    public SearchableRelationPicker()
    {
        InitializeComponent();
    }

    public void SetValue(long? relationId, string displayText)
    {
        SelectedRelationId = relationId;
        SelectedDisplay = displayText;
        TxtSearch.Text = displayText;
        ResultsPanel.Visibility = Visibility.Collapsed;
    }

    private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) DoSearch();
    }

    private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Auto-search after 3+ characters
        if (TxtSearch.Text.Length >= 3) DoSearch();
        else ResultsPanel.Visibility = Visibility.Collapsed;
    }

    private void BtnSearch_Click(object sender, RoutedEventArgs e) => DoSearch();

    private void BtnClear_Click(object sender, RoutedEventArgs e)
    {
        SelectedRelationId = null;
        SelectedDisplay = "";
        TxtSearch.Text = "";
        ResultsPanel.Visibility = Visibility.Collapsed;
    }

    private async void DoSearch()
    {
        var searchText = TxtSearch.Text.Trim();
        if (string.IsNullOrEmpty(searchText)) return;

        try
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var term = searchText.ToLower();
                var results = db.CustomerVehicleRelations
                    .Include(r => r.Customer)
                    .Include(r => r.Vehicle)
                    .Where(r => r.EndDate == null &&
                        (r.Customer.FirstName.ToLower().Contains(term) ||
                         r.Customer.LastName.ToLower().Contains(term) ||
                         (r.Vehicle.LastRegistrationNumber != null && r.Vehicle.LastRegistrationNumber.ToLower().Contains(term)) ||
                         r.Vehicle.ShellNumber.ToLower().Contains(term)))
                    .Take(20)
                    .Select(r => new
                    {
                        r.Id,
                        Display = r.Customer.FirstName + " " + r.Customer.LastName + " - " +
                            (r.Vehicle.LastRegistrationNumber ?? r.Vehicle.ShellNumber)
                    })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    ResultsList.Items.Clear();
                    foreach (var r in results)
                    {
                        var btn = new Button
                        {
                            Content = r.Display,
                            Tag = r.Id,
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            Padding = new Thickness(10, 8, 10, 8),
                            Background = Brushes.Transparent,
                            BorderThickness = new Thickness(0),
                            Cursor = Cursors.Hand,
                            FontSize = 13
                        };
                        btn.SetResourceReference(ForegroundProperty, "TextSecondary");
                        btn.MouseEnter += (s, _) => ((Button)s!).SetResourceReference(BackgroundProperty, "CardBgHover");
                        btn.MouseLeave += (s, _) => ((Button)s!).Background = Brushes.Transparent;
                        btn.Click += (s, _) =>
                        {
                            var b = (Button)s!;
                            SelectedRelationId = (long)b.Tag;
                            SelectedDisplay = b.Content.ToString()!;
                            TxtSearch.Text = SelectedDisplay;
                            ResultsPanel.Visibility = Visibility.Collapsed;
                            RelationSelected?.Invoke(SelectedRelationId.Value);
                        };
                        ResultsList.Items.Add(btn);
                    }

                    if (results.Count == 0)
                    {
                        var noResult = new TextBlock
                        {
                            Text = "\u041d\u0435\u043c\u0430 \u0440\u0435\u0437\u0443\u043b\u0442\u0430\u0442\u0438",
                            Padding = new Thickness(10, 8, 10, 8),
                            FontSize = 13
                        };
                        noResult.SetResourceReference(ForegroundProperty, "TextMuted");
                        ResultsList.Items.Add(noResult);
                    }

                    ResultsPanel.Visibility = Visibility.Visible;
                });
            });
        }
        catch { }
    }
}
