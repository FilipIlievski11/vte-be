namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Lookups;

public class LookupConfigurations :
    IEntityTypeConfiguration<Country>,
    IEntityTypeConfiguration<Community>,
    IEntityTypeConfiguration<City>,
    IEntityTypeConfiguration<Street>,
    IEntityTypeConfiguration<VehicleModel>,
    IEntityTypeConfiguration<DocumentType>,
    IEntityTypeConfiguration<TechnicalExamVehiclePart>,
    IEntityTypeConfiguration<PaymentCatalogItem>,
    IEntityTypeConfiguration<VATRate>
{
    public void Configure(EntityTypeBuilder<Country> b)
    {
        b.ToTable("Countries");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.ShortName).HasMaxLength(10);
    }

    public void Configure(EntityTypeBuilder<Community> b)
    {
        b.ToTable("Communities");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<City> b)
    {
        b.ToTable("Cities");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<Street> b)
    {
        b.ToTable("Streets");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<VehicleModel> b)
    {
        b.ToTable("VehicleModels");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<DocumentType> b)
    {
        b.ToTable("DocumentTypes");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.HasMany(e => e.Options).WithOne(o => o.DocumentType).HasForeignKey(o => o.DocumentTypeId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<TechnicalExamVehiclePart> b)
    {
        b.ToTable("TechnicalExamVehicleParts");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.HasOne(e => e.Parent).WithMany(e => e.Children).HasForeignKey(e => e.ParentId).OnDelete(DeleteBehavior.Restrict);
    }

    public void Configure(EntityTypeBuilder<PaymentCatalogItem> b)
    {
        b.ToTable("PaymentCatalogItems");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<VATRate> b)
    {
        b.ToTable("VATRates");
        b.Property(e => e.Rate).HasPrecision(5, 2);
    }
}
