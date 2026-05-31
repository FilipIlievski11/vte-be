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
    public bool? Active { get; set; } = true;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
}
