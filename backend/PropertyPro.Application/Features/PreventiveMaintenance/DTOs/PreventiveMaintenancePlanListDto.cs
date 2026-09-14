using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

public record PreventiveMaintenancePlanListDto(
    int Id,
    string PlanNumber,
    string Name,
    int AssetId,
    string AssetNumber,
    string AssetName,
    PreventiveMaintenanceFrequency Frequency,
    DateTime NextDueDate,
    MaintenancePriority Priority,
    bool AutoGenerateWorkOrder,
    bool IsActive);