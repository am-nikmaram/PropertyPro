using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilities;

public class GetFacilitiesQueryHandler
    : IRequestHandler<
        GetFacilitiesQuery,
        PagedResult<FacilityListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetFacilitiesQueryHandler(
        IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PagedResult<FacilityListDto>> Handle(
        GetFacilitiesQuery request,
        CancellationToken cancellationToken)
    {
        var organizationId = _currentUser.OrganizationId;
        if (!organizationId.HasValue)
            throw new UnauthorizedException("Your account is not assigned to an organization.");

        var query = _context.Facilities
            .AsNoTracking()
            .Where(x => x.IsActive && x.OrganizationId == organizationId.Value)
            .AsQueryable();

        // Search
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(x =>
                x.Name.Contains(search) ||
                x.Code.Contains(search) ||
                (x.City != null &&
                 x.City.Contains(search)));
        }

        // Total count before pagination
        var totalCount = await query.CountAsync(
            cancellationToken);

        // Sorting
        query = request.SortBy?.ToLower() switch
        {
            "code" => request.SortDescending
                ? query.OrderByDescending(x => x.Code)
                : query.OrderBy(x => x.Code),

            "city" => request.SortDescending
                ? query.OrderByDescending(x => x.City)
                : query.OrderBy(x => x.City),

            _ => request.SortDescending
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name)
        };

        // Pagination
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new FacilityListDto(
                x.Id,
                x.Name,
                x.Code,
                x.City,
                x.Country,
                x.IsActive))
            .ToListAsync(cancellationToken);

        return new PagedResult<FacilityListDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
