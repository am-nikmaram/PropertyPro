using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Application.Features.RoomTypes.Commands.DeleteRoomType;

public class DeleteRoomTypeCommandHandler
    : IRequestHandler<DeleteRoomTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteRoomTypeCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteRoomTypeCommand request,
        CancellationToken cancellationToken)
    {
        var roomType = await _context.RoomTypes
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                     x.IsActive,
                cancellationToken);

        if (roomType is null)
        {
            throw new NotFoundException(
                $"Room type with ID {request.Id} " +
                "was not found.");
        }

        var isInUse = await _context.Rooms
            .AnyAsync(
                x => x.RoomTypeId == request.Id &&
                     x.IsActive,
                cancellationToken);

        if (isInUse)
        {
            throw new ConflictException(
                "This room type cannot be deleted because " +
                "it is currently being used by one or more rooms.");
        }

        roomType.IsActive = false;

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}