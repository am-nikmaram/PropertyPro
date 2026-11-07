using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;
using PropertyPro.Infrastructure.Identity;

namespace PropertyPro.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly int? _organizationId;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUser)
        : base(options)
    {
        _organizationId = currentUser.OrganizationId;
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

    public DbSet<FileAttachment> FileAttachments
    => Set<FileAttachment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(x => x.Organization)
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.SetNull);

        // Fail closed for every record in the facility hierarchy when the
        // authenticated user has no organization claim.
        modelBuilder.Entity<Facility>()
            .HasQueryFilter(x => _organizationId.HasValue && x.OrganizationId == _organizationId.Value);
        modelBuilder.Entity<Building>()
            .HasQueryFilter(x => _organizationId.HasValue && x.Facility.OrganizationId == _organizationId.Value);
        modelBuilder.Entity<Floor>()
            .HasQueryFilter(x => _organizationId.HasValue && x.Building.Facility.OrganizationId == _organizationId.Value);
        modelBuilder.Entity<Unit>()
            .HasQueryFilter(x => _organizationId.HasValue && x.Floor.Building.Facility.OrganizationId == _organizationId.Value);
        modelBuilder.Entity<Room>()
            .HasQueryFilter(x => _organizationId.HasValue && x.Unit.Floor.Building.Facility.OrganizationId == _organizationId.Value);
        modelBuilder.Entity<Asset>()
            .HasQueryFilter(x => _organizationId.HasValue && x.Room.Unit.Floor.Building.Facility.OrganizationId == _organizationId.Value);
    }
}
