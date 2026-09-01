using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class FloorConfiguration
    : IEntityTypeConfiguration<Floor>
{
    public void Configure(
        EntityTypeBuilder<Floor> builder)
    {
        builder.ToTable("Floors");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.TotalArea)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.HasOne(x => x.Building)
            .WithMany(x => x.Floors)
            .HasForeignKey(x => x.BuildingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.BuildingId,
            x.FloorNumber
        })
        .IsUnique();
    }
}