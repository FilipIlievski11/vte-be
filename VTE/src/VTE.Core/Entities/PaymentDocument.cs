namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class PaymentDocument : AuditableEntity
{
    public long PaymentTypeId { get; set; }
    public PaymentType PaymentType { get; set; } = null!;
    public long CustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation CustomerVehicleRelation { get; set; } = null!;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal DiscountPercent { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCancelled { get; set; }
    public decimal? InsurancePolicy { get; set; }
    public long? AgreementId { get; set; }
    public long? InvoicedToId { get; set; }
    public long? OrganizationId { get; set; }
    public TechnicalExamOrganization? Organization { get; set; }
    public string? Note { get; set; }
    public List<PaymentLineItem> LineItems { get; set; } = [];
    public List<PaymentInstallment> Installments { get; set; } = [];
}
