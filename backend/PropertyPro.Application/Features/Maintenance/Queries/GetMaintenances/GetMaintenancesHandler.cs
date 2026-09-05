using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Maintenance.DTOs;

namespace PropertyPro.Application.Features.Maintenance.Queries.GetMaintenances;

public class GetMaintenancesHandler
    : IRequestHandler<
        GetMaintenancesQuery,
        PagedResult<MaintenanceListDto>>
{
    private readonly IApplicationDbContext _context;


    public GetMaintenancesHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<PagedResult<MaintenanceListDto>> Handle(
        GetMaintenancesQuery request,
        CancellationToken cancellationToken)
    {
        var query =
            _context.MaintenanceRecords
                .AsNoTracking()
                .Where(x => x.IsActive);


        if (request.AssetId.HasValue)
        {
            query = query.Where(x =>
                x.AssetId == request.AssetId.Value);
        }


        if (request.Status.HasValue)
        {
            query = query.Where(x =>
                x.Status == request.Status.Value);
        }


        if (request.Priority.HasValue)
        {
            query = query.Where(x =>
                x.Priority == request.Priority.Value);
        }


        if (request.MaintenanceType.HasValue)
        {
            query = query.Where(x =>
                x.MaintenanceType ==
                request.MaintenanceType.Value);
        }


        if (request.FromDate.HasValue)
        {
            query = query.Where(x =>
                x.ScheduledDate >= request.FromDate.Value);
        }


        if (request.ToDate.HasValue)
        {
            query = query.Where(x =>
                x.ScheduledDate <= request.ToDate.Value);
        }


        var totalCount =
            await query.CountAsync(
                cancellationToken);


        var items =
            await query
                .OrderByDescending(x =>
                    x.ScheduledDate)

                .Skip(
                    (request.PageNumber - 1)
                    * request.PageSize)

                .Take(request.PageSize)

                .Select(x =>
                    new MaintenanceListDto(
                        x.Id,
                        x.MaintenanceNumber,

                        x.AssetId,
                        x.Asset.AssetNumber,
                        x.Asset.Name,

                        x.MaintenanceType,
                        x.Priority,
                        x.Status,

                        x.ScheduledDate,
                        x.Cost,

                        x.IsActive
                    ))

                .ToListAsync(
                    cancellationToken);


        return new PagedResult<MaintenanceListDto>
        {
            Items = items,

            PageNumber = request.PageNumber,

            PageSize = request.PageSize,

            TotalCount = totalCount
        };
    }
}