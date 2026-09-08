using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.WorkOrders.DTOs;

namespace PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrders;

public class GetWorkOrdersQueryHandler
    : IRequestHandler<GetWorkOrdersQuery, PagedResult<WorkOrderListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetWorkOrdersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<WorkOrderListDto>> Handle(
        GetWorkOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.WorkOrders
            .AsNoTracking();

        if (request.MaintenanceRecordId.HasValue)
        {
            query = query.Where(x =>
                x.MaintenanceRecordId == request.MaintenanceRecordId.Value);
        }

        if (request.AssetId.HasValue)
        {
            query = query.Where(x =>
                x.MaintenanceRecord.AssetId == request.AssetId.Value);
        }

        if (request.TechnicianId.HasValue)
        {
            query = query.Where(x =>
                x.TechnicianId == request.TechnicianId.Value);
        }

        if (request.VendorId.HasValue)
        {
            query = query.Where(x =>
                x.VendorId == request.VendorId.Value);
        }

        if (request.WorkOrderType.HasValue)
        {
            query = query.Where(x =>
                x.WorkOrderType == request.WorkOrderType.Value);
        }

        if (request.Priority.HasValue)
        {
            query = query.Where(x =>
                x.Priority == request.Priority.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x =>
                x.Status == request.Status.Value);
        }

        if (request.ScheduledFrom.HasValue)
        {
            query = query.Where(x =>
                x.ScheduledDate >= request.ScheduledFrom.Value);
        }

        if (request.ScheduledTo.HasValue)
        {
            query = query.Where(x =>
                x.ScheduledDate <= request.ScheduledTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.WorkOrderNumber))
        {
            var workOrderNumber = request.WorkOrderNumber.Trim();

            query = query.Where(x =>
                x.WorkOrderNumber.Contains(workOrderNumber));
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == request.IsActive.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.ScheduledDate)
            .ThenByDescending(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new WorkOrderListDto(
                x.Id,
                x.WorkOrderNumber,
                x.MaintenanceRecordId,
                x.MaintenanceRecord.MaintenanceNumber,
                x.MaintenanceRecord.Asset.AssetNumber,
                x.MaintenanceRecord.Asset.Name,
                x.TechnicianId,
                x.Technician == null
                    ? null
                    : x.Technician.FirstName + " " + x.Technician.LastName,
                x.VendorId,
                x.Vendor == null
                    ? null
                    : x.Vendor.Name,
                x.WorkOrderType,
                x.Priority,
                x.Status,
                x.ScheduledDate,
                x.EstimatedCost,
                x.ActualCost,
                x.IsActive))
            .ToListAsync(cancellationToken);

        return new PagedResult<WorkOrderListDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}
