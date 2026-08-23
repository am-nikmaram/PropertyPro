using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class OrganizationConfiguration
    : IEntityTypeConfiguration<Organization>
{
    public void Configure(
        EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.HasIndex(x => x.Code)
            .IsUnique();
    }
}