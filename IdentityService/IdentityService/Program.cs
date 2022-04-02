﻿using Catalog.IdentityService;
using Catalog.IdentityService.Infrastructure.Persistence;

using Serilog;

static class Program
{
    /// <param name="seed">Seed the database</param>
    /// <param name="args">The rest of the arguments</param>
    /// <returns></returns>
    static async Task Main(bool seed, string? connectionString, string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        Log.Information("Starting up");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(ctx.Configuration));

            var app = builder
                .ConfigureServices()
                .ConfigurePipeline();

            //await SeedData.EnsureSeedData(app);

            // this seeding is only for the template to bootstrap the DB and users.
            // in production you will likely want a different approach.
            if (seed)
            {
                Log.Information("Seeding database...");
                await SeedData.EnsureSeedData(app);
                Log.Information("Done seeding database. Exiting.");

                return;
            }

            app.Run();
        }
        catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException") // https://github.com/dotnet/runtime/issues/60600
        {
            Log.Fatal(ex, "Unhandled exception");
        }
        finally
        {
            Log.Information("Shut down complete");
            Log.CloseAndFlush();

        }
    }
}