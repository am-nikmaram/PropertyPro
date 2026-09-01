using MediatR;
using PropertyPro.Application.Features.RoomTypes.DTOs;

namespace PropertyPro.Application.Features.RoomTypes.Commands.UpdateRoomType;

public record UpdateRoomTypeCommand(
    int Id,
    string Name,
    string? Description)
    : IRequest<RoomTypeDto>;