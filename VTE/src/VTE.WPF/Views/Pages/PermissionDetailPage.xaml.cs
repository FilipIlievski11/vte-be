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

public partial class PermissionDetailPage : UserControl
{
    private readonly long? _permissionId;
    private Permission? _permission;

    public PermissionDetailPage(long? permissionId)
    {
        _permissionId = permissionId;
        InitializeComponent();
        ApplyLanguage();
        Loaded += async (_, _) =>
        {
            await LoadDropdownsAsync();
            LoadPermission();
        };
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        LblPermissionNumber.Text = Strings.PermissionNumber + " *";
        LblIssuedDate.Text = Strings.IsMacedonian ? "Датум на издавање" : "Issued Date";
        LblValidUntilDate.Text = Strings.ValidUntil;
        LblRelation.Text = Strings.CustomerVehicleRelationRequired;
        LblNote.Text = Strings.Note;
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
    }

    private System.Threading.Tasks.Task LoadDropdownsAsync()
    {
        // Relations are now handled by the SearchableRelationPicker control
        return System.Threading.Tasks.Task.CompletedTask;
    }

    private void LoadPermission()
    {
        try
        {
            if (_permissionId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _permission = db.Permissions
                    .Include(p => p.Document)
                    .AsNoTracking()
                    .FirstOrDefault(p => p.Id == _permissionId.Value);

                if (_permission == null)
                {
                    MessageBox.Show("Permission not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                PageTitle.Text = $"{Strings.EditPermission} - {_permission.PermissionNumber}";
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _permission = new Permission();
                PageTitle.Text = Strings.NewPermissionTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
                DpIssuedDate.SelectedDate = DateTime.Today;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading permission:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_permission == null) return;

        TxtPermissionNumber.Text = _permission.PermissionNumber;
        DpIssuedDate.SelectedDate = _permission.IssuedDate;
        DpValidUntilDate.SelectedDate = _permission.ValidUntilDate;
        TxtNote.Text = _permission.Note ?? "";
        if (_permission.Document?.CustomerVehicleRelationId is long cvrId && cvrId > 0)
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var rel = db.CustomerVehicleRelations
                .Include(r => r.Customer).Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == cvrId);
            if (rel != null)
                RelationPicker.SetValue(rel.Id,
                    $"{rel.Customer.FirstName} {rel.Customer.LastName} - {rel.Vehicle.LastRegistrationNumber ?? rel.Vehicle.ShellNumber}");
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(TxtPermissionNumber.Text))
            {
                MessageBox.Show(Strings.IsMacedonian ? "Број на дозвола е задолжително." : "Permission Number is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtPermissionNumber.Focus();
                return;
            }

            var relationId = RelationPicker.SelectedRelationId ?? 0L;
            if (relationId <= 0)
            {
                MessageBox.Show(Strings.IsMacedonian ? "Клиент-Возило е задолжително." : "Customer-Vehicle Relation is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                RelationPicker.Focus();
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_permissionId.HasValue)
            {
                var existing = db.Permissions
                    .Include(p => p.Document)
                    .FirstOrDefault(p => p.Id == _permissionId.Value);

                if (existing == null)
                {
                    MessageBox.Show("Permission no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.PermissionNumber = TxtPermissionNumber.Text.Trim();
                existing.IssuedDate = DpIssuedDate.SelectedDate ?? DateTime.Today;
                existing.ValidUntilDate = DpValidUntilDate.SelectedDate;
                existing.Note = string.IsNullOrWhiteSpace(TxtNote.Text) ? null : TxtNote.Text.Trim();

                // Update document relation
                if (existing.Document != null)
                {
                    existing.Document.CustomerVehicleRelationId = relationId;
                    existing.Document.ModifiedAt = DateTime.UtcNow;
                }
            }
            else
            {
                // Create Document parent first
                var document = new Document
                {
                    CustomerVehicleRelationId = relationId,
                    DocumentTypeId = 1, // default document type
                    CreatedAt = DateTime.UtcNow
                };
                db.Documents.Add(document);
                db.SaveChanges();

                var newPermission = new Permission
                {
                    DocumentId = document.Id,
                    PermissionNumber = TxtPermissionNumber.Text.Trim(),
                    IssuedDate = DpIssuedDate.SelectedDate ?? DateTime.Today,
                    ValidUntilDate = DpValidUntilDate.SelectedDate,
                    Note = string.IsNullOrWhiteSpace(TxtNote.Text) ? null : TxtNote.Text.Trim()
                };
                db.Permissions.Add(newPermission);
            }

            db.SaveChanges();

            MessageBox.Show(Strings.SavedSuccessfully, Strings.IsMacedonian ? "Успех" : "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving permission:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (!_permissionId.HasValue) return;

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

            var permission = db.Permissions.FirstOrDefault(p => p.Id == _permissionId.Value);
            if (permission != null)
            {
                db.Permissions.Remove(permission);
                db.SaveChanges();
            }

            MessageBox.Show(Strings.DeletedSuccessfully, Strings.IsMacedonian ? "Успех" : "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting permission:\n{ex.Message}", "Error",
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
            mainWindow.ContentArea.Content = new PermissionListPage();
        }
    }
}
