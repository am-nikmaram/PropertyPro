using AutoMapper.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilityLookup;

public class GetFacilityLookupHandler
    : IRequestHandler<
        GetFacilityLookupQuery,
        IReadOnlyList<FacilityLookupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFacilityLookupHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<FacilityLookupDto>> Handle(
        GetFacilityLookupQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Facilities
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Code)
            .Select(x => new FacilityLookupDto(
                x.Id,
                x.Code,
                x.Name))
            .ToListAsync(cancellationToken);
    }
}