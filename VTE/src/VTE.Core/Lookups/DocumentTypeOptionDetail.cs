namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class DocumentTypeOptionDetail : LookupEntity
{
    public long DocumentTypeOptionId { get; set; }
    public DocumentTypeOption DocumentTypeOption { get; set; } = null!;
}
