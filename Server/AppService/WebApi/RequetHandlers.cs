using MediatR;

using Microsoft.Extensions.Caching.Distributed;

using Catalog.Application;
using Catalog.Infrastructure.Persistence;
using Catalog.Application.Queries;
using Catalog.Application.Commands;

namespace WebApi;

static class RequestHandlers
{
    static async Task<IResult> GetItems(IMediator mediator, CancellationToken cancellationToken, int page = 0, int pageSize = 10,
        string? sortBy = null, Catalog.Application.SortDirection sortDirection = Catalog.Application.SortDirection.Desc)
    {
        var result = await mediator.Send(new GetItemsQuery(page + 1, pageSize, sortBy, sortDirection), cancellationToken);

        return Results.Ok(result);
    }

    static async Task<IResult> GetItem(string id, IMediator mediator, IDistributedCache cache, CancellationToken cancellationToken)
    {
        string cacheKey = $"item-{id}";

        var item = await cache.GetAsync<ItemDto?>(cacheKey, cancellationToken);

        if (item is null)
        {
            item = await mediator.Send(new GetItemQuery(id), cancellationToken);

            if (item == null)
            {
                return Results.NotFound();
            }

            await cache.SetAsync(cacheKey, item,
                new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow =  TimeSpan.FromMinutes(1) },
                cancellationToken);
        }

        return Results.Ok(item);
    }

    static async Task<IResult> AddItem(AddItemDto dto, IMediator mediator, CancellationToken cancellationToken)
    {
        await mediator.Send(new AddItemCommand(dto.Name, dto.Description), cancellationToken);

        return Results.Ok(string.Empty);
    }

    static async Task<IResult> DeleteItem(string id, IMediator mediator, IDistributedCache cache, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteItemCommand(id), cancellationToken);

        if (result == DeletionResult.NotFound)
        {
            return Results.NotFound();
        }

        string cacheKey = $"item-{id}";

        await cache.RemoveAsync(cacheKey, cancellationToken);

        return Results.Ok();
    }

    public static WebApplication MapApplicationRequests(this WebApplication app)
    {
        app.MapGet("/", GetItems)
        .WithName("Items_GetItems")
        .WithTags("Items")
        .Produces<Results<ItemDto>>(StatusCodes.Status200OK);

        app.MapGet("/{id}", GetItem)
        .WithName("Items_GetItem")
        .WithTags("Items")
        .Produces<ItemDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPost("/", AddItem)
        .WithName("Items_AddItem")
        .WithTags("Items")
        .Produces<string>(StatusCodes.Status200OK);

        app.MapDelete("/{id}", DeleteItem)
        .WithName("Items_DeleteItem")
        .WithTags("Items")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }

    public class AddItemDto
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}