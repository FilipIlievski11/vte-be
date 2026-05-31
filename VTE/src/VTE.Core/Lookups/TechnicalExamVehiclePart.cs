namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class TechnicalExamVehiclePart : LookupEntity
{
    public long? ParentId { get; set; }
    public TechnicalExamVehiclePart? Parent { get; set; }
    public List<TechnicalExamVehiclePart> Children { get; set; } = [];
}
