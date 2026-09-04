
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Assets.DTOs;

public record AssetListDto(
    int Id,
    string AssetNumber,
    string Name,
    string AssetCategoryCode,
    string AssetCategoryName,
    string RoomNumber,
    string RoomName,
    AssetCriticality Criticality,
    AssetStatus Status,
    decimal? PurchaseCost,
    bool IsActive);