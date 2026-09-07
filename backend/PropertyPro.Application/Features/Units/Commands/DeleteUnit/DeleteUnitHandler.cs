using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Units.Commands.DeleteUnit;

public class DeleteUnitHandler
    : IRequestHandler<DeleteUnitCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteUnitHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteUnitCommand request,
        CancellationToken cancellationToken)
    {
        var unit = await _context.Units
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.IsActive,
                cancellationToken);

        if (unit is null)
        {
            throw new NotFoundException(
                nameof(PropertyPro.Domain.Entities.Unit),
                request.Id);
        }

        var hasActiveRooms = await _context.Rooms
            .AnyAsync(
                x => x.UnitId == request.Id && x.IsActive,
                cancellationToken);

        if (hasActiveRooms)
        {
            throw new ConflictException(
                "The unit cannot be deactivated because it contains active rooms.");
        }

        unit.IsActive = false;

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}