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
    private readonly ICurrentUserService _currentUser;

    public UpdateFacilityCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<FacilityDto> Handle(
        UpdateFacilityCommand request,
        CancellationToken cancellationToken)
    {
        var organizationId = _currentUser.OrganizationId;
        if (!organizationId.HasValue)
            throw new UnauthorizedException("Your account is not assigned to an organization.");

        var facility = await _context.Facilities
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.OrganizationId == organizationId.Value,
                cancellationToken);

        if (facility is null)
        {
            throw new NotFoundException(
                $"Facility with ID {request.Id} was not found.");
        }

        var normalizedCode =
            request.Code.Trim().ToUpperInvariant();

        var duplicateCodeExists =
            await _context.Facilities
                .AnyAsync(
                    x =>
                        x.Id != request.Id &&
                        x.OrganizationId == organizationId.Value &&
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
