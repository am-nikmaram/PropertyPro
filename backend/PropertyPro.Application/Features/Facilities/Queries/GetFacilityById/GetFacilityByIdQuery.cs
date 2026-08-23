using MediatR;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilityById;

public record GetFacilityByIdQuery(int Id)
    : IRequest<FacilityDto?>;