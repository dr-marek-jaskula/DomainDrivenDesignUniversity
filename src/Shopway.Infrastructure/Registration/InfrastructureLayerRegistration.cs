using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Infrastructure.Registration;

public static class InfrastructureLayerRegistration
{
    public static IServiceCollection RegisterAppOptions(this IServiceCollection services)
    {
        services
            .RegisterOptions();

        return services;
    }

    public static IServiceCollection RegisterInfrastructureLayer(this IServiceCollection services)
    {
        services
            .RegisterFuzzySearch()
            .RegisterDecorators()
            .RegisterServices();

        return services;
    }
}