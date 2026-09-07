using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Units.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Units.Queries.GetUnitById;

public class GetUnitByIdHandler
    : IRequestHandler<GetUnitByIdQuery, UnitDto>
{
    private readonly IApplicationDbContext _context;

    public GetUnitByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UnitDto> Handle(
        GetUnitByIdQuery request,
        CancellationToken cancellationToken)
    {
        var unit = await _context.Units
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.IsActive)
            .Select(x => new UnitDto(
                x.Id,
                x.FloorId,
                x.Floor.BuildingId,
                x.Code,
                x.Name,
                x.Description,
                x.Area,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (unit is null)
        {
            throw new NotFoundException(
                nameof(PropertyPro.Domain.Entities.Unit),
                request.Id);
        }

        return unit;
    }
}
