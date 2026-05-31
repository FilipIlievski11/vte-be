namespace VTE.Domain.Requests;

/// <summary>Proof of vehicle ownership attached to a <see cref="Request"/>.</summary>
public class RequestOwnershipProof
{
    public long Id { get; set; }
    public long RequestId { get; set; }
    public byte OwnershipProofTypeId { get; set; }
    public string? Detail { get; set; }              // free text: "Contract #1234 signed at notary X"
    public bool Active { get; set; } = true;
}
