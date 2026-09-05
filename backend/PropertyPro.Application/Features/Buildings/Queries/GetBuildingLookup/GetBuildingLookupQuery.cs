using MediatR;
using PropertyPro.Application.Features.Buildings.DTOs;

namespace PropertyPro.Application.Features.Buildings.Queries.GetBuildingLookup;

public record GetBuildingLookupQuery(
    int? FacilityId
) : IRequest<IReadOnlyList<BuildingLookupDto>>;