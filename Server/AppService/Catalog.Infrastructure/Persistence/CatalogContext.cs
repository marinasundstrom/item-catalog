﻿using System;

using Microsoft.EntityFrameworkCore;

using Catalog.Domain.Entities;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Repositories;

namespace Catalog.Infrastructure.Persistence;

class CatalogContext : DbContext, IUnitOfWork
{
    private readonly ICurrentUserService _currentUserService;
    private ItemsRepository _items;

    public CatalogContext(DbContextOptions<CatalogContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Configurations.ItemConfiguration).Assembly);
    }

    IItemsRepository IUnitOfWork.Items => _items ??= new ItemsRepository(this);

#nullable disable

    public DbSet<Item> Items { get; set; } = null!;

#nullable restore

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = DateTime.Now;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = _currentUserService.UserId;
                        softDelete.Deleted = DateTime.Now;

                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task<ITransaction> BeginTransactionAsync()
    {
        return new UoWTransaction(
            await Database.BeginTransactionAsync());
    }
}