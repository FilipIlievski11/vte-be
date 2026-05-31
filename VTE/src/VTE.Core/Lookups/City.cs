namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class City : LookupEntity
{
    public long CommunityId { get; set; }
    public Community Community { get; set; } = null!;
}
