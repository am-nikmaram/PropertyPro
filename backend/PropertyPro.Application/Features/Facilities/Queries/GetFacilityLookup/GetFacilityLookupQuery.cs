using MediatR;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilityLookup;

public record GetFacilityLookupQuery
    : IRequest<IReadOnlyList<FacilityLookupDto>>;