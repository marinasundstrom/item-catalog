﻿using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using WebApi.Data;
using WebApi.Hubs;
using MediatR;
using WebApi;
using WebApi.Application;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var Configuration = builder.Configuration;

services
    .AddControllers()
    .AddNewtonsoftJson();

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

app.MapGet("/info", () => {
    return System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString();
})
.WithDisplayName("GetInfo")
.WithName("GetInfo")
.WithTags("Info")
.Produces<string>();

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
//await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();

if(!context.Items.Any()) 
{
    context.Items.AddRange(new Item[] {
        new Item(Guid.NewGuid().ToString(), "Hat", "Green hat")
        {
            CreatedAt = DateTime.Now
        }
    });

    await context.SaveChangesAsync();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ItemsHub>("/hubs/items");
});

app.Run();

record Blob(string Name, string ContentType);

public class CreateItemDto
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}