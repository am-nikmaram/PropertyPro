using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Maintenance.DTOs;

public record MaintenanceListDto(
    int Id,
    string MaintenanceNumber,

    int AssetId,
    string AssetNumber,
    string AssetName,

    MaintenanceType MaintenanceType,
    MaintenancePriority Priority,
    MaintenanceStatus Status,

    DateTime ScheduledDate,
    decimal? Cost,

    bool IsActive);