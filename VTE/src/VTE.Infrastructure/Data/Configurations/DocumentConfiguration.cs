namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");
        builder.HasOne(e => e.CustomerVehicleRelation).WithMany().HasForeignKey(e => e.CustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Attachments).WithOne(a => a.Document).HasForeignKey(a => a.DocumentId).OnDelete(DeleteBehavior.Cascade);

        // Indexes for search and joins
        builder.HasIndex(e => e.DocumentTypeId);
        builder.HasIndex(e => e.CustomerVehicleRelationId);
    }
}

public class TrafficLicenseConfiguration : IEntityTypeConfiguration<TrafficLicense>
{
    public void Configure(EntityTypeBuilder<TrafficLicense> builder)
    {
        builder.ToTable("TrafficLicenses");
        builder.Property(e => e.LicenseNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.PlateNumber).HasMaxLength(20);
        builder.HasMany(e => e.Extensions).WithOne(x => x.TrafficLicense).HasForeignKey(x => x.TrafficLicenseId).OnDelete(DeleteBehavior.Cascade);
    }
}
