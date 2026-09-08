using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.WorkOrders.DTOs;

namespace PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrderLookup;

public class GetWorkOrderLookupQueryHandler
    : IRequestHandler<
        GetWorkOrderLookupQuery,
        IReadOnlyList<WorkOrderLookupDto>>
{
    private readonly IApplicationDbContext _context;

    public GetWorkOrderLookupQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<WorkOrderLookupDto>> Handle(
        GetWorkOrderLookupQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.WorkOrders
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (request.MaintenanceRecordId.HasValue)
        {
            query = query.Where(x =>
                x.MaintenanceRecordId ==
                request.MaintenanceRecordId.Value);
        }

        return await query
            .OrderBy(x => x.WorkOrderNumber)
            .Select(x => new WorkOrderLookupDto(
                x.Id,
                x.WorkOrderNumber,
                x.MaintenanceRecordId))
            .ToListAsync(cancellationToken);
    }
}