using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.RoomTypes.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeCommandHandler
    : IRequestHandler<CreateRoomTypeCommand, RoomTypeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateRoomTypeCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoomTypeDto> Handle(
        CreateRoomTypeCommand request,
        CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();

        var exists = await _context.RoomTypes
            .AnyAsync(
                x => x.Name == name &&
                     x.IsActive,
                cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                "A room type with this name already exists.");
        }

        var roomType = _mapper.Map<RoomType>(request);

        roomType.Name = name;

        roomType.Description =
            request.Description?.Trim();

        await _context.RoomTypes.AddAsync(
            roomType,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new RoomTypeDto(
            roomType.Id,
            roomType.Name,
            roomType.Description,
            roomType.IsActive);
    }
}