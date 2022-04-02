using System.Data.SqlClient;

using Hangfire;
using Hangfire.SqlServer;

using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Catalog.Messenger.Authentication;

using NSwag;
using NSwag.Generation.Processors.Security;

using Catalog.Messenger;
using Catalog.Messenger.Application;
using Catalog.Messenger.Infrastructure;
using Catalog.Messenger.Infrastructure.Persistence;
using Catalog.Messenger.Hubs;

static class Program
{
    /// <param name="seed">Seed the database</param>
    /// <param name="args">The rest of the arguments</param>
    /// <returns></returns>
    static async Task Main(bool seed, string? connectionString, string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

        var Configuration = builder.Configuration;

        var services = builder.Services;

        services.AddApplication(Configuration);
        services.AddInfrastructure(Configuration);
        services.AddServices();

        services
            .AddControllers()
            .AddNewtonsoftJson();

        services.AddSignalR();

        builder.Services.AddHttpContextAccessor();

        services.AddEndpointsApiExplorer();

        // Register the Swagger services
        services.AddOpenApiDocument(document =>
        {
            document.Title = "Messenger API";
            document.Version = "v1";

            document.AddSecurity("JWT", new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            document.AddSecurity("ApiKey", new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "X-API-KEY",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: {your API key}."
            });

            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("ApiKey"));
        });

        services.AddAuthWithJwt();
        services.AddAuthWithApiKey();

        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumers(typeof(Program).Assembly);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        })
        .AddMassTransitHostedService(true)
        .AddGenericRequestClient();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseOpenApi();
            app.UseSwaggerUi3(c =>
            {
                c.DocumentTitle = "Messenger API v1";
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGet("/", () => "Hello World!");

        app.MapControllers();

        app.MapHub<MessageHub>("/hubs/messages");

        var configuration = app.Services.GetService<IConfiguration>();

        if (seed)
        {
            await app.Services.SeedAsync();
            return;
        }

        app.Run();
    }
}