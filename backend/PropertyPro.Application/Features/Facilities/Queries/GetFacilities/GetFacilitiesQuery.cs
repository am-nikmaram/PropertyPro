using MediatR;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilities;

public record GetFacilitiesQuery
    : IRequest<IReadOnlyList<FacilityListDto>>;