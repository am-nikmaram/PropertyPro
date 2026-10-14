using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Configurations;

public class FileAttachmentConfiguration
    : IEntityTypeConfiguration<FileAttachment>
{
    public void Configure(EntityTypeBuilder<FileAttachment> builder)
    {
        builder.ToTable("FileAttachments", table =>
        {
            table.HasCheckConstraint(
                "CK_FileAttachments_ExactlyOneParent",
                """
                (
                    CASE WHEN AssetId IS NULL THEN 0 ELSE 1 END +
                    CASE WHEN MaintenanceRecordId IS NULL THEN 0 ELSE 1 END +
                    CASE WHEN WorkOrderId IS NULL THEN 0 ELSE 1 END
                ) = 1
                """);
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OriginalFileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.StorageKey)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasIndex(x => x.StorageKey)
            .IsUnique();

        builder.Property(x => x.ContentType)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.SizeInBytes)
            .IsRequired();

        builder.HasOne(x => x.Asset)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MaintenanceRecord)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.MaintenanceRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.WorkOrder)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}