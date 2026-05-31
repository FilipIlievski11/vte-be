using System.ComponentModel.DataAnnotations;

namespace VTE.Api.Dtos;

public record LoginRequest(
    [Required] string UserName,
    [Required] string Password,
    bool UseCookie = false
);

public record LoginResponse(
    string Token,
    DateTime ExpiresAt,
    string UserId,
    string UserName,
    string? FullName,
    byte? CompanyId,
    string? CompanyName,
    IReadOnlyList<string> Roles
);

public record RegisterRequest(
    [Required] string UserName,
    [Required, EmailAddress] string Email,
    [Required] string Password,
    string? FullName,
    byte? CompanyId,
    string Role = "Operator"
);

public record MeResponse(
    string UserId,
    string UserName,
    string? Email,
    string? FullName,
    byte? CompanyId,
    string? CompanyName,
    IReadOnlyList<string> Roles
);
