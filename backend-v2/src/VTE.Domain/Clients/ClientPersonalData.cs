namespace VTE.Domain.Clients;

public class ClientPersonalData
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public byte PersonalDataTypeId { get; set; }
    public byte DocumentIssuerId { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>Document expiry ("важи до"). Null for legacy rows — the old system never stored it.</summary>
    public DateTime? ExpiresAt { get; set; }
    public bool Active { get; set; } = true;
}
