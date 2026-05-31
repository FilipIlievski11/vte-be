namespace VTE.Core.Entities;

public class TechnicalExamVisualError : BaseEntity
{
    public long ReportId { get; set; }
    public TechnicalExamReport Report { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string? Note { get; set; }
}
