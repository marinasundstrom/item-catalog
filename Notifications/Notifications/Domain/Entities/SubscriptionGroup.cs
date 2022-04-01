
using Notifications.Domain.Common;

namespace Notifications.Domain.Entities;

public class SubscriptionGroup  : AuditableEntity, ISoftDelete, IHasDomainEvent
{
    public string Id { get; set; } = null!;

    public string? Name  { get; set; }

    public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public DateTime? Deleted { get; set; }
    public User? DeletedBy { get; set; }
    public string? DeletedById { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}