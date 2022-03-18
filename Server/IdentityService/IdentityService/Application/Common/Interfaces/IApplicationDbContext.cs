using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Catalog.IdentityService.Domain.Entities;

namespace Catalog.IdentityService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Role> Roles { get; }
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}