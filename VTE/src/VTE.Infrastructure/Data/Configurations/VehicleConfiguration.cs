namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.ShellNumber).HasMaxLength(100).IsRequired();
        builder.Property(v => v.EngineNumber).HasMaxLength(100);
        builder.Property(v => v.FirstRegistrationNumber).HasMaxLength(50);
        builder.Property(v => v.LastRegistrationNumber).HasMaxLength(50);
        builder.Property(v => v.ColorCode).HasMaxLength(20);
        builder.Property(v => v.EnginePowerKW).HasPrecision(10, 2);
        builder.Property(v => v.EngineTorqueNM).HasPrecision(10, 2);
        builder.Property(v => v.EngineWorkingCapacityCM3).HasPrecision(10, 2);
        builder.Property(v => v.EmptyWeightKG).HasPrecision(10, 2);
        builder.Property(v => v.MaxAllowedWeightKG).HasPrecision(10, 2);
        builder.Property(v => v.MaxSpeedKMH).HasPrecision(10, 2);
        builder.Property(v => v.FuelConsumption).HasPrecision(10, 2);
        builder.Property(v => v.FuelTankCapacityL).HasPrecision(10, 2);
        builder.HasOne(v => v.FirstRegistrationIssuer).WithMany().HasForeignKey(v => v.FirstRegistrationIssuerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.LastRegistrationIssuer).WithMany().HasForeignKey(v => v.LastRegistrationIssuerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.PrimaryPowerSource).WithMany().HasForeignKey(v => v.PrimaryPowerSourceId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.SecondaryPowerSource).WithMany().HasForeignKey(v => v.SecondaryPowerSourceId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.PrimaryColor).WithMany().HasForeignKey(v => v.PrimaryColorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.SecondaryColor).WithMany().HasForeignKey(v => v.SecondaryColorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(v => v.Axles).WithOne(a => a.Vehicle).HasForeignKey(a => a.VehicleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(v => v.Tyres).WithOne(t => t.Vehicle).HasForeignKey(t => t.VehicleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(v => v.AxleDistances).WithOne(d => d.Vehicle).HasForeignKey(d => d.VehicleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(v => v.ShellNumber);
        builder.HasIndex(v => v.LastRegistrationNumber);
        builder.HasIndex(v => v.VehicleModelId);
        builder.HasIndex(v => v.CategoryId);
    }
}
