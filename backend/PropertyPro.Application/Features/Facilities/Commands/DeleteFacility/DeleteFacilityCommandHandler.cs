using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Application.Features.Facilities.Commands.DeleteFacility;

public class DeleteFacilityCommandHandler
    : IRequestHandler<DeleteFacilityCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteFacilityCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteFacilityCommand request,
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

        if (!facility.IsActive)
        {
            return;
        }

        facility.IsActive = false;


        await _context.SaveChangesAsync(
            cancellationToken);
    }
}