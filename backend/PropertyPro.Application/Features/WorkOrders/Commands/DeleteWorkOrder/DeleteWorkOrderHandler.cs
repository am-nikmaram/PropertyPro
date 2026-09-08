using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.WorkOrders.Commands.DeleteWorkOrder;

public class DeleteWorkOrderHandler
    : IRequestHandler<DeleteWorkOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorkOrderHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteWorkOrderCommand request,
        CancellationToken cancellationToken)
    {
        var workOrder = await _context.WorkOrders
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.IsActive,
                cancellationToken);

        if (workOrder is null)
        {
            throw new NotFoundException(
                nameof(WorkOrder),
                request.Id);
        }

        if (workOrder.Status == WorkOrderStatus.InProgress)
        {
            throw new ConflictException(
                "An in-progress work order cannot be deactivated.");
        }

        if (workOrder.Status == WorkOrderStatus.Completed)
        {
            throw new ConflictException(
                "A completed work order cannot be deactivated.");
        }

        workOrder.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }
}