using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Maintenance.DTOs;

public record MaintenanceRecordDto(
    int Id,
    string MaintenanceNumber,
    int AssetId,
    string AssetNumber,
    string AssetName,
    MaintenanceType MaintenanceType,
    MaintenancePriority Priority,
    MaintenanceStatus Status,
    DateTime ScheduledDate,
    DateTime? StartedDate,
    DateTime? CompletedDate,
    decimal? Cost,
    string? Description,
    string? Notes,
    bool IsActive);