using MediatR;

namespace PropertyPro.Application.Features.RoomTypes.Commands.DeleteRoomType;

public record DeleteRoomTypeCommand(int Id)
    : IRequest;