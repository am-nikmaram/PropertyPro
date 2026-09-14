using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetPreventiveMaintenancePlans;

public class GetPreventiveMaintenancePlansQueryHandler
    : IRequestHandler<
        GetPreventiveMaintenancePlansQuery,
        PagedResult<PreventiveMaintenancePlanListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetPreventiveMaintenancePlansQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<PreventiveMaintenancePlanListDto>>
        Handle(
            GetPreventiveMaintenancePlansQuery request,
            CancellationToken cancellationToken)
    {
        var query = _context.PreventiveMaintenancePlans
            .AsNoTracking()
            .AsQueryable();

        if (request.AssetId.HasValue)
        {
            query = query.Where(
                x => x.AssetId == request.AssetId.Value);
        }

        if (request.Frequency.HasValue)
        {
            query = query.Where(
                x => x.Frequency == request.Frequency.Value);
        }

        if (request.Priority.HasValue)
        {
            query = query.Where(
                x => x.Priority == request.Priority.Value);
        }

        if (request.NextDueFrom.HasValue)
        {
            query = query.Where(
                x => x.NextDueDate >= request.NextDueFrom.Value);
        }

        if (request.NextDueTo.HasValue)
        {
            query = query.Where(
                x => x.NextDueDate <= request.NextDueTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.PlanNumber))
        {
            var planNumber = request.PlanNumber.Trim();

            query = query.Where(
                x => x.PlanNumber.Contains(planNumber));
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = request.Name.Trim();

            query = query.Where(
                x => x.Name.Contains(name));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(
                x => x.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(
            cancellationToken);

        var items = await query
            .OrderBy(x => x.NextDueDate)
            .ThenBy(x => x.Name)
            .Skip(
                (request.PageNumber - 1) *
                request.PageSize)
            .Take(request.PageSize)
            .Select(x => new PreventiveMaintenancePlanListDto(
                x.Id,
                x.PlanNumber,
                x.Name,
                x.AssetId,
                x.Asset.AssetNumber,
                x.Asset.Name,
                x.Frequency,
                x.NextDueDate,
                x.Priority,
                x.AutoGenerateWorkOrder,
                x.IsActive))
            .ToListAsync(cancellationToken);

        return new PagedResult<
            PreventiveMaintenancePlanListDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}