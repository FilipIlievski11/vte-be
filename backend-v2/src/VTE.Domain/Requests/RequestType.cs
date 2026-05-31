namespace VTE.Domain.Requests;

/// <summary>
/// Workflow definition for a <see cref="Request"/>. The flags drive what each
/// Request must validate and what side-effects fire when it is ended.
/// </summary>
public class RequestType
{
    public byte Id { get; set; }
    public byte? ParentRequestTypeId { get; set; }       // self-FK; nullable
    public byte DocumentPrintId { get; set; }            // -> RequestDocumentPrint

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Workflow flags (renamed from legacy + cleaned up typos)
    public TechnicalExamRequirement TechnicalExamRequirement { get; set; }
    public bool PaymentRequired { get; set; }
    public bool IssuesNewRegistration { get; set; }
    public bool DeactivatesRelation { get; set; }
    public bool DeactivatesVehicle { get; set; }
    public bool TransfersOwnership { get; set; }         // requires NewClientVehicleRelationId
    public bool MutatesVehicleData { get; set; }
    public bool MutatesClientData { get; set; }
    public bool IsSufficient { get; set; }
    public bool PreviousRegistrationRequired { get; set; }

    public bool Active { get; set; } = true;
}
