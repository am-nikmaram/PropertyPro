using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Maintenance.Commands.DeleteMaintenance;

public class DeleteMaintenanceHandler
    : IRequestHandler<DeleteMaintenanceCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteMaintenanceHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        var maintenance = await _context.MaintenanceRecords
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.IsActive,
                cancellationToken);

        if (maintenance is null)
        {
            throw new NotFoundException(
                nameof(MaintenanceRecord),
                request.Id);
        }

        maintenance.IsActive = false;

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}