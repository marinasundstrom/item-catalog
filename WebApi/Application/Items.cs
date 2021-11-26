using Azure.Storage.Blobs;

using MediatR;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using WebApi.Data;
using WebApi.Hubs;

namespace WebApi.Application;

public class GetItemsQuery : IRequest<Results<Item>>
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public GetItemsQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, Results<Item>>
    {
        private readonly CatalogContext context;

        public GetItemsQueryHandler(CatalogContext context)
        {
            this.context = context;
        }

        public async Task<Results<Item>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var totalCount = await context.Items.CountAsync();

            var query = context.Items
                .OrderBy(i => i.CreatedAt)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable();

            var items = await query.ToListAsync(cancellationToken);

            return new Results<Item>(items, totalCount);
        }
    }
}

public record class Results<T>(IEnumerable<T> Items, int TotalCount);

public class GetItemQuery : IRequest<Item?>
{
    public string Id { get; set; }

    public GetItemQuery(string id)
    {
        Id = id;
    }

    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, Item?>
    {
        private readonly CatalogContext context;

        public GetItemQueryHandler(CatalogContext context)
        {
            this.context = context;
        }

        public async Task<Item?> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            return await context.Items.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        }
    }
}

public class AddItemCommand : IRequest
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public AddItemCommand(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public class AddItemCommandHandler : IRequestHandler<AddItemCommand>
    {
        private readonly CatalogContext context;
        private readonly IHubContext<ItemsHub, IItemsClient> hubContext;

        public AddItemCommandHandler(CatalogContext context, IHubContext<ItemsHub, IItemsClient> hubContext)
        {
            this.context = context;
            this.hubContext = hubContext;
        }

        public async Task<Unit> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item(Guid.NewGuid().ToString(), request.Name, request.Description)
            {
                CreatedAt = DateTime.Now
            };

            context.Items.Add(item);
            await context.SaveChangesAsync();

            await hubContext.Clients.All.ItemAdded(item);

            return Unit.Value;
        }
    }
}

public class DeleteItemCommand : IRequest<DeletionResult>
{
    public string Id { get; set; }

    public DeleteItemCommand(string id)
    {
        Id = id;
    }

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, DeletionResult>
    {
        private readonly CatalogContext context;
        private readonly IHubContext<ItemsHub, IItemsClient> hubContext;

        public DeleteItemCommandHandler(CatalogContext context, IHubContext<ItemsHub, IItemsClient> hubContext)
        {
            this.context = context;
            this.hubContext = hubContext;
        }

        public async Task<DeletionResult> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item == null)
            {
                return DeletionResult.NotFound;
            }

            item.DeletedAt = DateTime.Now;
            await context.SaveChangesAsync();

            await hubContext.Clients.All.ItemDeleted(item.Id, item.Name);

            return DeletionResult.Successful;
        }
    }
}

public enum DeletionResult
{
    Successful,
    NotFound
}

public class UploadImageCommand : IRequest<UploadImageResult>
{
    public string Id { get; set; }

    public Stream Stream { get; set; }

    public UploadImageCommand(string id, Stream stream)
    {
        Id = id;
        Stream = stream;
    }

    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, UploadImageResult>
    {
        private readonly CatalogContext context;
        private readonly BlobServiceClient blobServiceClient;
        private readonly IHubContext<ItemsHub, IItemsClient> hubContext;

        public UploadImageCommandHandler(CatalogContext context, BlobServiceClient blobServiceClient, IHubContext<ItemsHub, IItemsClient> hubContext)
        {
            this.context = context;
            this.blobServiceClient = blobServiceClient;
            this.hubContext = hubContext;
        }

        public async Task<UploadImageResult> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item == null)
            {
                return UploadImageResult.Successful;
            }

            var blobContainerClient = blobServiceClient.GetBlobContainerClient("images");

            var response = await blobContainerClient.UploadBlobAsync(request.Id, request.Stream, cancellationToken);

            item.Image = $"http://127.0.0.1:10000/devstoreaccount1/images/{request.Id}";
            await context.SaveChangesAsync();

            await hubContext.Clients.All.ImageUploaded(item.Id, item.Image);

            return UploadImageResult.Successful;
        }
    }
}

public enum UploadImageResult
{
    Successful,
    NotFound
}