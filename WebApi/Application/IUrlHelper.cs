namespace WebApi.Application;

public interface IUrlHelper
{
    string GetHostUrl();

    string? CreateImageUrl(string? id);
}