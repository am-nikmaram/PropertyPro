using PropertyPro.Domain.Common;

namespace PropertyPro.Domain.Entities;

public class FileAttachment : BaseEntity
{
    public string OriginalFileName { get; set; } = null!;

    public string StorageKey { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long SizeInBytes { get; set; }

    public int? AssetId { get; set; }

    public int? MaintenanceRecordId { get; set; }

    public int? WorkOrderId { get; set; }

    public Asset? Asset { get; set; }

    public MaintenanceRecord? MaintenanceRecord { get; set; }

    public WorkOrder? WorkOrder { get; set; }
}