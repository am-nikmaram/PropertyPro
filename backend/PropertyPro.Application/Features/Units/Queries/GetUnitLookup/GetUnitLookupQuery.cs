using MediatR;
using PropertyPro.Application.Features.Units.DTOs;

namespace PropertyPro.Application.Features.Units.Queries.GetUnitLookup;

public record GetUnitLookupQuery(
    int? FloorId = null,
    int? BuildingId = null
) : IRequest<IReadOnlyList<UnitLookupDto>>;