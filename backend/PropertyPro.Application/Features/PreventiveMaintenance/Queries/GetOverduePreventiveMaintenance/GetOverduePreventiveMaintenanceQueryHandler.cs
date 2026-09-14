using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetOverduePreventiveMaintenance;

public class GetOverduePreventiveMaintenanceQueryHandler
    : IRequestHandler<
        GetOverduePreventiveMaintenanceQuery,
        IReadOnlyList<PreventiveMaintenanceScheduleDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOverduePreventiveMaintenanceQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PreventiveMaintenanceScheduleDto>>
        Handle(
            GetOverduePreventiveMaintenanceQuery request,
            CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var items = await _context.PreventiveMaintenancePlans
            .AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.NextDueDate < now)
            .OrderBy(x => x.NextDueDate)
            .ThenByDescending(x => x.Priority)
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
                (int)Math.Ceiling(
                    (now - x.NextDueDate).TotalDays),
                true))
            .ToList();
    }
}