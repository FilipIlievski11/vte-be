namespace VTE.Domain.Attachments;

public abstract class AttachmentBase
{
    public long Id { get; set; }
    public int? AttachmentTypeId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public string? Description { get; set; }
    public string? UploadedByUserId { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class CustomerAttachment              : AttachmentBase { public long CustomerId { get; set; } }
public class VehicleAttachment               : AttachmentBase { public long VehicleId { get; set; } }
public class RequestAttachment               : AttachmentBase { public long RequestId { get; set; } }
public class TechnicalExamReportAttachment   : AttachmentBase { public long TechnicalExamReportId { get; set; } }
public class PaymentDocumentAttachment       : AttachmentBase { public long PaymentDocumentId { get; set; } }
