using System.Data.Common;
using System.Data.SqlClient;

using Contracts;

using Hangfire;
using Hangfire.SqlServer;

using MassTransit;

using MediatR;

using Worker.Services;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddScoped<INotifier, Notifier>();

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

// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

var app = builder.Build();

app.MapHangfireDashboard();

app.MapGet("/", () => "Hello World!");

var configuration = app.Services.GetService<IConfiguration>();

using (var connection = new SqlConnection(configuration.GetConnectionString("HangfireConnection")
    .Replace("Database=HangfireDB;", string.Empty)))
{
    connection.Open();

    using (var command = new SqlCommand(string.Format(
        @"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}') 
                                    create database [{0}];
                      ", "HangfireDB"), connection))
    {
        command.ExecuteNonQuery();
    }
}

app.Services.InitializeJobs();

app.Run();