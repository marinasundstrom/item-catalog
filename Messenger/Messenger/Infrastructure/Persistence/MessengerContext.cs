﻿using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Messenger.Application.Common.Interfaces;
using Messenger.Domain.Common;
using Messenger.Domain.Entities;
using Messenger.Infrastructure;

namespace Messenger.Infrastructure.Persistence;

class MessengerContext : DbContext, IMessengerContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDomainEventService _domainEventService;
    private readonly IDateTime _dateTime;
    private Transaction? _transaction;

    public MessengerContext(
        DbContextOptions<MessengerContext> options,
        ICurrentUserService currentUserService,
        IDomainEventService domainEventService,
        IDateTime dateTime) : base(options)
    {
        _currentUserService = currentUserService;
        _domainEventService = domainEventService;
        _dateTime = dateTime;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging(); //.LogTo(Console.WriteLine);
#endif

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Configurations.MessageConfiguration).Assembly);
    }

#nullable disable

    public DbSet<Conversation> Conversations { get; set; } = null!;

    public DbSet<ConversationParticipant> ConversationParticipants { get; set; } = null!;

    public DbSet<Message> Messages { get; set; } = null!;

    public DbSet<MessageReceipt> MessageReceipts { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

#nullable restore

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedById = _currentUserService.UserId;
                    entry.Entity.Created = _dateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedById = _currentUserService.UserId;
                    entry.Entity.LastModified = _dateTime.Now;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedById = _currentUserService.UserId;
                        softDelete.Deleted = _dateTime.Now;

                        entry.State = EntityState.Modified;
                    }
                    break;
            }
        }

        if (_transaction is not null)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        DomainEvent[] events = GetDomainEvents();

        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchEvents(events);

        return result;
    }

    private DomainEvent[] GetDomainEvents()
    {
        return ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();
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
        var transaction = await Database.BeginTransactionAsync();

        _transaction = new Transaction(
            this,
            transaction);

        return _transaction;
    }

    class Transaction : ITransaction
    {
        private readonly MessengerContext _context;
        private readonly IDbContextTransaction _transaction;

        public Transaction(MessengerContext context, IDbContextTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public async Task CommitAsync()
        {
            DomainEvent[] events = _context.GetDomainEvents();

            await _transaction.CommitAsync();

            await _context.DispatchEvents(events);
            _context._transaction = null;
        }

        public void Dispose()
        {
            _transaction.Dispose();
            _context._transaction = null;
        }

        public async ValueTask DisposeAsync()
        {
            await _transaction.DisposeAsync();
            _context._transaction = null;
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            _context._transaction = null;
        }
    }
}