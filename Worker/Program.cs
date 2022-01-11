using Contracts;

using MassTransit;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumers(typeof(Program).Assembly);
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
})
.AddMassTransitHostedService()
.AddGenericRequestClient();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

