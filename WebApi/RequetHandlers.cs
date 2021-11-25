using MediatR;
using WebApi.Application;
using WebApi.Data;

namespace WebApi;

static class RequestHandlers 
{
    public static WebApplication MapApplicationRequests(this WebApplication app) 
    {
        app.MapGet("/", async (int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) => 
        {
            var result = await mediator.Send(new GetItemsQuery(page, pageSize));

            return Results.Ok(result);
        })
        .WithTags("Items")
        .WithGroupName("Items")
        .Produces<Results<Item>>();

        app.MapGet("/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) => 
        {
            var item = await mediator.Send(new GetItemQuery(id));

            if(item == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(item);
        })
        .WithTags("Items")
        .WithGroupName("Items")
        .Produces<Item>()
        .Produces(StatusCodes.Status404NotFound);

        app.MapPost("/", async (CreateItemDto dto, IMediator mediator, CancellationToken cancellationToken) =>
        {
            await mediator.Send(new AddItemCommand(dto.Name, dto.Description));

            return Results.Ok(string.Empty);
        })
        .WithTags("Items")
        .WithGroupName("Items")
        .Produces<string>(StatusCodes.Status200OK);

        app.MapDelete("/{id}", async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new DeleteItemCommand(id));

            if(result == DeletionResult.NotFound)
            {
                return Results.NotFound();
            }

            return Results.Ok();
        })
        .WithTags("Items")
        .WithGroupName("Items");

        return app;
    }
}