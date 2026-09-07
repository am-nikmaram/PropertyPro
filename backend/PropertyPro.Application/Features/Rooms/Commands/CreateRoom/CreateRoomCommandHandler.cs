
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


        //var unit = await _context.Units
        //    .AsNoTracking()
        //    .Where(x =>
        //    x.Id == request.UnitId &&
        //    x.IsActive)
        //    .Select(x => new
        //    { x.Id,
        //      x.FloorId,
        //      FloorBuildingId = x.Floor.BuildingId 
        //    }).FirstOrDefaultAsync(cancellationToken); 



        var roomNumber = request.RoomNumber.Trim().ToUpperInvariant();

        var unit = await _context.Units
            .AsNoTracking()
            .Where(x =>
            x.Id == request.UnitId &&
            x.IsActive)
            .Select(x => new
            {
                x.Id,
                x.FloorId,
                FloorBuildingId = x.Floor.BuildingId
            }).FirstOrDefaultAsync(cancellationToken);

        if (unit is null)
            throw new NotFoundException(
                $"Floor with ID {request.UnitId} was not found.");

        var roomType = await _context.RoomTypes
            .FirstOrDefaultAsync(
                x => x.Id == request.RoomTypeId &&
                     x.IsActive,
                cancellationToken);

        if (roomType is null)
            throw new NotFoundException(
                $"Room type with ID {request.RoomTypeId} was not found.");

        var duplicateRoom = await _context.Rooms
            .AnyAsync(
                x => x.UnitId == request.UnitId &&
                     x.RoomNumber == roomNumber &&
                     x.IsActive,
                cancellationToken);

        if (duplicateRoom)
        {
            throw new ConflictException(
                $"Room '{roomNumber}' already exists on this floor.");
        }

        var room = _mapper.Map<Room>(request);

        room.RoomNumber = roomNumber;
        room.Name = request.Name.Trim();
        room.Description = request.Description?.Trim();

        _context.Rooms.Add(room);

        await _context.SaveChangesAsync(cancellationToken);

        return new RoomDto(
            room.Id,
            room.UnitId,
            unit.FloorId, 
            unit.FloorBuildingId,
            room.RoomTypeId,
            roomType.Name,
            room.RoomNumber,
            room.Name,
            room.Description,
            room.Area,
            room.Capacity,
            room.IsActive);
    }
}

