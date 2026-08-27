using MediatR;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.System.SeedDatabase;

public class SeedDatabaseCommandHandler
    : IRequestHandler<SeedDatabaseCommand>
{
    private readonly IApplicationDbContext _context;

    public SeedDatabaseCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        SeedDatabaseCommand request,
        CancellationToken cancellationToken)
    {
        // Check whether seed data already exists

        // Create Organization

        // Create Facilities

        // Create Buildings

        // Create Floors

        // Create Rooms

        // Create Asset Categories

        // Create Assets

        await _context.SaveChangesAsync(cancellationToken);
    }
}