using MediatR;
using PropertyPro.Application.Features.AssetCategories.DTOs;

namespace PropertyPro.Application.Features.AssetCategories.Queries.GetAssetCategories;

public record GetAssetCategoriesQuery
    : IRequest<IReadOnlyList<AssetCategoryListDto>>;

