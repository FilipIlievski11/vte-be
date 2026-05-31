namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class RequestAttachment : BaseEntity
{
    public long RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public long AttachmentTypeId { get; set; }
    public AttachmentType AttachmentType { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
    public byte[]? FileData { get; set; }
    public string? Note { get; set; }
}
