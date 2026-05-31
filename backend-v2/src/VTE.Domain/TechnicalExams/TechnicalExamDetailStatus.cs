namespace VTE.Domain.TechnicalExams;

/// <summary>
/// Status of an inspected vehicle part on a report detail line:
/// 1 = исправен (OK), 2 = вратен (returned/pending), 3 = неисправен (defective).
/// A report passes (<see cref="TechnicalExamReport.VehicleIsRight"/>) only when
/// every detail line is status 1. Legacy: <c>DocumentsTehnicalExamsReportsDetailsStatus</c>.
/// </summary>
public class TechnicalExamDetailStatus
{
    public int Id { get; set; }

    /// <summary>Macedonian status name. Legacy: StatusName.</summary>
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}
