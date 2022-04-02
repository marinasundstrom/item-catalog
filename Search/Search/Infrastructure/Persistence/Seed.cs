using Microsoft.Extensions.DependencyInjection;

using Catalog.Search.Domain.Entities;
using Catalog.Search.Infrastructure.Persistence;

namespace Catalog.Search.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<SearchContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                Id = "api",
                FirstName = "API",
                LastName = "User",
                Email = "test@foo.com",
                Hidden = true
            });

            await context.SaveChangesAsync();
        }
    }
}