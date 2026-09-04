using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssetNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Manufacturer)
            .HasMaxLength(100);

        builder.Property(x => x.Model)
            .HasMaxLength(100);

        builder.Property(x => x.SerialNumber)
            .HasMaxLength(100);

        builder.Property(x => x.PurchaseCost)
            .HasPrecision(18, 2);

        builder.Property(x => x.Criticality)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        // AssetNumber must be globally unique.
        // We don't filter this by IsActive because an old asset number
        // should not normally be reused after an asset is retired/disposed.
        builder.HasIndex(x => x.AssetNumber)
            .IsUnique();

        // Serial numbers are unique when provided.
        // SQL Server allows multiple NULL values with this filtered index.
        builder.HasIndex(x => x.SerialNumber)
            .IsUnique()
            .HasFilter("[SerialNumber] IS NOT NULL");

        // Asset -> AssetCategory
        builder.HasOne(x => x.AssetCategory)
            .WithMany(x => x.Assets)
            .HasForeignKey(x => x.AssetCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Asset -> Room
        builder.HasOne(x => x.Room)
            .WithMany(x => x.Assets)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}