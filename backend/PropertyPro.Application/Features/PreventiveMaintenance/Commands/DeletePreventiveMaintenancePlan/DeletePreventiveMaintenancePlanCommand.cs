using MediatR;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Commands.DeletePreventiveMaintenancePlan;

public record DeletePreventiveMaintenancePlanCommand(
    int Id
) : IRequest;