using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class FacilityConfiguration
    : IEntityTypeConfiguration<Facility>
{
    public void Configure(
        EntityTypeBuilder<Facility> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Address)
            .HasMaxLength(500);

        builder.Property(x => x.City)
            .HasMaxLength(100);

        builder.Property(x => x.Country)
            .HasMaxLength(100);

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20);

        builder.HasIndex(x => new
        {
            x.OrganizationId,
            x.Code
        })
        .IsUnique();

        builder.HasOne(x => x.Organization)
            .WithMany(x => x.Facilities)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}