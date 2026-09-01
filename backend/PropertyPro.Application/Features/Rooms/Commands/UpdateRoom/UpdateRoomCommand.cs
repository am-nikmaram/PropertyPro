using MediatR;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Commands.UpdateRoom;

public record UpdateRoomCommand(
    int Id,
    int FloorId,
    int RoomTypeId,
    string RoomNumber,
    string Name,
    string? Description,
    decimal? Area,
    int? Capacity)
    : IRequest<RoomDto>;