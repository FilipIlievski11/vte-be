namespace VTE.Domain.Requests;

/// <summary>
/// Tri-state replacement for the legacy <c>RequestTypes.IsTehnicalExamRequired</c>
/// int column (legacy values: 0=no, 1=yes, 2=optional).
/// </summary>
public enum TechnicalExamRequirement : byte
{
    NotRequired = 0,
    Required = 1,
    Optional = 2,
}
