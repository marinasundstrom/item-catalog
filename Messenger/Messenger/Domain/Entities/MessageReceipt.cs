
using Messenger.Domain.Common;

namespace Messenger.Domain.Entities;

public class MessageReceipt : AuditableEntity, IHasDomainEvent
{
    public string Id { get; set; } = null!;

    public Message Message { get; set; } = null!;

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}
