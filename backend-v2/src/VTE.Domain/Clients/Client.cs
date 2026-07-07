using VTE.Domain.Common;

namespace VTE.Domain.Clients;

public class Client : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }
    public int? CityId { get; set; }
    public byte? CitizenshipId { get; set; }
    public bool? Business { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? MB { get; set; }
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Note { get; set; }

    // ---- Profile fields (legacy Customers parity — added for the Полномошна screen) ----
    /// <summary>Име на родител. Legacy: ParentName.</summary>
    public string? ParentName { get; set; }
    /// <summary>Место на раѓање. Legacy: IdBirhCity [sic].</summary>
    public int? BirthCityId { get; set; }
    /// <summary>Legacy: Fax.</summary>
    public string? Fax { get; set; }
    /// <summary>Професија. Legacy: Occupation.</summary>
    public string? Profession { get; set; }
    /// <summary>Работодавач. Legacy: WorksInCompany.</summary>
    public string? Employer { get; set; }
    /// <summary>Дозвола за известување. Legacy: CanSendNotifications.</summary>
    public bool? NotificationsAllowed { get; set; }

    public bool? Active { get; set; } = true;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
