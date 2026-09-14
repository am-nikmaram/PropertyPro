using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

public record PreventiveMaintenanceScheduleDto(
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
    int DaysUntilDue,
    bool IsOverdue);