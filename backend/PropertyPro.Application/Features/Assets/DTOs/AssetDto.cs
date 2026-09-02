using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Assets.DTOs;

public record AssetDto(
    int Id,
    int RoomId,
    int AssetCategoryId,
    string AssetCategoryCode,
    string AssetCategoryName,
    string AssetTag,
    string Name,
    string? Description,
    string? SerialNumber,
    string? Manufacturer,
    string? Model,
    DateTime? PurchaseDate,
    decimal? PurchaseCost,
    DateTime? WarrantyExpiryDate,
    AssetStatus Status,
    bool IsActive);