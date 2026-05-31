namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class TechnicalExamReportConfiguration : IEntityTypeConfiguration<TechnicalExamReport>
{
    public void Configure(EntityTypeBuilder<TechnicalExamReport> builder)
    {
        builder.ToTable("TechnicalExamReports");
        builder.Property(e => e.RegistrationNumber).HasMaxLength(50);
        builder.HasOne(e => e.FirstController).WithMany().HasForeignKey(e => e.FirstControllerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.SecondController).WithMany().HasForeignKey(e => e.SecondControllerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Details).WithOne(d => d.Report).HasForeignKey(d => d.ReportId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.VisualErrors).WithOne(v => v.Report).HasForeignKey(v => v.ReportId).OnDelete(DeleteBehavior.Cascade);

        // Indexes for search and joins (106K records)
        builder.HasIndex(e => e.RegistrationNumber);
        builder.HasIndex(e => e.ExamTypeId);
        builder.HasIndex(e => e.OrganizationId);
        builder.HasIndex(e => e.FirstControllerId);
        builder.HasIndex(e => e.CustomerVehicleRelationId);
        builder.HasIndex(e => e.ExamDate);
    }
}
