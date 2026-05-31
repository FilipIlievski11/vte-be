namespace VTE.WPF.Controls;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

public partial class SearchableLookupPicker : UserControl
{
    public long? SelectedId { get; private set; }
    public string SelectedName { get; private set; } = "";

    private List<LookupItem> _allItems = [];
    private List<LookupItem> _currentMatches = [];
    private bool _suppressSearch;
    private bool _isOpen;
    private int _loadedCount;
    private const int PageSize = 50;

    public event Action<long>? ItemSelected;

    public SearchableLookupPicker()
    {
        InitializeComponent();
        LostFocus += (_, _) =>
        {
            // Delay close so click on result can register
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!IsKeyboardFocusWithin && _isOpen)
                {
                    ResultsPanel.Visibility = Visibility.Collapsed;
                    _isOpen = false;
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        };
    }

    public void SetItems(List<LookupItem> items)
    {
        _allItems = items;
    }

    public void SetValue(long? id, string name)
    {
        SelectedId = id;
        SelectedName = name;
        _suppressSearch = true;
        TxtSearch.Text = name;
        _suppressSearch = false;
        ResultsPanel.Visibility = Visibility.Collapsed;
        _isOpen = false;
    }

    private void TxtSearch_GotFocus(object sender, RoutedEventArgs e)
    {
        if (!_isOpen && _allItems.Count > 0)
            ShowResults(TxtSearch.Text.Trim());
    }

    private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            ResultsPanel.Visibility = Visibility.Collapsed;
            _isOpen = false;
        }
    }

    private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_suppressSearch) return;
        ShowResults(TxtSearch.Text.Trim());
    }

    private void BtnToggle_Click(object sender, RoutedEventArgs e)
    {
        if (_isOpen)
        {
            ResultsPanel.Visibility = Visibility.Collapsed;
            _isOpen = false;
        }
        else
        {
            ShowResults(TxtSearch.Text.Trim());
            TxtSearch.Focus();
        }
    }

    private void ResultsScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        // Infinite scroll — load more when scrolled past 80% of content
        if (e.ExtentHeight > 0 && _loadedCount < _currentMatches.Count &&
            e.VerticalOffset + e.ViewportHeight >= e.ExtentHeight * 0.8)
        {
            LoadMoreItems();
        }
    }

    private void ShowResults(string filter)
    {
        var term = filter.ToLower();

        if (string.IsNullOrEmpty(term))
            _currentMatches = new List<LookupItem>(_allItems);
        else
            _currentMatches = _allItems.FindAll(i => i.Name.ToLower().Contains(term));

        ResultsList.Children.Clear();
        _loadedCount = 0;
        LoadMoreItems();

        ResultsPanel.Visibility = Visibility.Visible;
        _isOpen = true;
    }

    private void LoadMoreItems()
    {
        if (_loadedCount >= _currentMatches.Count) return;

        var end = Math.Min(_loadedCount + PageSize, _currentMatches.Count);
        for (int i = _loadedCount; i < end; i++)
        {
            var item = _currentMatches[i];
            var btn = new Button
            {
                Content = item.Name,
                Tag = item.Id,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(10, 6, 10, 6),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
                FontSize = 13
            };
            btn.SetResourceReference(ForegroundProperty, "TextSecondary");
            btn.MouseEnter += (s, _) => ((Button)s).SetResourceReference(BackgroundProperty, "CardBgHover");
            btn.MouseLeave += (s, _) => ((Button)s).Background = Brushes.Transparent;
            btn.Click += SelectItem;
            ResultsList.Children.Add(btn);
        }
        _loadedCount = end;

        if (_currentMatches.Count == 0 && _loadedCount == 0)
        {
            var noResult = new TextBlock
            {
                Text = "Нема резултати",
                Padding = new Thickness(10, 6, 10, 6),
                FontSize = 13
            };
            noResult.SetResourceReference(ForegroundProperty, "TextMuted");
            ResultsList.Children.Add(noResult);
        }
    }

    private void SelectItem(object sender, RoutedEventArgs e)
    {
        var btn = (Button)sender;
        SelectedId = (long)btn.Tag;
        SelectedName = btn.Content.ToString()!;
        _suppressSearch = true;
        TxtSearch.Text = SelectedName;
        _suppressSearch = false;
        ResultsPanel.Visibility = Visibility.Collapsed;
        _isOpen = false;
        ItemSelected?.Invoke(SelectedId.Value);
    }
}

public class LookupItem
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
}
