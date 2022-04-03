using Catalog.ApiKeys.Application.Common.Interfaces;

namespace Catalog.ApiKeys.Infrastructure.Services;

class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}