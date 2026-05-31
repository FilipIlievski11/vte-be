namespace VTE.Core.Tests.Entities;

using VTE.Core.Entities;

public class VehicleTests
{
    [Fact]
    public void Vehicle_IsAuditableEntity()
    {
        var vehicle = new Vehicle();
        Assert.IsAssignableFrom<AuditableEntity>(vehicle);
    }

    [Fact]
    public void Vehicle_Axles_InitializedAsEmptyList()
    {
        var vehicle = new Vehicle();
        Assert.NotNull(vehicle.Axles);
        Assert.Empty(vehicle.Axles);
    }
}
