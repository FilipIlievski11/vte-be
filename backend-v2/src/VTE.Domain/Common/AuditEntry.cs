namespace VTE.Domain.Common;

/// <summary>
/// One immutable row in the financial audit log — who changed what, when, on which
/// record. Written by AuditLogger (VTE.Api/Services) alongside the mutation in the
/// SAME SaveChanges, so an audit row can't exist without its change (and vice versa).
/// Never updated, never deleted. New in v2 — legacy had nothing like it.
/// </summary>
public class AuditEntry : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    public DateTime AtUtc { get; set; }

    /// <summary>AspNetUsers.Id of the actor (null for system/scheduler actions).</summary>
    public string? UserId { get; set; }

    /// <summary>Denormalized login name at the time of the action — survives user renames.</summary>
    public string? UserName { get; set; }

    /// <summary>Short machine code, e.g. "bill.storno", "debt.price". Prefix = entity family.</summary>
    public string Action { get; set; } = "";

    /// <summary>Entity type name, e.g. "PaymentDocument", "CustomerDebt".</summary>
    public string EntityType { get; set; } = "";

    public long EntityId { get; set; }

    /// <summary>Human-readable one-liner (MK) shown in the admin log.</summary>
    public string Summary { get; set; } = "";

    /// <summary>Optional JSON payload with the raw old/new values.</summary>
    public string? DetailsJson { get; set; }
}
