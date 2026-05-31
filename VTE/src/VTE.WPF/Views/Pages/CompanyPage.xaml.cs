namespace VTE.WPF.Views.Pages;

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;
using VTE.WPF.Resources;

public partial class CompanyPage : UserControl
{
    private long? _editingId;

    public CompanyPage()
    {
        InitializeComponent();
        ApplyLanguage();
        Loaded += (_, _) => LoadCompanies();
    }

    private void ApplyLanguage()
    {
        TxtTitle.Text = Strings.IsMacedonian ? "Организации" : "Organizations";
        BtnNew.Content = Strings.IsMacedonian ? "+ Нова организација" : "+ New Organization";
        BtnEdit.Content = Strings.Edit;
        BtnDelete.Content = Strings.Delete;
        BtnSave.Content = Strings.Save;
        BtnCancel.Content = Strings.Cancel;
        ColName.Header = Strings.IsMacedonian ? "Назив" : "Name";
        ColTaxNumber.Header = Strings.TaxNumber;
        ColAddress.Header = Strings.IsMacedonian ? "Адреса" : "Address";
        ColPhone.Header = Strings.PhoneNumber;
        LblName.Text = Strings.IsMacedonian ? "Назив" : "Name";
        LblTaxNumber.Text = Strings.TaxNumber;
        LblAddress.Text = Strings.IsMacedonian ? "Адреса" : "Address";
        LblPhone.Text = Strings.PhoneNumber;
    }

    private void LoadCompanies()
    {
        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var companies = db.Companies
                .OrderByDescending(c => c.Id)
                .Select(c => new CompanyListItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    TaxNumber = c.TaxNumber ?? "",
                    Address = c.Address ?? "",
                    PhoneNumber = c.PhoneNumber ?? ""
                })
                .ToList();

            CompaniesGrid.ItemsSource = companies;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading companies:\n{ex.Message}", Strings.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ClearEditPanel()
    {
        _editingId = null;
        TxtName.Text = string.Empty;
        TxtTaxNumber.Text = string.Empty;
        TxtAddress.Text = string.Empty;
        TxtPhone.Text = string.Empty;
    }

    private void ShowEditPanel(bool isNew)
    {
        EditPanelTitle.Text = isNew
            ? (Strings.IsMacedonian ? "Нова организација" : "New Organization")
            : (Strings.IsMacedonian ? "Измени организација" : "Edit Organization");
        EditPanel.Visibility = Visibility.Visible;
    }

    private void New_Click(object sender, RoutedEventArgs e)
    {
        ClearEditPanel();
        ShowEditPanel(isNew: true);
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (CompaniesGrid.SelectedItem is not CompanyListItem selected)
        {
            MessageBox.Show(Strings.IsMacedonian ? "Изберете организација." : "Please select an organization.",
                Strings.Error, MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var company = db.Companies.Find(selected.Id);

            if (company == null)
            {
                MessageBox.Show(Strings.IsMacedonian ? "Организацијата не е пронајдена." : "Organization not found.",
                    Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _editingId = company.Id;
            TxtName.Text = company.Name;
            TxtTaxNumber.Text = company.TaxNumber ?? "";
            TxtAddress.Text = company.Address ?? "";
            TxtPhone.Text = company.PhoneNumber ?? "";

            ShowEditPanel(isNew: false);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading organization:\n{ex.Message}", Strings.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (CompaniesGrid.SelectedItem is not CompanyListItem selected)
        {
            MessageBox.Show(Strings.IsMacedonian ? "Изберете организација." : "Please select an organization.",
                Strings.Error, MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

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
            var company = db.Companies.Find(selected.Id);

            if (company != null)
            {
                db.Companies.Remove(company);
                db.SaveChanges();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadCompanies();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting organization:\n{ex.Message}", Strings.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtName.Text))
        {
            MessageBox.Show(Strings.FieldRequired, Strings.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
            TxtName.Focus();
            return;
        }

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_editingId == null)
            {
                var company = new Company
                {
                    Name = TxtName.Text.Trim(),
                    TaxNumber = string.IsNullOrWhiteSpace(TxtTaxNumber.Text) ? null : TxtTaxNumber.Text.Trim(),
                    Address = string.IsNullOrWhiteSpace(TxtAddress.Text) ? null : TxtAddress.Text.Trim(),
                    PhoneNumber = string.IsNullOrWhiteSpace(TxtPhone.Text) ? null : TxtPhone.Text.Trim()
                };
                db.Companies.Add(company);
            }
            else
            {
                var company = db.Companies.Find(_editingId.Value);
                if (company == null)
                {
                    MessageBox.Show(Strings.IsMacedonian ? "Организацијата не е пронајдена." : "Organization not found.",
                        Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                company.Name = TxtName.Text.Trim();
                company.TaxNumber = string.IsNullOrWhiteSpace(TxtTaxNumber.Text) ? null : TxtTaxNumber.Text.Trim();
                company.Address = string.IsNullOrWhiteSpace(TxtAddress.Text) ? null : TxtAddress.Text.Trim();
                company.PhoneNumber = string.IsNullOrWhiteSpace(TxtPhone.Text) ? null : TxtPhone.Text.Trim();
            }

            db.SaveChanges();
            EditPanel.Visibility = Visibility.Collapsed;
            LoadCompanies();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving organization:\n{ex.Message}", Strings.Error,
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelEdit_Click(object sender, RoutedEventArgs e)
    {
        EditPanel.Visibility = Visibility.Collapsed;
        _editingId = null;
    }
}

public class CompanyListItem
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
