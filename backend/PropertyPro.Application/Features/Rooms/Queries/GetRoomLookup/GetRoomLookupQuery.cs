
using MediatR;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRoomLookup;

public record GetRoomLookupQuery(
    int? FloorId
) : IRequest<IReadOnlyList<RoomLookupDto>>;