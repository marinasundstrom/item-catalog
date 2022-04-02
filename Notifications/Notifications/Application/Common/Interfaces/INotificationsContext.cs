using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Catalog.Notifications.Domain.Entities;

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