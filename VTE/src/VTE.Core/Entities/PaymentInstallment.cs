namespace VTE.Core.Entities;

public class PaymentInstallment : BaseEntity
{
    public long PaymentDocumentId { get; set; }
    public PaymentDocument PaymentDocument { get; set; } = null!;
    public int InstallmentNumber { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
}
