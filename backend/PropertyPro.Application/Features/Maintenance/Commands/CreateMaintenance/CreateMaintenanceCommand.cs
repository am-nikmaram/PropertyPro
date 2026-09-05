using MediatR;
using PropertyPro.Application.Features.Maintenance.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Maintenance.Commands.CreateMaintenance;

public record CreateMaintenanceCommand(
    string MaintenanceNumber,
    int AssetId,
    MaintenanceType MaintenanceType,
    MaintenancePriority Priority,
    MaintenanceStatus Status,
    DateTime ScheduledDate,
    DateTime? StartedDate,
    DateTime? CompletedDate,
    decimal? Cost,
    string? Description,
    string? Notes
) : IRequest<MaintenanceRecordDto>;