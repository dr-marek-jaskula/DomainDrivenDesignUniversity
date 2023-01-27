using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Linq;
using Shopway.App.Options;
using Shopway.Domain.Entities;
using Shopway.Persistence.Framework;
using static Newtonsoft.Json.Formatting;
using static Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;
using Shopway.App.Utilities;

namespace Microsoft.Extensions.DependencyInjection;

public static class HealthCheckRegistration
{
    private const string Basic = nameof(Basic);
    private const string Critical = nameof(Critical);
    private const string Readiness = nameof(Readiness);

    public static IServiceCollection RegisterHealthCheck(this IServiceCollection services, IConfiguration configuration)
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
                tags: new[] { Readiness },
                failureStatus: Unhealthy,
                name: "DbContext readiness",
                customTestQuery: Products
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

    private static Task WriteResponse(HttpContext context, HealthReport result)
    {
        context.Response.ContentType = "application/json";

        var json = new JObject(
            new JProperty("status", result.Status.ToString()),
            new JProperty("result", new JObject(result.Entries.Select(pair =>
                new JProperty(pair.Key, new JObject(
                    new JProperty("status", pair.Value.Status.ToString()),
                    new JProperty("description", pair.Value.Description),
                    new JProperty("data", new JObject(pair.Value.Data.Select(
                        p => new JProperty(p.Key, p.Value))))))))));

        return context
            .Response
            .WriteAsync(json.ToString(Indented));
    }
}
