
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Assets.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Assets.Commands.CreateAsset;

public class CreateAssetHandler
    : IRequestHandler<CreateAssetCommand, AssetDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateAssetHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AssetDto> Handle(
        CreateAssetCommand request,
        CancellationToken cancellationToken)
    {
        var assetNumber = request.AssetNumber.Trim().ToUpperInvariant();

        // Check AssetNumber
        var assetNumberExists = await _context.Assets
            .AnyAsync(
                x => x.AssetNumber == assetNumber,
                cancellationToken);

        if (assetNumberExists)
        {
            throw new ConflictException(
                $"Asset number '{assetNumber}' already exists.");
        }

        // Check AssetCategory
        var categoryExists = await _context.AssetCategories
            .AnyAsync(
                x =>
                    x.Id == request.AssetCategoryId &&
                    x.IsActive,
                cancellationToken);

        if (!categoryExists)
        {
            throw new NotFoundException(
                nameof(AssetCategory),
                request.AssetCategoryId);
        }

        // Check Room
        var roomExists = await _context.Rooms
            .AnyAsync(
                x =>
                    x.Id == request.RoomId &&
                    x.IsActive,
                cancellationToken);

        if (!roomExists)
        {
            throw new NotFoundException(
                nameof(Room),
                request.RoomId);
        }

        // Check SerialNumber when provided
        if (!string.IsNullOrWhiteSpace(request.SerialNumber))
        {
            var serialNumber = request.SerialNumber.Trim();

            var serialNumberExists = await _context.Assets
                .AnyAsync(
                    x =>
                        x.SerialNumber == serialNumber,
                    cancellationToken);

            if (serialNumberExists)
            {
                throw new ConflictException(
                    $"Serial number '{serialNumber}' already exists.");
            }
        }

        var asset = _mapper.Map<Asset>(request);

        asset.AssetNumber = assetNumber;

        asset.Name = request.Name.Trim();

        asset.Manufacturer =
            string.IsNullOrWhiteSpace(request.Manufacturer)
                ? null
                : request.Manufacturer.Trim();

        asset.Model =
            string.IsNullOrWhiteSpace(request.Model)
                ? null
                : request.Model.Trim();

        asset.SerialNumber =
            string.IsNullOrWhiteSpace(request.SerialNumber)
                ? null
                : request.SerialNumber.Trim();

        asset.Description =
            string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim();

        asset.IsActive = true;

        _context.Assets.Add(asset);

        await _context.SaveChangesAsync(cancellationToken);

        // Load related information for the returned DTO.
        var result = await _context.Assets
            .AsNoTracking()
            .Where(x => x.Id == asset.Id)
            .Select(x => new AssetDto(
                x.Id,
                x.AssetNumber,
                x.Name,
                x.AssetCategoryId,
                x.AssetCategory.Code,
                x.AssetCategory.Name,
                x.RoomId,
                x.Room.RoomNumber,
                x.Room.Name,
                x.Manufacturer,
                x.Model,
                x.SerialNumber,
                x.InstallationDate,
                x.WarrantyStartDate,
                x.WarrantyEndDate,
                x.PurchaseCost,
                x.Criticality,
                x.Status,
                x.Description,
                x.IsActive))
            .FirstAsync(cancellationToken);

        return result;
    }
}
