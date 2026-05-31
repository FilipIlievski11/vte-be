namespace VTE.Domain.Reference;

// Lightweight reference-data entities. They share a uniform shape (Id + Name + IsActive + audit
// + RowVersion) and are mapped via convention in VteDbContext. Many of these are global
// (no StationId) per the bootstrap-reference-data.sql convention.

public abstract class RefBase
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public abstract class RefBaseInt : RefBase { public int Id { get; set; } }

public class Country : RefBaseInt { public string? Iso2 { get; set; } public string? Iso3 { get; set; } public string? Citizenship { get; set; } }
public class Community : RefBaseInt { public int? CountryId { get; set; } public string? CommunityCode { get; set; } public string? RegistrationCode { get; set; } }
public class City : RefBaseInt { public string? PostalCode { get; set; } public int? CommunityId { get; set; } public int? CountryId { get; set; } public string? Description { get; set; } }
public class Street : RefBaseInt { public int? CityId { get; set; } }
public class BusinessType : RefBaseInt { }
public class RegistrationIssuer : RefBaseInt { }
public class CustomerVehicleRelationType : RefBaseInt { }
public class VehicleBodyType : RefBaseInt { public string? Code { get; set; } public string? OldName { get; set; } }
public class VehicleCategory : RefBaseInt
{
    public string? Code { get; set; }
    public string? OldName { get; set; }
    public string? Mksjus { get; set; }
    public string? Iso { get; set; }
    public string? MksjusDescription { get; set; }
    public string? PicturePath { get; set; }
    public string? Description { get; set; }
    public string? DetailDescription { get; set; }
}

// Child rows for VehicleCategory — define valid (BodyType + Use + PaymentCategory) combinations
// per category, and which fields are required/disabled on the Vehicle form.
public class VehicleCategoryRelation
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int? BodyTypeId { get; set; }
    public int? UseId { get; set; }
    public int? CategoryForPaymentsId { get; set; }
    public string? DetailDescription { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class VehicleCategoryRequiredField
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class VehicleCategoryDisabledField
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
public class VehicleUse : RefBaseInt { }
public class VehicleMaker : RefBaseInt { }
public class VehicleModel : RefBaseInt { public int VehicleMakerId { get; set; } }
public class VehicleEngineType : RefBaseInt { }
public class VehicleEnginePowerSourceType : RefBaseInt { }
public class VehicleEngineEcoProgram : RefBaseInt { }
public class VehicleGearBox : RefBaseInt { }
public class VehicleBrake : RefBaseInt { }
public class VehicleSupporting : RefBaseInt { }
public class Color : RefBaseInt { public string? HexCode { get; set; } }
public class VehicleCategoryForPayments : RefBaseInt { }
public class VehicleTireType : RefBaseInt { public string? Dimensions { get; set; } }
public class TechnicalExamOrganization : RefBaseInt { public string? Address { get; set; } public string? TaxNumber { get; set; } public string? Phone { get; set; } }
public class TechnicalExamType : RefBaseInt { public int? ValidityMonths { get; set; } }
public class TechnicalExamVehiclePartCategory : RefBaseInt { public int? SortOrder { get; set; } }
public class TechnicalExamVehiclePart : RefBaseInt { public int? CategoryId { get; set; } public int? SortOrder { get; set; } }
public class TechnicalExamReportDetailStatus : RefBaseInt { public bool IsPass { get; set; } }
public class DrivingLicenceCategory : RefBaseInt { public string? Description { get; set; } }
public class VehicleOwnershipProofType : RefBaseInt { }
public class PaymentProofType : RefBaseInt { }

public class PaymentType : RefBaseInt
{
    public bool IsInvoice { get; set; }                  // legacy Faktura
    public bool IsCash { get; set; }                     // legacy Fiskalna_kes
    public bool IsFiscalCard { get; set; }               // legacy Fiskalna_karticka
    public bool IsAccount { get; set; }                  // legacy Smetka
    public bool IsInstallments { get; set; }             // legacy Rati
    public string? Prefix { get; set; }                  // legacy Prefix used in DocumentNumber template
    public string? PrintText { get; set; }               // legacy PrintText shown on receipt
    public bool PayedAmount { get; set; }                // legacy PayedAmount flag
}

public class DDVCatalog
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateOnly EffectiveFrom { get; set; }
    public DateOnly? EffectiveTo { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class CalculationItem
{
    public int Id { get; set; }
    public int StationId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string BankAccount { get; set; } = string.Empty;
    public string Bank { get; set; } = string.Empty;
    public string Form { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class PriceCatalog
{
    public int Id { get; set; }
    public int StationId { get; set; }
    public int? CalculationItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int? DDVId { get; set; }
    public DateOnly? EffectiveFrom { get; set; }
    public DateOnly? EffectiveTo { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
