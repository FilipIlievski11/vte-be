namespace VTE.WPF.Views.Pages;

using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class CustomerDetailPage : UserControl
{
    private readonly long? _customerId;
    private Customer? _customer;

    public CustomerDetailPage(long? customerId)
    {
        _customerId = customerId;
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadCustomer();
    }

    private void ApplyLanguage()
    {
        BtnBack.Content = Strings.BackToList;
        LblFirstName.Text = Strings.FirstNameRequired;
        LblLastName.Text = Strings.LastNameRequired;
        LblDateOfBirth.Text = Strings.DateOfBirth;
        LblIdentificationNumber.Text = Strings.IdentificationNumber;
        LblPhoneNumber.Text = Strings.PhoneNumber;
        LblEmail.Text = Strings.Email;
        ChkIsCompany.Content = Strings.IsCompanyLabel;
        LblCity.Text = Strings.IsMacedonian ? "Адреса на живеење" : "Living Address";
        LblNote.Text = Strings.Note;
        BtnSave.Content = Strings.Save;
        BtnDelete.Content = Strings.Delete;
        BtnCancel.Content = Strings.Cancel;
    }

    private void LoadCustomer()
    {
        try
        {
            if (_customerId.HasValue)
            {
                using var scope = App.Services!.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
                _customer = db.Customers.AsNoTracking().FirstOrDefault(c => c.Id == _customerId.Value);

                if (_customer == null)
                {
                    MessageBox.Show("Customer not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                PageTitle.Text = $"{Strings.EditCustomerTitle} - {_customer.FirstName} {_customer.LastName}";
                BtnDelete.Visibility = Visibility.Visible;
                BindFields();
            }
            else
            {
                _customer = new Customer();
                PageTitle.Text = Strings.NewCustomerTitle;
                BtnDelete.Visibility = Visibility.Collapsed;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading customer:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BindFields()
    {
        if (_customer == null) return;

        TxtFirstName.Text = _customer.FirstName;
        TxtLastName.Text = _customer.LastName;
        TxtParentName.Text = _customer.ParentName ?? "";
        TxtDateOfBirth.Text = _customer.DateOfBirth?.ToString("dd.MM.yyyy") ?? "";
        TxtIdentificationNumber.Text = _customer.IdentificationNumber;
        TxtIdentityCardNumber.Text = _customer.IdentityCardNumber ?? "";
        TxtDrivingLicenseNumber.Text = _customer.DrivingLicenseNumber ?? "";
        ChkIsCompany.IsChecked = _customer.IsCompany;
        TxtCompanyName.Text = _customer.CompanyName ?? "";
        TxtTaxNumber.Text = _customer.TaxNumber ?? "";
        TxtPhoneNumber.Text = _customer.PhoneNumber ?? "";
        TxtEmail.Text = _customer.Email ?? "";
        TxtFax.Text = _customer.Fax ?? "";
        TxtOccupation.Text = _customer.Occupation ?? "";
        TxtNote.Text = _customer.Note ?? "";

        // Load city name for address display
        if (_customer.LivingCityId.HasValue)
            TxtCity.Text = Services.LookupCache.GetName(Services.LookupCache.Cities, _customer.LivingCityId);
    }

    private void ReadFields()
    {
        if (_customer == null) return;

        _customer.FirstName = TxtFirstName.Text.Trim();
        _customer.LastName = TxtLastName.Text.Trim();
        _customer.ParentName = string.IsNullOrWhiteSpace(TxtParentName.Text) ? null : TxtParentName.Text.Trim();

        if (DateTime.TryParseExact(TxtDateOfBirth.Text.Trim(), "dd.MM.yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dob))
            _customer.DateOfBirth = dob;
        else if (string.IsNullOrWhiteSpace(TxtDateOfBirth.Text))
            _customer.DateOfBirth = null;

        _customer.IdentificationNumber = TxtIdentificationNumber.Text.Trim();
        _customer.IdentityCardNumber = string.IsNullOrWhiteSpace(TxtIdentityCardNumber.Text) ? null : TxtIdentityCardNumber.Text.Trim();
        _customer.DrivingLicenseNumber = string.IsNullOrWhiteSpace(TxtDrivingLicenseNumber.Text) ? null : TxtDrivingLicenseNumber.Text.Trim();
        _customer.IsCompany = ChkIsCompany.IsChecked == true;
        _customer.CompanyName = string.IsNullOrWhiteSpace(TxtCompanyName.Text) ? null : TxtCompanyName.Text.Trim();
        _customer.TaxNumber = string.IsNullOrWhiteSpace(TxtTaxNumber.Text) ? null : TxtTaxNumber.Text.Trim();
        _customer.PhoneNumber = string.IsNullOrWhiteSpace(TxtPhoneNumber.Text) ? null : TxtPhoneNumber.Text.Trim();
        _customer.Email = string.IsNullOrWhiteSpace(TxtEmail.Text) ? null : TxtEmail.Text.Trim();
        _customer.Fax = string.IsNullOrWhiteSpace(TxtFax.Text) ? null : TxtFax.Text.Trim();
        _customer.Occupation = string.IsNullOrWhiteSpace(TxtOccupation.Text) ? null : TxtOccupation.Text.Trim();
        _customer.Note = string.IsNullOrWhiteSpace(TxtNote.Text) ? null : TxtNote.Text.Trim();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ReadFields();

            if (string.IsNullOrWhiteSpace(_customer!.FirstName))
            {
                MessageBox.Show("First Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtFirstName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(_customer.LastName))
            {
                MessageBox.Show("Last Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtLastName.Focus();
                return;
            }

            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_customerId.HasValue)
            {
                var existing = db.Customers.Find(_customerId.Value);
                if (existing == null)
                {
                    MessageBox.Show("Customer no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NavigateBack();
                    return;
                }

                existing.FirstName = _customer.FirstName;
                existing.LastName = _customer.LastName;
                existing.ParentName = _customer.ParentName;
                existing.DateOfBirth = _customer.DateOfBirth;
                existing.IdentificationNumber = _customer.IdentificationNumber;
                existing.IdentityCardNumber = _customer.IdentityCardNumber;
                existing.DrivingLicenseNumber = _customer.DrivingLicenseNumber;
                existing.IsCompany = _customer.IsCompany;
                existing.CompanyName = _customer.CompanyName;
                existing.TaxNumber = _customer.TaxNumber;
                existing.PhoneNumber = _customer.PhoneNumber;
                existing.Email = _customer.Email;
                existing.Fax = _customer.Fax;
                existing.Occupation = _customer.Occupation;
                existing.Note = _customer.Note;
                existing.ModifiedAt = DateTime.UtcNow;
            }
            else
            {
                _customer.CreatedAt = DateTime.UtcNow;
                db.Customers.Add(_customer);
            }

            db.SaveChanges();

            MessageBox.Show("Customer saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving customer:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (!_customerId.HasValue) return;

        var result = MessageBox.Show(
            "Are you sure you want to delete this customer?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var customer = db.Customers.Find(_customerId.Value);
            if (customer != null)
            {
                db.Customers.Remove(customer);
                db.SaveChanges();
            }

            MessageBox.Show("Customer deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigateBack();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting customer:\n{ex.Message}", "Error",
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
            mainWindow.ContentArea.Content = new CustomerListPage();
        }
    }
}
