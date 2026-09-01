using MediatR;
using PropertyPro.Application.Features.RoomTypes.DTOs;

namespace PropertyPro.Application.Features.RoomTypes.Commands.CreateRoomType;

public record CreateRoomTypeCommand(
    string Name,
    string? Description)
    : IRequest<RoomTypeDto>;