using MediatR;
using PropertyPro.Application.Features.RoomTypes.DTOs;

namespace PropertyPro.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public record GetRoomTypeByIdQuery(int Id)
    : IRequest<RoomTypeDto?>;