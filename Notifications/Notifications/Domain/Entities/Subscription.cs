
using Catalog.Notifications.Domain.Common;

namespace Catalog.Notifications.Domain.Entities;

public class Subscription  : AuditableEntity, ISoftDelete, IHasDomainEvent
{
    public string Id { get; set; } = null!;

    public string? UserId { get; set; }
    public User? User { get; set; }

    public string? Tag { get; set; }

    public SubscriptionGroup? SubscriptionGroup { get; set; }
    public string? SubscriptionGroupId { get; set; }

    public List<Notification> Notifications { get; set; } = new List<Notification>();

    public DateTime? Deleted { get; set; }
    public User? DeletedBy { get; set; }
    public string? DeletedById { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}
