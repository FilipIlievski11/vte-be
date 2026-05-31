namespace VTE.Domain.Customers;

public class CustomerContactPerson
{
    public int Id { get; set; }
    public long CustomerId { get; set; }
    public string? EMBG { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class CustomerBankAccount
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public string BankAccount { get; set; } = string.Empty;
    public string DeponentBank { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
