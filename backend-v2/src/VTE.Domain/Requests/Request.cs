using VTE.Domain.Common;

namespace VTE.Domain.Requests;

/// <summary>
/// A single operator transaction (work order) at a station. Anchors a Client +
/// Vehicle (via ClientVehicleRelation), captures proofs and attachments, and
/// when ended triggers side-effects defined by <see cref="RequestType"/> flags.
/// Status is derived from the three nullable timestamps:
///   <c>EndedAt is null</c> => OPEN; <c>EndedAt is not null</c> => CLOSED.
/// </summary>
public class Request : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    public byte RequestTypeId { get; set; }

    // Anchors
    public long ClientVehicleRelationId { get; set; }            // BR-REQ-004 — required
    public long? NewClientVehicleRelationId { get; set; }        // required when RequestType.TransfersOwnership

    // Optional ties
    public long? TechnicalExamReportId { get; set; }             // future module
    public long? PreviousRegistrationId { get; set; }            // FK to VehicleRegistration

    // Lifecycle dates (state is derived)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    // Audit (user IDs are ASP.NET Identity strings)
    public string CreatedByUserId { get; set; } = string.Empty;
    public string? ModifiedByUserId { get; set; }
    public string? EndedByUserId { get; set; }

    // Trace of mutations actually performed at end-time
    public bool VehicleDataChanged { get; set; }
    public bool ClientDataChanged { get; set; }

    public string? Note { get; set; }
    public bool Active { get; set; } = true;

    /// <summary>
    /// Legacy reference number printed at the bottom of each Zelen/Plav/Bel
    /// form. Format: {StationCode}{IdTechnicalExamReport}{OperatorId}/{Year}.
    /// For migrated rows this is filled by backfill-request-reference-no.sql.
    /// For new rows it stays NULL until the Technical Exam module is built.
    /// </summary>
    public string? LegacyReferenceNumber { get; set; }

    // EF concurrency token
    public byte[] RowVersion { get; set; } = [];
}
