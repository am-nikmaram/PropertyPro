using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Application.Features.Facilities.Commands.DeleteFacility;

public class DeleteFacilityCommandHandler
    : IRequestHandler<DeleteFacilityCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteFacilityCommandHandler(
        IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(
        DeleteFacilityCommand request,
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

        if (!facility.IsActive)
        {
            return;
        }

        facility.IsActive = false;


        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
