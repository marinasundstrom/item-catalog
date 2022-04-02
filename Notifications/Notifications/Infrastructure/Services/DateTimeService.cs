using Catalog.Notifications.Application.Common.Interfaces;

namespace Catalog.Notifications.Infrastructure.Services;

class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}