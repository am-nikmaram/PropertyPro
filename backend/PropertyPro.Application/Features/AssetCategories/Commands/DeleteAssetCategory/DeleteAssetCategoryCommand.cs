using MediatR;

namespace PropertyPro.Application.Features.AssetCategories.Commands.DeleteAssetCategory;

public record DeleteAssetCategoryCommand(int Id) : IRequest;
