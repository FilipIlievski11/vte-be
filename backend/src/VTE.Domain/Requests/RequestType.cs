namespace VTE.Domain.Requests;

// Configurable workflow definition — see docs/superpowers/work/requests-business-rules.md.
// The 10 boolean / int flags drive Request validation and side-effects.
public class RequestType
{
    public int Id { get; set; }
    public int? ParentRequestTypeId { get; set; }
    public int? DocumentPrintId { get; set; }
    public string TypeName { get; set; } = string.Empty;       // BR-REQ-020 required, max 250
    public string? TypeDescription { get; set; }               // BR-REQ-021 max 250

    // Workflow flags
    public int IsTechnicalExamRequired { get; set; }           // tri-state: 0=no, 1=yes, 2=conditional
    public bool IsPayRequired { get; set; }
    public bool IsNewRegistration { get; set; }
    public bool IsRelationDeleted { get; set; }
    public bool IsVehicleDeleted { get; set; }
    public bool IsNewCustomer { get; set; }                    // when true, NewCustomerVehicleRelationId required (BR-REQ-005)
    public bool IsVehicleChanged { get; set; }
    public bool IsCustomerChanged { get; set; }
    public bool IsSufficient { get; set; }
    public bool IsPreviousRegistrationRequired { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
