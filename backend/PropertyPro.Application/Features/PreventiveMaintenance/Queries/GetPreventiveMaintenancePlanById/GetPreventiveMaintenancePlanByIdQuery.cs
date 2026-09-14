using MediatR;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetPreventiveMaintenancePlanById;

public record GetPreventiveMaintenancePlanByIdQuery(
    int Id
) : IRequest<PreventiveMaintenancePlanDto>;