namespace VTE.WPF.Views.Pages;

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class IntlLicenseDetailPage : UserControl
{
    private readonly long? _licenseId;
    private InternationalDrivingLicense? _license;

    public IntlLicenseDetailPage(long? licenseId)
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
        LblIssuedDate.Text = Strings.IssuedDate;
        LblValidUntil.Text = Strings.ValidUntil;
        LblCustomer.Text = Strings.Customer + " *";
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
    }

    private System.Threading.Tasks.Task LoadDropdownsAsync()
    {
        PickerCustomer.SetItems(Services.LookupCache.Customers);
        return System.Threading.Tasks.Task.CompletedTask;
    }

    private void LoadLicense()
    {
        try
        {
            if (_licenseId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _license = db.InternationalDrivingLicenses
                    .AsNoTracking()
                    .FirstOrDefault(l => l.Id == _licenseId.Value);

                if (_license == null)
                {
                    MessageBox.Show("International driving license not found.", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                PageTitle.Text = $"{Strings.EditIntlLicense} - {_license.LicenseNumber}";
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _license = new InternationalDrivingLicense();
                PageTitle.Text = Strings.NewIntlLicenseTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
                DpIssuedDate.SelectedDate = DateTime.Today;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading license:\n{ex.Message}", Strings.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_license == null) return;

        TxtLicenseNumber.Text = _license.LicenseNumber;
        DpIssuedDate.SelectedDate = _license.IssuedDate;
        DpValidUntil.SelectedDate = _license.ValidUntilDate;
        PickerCustomer.SetValue(_license.CustomerId, Services.LookupCache.GetName(Services.LookupCache.Customers, _license.CustomerId));
    }

    private void ReadFields()
    {
        if (_license == null) return;

        _license.LicenseNumber = TxtLicenseNumber.Text.Trim();
        _license.IssuedDate = DpIssuedDate.SelectedDate ?? DateTime.Today;
        _license.ValidUntilDate = DpValidUntil.SelectedDate ?? DateTime.Today.AddYears(1);
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

            var customerId = PickerCustomer.SelectedId;
            if (customerId is not > 0)
            {
                MessageBox.Show(Strings.FieldRequired, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                PickerCustomer.Focus();
                return;
            }

            _license.CustomerId = customerId.Value;

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_licenseId.HasValue)
            {
                var existing = db.InternationalDrivingLicenses
                    .FirstOrDefault(l => l.Id == _licenseId.Value);

                if (existing == null)
                {
                    MessageBox.Show("License no longer exists.", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.LicenseNumber = _license.LicenseNumber;
                existing.IssuedDate = _license.IssuedDate;
                existing.ValidUntilDate = _license.ValidUntilDate;
                existing.CustomerId = _license.CustomerId;
            }
            else
            {
                db.InternationalDrivingLicenses.Add(_license);
            }

            db.SaveChanges();

            MessageBox.Show(Strings.SavedSuccessfully, "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving license:\n{ex.Message}", Strings.Error,
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

            var license = db.InternationalDrivingLicenses.FirstOrDefault(l => l.Id == _licenseId.Value);
            if (license != null)
            {
                db.InternationalDrivingLicenses.Remove(license);
                db.SaveChanges();
            }

            MessageBox.Show(Strings.DeletedSuccessfully, "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting license:\n{ex.Message}", Strings.Error,
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
            mainWindow.ContentArea.Content = new IntlLicenseListPage();
        }
    }
}
