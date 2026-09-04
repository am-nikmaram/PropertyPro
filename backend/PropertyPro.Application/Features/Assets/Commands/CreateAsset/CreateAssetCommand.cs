
using MediatR;
using PropertyPro.Application.Features.Assets.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Assets.Commands.CreateAsset;

public record CreateAssetCommand(
    string AssetNumber,
    string Name,
    int AssetCategoryId,
    int RoomId,
    string? Manufacturer,
    string? Model,
    string? SerialNumber,
    DateTime? InstallationDate,
    DateTime? WarrantyStartDate,
    DateTime? WarrantyEndDate,
    decimal? PurchaseCost,
    AssetCriticality Criticality,
    AssetStatus Status,
    string? Description
) : IRequest<AssetDto>;
