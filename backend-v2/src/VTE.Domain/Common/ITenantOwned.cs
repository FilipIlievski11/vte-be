namespace VTE.Domain.Common;

/// <summary>Marker interface for entities that are scoped to a Company (tenant).</summary>
public interface ITenantOwned
{
    byte CompanyId { get; }
}
