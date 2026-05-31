namespace VTE.Domain.Requests;

/// <summary>
/// Catalog of ownership-proof kinds (sales contract, inheritance, gift deed…).
/// Replaces legacy <c>DocumentVehicleOwnershipProof</c>.
/// </summary>
public class RequestOwnershipProofType
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
