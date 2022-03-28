﻿
using Catalog.Domain.Common;

namespace Catalog.Domain.Entities;

public class Message : AuditableEntity, ISoftDelete, IHasDomainEvent
{
    protected Message()
    {

    }

    public Message(string text)
    {
        Id = Guid.NewGuid().ToString();
        Text = text;
    }

    public string Id { get; private set; } = null!;

    public string Text { get; set; } = null!;

    public List<MessageReceipt> Receipts { get; set; } = new List<MessageReceipt>();

    public Message? ReplyTo { get; set; }
    public string? ReplyToId { get; set; }
    public List<Message> Replies = new List<Message>();

    public DateTime? Deleted { get; set; }
    public string? DeletedById { get; set; }
    public User? DeletedBy { get; set; }

    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}