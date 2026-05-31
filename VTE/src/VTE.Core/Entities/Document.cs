namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class Document : AuditableEntity
{
    public long DocumentTypeId { get; set; }
    public DocumentType DocumentType { get; set; } = null!;
    public long? DocumentTypeOptionId { get; set; }
    public DocumentTypeOption? DocumentTypeOption { get; set; }
    public long? DocumentTypeOptionDetailId { get; set; }
    public DocumentTypeOptionDetail? DocumentTypeOptionDetail { get; set; }
    public long CustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation CustomerVehicleRelation { get; set; } = null!;
    public long? TechnicalExamReportId { get; set; }
    public TechnicalExamReport? TechnicalExamReport { get; set; }
    public long? PreviousRegistrationId { get; set; }
    public long? OwnershipProofId { get; set; }
    public OwnershipProofType? OwnershipProof { get; set; }
    public long? PaymentProofId { get; set; }
    public PaymentProofType? PaymentProof { get; set; }
    public string? Note { get; set; }
    public DateTime? DateEnded { get; set; }
    public long? EndedByUserId { get; set; }
    public User? EndedByUser { get; set; }
    public List<DocumentAttachment> Attachments { get; set; } = [];
}
