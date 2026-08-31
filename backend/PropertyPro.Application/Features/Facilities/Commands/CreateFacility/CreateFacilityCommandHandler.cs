using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Facilities.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Facilities.Commands.CreateFacility;

public class CreateFacilityCommandHandler
    : IRequestHandler<CreateFacilityCommand, FacilityDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateFacilityCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FacilityDto> Handle(
        CreateFacilityCommand request,
        CancellationToken cancellationToken)
    {
        // Check organization
        var organizationExists =
            await _context.Organizations
                .AnyAsync(
                    x => x.Id == request.OrganizationId &&
                         x.IsActive,
                    cancellationToken);

        if (!organizationExists)
        {
            throw new NotFoundException(
                "The specified organization does not exist.");
        }

        // Check duplicate code
        var codeExists =
            await _context.Facilities
                .AnyAsync(
                    x => x.OrganizationId ==
                         request.OrganizationId &&
                         x.Code == request.Code,
                    cancellationToken);

        if (codeExists)
        {
            throw new ConflictException(
                "A facility with this code already exists " +
                "in the organization.");
        }

        // Create entity
        var facility = _mapper.Map<Facility>(request);

        facility.Name = facility.Name.Trim();
        facility.Code = facility.Code.Trim().ToUpperInvariant();
        facility.CreatedAt = DateTime.UtcNow;
        facility.IsActive = true;

        await _context.Facilities.AddAsync(
            facility,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return _mapper.Map<FacilityDto>(facility);
    }
}