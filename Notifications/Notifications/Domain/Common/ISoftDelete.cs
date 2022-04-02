using Catalog.Notifications.Domain.Entities;

namespace Catalog.Notifications.Domain.Common;

public interface ISoftDelete
{
    DateTime? Deleted { get; set; }

    User? DeletedBy { get; set; }

    string? DeletedById { get; set; }
}