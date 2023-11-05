using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Infrastructure.Registration;

public static class InfrastructureLayerRegistration
{
    public static IServiceCollection RegisterInfrastructureLayer(this IServiceCollection services)
    {
        services
            .RegisterOptions()
            .RegisterFuzzySearch()
            .RegisterDecorators()
            .RegisterServices();

        return services;
    }
}