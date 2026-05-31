namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class PaymentDocumentConfiguration : IEntityTypeConfiguration<PaymentDocument>
{
    public void Configure(EntityTypeBuilder<PaymentDocument> builder)
    {
        builder.ToTable("PaymentDocuments");
        builder.Property(e => e.DocumentNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.DiscountPercent).HasPrecision(5, 2);
        builder.Property(e => e.InsurancePolicy).HasPrecision(18, 2);
        builder.HasOne(e => e.CustomerVehicleRelation).WithMany().HasForeignKey(e => e.CustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.LineItems).WithOne(i => i.PaymentDocument).HasForeignKey(i => i.PaymentDocumentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.Installments).WithOne(i => i.PaymentDocument).HasForeignKey(i => i.PaymentDocumentId).OnDelete(DeleteBehavior.Cascade);

        // Indexes for search and joins (225K records - highest volume)
        builder.HasIndex(e => e.DocumentNumber);
        builder.HasIndex(e => e.CustomerVehicleRelationId);
        builder.HasIndex(e => e.PaymentTypeId);
        builder.HasIndex(e => e.IsPaid);
        builder.HasIndex(e => e.PaymentDate);
    }
}

public class PaymentLineItemConfiguration : IEntityTypeConfiguration<PaymentLineItem>
{
    public void Configure(EntityTypeBuilder<PaymentLineItem> builder)
    {
        builder.ToTable("PaymentLineItems");
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.UnitPrice).HasPrecision(18, 2);
        builder.Property(e => e.VATPercent).HasPrecision(5, 2);
        builder.Ignore(e => e.VATAmount);
        builder.Ignore(e => e.TotalAmount);
        builder.HasIndex(e => e.PaymentDocumentId);
    }
}

public class PaymentInstallmentConfiguration : IEntityTypeConfiguration<PaymentInstallment>
{
    public void Configure(EntityTypeBuilder<PaymentInstallment> builder)
    {
        builder.ToTable("PaymentInstallments");
        builder.Property(e => e.Amount).HasPrecision(18, 2);
    }
}
