using MediatR;
using WebApi.Application;
using WebApi.Data;

namespace WebApi;

static class RequestHandlers 
{
    static async Task<IResult> GetItems(IMediator mediator, CancellationToken cancellationToken, int page = 0, int pageSize = 10)
    {
        var result = await mediator.Send(new GetItemsQuery(page, pageSize), cancellationToken);

        return Results.Ok(result);
    }

    static async Task<IResult> GetItem(string id, IMediator mediator, CancellationToken cancellationToken)
    {
        var item = await mediator.Send(new GetItemQuery(id), cancellationToken);

        if(item == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(item);
    }

    static async Task<IResult> PostItem(CreateItemDto dto, IMediator mediator, CancellationToken cancellationToken)
    {
        await mediator.Send(new AddItemCommand(dto.Name, dto.Description), cancellationToken);

        return Results.Ok(string.Empty);
    }

    static async Task<IResult> DeleteItem(string id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteItemCommand(id), cancellationToken);

        if(result == DeletionResult.NotFound)
        {
            return Results.NotFound();
        }

        return Results.Ok();
    }

    public static WebApplication MapApplicationRequests(this WebApplication app) 
    {
        app.MapGet("/", GetItems)
        .WithTags("Items")
        .WithGroupName("Items")
        .Produces<Results<Item>>(StatusCodes.Status200OK);

        app.MapGet("/{id}", GetItem)
        .WithTags("Items")
        .WithGroupName("Items")
        .Produces<Item>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPost("/", PostItem)
        .WithTags("Items")
        .WithGroupName("Items")
        .Produces<string>(StatusCodes.Status200OK);

        app.MapDelete("/{id}", DeleteItem)
        .WithTags("Items")
        .WithGroupName("Items")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}