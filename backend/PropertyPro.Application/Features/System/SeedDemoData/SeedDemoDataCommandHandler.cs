using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.System.SeedDemoData;

public class SeedDemoDataCommandHandler
    : IRequestHandler<SeedDemoDataCommand>
{
    private readonly IApplicationDbContext _context;

    public SeedDemoDataCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        SeedDemoDataCommand request,
        CancellationToken cancellationToken)
    {
        var organizationExists =
            await _context.Organizations
                .AnyAsync(
                    x => x.Code == "PPC",
                    cancellationToken);

        if (organizationExists)
        {
            return;
        }

        var organization = new Organization
        {
            Name = "PropertyPro Demo Corporation",
            Code = "PPC",
            Description =
                "Demo organization for PropertyPro"
        };

        var facility = new Facility
        {
            Name = "Toronto Headquarters",
            Code = "TOR-HQ",
            Address = "100 Main Street",
            City = "Toronto",
            Country = "Canada",
            PostalCode = "M5V 1A1",
            Organization = organization
        };

        var building = new Building
        {
            Name = "Main Office Building",
            Code = "BLDG-A",
            Description =
                "Main administration and operations building",
            YearBuilt = 2018,
            TotalArea = 12500,
            Facility = facility
        };

        var floor1 = new Floor
        {
            Name = "First Floor",
            FloorNumber = 1,
            Description =
                "Administration and reception",
            Building = building
        };

        var floor2 = new Floor
        {
            Name = "Second Floor",
            FloorNumber = 2,
            Description =
                "Engineering and IT",
            Building = building
        };

        var rooms = new[]
        {
            new Room
            {
                RoomNumber = "101",
                Name = "Reception",
                RoomType = "Office",
                Capacity = 10,
                Floor = floor1
            },

            new Room
            {
                RoomNumber = "102",
                Name = "Conference Room",
                RoomType = "Meeting",
                Capacity = 20,
                Floor = floor1
            },

            new Room
            {
                RoomNumber = "201",
                Name = "IT Department",
                RoomType = "Office",
                Capacity = 15,
                Floor = floor2
            },

            new Room
            {
                RoomNumber = "202",
                Name = "Server Room",
                RoomType = "Technical",
                Capacity = 4,
                Floor = floor2
            }
        };

        await _context.Organizations.AddAsync(
            organization,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);


        var serverRoom = rooms
            .First(x => x.RoomNumber == "202");

        var hvacCategory = await _context.AssetCategories
            .FirstAsync(
                x => x.Code == "HVAC",
                cancellationToken);

        var electricalCategory = await _context.AssetCategories
            .FirstAsync(
                x => x.Code == "ELEC",
                cancellationToken);

        var assets = new[]
        {
    new Asset
    {
        AssetNumber = "HVAC-0001",
        Name = "Main HVAC Unit",
        AssetCategoryId = hvacCategory.Id,
        RoomId = serverRoom.Id,
        Manufacturer = "Carrier",
        Model = "X500",
        SerialNumber = "CAR-2025-0001",
        InstallationDate =
            new DateTime(2022, 5, 10),
        WarrantyStartDate =
            new DateTime(2022, 5, 10),
        WarrantyEndDate =
            new DateTime(2027, 5, 10),
        PurchaseCost = 18500,
        Criticality = "High",
        Status = "Operational"
    },

    new Asset
    {
        AssetNumber = "ELEC-0001",
        Name = "Main Electrical Panel",
        AssetCategoryId = electricalCategory.Id,
        RoomId = serverRoom.Id,
        Manufacturer = "Schneider Electric",
        Model = "MNS",
        SerialNumber = "SCH-2023-001",
        InstallationDate =
            new DateTime(2023, 2, 15),
        PurchaseCost = 9200,
        Criticality = "Critical",
        Status = "Operational"
    }
};

        await _context.Assets.AddRangeAsync(
            assets,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}