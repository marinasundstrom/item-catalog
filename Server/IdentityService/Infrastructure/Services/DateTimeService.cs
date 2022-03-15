using Catalog.IdentityService.Application.Common.Interfaces;

namespace Catalog.IdentityService.Infrastructure.Services;

class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}