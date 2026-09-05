using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.AssetCategories.DTOs;

namespace PropertyPro.Application.Features.AssetCategories.Queries.GetAssetCategoryLookup;

public class GetAssetCategoryLookupHandler
    : IRequestHandler<
        GetAssetCategoryLookupQuery,
        IReadOnlyList<AssetCategoryLookupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAssetCategoryLookupHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AssetCategoryLookupDto>> Handle(
        GetAssetCategoryLookupQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.AssetCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Code)
            .Select(x => new AssetCategoryLookupDto(
                x.Id,
                x.Code,
                x.Name))
            .ToListAsync(cancellationToken);
    }
}
