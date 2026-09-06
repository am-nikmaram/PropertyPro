using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRooms;

public class GetRoomsQueryHandler
    : IRequestHandler<
        GetRoomsQuery,
        PagedResult<RoomListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRoomsQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<RoomListDto>> Handle(
        GetRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Rooms
            .AsNoTracking()
            .Where(x =>
                x.UnitId == request.UnitId &&
                x.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.RoomNumber.Contains(search) ||
                x.Name.Contains(search));
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderBy(x => x.RoomNumber)
            .Select(x => new RoomListDto(
                x.Id,
                x.UnitId,
                x.RoomTypeId,
                x.RoomType.Name,
                x.RoomNumber,
                x.Name,
                x.Area,
                x.Capacity,
                x.IsActive))
            .Skip((request.PageNumber - 1) *
                  request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<RoomListDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}