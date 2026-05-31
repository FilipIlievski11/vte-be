namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("Requests");
        builder.HasOne(e => e.CustomerVehicleRelation).WithMany().HasForeignKey(e => e.CustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.NewCustomerVehicleRelation).WithMany().HasForeignKey(e => e.NewCustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Attachments).WithOne(a => a.Request).HasForeignKey(a => a.RequestId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.OwnershipProofs).WithOne(p => p.Request).HasForeignKey(p => p.RequestId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.PaymentProofs).WithOne(p => p.Request).HasForeignKey(p => p.RequestId).OnDelete(DeleteBehavior.Cascade);

        // Indexes for search and joins (127K records)
        builder.HasIndex(e => e.RequestTypeId);
        builder.HasIndex(e => e.CustomerVehicleRelationId);
        builder.HasIndex(e => e.DateEnded);
    }
}
