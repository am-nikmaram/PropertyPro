using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Units.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Units.Commands.CreateUnit;

public class CreateUnitHandler
    : IRequestHandler<CreateUnitCommand, UnitDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateUnitHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UnitDto> Handle(
        CreateUnitCommand request,
        CancellationToken cancellationToken)
    {
        var floor = await _context.Floors
            .AsNoTracking()
            .Where(x => x.Id == request.FloorId && x.IsActive)
            .Select(x => new
            {
                x.Id,
                x.BuildingId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (floor is null)
        {
            throw new NotFoundException(
                nameof(Floor),
                request.FloorId);
        }

        var code = request.Code
            .Trim()
            .ToUpperInvariant();

        var codeExists = await _context.Units
            .AnyAsync(
                x => x.Code == code,
                cancellationToken);

        if (codeExists)
        {
            throw new ConflictException(
                $"Unit code '{code}' already exists.");
        }

        var unit = _mapper.Map<PropertyPro.Domain.Entities.Unit>(request);

        unit.Code = code;
        unit.Name = request.Name.Trim();

        unit.Description =
            string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim();

        unit.IsActive = true;

        _context.Units.Add(unit);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new UnitDto(
            unit.Id,
            unit.FloorId,
            floor.BuildingId,
            unit.Code,
            unit.Name,
            unit.Description,
            unit.Area,
            unit.IsActive);
    }
}