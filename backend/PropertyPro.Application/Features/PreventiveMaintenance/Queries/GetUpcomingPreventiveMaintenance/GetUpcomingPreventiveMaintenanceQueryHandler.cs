using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetUpcomingPreventiveMaintenance;

public class GetUpcomingPreventiveMaintenanceQueryHandler
    : IRequestHandler<
        GetUpcomingPreventiveMaintenanceQuery,
        IReadOnlyList<PreventiveMaintenanceScheduleDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUpcomingPreventiveMaintenanceQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PreventiveMaintenanceScheduleDto>>
        Handle(
            GetUpcomingPreventiveMaintenanceQuery request,
            CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var endDate = now.AddDays(request.Days);

        var items = await _context.PreventiveMaintenancePlans
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.NextDueDate >= now &&
                x.NextDueDate <= endDate)
            .OrderBy(x => x.NextDueDate)
            .ThenBy(x => x.Priority)
            .Take(request.Take)
            .Select(x => new
            {
                x.Id,
                x.PlanNumber,
                x.Name,
                x.AssetId,
                AssetNumber = x.Asset.AssetNumber,
                AssetName = x.Asset.Name,
                x.Frequency,
                x.NextDueDate,
                x.Priority,
                x.AutoGenerateWorkOrder
            })
            .ToListAsync(cancellationToken);

        return items
            .Select(x => new PreventiveMaintenanceScheduleDto(
                x.Id,
                x.PlanNumber,
                x.Name,
                x.AssetId,
                x.AssetNumber,
                x.AssetName,
                x.Frequency,
                x.NextDueDate,
                x.Priority,
                x.AutoGenerateWorkOrder,
                Math.Max(
                    0,
                    (int)Math.Ceiling(
                        (x.NextDueDate - now).TotalDays)),
                false))
            .ToList();
    }
}