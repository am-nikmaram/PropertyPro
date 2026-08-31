using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Commands.UpdateFacility;

public class UpdateFacilityCommandHandler
    : IRequestHandler<UpdateFacilityCommand, FacilityDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateFacilityCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FacilityDto> Handle(
        UpdateFacilityCommand request,
        CancellationToken cancellationToken)
    {
        var facility = await _context.Facilities
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (facility is null)
        {
            throw new NotFoundException(
                $"Facility with ID {request.Id} was not found.");
        }

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

        var normalizedCode =
            request.Code.Trim().ToUpperInvariant();

        var duplicateCodeExists =
            await _context.Facilities
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.OrganizationId == request.OrganizationId &&
                        x.Code == normalizedCode &&
                        x.IsActive,
                    cancellationToken);

        if (duplicateCodeExists)
        {
            throw new ConflictException(
                "A facility with this code already exists " +
                "in the organization.");
        }

        _mapper.Map(request, facility);

        facility.Code = normalizedCode;


        await _context.SaveChangesAsync(
            cancellationToken);

        return new FacilityDto(
            facility.Id,
            facility.Name,
            facility.Code,
            facility.Address,
            facility.City,
            facility.Country,
            facility.PostalCode,
            facility.IsActive);
    }
}