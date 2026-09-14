using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Commands.DeletePreventiveMaintenancePlan;

public class DeletePreventiveMaintenancePlanCommandHandler
    : IRequestHandler<DeletePreventiveMaintenancePlanCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePreventiveMaintenancePlanCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeletePreventiveMaintenancePlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _context.PreventiveMaintenancePlans
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (plan is null || !plan.IsActive)
        {
            throw new NotFoundException(
                $"Preventive maintenance plan '{request.Id}' was not found.");
        }

        plan.IsActive = false;

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}