using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PropertyPro.Application.Features.System.SeedDemoData;
using PropertyPro.Application.Features.System.SeedSystemData;

namespace PropertyPro.Infrastructure.Persistence;

public class DatabaseInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly ISender _sender;
    private readonly IConfiguration _configuration;

    public DatabaseInitializer(
        ApplicationDbContext context,
        ISender sender,
        IConfiguration configuration)
    {
        _context = context;
        _sender = sender;
        _configuration = configuration;
    }

    public async Task InitializeAsync(
        CancellationToken cancellationToken = default)
    {
        var section = _configuration
            .GetSection("Database");

        var applyMigrations =
            section.GetValue<bool>("ApplyMigrations");

        var seedSystemData =
            section.GetValue<bool>("SeedSystemData");

        var seedDemoData =
            section.GetValue<bool>("SeedDemoData");

        if (applyMigrations)
        {
            await _context.Database
                .MigrateAsync(cancellationToken);
        }

        if (seedSystemData)
        {
            await _sender.Send(
                new SeedSystemDataCommand(),
                cancellationToken);
        }

        if (seedDemoData)
        {
            await _sender.Send(
                new SeedDemoDataCommand(),
                cancellationToken);
        }
    }
}