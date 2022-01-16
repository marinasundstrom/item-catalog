namespace Catalog.Infrastructure;

public interface ICurrentUserService
{
    string UserId { get; }
}