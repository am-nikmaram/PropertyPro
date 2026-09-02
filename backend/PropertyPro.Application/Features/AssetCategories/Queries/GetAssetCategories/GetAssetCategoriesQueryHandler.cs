using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.AssetCategories.DTOs;

namespace PropertyPro.Application.Features.AssetCategories.Queries.GetAssetCategories;

public class GetAssetCategoriesQueryHandler
    : IRequestHandler<
        GetAssetCategoriesQuery,
        IReadOnlyList<AssetCategoryListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAssetCategoriesQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AssetCategoryListDto>> Handle(
        GetAssetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.AssetCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new AssetCategoryListDto(
                x.Id,
                x.Code,
                x.Name))
            .ToListAsync(cancellationToken);
    }
}