using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class UnitConfiguration
    : IEntityTypeConfiguration<Unit>
{
    public void Configure(
        EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("Units");


        builder.HasKey(x => x.Id);


        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);


        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);


        builder.Property(x => x.Description)
            .HasMaxLength(1000);


        builder.Property(x => x.Area)
            .HasPrecision(18, 2);


        builder.HasIndex(x => x.Code)
            .IsUnique();


        builder.HasOne(x => x.Floor)
            .WithMany(x => x.Units)
            .HasForeignKey(x => x.FloorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}