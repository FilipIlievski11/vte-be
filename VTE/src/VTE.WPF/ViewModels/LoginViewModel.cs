namespace VTE.WPF.ViewModels;

using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure.Data;
using VTE.Infrastructure.Services;

public partial class LoginViewModel : ObservableObject
{
    private readonly VteDbContext _dbContext;
    private readonly CurrentUserService _currentUserService;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoggingIn;

    public event Action? LoginSucceeded;

    public LoginViewModel(VteDbContext dbContext, CurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;
        IsLoggingIn = true;

        try
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.Privileges)
                .FirstOrDefaultAsync(u => u.Username == Username && u.IsActive);

            if (user is null || !BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                ErrorMessage = "Invalid username or password.";
                return;
            }

            _currentUserService.UserId = user.Id;
            _currentUserService.Username = user.Username;
            _currentUserService.RoleName = user.Role.Name;
            _currentUserService.SetPermissions(
                user.Role.Privileges.SelectMany(p => new[]
                {
                    p.CanCreate ? $"{p.EntityName}:Create" : null,
                    p.CanRead ? $"{p.EntityName}:Read" : null,
                    p.CanUpdate ? $"{p.EntityName}:Update" : null,
                    p.CanDelete ? $"{p.EntityName}:Delete" : null,
                }.Where(x => x is not null).Cast<string>()));

            LoginSucceeded?.Invoke();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Connection error: {ex.Message}";
        }
        finally
        {
            IsLoggingIn = false;
        }
    }
}
