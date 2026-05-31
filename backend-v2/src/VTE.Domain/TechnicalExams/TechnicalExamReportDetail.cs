namespace VTE.Domain.TechnicalExams;

/// <summary>
/// One inspected-part line on a technical-exam report: the part, its status
/// (OK / returned / defective) and where on the vehicle the issue was found.
/// Legacy: <c>DocumentsTehnicalExamsReportsDetails</c>.
/// </summary>
public class TechnicalExamReportDetail
{
    public long Id { get; set; }

    /// <summary>Parent report. Legacy: IdTehnicalExamsReports.</summary>
    public long TechnicalExamReportId { get; set; }

    /// <summary>Inspected part. Legacy: IdTehnicalExamVehivlePart.</summary>
    public int VehiclePartId { get; set; }

    /// <summary>Part status. Legacy: IdStatus.</summary>
    public int StatusId { get; set; }

    // Where the issue was located on the vehicle.
    public bool Front { get; set; }
    public bool Back { get; set; }
    public bool OnLeft { get; set; }
    public bool OnRight { get; set; }

    /// <summary>When the line was recorded. Legacy: DateEnter.</summary>
    public DateTime EnteredAt { get; set; } = DateTime.UtcNow;

    public string? Note { get; set; }

    public bool Active { get; set; } = true;
}
