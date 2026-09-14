using PropertyPro.Domain.Common;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Domain.Entities;

public class PreventiveMaintenancePlan : BaseEntity
{
    public string PlanNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int AssetId { get; set; }

    public PreventiveMaintenanceFrequency Frequency { get; set; }

    public DateTime NextDueDate { get; set; }

    public MaintenancePriority Priority { get; set; }

    public string? Description { get; set; }

    public bool AutoGenerateWorkOrder { get; set; }

    public Asset Asset { get; set; } = null!;
}