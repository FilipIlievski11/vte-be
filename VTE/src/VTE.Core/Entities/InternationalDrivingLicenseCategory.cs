namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class InternationalDrivingLicenseCategory : BaseEntity
{
    public long InternationalDrivingLicenseId { get; set; }
    public InternationalDrivingLicense InternationalDrivingLicense { get; set; } = null!;
    public long DrivingLicenseCategoryId { get; set; }
    public DrivingLicenseCategory DrivingLicenseCategory { get; set; } = null!;
}
