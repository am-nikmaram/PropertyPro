using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Floors.DTOs;

namespace PropertyPro.Application.Features.Floors.Commands.UpdateFloor;

public class UpdateFloorCommandHandler
    : IRequestHandler<UpdateFloorCommand, FloorDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateFloorCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FloorDto> Handle(
        UpdateFloorCommand request,
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

        var buildingExists =
            await _context.Buildings
                .AnyAsync(
                    x => x.Id == request.BuildingId &&
                         x.IsActive,
                    cancellationToken);

        if (!buildingExists)
        {
            throw new NotFoundException(
                $"Building with ID {request.BuildingId} " +
                "was not found.");
        }

        var duplicateFloorExists =
            await _context.Floors
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.BuildingId == request.BuildingId &&
                        x.FloorNumber == request.FloorNumber &&
                        x.IsActive,
                    cancellationToken);

        if (duplicateFloorExists)
        {
            throw new ConflictException(
                "A floor with this floor number already " +
                "exists in the building.");
        }

        _mapper.Map(request, floor);

        floor.Name = request.Name.Trim();

        floor.Description =
            request.Description?.Trim();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new FloorDto(
            floor.Id,
            floor.BuildingId,
            floor.FloorNumber,
            floor.Name,
            floor.Description,
            floor.TotalArea,
            floor.IsActive);
    }
}