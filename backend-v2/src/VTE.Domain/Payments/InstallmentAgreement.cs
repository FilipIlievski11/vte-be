using VTE.Domain.Common;

namespace VTE.Domain.Payments;

/// <summary>
/// Installment-plan agreement (Договор за рати). Stores the guarantor's identity and
/// the total scheduled installments. Linked from <see cref="PaymentDocument.AgreementId"/>.
///
/// Legacy: DogovorZaRati (6,796 rows).
/// </summary>
public class InstallmentAgreement : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    /// <summary>Agreement number (per-company sequence). Legacy: Broj.</summary>
    public string Number { get; set; } = "";

    /// <summary>Agreement date. Legacy: Datum.</summary>
    public DateOnly Date { get; set; }

    /// <summary>Number of scheduled installments. Legacy: BrNaRati.</summary>
    public int TotalInstallments { get; set; }

    /// <summary>Guarantor full name. Legacy: GarantNaziv.</summary>
    public string? GuarantorName { get; set; }

    /// <summary>Guarantor address. Legacy: GarantAdresa.</summary>
    public string? GuarantorAddress { get; set; }

    /// <summary>Guarantor EMBG (personal id). Legacy: GartEMB.</summary>
    public string? GuarantorEmbg { get; set; }

    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
}
