namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class Community : LookupEntity
{
    public long CountryId { get; set; }
    public Country Country { get; set; } = null!;
}
