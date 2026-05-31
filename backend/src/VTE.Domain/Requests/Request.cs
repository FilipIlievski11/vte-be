namespace VTE.Domain.Requests;

// Request workflow instance — every customer interaction starts here.
// Lifecycle: DateCreated → DateModified → DateEnded.
// While DateEnded is null the request is "open".
public class Request
{
    public long Id { get; set; }
    public int StationId { get; set; }
    public int RequestTypeId { get; set; }                                      // BR-REQ-003
    public long CustomerVehicleRelationId { get; set; }                         // BR-REQ-004 current owner
    public long? NewCustomerVehicleRelationId { get; set; }                     // BR-REQ-005 new owner (when RequestType.IsNewCustomer)
    public long? TechnicalExamReportId { get; set; }
    public long? PreviousRegistrationId { get; set; }
    public int? TechnicalExamOrganizationId { get; set; }

    public DateOnly DateCreated { get; set; }                                   // BR-REQ-001
    public DateOnly? DateModified { get; set; }
    public DateOnly? DateEnded { get; set; }

    public string? CreatedByOperatorUserId { get; set; }
    public string? ModifiedByOperatorUserId { get; set; }
    public string? EndedByOperatorUserId { get; set; }

    public bool IsCustomerChanged { get; set; }
    public bool IsVehicleChanged { get; set; }

    public string? Note { get; set; }                                           // BR-REQ-002 max 250

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public RequestType? RequestType { get; set; }
}
