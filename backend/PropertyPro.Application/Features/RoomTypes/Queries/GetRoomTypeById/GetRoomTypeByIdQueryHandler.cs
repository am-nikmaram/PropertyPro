using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.RoomTypes.DTOs;

namespace PropertyPro.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public class GetRoomTypeByIdQueryHandler
    : IRequestHandler<GetRoomTypeByIdQuery, RoomTypeDto?>
{
    private readonly IApplicationDbContext _context;

    public GetRoomTypeByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoomTypeDto?> Handle(
        GetRoomTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.RoomTypes
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                x.IsActive)
            .Select(x => new RoomTypeDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }
}