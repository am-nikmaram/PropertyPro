using MediatR;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetPreventiveMaintenancePlanLookup;

public record GetPreventiveMaintenancePlanLookupQuery
    : IRequest<IReadOnlyList<PreventiveMaintenancePlanLookupDto>>;