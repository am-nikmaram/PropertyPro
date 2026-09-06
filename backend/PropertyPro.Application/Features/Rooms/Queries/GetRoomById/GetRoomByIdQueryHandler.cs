using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryHandler
    : IRequestHandler<GetRoomByIdQuery, RoomDto?>
{
    private readonly IApplicationDbContext _context;

    public GetRoomByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoomDto?> Handle(
        GetRoomByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Rooms
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                x.IsActive)
            .Select(x => new RoomDto(
                x.Id,
                x.UnitId,
                x.RoomTypeId,
                x.RoomType.Name,
                x.RoomNumber,
                x.Name,
                x.Description,
                x.Area,
                x.Capacity,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }
}