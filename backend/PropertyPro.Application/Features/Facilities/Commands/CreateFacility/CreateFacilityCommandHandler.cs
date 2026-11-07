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
    private readonly ICurrentUserService _currentUser;

    public CreateFacilityCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<FacilityDto> Handle(
        CreateFacilityCommand request,
        CancellationToken cancellationToken)
    {
        var organizationId = _currentUser.OrganizationId;
        if (!organizationId.HasValue)
            throw new UnauthorizedException("Your account is not assigned to an organization.");

        var organizationExists =
            await _context.Organizations
                .AnyAsync(
                    x => x.Id == organizationId.Value &&
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
                    x => x.OrganizationId == organizationId.Value &&
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
        facility.OrganizationId = organizationId.Value;

        facility.Name = facility.Name.Trim();
        facility.Code = facility.Code.Trim().ToUpperInvariant();


        await _context.Facilities.AddAsync(
            facility,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return _mapper.Map<FacilityDto>(facility);
    }
}
