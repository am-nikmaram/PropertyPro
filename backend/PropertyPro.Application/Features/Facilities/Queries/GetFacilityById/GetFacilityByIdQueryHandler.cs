using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilityById;

public class GetFacilityByIdQueryHandler
    : IRequestHandler<
        GetFacilityByIdQuery,
        FacilityDto?>
{
    private readonly IApplicationDbContext _context;

    public GetFacilityByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FacilityDto?> Handle(
        GetFacilityByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Facilities
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new FacilityDto(
                x.Id,
                x.Name,
                x.Code,
                x.Address,
                x.City,
                x.Country,
                x.PostalCode,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }
}