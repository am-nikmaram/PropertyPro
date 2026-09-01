using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Rooms.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler
    : IRequestHandler<CreateRoomCommand, RoomDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateRoomCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoomDto> Handle(
        CreateRoomCommand request,
        CancellationToken cancellationToken)
    {
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

        var roomExists =
            await _context.Rooms
                .AnyAsync(
                    x =>
                        x.FloorId == request.FloorId &&
                        x.RoomNumber == roomNumber &&
                        x.IsActive,
                    cancellationToken);

        if (roomExists)
        {
            throw new ConflictException(
                "A room with this number already exists " +
                "on the floor.");
        }

        var room = _mapper.Map<Room>(request);

        room.RoomNumber = roomNumber;
        room.Name = request.Name.Trim();

        room.Description =
            request.Description?.Trim();

        await _context.Rooms.AddAsync(
            room,
            cancellationToken);

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