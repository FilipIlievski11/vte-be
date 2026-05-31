namespace VTE.Core.Entities;

public class TrafficLicense : BaseEntity
{
    public long DocumentId { get; set; }
    public Document Document { get; set; } = null!;
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime? ValidUntilDate { get; set; }
    public string? PlateNumber { get; set; }
    public List<TrafficLicenseExtension> Extensions { get; set; } = [];
}
