
namespace PropertyPro.Application.Features.AssetCategories.DTOs;

public record AssetCategoryDto(
    int Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);

