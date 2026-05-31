namespace VTE.WPF.Views.Pages;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class VehicleDetailPage : UserControl
{
    private readonly long? _vehicleId;
    private Vehicle? _vehicle;

    public VehicleDetailPage(long? vehicleId)
    {
        _vehicleId = vehicleId;
        InitializeComponent();
        ApplyLanguage();
        Loaded += async (_, _) =>
        {
            await LoadDropdownsAsync();
            LoadVehicle();
        };
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        LblRegistration.Text = Strings.Registration;
        LblShellNumber.Text = Strings.ShellNumberRequired;
        LblEngineNumber.Text = Strings.EngineNumber;
        LblFirstRegNumber.Text = Strings.FirstRegistrationNumber;
        LblFirstRegDate.Text = Strings.FirstRegistrationDate;
        LblLastRegNumber.Text = Strings.LastRegistrationNumber;
        LblLastRegDate.Text = Strings.LastRegistrationDate;
        LblMakeDate.Text = Strings.MakeDate;
        LblClassification.Text = Strings.Classification;
        LblVehicleModel.Text = Strings.VehicleModel;
        LblCategory.Text = Strings.VehicleCategory;
        LblPaymentCategory.Text = Strings.PaymentCategoryField;
        LblUseType.Text = Strings.UseType;
        LblBodyType.Text = Strings.BodyType;
        LblEngine.Text = Strings.Engine;
        LblEngineType.Text = Strings.EngineType;
        LblEnginePower.Text = Strings.EnginePowerKW;
        LblEngineTorque.Text = Strings.EngineTorqueNM;
        LblEngineCapacity.Text = Strings.EngineCapacity;
        LblDimensions.Text = Strings.Dimensions;
        LblHeight.Text = Strings.HeightMM;
        LblWidth.Text = Strings.WidthMM;
        LblLength.Text = Strings.LengthMM;
        LblEmptyWeight.Text = Strings.EmptyWeightKG;
        LblMaxWeight.Text = Strings.MaxWeightKG;
        LblSeating.Text = Strings.Seating;
        LblNumberOfDoors.Text = Strings.NumberOfDoors;
        LblNumberOfSeats.Text = Strings.NumberOfSeats;
        LblOther.Text = Strings.Other;
        LblMaxSpeed.Text = Strings.MaxSpeedKMH;
        ChkHasLPG.Content = Strings.HasLPG;
        ChkHasHook.Content = Strings.HasHook;
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
    }

    private System.Threading.Tasks.Task LoadDropdownsAsync()
    {
        // Use global cache — no DB query needed
        PickerVehicleModel.SetItems(Services.LookupCache.VehicleModels);
        PickerCategory.SetItems(Services.LookupCache.VehicleCategories);
        PickerPaymentCategory.SetItems(Services.LookupCache.VehiclePaymentCategories);
        PickerUseType.SetItems(Services.LookupCache.VehicleUseTypes);
        PickerBodyType.SetItems(Services.LookupCache.VehicleBodyTypes);
        PickerEngineType.SetItems(Services.LookupCache.EngineTypes);
        PickerEcoProgram.SetItems(Services.LookupCache.EcoPrograms);
        PickerPrimaryFuel.SetItems(Services.LookupCache.EnginePowerSourceTypes);
        PickerSecondaryFuel.SetItems(Services.LookupCache.EnginePowerSourceTypes);
        return System.Threading.Tasks.Task.CompletedTask;
    }


    private void LoadVehicle()
    {
        try
        {
            if (_vehicleId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _vehicle = db.Vehicles.AsNoTracking().FirstOrDefault(v => v.Id == _vehicleId.Value);

                if (_vehicle == null)
                {
                    MessageBox.Show("Vehicle not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                PageTitle.Text = $"{Strings.EditVehicleTitle} - {_vehicle.ShellNumber}";
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _vehicle = new Vehicle();
                PageTitle.Text = Strings.NewVehicleTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading vehicle:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_vehicle == null) return;

        // Registration
        TxtShellNumber.Text = _vehicle.ShellNumber;
        TxtEngineNumber.Text = _vehicle.EngineNumber ?? "";
        TxtFirstRegistrationNumber.Text = _vehicle.FirstRegistrationNumber ?? "";
        DpFirstRegistrationDate.SelectedDate = _vehicle.FirstRegistrationDate;
        TxtLastRegistrationNumber.Text = _vehicle.LastRegistrationNumber ?? "";
        DpLastRegistrationDate.SelectedDate = _vehicle.LastRegistrationDate;
        DpMakeDate.SelectedDate = _vehicle.MakeDate;

        // Classification — use global cache for name lookup
        PickerVehicleModel.SetValue(_vehicle.VehicleModelId, Services.LookupCache.GetName(Services.LookupCache.VehicleModels, _vehicle.VehicleModelId));
        PickerCategory.SetValue(_vehicle.CategoryId, Services.LookupCache.GetName(Services.LookupCache.VehicleCategories, _vehicle.CategoryId));
        PickerPaymentCategory.SetValue(_vehicle.PaymentCategoryId, Services.LookupCache.GetName(Services.LookupCache.VehiclePaymentCategories, _vehicle.PaymentCategoryId));
        PickerUseType.SetValue(_vehicle.UseTypeId, Services.LookupCache.GetName(Services.LookupCache.VehicleUseTypes, _vehicle.UseTypeId));
        PickerBodyType.SetValue(_vehicle.BodyTypeId, Services.LookupCache.GetName(Services.LookupCache.VehicleBodyTypes, _vehicle.BodyTypeId));
        PickerEngineType.SetValue(_vehicle.EngineTypeId, Services.LookupCache.GetName(Services.LookupCache.EngineTypes, _vehicle.EngineTypeId));
        TxtEngineNumber2.Text = _vehicle.EngineNumber ?? "";
        TxtEnginePowerKW.Text = _vehicle.EnginePowerKW?.ToString() ?? "";
        TxtEngineTorqueNM.Text = _vehicle.EngineTorqueNM?.ToString() ?? "";
        TxtEngineWorkingCapacityCM3.Text = _vehicle.EngineWorkingCapacityCM3?.ToString() ?? "";
        PickerEcoProgram.SetValue(_vehicle.EcoProgramId, Services.LookupCache.GetName(Services.LookupCache.EcoPrograms, _vehicle.EcoProgramId));
        PickerPrimaryFuel.SetValue(_vehicle.PrimaryPowerSourceId, Services.LookupCache.GetName(Services.LookupCache.EnginePowerSourceTypes, _vehicle.PrimaryPowerSourceId));
        PickerSecondaryFuel.SetValue(_vehicle.SecondaryPowerSourceId, Services.LookupCache.GetName(Services.LookupCache.EnginePowerSourceTypes, _vehicle.SecondaryPowerSourceId));

        // Dimensions
        TxtHeightMM.Text = _vehicle.HeightMM?.ToString() ?? "";
        TxtWidthMM.Text = _vehicle.WidthMM?.ToString() ?? "";
        TxtLengthMM.Text = _vehicle.LengthMM?.ToString() ?? "";
        TxtEmptyWeightKG.Text = _vehicle.EmptyWeightKG?.ToString() ?? "";
        TxtMaxAllowedWeightKG.Text = _vehicle.MaxAllowedWeightKG?.ToString() ?? "";

        // Seating & Axles
        TxtNumberOfDoors.Text = _vehicle.NumberOfDoors?.ToString() ?? "";
        TxtNumberOfSeats.Text = _vehicle.NumberOfSeats?.ToString() ?? "";
        TxtNumberOfAxles.Text = _vehicle.NumberOfAxles?.ToString() ?? "";
        TxtNumberOfWheels.Text = _vehicle.NumberOfWheels?.ToString() ?? "";

        // Other
        TxtMaxSpeedKMH.Text = _vehicle.MaxSpeedKMH?.ToString() ?? "";
        ChkHasLPG.IsChecked = _vehicle.HasLPG;
        ChkHasHook.IsChecked = _vehicle.HasHook;
    }

    private void ReadFields()
    {
        if (_vehicle == null) return;

        // Registration
        _vehicle.ShellNumber = TxtShellNumber.Text.Trim();
        _vehicle.EngineNumber = NullIfEmpty(TxtEngineNumber.Text);
        _vehicle.FirstRegistrationNumber = NullIfEmpty(TxtFirstRegistrationNumber.Text);
        _vehicle.FirstRegistrationDate = DpFirstRegistrationDate.SelectedDate;
        _vehicle.LastRegistrationNumber = NullIfEmpty(TxtLastRegistrationNumber.Text);
        _vehicle.LastRegistrationDate = DpLastRegistrationDate.SelectedDate;
        _vehicle.MakeDate = DpMakeDate.SelectedDate;

        // Classification
        _vehicle.VehicleModelId = PickerVehicleModel.SelectedId;
        _vehicle.CategoryId = PickerCategory.SelectedId;
        _vehicle.PaymentCategoryId = PickerPaymentCategory.SelectedId;
        _vehicle.UseTypeId = PickerUseType.SelectedId;
        _vehicle.BodyTypeId = PickerBodyType.SelectedId;

        // Engine
        _vehicle.EngineTypeId = PickerEngineType.SelectedId;
        _vehicle.EnginePowerKW = ParseDecimalOrNull(TxtEnginePowerKW.Text);
        _vehicle.EngineTorqueNM = ParseDecimalOrNull(TxtEngineTorqueNM.Text);
        _vehicle.EngineWorkingCapacityCM3 = ParseDecimalOrNull(TxtEngineWorkingCapacityCM3.Text);
        _vehicle.EcoProgramId = PickerEcoProgram.SelectedId;
        _vehicle.PrimaryPowerSourceId = PickerPrimaryFuel.SelectedId;
        _vehicle.SecondaryPowerSourceId = PickerSecondaryFuel.SelectedId;

        // Dimensions
        _vehicle.HeightMM = ParseIntOrNull(TxtHeightMM.Text);
        _vehicle.WidthMM = ParseIntOrNull(TxtWidthMM.Text);
        _vehicle.LengthMM = ParseIntOrNull(TxtLengthMM.Text);
        _vehicle.EmptyWeightKG = ParseDecimalOrNull(TxtEmptyWeightKG.Text);
        _vehicle.MaxAllowedWeightKG = ParseDecimalOrNull(TxtMaxAllowedWeightKG.Text);

        // Seating & Axles
        _vehicle.NumberOfDoors = ParseIntOrNull(TxtNumberOfDoors.Text);
        _vehicle.NumberOfSeats = ParseIntOrNull(TxtNumberOfSeats.Text);
        _vehicle.NumberOfAxles = ParseIntOrNull(TxtNumberOfAxles.Text);
        _vehicle.NumberOfWheels = ParseIntOrNull(TxtNumberOfWheels.Text);

        // Other
        _vehicle.MaxSpeedKMH = ParseDecimalOrNull(TxtMaxSpeedKMH.Text);
        _vehicle.HasLPG = ChkHasLPG.IsChecked == true;
        _vehicle.HasHook = ChkHasHook.IsChecked == true;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ReadFields();

            if (string.IsNullOrWhiteSpace(_vehicle!.ShellNumber))
            {
                MessageBox.Show("Shell Number is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtShellNumber.Focus();
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_vehicleId.HasValue)
            {
                var existing = db.Vehicles.Find(_vehicleId.Value);
                if (existing == null)
                {
                    MessageBox.Show("Vehicle no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.ShellNumber = _vehicle.ShellNumber;
                existing.EngineNumber = _vehicle.EngineNumber;
                existing.FirstRegistrationNumber = _vehicle.FirstRegistrationNumber;
                existing.FirstRegistrationDate = _vehicle.FirstRegistrationDate;
                existing.LastRegistrationNumber = _vehicle.LastRegistrationNumber;
                existing.LastRegistrationDate = _vehicle.LastRegistrationDate;
                existing.MakeDate = _vehicle.MakeDate;
                existing.VehicleModelId = _vehicle.VehicleModelId;
                existing.CategoryId = _vehicle.CategoryId;
                existing.PaymentCategoryId = _vehicle.PaymentCategoryId;
                existing.UseTypeId = _vehicle.UseTypeId;
                existing.BodyTypeId = _vehicle.BodyTypeId;
                existing.EngineTypeId = _vehicle.EngineTypeId;
                existing.EnginePowerKW = _vehicle.EnginePowerKW;
                existing.EngineTorqueNM = _vehicle.EngineTorqueNM;
                existing.EngineWorkingCapacityCM3 = _vehicle.EngineWorkingCapacityCM3;
                existing.HeightMM = _vehicle.HeightMM;
                existing.WidthMM = _vehicle.WidthMM;
                existing.LengthMM = _vehicle.LengthMM;
                existing.EmptyWeightKG = _vehicle.EmptyWeightKG;
                existing.MaxAllowedWeightKG = _vehicle.MaxAllowedWeightKG;
                existing.NumberOfDoors = _vehicle.NumberOfDoors;
                existing.NumberOfSeats = _vehicle.NumberOfSeats;
                existing.NumberOfAxles = _vehicle.NumberOfAxles;
                existing.NumberOfWheels = _vehicle.NumberOfWheels;
                existing.MaxSpeedKMH = _vehicle.MaxSpeedKMH;
                existing.HasLPG = _vehicle.HasLPG;
                existing.HasHook = _vehicle.HasHook;
                existing.ModifiedAt = DateTime.UtcNow;
            }
            else
            {
                _vehicle.CreatedAt = DateTime.UtcNow;
                db.Vehicles.Add(_vehicle);
            }

            db.SaveChanges();

            MessageBox.Show("Vehicle saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving vehicle:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (!_vehicleId.HasValue) return;

        var result = MessageBox.Show(
            "Are you sure you want to delete this vehicle?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var vehicle = db.Vehicles.Find(_vehicleId.Value);
            if (vehicle != null)
            {
                db.Vehicles.Remove(vehicle);
                db.SaveChanges();
            }

            MessageBox.Show("Vehicle deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting vehicle:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private static void SetPickerFromItems(Controls.SearchableLookupPicker picker, long? id)
    {
        if (!id.HasValue) return;
        // Items are already set via SetItems — find the name
        var field = typeof(Controls.SearchableLookupPicker).GetField("_allItems",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field?.GetValue(picker) is List<Controls.LookupItem> items)
        {
            var match = items.Find(i => i.Id == id.Value);
            if (match != null) picker.SetValue(id, match.Name);
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
            mainWindow.ContentArea.Content = new VehicleListPage();
        }
    }

    private static string? NullIfEmpty(string text)
    {
        var trimmed = text.Trim();
        return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
    }

    private static long? ParseLongOrNull(string text)
    {
        return long.TryParse(text.Trim(), out var val) ? val : null;
    }

    private static int? ParseIntOrNull(string text)
    {
        return int.TryParse(text.Trim(), out var val) ? val : null;
    }

    private static decimal? ParseDecimalOrNull(string text)
    {
        return decimal.TryParse(text.Trim(), out var val) ? val : null;
    }
}
