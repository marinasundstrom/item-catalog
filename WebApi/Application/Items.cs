using System.Text.Json.Serialization;

using Azure.Storage.Blobs;

using MediatR;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Converters;

using WebApi.Data;
using WebApi.Hubs;

namespace WebApi.Application;

public class GetItemsQuery : IRequest<Results<ItemDto>>
{
    public int Page { get; set; }

    public int PageSize { get; set; }
    public string? SortBy { get; }
    public SortDirection? SortDirection { get; }

    public GetItemsQuery(int page, int pageSize, string? sortBy = null, SortDirection? sortDirection = null)
    {
        Page = page;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }

    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, Results<ItemDto>>
    {
        private readonly CatalogContext context;
        private readonly IUrlHelper urlHelper;

        public GetItemsQueryHandler(CatalogContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            this.urlHelper = urlHelper;
        }

        public async Task<Results<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Items
                .OrderBy(i => i.Created)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable();


            var totalCount = await query.CountAsync();

            if (request.SortBy is not null)
            {
                query = query.OrderBy(
                    request.SortBy,
                    request.SortDirection == Application.SortDirection.Desc ? WebApi.SortDirection.Descending : WebApi.SortDirection.Ascending);
            }

            var items = await query.ToListAsync(cancellationToken);

            return new Results<ItemDto>(
                items.Select(item => new ItemDto(item.Id, item.Name, item.Description, urlHelper.CreateImageUrl(item.Image), item.Created, item.CreatedBy, item.LastModified, item.LastModifiedBy)),
                totalCount);
        }
    }
}

[JsonConverter(typeof(StringEnumConverter))]
public enum SortDirection
{
    Asc = 2,
    Desc = 1
}

public record ItemDto(
    string Id, string Name, string? Description, string? Image,
    DateTime Created, string CreatedBy, DateTime? LastModified, string? LastModifiedBy);

public record class Results<T>(IEnumerable<T> Items, int TotalCount);

public class GetItemQuery : IRequest<ItemDto?>
{
    public string Id { get; set; }

    public GetItemQuery(string id)
    {
        Id = id;
    }

    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, ItemDto?>
    {
        private readonly CatalogContext context;
        private readonly IUrlHelper urlHelper;

        public GetItemQueryHandler(CatalogContext context, IUrlHelper urlHelper)
        {
            this.context = context;
            this.urlHelper = urlHelper;
        }

        public async Task<ItemDto?> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item == null) return null;

            return new ItemDto(item.Id, item.Name, item.Description, urlHelper.CreateImageUrl(item.Image!), item.Created, item.CreatedBy, item.LastModified, item.LastModifiedBy);
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
        private readonly IUrlHelper urlHelper;
        private readonly IHubContext<ItemsHub, IItemsClient> hubContext;

        public AddItemCommandHandler(CatalogContext context, IUrlHelper urlHelper, IHubContext<ItemsHub, IItemsClient> hubContext)
        {
            this.context = context;
            this.urlHelper = urlHelper;
            this.hubContext = hubContext;
        }

        public async Task<Unit> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item(Guid.NewGuid().ToString(), request.Name, request.Description);

            context.Items.Add(item);
            await context.SaveChangesAsync();

            var itemDto = new ItemDto(item.Id, item.Name, item.Description, urlHelper.CreateImageUrl(item.Image), item.Created, item.CreatedBy, item.LastModified, item.LastModifiedBy);

            await hubContext.Clients.All.ItemAdded(itemDto);

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

            context.Items.Remove(item);

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
        private readonly IUrlHelper urlHelper;
        private readonly BlobServiceClient blobServiceClient;
        private readonly IHubContext<ItemsHub, IItemsClient> hubContext;

        public UploadImageCommandHandler(CatalogContext context, IUrlHelper urlHelper, BlobServiceClient blobServiceClient, IHubContext<ItemsHub, IItemsClient> hubContext)
        {
            this.context = context;
            this.urlHelper = urlHelper;
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

            string imageId = $"image-{request.Id}";

            var response = await blobContainerClient.UploadBlobAsync(imageId, request.Stream, cancellationToken);

            item.Image = imageId;
            await context.SaveChangesAsync();

            await hubContext.Clients.All.ImageUploaded(item.Id, urlHelper.CreateImageUrl(item.Image)!);

            return UploadImageResult.Successful;
        }
    }
}

public enum UploadImageResult
{
    Successful,
    NotFound
}
