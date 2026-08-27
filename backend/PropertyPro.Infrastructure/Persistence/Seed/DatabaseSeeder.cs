using Microsoft.EntityFrameworkCore;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence.Seed;

//public static class DatabaseSeeder
//{
//    public static async Task SeedAsync(
//        ApplicationDbContext context)
//    {
//        await context.Database.MigrateAsync();

//        if (await context.Organizations.AnyAsync())
//        {
//            return;
//        }

//        var organization = new Organization
//        {
//            Name = "PropertyPro Demo Corporation",
//            Code = "PPC",
//            Description = "Demo organization for PropertyPro"
//        };

//        var facility = new Facility
//        {
//            Name = "Toronto Headquarters",
//            Code = "TOR-HQ",
//            Address = "100 Main Street",
//            City = "Toronto",
//            Country = "Canada",
//            PostalCode = "M5V 1A1",
//            Organization = organization
//        };

//        var building = new Building
//        {
//            Name = "Main Office Building",
//            Code = "BLDG-A",
//            Description = "Main administration and operations building",
//            YearBuilt = 2018,
//            TotalArea = 12500,
//            Facility = facility
//        };

//        var floor1 = new Floor
//        {
//            Name = "First Floor",
//            FloorNumber = 1,
//            Description = "Administration and reception",
//            Building = building
//        };

//        var floor2 = new Floor
//        {
//            Name = "Second Floor",
//            FloorNumber = 2,
//            Description = "Engineering and IT",
//            Building = building
//        };

//        var rooms = new[]
//        {
//            new Room
//            {
//                RoomNumber = "101",
//                Name = "Reception",
//                RoomType = "Office",
//                Capacity = 10,
//                Floor = floor1
//            },

//            new Room
//            {
//                RoomNumber = "102",
//                Name = "Conference Room",
//                RoomType = "Meeting",
//                Capacity = 20,
//                Floor = floor1
//            },

//            new Room
//            {
//                RoomNumber = "201",
//                Name = "IT Department",
//                RoomType = "Office",
//                Capacity = 15,
//                Floor = floor2
//            },

//            new Room
//            {
//                RoomNumber = "202",
//                Name = "Server Room",
//                RoomType = "Technical",
//                Capacity = 4,
//                Floor = floor2
//            }
//        };

//        await context.Organizations.AddAsync(organization);
//        await context.Facilities.AddAsync(facility);
//        await context.Buildings.AddAsync(building);
//        await context.Floors.AddRangeAsync(floor1, floor2);
//        await context.Rooms.AddRangeAsync(rooms);


//        var assetCategories = new[]
//{
//    new AssetCategory
//    {
//        Name = "HVAC",
//        Code = "HVAC",
//        Description = "Heating, ventilation and air conditioning"
//    },

//    new AssetCategory
//    {
//        Name = "Electrical",
//        Code = "ELEC",
//        Description = "Electrical equipment"
//    },

//    new AssetCategory
//    {
//        Name = "Fire Protection",
//        Code = "FIRE",
//        Description = "Fire detection and protection equipment"
//    },

//    new AssetCategory
//    {
//        Name = "Security",
//        Code = "SEC",
//        Description = "Security and access control equipment"
//    },

//    new AssetCategory
//    {
//        Name = "Plumbing",
//        Code = "PLUMB",
//        Description = "Plumbing equipment"
//    },

//    new AssetCategory
//    {
//        Name = "Elevator",
//        Code = "ELEV",
//        Description = "Elevator and lift systems"
//    }
//};

//        await context.AssetCategories.AddRangeAsync(assetCategories);


//        var assets = new[]
//{
//    new Asset
//    {
//        AssetNumber = "HVAC-0001",
//        Name = "Main HVAC Unit",
//        AssetCategory = assetCategories[0],
//        Room = rooms[3],
//        Manufacturer = "Carrier",
//        Model = "X500",
//        SerialNumber = "CAR-2025-0001",
//        InstallationDate = new DateTime(2022, 5, 10),
//        WarrantyStartDate = new DateTime(2022, 5, 10),
//        WarrantyEndDate = new DateTime(2027, 5, 10),
//        PurchaseCost = 18500,
//        Criticality = "High",
//        Status = "Operational"
//    },

//    new Asset
//    {
//        AssetNumber = "ELEC-0001",
//        Name = "Main Electrical Panel",
//        AssetCategory = assetCategories[1],
//        Room = rooms[3],
//        Manufacturer = "Schneider Electric",
//        Model = "MNS",
//        SerialNumber = "SCH-2023-001",
//        InstallationDate = new DateTime(2023, 2, 15),
//        PurchaseCost = 9200,
//        Criticality = "Critical",
//        Status = "Operational"
//    },

//    new Asset
//    {
//        AssetNumber = "SEC-0001",
//        Name = "Access Control System",
//        AssetCategory = assetCategories[3],
//        Room = rooms[0],
//        Manufacturer = "HID",
//        Model = "Signo",
//        InstallationDate = new DateTime(2024, 1, 20),
//        PurchaseCost = 4500,
//        Criticality = "Medium",
//        Status = "Operational"
//    }
//};

//        await context.Assets.AddRangeAsync(assets);


//        await context.SaveChangesAsync();
//    }
//}