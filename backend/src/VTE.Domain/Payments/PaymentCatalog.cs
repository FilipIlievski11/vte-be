namespace VTE.Domain.Payments;

// Catalog driving the legacy auto-calculation engine:
//   PaymentCategory  (1)─(N)→ PaymentItem (1)─(N)→ PaymentItemParametar
//
// On a Request → walk all PaymentCategory rows with the matching TrigerdByX
// flag, then for each pick the PaymentItem with VehicleCategoryForPaymentsId
// matching the vehicle. If the item has Parametars, pick the row whose
// VehicleField (NumberOfSeats, EmptyWeight, ...) value falls in [From, To].
// Result is the set of PaymentDocumentDetail rows to propose.
public class PaymentCategory
{
    public int Id { get; set; }
    public int? DDVId { get; set; }                          // -> DDVCatalog.Id
    public int? CalculationItemId { get; set; }              // -> CalculationItem.Id (bank account / form)
    public string CategoryName { get; set; } = string.Empty; // legacy CategoryName nvarchar(250)
    public bool AllowDiscount { get; set; }
    public bool TriggerdByRequest { get; set; }              // BR-PAY-040 (TrigerdByRequest)
    public bool TriggerdByTechnicalExam { get; set; }
    public bool TriggerdByTrafficLicence { get; set; }
    public bool TriggerdByPermissionForVehicle { get; set; }
    public bool TriggerdByInternationalDriverLicence { get; set; }
    public bool TriggerdByIrregularTechnicalExam { get; set; }
    public int? VisibleOrder { get; set; }
    public int? CommunityId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class PaymentItem
{
    public int Id { get; set; }
    public int PaymentCategoryId { get; set; }
    public int? VehicleCategoryForPaymentsId { get; set; }   // null = any-category fallback
    public string ItemName { get; set; } = string.Empty;     // nvarchar(250)
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public List<PaymentItemParametar> Parametars { get; set; } = new();
}

// Range-based price selection. When ItemParametars has rows, the auto-calc engine
// walks each row and picks the one whose VehicleField (Vehicle property name)
// value is inside [ParametarFrom, ParametarTo]. IsOptional rows let the user
// pick manually instead of auto-applying.
public class PaymentItemParametar
{
    public int Id { get; set; }
    public int PaymentItemId { get; set; }
    public string ParametarName { get; set; } = string.Empty; // nvarchar(250)
    public string? VehicleField { get; set; }                 // e.g. "NumberOfSeats" or "EmptyWeight" or null = fixed
    public decimal? ParametarFrom { get; set; }
    public decimal? ParametarTo { get; set; }
    public decimal Price { get; set; }                        // money in legacy
    public bool IsOptional { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

// Per-station, per-PaymentType running sequence. DocumentNumber is composed as
//   {PaymentType.Prefix}-{StationId}-{Number}/{Year}
// On a save we INCREMENT the row keyed by (StationId, PaymentTypeId) and use the
// resulting Number. IsTehExamReport differentiates the technical-exam sequence
// from the regular one when both use the same PaymentType (legacy quirk).
public class PaymentDocumentNumber
{
    public int Id { get; set; }
    public int StationId { get; set; }
    public int PaymentTypeId { get; set; }
    public int Number { get; set; }
    public bool IsTechExamReport { get; set; }
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
