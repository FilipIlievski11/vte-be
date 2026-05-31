namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class CustomerVehicleRelation : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public long RelationTypeId { get; set; }
    public RelationType RelationType { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? BeginNote { get; set; }
    public string? TerminationNote { get; set; }
}
