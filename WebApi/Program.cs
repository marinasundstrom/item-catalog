using Azure.Identity;
using Azure.Storage.Blobs;

using Contracts;

using MassTransit;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

using WebApi;
using WebApi.Application;
using WebApi.Data;
using WebApi.Hubs;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var Configuration = builder.Configuration;

services
    .AddControllers()
    .AddNewtonsoftJson();

services.AddHttpContextAccessor();

services.AddScoped<ICurrentUserService, CurrentUserService>();

services.AddSqlServer<CatalogContext>(Configuration.GetConnectionString("mssql"));

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

services.AddMediatR(typeof(Program));

services.AddScoped<WebApi.Application.IUrlHelper, UrlHelper>();

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

await app.SeedAsync();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ItemsHub>("/hubs/items");
    endpoints.MapHub<SomethingHub>("/hubs/something");
    endpoints.MapHub<NotificationHub>("/hubs/notifications");
});

app.Run();