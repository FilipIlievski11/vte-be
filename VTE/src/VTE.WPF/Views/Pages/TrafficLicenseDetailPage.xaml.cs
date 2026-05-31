namespace VTE.WPF.Views.Pages;

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class TrafficLicenseDetailPage : UserControl
{
    private readonly long? _licenseId;
    private TrafficLicense? _license;
    private long? _documentId;
    private List<Controls.LookupItem> _relationItems = [];

    public TrafficLicenseDetailPage(long? licenseId)
    {
        _licenseId = licenseId;
        InitializeComponent();
        ApplyLanguage();
        Loaded += async (_, _) =>
        {
            await LoadDropdownsAsync();
            LoadLicense();
        };
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        LblLicenseNumber.Text = Strings.LicenseNumber + " *";
        LblPlateNumber.Text = Strings.PlateNumber;
        LblIssuedDate.Text = Strings.IssuedDate;
        LblValidUntil.Text = Strings.ValidUntil;
        LblDocumentId.Text = Strings.DocumentNumber;
        LblRelation.Text = Strings.CustomerVehicleRelation + " *";
        LblNote.Text = Strings.Note;
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
        BtnPrint.Content = Strings.Print;
    }

    private async System.Threading.Tasks.Task LoadDropdownsAsync()
    {
        try
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

                var relations = db.CustomerVehicleRelations
                    .Include(r => r.Customer).Include(r => r.Vehicle)
                    .OrderByDescending(r => r.Id).Take(500)
                    .Select(r => new Controls.LookupItem { Id = r.Id, Name = r.Customer.FirstName + " " + r.Customer.LastName + " - " + (r.Vehicle.LastRegistrationNumber ?? r.Vehicle.ShellNumber) })
                    .ToList();

                Dispatcher.Invoke(() =>
                {
                    _relationItems = relations;
                    PickerRelation.SetItems(relations);
                });
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading dropdowns:\n{ex.Message}", Strings.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoadLicense()
    {
        try
        {
            if (_licenseId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _license = db.TrafficLicenses
                    .Include(t => t.Document)
                    .AsNoTracking()
                    .FirstOrDefault(t => t.Id == _licenseId.Value);

                if (_license == null)
                {
                    MessageBox.Show("Traffic license not found.", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                _documentId = _license.DocumentId;
                PageTitle.Text = $"{Strings.EditTrafficLicense} - {_license.LicenseNumber}";
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _license = new TrafficLicense();
                PageTitle.Text = Strings.NewTrafficLicenseTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
                DpIssuedDate.SelectedDate = DateTime.Today;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading traffic license:\n{ex.Message}", Strings.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_license == null) return;

        TxtLicenseNumber.Text = _license.LicenseNumber;
        TxtPlateNumber.Text = _license.PlateNumber ?? "";
        DpIssuedDate.SelectedDate = _license.IssuedDate;
        DpValidUntil.SelectedDate = _license.ValidUntilDate;
        TxtDocumentId.Text = _license.DocumentId.ToString();
        TxtNote.Text = _license.Document?.Note ?? "";

        if (_license.Document != null)
        {
            var relName = _relationItems.Find(i => i.Id == _license.Document.CustomerVehicleRelationId)?.Name ?? "";
            PickerRelation.SetValue(_license.Document.CustomerVehicleRelationId, relName);
        }
    }

    private void ReadFields()
    {
        if (_license == null) return;

        _license.LicenseNumber = TxtLicenseNumber.Text.Trim();
        _license.PlateNumber = string.IsNullOrWhiteSpace(TxtPlateNumber.Text) ? null : TxtPlateNumber.Text.Trim();
        _license.IssuedDate = DpIssuedDate.SelectedDate ?? DateTime.Today;
        _license.ValidUntilDate = DpValidUntil.SelectedDate;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ReadFields();

            if (string.IsNullOrWhiteSpace(_license!.LicenseNumber))
            {
                MessageBox.Show(Strings.FieldRequired, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtLicenseNumber.Focus();
                return;
            }

            var cvrId = PickerRelation.SelectedId ?? 0L;
            if (cvrId <= 0)
            {
                MessageBox.Show(Strings.FieldRequired, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                PickerRelation.Focus();
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_licenseId.HasValue)
            {
                var existing = db.TrafficLicenses
                    .Include(t => t.Document)
                    .FirstOrDefault(t => t.Id == _licenseId.Value);

                if (existing == null)
                {
                    MessageBox.Show("Traffic license no longer exists.", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.LicenseNumber = _license.LicenseNumber;
                existing.PlateNumber = _license.PlateNumber;
                existing.IssuedDate = _license.IssuedDate;
                existing.ValidUntilDate = _license.ValidUntilDate;

                if (existing.Document != null)
                {
                    existing.Document.CustomerVehicleRelationId = cvrId;
                    existing.Document.Note = string.IsNullOrWhiteSpace(TxtNote.Text) ? null : TxtNote.Text.Trim();
                    existing.Document.ModifiedAt = DateTime.UtcNow;
                }
            }
            else
            {
                // Create a Document parent first
                var doc = new Document
                {
                    DocumentTypeId = 1, // default document type
                    CustomerVehicleRelationId = cvrId,
                    Note = string.IsNullOrWhiteSpace(TxtNote.Text) ? null : TxtNote.Text.Trim(),
                    CreatedAt = DateTime.UtcNow
                };
                db.Documents.Add(doc);
                db.SaveChanges();

                _license.DocumentId = doc.Id;
                db.TrafficLicenses.Add(_license);
            }

            db.SaveChanges();

            MessageBox.Show(Strings.SavedSuccessfully, "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving traffic license:\n{ex.Message}", Strings.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (!_licenseId.HasValue) return;

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

            var license = db.TrafficLicenses.FirstOrDefault(t => t.Id == _licenseId.Value);
            if (license != null)
            {
                db.TrafficLicenses.Remove(license);
                db.SaveChanges();
            }

            MessageBox.Show(Strings.DeletedSuccessfully, "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting traffic license:\n{ex.Message}", Strings.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Print_Click(object sender, RoutedEventArgs e)
    {
        if (_licenseId == null) { MessageBox.Show(Strings.SaveFirst); return; }
        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var license = db.TrafficLicenses
                .Include(t => t.Document).ThenInclude(d => d.CustomerVehicleRelation).ThenInclude(r => r.Customer).ThenInclude(c => c.LivingCity)
                .Include(t => t.Document).ThenInclude(d => d.CustomerVehicleRelation).ThenInclude(r => r.Customer).ThenInclude(c => c.LivingAddressStreet)
                .Include(t => t.Document).ThenInclude(d => d.CustomerVehicleRelation).ThenInclude(r => r.Vehicle).ThenInclude(v => v.VehicleModel).ThenInclude(m => m!.VehicleMaker)
                .Include(t => t.Document).ThenInclude(d => d.CustomerVehicleRelation).ThenInclude(r => r.Vehicle).ThenInclude(v => v.Category)
                .Include(t => t.Document).ThenInclude(d => d.CustomerVehicleRelation).ThenInclude(r => r.Vehicle).ThenInclude(v => v.BodyType)
                .Include(t => t.Document).ThenInclude(d => d.CustomerVehicleRelation).ThenInclude(r => r.Vehicle).ThenInclude(v => v.EngineType)
                .Include(t => t.Document).ThenInclude(d => d.CustomerVehicleRelation).ThenInclude(r => r.Vehicle).ThenInclude(v => v.PrimaryColor)
                .FirstOrDefault(t => t.Id == _licenseId);

            if (license?.Document == null) return;

            var customer = license.Document.CustomerVehicleRelation.Customer;
            var vehicle = license.Document.CustomerVehicleRelation.Vehicle;
            var orgName = db.TechnicalExamOrganizations.FirstOrDefault()?.Name ?? "VTE";

            var pdfBytes = Services.TrafficLicensePdf.Generate(license, customer, vehicle, orgName);

            var tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                $"TrafficLicense_{license.LicenseNumber.Replace("/", "_")}.pdf");
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
            mainWindow.ContentArea.Content = new TrafficLicenseListPage();
        }
    }
}
