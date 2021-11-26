using WebApi.Data;

namespace WebApi.Hubs;

public interface IItemsClient
{
    Task ItemAdded(Item item);

    Task ItemDeleted(string id, string name);

    Task ImageUploaded(string id, string image);
}