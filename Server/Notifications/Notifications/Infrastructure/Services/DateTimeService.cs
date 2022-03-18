using Notifications.Application.Common.Interfaces;

namespace Notifications.Infrastructure.Services;

class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}