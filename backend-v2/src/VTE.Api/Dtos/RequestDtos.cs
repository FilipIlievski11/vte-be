using System.ComponentModel.DataAnnotations;

namespace VTE.Api.Dtos;

/// <summary>One row in the operator's Requests list. Joins are resolved server-side
/// so the UI can render without a second round-trip.</summary>
public record RequestListItem(
    long Id,
    byte CompanyId,
    byte RequestTypeId,
    string? RequestTypeName,
    long ClientVehicleRelationId,
    string? ClientDisplayName,
    long? VehicleId,
    string? VehicleVin,
    string? VehiclePlate,
    DateTime CreatedAt,
    DateTime? ModifiedAt,
    DateTime? EndedAt,
    string? CreatedByUserName,
    string? Note,
    bool Active);

public record RequestReadDto(
    long Id,
    byte CompanyId,
    byte RequestTypeId,
    long ClientVehicleRelationId,
    long? NewClientVehicleRelationId,
    long? TechnicalExamReportId,
    long? PreviousRegistrationId,
    DateTime CreatedAt,
    DateTime? ModifiedAt,
    DateTime? EndedAt,
    string CreatedByUserId,
    string? ModifiedByUserId,
    string? EndedByUserId,
    string? CreatedByUserName,
    string? ModifiedByUserName,
    string? EndedByUserName,
    bool VehicleDataChanged,
    bool ClientDataChanged,
    string? Note,
    bool Active,
    string? LegacyReferenceNumber,
    byte[] RowVersion);

public record RequestWriteDto(
    [Required] byte RequestTypeId,
    [Required] long ClientVehicleRelationId,
    long? NewClientVehicleRelationId,
    long? TechnicalExamReportId,
    long? PreviousRegistrationId,
    [MaxLength(500)] string? Note,
    bool? Active,
    /// <summary>Admin-only override; operators always write to their own tenant.</summary>
    byte? CompanyId = null);
