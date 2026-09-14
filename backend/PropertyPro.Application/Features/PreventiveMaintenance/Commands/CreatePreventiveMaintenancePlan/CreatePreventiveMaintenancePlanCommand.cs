using MediatR;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Commands.CreatePreventiveMaintenancePlan;

public record CreatePreventiveMaintenancePlanCommand(
    string Name,
    int AssetId,
    PreventiveMaintenanceFrequency Frequency,
    DateTime NextDueDate,
    MaintenancePriority Priority,
    string? Description,
    bool AutoGenerateWorkOrder
) : IRequest<PreventiveMaintenancePlanDto>;