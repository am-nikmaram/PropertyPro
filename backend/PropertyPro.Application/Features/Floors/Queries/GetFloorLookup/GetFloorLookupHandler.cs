using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Floors.DTOs;

namespace PropertyPro.Application.Features.Floors.Queries.GetFloorLookup;

public class GetFloorLookupHandler
    : IRequestHandler<
        GetFloorLookupQuery,
        IReadOnlyList<FloorLookupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFloorLookupHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<FloorLookupDto>> Handle(
        GetFloorLookupQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Floors
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (request.BuildingId.HasValue)
        {
            query = query.Where(x =>
                x.BuildingId == request.BuildingId.Value);
        }

        return await query
            .OrderBy(x => x.FloorNumber)
            .Select(x => new FloorLookupDto(
                x.Id,
                x.BuildingId,
                x.FloorNumber,
                x.Name))
            .ToListAsync(cancellationToken);
    }
}