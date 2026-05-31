namespace VTE.Core.Entities;

public class InternationalDrivingLicense : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime ValidUntilDate { get; set; }
    public List<InternationalDrivingLicenseCategory> ValidCategories { get; set; } = [];
}
