using MediatR;
using PropertyPro.Application.Features.Units.DTOs;

namespace PropertyPro.Application.Features.Units.Commands.UpdateUnit;

public record UpdateUnitCommand(
    int Id,
    string Code,
    string Name,
    string? Description,
    decimal? Area
) : IRequest<UnitDto>;