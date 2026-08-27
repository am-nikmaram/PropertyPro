using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.System.SeedSystemData;

public class SeedSystemDataCommandHandler
    : IRequestHandler<SeedSystemDataCommand>
{
    private readonly IApplicationDbContext _context;

    public SeedSystemDataCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        SeedSystemDataCommand request,
        CancellationToken cancellationToken)
    {
        if (await _context.AssetCategories.AnyAsync(
                cancellationToken))
        {
            return;
        }

        var categories = new[]
        {
            new AssetCategory
            {
                Name = "HVAC",
                Code = "HVAC",
                Description =
                    "Heating, ventilation and air conditioning"
            },

            new AssetCategory
            {
                Name = "Electrical",
                Code = "ELEC",
                Description =
                    "Electrical equipment"
            },

            new AssetCategory
            {
                Name = "Fire Protection",
                Code = "FIRE",
                Description =
                    "Fire detection and protection equipment"
            },

            new AssetCategory
            {
                Name = "Security",
                Code = "SEC",
                Description =
                    "Security and access control equipment"
            },

            new AssetCategory
            {
                Name = "Plumbing",
                Code = "PLUMB",
                Description =
                    "Plumbing equipment"
            },

            new AssetCategory
            {
                Name = "Elevator",
                Code = "ELEV",
                Description =
                    "Elevator and lift systems"
            }
        };

        await _context.AssetCategories.AddRangeAsync(
            categories,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}