using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Assets.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Assets.Queries.GetAssetById;

public class GetAssetByIdHandler
    : IRequestHandler<GetAssetByIdQuery, AssetDto>
{
    private readonly IApplicationDbContext _context;

    public GetAssetByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssetDto> Handle(
        GetAssetByIdQuery request,
        CancellationToken cancellationToken)
    {
        var asset = await _context.Assets
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (asset is null)
        {
            throw new NotFoundException(
                nameof(Asset),
                request.Id);
        }

        return asset;
    }
}