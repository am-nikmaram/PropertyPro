using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Assets.DTOs;

public record AssetDto(
    int Id,
    string AssetNumber,
    string Name,
    int AssetCategoryId,
    string AssetCategoryCode,
    string AssetCategoryName,
    int RoomId,
    string RoomNumber,
    string RoomName,
    string? Manufacturer,
    string? Model,
    string? SerialNumber,
    DateTime? InstallationDate,
    DateTime? WarrantyStartDate,
    DateTime? WarrantyEndDate,
    decimal? PurchaseCost,
    AssetCriticality Criticality,
    AssetStatus Status,
    string? Description,
    bool IsActive);
