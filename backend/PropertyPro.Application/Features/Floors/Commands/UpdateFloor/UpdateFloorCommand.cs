using MediatR;
using PropertyPro.Application.Features.Floors.DTOs;

namespace PropertyPro.Application.Features.Floors.Commands.UpdateFloor;

public record UpdateFloorCommand(
    int Id,
    int BuildingId,
    int FloorNumber,
    string Name,
    string? Description,
    decimal? TotalArea)
    : IRequest<FloorDto>;