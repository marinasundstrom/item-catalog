using System;

using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Common;
using Catalog.Domain.Entities;
using Catalog.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Catalog.Infrastructure.Persistence;

class CatalogContext : DbContext, ICatalogContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDomainEventService _domainEventService;
    private readonly IDateTime _dateTime;

    public CatalogContext(
        DbContextOptions<CatalogContext> options,
        ICurrentUserService currentUserService,
        IDomainEventService domainEventService,
        IDateTime dateTime) : base(options)
    {
        _currentUserService = currentUserService;
        _domainEventService = domainEventService;
        _dateTime = dateTime;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Configurations.ItemConfiguration).Assembly);
    }

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
                    entry.Entity.Created = _dateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = _dateTime.Now;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = _currentUserService.UserId;
                        softDelete.Deleted = _dateTime.Now;

                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }

    public async Task<ITransaction> BeginTransactionAsync()
    {
        return new UoWTransaction(
            await Database.BeginTransactionAsync());
    }

    class UoWTransaction : ITransaction
    {
        private readonly IDbContextTransaction _transaction;

        public UoWTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public Task CommitAsync() => _transaction.CommitAsync();


        public void Dispose() => _transaction.Dispose();

        public Task RollbackAsync() => _transaction.RollbackAsync();
    }
}