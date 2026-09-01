using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Floors.DTOs;

namespace PropertyPro.Application.Features.Floors.Queries.GetFloorById;

public class GetFloorByIdQueryHandler
    : IRequestHandler<GetFloorByIdQuery, FloorDto?>
{
    private readonly IApplicationDbContext _context;

    public GetFloorByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FloorDto?> Handle(
        GetFloorByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Floors
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                x.IsActive)
            .Select(x => new FloorDto(
                x.Id,
                x.BuildingId,
                x.FloorNumber,
                x.Name,
                x.Description,
                x.TotalArea,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }
}