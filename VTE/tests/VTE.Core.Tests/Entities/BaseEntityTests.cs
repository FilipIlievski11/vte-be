namespace VTE.Core.Tests.Entities;

using VTE.Core.Entities;

public class BaseEntityTests
{
    [Fact]
    public void NewBaseEntity_HasDefaultId()
    {
        var entity = new TestEntity();
        Assert.Equal(0, entity.Id);
    }

    [Fact]
    public void BaseEntity_CanSetId()
    {
        var entity = new TestEntity { Id = 42 };
        Assert.Equal(42, entity.Id);
    }

    private class TestEntity : BaseEntity { }
}
