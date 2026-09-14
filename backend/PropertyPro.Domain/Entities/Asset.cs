using PropertyPro.Domain.Common;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Domain.Entities;

public class Asset : BaseEntity
{
    public string AssetNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int AssetCategoryId { get; set; }

    public int RoomId { get; set; }

    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public string? SerialNumber { get; set; }

    public DateTime? InstallationDate { get; set; }

    public DateTime? WarrantyStartDate { get; set; }

    public DateTime? WarrantyEndDate { get; set; }

    public decimal? PurchaseCost { get; set; }

    public AssetCriticality Criticality { get; set; }

    public AssetStatus Status { get; set; }

    public string? Description { get; set; }

    public AssetCategory AssetCategory { get; set; } = null!;

    public Room Room { get; set; } = null!;

    public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; }
    = new List<MaintenanceRecord>();

    public ICollection<PreventiveMaintenancePlan> PreventiveMaintenancePlans
    { get; set; } = new List<PreventiveMaintenancePlan>();
}