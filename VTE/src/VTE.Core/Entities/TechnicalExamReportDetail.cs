namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class TechnicalExamReportDetail : BaseEntity
{
    public long ReportId { get; set; }
    public TechnicalExamReport Report { get; set; } = null!;
    public long VehiclePartId { get; set; }
    public TechnicalExamVehiclePart VehiclePart { get; set; } = null!;
    public long StatusId { get; set; }
    public ExamDetailStatus Status { get; set; } = null!;
    public string? Note { get; set; }
}
