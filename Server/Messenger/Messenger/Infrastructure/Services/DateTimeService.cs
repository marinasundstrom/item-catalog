using Messenger.Application.Common.Interfaces;

namespace Messenger.Infrastructure.Services;

class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}