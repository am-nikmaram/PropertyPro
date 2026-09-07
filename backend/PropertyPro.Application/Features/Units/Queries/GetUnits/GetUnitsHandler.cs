using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Units.DTOs;

namespace PropertyPro.Application.Features.Units.Queries.GetUnits;

public class GetUnitsHandler
    : IRequestHandler<GetUnitsQuery, PagedResult<UnitListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUnitsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<UnitListDto>> Handle(
        GetUnitsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Units
            .AsNoTracking()
            .AsQueryable();

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

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var code = request.Code.Trim();

            query = query.Where(
                x => x.Code.Contains(code));
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim();

            query = query.Where(
                x => x.Name.Contains(name));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(
                x => x.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderBy(x => x.Code)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new UnitListDto(
                x.Id,
                x.FloorId,
                x.Floor.BuildingId,
                x.Code,
                x.Name,
                x.Area,
                x.IsActive))
            .ToListAsync(cancellationToken);

        return new PagedResult<UnitListDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}