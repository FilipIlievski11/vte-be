namespace VTE.Core.Tests.Entities;

using VTE.Core.Entities;

public class CustomerTests
{
    [Fact]
    public void Customer_IsAuditableEntity()
    {
        var customer = new Customer();
        Assert.IsAssignableFrom<AuditableEntity>(customer);
    }

    [Fact]
    public void Customer_DefaultIsCompany_IsFalse()
    {
        var customer = new Customer();
        Assert.False(customer.IsCompany);
    }

    [Fact]
    public void Customer_ContactPersons_InitializedAsEmptyList()
    {
        var customer = new Customer();
        Assert.NotNull(customer.ContactPersons);
        Assert.Empty(customer.ContactPersons);
    }

    [Fact]
    public void Customer_BankAccounts_InitializedAsEmptyList()
    {
        var customer = new Customer();
        Assert.NotNull(customer.BankAccounts);
        Assert.Empty(customer.BankAccounts);
    }
}
