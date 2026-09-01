using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Buildings.DTOs;

namespace PropertyPro.Application.Features.Buildings.Queries.GetBuildings;

public class GetBuildingsQueryHandler
    : IRequestHandler<
        GetBuildingsQuery,
        PagedResult<BuildingListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetBuildingsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<BuildingListDto>> Handle(
        GetBuildingsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Buildings
            .AsNoTracking()
            .Where(x =>
                x.FacilityId == request.FacilityId &&
                x.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.Name.Contains(search) ||
                x.Code.Contains(search));
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((request.PageNumber - 1) *
                  request.PageSize)
            .Take(request.PageSize)
            .Select(x => new BuildingListDto(
                x.Id,
                x.FacilityId,
                x.Name,
                x.Code,
                x.YearBuilt,
                x.TotalArea,
                x.IsActive))
            .ToListAsync(cancellationToken);

        return new PagedResult<BuildingListDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}