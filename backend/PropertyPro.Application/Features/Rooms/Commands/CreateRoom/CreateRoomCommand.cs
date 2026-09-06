using MediatR;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(
    int UnitId,
    int RoomTypeId,
    string RoomNumber,
    string Name,
    string? Description,
    decimal? Area,
    int? Capacity)
    : IRequest<RoomDto>;