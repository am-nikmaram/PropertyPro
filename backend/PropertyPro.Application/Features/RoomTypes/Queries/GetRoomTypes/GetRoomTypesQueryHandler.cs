using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.RoomTypes.DTOs;

namespace PropertyPro.Application.Features.RoomTypes.Queries.GetRoomTypes;

public class GetRoomTypesQueryHandler
    : IRequestHandler<
        GetRoomTypesQuery,
        IReadOnlyList<RoomTypeListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRoomTypesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RoomTypeListDto>> Handle(
        GetRoomTypesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.RoomTypes
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new RoomTypeListDto(
                x.Id,
                x.Name,
                x.Description))
            .ToListAsync(cancellationToken);
    }
}