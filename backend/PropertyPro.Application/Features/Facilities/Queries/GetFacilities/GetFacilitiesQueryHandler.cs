using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilities;

public class GetFacilitiesQueryHandler
    : IRequestHandler<
        GetFacilitiesQuery,
        IReadOnlyList<FacilityListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFacilitiesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;

    }

    public async Task<IReadOnlyList<FacilityListDto>> Handle(
        GetFacilitiesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Facilities
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new FacilityListDto(
                x.Id,
                x.Name,
                x.Code,
                x.City,
                x.Country,
                x.IsActive))
            .ToListAsync(cancellationToken);
    }
}