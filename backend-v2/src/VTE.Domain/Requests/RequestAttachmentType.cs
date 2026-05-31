namespace VTE.Domain.Requests;

/// <summary>
/// Catalog of attachment kinds (scanned ID card, traffic licence photo, …).
/// Replaces legacy <c>AttachmentTypes</c>.
/// </summary>
public class RequestAttachmentType
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
