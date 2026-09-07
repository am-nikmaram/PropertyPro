using MediatR;
using PropertyPro.Application.Features.Units.DTOs;

namespace PropertyPro.Application.Features.Units.Commands.CreateUnit;

public record CreateUnitCommand(
    int FloorId,
    string Code,
    string Name,
    string? Description,
    decimal? Area
) : IRequest<UnitDto>;