using MediatR;

namespace PropertyPro.Application.Features.Floors.Commands.DeleteFloor;

public record DeleteFloorCommand(int Id)
    : IRequest;