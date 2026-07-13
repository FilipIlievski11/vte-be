namespace VTE.Domain.Payments;

/// <summary>
/// How a PaymentDocument is collected (cash, card, installment, invoice…) and what
/// physical artefacts the system prints. Legacy: PaymentTypes (30 rows). Cross-tenant
/// catalog — the same payment types apply across companies.
/// </summary>
public class PaymentType
{
    public int Id { get; set; }

    /// <summary>Stable short code, e.g. "CASH", "CARD", "INV", "RATA". Legacy: not present.</summary>
    public string? Code { get; set; }

    /// <summary>Display name. Legacy: Name.</summary>
    public string Name { get; set; } = "";

    /// <summary>Cash sale → fiscal receipt printed. Legacy: Fiskalna_kes.</summary>
    public bool IsCash { get; set; }

    /// <summary>Card sale → fiscal receipt printed (card flag). Legacy: Fiskalna_karticka.</summary>
    public bool IsCard { get; set; }

    /// <summary>Sold on an installment plan (links to <see cref="InstallmentAgreement"/>). Legacy: Rati.</summary>
    public bool IsInstallment { get; set; }

    /// <summary>Prints a fiscal receipt (Smetka). Legacy: Smetka.</summary>
    public bool PrintsReceipt { get; set; }

    /// <summary>Prints an invoice (Faktura). Legacy: Faktura.</summary>
    public bool PrintsInvoice { get; set; }

    /// <summary>DocumentNumber prefix (e.g. "F" for Faktura). Legacy: Prefix.</summary>
    public string? Prefix { get; set; }

    /// <summary>Пари реално наплатени на шалтер (готово/картичка/договор/фактура) — false за
    /// авансно/вирмански/поништување/книжно. Легаси: PayedAmount; ги дели „наплатен" од
    /// „проверен" износ во ПРЕГЛЕД ЗА НАПЛАТА (ReportByCategoryForPayment).</summary>
    public bool PayedAmount { get; set; } = true;

    public bool Active { get; set; } = true;
}
