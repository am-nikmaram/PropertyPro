
using MediatR;
using PropertyPro.Application.Features.Assets.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Assets.Commands.CreateAsset;

public record CreateAssetCommand(
    int RoomId,
    int AssetCategoryId,
    string AssetTag,
    string Name,
    string? Description,
    string? SerialNumber,
    string? Manufacturer,
    string? Model,
    DateTime? PurchaseDate,
    decimal? PurchaseCost,
    DateTime? WarrantyExpiryDate,
    AssetStatus Status
) : IRequest<AssetDto>;