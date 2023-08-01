using Microsoft.EntityFrameworkCore;
using Shopway.Application.Exceptions;
using Shopway.Infrastructure.Policies;
using Shopway.Persistence.Exceptions;

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
            var result = PollyPolicies.MigrationRetryPolicy.ExecuteAndCapture(() => dbContext.Database.Migrate());

            if (result.FinalException is not null)
            {
                throw new MigrationException("Applying migrations failed.", result.FinalException);
            }
        }

        return app;
    }
}
