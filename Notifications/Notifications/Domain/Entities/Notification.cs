using System;

using Catalog.Notifications.Domain.Common;

namespace Catalog.Notifications.Domain.Entities;

public class Notification : AuditableEntity, ISoftDelete, IHasDomainEvent
{
    public Notification()
    {

    }

    public Notification(string title, string text)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Text = text;
    }

    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Text { get; set; }

    public string? Tag { get; set; }

    public string? Link { get; set; }

    public bool IsRead { get; set; }

    public DateTime? Read { get; set; }

    public string? ItemId { get; set; }

    public string? UserId { get; set; }

    public User? User { get; set; }

    public string? SubscriptionId { get; set; }

    public Subscription? Subscription { get; set; }

    public string? SubscriptionGroupId { get; set; }

    public SubscriptionGroup? SubscriptionGroup { get; set; }

    public DateTime? Published { get; set; }

    public DateTime? ScheduledFor { get; set; }

    public string? ScheduledJobId { get; set; }

    public DateTime? Deleted { get; set; }
    public User? DeletedBy { get; set; }
    public string? DeletedById { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}
