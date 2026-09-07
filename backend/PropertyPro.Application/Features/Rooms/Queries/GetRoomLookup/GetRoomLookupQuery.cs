using MediatR;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRoomLookup;

public record GetRoomLookupQuery(
    int? UnitId = null,
    int? FloorId = null,
    int? BuildingId = null
) : IRequest<IReadOnlyList<RoomLookupDto>>;