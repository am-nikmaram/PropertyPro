using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilityById;

public class GetFacilityByIdQueryHandler
    : IRequestHandler<
        GetFacilityByIdQuery,
        FacilityDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetFacilityByIdQueryHandler(
        IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<FacilityDto?> Handle(
        GetFacilityByIdQuery request,
        CancellationToken cancellationToken)
    {
        var organizationId = _currentUser.OrganizationId;
        if (!organizationId.HasValue)
            throw new UnauthorizedException("Your account is not assigned to an organization.");

        return await _context.Facilities
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.OrganizationId == organizationId.Value && x.IsActive)
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
