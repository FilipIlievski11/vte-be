namespace VTE.WPF.Views.Pages;

using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class RequestDetailPage : UserControl
{
    private readonly long? _requestId;
    private Request? _request;

    public RequestDetailPage(long? requestId)
    {
        _requestId = requestId;
        InitializeComponent();
        ApplyLanguage();
        Loaded += async (_, _) =>
        {
            await LoadDropdownsAsync();
            LoadRequest();
        };
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        LblRequestType.Text = Strings.RequestTypeRequired;
        LblCustomerVehicle.Text = Strings.CustomerVehicleRequired;
        LblOrganization.Text = Strings.Organization;
        LblDateCreated.Text = Strings.DateCreated;
        ChkIsCustomerChanged.Content = Strings.IsCustomerChanged;
        ChkIsVehicleChanged.Content = Strings.IsVehicleChanged;
        LblDateEnded.Text = Strings.DateEnded;
        LblNote.Text = Strings.Note;
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
    }

    private System.Threading.Tasks.Task LoadDropdownsAsync()
    {
        PickerRequestType.SetItems(Services.LookupCache.RequestTypes);
        var orgItems = new List<Controls.LookupItem>(Services.LookupCache.Organizations);
        orgItems.Insert(0, new Controls.LookupItem { Id = 0, Name = "(None)" });
        PickerOrganization.SetItems(orgItems);
        return System.Threading.Tasks.Task.CompletedTask;
    }

    private void LoadRequest()
    {
        try
        {
            if (_requestId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _request = db.Requests.AsNoTracking().FirstOrDefault(r => r.Id == _requestId.Value);

                if (_request == null)
                {
                    MessageBox.Show("Request not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                PageTitle.Text = $"{Strings.EditRequestTitle} #{_request.Id}";
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _request = new Request();
                PageTitle.Text = Strings.NewRequestTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
                TxtDateCreated.Text = "(will be set on save)";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading request:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_request == null) return;

        PickerRequestType.SetValue(_request.RequestTypeId, Services.LookupCache.GetName(Services.LookupCache.RequestTypes, _request.RequestTypeId));
        if (_request.CustomerVehicleRelationId > 0)
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var rel = db.CustomerVehicleRelations
                .Include(r => r.Customer).Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == _request.CustomerVehicleRelationId);
            if (rel != null)
                RelationPicker.SetValue(rel.Id,
                    $"{rel.Customer.FirstName} {rel.Customer.LastName} - {rel.Vehicle.LastRegistrationNumber ?? rel.Vehicle.ShellNumber}");
        }
        ChkIsCustomerChanged.IsChecked = _request.IsCustomerChanged;
        ChkIsVehicleChanged.IsChecked = _request.IsVehicleChanged;
        TxtDateCreated.Text = _request.CreatedAt.ToString("dd.MM.yyyy HH:mm");
        DpDateEnded.SelectedDate = _request.DateEnded;
        PickerOrganization.SetValue(_request.OrganizationId ?? 0L, Services.LookupCache.GetName(Services.LookupCache.Organizations, _request.OrganizationId ?? 0L));
        TxtNote.Text = _request.Note ?? "";
    }

    private void ReadFields()
    {
        if (_request == null) return;

        _request.RequestTypeId = PickerRequestType.SelectedId ?? 0L;
        _request.CustomerVehicleRelationId = RelationPicker.SelectedRelationId ?? 0L;
        _request.IsCustomerChanged = ChkIsCustomerChanged.IsChecked == true;
        _request.IsVehicleChanged = ChkIsVehicleChanged.IsChecked == true;
        _request.DateEnded = DpDateEnded.SelectedDate;
        var orgId = PickerOrganization.SelectedId;
        _request.OrganizationId = orgId == 0 ? null : orgId;
        _request.Note = string.IsNullOrWhiteSpace(TxtNote.Text) ? null : TxtNote.Text.Trim();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ReadFields();

            if (_request!.RequestTypeId == 0)
            {
                MessageBox.Show("Request Type is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_request.CustomerVehicleRelationId == 0)
            {
                MessageBox.Show("Customer - Vehicle is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_requestId.HasValue)
            {
                var existing = db.Requests.Find(_requestId.Value);
                if (existing == null)
                {
                    MessageBox.Show("Request no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.RequestTypeId = _request.RequestTypeId;
                existing.CustomerVehicleRelationId = _request.CustomerVehicleRelationId;
                existing.IsCustomerChanged = _request.IsCustomerChanged;
                existing.IsVehicleChanged = _request.IsVehicleChanged;
                existing.DateEnded = _request.DateEnded;
                existing.OrganizationId = _request.OrganizationId;
                existing.Note = _request.Note;
                existing.ModifiedAt = DateTime.UtcNow;
            }
            else
            {
                _request.CreatedAt = DateTime.UtcNow;
                _request.CreatedByUserId = 1;
                db.Requests.Add(_request);
            }

            db.SaveChanges();

            MessageBox.Show("Request saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving request:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (!_requestId.HasValue) return;

        var result = MessageBox.Show(
            "Are you sure you want to delete this request?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var request = db.Requests.Find(_requestId.Value);
            if (request != null)
            {
                db.Requests.Remove(request);
                db.SaveChanges();
            }

            MessageBox.Show("Request deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting request:\n{ex.Message}", "Error",
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
            mainWindow.ContentArea.Content = new RequestListPage();
        }
    }
}
