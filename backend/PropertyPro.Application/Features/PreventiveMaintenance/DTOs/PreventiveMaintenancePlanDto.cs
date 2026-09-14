using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

public record PreventiveMaintenancePlanDto(
    int Id,
    string PlanNumber,
    string Name,
    int AssetId,
    string AssetNumber,
    string AssetName,
    PreventiveMaintenanceFrequency Frequency,
    DateTime NextDueDate,
    MaintenancePriority Priority,
    string? Description,
    bool AutoGenerateWorkOrder,
    bool IsActive);