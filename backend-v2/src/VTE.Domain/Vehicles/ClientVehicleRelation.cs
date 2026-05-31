namespace VTE.Domain.Vehicles;

/// <summary>Links a Client to a Vehicle (or to nothing, for "client-only" relation type).
/// Legacy table: CustomerVehiclesRelations.</summary>
public class ClientVehicleRelation
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public long? VehicleId { get; set; }         // nullable: RelationType=3 has no vehicle
    public byte RelationTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? StartNote { get; set; }
    public string? EndNote { get; set; }
    public bool Active { get; set; } = true;
}
