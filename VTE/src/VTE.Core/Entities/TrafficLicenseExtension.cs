namespace VTE.Core.Entities;

public class TrafficLicenseExtension : BaseEntity
{
    public long TrafficLicenseId { get; set; }
    public TrafficLicense TrafficLicense { get; set; } = null!;
    public DateTime ExtensionDate { get; set; }
    public DateTime ValidUntilDate { get; set; }
    public string? Note { get; set; }
}
