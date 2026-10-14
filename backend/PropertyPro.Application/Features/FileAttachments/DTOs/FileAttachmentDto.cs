namespace PropertyPro.Application.Features.FileAttachments.DTOs;

public class FileAttachmentDto
{
    public int Id { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long SizeInBytes { get; set; }

    public int? AssetId { get; set; }

    public int? MaintenanceRecordId { get; set; }

    public int? WorkOrderId { get; set; }

    public DateTime CreatedAt { get; set; }
}