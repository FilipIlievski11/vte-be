namespace VTE.WPF.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VTE.Core.Interfaces;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ICurrentUserService _currentUserService;

    [ObservableProperty]
    private string _title = "VTE";

    [ObservableProperty]
    private string _currentUser = string.Empty;

    [ObservableProperty]
    private int _selectedTabIndex;

    public ObservableCollection<TabItemViewModel> OpenTabs { get; } = [];

    public MainWindowViewModel(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
        CurrentUser = currentUserService.Username ?? "Unknown";
    }

    public void AddTab(string header, object content)
    {
        var existing = OpenTabs.FirstOrDefault(t => t.Header == header);
        if (existing is not null)
        {
            SelectedTabIndex = OpenTabs.IndexOf(existing);
            return;
        }

        var tab = new TabItemViewModel { Header = header, Content = content };
        OpenTabs.Add(tab);
        SelectedTabIndex = OpenTabs.Count - 1;
    }

    [RelayCommand]
    private void CloseTab(TabItemViewModel tab)
    {
        OpenTabs.Remove(tab);
    }
}

public partial class TabItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string _header = string.Empty;

    [ObservableProperty]
    private object? _content;
}
