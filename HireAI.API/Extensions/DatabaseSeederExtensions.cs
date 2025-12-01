using HireAI.Infrastructure.Context;
using HireAI.Seeder;
using Microsoft.EntityFrameworkCore;

namespace HireAI.API.Extensions
{
    public static class DatabaseSeederExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    var context = services.GetRequiredService<HireAIDbContext>();

                    // Check if there are any pending migrations first (safe & fast)
                    var pending = await context.Database.GetPendingMigrationsAsync();
                    if (pending.Any())
                    {
                        logger.LogInformation("Applying {Count} pending migrations...", pending.Count());
                        await context.Database.MigrateAsync();
                        logger.LogInformation("Database migrated successfully.");
                    }
                    else
                    {
                        logger.LogInformation("No pending migrations.");
                    }

                    // Call your seed method here
                    await DbSeeder.SeedAsync(services);
                    logger.LogInformation("Database seeding finished.");
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    // In production you might rethrow or stop startup depending on policy.
                    // throw;
                }
            }
        }
    }
}