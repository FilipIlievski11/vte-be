namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class CustomerVehicleRelationConfiguration : IEntityTypeConfiguration<CustomerVehicleRelation>
{
    public void Configure(EntityTypeBuilder<CustomerVehicleRelation> builder)
    {
        builder.ToTable("CustomerVehicleRelations");

        // Indexes for search and joins (65K records)
        builder.HasIndex(e => new { e.VehicleId, e.EndDate }); // composite: owner lookup
        builder.HasIndex(e => e.CustomerId);
        builder.HasIndex(e => e.VehicleId);
        builder.HasIndex(e => e.RelationTypeId);
    }
}
