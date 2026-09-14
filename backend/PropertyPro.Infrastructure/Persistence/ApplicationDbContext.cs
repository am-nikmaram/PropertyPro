using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;
using PropertyPro.Infrastructure.Identity;

namespace PropertyPro.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations => Set<Organization>();

    public DbSet<Facility> Facilities => Set<Facility>();

    public DbSet<Building> Buildings => Set<Building>();

    public DbSet<Floor> Floors => Set<Floor>();
    public DbSet<Unit> Units => Set<Unit>();

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<RoomType> RoomTypes => Set<RoomType>();

    public DbSet<AssetCategory> AssetCategories => Set<AssetCategory>();

    public DbSet<Asset> Assets => Set<Asset>();

    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; } = null!;

    public DbSet<Technician> Technicians => Set<Technician>();

    public DbSet<Vendor> Vendors => Set<Vendor>();

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<PreventiveMaintenancePlan> PreventiveMaintenancePlans
    => Set<PreventiveMaintenancePlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}