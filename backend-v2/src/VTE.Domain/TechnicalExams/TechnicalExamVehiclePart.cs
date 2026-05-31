namespace VTE.Domain.TechnicalExams;

/// <summary>
/// Catalog of inspectable vehicle parts (brakes, lights, steering, …) that can
/// be flagged on a report detail line. Legacy: <c>TehnicalExamVehicleParts</c>.
/// </summary>
public class TechnicalExamVehiclePart
{
    public int Id { get; set; }

    /// <summary>Grouping bucket within the catalog (no separate lookup table in legacy).
    /// Legacy: IdCategoryVehicleParts.</summary>
    public int CategoryId { get; set; }

    /// <summary>Short code. Legacy: Code.</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Human-readable part name. Legacy: Description.</summary>
    public string Description { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}
