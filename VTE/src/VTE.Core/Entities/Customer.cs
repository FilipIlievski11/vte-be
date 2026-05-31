namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class Customer : AuditableEntity
{
    public string IdentificationNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ParentName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsCompany { get; set; }
    public string? CompanyName { get; set; }
    public string? TaxNumber { get; set; }
    public string? Occupation { get; set; }
    public string? WorksInCompany { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? PassportNumber { get; set; }
    public DateTime? PassportDateIssued { get; set; }
    public string? PassportIssuer { get; set; }
    public string? DrivingLicenseNumber { get; set; }
    public string? IdentityCardNumber { get; set; }
    public bool CanSendNotifications { get; set; }
    public int Status { get; set; }
    public string? Note { get; set; }
    public long? CitizenshipId { get; set; }
    public Country? Citizenship { get; set; }
    public long? BirthCityId { get; set; }
    public City? BirthCity { get; set; }
    public long? BirthAddressStreetId { get; set; }
    public Street? BirthAddressStreet { get; set; }
    public long? LivingCityId { get; set; }
    public City? LivingCity { get; set; }
    public long? LivingAddressStreetId { get; set; }
    public Street? LivingAddressStreet { get; set; }
    public long? BusinessTypeId { get; set; }
    public BusinessType? BusinessType { get; set; }
    public List<CustomerContactPerson> ContactPersons { get; set; } = [];
    public List<CustomerBankAccount> BankAccounts { get; set; } = [];
}
