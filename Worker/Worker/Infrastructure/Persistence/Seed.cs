using Microsoft.Extensions.DependencyInjection;

using Catalog.Worker.Domain.Entities;
using Catalog.Worker.Infrastructure.Persistence;

namespace Catalog.Worker.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<WorkerContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}