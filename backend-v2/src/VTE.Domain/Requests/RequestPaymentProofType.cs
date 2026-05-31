namespace VTE.Domain.Requests;

/// <summary>
/// Catalog of payment-proof kinds (uplata, fiskalna, virmanski nalog…).
/// Replaces legacy <c>DocumentPaymentProof</c>.
/// </summary>
public class RequestPaymentProofType
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
