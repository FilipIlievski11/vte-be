namespace VTE.WPF.Views.Pages;

using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class RelationDetailPage : UserControl
{
    private readonly long? _relationId;
    private CustomerVehicleRelation? _relation;

    public RelationDetailPage(long? relationId)
    {
        _relationId = relationId;
        InitializeComponent();
        ApplyLanguage();
        Loaded += async (_, _) =>
        {
            await LoadDropdownsAsync();
            LoadRelation();
        };
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        LblCustomer.Text = Strings.Customer + " *";
        LblVehicle.Text = Strings.VehicleLabel + " *";
        LblRelationType.Text = Strings.RelationTypeField + " *";
        LblStartDate.Text = Strings.StartDate + " *";
        LblEndDate.Text = Strings.EndDate;
        LblBeginNote.Text = Strings.BeginNote;
        LblTerminationNote.Text = Strings.TerminationNote;
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
    }

    private System.Threading.Tasks.Task LoadDropdownsAsync()
    {
        PickerCustomer.SetItems(Services.LookupCache.Customers);
        PickerVehicle.SetItems(Services.LookupCache.Vehicles);
        PickerRelationType.SetItems(Services.LookupCache.RelationTypes);
        return System.Threading.Tasks.Task.CompletedTask;
    }

    private void LoadRelation()
    {
        try
        {
            if (_relationId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _relation = db.CustomerVehicleRelations
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Id == _relationId.Value);

                if (_relation == null)
                {
                    MessageBox.Show("Relation not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                PageTitle.Text = Strings.EditRelation;
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _relation = new CustomerVehicleRelation();
                PageTitle.Text = Strings.NewRelationTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
                DpStartDate.SelectedDate = DateTime.Today;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading relation:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_relation == null) return;

        PickerCustomer.SetValue(_relation.CustomerId, Services.LookupCache.GetName(Services.LookupCache.Customers, _relation.CustomerId));
        PickerVehicle.SetValue(_relation.VehicleId, Services.LookupCache.GetName(Services.LookupCache.Vehicles, _relation.VehicleId));
        PickerRelationType.SetValue(_relation.RelationTypeId, Services.LookupCache.GetName(Services.LookupCache.RelationTypes, _relation.RelationTypeId));
        DpStartDate.SelectedDate = _relation.StartDate;
        DpEndDate.SelectedDate = _relation.EndDate;
        TxtBeginNote.Text = _relation.BeginNote ?? "";
        TxtTerminationNote.Text = _relation.TerminationNote ?? "";
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var customerId = PickerCustomer.SelectedId;
            if (customerId is not > 0)
            {
                MessageBox.Show(Strings.IsMacedonian ? "Клиент е задолжително." : "Customer is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                PickerCustomer.Focus();
                return;
            }

            var vehicleId = PickerVehicle.SelectedId;
            if (vehicleId is not > 0)
            {
                MessageBox.Show(Strings.IsMacedonian ? "Возило е задолжително." : "Vehicle is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                PickerVehicle.Focus();
                return;
            }

            var relationTypeId = PickerRelationType.SelectedId;
            if (relationTypeId is not > 0)
            {
                MessageBox.Show(Strings.IsMacedonian ? "Тип на врска е задолжително." : "Relation Type is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                PickerRelationType.Focus();
                return;
            }

            if (DpStartDate.SelectedDate == null)
            {
                MessageBox.Show(Strings.IsMacedonian ? "Почеток е задолжително." : "Start Date is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                DpStartDate.Focus();
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_relationId.HasValue)
            {
                var existing = db.CustomerVehicleRelations.FirstOrDefault(r => r.Id == _relationId.Value);

                if (existing == null)
                {
                    MessageBox.Show("Relation no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.CustomerId = customerId!.Value;
                existing.VehicleId = vehicleId!.Value;
                existing.RelationTypeId = relationTypeId!.Value;
                existing.StartDate = DpStartDate.SelectedDate!.Value;
                existing.EndDate = DpEndDate.SelectedDate;
                existing.BeginNote = string.IsNullOrWhiteSpace(TxtBeginNote.Text) ? null : TxtBeginNote.Text.Trim();
                existing.TerminationNote = string.IsNullOrWhiteSpace(TxtTerminationNote.Text) ? null : TxtTerminationNote.Text.Trim();
            }
            else
            {
                var newRelation = new CustomerVehicleRelation
                {
                    CustomerId = customerId!.Value,
                    VehicleId = vehicleId!.Value,
                    RelationTypeId = relationTypeId!.Value,
                    StartDate = DpStartDate.SelectedDate!.Value,
                    EndDate = DpEndDate.SelectedDate,
                    BeginNote = string.IsNullOrWhiteSpace(TxtBeginNote.Text) ? null : TxtBeginNote.Text.Trim(),
                    TerminationNote = string.IsNullOrWhiteSpace(TxtTerminationNote.Text) ? null : TxtTerminationNote.Text.Trim()
                };
                db.CustomerVehicleRelations.Add(newRelation);
            }

            db.SaveChanges();

            MessageBox.Show(Strings.SavedSuccessfully, Strings.IsMacedonian ? "Успех" : "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving relation:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (!_relationId.HasValue) return;

        var result = MessageBox.Show(
            Strings.ConfirmDelete,
            Strings.ConfirmDeleteTitle,
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var relation = db.CustomerVehicleRelations.FirstOrDefault(r => r.Id == _relationId.Value);
            if (relation != null)
            {
                db.CustomerVehicleRelations.Remove(relation);
                db.SaveChanges();
            }

            MessageBox.Show(Strings.DeletedSuccessfully, Strings.IsMacedonian ? "Успех" : "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting relation:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
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
            mainWindow.ContentArea.Content = new RelationListPage();
        }
    }
}
