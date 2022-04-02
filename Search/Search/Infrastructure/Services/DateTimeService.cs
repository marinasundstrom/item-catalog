using Catalog.Search.Application.Common.Interfaces;

namespace Catalog.Search.Infrastructure.Services;

class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}