using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Application.Features.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandHandler
    : IRequestHandler<DeleteRoomCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteRoomCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteRoomCommand request,
        CancellationToken cancellationToken)
    {
        var room = await _context.Rooms
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                     x.IsActive,
                cancellationToken);

        if (room is null)
        {
            throw new NotFoundException(
                $"Room with ID {request.Id} was not found.");
        }

        var hasActiveAssets = await _context.Assets
    .AnyAsync(
        x => x.RoomId == request.Id && x.IsActive,
        cancellationToken);

        if (hasActiveAssets)
        {
            throw new ConflictException(
                "This room cannot be deleted because it has active assets.");
        }

        room.IsActive = false;

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}