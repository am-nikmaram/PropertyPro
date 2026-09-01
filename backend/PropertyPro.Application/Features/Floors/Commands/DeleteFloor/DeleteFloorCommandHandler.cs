using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Application.Features.Floors.Commands.DeleteFloor;

public class DeleteFloorCommandHandler
    : IRequestHandler<DeleteFloorCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteFloorCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteFloorCommand request,
        CancellationToken cancellationToken)
    {
        var floor = await _context.Floors
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                     x.IsActive,
                cancellationToken);

        if (floor is null)
        {
            throw new NotFoundException(
                $"Floor with ID {request.Id} was not found.");
        }

        floor.IsActive = false;

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}