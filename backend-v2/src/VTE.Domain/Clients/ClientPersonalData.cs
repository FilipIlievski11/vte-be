namespace VTE.Domain.Clients;

public class ClientPersonalData
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public byte PersonalDataTypeId { get; set; }
    public byte DocumentIssuerId { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Active { get; set; } = true;
}
