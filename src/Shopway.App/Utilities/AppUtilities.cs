using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Presentation.Exceptions;

namespace Microsoft.Extensions.DependencyInjection;

public static class AppUtilities
{
    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

        using var applyMigrationsScope = serviceScopeFactory.CreateScope();

        var dbContext = applyMigrationsScope.ServiceProvider.GetService<ShopwayDbContext>();

        if (dbContext is null)
        {
            throw new UnavailableException("Database is not available");
        }

        var pendingMigrations = dbContext.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
        {
            dbContext.Database.Migrate();
        }

        return app;
    }
}
