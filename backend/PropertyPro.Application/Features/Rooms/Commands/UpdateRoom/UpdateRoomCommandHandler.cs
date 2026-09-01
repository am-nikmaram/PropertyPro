using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Commands.UpdateRoom;

public class UpdateRoomCommandHandler
    : IRequestHandler<UpdateRoomCommand, RoomDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateRoomCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoomDto> Handle(
        UpdateRoomCommand request,
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

        var floorExists =
            await _context.Floors
                .AnyAsync(
                    x => x.Id == request.FloorId &&
                         x.IsActive,
                    cancellationToken);

        if (!floorExists)
        {
            throw new NotFoundException(
                $"Floor with ID {request.FloorId} " +
                "was not found.");
        }

        var roomTypeExists =
            await _context.RoomTypes
                .AnyAsync(
                    x => x.Id == request.RoomTypeId &&
                         x.IsActive,
                    cancellationToken);

        if (!roomTypeExists)
        {
            throw new NotFoundException(
                $"Room type with ID {request.RoomTypeId} " +
                "was not found.");
        }

        var roomNumber =
            request.RoomNumber.Trim().ToUpperInvariant();

        var duplicateRoomExists =
            await _context.Rooms
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.FloorId == request.FloorId &&
                        x.RoomNumber == roomNumber &&
                        x.IsActive,
                    cancellationToken);

        if (duplicateRoomExists)
        {
            throw new ConflictException(
                "A room with this number already exists " +
                "on the floor.");
        }

        _mapper.Map(request, room);

        room.RoomNumber = roomNumber;
        room.Name = request.Name.Trim();

        room.Description =
            request.Description?.Trim();

        await _context.SaveChangesAsync(
            cancellationToken);

        var roomTypeName =
            await _context.RoomTypes
                .Where(x => x.Id == room.RoomTypeId)
                .Select(x => x.Name)
                .FirstAsync(cancellationToken);

        return new RoomDto(
            room.Id,
            room.FloorId,
            room.RoomTypeId,
            roomTypeName,
            room.RoomNumber,
            room.Name,
            room.Description,
            room.Area,
            room.Capacity,
            room.IsActive);
    }
}