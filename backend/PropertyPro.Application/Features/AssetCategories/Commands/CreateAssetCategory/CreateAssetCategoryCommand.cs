
using MediatR;
using PropertyPro.Application.Features.AssetCategories.DTOs;

namespace PropertyPro.Application.Features.AssetCategories.Commands.CreateAssetCategory;

public record CreateAssetCategoryCommand(
    string Code,
    string Name,
    string? Description
) : IRequest<AssetCategoryDto>;

