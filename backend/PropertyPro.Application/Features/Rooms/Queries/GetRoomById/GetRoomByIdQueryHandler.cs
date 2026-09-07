using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Rooms.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRoomById;

public class GetRoomByIdHandler
    : IRequestHandler<GetRoomByIdQuery, RoomDto>
{
    private readonly IApplicationDbContext _context;

    public GetRoomByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoomDto> Handle(
        GetRoomByIdQuery request,
        CancellationToken cancellationToken)
    {
        var room = await _context.Rooms
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                x.IsActive)
            .Select(x => new RoomDto(
                x.Id,
                x.UnitId,
                x.Unit.FloorId,
                x.Unit.Floor.BuildingId,
                x.RoomTypeId,
                x.RoomType.Name,
                x.RoomNumber,
                x.Name,
                x.Description,
                x.Area,
                x.Capacity,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (room is null)
        {
            throw new NotFoundException(
                nameof(Room),
                request.Id);
        }

        return room;
    }
}