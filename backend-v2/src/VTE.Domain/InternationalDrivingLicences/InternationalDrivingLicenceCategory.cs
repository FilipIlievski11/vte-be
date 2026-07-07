namespace VTE.Domain.InternationalDrivingLicences;

/// <summary>
/// One checked driving-licence category on an <see cref="InternationalDrivingLicence"/>.
/// One row per CHECKED category only — legacy inserts all 16 with an IsCheck flag instead;
/// simplified here since "row exists" already means "checked".
/// Legacy source: <c>DocumentsInternationalDriveingLicences.ValidForCategories</c>.
/// </summary>
public class InternationalDrivingLicenceCategory
{
    public long Id { get; set; }

    /// <summary>Parent licence. Legacy: IdInternationalDrivingLicence.</summary>
    public long InternationalDrivingLicenceId { get; set; }

    /// <summary>Legacy: IdLicenceCategorie.</summary>
    public int DrivingLicenceCategoryId { get; set; }
}
