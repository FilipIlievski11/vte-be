namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class RequestOwnershipProof : BaseEntity
{
    public long RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public long OwnershipProofTypeId { get; set; }
    public OwnershipProofType OwnershipProofType { get; set; } = null!;
    public string? Note { get; set; }
}
