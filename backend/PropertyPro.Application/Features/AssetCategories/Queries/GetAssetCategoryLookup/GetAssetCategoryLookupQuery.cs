using MediatR;
using PropertyPro.Application.Features.AssetCategories.DTOs;

namespace PropertyPro.Application.Features.AssetCategories.Queries.GetAssetCategoryLookup;

public record GetAssetCategoryLookupQuery
    : IRequest<IReadOnlyList<AssetCategoryLookupDto>>;