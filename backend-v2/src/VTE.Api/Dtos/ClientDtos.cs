using System.ComponentModel.DataAnnotations;

namespace VTE.Api.Dtos;

/// <summary>Standard paged envelope returned by list endpoints (mirrors v1 shape).</summary>
public record PagedDto<T>(int Page, int PageSize, int Total, IReadOnlyList<T> Items);

public record ClientReadDto(
    long Id,
    byte CompanyId,
    int? CityId,
    byte? CitizenshipId,
    bool? Business,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? MB,
    string? Address,
    string? TaxNumber,
    string? PhoneNumber,
    string? Email,
    DateTime? DateOfBirth,
    string? Note,
    bool? Active,
    DateTime? CreatedAt,
    // Profile fields (legacy Customers parity)
    string? ParentName = null,
    int? BirthCityId = null,
    string? Fax = null,
    string? Profession = null,
    string? Employer = null,
    bool? NotificationsAllowed = null
);

public record ClientWriteDto(
    int? CityId,
    byte? CitizenshipId,
    bool? Business,
    [MaxLength(100)] string? FirstName,
    [MaxLength(100)] string? MiddleName,
    [MaxLength(100)] string? LastName,
    [MaxLength(13)] string? MB,
    [MaxLength(100)] string? Address,
    [MaxLength(50)] string? TaxNumber,
    [MaxLength(20)] string? PhoneNumber,
    [MaxLength(100), EmailAddress] string? Email,
    DateTime? DateOfBirth,
    [MaxLength(250)] string? Note,
    bool? Active,
    /// <summary>Optional admin-only override. Operators always write to their own tenant
    /// regardless of what's sent here.</summary>
    byte? CompanyId = null,
    // Profile fields (legacy Customers parity)
    [MaxLength(100)] string? ParentName = null,
    int? BirthCityId = null,
    [MaxLength(50)] string? Fax = null,
    [MaxLength(150)] string? Profession = null,
    [MaxLength(200)] string? Employer = null,
    bool? NotificationsAllowed = null
);

public record ClientPersonalDataReadDto(
    long Id,
    long ClientId,
    byte PersonalDataTypeId,
    byte DocumentIssuerId,
    string Number,
    DateTime CreatedAt,
    DateTime? ExpiresAt,
    bool Active
);

public record ClientPersonalDataWriteDto(
    [Required] long ClientId,
    [Required] byte PersonalDataTypeId,
    [Required] byte DocumentIssuerId,
    [Required, MaxLength(100)] string Number,
    /// <summary>Date issued (CreatedAt doubles as it). Null → server keeps existing / stamps now.</summary>
    DateTime? CreatedAt = null,
    DateTime? ExpiresAt = null,
    bool Active = true
);
