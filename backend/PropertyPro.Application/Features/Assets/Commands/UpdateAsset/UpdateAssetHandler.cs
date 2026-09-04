using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Assets.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Assets.Commands.UpdateAsset;

public class UpdateAssetHandler
    : IRequestHandler<UpdateAssetCommand, AssetDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAssetHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AssetDto> Handle(
        UpdateAssetCommand request,
        CancellationToken cancellationToken)
    {
        var asset = await _context.Assets
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.Id &&
                    x.IsActive,
                cancellationToken);

        if (asset is null)
        {
            throw new NotFoundException(
                nameof(Asset),
                request.Id);
        }

        // Validate AssetCategory
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

        // Validate Room
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

        // Check SerialNumber uniqueness
        if (!string.IsNullOrWhiteSpace(request.SerialNumber))
        {
            var serialNumber = request.SerialNumber.Trim();

            var serialNumberExists = await _context.Assets
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.SerialNumber == serialNumber,
                    cancellationToken);

            if (serialNumberExists)
            {
                throw new ConflictException(
                    $"Serial number '{serialNumber}' already exists.");
            }
        }

        // Map editable properties
        _mapper.Map(request, asset);

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

        await _context.SaveChangesAsync(cancellationToken);

        // Direct projection — no AutoMapper for query/result mapping.
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
