using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.RoomTypes.DTOs;

namespace PropertyPro.Application.Features.RoomTypes.Commands.UpdateRoomType;

public class UpdateRoomTypeCommandHandler
    : IRequestHandler<UpdateRoomTypeCommand, RoomTypeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateRoomTypeCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoomTypeDto> Handle(
        UpdateRoomTypeCommand request,
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

        var name = request.Name.Trim();

        var duplicateExists =
            await _context.RoomTypes
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.Name == name &&
                        x.IsActive,
                    cancellationToken);

        if (duplicateExists)
        {
            throw new ConflictException(
                "A room type with this name already exists.");
        }

        _mapper.Map(request, roomType);

        roomType.Name = name;

        roomType.Description =
            request.Description?.Trim();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new RoomTypeDto(
            roomType.Id,
            roomType.Name,
            roomType.Description,
            roomType.IsActive);
    }
}