using AutoMapper.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilityLookup;

public class GetFacilityLookupHandler
    : IRequestHandler<
        GetFacilityLookupQuery,
        IReadOnlyList<FacilityLookupDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetFacilityLookupHandler(
        IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<FacilityLookupDto>> Handle(
        GetFacilityLookupQuery request,
        CancellationToken cancellationToken)
    {
        var organizationId = _currentUser.OrganizationId;
        if (!organizationId.HasValue)
            throw new UnauthorizedException("Your account is not assigned to an organization.");

        return await _context.Facilities
            .AsNoTracking()
            .Where(x => x.IsActive && x.OrganizationId == organizationId.Value)
            .OrderBy(x => x.Code)
            .Select(x => new FacilityLookupDto(
                x.Id,
                x.Code,
                x.Name))
            .ToListAsync(cancellationToken);
    }
}
