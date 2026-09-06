using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRoomLookup;

public class GetRoomLookupHandler
    : IRequestHandler<
        GetRoomLookupQuery,
        IReadOnlyList<RoomLookupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRoomLookupHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RoomLookupDto>> Handle(
        GetRoomLookupQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Rooms
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (request.UnitId.HasValue)
        {
            query = query.Where(x =>
                x.UnitId == request.UnitId.Value);
        }

        return await query
            .OrderBy(x => x.RoomNumber)
            .Select(x => new RoomLookupDto(
                x.Id,
                x.UnitId,
                x.RoomTypeId,
                x.RoomNumber,
                x.Name))
            .ToListAsync(cancellationToken);
    }
}