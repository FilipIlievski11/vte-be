using System.ComponentModel.DataAnnotations;

namespace VTE.Api.Dtos;

public record UserListItem(
    string Id,
    string UserName,
    string? FullName,
    string? Email,
    byte? CompanyId,
    string? CompanyName,
    string Role,
    bool IsActive,
    DateTime CreatedAt
);

public record UserCreateRequest(
    [Required] string UserName,
    [Required, EmailAddress] string Email,
    [Required] string Password,
    string? FullName,
    byte? CompanyId,
    string Role = "Operator"
);

public record UserUpdateRequest(
    string? FullName,
    [EmailAddress] string? Email,
    byte? CompanyId,
    bool? IsActive
);

public record ResetPasswordRequest(
    [Required] string NewPassword
);
