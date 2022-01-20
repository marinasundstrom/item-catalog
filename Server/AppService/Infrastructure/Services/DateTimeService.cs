using Catalog.Application.Common.Interfaces;

namespace Catalog.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}