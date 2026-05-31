namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class PaymentLineItem : BaseEntity
{
    public long PaymentDocumentId { get; set; }
    public PaymentDocument PaymentDocument { get; set; } = null!;
    public long PaymentItemId { get; set; }
    public PaymentCatalogItem PaymentItem { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal VATPercent { get; set; }
    public int SortOrder { get; set; }
    public decimal VATAmount => UnitPrice * Quantity * VATPercent / 100m;
    public decimal TotalAmount => UnitPrice * Quantity + VATAmount;
}
