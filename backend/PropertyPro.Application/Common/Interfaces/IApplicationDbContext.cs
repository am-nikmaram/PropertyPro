using Microsoft.EntityFrameworkCore;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Common.Interfaces;

public interface IApplicationDbContext
{

        DbSet<Organization> Organizations { get; }

    DbSet<Facility> Facilities { get; }

    DbSet<Building> Buildings { get; }

    DbSet<Floor> Floors { get; }

    DbSet<Room> Rooms { get; }

    DbSet<AssetCategory> AssetCategories { get; }

    DbSet<Asset> Assets { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}