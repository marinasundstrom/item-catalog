using Catalog.ApiKeys.Domain.Entities;

namespace Catalog.ApiKeys.Domain.Common;

public interface ISoftDelete
{
    DateTime? Deleted { get; set; }

    User? DeletedBy { get; set; }

    string? DeletedById { get; set; }
}