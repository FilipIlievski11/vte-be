namespace VTE.Infrastructure.Tests.Data;

using Microsoft.EntityFrameworkCore;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;

public class VteDbContextTests
{
    private VteDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<VteDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new VteDbContext(options);
    }

    [Fact]
    public async Task CanAddAndRetrieveCustomer()
    {
        using var ctx = CreateContext();
        var customer = new Customer
        {
            IdentificationNumber = "12345",
            FirstName = "Test",
            LastName = "User",
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = 1
        };
        ctx.Customers.Add(customer);
        await ctx.SaveChangesAsync();

        var loaded = await ctx.Customers.FirstAsync();
        Assert.Equal("Test", loaded.FirstName);
        Assert.Equal("12345", loaded.IdentificationNumber);
    }

    [Fact]
    public async Task CanAddAndRetrieveVehicle()
    {
        using var ctx = CreateContext();
        var vehicle = new Vehicle
        {
            ShellNumber = "ABC123",
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = 1
        };
        ctx.Vehicles.Add(vehicle);
        await ctx.SaveChangesAsync();

        var loaded = await ctx.Vehicles.FirstAsync();
        Assert.Equal("ABC123", loaded.ShellNumber);
    }

    [Fact]
    public async Task CanAddCustomerWithContactPersons()
    {
        using var ctx = CreateContext();
        var customer = new Customer
        {
            IdentificationNumber = "99999",
            FirstName = "Jane",
            LastName = "Doe",
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = 1,
            ContactPersons =
            [
                new CustomerContactPerson { FullName = "Contact One" },
                new CustomerContactPerson { FullName = "Contact Two" }
            ]
        };
        ctx.Customers.Add(customer);
        await ctx.SaveChangesAsync();

        var loaded = await ctx.Customers.Include(c => c.ContactPersons).FirstAsync();
        Assert.Equal(2, loaded.ContactPersons.Count);
    }
}
