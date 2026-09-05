using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Buildings.DTOs;

namespace PropertyPro.Application.Features.Buildings.Queries.GetBuildingLookup;

public class GetBuildingLookupHandler
    : IRequestHandler<
        GetBuildingLookupQuery,
        IReadOnlyList<BuildingLookupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetBuildingLookupHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<BuildingLookupDto>> Handle(
        GetBuildingLookupQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Buildings
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (request.FacilityId.HasValue)
        {
            query = query.Where(x =>
                x.FacilityId == request.FacilityId.Value);
        }

        return await query
            .OrderBy(x => x.Code)
            .Select(x => new BuildingLookupDto(
                x.Id,
                x.FacilityId,
                x.Code,
                x.Name))
            .ToListAsync(cancellationToken);
    }
}