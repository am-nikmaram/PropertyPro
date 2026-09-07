using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}