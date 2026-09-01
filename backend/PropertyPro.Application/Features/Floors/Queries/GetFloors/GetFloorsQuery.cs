using MediatR;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Floors.DTOs;

namespace PropertyPro.Application.Features.Floors.Queries.GetFloors;

public record GetFloorsQuery(
    int BuildingId,
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null)
    : IRequest<PagedResult<FloorListDto>>;