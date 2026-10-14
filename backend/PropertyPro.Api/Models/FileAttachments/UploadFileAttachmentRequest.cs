using Microsoft.AspNetCore.Http;

namespace PropertyPro.Api.Models.FileAttachments;

public class UploadFileAttachmentRequest
{
    public IFormFile File { get; set; } = null!;

    public int? AssetId { get; set; }

    public int? MaintenanceRecordId { get; set; }

    public int? WorkOrderId { get; set; }
}