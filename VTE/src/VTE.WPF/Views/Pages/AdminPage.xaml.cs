namespace VTE.WPF.Views.Pages;

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;

public partial class AdminPage : UserControl
{
    private long? _editingUserId;

    public AdminPage()
    {
        InitializeComponent();
        Loaded += (_, _) => LoadUsers();
    }

    private void LoadUsers()
    {
        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            var users = db.Users
                .Include(u => u.Role)
                .Include(u => u.Organization)
                .OrderByDescending(u => u.Id)
                .Select(u => new UserListItem
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName,
                    RoleName = u.Role != null ? u.Role.Name : "",
                    OrganizationName = u.Organization != null ? u.Organization.Name : "",
                    IsActive = u.IsActive,
                    DateOfHiring = u.DateOfHiring
                })
                .ToList();

            UsersGrid.ItemsSource = users;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading users:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void LoadComboBoxes()
    {
        PickerRole.SetItems(Services.LookupCache.Roles);
        PickerOrganization.SetItems(Services.LookupCache.Organizations);
    }

    private void ShowEditPanel(bool isNew)
    {
        LoadComboBoxes();
        EditPanelTitle.Text = isNew ? "New User" : "Edit User";
        LblPassword.Visibility = isNew ? Visibility.Visible : Visibility.Collapsed;
        TxtPassword.Visibility = isNew ? Visibility.Visible : Visibility.Collapsed;
        EditPanel.Visibility = Visibility.Visible;
    }

    private void ClearEditPanel()
    {
        _editingUserId = null;
        TxtUsername.Text = string.Empty;
        TxtPassword.Clear();
        TxtFirstName.Text = string.Empty;
        TxtLastName.Text = string.Empty;
        TxtFullName.Text = string.Empty;
        PickerRole.SetValue(null, "");
        PickerOrganization.SetValue(null, "");
        ChkIsActive.IsChecked = true;
    }

    private void NewUser_Click(object sender, RoutedEventArgs e)
    {
        ClearEditPanel();
        ShowEditPanel(isNew: true);
    }

    private void EditUser_Click(object sender, RoutedEventArgs e)
    {
        if (UsersGrid.SelectedItem is not UserListItem selected)
        {
            MessageBox.Show("Please select a user to edit.", "Information",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var user = db.Users.Find(selected.Id);

            if (user == null)
            {
                MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _editingUserId = user.Id;
            TxtUsername.Text = user.Username;
            TxtPassword.Clear();
            TxtFirstName.Text = user.FirstName;
            TxtLastName.Text = user.LastName;
            TxtFullName.Text = user.FullName;
            ChkIsActive.IsChecked = user.IsActive;

            ShowEditPanel(isNew: false);

            PickerRole.SetValue(user.RoleId, Services.LookupCache.GetName(Services.LookupCache.Roles, user.RoleId));
            PickerOrganization.SetValue(user.OrganizationId, Services.LookupCache.GetName(Services.LookupCache.Organizations, user.OrganizationId));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading user:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void DeleteUser_Click(object sender, RoutedEventArgs e)
    {
        if (UsersGrid.SelectedItem is not UserListItem selected)
        {
            MessageBox.Show("Please select a user to delete.", "Information",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete user '{selected.Username}'?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            var user = db.Users.Find(selected.Id);

            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                EditPanel.Visibility = Visibility.Collapsed;
                LoadUsers();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting user:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void SaveUser_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtUsername.Text))
        {
            MessageBox.Show("Username is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (PickerRole.SelectedId == null)
        {
            MessageBox.Show("Role is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (PickerOrganization.SelectedId == null)
        {
            MessageBox.Show("Organization is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            using var scope = App.Services!.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();

            if (_editingUserId == null)
            {
                // New user
                if (string.IsNullOrWhiteSpace(TxtPassword.Password))
                {
                    MessageBox.Show("Password is required for new users.", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var user = new User
                {
                    Username = TxtUsername.Text.Trim(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(TxtPassword.Password),
                    FirstName = TxtFirstName.Text.Trim(),
                    LastName = TxtLastName.Text.Trim(),
                    FullName = TxtFullName.Text.Trim(),
                    RoleId = PickerRole.SelectedId!.Value,
                    OrganizationId = PickerOrganization.SelectedId!.Value,
                    IsActive = ChkIsActive.IsChecked == true,
                    CreatedAt = DateTime.Now
                };

                db.Users.Add(user);
            }
            else
            {
                // Edit existing user
                var user = db.Users.Find(_editingUserId.Value);
                if (user == null)
                {
                    MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                user.Username = TxtUsername.Text.Trim();
                user.FirstName = TxtFirstName.Text.Trim();
                user.LastName = TxtLastName.Text.Trim();
                user.FullName = TxtFullName.Text.Trim();
                user.RoleId = PickerRole.SelectedId!.Value;
                user.OrganizationId = PickerOrganization.SelectedId!.Value;
                user.IsActive = ChkIsActive.IsChecked == true;
                user.ModifiedAt = DateTime.Now;
            }

            db.SaveChanges();
            EditPanel.Visibility = Visibility.Collapsed;
            LoadUsers();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving user:\n{ex.Message}", "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelEdit_Click(object sender, RoutedEventArgs e)
    {
        EditPanel.Visibility = Visibility.Collapsed;
        _editingUserId = null;
    }
}

public class UserListItem
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string OrganizationName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? DateOfHiring { get; set; }
}
