using VTE.Domain.Common;

namespace VTE.Domain.InternationalDrivingLicences;

/// <summary>
/// A client's application for / issuance of an International Driving Licence
/// (Меѓународна возачка дозвола). Person-level — no vehicle involved. Which
/// categories the licence covers lives in <see cref="InternationalDrivingLicenceCategory"/>.
///
/// Legacy source: <c>DocumentsInternationalDriveingLicences</c>.
/// </summary>
public class InternationalDrivingLicence : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    /// <summary>The applicant. Legacy: IdCustomer.</summary>
    public long ClientId { get; set; }

    /// <summary>Issuing station/organization. Legacy: IdIssuer (→ TehnicalExamOrganizations).</summary>
    public int IssuerOrganizationId { get; set; }

    /// <summary>The international licence number being issued. Legacy: NumberOfLicence.
    /// Must be unique — legacy rule: "Бројот на дозволата мора да биде единствен".</summary>
    public string NumberOfLicence { get; set; } = string.Empty;

    /// <summary>The applicant's existing national driving-licence number. Legacy: NumberOfNationalLicence.</summary>
    public string NumberOfNationalLicence { get; set; } = string.Empty;

    public DateTime IssuedDate { get; set; }
    public DateTime ValidTillDate { get; set; }

    public string? Note { get; set; }

    // ---- Applicant snapshot ----
    // Auto-filled from the picked client when the licence is created, but freely editable
    // and stored HERE (on the document), NEVER written back to the Client. This makes the
    // printed licence + request-form a stable, self-contained snapshot and lets the operator
    // correct/complete details for print without mutating the client's master record.
    // Free-text (issuers/citizenship/birthplace are strings, not FKs) so they stay editable.
    public string? ApplicantFirstName { get; set; }
    public string? ApplicantLastName { get; set; }
    public string? ApplicantParentName { get; set; }
    public string? ApplicantCitizenship { get; set; }
    public DateTime? ApplicantDateOfBirth { get; set; }
    public string? ApplicantBirthPlace { get; set; }
    public string? ApplicantAddress { get; set; }
    public string? ApplicantPassportNumber { get; set; }
    public string? ApplicantPassportIssuer { get; set; }
    public DateTime? ApplicantPassportDate { get; set; }
    public DateTime? ApplicantPassportExpiry { get; set; }
    public string? ApplicantIdCardNumber { get; set; }
    public string? ApplicantIdCardIssuer { get; set; }
    public DateTime? ApplicantIdCardDate { get; set; }
    public DateTime? ApplicantIdCardExpiry { get; set; }
    public string? ApplicantNationalLicenceIssuer { get; set; }
    public DateTime? ApplicantNationalLicenceDate { get; set; }
    /// <summary>National licence "датум на важење" — printed on the request form (page 1, section 1).</summary>
    public DateTime? ApplicantNationalLicenceExpiry { get; set; }

    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }

    /// <summary>Source row id in the legacy VERTEST.DocumentsInternationalDriveingLicences
    /// registry when migrated/synced from there; null for v2-native licences. Unique
    /// (filtered) — makes migrate-idl-from-vertest.sql idempotent. Note: NumberOfLicence
    /// uniqueness is only enforced for v2-native rows (LegacyId IS NULL) — the historical
    /// registry contains reused serials.</summary>
    public long? LegacyId { get; set; }

    /// <summary>EF concurrency token.</summary>
    public byte[] RowVersion { get; set; } = [];
}
