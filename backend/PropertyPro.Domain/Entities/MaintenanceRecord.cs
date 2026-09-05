using PropertyPro.Domain.Common;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Domain.Entities;

public class MaintenanceRecord : BaseEntity
{

    public string MaintenanceNumber { get; set; } = null!;
    public int AssetId { get; set; }

    public MaintenanceType MaintenanceType { get; set; }

    public MaintenancePriority Priority { get; set; }

    public MaintenanceStatus Status { get; set; }

    public DateTime ScheduledDate { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public decimal? Cost { get; set; }

    public string? Description { get; set; }

    public string? Notes { get; set; }

    public Asset Asset { get; set; } = null!;
}