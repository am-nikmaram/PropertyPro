using MediatR;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Units.DTOs;

namespace PropertyPro.Application.Features.Units.Queries.GetUnits;

public record GetUnitsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    int? FloorId = null,
    int? BuildingId = null,
    string? Code = null,
    string? Name = null,
    bool? IsActive = true
) : IRequest<PagedResult<UnitListDto>>;