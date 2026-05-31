using VTE.Domain.Common;

namespace VTE.Domain.Stations;

public class Station : ITenantOwned
{
    public short Id { get; set; }
    public byte CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool? Active { get; set; } = true;
}
