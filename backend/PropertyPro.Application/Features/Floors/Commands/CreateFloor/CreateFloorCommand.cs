using MediatR;
using PropertyPro.Application.Features.Floors.DTOs;

namespace PropertyPro.Application.Features.Floors.Commands.CreateFloor;

public record CreateFloorCommand(
    int BuildingId,
    int FloorNumber,
    string Name,
    string? Description,
    decimal? TotalArea)
    : IRequest<FloorDto>;