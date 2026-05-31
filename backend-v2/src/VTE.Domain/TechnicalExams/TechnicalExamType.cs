namespace VTE.Domain.TechnicalExams;

/// <summary>
/// Catalog of technical-exam types (РЕД-12М, РЕД-06М, РЕД-24М, ВОНРЕДЕН, …).
/// Drives how long the resulting certificate is valid for.
/// Legacy source: <c>TehnicalExamsTypes</c>.
/// </summary>
public class TechnicalExamType
{
    public int Id { get; set; }

    /// <summary>Short code, e.g. "РЕД-12М". Legacy: Code.</summary>
    public string? Code { get; set; }

    /// <summary>Human-readable description. Legacy: Description.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Validity period added to MadeDate to derive ValidTillDate. Legacy: ValidNumOfDays.</summary>
    public int ValidDays { get; set; }

    /// <summary>Share of a full exam this type represents (used in pricing). Legacy: PercentOfFullExam.</summary>
    public int PercentOfFullExam { get; set; }

    /// <summary>Whether this type is reported into the national register. Legacy: IsInRegistar.</summary>
    public bool IsInRegister { get; set; }

    public bool Active { get; set; } = true;
}
