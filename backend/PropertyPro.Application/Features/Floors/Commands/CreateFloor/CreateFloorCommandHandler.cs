using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Floors.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Floors.Commands.CreateFloor;

public class CreateFloorCommandHandler
    : IRequestHandler<CreateFloorCommand, FloorDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateFloorCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FloorDto> Handle(
        CreateFloorCommand request,
        CancellationToken cancellationToken)
    {
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

        var floorExists =
            await _context.Floors
                .AnyAsync(
                    x =>
                        x.BuildingId == request.BuildingId &&
                        x.FloorNumber == request.FloorNumber &&
                        x.IsActive,
                    cancellationToken);

        if (floorExists)
        {
            throw new ConflictException(
                "A floor with this floor number already " +
                "exists in the building.");
        }

        var floor = _mapper.Map<Floor>(request);

        floor.Name = request.Name.Trim();

        floor.Description =
            request.Description?.Trim();

        await _context.Floors.AddAsync(
            floor,
            cancellationToken);

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