using MediatR;
using PropertyPro.Application.Features.Maintenance.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Maintenance.Commands.UpdateMaintenance;

public record UpdateMaintenanceCommand(
    int Id,
    MaintenanceType MaintenanceType,
    MaintenancePriority Priority,
    MaintenanceStatus Status,
    DateTime ScheduledDate,
    DateTime? StartedDate,
    DateTime? CompletedDate,
    decimal? Cost,
    string? Description,
    string? Notes
)
    : IRequest<MaintenanceRecordDto>;