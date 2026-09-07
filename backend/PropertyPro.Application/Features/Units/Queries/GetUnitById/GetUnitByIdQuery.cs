using MediatR;
using PropertyPro.Application.Features.Units.DTOs;

namespace PropertyPro.Application.Features.Units.Queries.GetUnitById;

public record GetUnitByIdQuery(int Id)
    : IRequest<UnitDto>;