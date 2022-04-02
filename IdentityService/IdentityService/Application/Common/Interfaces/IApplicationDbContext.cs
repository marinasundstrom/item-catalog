using Catalog.IdentityService.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Catalog.IdentityService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Role> Roles { get; }
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}