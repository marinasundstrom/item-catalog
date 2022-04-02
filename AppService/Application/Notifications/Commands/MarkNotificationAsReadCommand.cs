
using Catalog.Notifications.Client;

using MediatR;

namespace Catalog.Application.Notifications.Commands;

public class MarkNotificationAsReadCommand : IRequest
{
    public MarkNotificationAsReadCommand(string notificationId)
    {
        NotificationId = notificationId;
    }

    public string NotificationId { get; }

    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand>
    {
        private readonly INotificationsClient _notificationsClient;

        public MarkNotificationAsReadCommandHandler(INotificationsClient notificationsClient)
        {
            _notificationsClient = notificationsClient;
        }

        public async Task<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            await _notificationsClient.MarkNotificationAsReadAsync(request.NotificationId);

            return Unit.Value;
        }
    }
}