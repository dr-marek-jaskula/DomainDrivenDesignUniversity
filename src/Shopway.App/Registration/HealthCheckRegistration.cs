using Shopway.App.Options;
using Shopway.Persistence.Framework;

namespace Shopway.App.Registration;

public static class HealthCheckRegistration
{
    //public static IServiceCollection RegisterHealthCheck(this IServiceCollection services, IConfiguration configuration)
    //{
    //    var healthOptions = configuration.GetOptions<HealthCheckOptions>(); //

    //    services
    //        .AddHealthChecks()
    //        .AddSqlServer
    //        (
    //            connectionString: configuration.GetConnectionStringWithValueCheck(""),
    //            failureStatus: Unhealthy,
    //            healthQuery: "SELECT 1",
    //            name: "SqlServer connection",
    //            tags: new[] { BasicCheck, Critical }
    //        )
    //        .AddDbContextCheck<ShopwayDbContext>
    //        (
    //            tags: new[] { Readiness },
    //            failureStatus: Unhealthy,
    //            name: "DbContext readiness",
    //            customTestQuery: async (context, cancellationToken) => await Products(context, cancellationToken)
    //        );

    //    services
    //        .Configure<HealthCheckPublisherOptions>(options =>
    //        {
    //            options.Delay = TimeSpan.FromSeconds(healthOptions.DelayInSeconds > 0 ? healthOptions.DelayInSeconds : 5);
    //            options.Period = TimeSpan.FromSeconds(healthOptions.PeriodInSeconds > 0 ? healthOptions.PeriosInSeconds : 30);
    //            options.Predicate = (check) => check.Tags.Contains(BasicCheck);
    //        });


    //    return services;
    //}
}
