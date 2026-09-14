using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class PreventiveMaintenancePlanConfiguration
    : IEntityTypeConfiguration<PreventiveMaintenancePlan>
{
    public void Configure(
        EntityTypeBuilder<PreventiveMaintenancePlan> builder)
    {
        builder.ToTable("PreventiveMaintenancePlans");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PlanNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Frequency)
            .IsRequired();

        builder.Property(x => x.Priority)
            .IsRequired();

        builder.Property(x => x.NextDueDate)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.AutoGenerateWorkOrder)
            .IsRequired();

        builder.HasIndex(x => x.PlanNumber)
            .IsUnique();

        builder.HasIndex(x => x.AssetId);

        builder.HasIndex(x => x.NextDueDate);

        builder.HasIndex(x => new
        {
            x.IsActive,
            x.NextDueDate
        });

        builder.HasOne(x => x.Asset)
            .WithMany(x => x.PreventiveMaintenancePlans)
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}