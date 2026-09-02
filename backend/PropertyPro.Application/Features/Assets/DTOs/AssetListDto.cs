using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Assets.DTOs;

public record AssetListDto(
    int Id,
    int RoomId,
    int AssetCategoryId,
    string AssetCategoryCode,
    string AssetCategoryName,
    string AssetTag,
    string Name,
    string? SerialNumber,
    AssetStatus Status);