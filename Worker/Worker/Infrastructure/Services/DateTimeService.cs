using Catalog.Worker.Application.Common.Interfaces;

namespace Catalog.Worker.Infrastructure.Services;

class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}