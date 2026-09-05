using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class MaintenanceRecordConfiguration
    : IEntityTypeConfiguration<MaintenanceRecord>
{
    public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
    {
        builder.ToTable("MaintenanceRecords");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MaintenanceNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.MaintenanceType)
            .IsRequired();

        builder.Property(x => x.Priority)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.ScheduledDate)
            .IsRequired();

        builder.Property(x => x.Cost)
            .HasPrecision(18, 2);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.HasIndex(x => x.MaintenanceNumber)
            .IsUnique();

        builder.HasOne(x => x.Asset)
            .WithMany(x => x.MaintenanceRecords)
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.AssetId);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.ScheduledDate);
    }
}