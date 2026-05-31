namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class DocumentType : LookupEntity
{
    public bool RequiresTechnicalExam { get; set; }
    public bool RequiresOwnershipProof { get; set; }
    public bool RequiresPaymentProof { get; set; }
    public List<DocumentTypeOption> Options { get; set; } = [];
}
