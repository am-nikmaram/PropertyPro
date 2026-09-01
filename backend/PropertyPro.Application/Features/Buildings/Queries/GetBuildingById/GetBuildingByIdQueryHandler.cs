using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Buildings.DTOs;

namespace PropertyPro.Application.Features.Buildings.Queries.GetBuildingById;

public class GetBuildingByIdQueryHandler
    : IRequestHandler<
        GetBuildingByIdQuery,
        BuildingDto?>
{
    private readonly IApplicationDbContext _context;

    public GetBuildingByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BuildingDto?> Handle(
        GetBuildingByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Buildings
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                x.IsActive)
            .Select(x => new BuildingDto(
                x.Id,
                x.FacilityId,
                x.Name,
                x.Code,
                x.Description,
                x.YearBuilt,
                x.TotalArea,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }
}