using MediatR;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRoomById;

public record GetRoomByIdQuery(int Id)
    : IRequest<RoomDto?>;