namespace VTE.Domain.Requests;

// Attached "ownership proof" rows for a request — the user can attach multiple
// (e.g. traffic licence, gift contract, customs declaration). Each row carries the
// proof-type FK plus free-text Number / DateIssued / Note fields. Already populated
// from the legacy `Request.VehicleOwnershipProofs` table during the bulk migration.
public class RequestVehicleOwnershipProof
{
    public long Id { get; set; }
    public long RequestId { get; set; }
    public int? OwnershipProofId { get; set; }
    public string? Number { get; set; }
    public DateOnly? DateIssued { get; set; }
    public string? Note { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

// Attached "payment proof" rows for a request (invoice, receipt, fiscal slip, etc.).
// New schema adds Amount on top of legacy fields.
public class RequestPaymentProof
{
    public long Id { get; set; }
    public long RequestId { get; set; }
    public int? PaymentProofId { get; set; }
    public string? Number { get; set; }
    public decimal? Amount { get; set; }
    public DateOnly? DateIssued { get; set; }
    public string? Note { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
