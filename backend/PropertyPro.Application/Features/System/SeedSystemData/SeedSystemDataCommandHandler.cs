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
        await SeedAssetCategoriesAsync(cancellationToken);

        await SeedRoomTypesAsync(cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedAssetCategoriesAsync(
        CancellationToken cancellationToken)
    {
        if (await _context.AssetCategories
            .AnyAsync(cancellationToken))
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
    }

    private async Task SeedRoomTypesAsync(
        CancellationToken cancellationToken)
    {
        if (await _context.RoomTypes
            .AnyAsync(cancellationToken))
        {
            return;
        }

        var roomTypes = new[]
        {
            new RoomType
            {
                Name = "Office",
                Description = "General office space"
            },

            new RoomType
            {
                Name = "Meeting Room",
                Description =
                    "Meeting and conference room"
            },

            new RoomType
            {
                Name = "Laboratory",
                Description =
                    "Laboratory space"
            },

            new RoomType
            {
                Name = "Storage",
                Description =
                    "Storage area"
            },

            new RoomType
            {
                Name = "Server Room",
                Description =
                    "IT server room"
            },

            new RoomType
            {
                Name = "Workshop",
                Description =
                    "Workshop area"
            },

            new RoomType
            {
                Name = "Lobby",
                Description =
                    "Lobby and reception area"
            },

            new RoomType
            {
                Name = "Restroom",
                Description =
                    "Restroom"
            },

            new RoomType
            {
                Name = "Electrical Room",
                Description =
                    "Electrical equipment room"
            },

            new RoomType
            {
                Name = "Security Room",
                Description =
                    "Security and monitoring room"
            },

            new RoomType
            {
                Name = "Other",
                Description =
                    "Other room type"
            }
        };

        await _context.RoomTypes.AddRangeAsync(
            roomTypes,
            cancellationToken);
    }
}