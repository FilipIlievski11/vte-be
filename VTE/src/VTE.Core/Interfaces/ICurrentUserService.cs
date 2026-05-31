namespace VTE.Core.Interfaces;

public interface ICurrentUserService
{
    long? UserId { get; }
    string? Username { get; }
    string? RoleName { get; }
    bool IsAuthenticated { get; }
    bool HasPermission(string entityName, string action);
}
