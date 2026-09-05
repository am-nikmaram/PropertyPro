using MediatR;
using PropertyPro.Application.Features.Assets.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Assets.Queries.GetAssets;

public record GetAssetsQuery(
    string? AssetNumber,
    string? Name,
    string? SerialNumber,
    int? AssetCategoryId,
    int? RoomId,
    AssetCriticality? Criticality,
    AssetStatus? Status,
    bool? IsActive
) : IRequest<IReadOnlyList<AssetListDto>>;