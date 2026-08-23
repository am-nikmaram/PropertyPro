using Microsoft.EntityFrameworkCore;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Facility> Facilities { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}