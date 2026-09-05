using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Assets.DTOs;
using static System.Collections.Specialized.BitVector32;
using static System.Net.WebRequestMethods;

namespace PropertyPro.Application.Features.Assets.Queries.GetAssets;

public class GetAssetsHandler
    : IRequestHandler<GetAssetsQuery, IReadOnlyList<AssetListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAssetsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AssetListDto>> Handle(
        GetAssetsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Assets
            .AsNoTracking()
            .AsQueryable();

        // Default: return active assets only.
        var isActive = request.IsActive ?? true;

        query = query.Where(x =>
            x.IsActive == isActive);

        // Asset Number filter
        if (!string.IsNullOrWhiteSpace(request.AssetNumber))
        {
            var assetNumber = request.AssetNumber
                .Trim()
                .ToUpperInvariant();

            query = query.Where(x =>
                x.AssetNumber.Contains(assetNumber));
        }

        // Name filter
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim();

            query = query.Where(x =>
                x.Name.Contains(name));
        }

        // Serial Number filter
        if (!string.IsNullOrWhiteSpace(request.SerialNumber))
        {
            var serialNumber = request.SerialNumber.Trim();

            query = query.Where(x =>
                x.SerialNumber != null &&
                x.SerialNumber.Contains(serialNumber));
        }

        // Category filter
        if (request.AssetCategoryId.HasValue)
        {
            query = query.Where(x =>
                x.AssetCategoryId == request.AssetCategoryId.Value);
        }

        // Room filter
        if (request.RoomId.HasValue)
        {
            query = query.Where(x =>
                x.RoomId == request.RoomId.Value);
        }

        // Criticality filter
        if (request.Criticality.HasValue)
        {
            query = query.Where(x =>
                x.Criticality == request.Criticality.Value);
        }

        // Status filter
        if (request.Status.HasValue)
        {
            query = query.Where(x =>
                x.Status == request.Status.Value);
        }

        return await query
            .OrderBy(x => x.AssetNumber)
            .Select(x => new AssetListDto(
                x.Id,
                x.AssetNumber,
                x.Name,
                x.AssetCategory.Code,
                x.AssetCategory.Name,
                x.Room.RoomNumber,
                x.Room.Name,
                x.Criticality,
                x.Status,
                x.PurchaseCost,
                x.IsActive))
            .ToListAsync(cancellationToken);
    }
}