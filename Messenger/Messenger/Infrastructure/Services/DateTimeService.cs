using Catalog.Messenger.Application.Common.Interfaces;

namespace Catalog.Messenger.Infrastructure.Services;

class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}