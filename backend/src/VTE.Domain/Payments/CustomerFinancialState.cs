namespace VTE.Domain.Payments;

public class CustomerFinancialState
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public int StationId { get; set; }
    public DateOnly Date { get; set; }
    public string? Description { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public decimal RunningBalance { get; set; }
    public long? PaymentDocumentId { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
