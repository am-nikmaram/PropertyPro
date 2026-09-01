using MediatR;

namespace PropertyPro.Application.Features.Buildings.Commands.DeleteBuilding;

public record DeleteBuildingCommand(int Id)
    : IRequest;