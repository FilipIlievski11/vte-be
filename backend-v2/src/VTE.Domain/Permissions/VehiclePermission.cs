using VTE.Domain.Common;

namespace VTE.Domain.Permissions;

/// <summary>
/// Одобрение за управување со туѓо возило (Полномошно) — the vehicle owner authorizes
/// another person to drive the vehicle. Prints a request form + the A4-landscape
/// authorization certificate.
///
/// Legacy source: <c>DocumentsPermisions</c> [sic]. In legacy BOTH parties were bound via
/// <c>CustomerVehiclesRelations</c>, but all 2,784 historical rows bind the authorized
/// person through a vehicle-less "не е потребно возило" relation — i.e. the authorized
/// party is effectively just a Client. v2 models that directly: the OWNER keeps the real
/// vehicle relation (debts anchor there, mirroring legacy
/// <c>insertFinancialStatePriceCatalogForPermisions</c>), the authorized person is a
/// plain <see cref="AuthorizedClientId"/>.
/// </summary>
public class VehiclePermission : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    /// <summary>The OWNER's client↔vehicle relation (who authorizes). Debts anchor here.
    /// Legacy: IdCustomerVehicleRelationOwner.</summary>
    public long ClientVehicleRelationId { get; set; }

    /// <summary>The authorized person (who may drive). Legacy: IdCustomerVehicleRelation → its client.</summary>
    public long AuthorizedClientId { get; set; }

    /// <summary>Who issued the document (МВР…). Legacy: IdIssuer → RegistrationIssuers = v2 DocumentIssuer.</summary>
    public byte IssuerId { get; set; }

    /// <summary>City where the permission is issued. Legacy: IdCityOfIssuing.</summary>
    public int IssuingCityId { get; set; }

    /// <summary>Issuing station/organization. Legacy: IdOrganisation (→ TehnicalExamOrganizations).</summary>
    public int IssuerOrganizationId { get; set; }

    /// <summary>Manual document number. Legacy: PermissionNumber (nullable there too).</summary>
    public string? PermissionNumber { get; set; }

    /// <summary>Сообраќајна дозвола бр. — manual entry (the TrafficLicences module is legacy-only).
    /// Required, mirrors the legacy hard rule.</summary>
    public string TrafficLicenceNumber { get; set; } = string.Empty;

    /// <summary>Optional carnet number. Legacy: TriptiqueNumber.</summary>
    public string? TriptiqueNumber { get; set; }

    public DateTime IssuedDate { get; set; }        // legacy DateCreated
    public DateTime? StartDate { get; set; }        // legacy DateStart
    public DateTime ValidTillDate { get; set; }

    public string? Note { get; set; }

    // ---- Print snapshot ----
    // Auto-filled from the picked vehicle/owner/authorized client, freely editable, and
    // stored HERE so reprints stay stable even if the master records change later
    // (same principle as InternationalDrivingLicence.Applicant*).
    public string? OwnerName { get; set; }              // owner display (person or firm)
    public string? OwnerIdNumber { get; set; }          // ЕМБГ / МБ (legacy OwnerMB / OwnerBLK context)
    public string? OwnerAddress { get; set; }           // street + no + city
    public string? AuthorizedName { get; set; }
    public string? AuthorizedEmbg { get; set; }
    public string? AuthorizedIdCardNumber { get; set; } // legacy BLK
    public string? AuthorizedPassportNumber { get; set; } // legacy CustomerPasswordNumber [sic]
    public string? AuthorizedAddress { get; set; }
    public string? VehicleDisplay { get; set; }         // make + model (+ adding)
    public string? PlateNumber { get; set; }            // legacy LastRegistratinNumber [sic]
    public string? VehicleVin { get; set; }             // shell number
    public string? VehicleEngineNumber { get; set; }

    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }

    /// <summary>Source row id in the live legacy VERTEST.DocumentsPermisions registry when
    /// migrated/synced from there; null for v2-native permissions. Unique (filtered) —
    /// makes migrate-permissions-from-vertest.sql idempotent. The (authorized, relation)
    /// active-uniqueness rule only applies to v2-native rows — the historical registry
    /// has 803 repeated active pairs.</summary>
    public long? LegacyId { get; set; }

    /// <summary>EF concurrency token.</summary>
    public byte[] RowVersion { get; set; } = [];
}
