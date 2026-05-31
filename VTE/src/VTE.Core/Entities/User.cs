namespace VTE.Core.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? LegacyPasswordHash { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? IdentityCardNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfHiring { get; set; }
    public string? RFID { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public long OrganizationId { get; set; }
    public TechnicalExamOrganization Organization { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
