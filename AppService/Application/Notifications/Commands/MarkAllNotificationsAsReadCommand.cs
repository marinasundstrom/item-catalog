
using Catalog.Notifications.Client;

using MediatR;

namespace Catalog.Application.Notifications.Commands;

public class MarkAllNotificationsAsReadCommand : IRequest
{
    public class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand>
    {
        private readonly INotificationsClient _notificationsClient;

        public MarkAllNotificationsAsReadCommandHandler(INotificationsClient notificationsClient)
        {
            _notificationsClient = notificationsClient;
        }

        public async Task<Unit> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            await _notificationsClient.MarkAllNotificationsAsReadAsync();

            return Unit.Value;
        }
    }
}