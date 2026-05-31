namespace VTE.WPF.Views.Pages;

using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Core.Lookups;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class RequestWizardPage : UserControl
{
    private int _currentStep = 1;
    private const int TotalSteps = 5;

    // Selected data across steps
    private RequestType? _selectedRequestType;
    private List<CvrDisplayItem>? _cvrDisplayItems;
    private long _selectedCustomerVehicleRelationId;
    private string _selectedCustomerName = string.Empty;
    private string _selectedVehicleRegNumber = string.Empty;
    private long _selectedCustomerId;
    private long _selectedVehicleId;
    private TechnicalExamReport? _existingExam;

    public RequestWizardPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadStep1Data();
        PickerCustomerVehicleRelation.ItemSelected += OnCustomerVehicleRelationSelected;
        UpdateStepUI();
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        PageTitle.Text = Strings.NewRequestTitle;
        LblStep1Title.Text = Strings.SelectRequestType;
        LblStep2Title.Text = Strings.SelectCustomerVehicle;
        LblStep3Title.Text = Strings.DocumentChecklist;
        LblStep4Title.Text = Strings.TechnicalExamCheck;
        LblStep5Title.Text = Strings.SummaryAndComplete;
        BtnNext.Content = Strings.NextStep;
        BtnPrevious.Content = Strings.PreviousStep;
        BtnComplete.Content = Strings.CompleteRequest;
        LblSearchCustomer.Text = Strings.SearchCustomer;
        LblSearchVehicle.Text = Strings.SearchVehicle;
        BtnSearchCustomer.Content = Strings.Search;
        BtnSearchVehicle.Content = Strings.Search;
        ChkOwnershipProof.Content = Strings.OwnershipProof;
        ChkPaymentProof.Content = Strings.PaymentProof;
        LblDocNote.Text = Strings.Note;
        BtnCreateExam.Content = Strings.CreateNewExam;
        LblExistingRelation.Text = Strings.CustomerVehicleRelation;
        LblSumRequestType.Text = Strings.RequestType;
        LblSumCustomer.Text = Strings.Customer;
        LblSumVehicle.Text = Strings.VehicleLabel;
        LblSumDocuments.Text = Strings.DocumentChecklist;
        LblSumExam.Text = Strings.ExamStatus;
        LblSumNote.Text = Strings.Note;
    }

    // ======================== Step Navigation ========================

    private void UpdateStepUI()
    {
        // Step indicator text
        string stepName = _currentStep switch
        {
            1 => Strings.SelectRequestType,
            2 => Strings.SelectCustomerVehicle,
            3 => Strings.DocumentChecklist,
            4 => Strings.TechnicalExamCheck,
            5 => Strings.SummaryAndComplete,
            _ => ""
        };
        TxtStepIndicator.Text = $"{Strings.Step} {_currentStep} {Strings.Of2} {GetEffectiveTotalSteps()} \u2014 {stepName}";

        // Show/hide panels
        Step1Panel.Visibility = _currentStep == 1 ? Visibility.Visible : Visibility.Collapsed;
        Step2Panel.Visibility = _currentStep == 2 ? Visibility.Visible : Visibility.Collapsed;
        Step3Panel.Visibility = _currentStep == 3 ? Visibility.Visible : Visibility.Collapsed;
        Step4Panel.Visibility = _currentStep == 4 ? Visibility.Visible : Visibility.Collapsed;
        Step5Panel.Visibility = _currentStep == 5 ? Visibility.Visible : Visibility.Collapsed;

        // Show/hide navigation buttons
        BtnPrevious.Visibility = _currentStep > 1 ? Visibility.Visible : Visibility.Collapsed;
        BtnNext.Visibility = _currentStep < 5 ? Visibility.Visible : Visibility.Collapsed;
        BtnComplete.Visibility = _currentStep == 5 ? Visibility.Visible : Visibility.Collapsed;
    }

    private int GetEffectiveTotalSteps()
    {
        // Always show 5 steps; steps 3 and 4 may be skipped but still count
        return TotalSteps;
    }

    private int GetNextStep(int current)
    {
        int next = current + 1;
        // Skip step 3 if no documents needed (we always show it, but could skip)
        // Skip step 4 if technical exam is not required
        if (next == 4 && _selectedRequestType != null && !_selectedRequestType.IsTehnicalExamRequired)
            next = 5;
        return next;
    }

    private int GetPrevStep(int current)
    {
        int prev = current - 1;
        if (prev == 4 && _selectedRequestType != null && !_selectedRequestType.IsTehnicalExamRequired)
            prev = 3;
        return prev;
    }

    private void Next_Click(object sender, RoutedEventArgs e)
    {
        if (!ValidateCurrentStep()) return;

        _currentStep = GetNextStep(_currentStep);
        if (_currentStep > TotalSteps) _currentStep = TotalSteps;

        OnStepEntered();
        UpdateStepUI();
    }

    private void Previous_Click(object sender, RoutedEventArgs e)
    {
        _currentStep = GetPrevStep(_currentStep);
        if (_currentStep < 1) _currentStep = 1;

        UpdateStepUI();
    }

    private bool ValidateCurrentStep()
    {
        switch (_currentStep)
        {
            case 1:
                if (_selectedRequestType == null)
                {
                    MessageBox.Show(Strings.FieldRequired, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;

            case 2:
                if (_selectedCustomerVehicleRelationId == 0)
                {
                    MessageBox.Show(Strings.FieldRequired, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;

            default:
                return true;
        }
    }

    private void OnStepEntered()
    {
        switch (_currentStep)
        {
            case 2:
                LoadStep2Data();
                break;
            case 4:
                LoadStep4Data();
                break;
            case 5:
                PopulateSummary();
                break;
        }
    }

    // ======================== Step 1: Request Type ========================

    private void LoadStep1Data()
    {
        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var types = db.RequestTypes.Where(rt => rt.IsActive).OrderBy(rt => rt.Name).ToList();
            RequestTypesGrid.ItemsSource = types;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{Strings.Error}:\n{ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RequestTypesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (RequestTypesGrid.SelectedItem is RequestType rt)
        {
            _selectedRequestType = rt;
            TxtFlagTechExam.Text = $"{Strings.TechExamRequired}: {(rt.IsTehnicalExamRequired ? Strings.YesText : Strings.NoText)}";
            TxtFlagPayRequired.Text = $"{Strings.PaymentRequired}: {(rt.IsPayRequired ? Strings.YesText : Strings.NoText)}";
            TxtFlagNewRegistration.Text = $"{Strings.NewRegistration}: {(rt.IsNewRegistration ? Strings.YesText : Strings.NoText)}";
        }
    }

    // ======================== Step 2: Customer & Vehicle ========================

    private void LoadStep2Data()
    {
        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var relations = db.CustomerVehicleRelations
                .Include(r => r.Customer)
                .Include(r => r.Vehicle)
                .Where(r => r.EndDate == null)
                .OrderBy(r => r.Customer.FirstName)
                .ThenBy(r => r.Customer.LastName)
                .ToList()
                .Select(r => new CvrDisplayItem
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    VehicleId = r.VehicleId,
                    CustomerName = $"{r.Customer.FirstName} {r.Customer.LastName}",
                    VehicleRegNumber = r.Vehicle.LastRegistrationNumber ?? "N/A",
                    DisplayText = $"{r.Customer.FirstName} {r.Customer.LastName} - {r.Vehicle.LastRegistrationNumber ?? "N/A"}"
                })
                .ToList();

            _cvrDisplayItems = relations;
            var lookupItems = relations.Select(r => new Controls.LookupItem { Id = r.Id, Name = r.DisplayText }).ToList();
            PickerCustomerVehicleRelation.SetItems(lookupItems);

            // Pre-select if already chosen
            if (_selectedCustomerVehicleRelationId > 0)
            {
                var name = relations.Find(r => r.Id == _selectedCustomerVehicleRelationId)?.DisplayText ?? "";
                PickerCustomerVehicleRelation.SetValue(_selectedCustomerVehicleRelationId, name);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{Strings.Error}:\n{ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void OnCustomerVehicleRelationSelected(long selectedId)
    {
        var item = _cvrDisplayItems?.Find(r => r.Id == selectedId);
        if (item != null)
        {
            _selectedCustomerVehicleRelationId = item.Id;
            _selectedCustomerName = item.CustomerName;
            _selectedVehicleRegNumber = item.VehicleRegNumber;
            _selectedCustomerId = item.CustomerId;
            _selectedVehicleId = item.VehicleId;
        }
    }

    private async void SearchCustomer_Click(object sender, RoutedEventArgs e)
    {
        await SearchCustomersAsync();
    }

    private async void CustomerSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) await SearchCustomersAsync();
    }

    private async Task SearchCustomersAsync()
    {
        var term = TxtCustomerSearch.Text?.Trim();
        if (string.IsNullOrEmpty(term)) return;

        try
        {
            var results = await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                return db.Customers
                    .Where(c => c.FirstName.Contains(term) || c.LastName.Contains(term) || c.IdentificationNumber.Contains(term))
                    .OrderBy(c => c.FirstName).ThenBy(c => c.LastName)
                    .Take(50)
                    .Select(c => new CustomerSearchItem
                    {
                        Id = c.Id,
                        FullName = c.FirstName + " " + c.LastName,
                        IdentificationNumber = c.IdentificationNumber
                    })
                    .ToList();
            });

            CustomersGrid.ItemsSource = results;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{Strings.Error}:\n{ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void SearchVehicle_Click(object sender, RoutedEventArgs e)
    {
        await SearchVehiclesAsync();
    }

    private async void VehicleSearch_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) await SearchVehiclesAsync();
    }

    private async Task SearchVehiclesAsync()
    {
        var term = TxtVehicleSearch.Text?.Trim();
        if (string.IsNullOrEmpty(term)) return;

        try
        {
            var results = await Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                return db.Vehicles
                    .Where(v => (v.LastRegistrationNumber != null && v.LastRegistrationNumber.Contains(term)) ||
                                v.ShellNumber.Contains(term))
                    .OrderBy(v => v.LastRegistrationNumber)
                    .Take(50)
                    .Select(v => new VehicleSearchItem
                    {
                        Id = v.Id,
                        RegNumber = v.LastRegistrationNumber ?? "",
                        ShellNumber = v.ShellNumber
                    })
                    .ToList();
            });

            VehiclesGrid.ItemsSource = results;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{Strings.Error}:\n{ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // ======================== Step 4: Technical Exam ========================

    private void LoadStep4Data()
    {
        _existingExam = null;
        ExamDetailsBorder.Visibility = Visibility.Collapsed;
        BtnCreateExam.Visibility = Visibility.Collapsed;

        if (_selectedCustomerVehicleRelationId == 0) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var exam = db.TechnicalExamReports
                .Where(e => e.CustomerVehicleRelationId == _selectedCustomerVehicleRelationId)
                .OrderByDescending(e => e.ExamDate)
                .FirstOrDefault();

            if (exam != null)
            {
                _existingExam = exam;
                TxtExamInfo.Text = "";
                ExamDetailsBorder.Visibility = Visibility.Visible;
                TxtExamDate.Text = $"{Strings.ExamDate}: {exam.ExamDate:dd.MM.yyyy}";
                TxtExamValidUntil.Text = $"{Strings.ValidUntil}: {exam.ValidUntilDate:dd.MM.yyyy}";
                TxtExamResult.Text = $"{Strings.ExamResult}: {(exam.VehiclePassed ? Strings.Passed : Strings.Failed)}";
                TxtExamRegNumber.Text = $"{Strings.RegistrationNumber}: {exam.RegistrationNumber}";
            }
            else
            {
                TxtExamInfo.Text = Strings.NoTechExam;
                BtnCreateExam.Visibility = Visibility.Visible;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{Strings.Error}:\n{ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CreateExam_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
            mainWindow.ContentArea.Content = new ExamDetailPage(null);
    }

    // ======================== Step 5: Summary ========================

    private void PopulateSummary()
    {
        TxtSumRequestType.Text = _selectedRequestType?.Name ?? "-";
        TxtSumCustomer.Text = !string.IsNullOrEmpty(_selectedCustomerName) ? _selectedCustomerName : "-";
        TxtSumVehicle.Text = !string.IsNullOrEmpty(_selectedVehicleRegNumber) ? _selectedVehicleRegNumber : "-";

        var docs = new System.Collections.Generic.List<string>();
        if (ChkOwnershipProof.IsChecked == true) docs.Add(Strings.OwnershipProof);
        if (ChkPaymentProof.IsChecked == true) docs.Add(Strings.PaymentProof);
        TxtSumDocuments.Text = docs.Count > 0 ? string.Join(", ", docs) : "-";

        if (_selectedRequestType?.IsTehnicalExamRequired == true)
        {
            TxtSumExam.Text = _existingExam != null
                ? $"{Strings.Passed}: {(_existingExam.VehiclePassed ? Strings.YesText : Strings.NoText)} ({_existingExam.ExamDate:dd.MM.yyyy})"
                : Strings.NoTechExam;
        }
        else
        {
            TxtSumExam.Text = "-";
        }

        TxtSumNote.Text = !string.IsNullOrWhiteSpace(TxtDocNote.Text) ? TxtDocNote.Text.Trim() : "-";
    }

    // ======================== Complete ========================

    private void Complete_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var request = new Request
            {
                RequestTypeId = _selectedRequestType!.Id,
                CustomerVehicleRelationId = _selectedCustomerVehicleRelationId,
                TechnicalExamReportId = _existingExam?.Id,
                IsCustomerChanged = _selectedRequestType.IsCustomerChanged,
                IsVehicleChanged = _selectedRequestType.IsVehicleChanged,
                Note = string.IsNullOrWhiteSpace(TxtDocNote.Text) ? null : TxtDocNote.Text.Trim(),
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = 1
            };

            db.Requests.Add(request);
            db.SaveChanges();

            MessageBox.Show(Strings.RequestCompleted, Strings.SavedSuccessfully, MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{Strings.Error}:\n{ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // ======================== Navigation ========================

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        NavigateBack();
    }

    private void NavigateBack()
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
            mainWindow.ContentArea.Content = new RequestListPage();
    }
}

// Helper classes for data binding
public class CvrDisplayItem
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public long VehicleId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string VehicleRegNumber { get; set; } = string.Empty;
    public string DisplayText { get; set; } = string.Empty;
}

public class CustomerSearchItem
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string IdentificationNumber { get; set; } = string.Empty;
}

public class VehicleSearchItem
{
    public long Id { get; set; }
    public string RegNumber { get; set; } = string.Empty;
    public string ShellNumber { get; set; } = string.Empty;
}
