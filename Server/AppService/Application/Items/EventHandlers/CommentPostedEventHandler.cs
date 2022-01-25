using Catalog.Application.Common.Models;
using Catalog.Domain.Events;

using MediatR;

namespace Catalog.Application.Items.EventHandlers;

public class CommentPostedEventHandler : INotificationHandler<DomainEventNotification<CommentPostedEvent>>
{
    public CommentPostedEventHandler()
    {

    }

    public Task Handle(DomainEventNotification<CommentPostedEvent> notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}