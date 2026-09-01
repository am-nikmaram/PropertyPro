using MediatR;
using PropertyPro.Application.Features.Buildings.DTOs;

namespace PropertyPro.Application.Features.Buildings.Commands.CreateBuilding;

public record CreateBuildingCommand(
    int FacilityId,
    string Name,
    string Code,
    string? Description,
    int? YearBuilt,
    decimal? TotalArea)
    : IRequest<BuildingDto>;