namespace VTE.Domain.Requests;

/// <summary>Proof of payment attached to a <see cref="Request"/>.</summary>
public class RequestPaymentProof
{
    public long Id { get; set; }
    public long RequestId { get; set; }
    public byte PaymentProofTypeId { get; set; }
    public string? Detail { get; set; }
    public bool Active { get; set; } = true;
}
