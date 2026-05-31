namespace VTE.Core.Entities;

public class TechnicalExamOrganization : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public long CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
