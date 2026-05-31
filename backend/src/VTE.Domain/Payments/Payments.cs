namespace VTE.Domain.Payments;

// Payment domain — see docs/superpowers/work/payments-business-rules.md.

public class InstallmentContract
{
    public long Id { get; set; }
    public int StationId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;                   // BR-PAY-030 max 50
    public DateOnly ContractDate { get; set; }
    public int NumberOfInstallments { get; set; }                                // BR-PAY-034 (CHECK 2..60 in DB)
    public string? GuarantorName { get; set; }                                   // BR-PAY-031 max 50
    public string? GuarantorAddress { get; set; }                                // BR-PAY-032 max 250
    public string? GuarantorEMBG { get; set; }                                   // BR-PAY-033 max 20
    public string? Note { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class PaymentDocument
{
    public long Id { get; set; }
    public int StationId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public int PaymentTypeId { get; set; }                                       // BR-PAY-004
    public long CustomerVehicleRelationId { get; set; }                          // BR-PAY-005
    public long? InstallmentContractId { get; set; }
    public long? BillToCustomerId { get; set; }
    public int? TechnicalExamOrganizationId { get; set; }
    public DateOnly DatePay { get; set; }                                        // BR-PAY-001
    public DateOnly DateRequired { get; set; }                                   // BR-PAY-002
    public decimal DiscountPercent { get; set; }                                 // BR-PAY-012 doc-level (CHECK 0..<100)
    public bool Payed { get; set; }
    public bool Storno { get; set; }
    public decimal? PolicyNumber { get; set; }
    public string? Note { get; set; }                                            // BR-PAY-003 max 150
    public string? CreatedByOperatorUserId { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public List<PaymentDocumentDetail> Details { get; set; } = new();
    public List<PaymentDocumentInstallment> Installments { get; set; } = new();
}

public class PaymentDocumentDetail
{
    public long Id { get; set; }
    public long PaymentDocumentId { get; set; }
    public int PriceCatalogId { get; set; }                                      // BR-PAY-013 (legacy compat)
    public int? PaymentItemId { get; set; }                                      // actual product link: PaymentItem (legacy IdPriceCatalog)
    public decimal Price { get; set; }
    public decimal DDVRate { get; set; }                                         // VAT % at issue time
    public decimal DiscountPercent { get; set; }                                 // BR-PAY-012 line-level (CHECK)
    public bool PrePayed { get; set; }
    public string? Note { get; set; }                                            // BR-PAY-010
    public string? NotePrePayed { get; set; }                                    // BR-PAY-011
    public long? CustomerFinancialStateId { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class PaymentDocumentInstallment
{
    public long Id { get; set; }
    public long PaymentDocumentId { get; set; }
    public int InstallmentNumber { get; set; }
    public decimal Price { get; set; }
    public DateOnly? DueDate { get; set; }
    public bool Payed { get; set; }
    public DateOnly? DatePayed { get; set; }
    public string? Note { get; set; }                                            // BR-PAY-020
    public string? CollectedByOperatorUserId { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
