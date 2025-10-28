using Microsoft.EntityFrameworkCore;

namespace CurrencyUpdaterService.WebApi.Extensions;

public static class IWebHostExtensions
{
    public static IHost MigrateDb<TContext>(this IHost host) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation("Started DB migration with context {dbContext}", typeof(TContext).Name);

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            logger.LogInformation("Finished DB migration with context {dbContext}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to migrate DB with context {dbContext}", typeof(TContext).Name);
        }

        return host;
    }
}
