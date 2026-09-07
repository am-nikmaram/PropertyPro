using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRooms;

public class GetRoomsHandler
    : IRequestHandler<GetRoomsQuery, PagedResult<RoomListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRoomsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<RoomListDto>> Handle(
        GetRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Rooms
            .AsNoTracking()
            .AsQueryable();

        if (request.UnitId.HasValue)
        {
            query = query.Where(
                x => x.UnitId == request.UnitId.Value);
        }

        if (request.FloorId.HasValue)
        {
            query = query.Where(
                x => x.Unit.FloorId == request.FloorId.Value);
        }

        if (request.BuildingId.HasValue)
        {
            query = query.Where(
                x => x.Unit.Floor.BuildingId ==
                     request.BuildingId.Value);
        }

        if (request.RoomTypeId.HasValue)
        {
            query = query.Where(
                x => x.RoomTypeId == request.RoomTypeId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.RoomNumber))
        {
            var roomNumber = request.RoomNumber.Trim();

            query = query.Where(
                x => x.RoomNumber.Contains(roomNumber));
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
            .OrderBy(x => x.RoomNumber)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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