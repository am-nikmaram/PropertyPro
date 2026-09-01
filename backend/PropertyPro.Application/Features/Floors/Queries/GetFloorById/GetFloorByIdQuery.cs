using MediatR;
using PropertyPro.Application.Features.Floors.DTOs;

namespace PropertyPro.Application.Features.Floors.Queries.GetFloorById;

public record GetFloorByIdQuery(int Id)
    : IRequest<FloorDto?>;