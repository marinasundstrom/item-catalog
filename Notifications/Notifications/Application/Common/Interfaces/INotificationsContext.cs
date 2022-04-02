using Catalog.Notifications.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Notifications.Application.Common.Interfaces;

public interface INotificationsContext
{
    DbSet<Notification> Notifications { get; }

    DbSet<Subscription> Subscriptions { get; }

    DbSet<SubscriptionGroup> SubscriptionGroups { get; }

    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<ITransaction> BeginTransactionAsync();
}

public interface ITransaction : IDisposable, IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}