using MediatR;
using PropertyPro.Application.Features.Buildings.DTOs;

namespace PropertyPro.Application.Features.Buildings.Queries.GetBuildingById;

public record GetBuildingByIdQuery(int Id)
    : IRequest<BuildingDto?>;