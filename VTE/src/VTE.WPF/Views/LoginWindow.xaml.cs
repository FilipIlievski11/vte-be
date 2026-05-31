namespace VTE.WPF.Views;

using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VTE.Infrastructure.Data;
using VTE.Infrastructure.Services;
using VTE.WPF.Resources;

public partial class LoginWindow : Window
{
    private readonly IServiceScope _scope;
    private readonly VteDbContext _db;
    private readonly CurrentUserService _currentUser;

    public LoginWindow()
    {
        InitializeComponent();

        _scope = App.Services!.CreateScope();
        _db = _scope.ServiceProvider.GetRequiredService<VteDbContext>();
        _currentUser = App.Services!.GetRequiredService<CurrentUserService>();

        UsernameBox.Focus();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        AttemptLogin();
    }

    private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            AttemptLogin();
    }

    private void AttemptLogin()
    {
        var username = UsernameBox.Text.Trim();
        var password = PasswordBox.Password;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError(Strings.PleaseEnterCredentials);
            return;
        }

        try
        {
            var user = _db.Users
                .Include(u => u.Role)
                    .ThenInclude(r => r.Privileges)
                .FirstOrDefault(u => u.Username == username && u.IsActive);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ShowError(Strings.InvalidCredentials);
                return;
            }

            // Set current user info
            _currentUser.UserId = user.Id;
            _currentUser.Username = user.Username;
            _currentUser.RoleName = user.Role.Name;

            // Build permissions list from role privileges
            var permissions = new List<string>();
            foreach (var p in user.Role.Privileges)
            {
                if (p.CanCreate) permissions.Add($"{p.EntityName}:Create");
                if (p.CanRead)   permissions.Add($"{p.EntityName}:Read");
                if (p.CanUpdate) permissions.Add($"{p.EntityName}:Update");
                if (p.CanDelete) permissions.Add($"{p.EntityName}:Delete");
            }
            _currentUser.SetPermissions(permissions);

            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            ShowError($"{Strings.ConnectionError} {ex.Message}");
        }
    }

    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.Visibility = Visibility.Visible;
    }

    protected override void OnClosed(EventArgs e)
    {
        _scope.Dispose();
        base.OnClosed(e);
    }
}
