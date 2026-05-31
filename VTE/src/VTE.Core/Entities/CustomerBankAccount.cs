namespace VTE.Core.Entities;

public class CustomerBankAccount : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string BankName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? Note { get; set; }
}
