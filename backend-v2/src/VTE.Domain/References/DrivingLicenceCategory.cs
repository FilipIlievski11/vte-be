namespace VTE.Domain.References;

/// <summary>
/// Fixed enumeration of the 16 national/international driving-licence categories
/// (A, B, C, D, E, A1, C1, D1, BE, C1E, CE, D1E, DE, G, F, M). Ids 3-18 are preserved
/// 1:1 with legacy <c>DriveingLicenceCtegories</c> so a future historical migration of
/// International Driving Licences lines up without remapping.
/// </summary>
public class DrivingLicenceCategory
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Active { get; set; } = true;
}
