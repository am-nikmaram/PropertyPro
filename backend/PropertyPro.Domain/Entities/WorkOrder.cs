using PropertyPro.Domain.Common;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Domain.Entities;

public class WorkOrder : BaseEntity
{
    public string WorkOrderNumber { get; set; } = null!;

    public int MaintenanceRecordId { get; set; }

    public WorkOrderType WorkOrderType { get; set; }

    public WorkOrderPriority Priority { get; set; }

    public WorkOrderStatus Status { get; set; }

    public DateTime ScheduledDate { get; set; }

    public DateTime? StartedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public decimal? EstimatedCost { get; set; }

    public decimal? ActualCost { get; set; }

    public string? Description { get; set; }

    public string? Notes { get; set; }

    public int? TechnicianId { get; set; }

    public int? VendorId { get; set; }

    public Technician? Technician { get; set; }

    public Vendor? Vendor { get; set; }

    public MaintenanceRecord MaintenanceRecord { get; set; } = null!;

    public ICollection<FileAttachment> Attachments { get; set; }
    = new List<FileAttachment>();
}