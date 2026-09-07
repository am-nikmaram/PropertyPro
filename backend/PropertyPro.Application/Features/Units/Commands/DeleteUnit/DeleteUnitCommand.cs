using MediatR;

namespace PropertyPro.Application.Features.Units.Commands.DeleteUnit;

public record DeleteUnitCommand(int Id) : IRequest;