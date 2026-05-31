namespace VTE.WPF.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private string _welcomeMessage = "Welcome to VTE";

    [ObservableProperty]
    private int _totalCustomers;

    [ObservableProperty]
    private int _totalVehicles;

    [ObservableProperty]
    private int _todayExams;

    [ObservableProperty]
    private int _pendingPayments;
}
