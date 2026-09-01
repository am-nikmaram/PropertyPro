using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Buildings.DTOs;

namespace PropertyPro.Application.Features.Buildings.Commands.UpdateBuilding;

public class UpdateBuildingCommandHandler
    : IRequestHandler<UpdateBuildingCommand, BuildingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateBuildingCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BuildingDto> Handle(
        UpdateBuildingCommand request,
        CancellationToken cancellationToken)
    {
        var building = await _context.Buildings
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                     x.IsActive,
                cancellationToken);

        if (building is null)
        {
            throw new NotFoundException(
                $"Building with ID {request.Id} was not found.");
        }

        var facilityExists =
            await _context.Facilities
                .AnyAsync(
                    x => x.Id == request.FacilityId &&
                         x.IsActive,
                    cancellationToken);

        if (!facilityExists)
        {
            throw new NotFoundException(
                $"Facility with ID {request.FacilityId} " +
                "was not found.");
        }

        var normalizedCode =
            request.Code.Trim().ToUpperInvariant();

        var duplicateCodeExists =
            await _context.Buildings
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.FacilityId == request.FacilityId &&
                        x.Code == normalizedCode &&
                        x.IsActive,
                    cancellationToken);

        if (duplicateCodeExists)
        {
            throw new ConflictException(
                "A building with this code already exists " +
                "in the facility.");
        }

        _mapper.Map(request, building);

        building.Name = request.Name.Trim();

        building.Code = normalizedCode;

        building.Description =
            request.Description?.Trim();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new BuildingDto(
            building.Id,
            building.FacilityId,
            building.Name,
            building.Code,
            building.Description,
            building.YearBuilt,
            building.TotalArea,
            building.IsActive);
    }
}