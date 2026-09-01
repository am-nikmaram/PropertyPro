using MediatR;

namespace PropertyPro.Application.Features.Rooms.Commands.DeleteRoom;

public record DeleteRoomCommand(int Id)
    : IRequest;