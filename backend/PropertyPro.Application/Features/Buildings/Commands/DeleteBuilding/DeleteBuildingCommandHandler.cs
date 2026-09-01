using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Application.Features.Buildings.Commands.DeleteBuilding;

public class DeleteBuildingCommandHandler
    : IRequestHandler<DeleteBuildingCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteBuildingCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteBuildingCommand request,
        CancellationToken cancellationToken)
    {
        var building = await _context.Buildings
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                     x.IsActive,
                cancellationToken);

        if (building is null)
        {
            throw new NotFoundException(
                $"Building with ID {request.Id} was not found.");
        }

        building.IsActive = false;

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}