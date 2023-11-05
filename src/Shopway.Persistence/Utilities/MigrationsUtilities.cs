using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Exceptions;
using Shopway.Application.Exceptions;
using Shopway.Infrastructure.Policies;
using Shopway.Infrastructure.Utilities;

namespace Microsoft.Extensions.DependencyInjection;

public static class MigrationsUtilities
{
    internal static IApplicationBuilder ApplyMigrations<TDbContext>(this IApplicationBuilder app)
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
            var result = PollyPipelines.MigrationRetryPipeline.ExecuteAndReturnResult(() => dbContext.Database.Migrate());

            if (result.IsFailure)
            {
                throw new MigrationException(result.Error);
            }
        }

        return app;
    }
}
