using MediatR;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Commands.UpdatePreventiveMaintenancePlan;

public record UpdatePreventiveMaintenancePlanCommand(
    int Id,
    string Name,
    PreventiveMaintenanceFrequency Frequency,
    DateTime NextDueDate,
    MaintenancePriority Priority,
    string? Description,
    bool AutoGenerateWorkOrder
) : IRequest<PreventiveMaintenancePlanDto>;