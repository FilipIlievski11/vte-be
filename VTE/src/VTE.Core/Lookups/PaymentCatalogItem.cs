namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class PaymentCatalogItem : LookupEntity
{
    public long PaymentCategoryId { get; set; }
    public PaymentCategory PaymentCategory { get; set; } = null!;
    public long VehiclePaymentCategoryId { get; set; }
    public VehiclePaymentCategory VehiclePaymentCategory { get; set; } = null!;
}
