namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class RequestPaymentProof : BaseEntity
{
    public long RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public long PaymentProofTypeId { get; set; }
    public PaymentProofType PaymentProofType { get; set; } = null!;
    public string? Note { get; set; }
}
