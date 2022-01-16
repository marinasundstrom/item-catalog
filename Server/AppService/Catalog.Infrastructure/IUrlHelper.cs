namespace Catalog.Infrastructure;

public interface IUrlHelper
{
    string GetHostUrl();

    string? CreateImageUrl(string? id);
}