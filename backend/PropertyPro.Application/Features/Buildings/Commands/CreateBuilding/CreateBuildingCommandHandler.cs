using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Buildings.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Buildings.Commands.CreateBuilding;

public class CreateBuildingCommandHandler
    : IRequestHandler<CreateBuildingCommand, BuildingDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateBuildingCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BuildingDto> Handle(
        CreateBuildingCommand request,
        CancellationToken cancellationToken)
    {
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

        var codeExists =
            await _context.Buildings
                .AnyAsync(
                    x =>
                        x.FacilityId == request.FacilityId &&
                        x.Code == normalizedCode &&
                        x.IsActive,
                    cancellationToken);

        if (codeExists)
        {
            throw new ConflictException(
                "A building with this code already exists " +
                "in the facility.");
        }

        var building = _mapper.Map<Building>(request);

        building.Name = request.Name.Trim();
        building.Code = normalizedCode;
        building.Description =
            request.Description?.Trim();

        await _context.Buildings.AddAsync(
            building,
            cancellationToken);

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