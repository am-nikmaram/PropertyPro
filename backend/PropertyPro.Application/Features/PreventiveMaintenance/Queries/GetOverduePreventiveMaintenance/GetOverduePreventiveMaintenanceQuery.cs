using MediatR;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetOverduePreventiveMaintenance;

public record GetOverduePreventiveMaintenanceQuery(
    int Take = 10
) : IRequest<IReadOnlyList<PreventiveMaintenanceScheduleDto>>;