using WebApi.Data;

static class Seed 
{
    public static async Task SeedAsync(this WebApplication app) 
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
        
        //await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        if (!context.Items.Any())
        {
            context.Items.AddRange(new Item[] {
                new Item(Guid.NewGuid().ToString(), "Hat", "Green hat")
            });

            await context.SaveChangesAsync();
        }
    }
}