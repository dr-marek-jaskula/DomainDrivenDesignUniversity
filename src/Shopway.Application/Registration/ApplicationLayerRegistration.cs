using Shopway.Application.Cache;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationLayerRegistration
{
    public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services)
    {
        Console.WriteLine($"Seeding Application Layer Memory Cache: {ApplicationCache.SeedCache}");

        services
            .RegisterFluentValidation()
            .RegisterMiddlewares();

        return services;
    }

    public static IApplicationBuilder UseApplicationLayer(this IApplicationBuilder app)
    {
        app
            .UseMiddlewares();

        return app;
    }
}