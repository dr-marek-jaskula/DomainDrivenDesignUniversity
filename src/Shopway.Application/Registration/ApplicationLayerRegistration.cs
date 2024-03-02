using Microsoft.AspNetCore.Builder;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Cache;
using Shopway.Application.Features.Proxy;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationLayerRegistration
{
    public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services)
    {
        Console.WriteLine($"Seeding Application Layer Memory Cache: {ApplicationCache.SeedCache}");

        services.AddSingleton<IMediatorProxyService, MediatorProxyService>();

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