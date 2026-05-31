namespace VTE.Domain.Documents;

public class TechnicalExamReportDetail
{
    public long Id { get; set; }
    public long TechnicalExamReportId { get; set; }
    public int TechnicalExamVehiclePartId { get; set; }
    public int StatusId { get; set; }
    public bool Front { get; set; }
    public bool Back { get; set; }
    public bool OnLeft { get; set; }
    public bool OnRight { get; set; }
    public DateOnly DateEnter { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public string? Note { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class TechnicalExamReportVisualError
{
    public long Id { get; set; }
    public long TechnicalExamReportId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Severity { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
