namespace Catalog.Services;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}

