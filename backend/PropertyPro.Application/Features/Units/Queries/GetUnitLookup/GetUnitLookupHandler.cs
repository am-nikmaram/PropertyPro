using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Units.DTOs;

namespace PropertyPro.Application.Features.Units.Queries.GetUnitLookup;

public class GetUnitLookupHandler
    : IRequestHandler<
        GetUnitLookupQuery,
        IReadOnlyList<UnitLookupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUnitLookupHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<UnitLookupDto>> Handle(
        GetUnitLookupQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Units
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (request.FloorId.HasValue)
        {
            query = query.Where(
                x => x.FloorId == request.FloorId.Value);
        }

        if (request.BuildingId.HasValue)
        {
            query = query.Where(
                x => x.Floor.BuildingId == request.BuildingId.Value);
        }

        return await query
            .OrderBy(x => x.Code)
            .Select(x => new UnitLookupDto(
                x.Id,
                x.FloorId,
                x.Floor.BuildingId,
                x.Code,
                x.Name))
            .ToListAsync(cancellationToken);
    }
}