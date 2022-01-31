
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
            if(request.UserId is null)
            {
                var users = await GetUsers();

                foreach (var user in users)
                {
                    var userId = request.UserId;

                    Notification notification = CreateNotification(request, userId);

                    context.Notifications.Add(notification);
                }
            }
            else 
            {
                Notification notification = CreateNotification(request, null);

                context.Notifications.Add(notification);         
            }

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private static Task<IEnumerable<string>> GetUsers()
        {
            return Task.FromResult<IEnumerable<string>>(new string[] { "AliceSmith@email.com", "BobSmith@email.com" });
        }

        private static Notification CreateNotification(CreateNotificationCommand request, string? userId)
        {
            var notification = new Notification();
            notification.Id = Guid.NewGuid().ToString();
            notification.Title = request.Title;
            notification.Text = request.Text;
            notification.Link = request.Link;
            notification.UserId = userId ?? request.UserId;
            notification.Published = DateTime.Now;

            notification.DomainEvents.Add(new NotificationCreatedEvent(notification.Id));
            return notification;
        }
    }
}
