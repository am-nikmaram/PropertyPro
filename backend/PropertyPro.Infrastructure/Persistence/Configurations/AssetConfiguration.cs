using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class AssetConfiguration
    : IEntityTypeConfiguration<Asset>
{
    public void Configure(
        EntityTypeBuilder<Asset> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssetNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Manufacturer)
            .HasMaxLength(150);

        builder.Property(x => x.Model)
            .HasMaxLength(150);

        builder.Property(x => x.SerialNumber)
            .HasMaxLength(150);

        builder.Property(x => x.PurchaseCost)
            .HasPrecision(18, 2);

        builder.Property(x => x.Criticality)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .HasMaxLength(50);

        builder.HasIndex(x => x.AssetNumber)
            .IsUnique();

        builder.HasIndex(x => x.SerialNumber)
            .IsUnique()
            .HasFilter("[SerialNumber] IS NOT NULL");

        builder.HasOne(x => x.AssetCategory)
            .WithMany(x => x.Assets)
            .HasForeignKey(x => x.AssetCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Room)
            .WithMany(x => x.Assets)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}