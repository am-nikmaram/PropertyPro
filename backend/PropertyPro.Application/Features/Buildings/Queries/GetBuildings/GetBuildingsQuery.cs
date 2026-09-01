using MediatR;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Buildings.DTOs;

namespace PropertyPro.Application.Features.Buildings.Queries.GetBuildings;

public record GetBuildingsQuery(
    int FacilityId,
    int PageNumber = 1,
    int PageSize = 10,
    string? Search = null)
    : IRequest<PagedResult<BuildingListDto>>;