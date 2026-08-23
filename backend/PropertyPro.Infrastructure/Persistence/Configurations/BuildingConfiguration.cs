using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class BuildingConfiguration
    : IEntityTypeConfiguration<Building>
{
    public void Configure(
        EntityTypeBuilder<Building> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.TotalArea)
            .HasPrecision(18, 2);

        builder.HasIndex(x => new
        {
            x.FacilityId,
            x.Code
        })
        .IsUnique();

        builder.HasOne(x => x.Facility)
            .WithMany(x => x.Buildings)
            .HasForeignKey(x => x.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}