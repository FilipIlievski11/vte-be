namespace VTE.Domain.TechnicalExams;

/// <summary>
/// An inspection organization / station that issues technical-exam reports.
/// Holds the letterhead details printed on the certificate (name, address,
/// bank account, tax number, responsible officer). Kept as its own lookup
/// (rather than reusing <c>Station</c>) so legacy report → org Ids migrate 1:1.
/// Legacy: <c>TehnicalExamOrganizations</c> (print-relevant columns only).
/// </summary>
public class TechnicalExamOrganization
{
    public int Id { get; set; }

    /// <summary>Owning company grouping in legacy (0 → null). Legacy: IdCompany.</summary>
    public byte? CompanyId { get; set; }

    /// <summary>Station/organization code used in RegNumber. Legacy: Code.</summary>
    public string? Code { get; set; }

    /// <summary>Station/organization name. Legacy: Station.</summary>
    public string? Name { get; set; }

    /// <summary>Geography (loose — legacy ids, no FK). Legacy: IdCity / IdCommunity.</summary>
    public int? CityId { get; set; }
    public int? CommunityId { get; set; }

    /// <summary>Legacy: StationAddress.</summary>
    public string? Address { get; set; }
    /// <summary>Legacy: Tel.</summary>
    public string? Phone { get; set; }
    /// <summary>Legacy: Fax.</summary>
    public string? Fax { get; set; }

    /// <summary>Bank (giro) account number printed on payment docs. Legacy: ZiroSmetka.</summary>
    public string? BankAccount { get; set; }
    /// <summary>Depository bank. Legacy: Deponent.</summary>
    public string? Depositor { get; set; }
    /// <summary>Tax id. Legacy: EDB.</summary>
    public string? TaxNumber { get; set; }

    /// <summary>Responsible officer signed on the report. Legacy: OdgovorenOrgan.</summary>
    public string? ResponsibleOfficer { get; set; }
    /// <summary>Secretary signed on the report. Legacy: Sekretar.</summary>
    public string? Secretary { get; set; }

    public bool Active { get; set; } = true;
}
