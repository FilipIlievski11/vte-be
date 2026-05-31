namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class Street : LookupEntity
{
    public long CityId { get; set; }
    public City City { get; set; } = null!;
}
