namespace VTE.Domain.Requests;

/// <summary>
/// File attached to a <see cref="Request"/>. Stores metadata in DB; the actual
/// blob lives at <see cref="StoragePath"/> (local FS for now; can be swapped
/// for Azure Blob / S3 later without schema changes).
/// </summary>
public class RequestAttachment
{
    public long Id { get; set; }
    public long RequestId { get; set; }
    public byte AttachmentTypeId { get; set; }

    public string FileName { get; set; } = string.Empty;     // operator-visible name
    public string ContentType { get; set; } = string.Empty;  // MIME
    public long SizeBytes { get; set; }
    public string StoragePath { get; set; } = string.Empty;  // resolver-relative path

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public string UploadedByUserId { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}
