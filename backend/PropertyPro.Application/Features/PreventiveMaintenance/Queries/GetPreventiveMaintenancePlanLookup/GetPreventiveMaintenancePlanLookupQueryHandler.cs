using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetPreventiveMaintenancePlanLookup;

public class GetPreventiveMaintenancePlanLookupQueryHandler
    : IRequestHandler<
        GetPreventiveMaintenancePlanLookupQuery,
        IReadOnlyList<PreventiveMaintenancePlanLookupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPreventiveMaintenancePlanLookupQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PreventiveMaintenancePlanLookupDto>>
        Handle(
            GetPreventiveMaintenancePlanLookupQuery request,
            CancellationToken cancellationToken)
    {
        return await _context.PreventiveMaintenancePlans
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new PreventiveMaintenancePlanLookupDto(
                x.Id,
                x.PlanNumber,
                x.Name,
                x.AssetId))
            .ToListAsync(cancellationToken);
    }
}