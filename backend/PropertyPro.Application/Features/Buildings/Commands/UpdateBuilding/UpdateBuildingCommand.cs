using MediatR;
using PropertyPro.Application.Features.Buildings.DTOs;

namespace PropertyPro.Application.Features.Buildings.Commands.UpdateBuilding;

public record UpdateBuildingCommand(
    int Id,
    int FacilityId,
    string Name,
    string Code,
    string? Description,
    int? YearBuilt,
    decimal? TotalArea)
    : IRequest<BuildingDto>;