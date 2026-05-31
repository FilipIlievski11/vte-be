namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.IdentificationNumber).HasMaxLength(50);
        builder.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.ParentName).HasMaxLength(100);
        builder.Property(c => c.CompanyName).HasMaxLength(200);
        builder.Property(c => c.TaxNumber).HasMaxLength(50);
        builder.Property(c => c.PhoneNumber).HasMaxLength(50);
        builder.Property(c => c.Fax).HasMaxLength(50);
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.PassportNumber).HasMaxLength(50);
        builder.Property(c => c.DrivingLicenseNumber).HasMaxLength(50);
        builder.Property(c => c.IdentityCardNumber).HasMaxLength(50);
        builder.HasMany(c => c.ContactPersons).WithOne(cp => cp.Customer).HasForeignKey(cp => cp.CustomerId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.BankAccounts).WithOne(ba => ba.Customer).HasForeignKey(ba => ba.CustomerId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(c => c.IdentificationNumber);
        builder.HasIndex(c => c.LastName);
        builder.HasIndex(c => c.FirstName);
        builder.HasIndex(c => c.LivingCityId);
    }
}

public class CustomerContactPersonConfiguration : IEntityTypeConfiguration<CustomerContactPerson>
{
    public void Configure(EntityTypeBuilder<CustomerContactPerson> builder)
    {
        builder.ToTable("CustomerContactPersons");
        builder.Property(c => c.FullName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.PhoneNumber).HasMaxLength(50);
        builder.Property(c => c.Email).HasMaxLength(200);
    }
}

public class CustomerBankAccountConfiguration : IEntityTypeConfiguration<CustomerBankAccount>
{
    public void Configure(EntityTypeBuilder<CustomerBankAccount> builder)
    {
        builder.ToTable("CustomerBankAccounts");
        builder.Property(c => c.BankName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.AccountNumber).HasMaxLength(50).IsRequired();
    }
}
