using Microsoft.EntityFrameworkCore;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Common.Interfaces;

public interface IApplicationDbContext
{

        DbSet<Organization> Organizations { get; }

    DbSet<Facility> Facilities { get; }

    DbSet<Building> Buildings { get; }

    DbSet<Floor> Floors { get; }
    DbSet<Unit> Units { get; }

    DbSet<Room> Rooms { get; }
    DbSet<RoomType> RoomTypes { get; }

    DbSet<AssetCategory> AssetCategories { get; }

    DbSet<Asset> Assets { get; }

    DbSet<MaintenanceRecord> MaintenanceRecords { get; }

    DbSet<Technician> Technicians { get; }

    DbSet<Vendor> Vendors { get; }

    DbSet<WorkOrder> WorkOrders { get; }

    DbSet<PreventiveMaintenancePlan> PreventiveMaintenancePlans { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}