
using System.Data.Common;

using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Events;

using MediatR;

namespace Catalog.Application.Notifications.Commands;

public class CreateNotificationCommand : IRequest
{
    public string Title { get; set; } = null!;

    public string? Text { get; set; } = null!;

    public string? Link { get; set; }

    public string? UserId { get; set; }

    public CreateNotificationCommand(string title, string? text, string? link, string? userId)
    {
        Title = title;
        Text = text;
        Link = link;
        UserId = userId;
    }

    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand>
    {
        private readonly ICatalogContext context;
        private readonly INotificationClient client;

        public CreateNotificationCommandHandler(ICatalogContext context, INotificationClient client)
        {
            this.context = context;
            this.client = client;
        }

        public async Task<Unit> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification();
            notification.Id = Guid.NewGuid().ToString();
            notification.Title = request.Title;
            notification.Text = request.Text;
            notification.Link = request.Link;
            notification.UserId = request.UserId;
            notification.Published = DateTime.Now;

            notification.DomainEvents.Add(new NotificationCreatedEvent(notification.Id));

            context.Notifications.Add(notification);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
