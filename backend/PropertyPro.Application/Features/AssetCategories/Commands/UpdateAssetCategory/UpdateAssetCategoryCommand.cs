
using MediatR;
using PropertyPro.Application.Features.AssetCategories.DTOs;

namespace PropertyPro.Application.Features.AssetCategories.Commands.UpdateAssetCategory;

public record UpdateAssetCategoryCommand(
    int Id,
    string Code,
    string Name,
    string? Description
) : IRequest<AssetCategoryDto>;

