using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Shopway.Infrastructure.Options;
using Shopway.Domain.Entities;
using Shopway.Persistence.Framework;
using static Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection;

public static class HealthCheckRegistration
{
    private const string Basic = nameof(Basic);
    private const string Critical = nameof(Critical);
    private const string Readiness = nameof(Readiness);

    public static IServiceCollection RegisterHealthChecks(this IServiceCollection services)
    {
        var healthOptions = services.GetOptions<HealthOptions>();
        var databaseOptions = services.GetOptions<DatabaseOptions>();

        services
            .AddHealthChecks()
            .AddSqlServer
            (
                connectionString: databaseOptions.ConnectionString!,
                failureStatus: Unhealthy,
                healthQuery: "SELECT 1",
                name: "SqlServer connection",
                tags: new[] { Basic, Critical }
            )
            .AddDbContextCheck<ShopwayDbContext>
            (
                failureStatus: Unhealthy,
                name: "DbContext readiness",
                customTestQuery: Products,
                tags: new[] { Readiness }
            );

        services
            .Configure<HealthCheckPublisherOptions>(options =>
            {
                options.Delay = TimeSpan.FromSeconds(healthOptions.DelayInSeconds);
                options.Period = TimeSpan.FromSeconds(healthOptions.PeriodInSeconds);
                options.Predicate = (check) => check.Tags.Contains(Basic);
            });

        return services;
    }

    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/api/health", new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [Healthy] = StatusCodes.Status200OK,
                [Degraded] = StatusCodes.Status200OK,
                [Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            },
            ResponseWriter = WriteResponse,
            Predicate = c => c.Tags.Contains(Basic)
        });

        app.UseHealthChecks("/api/readiness", new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [Healthy] = StatusCodes.Status200OK,
                [Degraded] = StatusCodes.Status200OK,
                [Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            },
            ResponseWriter = WriteResponse,
            Predicate = c => c.Tags.Contains(Readiness)
        });

        return app;
    }

    private static async Task<bool> Products(ShopwayDbContext context, CancellationToken cancellationToken)
    {
        var products = await context
            .Set<Product>()
            .Take(2)
            .ToListAsync(cancellationToken);

        return await Task.FromResult(products.Count > 0);
    }

    private static Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        return context
            .Response
            .WriteAsync(JsonSerializer.Serialize
            (
                new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        status = e.Value.Status.ToString(),
                        exception = e.Value.Exception?.Message,
                        duration = e.Value.Duration.ToString(),
                        description = e.Value.Description,
                        data = e.Value.Data.Select(p => new
                        {
                            p.Key,
                            p.Value
                        })
                    })
                })
            );
    }
}
