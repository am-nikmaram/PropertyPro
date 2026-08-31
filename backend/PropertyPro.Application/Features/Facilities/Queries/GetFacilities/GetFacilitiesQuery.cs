using MediatR;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilities;

public record GetFacilitiesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    string? Search = null,
    string? SortBy = "name",
    bool SortDescending = false)
    : IRequest<PagedResult<FacilityListDto>>;