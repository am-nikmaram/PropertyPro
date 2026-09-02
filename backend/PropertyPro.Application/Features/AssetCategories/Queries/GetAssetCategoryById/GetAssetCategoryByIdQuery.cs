using MediatR;
using PropertyPro.Application.Features.AssetCategories.DTOs;

namespace PropertyPro.Application.Features.AssetCategories.Queries.GetAssetCategoryById;

public record GetAssetCategoryByIdQuery(int Id)
    : IRequest<AssetCategoryDto>;