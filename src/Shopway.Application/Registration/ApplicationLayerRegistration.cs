using Microsoft.AspNetCore.Builder;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Cache;
using Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.Features.Proxy;
using Shopway.Application.Registration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationLayerRegistration
{
    public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services)
    {
        Console.WriteLine($"Seeding Application Layer Memory Cache: {ApplicationCache.SeedCache}");

        services.AddScoped<IMediatorProxyService, MediatorProxyService>();
        services.AddSingleton<CreateOrderHeaderOpenTelemetry>();

        services
            .RegisterFluentValidation()
            .RegisterMiddlewares()
            .RegisterMediator();

        return services;
    }

    public static IApplicationBuilder UseApplicationLayer(this IApplicationBuilder app)
    {
        app
            .UseMiddlewares();

        return app;
    }
}