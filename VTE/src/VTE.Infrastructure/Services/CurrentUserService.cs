namespace VTE.Infrastructure.Services;

using VTE.Core.Interfaces;

public class CurrentUserService : ICurrentUserService
{
    public long? UserId { get; set; }
    public string? Username { get; set; }
    public string? RoleName { get; set; }
    public bool IsAuthenticated => UserId.HasValue;

    private readonly HashSet<string> _permissions = [];

    public void SetPermissions(IEnumerable<string> permissions)
    {
        _permissions.Clear();
        foreach (var p in permissions) _permissions.Add(p);
    }

    public bool HasPermission(string entityName, string action)
        => _permissions.Contains($"{entityName}:{action}");

    public void Clear()
    {
        UserId = null;
        Username = null;
        RoleName = null;
        _permissions.Clear();
    }
}
