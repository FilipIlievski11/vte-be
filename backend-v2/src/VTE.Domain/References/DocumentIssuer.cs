namespace VTE.Domain.References;

public class DocumentIssuer
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    /// <summary>The community this registration issuer (MVR office) serves — legacy
    /// RegistrationIssuers.IdCommunity, dropped in the original migration and restored
    /// so the Plav print can show the new owner's destination MVR. No FK (loose link).</summary>
    public int? CommunityId { get; set; }
}
