using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class RoomConfiguration
    : IEntityTypeConfiguration<Room>
{
    public void Configure(
        EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoomNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.RoomType)
            .HasMaxLength(100);

        builder.HasIndex(x => new
        {
            x.FloorId,
            x.RoomNumber
        })
        .IsUnique();

        builder.HasOne(x => x.Floor)
            .WithMany(x => x.Rooms)
            .HasForeignKey(x => x.FloorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}