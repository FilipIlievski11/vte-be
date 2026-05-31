namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class Request : AuditableEntity
{
    public long RequestTypeId { get; set; }
    public RequestType RequestType { get; set; } = null!;
    public long CustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation CustomerVehicleRelation { get; set; } = null!;
    public long? NewCustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation? NewCustomerVehicleRelation { get; set; }
    public long? TechnicalExamReportId { get; set; }
    public TechnicalExamReport? TechnicalExamReport { get; set; }
    public long? PreviousRegistrationId { get; set; }
    public bool IsCustomerChanged { get; set; }
    public bool IsVehicleChanged { get; set; }
    public long? OrganizationId { get; set; }
    public TechnicalExamOrganization? Organization { get; set; }
    public string? Note { get; set; }
    public DateTime? DateEnded { get; set; }
    public long? EndedByUserId { get; set; }
    public User? EndedByUser { get; set; }
    public List<RequestAttachment> Attachments { get; set; } = [];
    public List<RequestOwnershipProof> OwnershipProofs { get; set; } = [];
    public List<RequestPaymentProof> PaymentProofs { get; set; } = [];
}
