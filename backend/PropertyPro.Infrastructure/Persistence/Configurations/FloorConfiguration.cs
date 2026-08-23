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
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.BuildingId,
            x.FloorNumber
        })
        .IsUnique();

        builder.HasOne(x => x.Building)
            .WithMany(x => x.Floors)
            .HasForeignKey(x => x.BuildingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}