namespace VTE.WPF.Views.Pages;

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class ExamDetailPage : UserControl
{
    private readonly long? _examId;
    private TechnicalExamReport? _exam;

    public ExamDetailPage(long? examId)
    {
        _examId = examId;
        InitializeComponent();
        ApplyLanguage();
        Loaded += async (_, _) =>
        {
            await LoadDropdownsAsync();
            LoadExam();
        };
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        LblGeneral.Text = Strings.General;
        LblRegistrationNumber.Text = Strings.RegistrationNumberRequired;
        LblExamDate.Text = Strings.ExamDateRequired;
        LblValidUntilDate.Text = Strings.ValidUntilDateRequired;
        ChkVehiclePassed.Content = Strings.VehiclePassed;
        LblExamType.Text = Strings.ExamTypeRequired;
        LblOrganization.Text = Strings.OrganizationRequired;
        LblFirstController.Text = Strings.FirstControllerRequired;
        LblRelation.Text = Strings.CustomerVehicleRelationRequired;
        LblBrakeAxle1.Text = Strings.BrakeMeasurementsAxle1;
        LblBrakeAxle2.Text = Strings.BrakeMeasurementsAxle2;
        LblBrakeEffectiveness.Text = Strings.BrakeEffectiveness;
        LblWorkingBrakeEmpty.Text = Strings.WorkingBrakeEmpty;
        LblWorkingBrakeFull.Text = Strings.WorkingBrakeFull;
        LblSecondaryBrake.Text = Strings.SecondaryBrake;
        LblParkingBrake.Text = Strings.ParkingBrake;
        LblVehicleWeight.Text = Strings.VehicleWeight;
        LblEmissions.Text = Strings.Emissions;
        LblEngineSpeed.Text = Strings.EngineSpeedRPM;
        LblEngineTurns.Text = Strings.EngineTurns;
        LblCOPlusTurns.Text = Strings.COPlusTurns;
        LblLambda.Text = Strings.Lambda;
        LblPinpoints.Text = Strings.Pinpoints;
        LblNoiseDB.Text = Strings.NoiseDB;
        LblEngineOilTemp.Text = Strings.EngineOilTemperature;
        LblNotes.Text = Strings.Notes;
        LblTechnicalChanges.Text = Strings.TechnicalChanges;
        LblExplanationNote.Text = Strings.ExplanationNote;
        LblDriverWarning.Text = Strings.DriverWarning;
        LblNote.Text = Strings.Note;
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
        BtnPrintReport.Content = Strings.PrintReport;
        BtnPrintCertificate.Content = Strings.PrintCertificate;
    }

    private System.Threading.Tasks.Task LoadDropdownsAsync()
    {
        PickerExamType.SetItems(Services.LookupCache.TechnicalExamTypes);
        PickerOrganization.SetItems(Services.LookupCache.Organizations);
        PickerFirstController.SetItems(Services.LookupCache.Users);
        return System.Threading.Tasks.Task.CompletedTask;
    }

    private void LoadExam()
    {
        try
        {
            if (_examId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _exam = db.TechnicalExamReports.AsNoTracking().FirstOrDefault(e => e.Id == _examId.Value);

                if (_exam == null)
                {
                    MessageBox.Show("Exam not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                PageTitle.Text = $"{Strings.EditExamTitle} - {_exam.RegistrationNumber}";
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _exam = new TechnicalExamReport();
                PageTitle.Text = Strings.NewExamTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading exam:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_exam == null) return;

        TxtRegistrationNumber.Text = _exam.RegistrationNumber;
        DpExamDate.SelectedDate = _exam.ExamDate;
        DpValidUntilDate.SelectedDate = _exam.ValidUntilDate;
        ChkVehiclePassed.IsChecked = _exam.VehiclePassed;
        PickerExamType.SetValue(_exam.ExamTypeId, Services.LookupCache.GetName(Services.LookupCache.TechnicalExamTypes, _exam.ExamTypeId));
        PickerOrganization.SetValue(_exam.OrganizationId, Services.LookupCache.GetName(Services.LookupCache.Organizations, _exam.OrganizationId));
        PickerFirstController.SetValue(_exam.FirstControllerId, Services.LookupCache.GetName(Services.LookupCache.Users, _exam.FirstControllerId));
        if (_exam.CustomerVehicleRelationId > 0)
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var rel = db.CustomerVehicleRelations
                .Include(r => r.Customer).Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == _exam.CustomerVehicleRelationId);
            if (rel != null)
                RelationPicker.SetValue(rel.Id,
                    $"{rel.Customer.FirstName} {rel.Customer.LastName} - {rel.Vehicle.LastRegistrationNumber ?? rel.Vehicle.ShellNumber}");
        }

        // Brake Measurements - Axle 1
        TxtAxle1BrakeLeftKN.Text = _exam.Axle1BrakeLeftKN?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtAxle1BrakeRightKN.Text = _exam.Axle1BrakeRightKN?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtAxle1BrakeGj.Text = _exam.Axle1BrakeGj?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtAxle1BrakeLeftPj.Text = _exam.Axle1BrakeLeftPj?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtAxle1BrakePN.Text = _exam.Axle1BrakePN?.ToString(CultureInfo.InvariantCulture) ?? "";

        // Brake Measurements - Axle 2
        TxtAxle2BrakeLeftKN.Text = _exam.Axle2BrakeLeftKN?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtAxle2BrakeRightKN.Text = _exam.Axle2BrakeRightKN?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtAxle2BrakeGj.Text = _exam.Axle2BrakeGj?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtAxle2BrakeLeftPj.Text = _exam.Axle2BrakeLeftPj?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtAxle2BrakePN.Text = _exam.Axle2BrakePN?.ToString(CultureInfo.InvariantCulture) ?? "";

        // Brake Effectiveness
        TxtWorkingBrakeEffectivenessEmpty.Text = _exam.WorkingBrakeEffectivenessEmpty?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtWorkingBrakeEffectivenessFull.Text = _exam.WorkingBrakeEffectivenessFull?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtSecondaryBrakeEffectiveness.Text = _exam.SecondaryBrakeEffectiveness?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtParkingBrakeEffectiveness.Text = _exam.ParkingBrakeEffectiveness?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtVehicleWeightKG.Text = _exam.VehicleWeightKG?.ToString(CultureInfo.InvariantCulture) ?? "";

        // Emissions
        TxtEngineSpeedRPM.Text = _exam.EngineSpeedRPM?.ToString() ?? "";
        TxtCO.Text = _exam.CO?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtEngineTurns.Text = _exam.EngineTurns?.ToString() ?? "";
        TxtCOPlusTurns.Text = _exam.COPlusTurns?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtLambda.Text = _exam.Lambda?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtPinpoints.Text = _exam.Pinpoints?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtNoiseDB.Text = _exam.NoiseDB?.ToString(CultureInfo.InvariantCulture) ?? "";
        TxtEngineOilTemperatureC.Text = _exam.EngineOilTemperatureC?.ToString(CultureInfo.InvariantCulture) ?? "";

        // Notes
        TxtTechnicalChanges.Text = _exam.TechnicalChanges ?? "";
        TxtExplanationNote.Text = _exam.ExplanationNote ?? "";
        TxtDriverWarning.Text = _exam.DriverWarning ?? "";
        TxtNote.Text = _exam.Note ?? "";
    }

    private static decimal? ParseNullableDecimal(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        return decimal.TryParse(text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var val) ? val : null;
    }

    private static int? ParseNullableInt(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        return int.TryParse(text.Trim(), out var val) ? val : (int?)null;
    }

    private void ReadFields()
    {
        if (_exam == null) return;

        _exam.RegistrationNumber = TxtRegistrationNumber.Text.Trim();
        _exam.ExamDate = DpExamDate.SelectedDate ?? DateTime.Today;
        _exam.ValidUntilDate = DpValidUntilDate.SelectedDate ?? DateTime.Today;
        _exam.VehiclePassed = ChkVehiclePassed.IsChecked == true;

        _exam.ExamTypeId = PickerExamType.SelectedId ?? 0;
        _exam.OrganizationId = PickerOrganization.SelectedId ?? 0;
        _exam.FirstControllerId = PickerFirstController.SelectedId ?? 0;
        if (RelationPicker.SelectedRelationId is long relationId)
            _exam.CustomerVehicleRelationId = relationId;

        // Brake Measurements - Axle 1
        _exam.Axle1BrakeLeftKN = ParseNullableDecimal(TxtAxle1BrakeLeftKN.Text);
        _exam.Axle1BrakeRightKN = ParseNullableDecimal(TxtAxle1BrakeRightKN.Text);
        _exam.Axle1BrakeGj = ParseNullableDecimal(TxtAxle1BrakeGj.Text);
        _exam.Axle1BrakeLeftPj = ParseNullableDecimal(TxtAxle1BrakeLeftPj.Text);
        _exam.Axle1BrakePN = ParseNullableDecimal(TxtAxle1BrakePN.Text);

        // Brake Measurements - Axle 2
        _exam.Axle2BrakeLeftKN = ParseNullableDecimal(TxtAxle2BrakeLeftKN.Text);
        _exam.Axle2BrakeRightKN = ParseNullableDecimal(TxtAxle2BrakeRightKN.Text);
        _exam.Axle2BrakeGj = ParseNullableDecimal(TxtAxle2BrakeGj.Text);
        _exam.Axle2BrakeLeftPj = ParseNullableDecimal(TxtAxle2BrakeLeftPj.Text);
        _exam.Axle2BrakePN = ParseNullableDecimal(TxtAxle2BrakePN.Text);

        // Brake Effectiveness
        _exam.WorkingBrakeEffectivenessEmpty = ParseNullableDecimal(TxtWorkingBrakeEffectivenessEmpty.Text);
        _exam.WorkingBrakeEffectivenessFull = ParseNullableDecimal(TxtWorkingBrakeEffectivenessFull.Text);
        _exam.SecondaryBrakeEffectiveness = ParseNullableDecimal(TxtSecondaryBrakeEffectiveness.Text);
        _exam.ParkingBrakeEffectiveness = ParseNullableDecimal(TxtParkingBrakeEffectiveness.Text);
        _exam.VehicleWeightKG = ParseNullableDecimal(TxtVehicleWeightKG.Text);

        // Emissions
        _exam.EngineSpeedRPM = ParseNullableInt(TxtEngineSpeedRPM.Text);
        _exam.CO = ParseNullableDecimal(TxtCO.Text);
        _exam.EngineTurns = ParseNullableInt(TxtEngineTurns.Text);
        _exam.COPlusTurns = ParseNullableDecimal(TxtCOPlusTurns.Text);
        _exam.Lambda = ParseNullableDecimal(TxtLambda.Text);
        _exam.Pinpoints = ParseNullableDecimal(TxtPinpoints.Text);
        _exam.NoiseDB = ParseNullableDecimal(TxtNoiseDB.Text);
        _exam.EngineOilTemperatureC = ParseNullableDecimal(TxtEngineOilTemperatureC.Text);

        // Notes
        _exam.TechnicalChanges = string.IsNullOrWhiteSpace(TxtTechnicalChanges.Text) ? null : TxtTechnicalChanges.Text.Trim();
        _exam.ExplanationNote = string.IsNullOrWhiteSpace(TxtExplanationNote.Text) ? null : TxtExplanationNote.Text.Trim();
        _exam.DriverWarning = string.IsNullOrWhiteSpace(TxtDriverWarning.Text) ? null : TxtDriverWarning.Text.Trim();
        _exam.Note = string.IsNullOrWhiteSpace(TxtNote.Text) ? null : TxtNote.Text.Trim();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ReadFields();

            if (string.IsNullOrWhiteSpace(_exam!.RegistrationNumber))
            {
                MessageBox.Show("Registration Number is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtRegistrationNumber.Focus();
                return;
            }

            if (_exam.CustomerVehicleRelationId <= 0)
            {
                MessageBox.Show("Customer Vehicle Relation is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                RelationPicker.Focus();
                return;
            }

            if (_exam.ExamTypeId <= 0)
            {
                MessageBox.Show("Exam Type is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                PickerExamType.Focus();
                return;
            }

            if (_exam.OrganizationId <= 0)
            {
                MessageBox.Show("Organization is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                PickerOrganization.Focus();
                return;
            }

            if (_exam.FirstControllerId <= 0)
            {
                MessageBox.Show("First Controller is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                PickerFirstController.Focus();
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_examId.HasValue)
            {
                var existing = db.TechnicalExamReports.Find(_examId.Value);
                if (existing == null)
                {
                    MessageBox.Show("Exam no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.RegistrationNumber = _exam.RegistrationNumber;
                existing.ExamDate = _exam.ExamDate;
                existing.ValidUntilDate = _exam.ValidUntilDate;
                existing.VehiclePassed = _exam.VehiclePassed;
                existing.ExamTypeId = _exam.ExamTypeId;
                existing.OrganizationId = _exam.OrganizationId;
                existing.FirstControllerId = _exam.FirstControllerId;
                existing.CustomerVehicleRelationId = _exam.CustomerVehicleRelationId;

                existing.Axle1BrakeLeftKN = _exam.Axle1BrakeLeftKN;
                existing.Axle1BrakeRightKN = _exam.Axle1BrakeRightKN;
                existing.Axle1BrakeGj = _exam.Axle1BrakeGj;
                existing.Axle1BrakeLeftPj = _exam.Axle1BrakeLeftPj;
                existing.Axle1BrakePN = _exam.Axle1BrakePN;

                existing.Axle2BrakeLeftKN = _exam.Axle2BrakeLeftKN;
                existing.Axle2BrakeRightKN = _exam.Axle2BrakeRightKN;
                existing.Axle2BrakeGj = _exam.Axle2BrakeGj;
                existing.Axle2BrakeLeftPj = _exam.Axle2BrakeLeftPj;
                existing.Axle2BrakePN = _exam.Axle2BrakePN;

                existing.WorkingBrakeEffectivenessEmpty = _exam.WorkingBrakeEffectivenessEmpty;
                existing.WorkingBrakeEffectivenessFull = _exam.WorkingBrakeEffectivenessFull;
                existing.SecondaryBrakeEffectiveness = _exam.SecondaryBrakeEffectiveness;
                existing.ParkingBrakeEffectiveness = _exam.ParkingBrakeEffectiveness;
                existing.VehicleWeightKG = _exam.VehicleWeightKG;

                existing.EngineSpeedRPM = _exam.EngineSpeedRPM;
                existing.CO = _exam.CO;
                existing.EngineTurns = _exam.EngineTurns;
                existing.COPlusTurns = _exam.COPlusTurns;
                existing.Lambda = _exam.Lambda;
                existing.Pinpoints = _exam.Pinpoints;
                existing.NoiseDB = _exam.NoiseDB;
                existing.EngineOilTemperatureC = _exam.EngineOilTemperatureC;

                existing.TechnicalChanges = _exam.TechnicalChanges;
                existing.ExplanationNote = _exam.ExplanationNote;
                existing.DriverWarning = _exam.DriverWarning;
                existing.Note = _exam.Note;
                existing.ModifiedAt = DateTime.UtcNow;
            }
            else
            {
                _exam.CreatedAt = DateTime.UtcNow;
                db.TechnicalExamReports.Add(_exam);
            }

            db.SaveChanges();

            MessageBox.Show("Exam saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving exam:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (!_examId.HasValue) return;

        var result = MessageBox.Show(
            "Are you sure you want to delete this exam?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var exam = db.TechnicalExamReports.Find(_examId.Value);
            if (exam != null)
            {
                db.TechnicalExamReports.Remove(exam);
                db.SaveChanges();
            }

            MessageBox.Show("Exam deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting exam:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void PrintReport_Click(object sender, RoutedEventArgs e)
    {
        if (_examId == null) { MessageBox.Show(Strings.SaveFirst); return; }
        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var exam = db.TechnicalExamReports
                .Include(e => e.CustomerVehicleRelation).ThenInclude(r => r.Customer)
                .Include(e => e.CustomerVehicleRelation).ThenInclude(r => r.Vehicle).ThenInclude(v => v.VehicleModel).ThenInclude(m => m!.VehicleMaker)
                .Include(e => e.CustomerVehicleRelation).ThenInclude(r => r.Vehicle).ThenInclude(v => v.Category)
                .Include(e => e.ExamType)
                .Include(e => e.Organization)
                .Include(e => e.FirstController)
                .Include(e => e.SecondController)
                .FirstOrDefault(e => e.Id == _examId);

            if (exam == null) return;

            var customer = exam.CustomerVehicleRelation.Customer;
            var vehicle = exam.CustomerVehicleRelation.Vehicle;

            var pdfBytes = Services.TechnicalExamPdf.Generate(
                exam, customer, vehicle,
                exam.Organization.Name, exam.Organization.Address,
                exam.ExamType.Name, exam.FirstController.FullName,
                exam.SecondController?.FullName);

            var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                $"ExamReport_{exam.RegistrationNumber.Replace("/", "_")}_{exam.Id}.pdf");
            System.IO.File.WriteAllBytes(tempPath, pdfBytes);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating PDF:\n{ex.Message}", Strings.Error);
        }
    }

    private void PrintCertificate_Click(object sender, RoutedEventArgs e)
    {
        if (_examId == null) { MessageBox.Show(Strings.SaveFirst); return; }
        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var exam = db.TechnicalExamReports
                .Include(e => e.CustomerVehicleRelation).ThenInclude(r => r.Customer)
                .Include(e => e.CustomerVehicleRelation).ThenInclude(r => r.Vehicle).ThenInclude(v => v.VehicleModel).ThenInclude(m => m!.VehicleMaker)
                .Include(e => e.CustomerVehicleRelation).ThenInclude(r => r.Vehicle).ThenInclude(v => v.Category)
                .Include(e => e.Organization).ThenInclude(o => o.Company)
                .Include(e => e.FirstController)
                .Include(e => e.SecondController)
                .FirstOrDefault(e => e.Id == _examId);

            if (exam == null) return;

            var customer = exam.CustomerVehicleRelation.Customer;
            var vehicle = exam.CustomerVehicleRelation.Vehicle;

            var pdfBytes = Services.TechnicalCertificatePdf.Generate(
                exam, customer, vehicle,
                exam.Organization.Name, exam.Organization.Address,
                exam.Organization.Company?.PhoneNumber,
                exam.FirstController.FullName, exam.SecondController?.FullName);

            var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                $"Certificate_{exam.RegistrationNumber.Replace("/", "_")}_{exam.Id}.pdf");
            System.IO.File.WriteAllBytes(tempPath, pdfBytes);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating PDF:\n{ex.Message}", Strings.Error);
        }
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
        NavigateBack();
    }

    private void NavigateBack()
    {
        var mainWindow = Window.GetWindow(this) as MainWindow;
        if (mainWindow != null)
        {
            mainWindow.ContentArea.Content = new ExamListPage();
        }
    }
}
