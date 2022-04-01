
using System.Data.Common;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Notifications.Application.Common.Interfaces;
using Notifications.Domain.Entities;
using Notifications.Domain.Events;

namespace Notifications.Application.Notifications.Commands;

public class CreateNotificationCommand : IRequest
{
    public string Title { get; set; } = null!;

    public string? Text { get; set; } = null!;

    public string? Link { get; set; }

    public string? UserId { get; set; }

    public string[]? ExceptUserIds { get; set; }

    public string? SubscriptionId { get; set; }

    public string? SubscriptionGroupId { get; set; }

    public DateTime? ScheduledFor { get; set; }

    public CreateNotificationCommand(string title, string? text, string? link, string? userId, string[]? exceptUserIds, string? subscriptionId, string? subscriptionGroupId, DateTime? scheduledFor)
    {
        Title = title;
        Text = text;
        Link = link;
        UserId = userId;
        ExceptUserIds = exceptUserIds;
        SubscriptionId = subscriptionId;
        SubscriptionGroupId = subscriptionGroupId;
        ScheduledFor = scheduledFor;
    }

    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand>
    {
        private readonly INotificationsContext context;

        public CreateNotificationCommandHandler(INotificationsContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            if (request.SubscriptionGroupId is not null)
            {
                await CreateMultipleNotifications(request, cancellationToken);
            }
            else
            {
                CreateNotification(request);
            }

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private void CreateNotification(CreateNotificationCommand request)
        {
            Notification notification = CreateNotificationDO(request, request.UserId, request.SubscriptionId, request.SubscriptionGroupId);

            context.Notifications.Add(notification);
        }

        private async Task CreateMultipleNotifications(CreateNotificationCommand request, CancellationToken cancellationToken = default)
        {
            if(request.SubscriptionGroupId is not null)
            {
                var subscriptionGroup = await context.SubscriptionGroups
                        .Include(sg => sg.Subscriptions)
                        //.ThenInclude(s => s.User)
                        .AsNoTracking()
                        .AsSplitQuery()
                        .FirstOrDefaultAsync(sg => sg.Id == request.SubscriptionGroupId, default);

                if (subscriptionGroup is null)
                {
                    return;
                }

                foreach(var subscription in subscriptionGroup.Subscriptions)
                {
                    if(request.ExceptUserIds is not null 
                        && request.ExceptUserIds.Contains(subscription.UserId)) 
                    {
                        continue;
                    }

                    Notification notification = CreateNotificationDO(request, subscription.UserId, request.SubscriptionId, request.SubscriptionGroupId);

                    context.Notifications.Add(notification);
                }
            }

            /*
            var users = await GetUsers();

            foreach (var user in users)
            {
                var userId = user;

                Notification notification = CreateNotificationDO(request, userId);

                context.Notifications.Add(notification);
            }
            */
        }

        private static Task<IEnumerable<string>> GetUsers()
        {
            return Task.FromResult<IEnumerable<string>>(new string[] { "AliceSmith@email.com", "BobSmith@email.com" });
        }

        private static Notification CreateNotificationDO(CreateNotificationCommand request, string? userId, string? subscriptionId, string? subscriptionGroupId)
        {
            var notification = new Notification();
            notification.Id = Guid.NewGuid().ToString();
            notification.Title = request.Title;
            notification.Text = request.Text;
            notification.Link = request.Link;
            notification.UserId = userId ?? request.UserId;
            notification.SubscriptionId = subscriptionId;
            notification.SubscriptionGroupId = subscriptionGroupId;
            notification.ScheduledFor = request.ScheduledFor;

            notification.DomainEvents.Add(new NotificationCreatedEvent(notification.Id));
            return notification;
        }
    }
}