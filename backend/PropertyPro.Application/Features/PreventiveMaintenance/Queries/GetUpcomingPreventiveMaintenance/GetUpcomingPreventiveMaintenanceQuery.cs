using MediatR;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetUpcomingPreventiveMaintenance;

public record GetUpcomingPreventiveMaintenanceQuery(
    int Days = 30,
    int Take = 10
) : IRequest<IReadOnlyList<PreventiveMaintenanceScheduleDto>>;