using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Units.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Units.Commands.UpdateUnit;

public class UpdateUnitHandler
    : IRequestHandler<UpdateUnitCommand, UnitDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateUnitHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UnitDto> Handle(
        UpdateUnitCommand request,
        CancellationToken cancellationToken)
    {
        var unit = await _context.Units
            .Include(x => x.Floor)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.IsActive,
                cancellationToken);

        if (unit is null)
        {
            throw new NotFoundException(
                nameof(PropertyPro.Domain.Entities.Unit),
                request.Id);
        }

        var code = request.Code
            .Trim()
            .ToUpperInvariant();

        var codeExists = await _context.Units
            .AnyAsync(
                x => x.Code == code &&
                     x.Id != request.Id,
                cancellationToken);

        if (codeExists)
        {
            throw new ConflictException(
                $"Unit code '{code}' already exists.");
        }

        _mapper.Map(request, unit);

        unit.Code = code;

        unit.Name = request.Name.Trim();

        unit.Description =
            string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim();

        await _context.SaveChangesAsync(
            cancellationToken);

        return new UnitDto(
            unit.Id,
            unit.FloorId,
            unit.Floor.BuildingId,
            unit.Code,
            unit.Name,
            unit.Description,
            unit.Area,
            unit.IsActive);
    }
}