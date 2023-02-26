using Microsoft.EntityFrameworkCore;
using Shopway.Application.Exceptions;

namespace Microsoft.Extensions.DependencyInjection;

public static class MigrationsUtilities
{
    public static IApplicationBuilder ApplyMigrations<TDbContext>(this IApplicationBuilder app)
        where TDbContext : DbContext
    {
        var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

        using var applyMigrationsScope = serviceScopeFactory.CreateScope();

        var dbContext = applyMigrationsScope.ServiceProvider.GetService<TDbContext>();

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
