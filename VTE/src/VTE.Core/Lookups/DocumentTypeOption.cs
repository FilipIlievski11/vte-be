namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class DocumentTypeOption : LookupEntity
{
    public long DocumentTypeId { get; set; }
    public DocumentType DocumentType { get; set; } = null!;
    public List<DocumentTypeOptionDetail> Details { get; set; } = [];
}
