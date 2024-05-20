namespace Microsoft.Extensions.DependencyInjection;

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
            .RegisterServices()
            .RegisterPaymentGateway();

        return services;
    }
}