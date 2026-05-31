using VTE.Domain.Common;

namespace VTE.Domain.Customers;

// Customer entity — see docs/superpowers/work/customers-business-rules.md
// for the BR-CUS-* rule references.
public class Customer : TenantEntity
{
    public long Id { get; set; }
    public bool IsCompany { get; set; }                    // legacy IsCompany flag

    // Personal / company identity (BR-CUS-001..005)
    public string? EMBG { get; set; }                      // BR-CUS-001 max 13; unique-per-tenant index
    public string FirstName { get; set; } = string.Empty;  // BR-CUS-005 required
    public string? Surname { get; set; }
    public string? ParentName { get; set; }
    public DateOnly? DateOfBirth { get; set; }             // BR-CUS-011

    // FKs to REF (deferred to ID-only fields for now; navigation properties added when needed)
    public int? CitizenshipId { get; set; }
    public int? BusinessTypeId { get; set; }
    public int? LivingAddressId { get; set; }
    public int? LivingCityId { get; set; }
    public int? BirthCityId { get; set; }
    public int? BirthAddressId { get; set; }

    public string? LivingAddressNumber { get; set; }       // BR-CUS-008
    public string? BirthAddressNumber { get; set; }        // BR-CUS-009 (typo fixed from "Brith")
    public string? Occupation { get; set; }                // BR-CUS-010
    public string? WorksInCompany { get; set; }

    // Contact (BR-CUS-006, BR-CUS-007, BR-CUS-012, BR-CUS-013)
    public string? PhoneNumber { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public bool CanSendNotifications { get; set; }         // BR-CUS-013: when true, Email required (enforce at app)

    // Identity documents (typo fixes: BLK→IDCard, Driveing→Driving)
    public string? IDCardNumber { get; set; }
    public DateOnly? IDCardDateIssued { get; set; }
    public int? IDCardIssuerId { get; set; }
    public string? PassportNumber { get; set; }
    public DateOnly? PassportDateIssued { get; set; }
    public int? PassportIssuerId { get; set; }
    public string? DrivingLicenceNumber { get; set; }
    public DateOnly? DrivingLicenceDateIssued { get; set; }
    public int? DrivingLicenceIssuerId { get; set; }

    public string? TaxNumber { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
}
