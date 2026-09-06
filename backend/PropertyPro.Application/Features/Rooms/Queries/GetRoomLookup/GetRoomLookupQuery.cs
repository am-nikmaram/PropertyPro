
using MediatR;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRoomLookup;

public record GetRoomLookupQuery(
    int? UnitId
) : IRequest<IReadOnlyList<RoomLookupDto>>;