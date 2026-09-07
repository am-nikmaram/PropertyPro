using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Rooms.DTOs;
using PropertyPro.Application.Features.Rooms.Commands.UpdateRoom;

namespace PropertyPro.Application.Features.Rooms.UpdateRoom;

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
        //var room = await _context.Rooms
        //    .FirstOrDefaultAsync(
        //        x => x.Id == request.Id && x.IsActive,
        //        cancellationToken);


        var room = await _context.Rooms
            .Include(x => x.Unit)
            .ThenInclude(x => x.Floor)
            .Include(x => x.RoomType)
            .FirstOrDefaultAsync(
            x => x.Id == request.Id && 
            x.IsActive, 
            cancellationToken);

        if (room is null)
            throw new NotFoundException(
                $"Room with ID {request.Id} was not found.");

        // Validate Floor
        //var floorExists = await _context.Floors
        //    .AnyAsync(
        //        x => x.Id == request.UnitId && x.IsActive,
        //        cancellationToken);

        //if (!floorExists)
        //    throw new NotFoundException(
        //        $"Floor with ID {request.UnitId} was not found.");

        // Validate Room Type
        var roomTypeExists = await _context.RoomTypes
            .AnyAsync(
                x => x.Id == request.RoomTypeId && x.IsActive,
                cancellationToken);

        if (!roomTypeExists)
            throw new NotFoundException(
                $"Room Type with ID {request.RoomTypeId} was not found.");

        // Check duplicate room number
        var duplicateRoomNumber = await _context.Rooms
            .AnyAsync(
                x =>
                    x.Id != request.Id &&
                    x.UnitId == room.UnitId &&
                    x.RoomNumber == request.RoomNumber.Trim() &&
                    x.IsActive,
                cancellationToken);

        if (duplicateRoomNumber)
            throw new ConflictException(
                $"Room number '{request.RoomNumber}' already exists on this floor.");

        // Map command → entity
        _mapper.Map(request, room);

        // Normalize strings
        room.RoomNumber = request.RoomNumber.Trim();
        room.Name = request.Name.Trim();

        if (request.Description is not null)
            room.Description = request.Description.Trim();

        await _context.SaveChangesAsync(cancellationToken);
        var roomTypeName = await _context.RoomTypes
    .Where(x => x.Id == room.RoomTypeId)
    .Select(x => x.Name)
    .FirstAsync(cancellationToken);

        return new RoomDto(
            room.Id,
            room.UnitId,
            room.Unit.FloorId,
            room.Unit.Floor.BuildingId,
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