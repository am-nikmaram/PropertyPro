
using MediatR;
using PropertyPro.Application.Features.Floors.DTOs;

namespace PropertyPro.Application.Features.Floors.Queries.GetFloorLookup;

public record GetFloorLookupQuery(
    int? BuildingId
) : IRequest<IReadOnlyList<FloorLookupDto>>;