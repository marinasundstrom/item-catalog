using Azure.Identity;
using Azure.Storage.Blobs;

using Catalog.Application;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Persistence;
using Catalog.WebApi;
using Catalog.WebApi.Hubs;

using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var Configuration = builder.Configuration;

services.AddApplication(Configuration);
services.AddInfrastructure(Configuration);
services.AddServices();

services
    .AddControllers()
    .AddNewtonsoftJson();

services.AddHttpContextAccessor();

services.AddEndpointsApiExplorer();

// Register the Swagger services
services.AddOpenApiDocument(config =>
{
    config.Title = "Web API";
    config.Version = "v1";
});

services.AddAzureClients(builder =>
{
    // Add a KeyVault client
    //builder.AddSecretClient(keyVaultUrl);

    // Add a Storage account client
    builder.AddBlobServiceClient(Configuration.GetConnectionString("Azure:Storage"))
            .WithVersion(BlobClientOptions.ServiceVersion.V2019_07_07);

    // Use DefaultAzureCredential by default
    builder.UseCredential(new DefaultAzureCredential());
});

services.AddSignalR();

services.AddMassTransit(x =>
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

services.AddStackExchangeRedisCache(o =>
{
    o.Configuration = Configuration.GetConnectionString("redis");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseOpenApi();
    app.UseSwaggerUi3(c => c.DocumentTitle = "Web API v1");
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapApplicationRequests();

app.MapGet("/info", () =>
{
    return System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString();
})
.WithDisplayName("GetInfo")
.WithName("GetInfo")
.WithTags("Info")
.Produces<string>();

await app.Services.SeedAsync();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ItemsHub>("/hubs/items");
    endpoints.MapHub<SomethingHub>("/hubs/something");
    endpoints.MapHub<NotificationHub>("/hubs/notifications");
});

app.Run();