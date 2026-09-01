using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Floors.DTOs;

namespace PropertyPro.Application.Features.Floors.Queries.GetFloors;

public class GetFloorsQueryHandler
    : IRequestHandler<
        GetFloorsQuery,
        PagedResult<FloorListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFloorsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<FloorListDto>> Handle(
        GetFloorsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Floors
            .AsNoTracking()
            .Where(x =>
                x.BuildingId == request.BuildingId &&
                x.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.Name.Contains(search));
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderBy(x => x.FloorNumber)
            .Select(x => new FloorListDto(
                x.Id,
                x.BuildingId,
                x.FloorNumber,
                x.Name,
                x.TotalArea,
                x.IsActive))
            .Skip((request.PageNumber - 1) *
                  request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<FloorListDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}